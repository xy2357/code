orders = []
order_id = 1
while True:
    print("1.查看所有订单\n2.创建订单\n3.查询订单\n4.修改订单状态\n5.删除订单\n6.退出")
    index = input("请输入操作：")
    found = False

    if index == "1":
        if len(orders) == 0:
            print("订单为空！")
        else:
            for each in orders:
                print(f"{each['id']}:商品为{each['items']},总价为{each['total']},状态为{each['status']}")

    if index == "2":
        items = [item for item in input("请输入商品名称：").split(" ")]
        total = int(input("请输入总价："))
        status = "未完成"
        orders.append({"id":order_id, "items": items, "total":total, "status": status})
        order_id += 1

    if index == "3":
        query_id = int(input("请输入订单编号："))
        for each in orders:
            if query_id == each['id']:
                print(f"{each['id']}:商品为{each['items']},总价为{each['total']},状态为{each['status']}")
                found = True
                break
        if not found:
                print("订单编号不存在")

    if index == "4":
        query_id = int(input("请输入订单编号："))
        for each in orders:
            if query_id == each['id']:
                status = "已完成"
                each['status'] = status
                found = True
                break
        if not found:
                print("订单编号不存在")

    if index == "5":
        query_id = int(input("请输入订单编号："))
        for each in orders:
            if query_id == each['id']:
                orders.remove(each)
                found = True
                break
        if not found:
                print("订单编号不存在")

    if index == "6":
        break
