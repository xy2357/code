from pathlib import Path
# import re
#
# file_path = Path('log.txt')
# error_list = []
# count = 0
#
# with open(file_path, 'r', encoding='utf-8') as file:
#     logs = file.readlines()
#
# for log in logs:
#     m = re.search(r'error', log, re.I)
#
#     if m:
#         error_list.append(log)
#         count += 1
#
# with open('error_log.txt', 'w', encoding='utf-8') as file:
#     for log in error_list:
#         file.write(log)
# print(f"一共有{count}条error")

file_path = Path("log.txt")
count = 0

with open(file_path, 'r', encoding='utf-8') as infile, open('error.txt', 'w', encoding='utf-8') as outfile:
    for line in infile:
        if "error" in line.lower():
            outfile.write(line)
            count += 1

print(f"一共有{count}条error")