#pragma once

#include "stl/threadpool/dynamic_decaying_centralised_threadpool.hpp"
#include "net/http_socket.hpp"
#include <mutex>

class webserver {
public:
 using callback_t = std::function<void(std::tuple<net::http_socket*, std::mutex*>)>;
 using callable_t = std::function<std::tuple<net::http_socket*, std::mutex*>(net::http_socket*, std::mutex*)>;
 using ddct = stl::threadpool::dynamic_decaying_centralised_threadpool<4096, 1024, callback_t, callable_t, net::http_socket*, std::mutex*>;

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

 static std::tuple<net::http_socket*, std::mutex*> handle_client_callable(net::http_socket* client, std::mutex* mtx) noexcept;
 static void handle_client_callback(std::tuple<net::http_socket*, std::mutex*> stuff) noexcept;

 static void process_log(net::http_request const& request) noexcept;
 static login_return_type process_login(net::http_request const& request) noexcept;
 static std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string>> process_shifts(net::http_request const& request) noexcept;
 static std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>> process_medicine(net::http_request const& request) noexcept;
 static bool process_prelogin(net::http_request const& request) noexcept;
 static user_info_type process_userinfo(net::http_request const& request) noexcept;

 net::http_socket m_server;
 std::mutex m_clients_mutex; //naive
 std::array<std::mutex, 1024> m_client_mutices;
 std::array<net::http_socket, 1024> m_clients;
 std::array<bool, 1024> m_clients_assigned{};
 ddct m_threadpool;
 bool m_running;
};