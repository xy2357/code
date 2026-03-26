menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
while menu:
    if menu == 5:
        break
    elif menu not in [1, 2, 3, 4, 5]:
        print("无效选项")
        menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
    else:
        num1 = int(input("请输入第一个数字："))
        num2 = int(input("请输入第二个数字："))
        if menu == 1:
            print(f"{num1 + num2}")
            menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
        elif menu == 2:
            print(f"{num1 - num2}")
            menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
        elif menu == 3:
            print(f"{num1 * num2}")
            menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
        elif menu == 4:
            print(f"{num1 / num2}")
            menu = int(input("1.加分\n2.减法\n3.乘法\n4.除法\n5.退出\n:"))
