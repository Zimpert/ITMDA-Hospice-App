#include "webserver.hpp"
#include "net/socket_error_code.hpp"
#include "net/address_conversion.hpp"
#include "net/http_header.hpp"
#include <span>
#include <format>
#include <string>
#include <thread>
#include <winsock2.h>

webserver::webserver() noexcept : m_server{std::invoke([]{
 auto maybe_server = net::http_socket::create_server(net::convert_ipv4_string_to_u32("192.168.110.108"), 80);
 if (maybe_server.status != net::socket_error_code::success) [[unlikely]] {
  std::cout << std::format("Failed to Create Server\n{}\n", net::lookup_enum_verbose(maybe_server.status));
 }
 return maybe_server.value;
 })} {
 this->m_running = true;
}
webserver::~webserver() noexcept {
 this->m_running = false;
}

void webserver::stop() noexcept {
 this->m_running = false;
}
void webserver::run() noexcept {
 while (this->m_running) {
  this->accept_incoming_connections();
  this->distribute_jobs();
 }
 for (auto& client : this->m_clients) {
  if (client.socket().socket_handle != 0) {
   client.close();
  }
 }
 this->m_server.close();
}
void webserver::accept_incoming_connections() noexcept {
 while (true) {
  auto [status, has_incoming_connection] = this->has_incoming_connection();
  if (status != net::socket_error_code::success) [[unlikely]] {
   std::cout << std::format("Failed to Select on Server Socket:\n{}", net::lookup_enum_verbose(status));
   return;
  }
  if (has_incoming_connection) {
   this->accept_client();
  } else {
   return;
  }
 } 
}
stl::status_type<net::socket_error_code, bool> webserver::has_incoming_connection() const noexcept {
 static constexpr ::TIMEVAL timeout{ 0, 2000 };
 ::fd_set server_socket { .fd_count = 1 };
 server_socket.fd_array[0] = this->m_server.socket().socket_handle;
 if (::select(NULL, &server_socket, nullptr, nullptr, &timeout) == SOCKET_ERROR) [[unlikely]] {
  net::socket_error_code const socket_error_code = static_cast<net::socket_error_code>(::WSAGetLastError());
  return stl::status_type<net::socket_error_code, bool>{ socket_error_code, false };
 } else {
  return stl::status_type<net::socket_error_code, bool>{ net::socket_error_code::success, server_socket.fd_count == 1 };
 }
}
void webserver::accept_client() noexcept {
 auto it = std::find_if(std::begin(this->m_clients), std::end(this->m_clients), [&](auto const& client) noexcept { return client.socket().socket_handle == 0; });
 if (it == std::end(this->m_clients)) [[unlikely]] {
  std::cout << "Clients Full!\n";
 }
 auto maybe_client = this->m_server.accept();
 if (maybe_client.status != net::socket_error_code::success) [[unlikely]] {
  std::cout << "Failed to Connect New Client\n";
 }
 *it = std::move(maybe_client.value);
}
void webserver::distribute_jobs() noexcept {
 static constexpr ::timeval timeout{ 0, 8000 };
 auto client_it = std::begin(this->m_clients);
 while (true) {
  static ::fd_set client_sockets{};
  client_sockets.fd_count = 0;
  for (auto& socket_handle : client_sockets.fd_array) {
   if (client_it == std::end(this->m_clients)) {
    break;
   }
   if (client_it->socket().socket_handle == NULL) {
    ++client_it;
    continue;
   }
   socket_handle = client_it->socket().socket_handle;
   ++client_it;
   ++client_sockets.fd_count;
  }
  if (client_sockets.fd_count == 0) {
   return;
  }
  if (::select(NULL, &client_sockets, nullptr, nullptr, &timeout) == SOCKET_ERROR) {
   net::socket_error_code const socket_error_code = static_cast<net::socket_error_code>(::WSAGetLastError());
   if (socket_error_code != net::socket_error_code::success) [[likely]] {
    std::cout << std::format("Failed to Select on Client Sockets\n{}\n", net::lookup_enum_verbose(socket_error_code));
    return; /* To prevent infinite loop */
   }
  }
  for (auto const socket_handle : std::span{client_sockets.fd_array, client_sockets.fd_count}) {
   auto it = std::find_if(std::begin(this->m_clients), std::end(this->m_clients), [&](auto const& client) noexcept { return client.socket().socket_handle == socket_handle; });
   if (it == std::end(this->m_clients)) [[unlikely]] {
    std::cout << "Socket Handle Returned from \"select\" Not Valid\n";
    return;
   }
   auto [request_status, request] = it->receive_request();
   if (request_status != net::socket_error_code::success) [[unlikely]] {
    std::cout << std::format("Failed HTTP Request Receival: {}\n", net::lookup_enum_verbose(request_status));
    auto const close_result = it->close();
    if (close_result.status != net::socket_error_code::success) [[unlikely]] {
     std::cout << std::format("Failed to Close Socket: {}\n", net::lookup_enum_verbose(close_result.status));
    }
    continue;
   }
   std::cout << std::format("Request Header:\n- Resource: {}\n- Fields ({}):\n", request.header().resource, std::size(request.header().header_fields));
   for (auto const& [key, val] : request.header().header_fields) {
    std::cout << std::format("-- {}: {}\n", key, val);
   }
   std::cout << std::format("- Variables ({}): \n", std::size(request.header().header_variables));
   for (auto const& [key, val] : request.header().header_variables) {
    std::cout << std::format("-- {}: {}\n", key, val);
   }
   if (request.content().type == net::http_content_type::json) {
    std::cout << "- Data: \n";
    auto const& json = request.content().get_json_content();
    std::cout << json.dump(1) << '\n';
   }

   std::cout << "Sending Response...\n";
   nlohmann::json response_json;
   if constexpr (false) {
    response_json["url"] = std::format("You requested {}", request.header().resource);
    auto const content_length_it = request.header().header_fields.find("Content-Length");
    if (content_length_it == std::cend(request.header().header_fields)) {
     response_json["response"] = "You sent me 0 bytes of content.";
    } else {
     response_json["response"] = std::format("You sent me {} bytes of content.", content_length_it->second);
    }
    response_json["num"] = std::format("Your URL had {} parameters in it.", std::size(request.header().header_variables));
   } else {
    if (request.header().resource == "/test") {
     response_json["message"] = "success";
    } else {
     response_json["message"] = std::format("Unsupported Endpoint: \"{}\"", request.header().resource);
    }
   }
   
   it->send_response(response_json);
   it->close();
  }
  if (client_sockets.fd_count != 64) {
   return;
  }
 }
}