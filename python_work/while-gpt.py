while True:
    age = input("请输入您的年龄（输入'q'退出）：")

    if age.lower() == 'q':
        print("感谢使用！")
        break

    age = int(age)

    if age < 3:
        print("您的票价为：免费")
    elif age <= 12:
        print("您的票价为：$10")
    else:
        print("您的票价为：$15")
