import json
from pathlib import Path

from level6.todo import title

FILE_PATH = Path("todo.json")

def load_todo():
    if not FILE_PATH.exists():
        return []

    try:
        with open(FILE_PATH, 'r', encoding='utf-8') as file:
            data = json.load(file)
        if isinstance(data, list):
            return data
        print("文件格式错误！")
        return []
    except json.JSONDecodeError:
        print("文件已损坏！")
        return []

def save_todo(todo):
    with open(FILE_PATH, 'w', encoding='utf-8') as file:
        json.dump(todo, file, ensure_ascii=False, indent=2)

def input_int(prompt, todo):
    """安全读取整数。"""
    while True:
        text = input(prompt).strip()
        try:
            if int(text) >= len(todo):
                print("序号越界！")
            return int(text) + 1
        except ValueError:
            print("请输入整数！")

def show_all_event(todo):
    if len(todo) == 0:
        print("暂无数据！")

    for event in todo:
        print(f"{event['title']} - {event['done']}")

def show_not_done(todo):
    if len(todo) == 0:
        print("暂无数据！")

    for event in todo:
        if not event['done']:
            print(f"{event['title']}")

def add_event(todo):
    found = False
    event_title = input("请输入名称：")
    for event in todo:
        if event_title == event['name']:
            print("事项已存在！")
            found = True
            break
    if not found:
        todo.append({'title':event_title, 'done':False})
        save_todo(todo)

def delete_event(todo):
    event_index = input_int("请输入事项序号：", todo)
    del todo[event_index]

def find_event(todo):
    event_index = input_int("请输入事项序号：", todo)
    print(f"{todo[event_index]['title']} - {todo[event_index]['done']}")