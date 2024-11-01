#define UUID_SYSTEM_GENERATOR

#include "webserver.hpp"
#include "request_status.hpp"
#include "local_db_credentials.hpp"
#include "remote_db_credentials.hpp"
#include "stl/cvt/to_hex_string.hpp"
#include "stl/cvt/to_time_point.hpp"
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
#include <chrono>
#include <ranges>
#include <vector>

stl::status_type<request_status, std::string> get_token_from_request(net::http_request const& http_request) noexcept {
 auto const& data = http_request.content().get_json_content();
 auto const token_it = data.find("Token");
 if (token_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::token_not_found, "" };
 }
 auto const& token_object = *token_it;
 if (!token_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::token_not_string, "" };
 }
 auto token = token_object.get<std::string>();
 if (!::is_uuid(token)) [[unlikely]] /* Format Check */ {
  return stl::status_type<request_status, std::string>{ request_status::token_not_uuid, "" };
 }
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(token) };
}
stl::status_type<request_status, std::string> get_userid_from_request(net::http_request const& http_request) noexcept {
 auto const& data = http_request.content().get_json_content();
 auto const userid_it = data.find("UserID");
 if (userid_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::userid_not_found, "" };
 }
 auto const& userid_object = *userid_it;
 if (!userid_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::userid_not_string, "" };
 }
 auto userid = userid_object.get<std::string>();
 if (!::is_uuid(userid)) [[unlikely]] /* Format Check */ {
  return stl::status_type<request_status, std::string>{ request_status::userid_not_uuid, "" };
 }
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(userid) };
}
stl::status_type<request_status, std::string> get_email_from_request(net::http_request const& http_request) noexcept {
 auto const& data = http_request.content().get_json_content();
 auto const email_it = data.find("Email"); 
 if (email_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::email_not_found, "" };
 }
 auto const& email_object = *email_it;
 if (!email_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::email_not_string, "" };
 }
 auto email = email_object.get<std::string>();
 if (!::is_email(email)) [[unlikely]] /* Format Check */ {
  //return stl::status_type<request_status, std::string>{ request_status::email_not_email, "" };
 }
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(email) };
}
stl::status_type<request_status, std::string> get_passwordhash_from_request(net::http_request const& http_request) noexcept {
 auto const& data = http_request.content().get_json_content();
 auto const passwordhash_it = data.find("PasswordHash");
 if (passwordhash_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::passwordhash_not_found, "" };
 }
 auto const& passwordhash_object = *passwordhash_it;
 if (!passwordhash_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::passwordhash_not_string, "" };
 }
 auto passwordhash = passwordhash_object.get<std::string>();
 if (!::is_hex(passwordhash) || std::size(passwordhash) != 64) [[unlikely]] /* Format Check */ {
  return stl::status_type<request_status, std::string>{ request_status::passwordhash_not_hash, "" };
 }
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(passwordhash) };
}

