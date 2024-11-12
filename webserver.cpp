#define UUID_SYSTEM_GENERATOR

#include "webserver.hpp"
#include "request_status.hpp"
#include "local_db_credentials.hpp"
#include "remote_db_credentials.hpp"
#include "stl/cvt/move_vector.hpp"
#include "stl/cvt/to_hex_string.hpp"
#include "stl/cvt/to_time_point.hpp"
#include "net/db_exec.hpp"
#include "net/db_fetch.hpp"
#include "net/http_header.hpp"
#include "net/socket_error_code.hpp"
#include "net/address_conversion.hpp"
#include "timing/scoped_timer.hpp"
#include <uuid.h>
#include <spdlog/spdlog.h>
#include <span>
#include <format>
#include <string>
#include <thread>
#include <chrono>
#include <ranges>
#include <vector>
#include <filesystem>
#include <winsock2.h>

inline constexpr bool LOG_API_STATUS = true;

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
stl::status_type<request_status, std::string> get_date_due_from_request(net::http_request const& request) noexcept {
 auto const& data = request.content().get_json_content();
 auto const date_due_it = data.find("DateDue");
 if (date_due_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::date_due_not_found, "" };
 }
 auto const& date_due_object = *date_due_it;
 if (!date_due_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::date_due_not_string, "" };
 }
 auto date_due = date_due_object.get<std::string>();
 if (!is_datetime<"YYYY-MM-DD hh:mm:ss">(date_due)) [[unlikely]] {
  return stl::status_type<request_status, std::string>{ request_status::date_due_not_date, "" };
 }
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(date_due) };
}
stl::status_type<request_status, std::string> get_description_from_request(net::http_request const& request) noexcept {
 auto const& data = request.content().get_json_content();
 auto const description_it = data.find("Description");
 if (description_it == std::cend(data)) [[unlikely]] /* Presence Check */ {
  return stl::status_type<request_status, std::string>{ request_status::description_not_found, "" };
 }
 auto const& description_object = *description_it;
 if (!description_object.is_string()) [[unlikely]] /* Type Check */ {
  return stl::status_type<request_status, std::string>{ request_status::description_not_string, "" };
 }
 auto description = description_object.get<std::string>();
 return stl::status_type<request_status, std::string>{ request_status::success, std::move(description) };
}

void destroy_session_by_token(webserver_resource* webserver_resource, std::string_view const token) noexcept {
 auto const query = std::format(
  "DELETE FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sToken = '{}'",
  token);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return; }
 net::db_exec(connection.connection(), local::database, query);
}
void destroy_session_by_user_id(webserver_resource* webserver_resource, std::string_view const user_id) noexcept {
 SPDLOG_INFO("destroy session by user id");
 auto const query = std::format(
  "DELETE FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return; }
 net::db_exec(connection.connection(), local::database, query);
}
std::string fetch_session_userid(webserver_resource* webserver_resource, std::string_view const token) noexcept {
 using namespace std::chrono_literals;
 auto const query = std::format(
  "SELECT sUserID, sCreated "
  "FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sToken = '{}'",
  token);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return ""; }
 auto sessions = net::db_fetch<net::mysql_field_type::str, net::mysql_field_type::str>(connection.connection(), local::database, query);
 if (std::size(sessions) == 0) [[unlikely]] { return ""; }
 auto user_id = std::get<0>(sessions[0]);
 auto date = std::get<1>(sessions[0]);
 if (std::chrono::system_clock::now() - stl::cvt::to_time_point(date) > 3600s) [[unlikely]] {
  destroy_session_by_token(webserver_resource, token);
  return "";
 } else [[likely]] { return std::move(user_id); }
}
std::string fetch_session_token(webserver_resource* webserver_resource, std::string_view const user_id) noexcept {
 using namespace std::chrono_literals;

 auto const query = std::format(
  "SELECT sToken, sCreated "
  "FROM TB_HospiceSession "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return ""; }
 auto sessions = net::db_fetch<net::mysql_field_type::str, net::mysql_field_type::str>(connection.connection(), local::database, query);
 if (std::size(sessions) == NULL) [[unlikely]] { return ""; }
 auto& token = std::get<0>(sessions[0]);
 auto& date = std::get<1>(sessions[0]);
 if (std::chrono::system_clock::now() - stl::cvt::to_time_point(date) > 3600s) [[unlikely]] {
  destroy_session_by_user_id(webserver_resource, user_id);
  return "";
 } else [[likely]] { return std::move(token); }
}
void update_session(webserver_resource* webserver_resource, std::string_view const user_id) noexcept {
 auto const query = std::format(
  "UPDATE TB_HospiceSession "
  "SET TB_HospiceSession.sCreated = NOW() "
  "WHERE TB_HospiceSession.sUserID = '{}'",
  user_id);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return; }
 net::db_exec(connection.connection(), local::database, query);
}
void create_session(webserver_resource* webserver_resource, std::string_view const token, std::string_view const user_id) noexcept {
 auto const query = std::format(
  "INSERT INTO TB_HospiceSession (sToken, sUserID) "
  "VALUES ('{}', '{}')",
  token, user_id);
 auto connection = webserver_resource->get_local_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return; }
 net::db_exec(connection.connection(), local::database, query);
}

