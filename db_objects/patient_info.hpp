#pragma once

#include "user_info.hpp"
#include <tuple>
#include <string>

namespace db_objects {
 struct patient_info {
  using args_t = std::tuple<std::string, std::string, std::string, std::string, std::string, std::string>;

  std::string user_id;
  std::string name;
  std::string surname;
  std::string phone;
  std::string email;
  std::string address;
  
  patient_info() noexcept = default;
  patient_info(args_t&& args) noexcept : 
   user_id{std::get<0>(args)},
   name   {std::get<1>(args)},
   surname{std::get<2>(args)},
   phone  {std::get<3>(args)},
   email  {std::get<4>(args)},
   address{std::get<5>(args)}
  { }

  [[nodiscard]] bool is_empty() const noexcept { return std::size(user_id) == 0; }
 };
}