void destroy_session_by_token(std::string_view const token) noexcept {
 auto const query = std::format(
  "DELETE FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sToken = '{}'",
  token);
 net::db_exec(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query);
}
void destroy_session_by_user_id(std::string_view const user_id) noexcept {
 auto const query = std::format(
  "DELETE FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 net::db_exec(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query);
}
std::string fetch_session_userid(std::string_view const token) noexcept {
 using namespace std::chrono_literals;

 auto const query = std::format(
  "SELECT sUserID, sCreated "
  "FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sToken = '{}'",
  token
 );
 auto sessions = net::db_fetch<
  net::mysql_field_type::str, 
  net::mysql_field_type::str>(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query
 );
 if (std::size(sessions) == 0) [[unlikely]] {
  return std::string("");
 };
 
 auto session = std::make_tuple(std::get<0>(sessions[0]).move_to_string(), std::get<1>(sessions[0]).move_to_string());
 std::cout << std::chrono::system_clock::now() << ", " << stl::cvt::to_time_point(std::get<1>(session)) << '\n';
 if (std::chrono::system_clock::now() - stl::cvt::to_time_point(std::get<1>(session)) > 3600s) [[unlikely]] {
  destroy_session_by_token(token);
  return std::string("");
 } else [[likely]] {
  return std::move(std::get<0>(session));
 }
}
std::string fetch_session_token(std::string_view const user_id) noexcept {
 using namespace std::chrono_literals;

 auto const query = std::format(
  "SELECT sToken, sCreated "
  "FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 auto sessions = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query);
 if (std::size(sessions) == NULL) [[unlikely]] {
  std::cout << "fok\n";
  return std::string("");
 }
 auto session = std::make_tuple(std::get<0>(sessions[0]).move_to_string(), std::get<1>(sessions[0]).move_to_string());
  if (std::chrono::system_clock::now() - stl::cvt::to_time_point(std::get<1>(session)) > 3600s) [[unlikely]] {
   std::cout << "warrafak you are not expire you liar\n";
  destroy_session_by_user_id(user_id);
  return std::string("");
 } else [[likely]] {
  return std::move(std::get<0>(session));
 }
}
void update_session(std::string_view const user_id) noexcept {
 auto const query = std::format(
  "UPDATE TB_HospiceSession "
  "SET TB_HospiceSession.sCreated = NOW() "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 net::db_exec(
  credentials::local::hostport,
  credentials::local::username,
  credentials::local::password,
  credentials::local::database,
  query);
}
void create_session(std::string_view const token, std::string_view const user_id) noexcept {
  auto const query = std::format(
   "INSERT INTO TB_HospiceSession (sToken, sUserID) "
   "VALUES ('{}', '{}')",
   token,
   user_id);
  net::db_exec(
   credentials::local::hostport, 
   credentials::local::username, 
   credentials::local::password, 
   credentials::local::database, 
   query);
}

webserver::user_info_type fetch_userinfo(std::string_view const userid) {
 auto const query = std::format(
  "SELECT UserID, Role, Name, Surname, Phone, Email, Address "
  "FROM Users "
  "WHERE Users.UserID = '{}'",
  userid
 );
 auto userinfos = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
  credentials::remote::hostport,
  credentials::remote::username,
  credentials::remote::password,
  credentials::remote::database,
  query);
 if (std::size(userinfos) == 0) [[unlikely]] {
  SPDLOG_ERROR("Failed to Fetch User Info");
  return {};
 } else [[likely]] {
  return std::make_tuple(
   std::get<0>(userinfos[0]).move_to_string(),
   std::get<1>(userinfos[0]).move_to_string(), 
   std::get<2>(userinfos[0]).move_to_string(), 
   std::get<3>(userinfos[0]).move_to_string(), 
   std::get<4>(userinfos[0]).move_to_string(), 
   std::get<5>(userinfos[0]).move_to_string(), 
   std::get<6>(userinfos[0]).move_to_string());
 }
}
webserver::user_info_type fetch_userinfo(std::string_view const email, std::string_view const passwordhash) noexcept {
 auto const query = std::format(
  "SELECT UserID, Role, Name, Surname, Phone, Email, Address "
  "FROM Users "
  "WHERE "
  " Users.Email = '{}' AND "
  " Users.Passwordhash = '{}'",
  email,
  passwordhash);

 auto user_infos = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
  credentials::remote::hostport,
  credentials::remote::username,
  credentials::remote::password,
  credentials::remote::database,
  query);
 if (std::size(user_infos) == 0) [[unlikely]] {
  return {};
 } else [[likely]] {
  return std::make_tuple(
   std::get<0>(user_infos[0]).move_to_string(),
   std::get<1>(user_infos[0]).move_to_string(),
   std::get<2>(user_infos[0]).move_to_string(),
   std::get<3>(user_infos[0]).move_to_string(),
   std::get<4>(user_infos[0]).move_to_string(),
   std::get<5>(user_infos[0]).move_to_string(),
   std::get<6>(user_infos[0]).move_to_string());
 }
}

std::tuple<std::string, std::string, std::string> fetch_minimal_users(std::string_view const user_id) noexcept {
 auto const query = std::format(
  "SELECT Name, Surname, Address "
  "FROM Users "
  "WHERE Users.UserID = '{}'",
  user_id);
 auto minimal_user_infos = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
   credentials::remote::hostport,
   credentials::remote::username,
   credentials::remote::password,
   credentials::remote::database,
   query);
 if (std::size(minimal_user_infos) == NULL) [[unlikely]] {
  return {};
 }
 return std::make_tuple(
  std::get<0>(minimal_user_infos[0]).move_to_string(),
  std::get<0>(minimal_user_infos[0]).move_to_string(),
  std::get<0>(minimal_user_infos[0]).move_to_string());
}