db_objects::user_info fetch_user_info(webserver_resource* resource, std::string_view const userid) {
 static constexpr auto mysql_str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT UserID, Role, Name, Surname, Phone, Email, Address "
  "FROM Users "
  "WHERE Users.UserID = '{}'",
  userid);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto userinfos = net::db_fetch<mysql_str, mysql_str, mysql_str, mysql_str, mysql_str, mysql_str, mysql_str>(connection.connection(), remote::database, query);
 if (std::size(userinfos) == 0) [[unlikely]] { return {}; }
 else [[likely]] { return db_objects::user_info{std::move(userinfos[0])}; }
}
db_objects::user_info fetch_user_info(webserver_resource* resource, std::string_view const email, std::string_view const passwordhash) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT UserID, Role, Name, Surname, Phone, Email, Address "
  "FROM Users "
  "WHERE "
  " Users.Email = '{}' AND "
  " Users.Passwordhash = '{}'",
  email, passwordhash);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto user_infos = net::db_fetch<str, str, str, str, str, str, str>(connection.connection(), remote::database, query);
 if (std::size(user_infos) == 0) [[unlikely]] { return {}; }
 else [[likely]] { return db_objects::user_info{std::move(user_infos[0])}; }
}

db_objects::minimal_user_info fetch_minimal_user_info(webserver_resource* webserver_resource, std::string_view const user_id) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT Name, Surname, Address "
  "FROM Users "
  "WHERE Users.UserID = '{}'",
  user_id);
 auto const connection = webserver_resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto minimal_user_infos = net::db_fetch<str, str, str>(connection.connection(), remote::database, query);
 if (std::size(minimal_user_infos) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return db_objects::minimal_user_info{std::move(minimal_user_infos[0])}; }
}

