str1 = input("请输入一个字符串：")
str2 = input("请输入一个字符：")
count = 0
for i in str1:
    if str2 == i:
        count += 1
print(f"字符{str2}一共出现了{count}次")