std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string>> fetch_caregiver_shifts(std::string_view const user_id) noexcept {
 auto const query = std::format(
  "SELECT Users.Name, Users.Surname, Users.Address, CaregiverShifts.ShiftStart, CaregiverShifts.ShiftEnd "
  "FROM Users INNER JOIN CaregiverShifts "
  "ON Users.UserID = CaregiverShifts.PatientID "
  "WHERE Users.UserID IN ("
  " SELECT CaregiverShifts.PatientID "
  " FROM Users INNER JOIN CaregiverShifts ON Users.UserID = CaregiverShifts.CaregiverID "
  " WHERE Users.UserID = '{}')",
  user_id);
 auto patient_infos = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
  credentials::remote::hostport,
  credentials::remote::username,
  credentials::remote::password,
  credentials::remote::database,
  query);
 if (std::size(patient_infos) == NULL) [[unlikely]] {
  return {};
 }
 std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string>> temp(std::size(patient_infos));
 (void)std::transform(std::begin(patient_infos), std::end(patient_infos), std::begin(temp), [](auto&& patient_info) { 
  return std::make_tuple(
   std::get<0>(patient_info).move_to_string(),
   std::get<1>(patient_info).move_to_string(),
   std::get<2>(patient_info).move_to_string(),
   std::get<3>(patient_info).move_to_string(),
   std::get<4>(patient_info).move_to_string()
  );});
 return std::move(temp);
}

std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>> fetch_patient_medications(std::string_view const patient_id) noexcept {
 auto const query = std::format(
  "SELECT "
  " Medications.MedicationName, Medications.Description, Medications.Interactions, "
  " MedicationDays.Day, MedicationDays.Frequency, MedicationDays.Dosage, "
  " PatientMedications.StartDate, PatientMedications.EndDate "
  "FROM "
  " PatientMedications "
  "  INNER JOIN Medications "
  "   ON PatientMedications.MedicationID = Medications.MedicationID "
  "  INNER JOIN MedicationDays "
  "   ON PatientMedications.PatientMedicationID = MedicationDays.PatientMedicationID "
  "WHERE PatientMedications.PatientID = '{}'",
  patient_id);
 auto patient_medications = net::db_fetch<
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str,
  net::mysql_field_type::str>(
  credentials::remote::hostport,
  credentials::remote::username,
  credentials::remote::password,
  credentials::remote::database,
  query);
 if (std::size(patient_medications) == NULL) [[unlikely]] {
  return {};
 }
 std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>> temp(std::size(patient_medications));
 (void)std::transform(std::begin(patient_medications), std::end(patient_medications), std::begin(temp), [&](auto&& patient_medication) noexcept {
  return std::make_tuple(
   std::get<0>(patient_medication).move_to_string(),
   std::get<1>(patient_medication).move_to_string(),
   std::get<2>(patient_medication).move_to_string(),
   std::get<3>(patient_medication).move_to_string(),
   std::get<4>(patient_medication).move_to_string(),
   std::get<5>(patient_medication).move_to_string(),
   std::get<6>(patient_medication).move_to_string(),
   std::get<7>(patient_medication).move_to_string()
  );});
 return std::move(temp);
}