std::vector<db_objects::shift_info> fetch_caregiver_shifts(webserver_resource* resource, std::string_view const user_id) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT Users.Name, Users.Surname, Users.Address, CaregiverShifts.ShiftStart, CaregiverShifts.ShiftEnd "
  "FROM Users INNER JOIN CaregiverShifts "
  "ON Users.UserID = CaregiverShifts.PatientID "
  "WHERE Users.UserID IN ("
  " SELECT CaregiverShifts.PatientID "
  " FROM Users INNER JOIN CaregiverShifts ON Users.UserID = CaregiverShifts.CaregiverID "
  " WHERE Users.UserID = '{}')",
  user_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto patient_infos = net::db_fetch<str, str, str, str, str>(connection.connection(), remote::database, query);
 if (std::size(patient_infos) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return stl::cvt::move_vector<decltype(patient_infos)::value_type, db_objects::shift_info>(patient_infos); }
}
std::vector<db_objects::patient_medication_info> fetch_patient_medications(webserver_resource* resource, std::string_view const caregiver_id) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT Users.UserID, Users.Name, Users.Surname, MedicationDays.Day, MedicationDays.Frequency, PatientMedications.StartDate, PatientMedications.EndDate, Medications.MedicationName, Medications.Description, Medications.Interactions, MedicationDays.Dosage " 
  "FROM AssignedPatients "
  " INNER JOIN Users "
  "  ON AssignedPatients.PatientID = Users.UserID "
  " INNER JOIN PatientMedications "
  "  ON AssignedPatients.PatientID = PatientMedications.PatientID "
  " INNER JOIN Medications "
  "  ON PatientMedications.MedicationID = Medications.MedicationID "
  " INNER JOIN MedicationDays "
  "  ON PatientMedications.PatientMedicationID = MedicationDays.PatientMedicationID "
  "WHERE AssignedPatients.CaregiverID = '{}'",
  caregiver_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto patient_medications = net::db_fetch<str, str, str, str, str, str, str, str, str, str, str>(connection.connection(), remote::database, query);
 if (std::size(patient_medications) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return stl::cvt::move_vector<decltype(patient_medications)::value_type, db_objects::patient_medication_info>(patient_medications); }
}

std::vector<std::string> fetch_caregiver_userids(webserver_resource* resource, std::string_view const caregiver_id) noexcept {
 auto const query = std::format(
  "SELECT PatientID "
  "FROM AssignedPatients "
  "WHERE AssignedPatients.CaregiverID = '{}'",
  caregiver_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto caregiver_userids = net::db_fetch<net::mysql_field_type::str>(connection.connection(), remote::database, query);
 if (std::size(caregiver_userids) == 0) [[unlikely]] { return {}; } 
 else [[likely]] {
  std::vector<std::string> temp(std::size(caregiver_userids));
  std::transform(std::begin(caregiver_userids), std::end(caregiver_userids), std::begin(temp), [](auto&& user_id) noexcept { return std::move(std::get<0>(user_id)); });
  return temp;
 }
}
std::vector<db_objects::patient_info> fetch_patient_infos(webserver_resource* resource, std::string_view const caregiver_id) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  "SELECT Users.UserID, Users.Name, Users.Surname, Users.Phone, Users.Email, Users.Address "
  "FROM "
  " AssignedPatients "
  "  INNER JOIN Users "
  "   ON AssignedPatients.PatientID = Users.UserID "
  "WHERE AssignedPatients.CaregiverID = '{}'",
  caregiver_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto data = net::db_fetch<str, str, str, str, str, str>(connection.connection(), remote::database, query);
 if (std::size(data) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return stl::cvt::move_vector<decltype(data)::value_type, db_objects::patient_info>(data); }
}

std::vector<db_objects::task_info> fetch_caregiver_patient_tasks(webserver_resource *const resource, std::string_view const caregiver_id, std::string_view const patient_id) noexcept {
 static constexpr auto str = net::mysql_field_type::str;
 auto const query = std::format(
  R"(
  SELECT
   Tasks.TaskID, 
   Tasks.DateDue, 
   Tasks.Description
  FROM
   AssignedPatients
    INNER JOIN 
   Tasks
    ON AssignedPatients.PatientID = Tasks.PatientID
  WHERE
   AssignedPatients.CaregiverID = '{}'
    AND
   AssignedPatients.PatientID = '{}'
  )",
  caregiver_id,
  patient_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() == nullptr) [[unlikely]] { return {}; }
 auto data = net::db_fetch<str, str, str>(connection.connection(), remote::database, query);
 if (std::size(data) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return stl::cvt::move_vector<decltype(data)::value_type, db_objects::task_info>(data); }
}

