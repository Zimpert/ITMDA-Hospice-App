#pragma once

#include "webserver_resource.hpp"
#include "db_objects/core.hpp"
#include "stl/threadpool/dynamic_decaying_centralised_threadpool.hpp"
#include "net/http_socket.hpp"
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <spdlog/spdlog.h>
#include <span>
#include <mutex>

/* Remember to use timestamps */

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

 std::span<net::http_socket const> clients() const noexcept;
 std::span<ddct::wthread_t const> wthreads() const noexcept;
 auto const& thread_resource() const noexcept { return this->m_threadpool.resource(); }
 
private:
 void accept_incoming_connections() noexcept;
 stl::status_type<net::socket_error_code, bool> has_incoming_connection() const noexcept;
 void accept_client() noexcept;
 void distribute_jobs() noexcept;

 static void handle_client_callable(::webserver_resource* resource, net::http_socket* client, std::mutex* mtx) noexcept;

 static void                                             process_log(webserver_resource* resource, net::http_request const& request) noexcept;
 static db_objects::login_user_info                      process_login(webserver_resource* resource, net::http_request const& request) noexcept;
 static std::vector<db_objects::shift_info>              process_shifts(webserver_resource* resource, net::http_request const& request) noexcept;
 static std::vector<db_objects::patient_medication_info> process_medicine(webserver_resource* resource, net::http_request const& request) noexcept;
 static bool                                             process_prelogin(webserver_resource* resource, net::http_request const& request) noexcept;
 static db_objects::user_info                            process_userinfo(webserver_resource* resource, net::http_request const& request) noexcept;
 static std::vector<db_objects::patient_info>            process_patientinfos(webserver_resource* resource, net::http_request const& request) noexcept;

 net::http_socket m_server;
 std::array<std::mutex, 1024> m_client_mutices;
 std::array<std::chrono::steady_clock::time_point, 1024> m_client_timestamps;
 std::array<net::http_socket, 1024> m_clients;
 std::array<bool, 1024> m_clients_assigned{};
 ddct m_threadpool;
 bool m_running;
};