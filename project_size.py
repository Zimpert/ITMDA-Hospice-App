import os

file_paths = "entrypoint.cpp", "webserver.cpp", "webserver.hpp", "request_status.hpp"

total_line_count = 0
total_char_count = 0
total_file_count = len(file_paths)
for file_path in file_paths:
 with open(file_path, "r") as file_handler:
  file_contents = file_handler.read()
  line_count = file_contents.count('\n')
  char_count = len(file_contents)
  total_line_count += line_count
  total_char_count += char_count
  print(f"{file_path}: {line_count} lines, {char_count} chars")
print(f"Total Line Count: {total_line_count}")
print(f"Total Char Count: {total_char_count}")
print(f"Total File Count: {total_file_count}")