num = int(input("请输入一个数字："))
if 90 <= num <= 100:
    print("A")
elif 80 <= num < 90:
    print("B")
elif 70 <= num < 80:
    print("C")
elif 60 <= num < 70:
    print("D")
elif 0 <= num < 60:
    print("E")
else:
    print("请输入0~100之间的数字！")