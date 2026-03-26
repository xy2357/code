str_org = input("请输入一个字符串：")
str_res = ""
org = []
res = []
for each in str_org:
    org.append(each)
print(org)

for i in range(0, len(org)):
    print(i)
    res.append(org.pop())
print(res)

for each in res:
    str_res += each

if str_org == str_res:
    print(f"{str_org}是回文")
else:
    print("不是回文")