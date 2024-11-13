#pragma once

#include <tuple>
#include <string>

namespace db_objects {
 struct shift_info {
  using args_t = std::tuple<std::string, std::string, std::string, std::string, std::string, std::string>;

  std::string shift_id;
  std::string name;
  std::string surname;
  std::string address;
  std::string shift_start;
  std::string shift_end;
  
  shift_info() noexcept = default;
  shift_info(args_t&& args) noexcept : 
   shift_id   {std::get<0>(args)},
   name       {std::get<1>(args)},
   surname    {std::get<2>(args)},
   address    {std::get<3>(args)},
   shift_start{std::get<4>(args)},
   shift_end  {std::get<5>(args)}
   {}

  [[nodiscard]] bool is_empty() const noexcept { return std::size(shift_id) == 0; }
 };
}