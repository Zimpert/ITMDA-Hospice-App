#pragma once

#include <tuple>
#include <string>

namespace db_objects {
 struct minimal_user_info {
  std::string name;
  std::string surname;
  std::string address;
  
  minimal_user_info() noexcept = default;
  minimal_user_info(std::tuple<std::string, std::string, std::string>&& args) noexcept : name{std::get<0>(args)}, surname{std::get<1>(args)}, address{std::get<2>(args)} {}

  [[nodiscard]] bool is_empty() const noexcept { return std::size(name) == 0; }
 };
}