todo_list = []
while True:
    print("1.查看所有待办事项\n2.添加待办事项\n3.删除待办事项\n4.标记待办事项为完成\n5.退出")
    index = input("请输入操作：")

    if index == "1":
        if len(todo_list) == 0:
            print("暂无待办事项")
        else:
            for i, each in enumerate(todo_list, start=1):
                status = "已完成" if each["done"] else "未完成"
                print(f"{i}. {each['title']} - {status}")

    if index == "2":
        title = input("请输入事项名称：")
        done = False
        todo_list.append({"title":title, "done":done})

    if index == "3":
        num = int(input("请输入序号："))
        if num < 1 or num > len(todo_list):
            print("序号不存在！")
        else:
            del todo_list[num - 1]

    if index == "4":
        num = int(input("请输入序号："))
        if num < 1 or num > len(todo_list):
            print("序号不存在！")
        else:
            todo_list[num - 1]["done"] = True

    if index == "5":
        break

