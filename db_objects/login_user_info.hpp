#pragma once

#include "user_info.hpp"
#include <tuple>
#include <string>

namespace db_objects {
 struct login_user_info {
  std::string user_id;
  std::string role;
  std::string name;
  std::string surname;
  std::string phone;
  std::string email;
  std::string address;
  std::string token;
  
  login_user_info() noexcept = default;
  login_user_info(user_info&& args, std::string&& _token) noexcept : 
   user_id{args.user_id},
   role{args.role},
   name{args.name},
   surname{args.surname},
   phone{args.phone},
   email{args.email},
   address{args.address},
   token{_token}
  { }

  [[nodiscard]] bool is_empty() const noexcept { return std::size(user_id) == 0; }
 };
}