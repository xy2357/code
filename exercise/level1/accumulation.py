num = int(input("请输入一个整数："))
total = 0
for i in range(1, num + 1):
    total += i
print(f"1到{num}的和为{total}")