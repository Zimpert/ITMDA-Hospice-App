#pragma once

#include "local_db_credentials.hpp"
#include "remote_db_credentials.hpp"
#include "net/connection_pool.hpp"
#include <mutex>
#include <vector>

struct webserver_resource {
 net::connection_pool<4> remote_connection_pool{ remote::credentials };
 net::connection_pool<128> local_connection_pool{ local::credentials };

 webserver_resource() = default;
 webserver_resource(webserver_resource const&) = delete;
 ~webserver_resource() noexcept {
  SPDLOG_WARN("Webserver Resource Destructor Called...");
 }

 void startup() noexcept;
 void shutdown() noexcept;

 [[nodiscard]] auto get_remote_connection() noexcept {
  while (true) {
   auto connection = this->remote_connection_pool.get();
   if (connection.connection() != nullptr) {
    return connection;
   }
  }
 }
 [[nodiscard]] auto get_local_connection() noexcept {
  while (true) {
   auto connection = this->local_connection_pool.get();
   if (connection.connection() != nullptr) {
    return connection;
   }
  }
 }
};