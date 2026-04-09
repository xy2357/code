import json
from pathlib import Path

FILE_PATH = Path("students.json")


def load_students():
    """读取学生数据，保证返回列表。"""
    if not FILE_PATH.exists():
        return []

    try:
        with FILE_PATH.open("r", encoding="utf-8") as file:
            data = json.load(file)
        if isinstance(data, list):
            return data
        print("students.json 格式错误，已重置为空数据。")
        return []
    except json.JSONDecodeError:
        print("students.json 内容损坏，已重置为空数据。")
        return []


def save_students(students):
    """保存学生数据到 JSON 文件。"""
    with FILE_PATH.open("w", encoding="utf-8") as file:
        json.dump(students, file, ensure_ascii=False, indent=2)


def input_int(prompt):
    """安全读取整数。"""
    while True:
        text = input(prompt).strip()
        try:
            return int(text)
        except ValueError:
            print("请输入整数！")


def find_student_index(students, name):
    """按姓名查找学生下标，找不到返回 -1。"""
    for i, student in enumerate(students):
        if student["name"] == name:
            return i
    return -1


def show_all_students(students):
    if not students:
        print("暂无数据！")
        return

    for student in students:
        print(
            f"{student['name']} 的年龄是 {student['age']}，分数是 {student['score']}"
        )


def add_student(students):
    name = input("请输入姓名：").strip()
    if not name:
        print("姓名不能为空！")
        return

    if find_student_index(students, name) != -1:
        print("学生已存在！")
        return

    age = input_int("请输入年龄：")
    score = input_int("请输入分数：")

    students.append({
        "name": name,
        "age": age,
        "score": score
    })
    save_students(students)
    print("添加成功。")


def find_student(students):
    if not students:
        print("暂无数据！")
        return

    name = input("请输入姓名：").strip()
    index = find_student_index(students, name)

    if index == -1:
        print("未找到学生！")
        return

    student = students[index]
    print(f"{student['name']} 的年龄是 {student['age']}，分数是 {student['score']}")


def update_student(students):
    if not students:
        print("暂无数据！")
        return

    name = input("请输入姓名：").strip()
    index = find_student_index(students, name)

    if index == -1:
        print("未找到学生！")
        return

    print("1. 修改年龄")
    print("2. 修改分数")
    choice = input("请选择要修改的字段：").strip()

    if choice == "1":
        students[index]["age"] = input_int("请输入最新年龄：")
        save_students(students)
        print("年龄修改成功。")
    elif choice == "2":
        students[index]["score"] = input_int("请输入最新分数：")
        save_students(students)
        print("分数修改成功。")
    else:
        print("请输入有效操作！")


def delete_student(students):
    if not students:
        print("暂无数据！")
        return

    name = input("请输入姓名：").strip()
    index = find_student_index(students, name)

    if index == -1:
        print("未找到学生！")
        return

    del students[index]
    save_students(students)
    print("删除成功。")


def average_score(students):
    if not students:
        print("暂无数据！")
        return

    total = 0
    for student in students:
        total += student["score"]

    avg = total / len(students)
    print(f"平均分为 {avg}")


def find_max_score(students):
    if not students:
        print("暂无数据！")
        return

    max_student = students[0]
    for student in students[1:]:
        if student["score"] > max_student["score"]:
            max_student = student

    print(f"最高分学生是 {max_student['name']}，分数为 {max_student['score']}")


def sort_students(students):
    if not students:
        print("暂无数据！")
        return

    for i in range(len(students) - 1):
        for j in range(len(students) - 1 - i):
            if students[j]["score"] < students[j + 1]["score"]:
                students[j], students[j + 1] = students[j + 1], students[j]

    save_students(students)
    print("已按分数从高到低排序。")
    show_all_students(students)


def main():
    students = load_students()
    save_students(students)

    while True:
        print("\n1. 查看所有学生")
        print("2. 添加学生")
        print("3. 查询学生")
        print("4. 修改学生信息")
        print("5. 删除学生")
        print("6. 统计平均分")
        print("7. 查看最高分学生")
        print("8. 按分数排序")
        print("9. 退出")

        choice = input("请输入操作：").strip()

        if choice == "1":
            show_all_students(students)
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
        elif choice == "7":
            find_max_score(students)
        elif choice == "8":
            sort_students(students)
        elif choice == "9":
            save_students(students)
            print("程序已退出。")
            break
        else:
            print("请输入有效操作！")


if __name__ == "__main__":
    main()