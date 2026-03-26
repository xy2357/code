answer = int(input("请先预设一个数字："))
num = int(input("请输入你猜的数字："))
while num != answer:
    if num < answer:
        print("猜小了")
    elif num > answer:
        print("猜大了")
    num = int(input("请输入你猜的数字："))
print("恭喜猜对了")
