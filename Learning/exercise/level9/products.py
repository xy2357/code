products = {
    "苹果": {"price": 5, "stock": 10, "sold": 0},
    "牛奶": {"price": 12, "stock": 6, "sold": 0}
}

while True:
    print("1.查看所有商品\n2.添加商品\n3.补货\n4.售卖商品\n5.查询商品\n6.修改价格\n7.查看销量排行最高的商品\n8.退出")
    index = input("请输入操作：")

    if index == "1":
        if len(products) != 0:
            for each in products:
                print(f"{each}的价格为{products[each]['price']},库存为{products[each]['stock']},已售{products[each]['sold']}")
        else:
            print("库存为空！")

    elif index == "2":
        name = input("请输入商品名称：")
        if name not in products:
            price = int(input("请输入商品价格："))
            stock = int(input("请输入库存"))
            sold = 0
            products[name] = {"price":price, "stock":stock, "sold":sold}
        else:
            print("商品已存在！")

    elif index == "3":
        name = input("请输入商品名：")
        if name in products:
            stock = int(input("请输入补货数量："))
            products[name]["stock"] += stock
        else:
            print("商品不存在！")

    elif index == "4":
        name = input("请输入商品名：")
        if name in products:
            nums = int(input("请输入售卖数量："))
            if nums <= products[name]["stock"]:
                products[name]["stock"] -= nums
                products[name]["sold"] += nums
            else:
                print("库存不足！")
        else:
            print("商品不存在！")

    elif index == "5":
        name = input("请输入商品名：")
        if name in products:
            print(f"{name}的价格为{products[name]['price']},库存为{products[name]['stock']},已售{products[name]['sold']}")
        else:
            print("商品不存在！")

    elif index == "6":
        name = input("请输入商品名：")
        if name in products:
            price = int(input("请输入修改后价格："))
            products[name]["price"] = price
        else:
            print("商品不存在！")

    elif index == "7":
        max_sold = 0
        max_product = ""
        for each in products:
            if products[each]['sold'] > max_sold:
                max_sold = products[each]['sold']
                max_product = each
        if max_sold != 0:
            print(f"销量最高的商品为{max_product},销量为{max_sold}")
        else:
            print("暂无销量排行！")

    elif index == "8":
        break