bookkeeping = []

while True:
    total_earn = 0
    total_cost = 0
    print("1.查看所有记录\n2.添加收入记录\n3.添加支出记录\n4.统计总收入\n5.统计总支出\n6.计算当前结余\n7.退出")
    index = input("请输入操作：")

    if index == "1":
        if len(bookkeeping) != 0:
            for each in bookkeeping:
                print(f"{each['desc']}{each['type']}:{each['amount']}")
        else:
            print("暂无统计")

    if index == "2":
        money_desc = input("请输入描述：")
        money_amount = int(input("请输入数量："))
        money_type = "收入"
        bookkeeping.append({"type":money_type, "amount":money_amount, "desc":money_desc})

    if index == "3":
        money_desc = input("请输入描述：")
        money_amount = int(input("请输入数量："))
        money_type = "支出"
        bookkeeping.append({"type": money_type, "amount": money_amount, "desc": money_desc})

    if index == "4":
        if len(bookkeeping) != 0:
            for each in bookkeeping:
                if each["type"] == "收入":
                    total_earn += each["amount"]
            print(f"总输入为{total_earn}")
        else:
            print("暂无统计")

    if index == "5":
        if len(bookkeeping) != 0:
            for each in bookkeeping:
                if each["type"] == "支出":
                    total_cost += each["amount"]
            print(f"总输入为{total_cost}")
        else:
            print("暂无统计")

    if index == "6":
        for each in bookkeeping:
            if each["type"] == "收入":
                total_earn += each["amount"]
            elif each["type"] == "支出":
                total_cost += each["amount"]
        balance = total_earn - total_cost
        print(f"结余为{balance}")

    if index == "7":
        break
