#pragma once

#include "types.hpp"
#include <array>
#include <tuple>
#include <string_view>

enum class request_status : u08 {
 success,
 token_not_found,
 token_not_string,
 token_not_uuid,
 token_not_in_db,
 token_expired,
 userid_not_found,
 userid_not_string,
 userid_not_uuid,
 email_not_found,
 email_not_string,
 email_not_email,
 passwordhash_not_found,
 passwordhash_not_string,
 passwordhash_not_hash,
 date_due_not_found,
 date_due_not_string,
 date_due_not_date,
 description_not_found,
 description_not_string,
};

inline constexpr std::array<std::pair<request_status, std::string_view>, 20> REQUEST_STATUS_LOOKUP {
 std::pair<request_status, std::string_view>{ request_status::success,                 "request_status::success"                 },
 std::pair<request_status, std::string_view>{ request_status::token_not_found,         "request_status::token_not_found"         },
 std::pair<request_status, std::string_view>{ request_status::token_not_string,        "request_status::token_not_string"        },
 std::pair<request_status, std::string_view>{ request_status::token_not_uuid,          "request_status::token_not_uuid"          },
 std::pair<request_status, std::string_view>{ request_status::token_not_in_db,         "request_status::token_not_in_db"         },
 std::pair<request_status, std::string_view>{ request_status::token_expired,           "request_status::token_expired"           },
 std::pair<request_status, std::string_view>{ request_status::userid_not_found,        "request_status::userid_not_found"        },
 std::pair<request_status, std::string_view>{ request_status::userid_not_string,       "request_status::userid_not_string"       },
 std::pair<request_status, std::string_view>{ request_status::userid_not_uuid,         "request_status::userid_not_uuid"         },
 std::pair<request_status, std::string_view>{ request_status::email_not_found,         "request_status::email_not_found"         },
 std::pair<request_status, std::string_view>{ request_status::email_not_string,        "request_status::email_not_string"        },
 std::pair<request_status, std::string_view>{ request_status::email_not_email,         "request_status::email_not_email"         },
 std::pair<request_status, std::string_view>{ request_status::passwordhash_not_found,  "request_status::passwordhash_not_found"  },
 std::pair<request_status, std::string_view>{ request_status::passwordhash_not_string, "request_status::passwordhash_not_string" },
 std::pair<request_status, std::string_view>{ request_status::passwordhash_not_hash,   "request_status::passwordhash_not_hash"   },
 std::pair<request_status, std::string_view>{ request_status::date_due_not_found,      "request_status::date_due_not_found"      },
 std::pair<request_status, std::string_view>{ request_status::date_due_not_string,     "request_status::date_due_not_string"     },
 std::pair<request_status, std::string_view>{ request_status::date_due_not_date,       "request_status::date_due_not_date"       },
 std::pair<request_status, std::string_view>{ request_status::description_not_found,   "request_status::description_not_found"   },
 std::pair<request_status, std::string_view>{ request_status::description_not_string,  "request_status::description_not_string"  },
};

inline constexpr std::string_view lookup_enum(request_status const request_status) noexcept {
 auto const it = std::find_if(std::cbegin(REQUEST_STATUS_LOOKUP), std::cend(REQUEST_STATUS_LOOKUP), [&](auto const entry) noexcept { return entry.first == request_status; });
 if (it == std::cend(REQUEST_STATUS_LOOKUP)) [[unlikely]] {
  return "request_status::invalid_enum";
 } else [[likely]] {
  return it->second;
 }
}