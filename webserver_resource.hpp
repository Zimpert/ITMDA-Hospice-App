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

 void startup() noexcept;
 void shutdown() noexcept;

 [[nodiscard]] auto get_remote_connection() noexcept {
  return this->remote_connection_pool.get();
 }
 [[nodiscard]] auto get_local_connection() noexcept {
  return this->local_connection_pool.get();
 }
};