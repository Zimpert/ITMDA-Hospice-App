#pragma once

#include <tuple>
#include <string>

namespace db_objects {
 struct shift_info {
  using args_t = std::tuple<std::string, std::string, std::string, std::string, std::string>;

  std::string name;
  std::string surname;
  std::string address;
  std::string shift_start;
  std::string shift_end;
  
  shift_info() noexcept = default;
  shift_info(args_t&& args) noexcept : 
   name       {std::get<0>(args)},
   surname    {std::get<1>(args)},
   address    {std::get<2>(args)},
   shift_start{std::get<3>(args)},
   shift_end  {std::get<4>(args)}
   {}

  [[nodiscard]] bool is_empty() const noexcept { return std::size(name) == 0; }
 };
}