void add_task(webserver_resource *const resource, std::string_view const date_due, std::string_view const description, std::string_view const caregiver_id, std::string_view const patient_id) noexcept { 
 auto const query = std::format(
  R"(
  INSERT INTO Tasks (
   TaskID, 
   DateAdded, 
   DateDue,
   Description,
   CaregiverID, 
   PatientID
  )
  SELECT 
   uuid(), 
   NOW(), 
   '{}',
   '{}',
   '{}',
   '{}' 
  )",
  date_due,
  description,
  caregiver_id,
  patient_id);
 auto const connection = resource->get_remote_connection();
 if (connection.connection() != nullptr) [[likely]] { net::db_exec(connection.connection(), remote::database, query); }
}

webserver::webserver() noexcept : m_server{std::invoke([] () noexcept {
 auto maybe_server = net::http_socket::create_server(net::convert_ipv4_string_to_u32("192.168.88.2"), 80);
 if (maybe_server.status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Thread {}: Failed to Create Server: {}", std::bit_cast<u32>(std::this_thread::get_id()), net::lookup_enum_verbose(maybe_server.status));
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
 std::ranges::for_each(this->m_clients | std::views::filter(std::mem_fn(&net::http_socket::is_valid)), std::mem_fn(&net::http_socket::close));
 // for (auto& client : this->m_clients) {
 //  if (client.socket().socket_handle != 0) {
 //   client.close();
 //  }
 // }
 (void)this->m_server.close();
}

[[nodiscard]] std::span<net::http_socket const> webserver::clients() const noexcept { return this->m_clients; }
[[nodiscard]] std::span<webserver::ddct::wthread_t const> webserver::wthreads() const noexcept { return this->m_threadpool.wthreads(); }

void webserver::accept_incoming_connections() noexcept {
 while (true) {
  auto [status, has_incoming_connection] = this->has_incoming_connection();
  if (status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Thread {}: Failed to Select on Server Socket: {}", std::bit_cast<u32>(std::this_thread::get_id()), net::lookup_enum_verbose(status));
   return;
  }
  if (has_incoming_connection) { this->accept_client(); }
  else { return; }
 } 
}
stl::status_type<net::socket_error_code, bool> webserver::has_incoming_connection() const noexcept {
 static constexpr ::TIMEVAL timeout{ 0, 2000 };
 ::fd_set server_socket { .fd_count = 1 };
 server_socket.fd_array[0] = this->m_server.socket().socket_handle;
 if (::select(NULL, &server_socket, nullptr, nullptr, &timeout) == SOCKET_ERROR) [[unlikely]] { return { static_cast<net::socket_error_code>(::WSAGetLastError()), false }; }
 else { return { net::socket_error_code::success, server_socket.fd_count == 1 }; }
}
void webserver::accept_client() noexcept {
 auto it = std::find_if(std::begin(this->m_clients), std::end(this->m_clients), [&](auto const& client) noexcept { return client.socket().socket_handle == NULL; });
 if (it == std::end(this->m_clients)) [[unlikely]] {
  SPDLOG_ERROR("Thread {}: Clients Full!", std::bit_cast<u32>(std::this_thread::get_id()));
  return;
 }
 auto maybe_client = this->m_server.accept();
 if (maybe_client.status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("Thread {}: Failed to Connect New Client", std::bit_cast<u32>(std::this_thread::get_id()));
  return;
 }
 SPDLOG_INFO("{}Accepted New Client", pid_client_prefix(&maybe_client.value));
 auto const index = it - std::begin(this->m_clients);
 this->m_clients_assigned[index] = false;
 this->m_client_mutices[index].lock();
 *it = std::move(maybe_client.value);
 this->m_client_timestamps[index] = std::chrono::steady_clock::now();
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
   } else {
    mutices[i++] = mutex;
   }
  }
  /* Distribute */
  for (std::size_t mutex_idx = 0; auto const socket_handle : std::span{ client_sockets_copy.fd_array, client_sockets_copy.fd_count }) {
   auto it = std::find_if(std::begin(this->m_clients), std::end(this->m_clients), [&](auto const& client) noexcept { return client.socket().socket_handle == socket_handle; });
   if (it == std::end(this->m_clients)) [[unlikely]] {
    SPDLOG_ERROR("Socket Handle Returned from \"select\" Not Valid");
    for (std::size_t i = mutex_idx; i < client_sockets_copy.fd_count; ++i) { mutices[i]->unlock(); }
    return;
   }

   auto& is_assigned = this->m_clients_assigned[it - std::begin(this->m_clients)];
   if (is_assigned) /* Already Assigned */ {
    mutices[mutex_idx++]->unlock();
    continue;
   }
   is_assigned = true;

   mutices[mutex_idx]->unlock();
   SPDLOG_INFO("Assigned Client ({}:{})", net::convert_ipv4_u32_to_string(it->socket().host), it->socket().port);
   this->m_threadpool.assign(&webserver::handle_client_callable, &*it, std::forward<std::mutex*>(mutices[mutex_idx]));
 
   ++mutex_idx;
  }
  if (client_sockets.fd_count != 64) { return; }
 }
}