std::vector<std::string> fetch_caregiver_userids(std::string_view const caregiver_id) noexcept {
 auto const query = std::format(
  "SELECT PatientID "
  "FROM AssignedPatients "
  "WHERE AssignedPatients.CaregiverID = '{}'",
  caregiver_id);
 auto caregiver_userids = net::db_fetch<net::mysql_field_type::str>(
  credentials::remote::hostport,
  credentials::remote::username,
  credentials::remote::password,
  credentials::remote::database,
  query);
 if (std::size(caregiver_userids) == 0) {
  return {};
 } else {
  std::vector<std::string> temp(std::size(caregiver_userids));
  std::transform(std::begin(caregiver_userids), std::end(caregiver_userids), std::begin(temp), [](auto&& userid) noexcept { return std::get<0>(userid).move_to_string(); });
  return std::move(temp);
 }
}

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
   this->accept_client();
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
 SPDLOG_INFO("Accepted New Client: {}:{}", net::convert_ipv4_u32_to_string(maybe_client.value.socket().host), maybe_client.value.socket().port);
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
    SPDLOG_ERROR("Failed to Select on Client Sockets: {}", net::lookup_enum_verbose(socket_error_code));
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

   mutices[mutex_idx]->unlock();
   std::cout << std::format("Assigned Client ({}:{})\n", net::convert_ipv4_u32_to_string(it->socket().host), it->socket().port);
   this->m_threadpool.assign(
    &webserver::handle_client_callback,
    &webserver::handle_client_callable,
    &*it,
    std::forward<std::mutex*>(mutices[mutex_idx])
   );
   
   ++mutex_idx;
  }
  if (client_sockets.fd_count != 64) {
   return;
  }
 }
}

