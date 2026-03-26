products = {
    "苹果": 5,
    "香蕉": 3,
    "牛奶": 12,
    "面包": 8
}
shop = []
total = 0
name = input("请输入你需要的商品：")
while name != "q":
    if name in products:
        shop.append(name)
        name = input("请输入你需要的商品：")
    else:
        print("商品不存在，请重新输入！")
        name = input("请输入你需要的商品：")
else:
    for each in shop:
        total += products[each]
print(f"你买了{shop}")
print(f"总价是{total}")