# 创建字典并存储人员及其喜欢的地方列表
people = {
    "Alice": ["公园", "海滩"],
    "Bob": ["山脉"],
    "Charlie": ["博物馆", "咖啡厅", "电影院"]
}

# 遍历字典并打印每个人的名字和喜欢的地方

for person, places in people.items():
    print(f"{person} 喜欢的地方:")
    for place in places:
        print(place)
