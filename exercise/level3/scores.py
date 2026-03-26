scores = {
    "小明": 85,
    "小红": 92,
    "小刚": 78
}
while True:
    name = input("请输入名字：")
    if name in scores:
        print(f"{name}的成绩为{scores[name]}")
    else:
        print("查无此人")