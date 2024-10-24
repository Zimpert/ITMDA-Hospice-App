#include "types.hpp"
#include "webserver.hpp"
#include "net/db_fetch.hpp"
#include "login_credentials.hpp"
#include <chrono>
#include <thread>
#include <tuple>
#include <unordered_map>
#include <mysql_connection.h>
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <utility>

/* Reminder:
- Lower client thread priority
 */

using namespace std;

i32 main() {
 using namespace std::chrono_literals;

 //net::socket::init_backend();

 auto data = net::db_fetch<int, net::get_field_type<net::mysql_field_type::str>>(
  crd::host_port,
  crd::username,
  crd::password,
  "DB_Testing",
  "SELECT * FROM TB_Testing"
 );
 std::cout << "Testing:\n";
 for (auto const& entry : data) {
  std::cout << std::format("- {}: {}\n", std::get<0>(entry), (std::string)std::get<1>(entry));
 }
 

 // webserver server;
 // std::thread server_thread(&webserver::run, std::ref(server));
 // std::this_thread::sleep_for(30000ms);
 // server.stop();
 // server_thread.join();

 //net::socket::deinit_backend();
}