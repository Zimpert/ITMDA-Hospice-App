#pragma once

#include <tuple>
#include <string>

namespace db_objects {
 struct user_info {
  std::string user_id;
  std::string role;
  std::string name;
  std::string surname;
  std::string phone;
  std::string email;
  std::string address;
  
  user_info() noexcept = default;
  user_info(std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string>&& args) noexcept : 
   user_id{std::get<0>(args)}, 
   role{std::get<1>(args)}, 
   name{std::get<2>(args)}, 
   surname{std::get<3>(args)}, 
   phone{std::get<4>(args)}, 
   email{std::get<5>(args)}, 
   address{std::get<6>(args)}
  {

  }

  [[nodiscard]] bool is_empty() const noexcept { return std::size(user_id) == 0; }
 };
}