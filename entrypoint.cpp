#include "types.hpp"
#include "webserver.hpp"
#include "webserver_terminal.hpp"
#include "net/db_fetch.hpp"
#include <chrono>
#include <thread>
#include <tuple>
#include <unordered_map>
#include <mysql_connection.h>
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <atomic>
#include <utility>
#include <iostream>


using namespace std;

i32 main() {
 using namespace std::chrono_literals;

 net::socket::init_backend();

 webserver server;
 std::thread server_thread(&webserver::run, std::ref(server));
 webserver_terminal terminal(server);
 std::thread terminal_thread(&webserver_terminal::run, std::ref(terminal));

 server_thread.join();
 terminal_thread.join();

 net::socket::deinit_backend();
}