/* Assumes: Has mutex lock */
void handle_client_callback(net::http_socket *const client) noexcept {
 auto const client_shutdown_status = client->shutdown().status;
 if (client_shutdown_status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("{}Failed to Shutdown Socket: {}", pid_client_prefix(client), net::lookup_enum_verbose(client_shutdown_status));
 }
 auto const client_close_status = client->close().status;
 if (client_close_status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("{}Failed to Close Socket: {}", pid_client_prefix(client), net::lookup_enum_verbose(client_close_status));
 } else [[likely]] { SPDLOG_INFO("{}Closed Client", pid_client_prefix(client)); }
}
void webserver::handle_client_callable(::webserver_resource* webserver_resource, net::http_socket* client, std::mutex* mtx) noexcept {
 timing::scoped_timer timer([&](auto&& location, auto&& time) { 
   SPDLOG_INFO("{}at {}:{} line {} ran for {}ms",
   pid_client_prefix(client), 
   std::filesystem::path(location.file_name).filename().string(), location.function_name, location.line,
   std::chrono::duration_cast<std::chrono::milliseconds>(time).count()); 
  });
 
 SPDLOG_INFO("{}Started Client Interaction", pid_client_prefix(client));

 mtx->lock();
 auto [request_status, request] = client->receive_request();
 if (request_status != net::socket_error_code::success) [[unlikely]] {
  SPDLOG_ERROR("{}Failed HTTP Request Receival for Socket: {}", pid_client_prefix(client), net::lookup_enum_verbose(request_status));
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 mtx->unlock();
 auto const& resource = request.header().resource;

 if (resource == "/login") {
  auto data = webserver::process_login(webserver_resource, request);
  auto [success, response] = webserver::generate_login_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/login\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/login\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_response = client->send_response(response).status;
  if (send_response != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/medlog") {
  return;
 }
 if (resource == "/shifts") {
  auto data = webserver::process_shifts(webserver_resource, request);
  auto [success, response] = webserver::generate_shifts_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/shifts\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/shifts\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/addtask") {
  [[maybe_unused]] auto const success = webserver::process_addtask(webserver_resource, request);
  if constexpr(LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/addtask\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/addtask\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/gettasks") {
  auto data = webserver::process_gettasks(webserver_resource, request);
  auto [success, response] = webserver::generate_gettasks_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/gettasks\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/gettasks\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port);
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/tasklog") {
  return;
 }
 if (resource == "/medicine") {
  auto data = webserver::process_medicine(webserver_resource, request);
  auto [success, response] = webserver::generate_medicine_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/medicine\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/medicine\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: {}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/prelogin") {
  auto const success = webserver::process_prelogin(webserver_resource, request);
  auto response = webserver::generate_prelogin_response(success).value;
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/prelogin\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/prelogin\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: ", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/shiftlog") {
  return;
 }
 if (resource == "/userinfo") {
  auto data = webserver::process_userinfo(webserver_resource, request);
  auto [success, response] = webserver::generate_userinfo_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/userinfo\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/userinfo\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: {}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 if (resource == "/patientinfos") {
  auto data = webserver::process_patientinfos(webserver_resource, request);
  auto [success, response] = webserver::generate_patientinfos_response(data);
  if constexpr (LOG_API_STATUS) {
   if (success) [[likely]] { SPDLOG_INFO("{}\"/patientinfos\" API Call Success", pid_client_prefix(client)); }
   else [[unlikely]] { SPDLOG_WARN("{}\"/patientinfos\" API Call Failure", pid_client_prefix(client)); }
  }
  mtx->lock();
  auto const send_status = client->send_response(response).status;
  if (send_status != net::socket_error_code::success) [[unlikely]] {
   SPDLOG_ERROR("Client ({}:{}) Failed to Send Response: {}", net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port, net::lookup_enum_verbose(send_status));
   mtx->unlock();
   return;
  }
  handle_client_callback(client);
  mtx->unlock();
  return;
 }
 
 SPDLOG_WARN("{}Unknown Resource Requested (\"{}\")", pid_client_prefix(client), resource);
 mtx->lock();
 handle_client_callback(client);
 mtx->unlock();
}

db_objects::login_user_info                      webserver::process_login(webserver_resource* resource, net::http_request const& request) noexcept {
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

 auto user_info = fetch_user_info(resource, email, passwordhash);
 if (user_info.is_empty()) [[unlikely]] {
  SPDLOG_ERROR("Failed to Fetch User Info");
  return {};
 }

 auto token = std::invoke([&]() noexcept {
  auto token = fetch_session_token(resource, user_info.user_id);
  if (std::size(token) == 0) {
   token = uuids::to_string(uuids::uuid_system_generator{}());
   create_session(resource, token, user_info.user_id);
   return token;
  } else {
   update_session(resource, user_info.user_id);
   return token;
  }
 });

 return db_objects::login_user_info{std::move(user_info), std::move(token)};
}
void                                             webserver::process_medlog(webserver_resource* resource, net::http_request const& request) noexcept {

}
std::vector<db_objects::shift_info>              webserver::process_shifts(webserver_resource* resource, net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(resource, token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 auto shift_infos = fetch_caregiver_shifts(resource, user_id);
 if (std::size(shift_infos) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return shift_infos; }
}
bool                                             webserver::process_addtask(webserver_resource* resource, net::http_request const& request) noexcept {
 auto const [token_status, token] = get_token_from_request(request);
 if (token_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(token_status));
  return false;
 }
 auto const [date_due_status, date_due] = get_date_due_from_request(request);
 if (date_due_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(date_due_status));
  return false;
 }
 auto const [description_status, description] = get_description_from_request(request);
 if (description_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(description_status));
  return false;
 }
 auto const [user_id_status, user_id] = get_userid_from_request(request);
 if (user_id_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(user_id_status));
  return false;
 }
 auto const caregiver_id = fetch_session_userid(resource, token);
 if (std::size(user_id) == NULL) [[unlikely]] { return false; }
 else [[likely]] {
  add_task(resource, date_due, description, caregiver_id, user_id);
  return true;
 }
}
void                                             webserver::process_tasklog(webserver_resource* resource, net::http_request const& request) noexcept {

}
std::vector<db_objects::task_info>               webserver::process_gettasks(webserver_resource* resource, net::http_request const& request) noexcept {
 auto const [token_status, token] = get_token_from_request(request);
 if (token_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(token_status));
  return {};
 }
 auto const [user_id_status, user_id] = get_userid_from_request(request);
 if (user_id_status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(user_id_status));
  return {};
 }
 auto const caregiver_id = fetch_session_userid(resource, token);
 if (std::size(caregiver_id) == NULL) [[unlikely]] { return {}; }
 else [[likely]] { return fetch_caregiver_patient_tasks(resource, caregiver_id, user_id); }
}
std::vector<db_objects::patient_medication_info> webserver::process_medicine(webserver_resource* resource, net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(resource, token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 return fetch_patient_medications(resource, user_id);
}
bool                                             webserver::process_prelogin(webserver_resource* resource, net::http_request const& request) noexcept {
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return false;
 }
 auto const& token = maybe_token.value;
 return std::size(fetch_session_userid(resource, token)) != 0;
}
void                                             webserver::process_shiftlog(webserver_resource* resource, net::http_request const& request) noexcept {

}
db_objects::user_info                            webserver::process_userinfo(webserver_resource* resource, net::http_request const& request) noexcept { 
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(resource, token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 auto user_info = fetch_user_info(resource, user_id);
 if (user_info.is_empty()) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: Failed to Fetch User Info From DB");
  return {};
 }

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
 auto other_user_info = fetch_user_info(resource, other_user_id);
 if (other_user_info.is_empty()) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: Failed to Fetch Other User's Info");
  return {};
 }
 if (other_user_id == user_id) /* You're allowed to request your own info */ { return other_user_info; }

 if (user_info.role == "Patient") [[unlikely]] {
  SPDLOG_ERROR("Client {} trying to request somebody else ({})'s data.", user_id, other_user_info.user_id);
  return {};
 }

 auto const patient_ids = fetch_caregiver_userids(resource, user_id);
 if (std::find(std::cbegin(patient_ids), std::cend(patient_ids), other_user_id) == std::cend(patient_ids)) [[unlikely]] {
  SPDLOG_ERROR("Caretaker {} trying to request user {}'s data, for whom they are not currently a caretaker.", user_id, other_user_id);
  return {};
 }
 auto const requested_info = fetch_user_info(resource, other_user_id);
 if (requested_info.is_empty()) [[unlikely]] {
  SPDLOG_ERROR("Caretaker {} trying to request non-existend user {}'s data.", user_id, other_user_id);
  return {};
 }
 
 return requested_info;
}
std::vector<db_objects::patient_info>            webserver::process_patientinfos(webserver_resource* resource, net::http_request const& request) noexcept {
 auto const maybe_token = get_token_from_request(request);
 if (maybe_token.status != request_status::success) [[unlikely]] {
  SPDLOG_ERROR("Invalid Request: {}", ::lookup_enum(maybe_token.status));
  return {};
 }
 auto const& token = maybe_token.value;
 auto const user_id = fetch_session_userid(resource, token);
 if (std::size(user_id) == NULL) [[unlikely]] {
  SPDLOG_ERROR("Invalid Session Token: {}", token);
  return {};
 }

 return fetch_patient_infos(resource, user_id);;
}

