orders = {}
while True:
    print("1.查看所有订单\n2.创建订单\n3.查询订单\n4.修改订单状态\n5.删除订单\n6.退出")
    index = input("请输入操作：")

    if index == "1":
        if len(orders) == 0:
            print("订单为空！")
        else:
            print(orders)

    if index == "2":

