#pragma once

#include "user_info.hpp"
#include <tuple>
#include <string>

namespace db_objects {
 struct patient_medication_info {
  using args_t = std::tuple<std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string, std::string>;

  std::string user_id;
  std::string user_name;
  std::string user_surname;
  std::string day;
  std::string frequency;
  std::string start_date;
  std::string end_date;
  std::string name;
  std::string description;
  std::string interactions;
  std::string dosage;
  
  patient_medication_info() noexcept = default;
  patient_medication_info(args_t&& args) noexcept : 
   user_id     {std::get< 0>(args)},
   user_name   {std::get< 1>(args)},
   user_surname{std::get< 2>(args)},
   day         {std::get< 3>(args)},
   frequency   {std::get< 4>(args)},
   start_date  {std::get< 5>(args)},
   end_date    {std::get< 6>(args)},
   name        {std::get< 7>(args)},
   description {std::get< 8>(args)},
   interactions{std::get< 9>(args)},
   dosage      {std::get<10>(args)}
  { }

  [[nodiscard]] bool is_empty() const noexcept { return std::size(user_id) == 0; }
 };
}