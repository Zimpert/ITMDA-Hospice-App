#pragma once

#include "stl/threadpool/dynamic_decaying_centralised_threadpool.hpp"
#include "net/http_socket.hpp"

class webserver {
public:
 using callback_t = std::function<void(int)>;
 using callable_t = std::function<int(int, int)>;
 using ddct = stl::threadpool::dynamic_decaying_centralised_threadpool<4096, 1024, callback_t, callable_t, int, int>;

 webserver() noexcept;
 ~webserver() noexcept;

 void stop() noexcept;
 void run() noexcept;
 
private:
 void accept_incoming_connections() noexcept;
 stl::status_type<net::socket_error_code, bool> has_incoming_connection() const noexcept;
 void accept_client() noexcept;
 void distribute_jobs() noexcept;

 net::http_socket m_server;
 std::array<net::http_socket, 1024> m_clients;
 ddct m_threadpool;
 bool m_running;
};