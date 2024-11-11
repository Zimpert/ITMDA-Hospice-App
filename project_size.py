import os

file_types = "hpp", "cpp"

total_line_count = 0
total_char_count = 0
total_file_count = 0
for directory, _, file_names in os.walk(os.getcwd()):
 directory = directory.replace('\\', '/')
 if "/build/" in directory: continue
 for file_name in file_names:
  if sum(file_name.endswith(file_type) for file_type in file_types) == 0: continue
  file_path = f"{directory}/{file_name}"
  with open(file_path, "r") as file_handler:
   file_contents = file_handler.read()
   line_count = file_contents.count('\n')
   char_count = len(file_contents)
   total_line_count += line_count
   total_char_count += char_count
   total_file_count += 1
   print(f"{file_path}: {line_count} lines, {char_count} chars")
print(f"Total Line Count: {total_line_count}")
print(f"Total Char Count: {total_char_count}")
print(f"Total File Count: {total_file_count}")