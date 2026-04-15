import  os
from pathlib import Path
import re

# path = Path('rename_file')
# files = [file.name for file in path.rglob("*.*")]
# for file in files:
#     print(file)
# os.chdir('E:\python')
# print(os.listdir())
# print(os.stat('countstr.py').st_mtime)
# mod_time =os.stat('countstr.py').st_mtime
# print(datetime.fromtimestamp(mod_time))
# print(datetime.fromisocalendar(2026, 4, 5))
# # print(datetime.fromisoformat('2026-04-14'))
# for name1, name2, name3 in os.walk('.'):
#     print(name1)
#     print(name2)
#     print(name3)
# print(os.environ.get('HOME'))
# file_path = os.path.join('.', 'text1.txt')
# mkdir(file_path)
# print(os.path.splitext('E:/python/python/Learning/exercise/level11/renamefile.py '))

# file_path = Path('rename_file')
# for file in file_path.iterdir():
#     if not file.is_file():
#         continue
#
#     m = re.search(r'\d+', file.stem)
#     if not m:
#         continue
#
#     num = int(m.group())
#     new_name = f"photo_{num:03d}{file.suffix}"
#     new_path = file.with_name(new_name)
#
#     file.rename(new_path)
#     print(f"{file.name} -> {new_name}")
def rename_files(files, prefix):
    new_files = []

    for i, file in enumerate(files, start=1):
        dot_index = file.rfind(".")
        if dot_index == -1:
            extension = ""
        else:
            extension = file[dot_index:]

        new_name = f"{prefix}_{i:03d}{extension}"
        new_files.append(new_name)

    return new_files

files = ["img1.png", "img2.png", "img3.png"]
print(rename_files(files, "photo"))