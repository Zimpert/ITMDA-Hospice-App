#define UUID_SYSTEM_GENERATOR

#include "webserver.hpp"
#include "local_db_credentials.hpp"
#include "remote_db_credentials.hpp"
#include "stl/cvt/to_hex_string.hpp"
#include "net/db_exec.hpp"
#include "net/db_fetch.hpp"
#include "net/http_header.hpp"
#include "net/socket_error_code.hpp"
#include "net/address_conversion.hpp"
#include <uuid.h>
#include <spdlog/spdlog.h>
#include <span>
#include <format>
#include <string>
#include <thread>
#include <winsock2.h>
#include <ranges>

webserver::webserver() noexcept : m_server{std::invoke([] () noexcept {
 auto maybe_server = net::http_socket::create_server(net::convert_ipv4_string_to_u32("192.168.88.2"), 80);
 if (maybe_server.status != net::socket_error_code::success) [[unlikely]] {
  std::cout << std::format("Failed to Create Server\n{}\n", net::lookup_enum_verbose(maybe_server.status));
 }
 return maybe_server.value;
 })} 
 {
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
   SPDLOG_ERROR("Failed to Select on Server Socket:\n{}", net::lookup_enum_verbose(status));
   return;
  }
  if (has_incoming_connection) {
      std::cout << "Has New Client\n";
          this->accept_client();
          std::cout << "Accepted New Client\n";

  }
  else {
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
  SPDLOG_ERROR("Clients Full!\n");
  return;
 }
 auto maybe_client = this->m_server.accept();
 if (maybe_client.status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Failed to Connect New Client\n");
  return;
 }
 auto const index = it - std::begin(this->m_clients);
 this->m_clients_assigned[index] = false;
 this->m_client_mutices[index].lock();
 *it = std::move(maybe_client.value);
 this->m_client_mutices[index].unlock();
}
void webserver::distribute_jobs() noexcept {
 static constexpr ::timeval timeout{ 0, 8000 };
 auto client_it = std::begin(this->m_clients);
 while (true) {
  static ::fd_set client_sockets{};
  std::array<std::mutex*, FD_SETSIZE> client_mutices{};
  for (client_sockets.fd_count = 0; client_sockets.fd_count < FD_SETSIZE;) {
   if (client_it == std::end(this->m_clients)) {
    break;
   }
   auto const index = client_it - std::begin(this->m_clients);
   if (!this->m_client_mutices[index].try_lock()) /* Need lock to check socket_handle below */ {
    ++client_it;
    continue;
   }
   auto& is_assigned = this->m_clients_assigned[index];
   if (client_it->socket().socket_handle == NULL || is_assigned) {
    this->m_client_mutices[index].unlock();
    ++client_it;
    continue;
   }
   std::cout << std::format("Owns: {}\n", reinterpret_cast<std::size_t>(&this->m_client_mutices[index]));
   client_sockets.fd_array[client_sockets.fd_count] = client_it->socket().socket_handle;
   client_mutices[client_sockets.fd_count] = &this->m_client_mutices[index];
   ++client_it;
   ++client_sockets.fd_count;
  }
  if (client_sockets.fd_count == 0) {
   return;
  }
  decltype(client_sockets) client_sockets_copy;
  (void)std::memcpy(&client_sockets_copy, &client_sockets, sizeof(client_sockets));
  if (::select(NULL, &client_sockets_copy, nullptr, nullptr, &timeout) == SOCKET_ERROR) {
   net::socket_error_code const socket_error_code = static_cast<net::socket_error_code>(::WSAGetLastError());
   if (socket_error_code != net::socket_error_code::success) [[likely]] {
    std::cout << std::format("Failed to Select on Client Sockets\n{}\n", net::lookup_enum_verbose(socket_error_code));
    return; /* To prevent infinite loop */
   }
  }
  /* Unlock Unused Sockets and Find Used Ones */
  std::array<std::mutex*, FD_SETSIZE> mutices;
  for (std::size_t i = 0; auto const [socket_handle, mutex] : std::views::zip(
      std::span{ client_sockets.fd_array, client_sockets.fd_count }, 
      std::span{ std::data(client_mutices), client_sockets.fd_count })) {
   auto socket_handle_it = std::find(std::begin(client_sockets_copy.fd_array), std::end(client_sockets_copy.fd_array), socket_handle);
   if (socket_handle_it == std::end(client_sockets_copy.fd_array)) {
    mutex->unlock();
    std::cout << std::format("Unlocked Unused Mutex #{}\n", std::find_if(std::begin(this->m_client_mutices), std::end(this->m_client_mutices), [&](auto const& client_mutex) noexcept { return &client_mutex == mutex; }) - std::begin(this->m_client_mutices));
   } else {
    mutices[i++] = mutex;
   }
  }
  /* Distribute */
  for (std::size_t mutex_idx = 0; auto const socket_handle : std::span{ client_sockets_copy.fd_array, client_sockets_copy.fd_count }) {
   auto it = std::find_if(std::begin(this->m_clients), std::end(this->m_clients), [&](auto const& client) noexcept { return client.socket().socket_handle == socket_handle; });
   if (it == std::end(this->m_clients)) [[unlikely]] {
    SPDLOG_ERROR("Socket Handle Returned from \"select\" Not Valid\n");
    for (std::size_t i = mutex_idx; i < client_sockets_copy.fd_count; ++i) {
     mutices[i]->unlock();
     std::cout << std::format("Unlocked Failed Socket's Mutex #{}\n", std::find_if(std::begin(this->m_client_mutices), std::end(this->m_client_mutices), [&](auto const& client_mutex) noexcept { return &client_mutex == mutices[mutex_idx]; }) - std::begin(this->m_client_mutices));
    }
    return;
   }

   auto& is_assigned = this->m_clients_assigned[it - std::begin(this->m_clients)];
   if (is_assigned) /* Already Assigned */ {
    mutices[mutex_idx++]->unlock();
    std::cout << std::format("Unlocked Already-Assigned Socket's Mutex #{}\n", std::find_if(std::begin(this->m_client_mutices), std::end(this->m_client_mutices), [&](auto const& client_mutex) noexcept { return &client_mutex == mutices[mutex_idx-1]; }) - std::begin(this->m_client_mutices));
    continue;
   }
   is_assigned = true;

   //this->m_client_mutices[0].unlock();
   mutices[mutex_idx]->unlock();
   std::cout << std::format("Assigned Client: {}\n", reinterpret_cast<std::size_t>(&*it));
   this->m_threadpool.assign(
    &webserver::handle_client_callback,
    &webserver::handle_client_callable,
    &*it,
    std::forward<std::mutex*>(mutices[mutex_idx])
   );
   ++mutex_idx;
   std::cout << std::format("Unlocked Successful Socket's Mutex #{}\n", std::find_if(std::begin(this->m_client_mutices), std::end(this->m_client_mutices), [&](auto const& client_mutex) noexcept { return &client_mutex == mutices[mutex_idx-1]; }) - std::begin(this->m_client_mutices));
  }
  if (client_sockets.fd_count != 64) {
   return;
  }
 }
}

