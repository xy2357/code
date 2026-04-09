import json
from pathlib import Path

file_path = Path("students.json")

def load_students():
    if not file_path.exists():
        return []
    try:
        with open(file_path, 'r', encoding='utf_8') as file:
            students_data = json.load(file)
        if isinstance(students_data,list):
            return students_data
        else:
            return []
    except json.JSONDecodeError:
        print("文件内容损坏，已重置为空数据！")
        return []

def save_students(students):
    with open(file_path, 'w', encoding='utf-8') as file:
        json.dump(students, file, ensure_ascii=False, indent=2)

def show_all_student(students):
    if len(students) != 0:
        for student in students:
            print(f"{student['name']}的年龄是{student['age']},分数是{student['score']}")
    else:
        print("暂无数据！")

def add_student(students):
    found = False
    name = input("请输入姓名：")
    for student in students:
        if name == student['name']:
            print("学生已存在！")
            found = True
            break
    if not found:
        age = input("请输入年龄：")
        try:
            age = int(age)
        except ValueError:
            print("请输入整数！")
            return

        score = input("请输入分数：")
        try:
            score = int(score)
        except ValueError:
            print("请输入整数！")
            return

        students.append({'name':name, 'age':age, 'score':score})
        save_students(students)

def find_student(students):
    found = False
    if len(students) != 0:
        name = input("请输入姓名：")
        for student in students:
            if name == student['name']:
                print(f"{name}的年龄是{student['age']},分数是{student['score']}")
                found = True
                break
        if not found:
            print("未找到学生！")
    else:
        print("暂无数据！")

def update_student(students):
    found = False
    name = input("请输入姓名：")
    for student in students:
        if name == student['name']:
            score = input("请输入最新分数：")

            try:
                score = int(score)
            except ValueError:
                print("请输入整数！")
                return

            student['score'] = score
            save_students(students)
            found = True
            break
    if not found:
        print("未找到学生！")

def delete_student(students):
    found = False
    name = input("请输入姓名：")
    for student in students:
        if name == student['name']:
            students.remove(student)
            save_students(students)
            found = True
            break
    if not found:
        print("未找到学生！")

def average_score(students):
    total_score = 0
    if len(students) != 0:
        for student in students:
            total_score += student['score']
        average_score = total_score / len(students)

        return print(f"平均分为{average_score}")
    else:
        print("暂无数据！")
        return None

def find_max_score(students):
    max_score = 0
    max_student = ''
    if len(students) != 0:
        for student in students:
            if student['score'] > max_score:
                max_student = student['name']
                max_score = student['score']
        return print(f"{max_student}分数最高，为{max_score}")
    else:
        return print("暂无数据！")

def sort_students(students):
    for i in range(len(students) - 1):
        for j in range(len(students) - i - 1):
            if students[j]['score'] < students[j+1]['score']:
                temp = students[j]
                students[j] = students[j+1]
                students[j + 1] = temp
    save_students(students)
    return students

def main():

    students = load_students()
    save_students(students)

    while True:
        print("\n1.查看所有学生\n2.添加学生\n3.查询学生\n4.修改学生信息\n5.删除学生\n6.统计平均分\n7.查看最高分学生\n8.按分数排序\n9.退出")
        choice = input("请输入操作：")

        if choice == "1":
            show_all_student(students)
        elif choice == "2":
            add_student(students)
        elif choice == "3":
            find_student(students)
        elif choice == "4":
            update_student(students)
        elif choice == "5":
            delete_student(students)
        elif choice == "6":
            average_score(students)
        elif choice == '7':
            find_max_score(students)
        elif choice == '8':
            sort_students(students)
            show_all_student(students)
        elif choice == '9':
            save_students(students)
            break
        else:
            print("请输入有效操作！")

main()