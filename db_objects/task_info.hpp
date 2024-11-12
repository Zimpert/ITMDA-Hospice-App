#pragma once

#include <tuple>
#include <string>

namespace db_objects {
 struct task_info {
  using args_t = std::tuple<std::string, std::string, std::string>;

  std::string task_id;
  std::string date_due;
  std::string description;
  
  task_info() noexcept = default;
  task_info(args_t&& args) noexcept : 
   task_id{ std::get<0>(args) },
   date_due{ std::get<1>(args) },
   description{ std::get<2>(args) }
  { }

  [[nodiscard]] bool is_empty() const noexcept { return std::size(task_id) == 0; }
 };
}