std::tuple<net::http_socket*, std::mutex*> webserver::handle_client_callable(net::http_socket* client, std::mutex* mtx) noexcept {
 //must own mutex for whole lifespan.
 std::cout << std::format("Started Client Interaction (PID {}): {}, mtx {}\n", std::this_thread::get_id(), reinterpret_cast<std::size_t>(client), reinterpret_cast<std::size_t>(mtx));
 
 mtx->lock();
 std::cout << std::format("Locked Mutex In Thread {} for Receive\n", std::this_thread::get_id());
 auto [request_status, request] = client->receive_request();
 if (request_status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Failed HTTP Request Receival for Socket {}: {}\n", stl::cvt::to_hex_string(client->socket().socket_handle), net::lookup_enum_verbose(request_status));
  mtx->unlock();
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 mtx->unlock();
 std::cout << std::format("Unlocked Mutex In Thread {} after Receive\n", std::this_thread::get_id());
 auto const& resource = request.header().resource;
 
 if (resource == "/log") {
  webserver::process_log(request);
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/login") {
  auto const user_details = webserver::process_login(request);

  nlohmann::json response;
  if (std::size(std::get<7>(user_details)) == 0) [[unlikely]] {
   std::cout << "Invalid Login!\n";
   response["UserID"] = nullptr;
   response["Role"] = nullptr;
   response["Name"] = nullptr;
   response["Surname"] = nullptr;
   response["PhoneNo"] = nullptr;
   response["Email"] = nullptr;
   response["Address"] = nullptr;
   response["Token"] = nullptr;
  } else [[likely]] {
   std::cout << "Logged In!\n";
   response["UserID"] = std::get<0>(user_details);
   response["Role"] = std::get<1>(user_details);
   response["Name"] = std::get<2>(user_details);
   response["Surname"] = std::get<3>(user_details);
   response["PhoneNo"] = std::get<4>(user_details);
   response["Email"] = std::get<5>(user_details);
   response["Address"] = std::get<6>(user_details);
   response["Token"] = std::get<7>(user_details);
  }
  mtx->lock();
  std::cout << std::format("Locked Mutex In Thread {} for Send\n", std::this_thread::get_id());
  client->send_response(response);
  mtx->unlock();
  std::cout << std::format("Unlocked Mutex In Thread {} after Send\n", std::this_thread::get_id());
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/shifts") {
  webserver::process_shifts(request);
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/medicine") {
  webserver::process_medicine(request);
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/prelogin") {
  auto const success = webserver::process_prelogin(request);
  mtx->lock();
  if (success) {
   client->send_response(nlohmann::json::parse("{\"Success\": true}"));
  } else {
   client->send_response(nlohmann::json::parse("{\"Success\": false}"));
  }
  mtx->unlock();
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/userinfo") {
  webserver::process_userinfo(request);
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 
 SPDLOG_ERROR("Unknown Resource Requested: {}\n", resource);
 return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };;
}
void webserver::handle_client_callback(std::tuple<net::http_socket*, std::mutex*> stuff) noexcept {
 auto [client, mtx] = stuff;
 mtx->lock();
 std::cout << "Locked Client Mutex in Thread Callback\n";
 client->shutdown();
 auto const retval = client->close();
 if (retval.status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Failed to Close Socket ({}): {}\n", client->socket().socket_handle, net::lookup_enum_verbose(retval.status));
 } else [[likely]] {
  std::cout << std::format("Closed Client: {}\n", reinterpret_cast<std::size_t>(client));
 }
 mtx->unlock();
 std::cout << "Unlocked Client Mutex In Thread Callback\n";
}

void webserver::process_log(net::http_request const& request) noexcept { 
 
}
webserver::login_return_type webserver::process_login(net::http_request const& request) noexcept { 
 auto const& data = request.content().get_json_content();

 auto const email_it = data.find("Email");
 if (email_it == std::cend(data)) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"Email\" variable not found\n");
  return {};
 }
 auto const& email_object = *email_it;
 if (!email_object.is_string()) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"Email\" variable non-string\n");
  return {};
 }
 auto const& email = email_object.get<std::string>();

 auto const password_hash_it = data.find("PasswordHash");
 if (password_hash_it == std::cend(data)) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"PasswordHash\" variable not found\n");
  return {};
 }
 auto const& password_hash_object = *password_hash_it;
 if (!password_hash_object.is_string()) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"PasswordHash\" variable non-string\n");
  return {};
 }
 auto const& password_hash = password_hash_object.get<std::string>();

 auto const query = std::format(
  "SELECT UserID, Role, Name, Surname, Phone, Email, Address "
  "FROM Users "
  "WHERE "
  " Users.Email = '{}' AND "
  " Users.Passwordhash = '{}'",
  email,
  password_hash
 );

 auto user_infos = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str
  >(
   credentials::remote::hostport,
   credentials::remote::username,
   credentials::remote::password,
   credentials::remote::database,
   query
  );
 if (std::size(user_infos) == 0) [[unlikely]] {
  return webserver::login_return_type{"", "", "", "", "", "", "", ""};
 } else {
  auto user_info_pre{ std::move(user_infos[0]) };
  auto user_id{ std::get<0>(user_info_pre).move_to_string() };
  auto uuid = uuids::to_string(uuids::uuid_system_generator{}());
  auto const query = std::format(
   "INSERT INTO TB_HospiceSession (sToken, sUserID) "
   "VALUES ('{}', '{}')",
   uuid,
   user_id
  );
  net::db_exec(credentials::local::hostport, credentials::local::username, credentials::local::password, credentials::local::database, query);
  return std::tuple_cat(
   user_info_type{
    std::move(user_id),
    std::get<1>(user_info_pre).move_to_string(),
    std::get<2>(user_info_pre).move_to_string(),
    std::get<3>(user_info_pre).move_to_string(),
    std::get<4>(user_info_pre).move_to_string(),
    std::get<5>(user_info_pre).move_to_string(),
    std::get<6>(user_info_pre).move_to_string()
   }, 
   std::tuple<std::string>(uuids::to_string(uuids::uuid_system_generator{}())));
 }
}
void webserver::process_shifts(net::http_request const& request) noexcept { 

}
void webserver::process_medicine(net::http_request const& request) noexcept { 

}
bool webserver::process_prelogin(net::http_request const& request) noexcept {
 auto const& data = request.content().get_json_content();
 auto const token_it = data.find("Token");
 if (token_it == std::cend(data)) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"Token\" variable not found\n");
  return false;
 }
 auto const& token_object = *token_it;
 if (!token_object.is_string()) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"Token\" variable non-string\n");
  return false;
 }
 auto const& token = token_object.get<std::string>();
 if (!::is_uuid(token)) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: \"Token\" is not UUIDv4\n");
  return false;
 }

 auto const query = std::format(
  "SELECT * "
  "FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sToken = '{}'",
  token
 );
 auto const session = net::db_fetch<net::mysql_field_type::str, net::mysql_field_type::str, net::mysql_field_type::str>(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query
 );
 return std::size(session) != 0;
}
void webserver::process_userinfo(net::http_request const& request) noexcept { 

}