stl::status_type<bool, nlohmann::json> webserver::generate_login_response(db_objects::login_user_info& data) noexcept {
 nlohmann::json response;
 if (data.is_empty()) [[unlikely]] {
  return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{
   { "UserID" , nullptr },
   { "Role"   , nullptr },
   { "Name"   , nullptr },
   { "Surname", nullptr },
   { "PhoneNo", nullptr },
   { "Email"  , nullptr },
   { "Address", nullptr },
   { "Token"  , nullptr },
  } };
 } else [[likely]] {
  return stl::status_type<bool, nlohmann::json>{ true, nlohmann::json{
   { "UserID" , std::move(data.user_id) },
   { "Role"   , std::move(data.role) },
   { "Name"   , std::move(data.name) },
   { "Surname", std::move(data.surname) },
   { "PhoneNo", std::move(data.phone) },
   { "Email"  , std::move(data.email) },
   { "Address", std::move(data.address) },
   { "Token"  , std::move(data.token) },
  } };
 }
}
stl::status_type<bool, nlohmann::json> webserver::generate_shifts_response(std::vector<db_objects::shift_info>& data) noexcept {
 if (std::size(data) == NULL) [[unlikely]] { return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{ { "data", nullptr } } }; } 
 else [[likely]] {
  nlohmann::json response;
  for (auto&& entry : data) {
   response["data"].push_back(nlohmann::json({
    { "Name"      , std::move(entry.name)        },
    { "Surname"   , std::move(entry.surname)     },
    { "Address"   , std::move(entry.address)     },
    { "ShiftStart", std::move(entry.shift_start) },
    { "ShiftEnd"  , std::move(entry.shift_end)   },
   }));
  }
  return stl::status_type<bool, nlohmann::json>{ true, std::move(response) };
 }
}
stl::status_type<bool, nlohmann::json> webserver::generate_gettasks_response(std::vector<db_objects::task_info>& data) noexcept {
 if (std::size(data) == NULL) [[unlikely]] { return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{ { "data", nullptr } } }; }
 else [[likely]] {
  nlohmann::json response;
  for (auto&& entry : data) {
   response["data"].push_back({
    { "TaskID"     , std::move(entry.task_id)     },
    { "DateDue"    , std::move(entry.date_due)    },
    { "Description", std::move(entry.description) },
   });
  }
  return stl::status_type<bool, nlohmann::json>{ true, std::move(response) };
 }
}
stl::status_type<bool, nlohmann::json> webserver::generate_medicine_response(std::vector<db_objects::patient_medication_info>& data) noexcept {
 if (std::size(data) == NULL) [[unlikely]] { return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{ { "data", nullptr } } }; }
 else [[likely]] {
  nlohmann::json response;
  for (auto&& entry : data) {
   auto& user_data = response["data"][std::move(entry.user_id)];
   if (user_data.find("PatientInfo") == std::end(user_data)) [[unlikely]] {
    user_data["PatientInfo"] = {
     { "PatientName", std::move(entry.user_name) },
     { "PatientSurname", std::move(entry.user_surname) }
    };
   }
   user_data["Medication"].push_back({
    { "Day",            std::move(entry.day) },
    { "Frequency",      std::move(entry.frequency) },
    { "StartDate",      std::move(entry.start_date) },
    { "EndDate",        std::move(entry.end_date) },
    { "MedicationName", std::move(entry.name) },
    { "Description",    std::move(entry.description) },
    { "Interactions",   std::move(entry.interactions) },
    { "Dosage",         std::move(entry.dosage) }
   });
  }
  return stl::status_type<bool, nlohmann::json>{ true, std::move(response) };
 }
}
stl::status_type<bool, nlohmann::json> webserver::generate_prelogin_response(bool const success) noexcept {
 if (success) [[likely]] { return stl::status_type<bool, nlohmann::json>{ true, nlohmann::json::parse("{\"Success\":true}") }; }
 else [[unlikely]] { return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json::parse("{\"Success\":false}") }; }
}
stl::status_type<bool, nlohmann::json> webserver::generate_userinfo_response(db_objects::user_info& data) noexcept {
 if (data.is_empty()) [[unlikely]] {
  return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{
   { "UserID" , nullptr },
   { "Role"   , nullptr },
   { "Name"   , nullptr },
   { "Surname", nullptr },
   { "PhoneNo", nullptr },
   { "Email"  , nullptr },
   { "Address", nullptr },
  } };
 } else [[likely]] {
  return stl::status_type<bool, nlohmann::json>{ true, nlohmann::json{
   { "UserID" , std::move(data.user_id) },
   { "Role"   , std::move(data.role) },
   { "Name"   , std::move(data.name) },
   { "Surname", std::move(data.surname) },
   { "PhoneNo", std::move(data.phone) },
   { "Email"  , std::move(data.email) },
   { "Address", std::move(data.address) },
  } };
 }
}
stl::status_type<bool, nlohmann::json> webserver::generate_patientinfos_response(std::vector<db_objects::patient_info>& data) noexcept {
 if (std::size(data) == NULL) [[unlikely]] { return stl::status_type<bool, nlohmann::json>{ false, nlohmann::json{ { "data", nullptr } } }; }
 else [[likely]] {
  nlohmann::json response;
  for (auto&& entry : data) {
   response["data"].push_back({
    { "UserID",  std::move(entry.user_id) },
    { "Name",    std::move(entry.name)    },
    { "Surname", std::move(entry.surname) },
    { "Phone",   std::move(entry.phone)   },
    { "Email",   std::move(entry.email)   },
    { "Address", std::move(entry.address) },
   });
  }
  return stl::status_type<bool, nlohmann::json>{ true, std::move(response) };
 }
}


