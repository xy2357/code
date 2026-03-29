books = {
    "Python基础": 3,
    "算法入门": 2
}
while True:
    print("1.查看所有图书\n2.添加图书\n3.查询某本书库存\n4.借书\n5.还书\n6.退出")
    index = input("请输入操作：")

    if index == "1":
        if len(books) != 0:
            for book, num in books.items():
                print(f"{book}:{num}")
        else:
            print("图书馆为空")

    if index == "2":
        name = input("请输入图书名字：")
        if name in books:
            books[name] += 1
        else:
            books[name] = 1

    if index == "3":
        name = input("请输入图书名字：")
        if name in books:
            print(f"{name}的库存为{books[name]}")
        else:
            print("查无此书")

    if index == "4":
        name = input("请输入图书名字：")
        if name not in books:
            print("查无此书")
        elif books[name] == 0:
            print("库存不足")
        else:
            books[name] -= 1

    if index == "5":
        name = input("请输入图书名字：")
        if name not in books:
            print("查无此书")
        else:
            books[name] += 1

    if index == "6":
        break
