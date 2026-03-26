address = {"李三": "111111", "王五": "222222", "张四": "333333"}
name = input("请输入一个名字：")
if name in address.keys():
    print(f"{name}的手机号是：{address[name]}")
else:
    print("查无此人")