void webserver::handle_client_callback(std::tuple<net::http_socket*, std::mutex*> stuff) noexcept {
 auto [client, mtx] = stuff;
 mtx->lock();
 client->shutdown();
 auto const retval = client->close();
 if (retval.status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Failed to Close Socket ({}:{}): {}\n", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(retval.status));
 } else [[likely]] {
  SPDLOG_INFO("Closed Client: {}:{}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
 }
 mtx->unlock();
}
std::tuple<net::http_socket*, std::mutex*> webserver::handle_client_callable(net::http_socket* client, std::mutex* mtx) noexcept {
 //must own mutex for whole lifespan.
 auto const thread_id = std::this_thread::get_id();
 SPDLOG_INFO("Started Client Interaction (PID {}): {}:{}", *reinterpret_cast<u32 const*>(&thread_id), net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
 
 mtx->lock();
 auto [request_status, request] = client->receive_request();
 if (request_status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Failed HTTP Request Receival for Socket {}: {}\n", stl::cvt::to_hex_string(client->socket().socket_handle), net::lookup_enum_verbose(request_status));
  mtx->unlock();
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 mtx->unlock();
 auto const& resource = request.header().resource;
 
 if (resource == "/log") {
  webserver::process_log(request);
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/login") {
  auto const user_details = webserver::process_login(request);

  nlohmann::json response;
  if (std::size(std::get<7>(user_details)) == 0) [[unlikely]] {
   SPDLOG_INFO("Client ({}:{}): Invalid Login", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   response["UserID"]  = nullptr;
   response["Role"]    = nullptr;
   response["Name"]    = nullptr;
   response["Surname"] = nullptr;
   response["PhoneNo"] = nullptr;
   response["Email"]   = nullptr;
   response["Address"] = nullptr;
   response["Token"]   = nullptr;
  } else [[likely]] {
   SPDLOG_INFO("Client ({}:{}): Logged In", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
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
  client->send_response(response);
  mtx->unlock();
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/shifts") {
  auto shifts = webserver::process_shifts(request);
  nlohmann::json response;
  if (std::size(shifts) == NULL) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}): Shifts Failure", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   response["data"] = nullptr;
  } else [[likely]] {
   SPDLOG_INFO("Client ({}:{}): Shifts Success", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   for (auto&& shift : shifts) {
    response["data"].push_back(
     nlohmann::json({
      { "Name",       std::move(std::get<0>(shift)) },
      { "Surname",    std::move(std::get<1>(shift)) },
      { "Address",    std::move(std::get<2>(shift)) },
      { "ShiftStart", std::move(std::get<3>(shift)) },
      { "ShiftEnd",   std::move(std::get<4>(shift)) },
     }));
   }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  mtx->unlock();
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
  }
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/medicine") {
  auto patient_medications = webserver::process_medicine(request);
  nlohmann::json response;
  if (std::size(patient_medications) == NULL) [[unlikely]] {
   SPDLOG_INFO("Client ({}:{}): Patient has no Medication", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   response["data"] = nullptr;
  } else {
   SPDLOG_INFO("Client ({}:{}): Patient Medication Retrieved", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   for (auto&& patient_medication : patient_medications) {
    response["data"].push_back({
     { "Name"        , std::move(std::get<0>(patient_medication)) },
     { "Description" , std::move(std::get<1>(patient_medication)) },
     { "Interactions", std::move(std::get<2>(patient_medication)) },
     { "Day"         , std::move(std::get<3>(patient_medication)) },
     { "Frequency"   , std::move(std::get<4>(patient_medication)) },
     { "Dosage"      , std::move(std::get<5>(patient_medication)) },
     { "StartDate"   , std::move(std::get<6>(patient_medication)) },
     { "EndDate"     , std::move(std::get<7>(patient_medication)) },
    });
   }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  mtx->unlock();
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: {}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
  }
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/prelogin") {
  std::cout << "prelogin started\n";
  auto const success = webserver::process_prelogin(request);
  mtx->lock();
  auto const send_status = std::invoke([&]() noexcept {
   if (success) {
    SPDLOG_INFO("Client ({}:{}): Prelogin Success", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
    return client->send_response(nlohmann::json::parse("{\"Success\": true}")).status;
   } else {
    SPDLOG_INFO("Client ({}:{}): Prelogin Failure", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
    return client->send_response(nlohmann::json::parse("{\"Success\": false}")).status;
   }
  });
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: ", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
  }
  mtx->unlock();
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 if (resource == "/userinfo") {
  auto userinfo = webserver::process_userinfo(request);
  nlohmann::json response;
  if (std::size(std::get<0>(userinfo)) == NULL) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}): Failed to Retrieve UserInfo", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   response["UserID"]  = nullptr;
   response["Role"]    = nullptr;
   response["Name"]    = nullptr;
   response["Surname"] = nullptr;
   response["PhoneNo"] = nullptr;
   response["Email"]   = nullptr;
   response["Address"] = nullptr;
  } else {
   SPDLOG_INFO("Client ({}:{}): Retrieved UserInfo", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   response["UserID"]  = std::get<0>(userinfo);
   response["Role"]    = std::get<1>(userinfo);
   response["Name"]    = std::get<2>(userinfo);
   response["Surname"] = std::get<3>(userinfo);
   response["PhoneNo"] = std::get<4>(userinfo);
   response["Email"]   = std::get<5>(userinfo);
   response["Address"] = std::get<6>(userinfo);
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  mtx->unlock();
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: {}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
  }
  return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
 }
 
 SPDLOG_ERROR("Client ({}:{}): Unknown Resource Requested (\"{}\")", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, resource);
 return std::tuple<net::http_socket*, std::mutex*>{ client, mtx };
}


void webserver::process_log(net::http_request const& request) noexcept { 
 
}
//reset datetime to now for session
webserver::login_return_type webserver::process_login(net::http_request const& request) noexcept {
 auto const maybe_email = get_email_from_request(request);
 if (maybe_email.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_email.status));
  return {};
 }
 auto const& email = maybe_email.value;

 auto const maybe_passwordhash = get_passwordhash_from_request(request);
 if (maybe_passwordhash.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_passwordhash.status));
  return {};
 }
 auto const& passwordhash = maybe_passwordhash.value;

 auto userinfo = fetch_userinfo(email, passwordhash);
 if (std::size(std::get<0>(userinfo)) == 0) [[unlikely]] {
  SPDLOG_ERROR("Failed to Fetch User Info");
  return {};
 }

 auto token = std::invoke([&]() noexcept {
  auto token = fetch_session_token(std::get<0>(userinfo));
  std::cout << std::format("{}\n", token);
  if (std::size(token) == 0) {
   token = uuids::to_string(uuids::uuid_system_generator{}());
   std::cout << std::format("{}\n", token);
   create_session(token, std::get<0>(userinfo));
   return token;
  } else {
   std::cout << "updated session\n";
   update_session(std::get<0>(userinfo));
   return token;
  }
 });

 return std::tuple_cat(userinfo, std::make_tuple(token));
}
std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string>> webserver::process_shifts(net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 auto shift_infos = fetch_caregiver_shifts(user_id);
 if (std::size(shift_infos) == NULL) [[unlikely]] {
  return {};
 } else [[likely]] {
  return std::move(shift_infos);
 }
}
std::vector<std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>> webserver::process_medicine(net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 auto user_info = fetch_userinfo(user_id);
 if (std::size(std::get<0>(user_info)) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: Failed to Fetch User Info From DB");
  return {};
 }
 auto& role = std::get<1>(user_info);

 auto const maybe_other_user_id = get_userid_from_request(request);
 switch (maybe_other_user_id.status) {
 case request_status::success: [[likely]] {
  break;
 }
 default: [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_other_user_id.status));
  break;
 }
 }
 auto& other_user_id = maybe_other_user_id.value;
 auto other_user_info = fetch_userinfo(other_user_id);
 if (std::size(std::get<0>(other_user_info)) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Failed to Fetch Other User ({})'s Info", other_user_id);
  return {};
 }

 if (role == "Patient") {
  if (user_id != other_user_id) [[unlikely]] {
   SPDLOG_ERROR("Invalid Request: Patient {} attempting to request medication from other patient {}", user_id, other_user_id);
   return {};
  }
 } else if (role == "Caregiver") {
  auto const caregiver_userids = fetch_caregiver_userids(user_id);
  if (std::size(caregiver_userids) == NULL) [[unlikely]] {
   SPDLOG_ERROR("Failed to Fetch Caretaker {}'s Patients", user_id);
   return {};
  }
  if (std::find(std::cbegin(caregiver_userids), std::cend(caregiver_userids), other_user_id) == std::cend(caregiver_userids)) [[unlikely]] {
   SPDLOG_ERROR("Caretaker {} attempted to fetch medical information about patient {} for whom they don't care", user_id, other_user_id);
   return {};
  }
 }

 return fetch_patient_medications(other_user_id);
}
bool webserver::process_prelogin(net::http_request const& request) noexcept {
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return false;
 }
 auto const& token = maybe_token.value;
 return std::size(fetch_session_userid(token)) != 0;
}
webserver::user_info_type webserver::process_userinfo(net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 auto user_info = fetch_userinfo(user_id);
 if (std::size(std::get<0>(user_info)) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: Failed to Fetch User Info From DB");
  return {};
 }
 auto& role = std::get<1>(user_info);

 auto const maybe_other_user_id = get_userid_from_request(request);
 switch (maybe_other_user_id.status) {
 case request_status::success: [[likely]] {
  break;
 }
 default: [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_other_user_id.status));
  break;
 }
 }
 auto& other_user_id = maybe_other_user_id.value;
 auto other_user_info = fetch_userinfo(other_user_id);
 if (std::size(std::get<0>(other_user_info)) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: Failed to Fetch Other User's Info");
  return {};
 }
 if (std::get<0>(other_user_info) == user_id) /* You're allowed to request your own info */ {
  return std::move(other_user_info);
 }

 if (role == "Patient") [[unlikely]] {
  SPDLOG_ERROR("Client {} trying to request somebody else ({})'s data.", user_id, std::get<1>(other_user_info));
  return {};
 }

 auto const patient_ids = fetch_caregiver_userids(user_id);
 if (std::find(std::cbegin(patient_ids), std::cend(patient_ids), other_user_id) == std::cend(patient_ids)) [[unlikely]] {
  SPDLOG_ERROR("Caretaker {} trying to request user {}'s data, for whom they are not currently a caretaker.", user_id, other_user_id);
  return {};
 }
 auto const requested_info = fetch_userinfo(other_user_id);
 if (std::size(std::get<0>(requested_info)) == 0) [[unlikely]] {
  SPDLOG_ERROR("Caretaker {} trying to request non-existend user {}'s data.", user_id, other_user_id);
  return {};
 }
 
 return std::move(requested_info);
}