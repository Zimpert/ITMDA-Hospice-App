#pragma once

#include "webserver_resource.hpp"
#include "stl/threadpool/dynamic_decaying_centralised_threadpool.hpp"
#include "net/http_socket.hpp"
#include <mutex>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>

#include <spdlog/spdlog.h>

class webserver {
public:
 using callable_t = std::function<void(webserver_resource*, net::http_socket*, std::mutex*)>;
 using ddct = stl::threadpool::dynamic_decaying_centralised_threadpool<4096, 1024, callable_t, webserver_resource, net::http_socket*, std::mutex*>;

 using user_info_type = std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string>;
 using login_return_type = std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>;

 webserver() noexcept;
 ~webserver() noexcept;

 void stop() noexcept;
 void run() noexcept;
 
private:
 void accept_incoming_connections() noexcept;
 stl::status_type<net::socket_error_code, bool> has_incoming_connection() const noexcept;
 void accept_client() noexcept;
 void distribute_jobs() noexcept;

 static void handle_client_callable(::webserver_resource* webserver_resource, net::http_socket* client, std::mutex* mtx) noexcept;

 static void process_log(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;
 static login_return_type process_login(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;
 static std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string>> process_shifts(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;
 static std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>> process_medicine(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;
 static bool process_prelogin(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;
 static user_info_type process_userinfo(::webserver_resource* webserver_resource, net::http_request const& request) noexcept;

 net::http_socket m_server;
 std::array<std::mutex, 1024> m_client_mutices;
 std::array<net::http_socket, 1024> m_clients;
 std::array<bool, 1024> m_clients_assigned{};
 ddct m_threadpool;
 bool m_running;
};