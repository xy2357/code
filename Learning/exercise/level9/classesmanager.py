classes = []
while True:
    found = False
    max_student = 0
    max_class = ""
    print("1.查看所有课程\n2.添加课程\n3.查询课程\n4.修改任课老师\n5.修改选课人数\n6.删除课程\n7.查看学生最多的课程\n8.退出")
    index = input("请输入：")

    if index == "1":
        if len(classes) != 0:
            for each in classes:
                print(f"{each['name']}的老师为{each['teacher']}，学生人数为{each['students']}")
        else:
            print("课程为空！")

    elif index == "2":
        name = input("请输入课程名称：")
        if len(classes) != 0:
            for each in classes:
                if name == each['name']:
                    print("课程已存在！")
                    found = True
                    break
            if not found:
                teacher = input("请输入任课老师：")
                students = int(input("请输入学生数："))
                one_class = {"name": name, "teacher": teacher, "students": students}
                classes.append(one_class)

        else:
            teacher = input("请输入任课老师：")
            students = int(input("请输入学生数："))
            one_class = {"name": name, "teacher": teacher, "students": students}
            classes.append(one_class)

    elif index == "3":
        name = input("请输入课程名称：")
        for each in classes:
            if name == each['name']:
                print(f"{each['name']}的老师为{each['teacher']}，学生人数为{each['students']}")
                found = True
                break
        if not found:
            print("课程不存在！")

    elif index == "4":
        name = input("请输入课程名称：")
        for each in classes:
            if name == each['name']:
                teacher = input("请输入新老师的姓名：")
                each['teacher'] = teacher
                found = True
                break
        if not found:
            print("课程不存在！")

    elif index == "5":
        name = input("请输入课程名称：")
        for each in classes:
            if name == each['name']:
                students = int(input("请输入新人数："))
                each['students'] = students
                found = True
                break
        if not found:
            print("课程不存在！")

    elif index == "6":
        name = input("请输入课程名称：")
        for each in classes:
            if name == each['name']:
                classes.remove(each)
                found = True
                break
        if not found:
            print("课程不存在！")

    elif index == "7":
        for each in classes:
            if each['students'] > max_student:
                max_student = each['students']
                max_class = each['name']
        print(f"{max_class}人数最多，为{max_student}")

    elif index == "8":
        break

    else:
        print("请输入有效操作！")