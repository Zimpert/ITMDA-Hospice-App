#include "types.hpp"
#include "webserver.hpp"
#include <chrono>
#include <thread>
#include <unordered_map>

/* Reminder:
- Lower client thread priority
 */

i32 main() {
 using namespace std::chrono_literals;

 net::socket::init_backend();

 webserver server;
 std::thread server_thread(&webserver::run, std::ref(server));
 std::this_thread::sleep_for(30000ms);
 server.stop();
 server_thread.join();

 net::socket::deinit_backend();
}