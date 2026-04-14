from pathlib import Path

path = Path('rename_file')
files = [file.name for file in path.rglob("*.*")]
for file in files:
    print(file)