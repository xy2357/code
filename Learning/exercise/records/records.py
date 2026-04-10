import json
from pathlib import Path

FILE_PATH = Path("records.json")

def load_records():
    if not FILE_PATH.exists():
        return []
    try:
        with open(FILE_PATH, 'r', encoding='utf_8') as file:
            records = json.load(file)
        if isinstance(records, list):
            return records
        print("文件格式不对，返回空数据！")
        return []
    except json.JSONDecodeError:
        print("文件损坏，返回空数据！")
        return []

def save_records(records):
    with open(FILE_PATH, 'w', encoding='utf_8') as file:
        json.dump(records, file, ensure_ascii=False, indent=2)

def input_amount(prompt):
    while True:
        text = input(prompt).strip()
        try:
            amount = int(text)
            if amount <= 0:
                print("请输入正整数！")
                continue
            return amount
        except ValueError:
            print("请输入数字！")

def input_month(prompt):
    while True:
        text = input(prompt).strip()
        try:
            month = int(text)
            if month <= 0 or month > 12:
                print("请输入正确的月份！")
                continue
            return month
        except ValueError:
            print("请输入月份！")

def show_all_records(records):
    if len(records) == 0:
        print("暂无数据！")
        return
    for record in records:
        print(f"{record['month']}月:{record['desc']}{record['type']} -> {record['amount']}")

def add_income_record(records):
    month = str(input_month("请输入月份："))
    desc = input("请输入描述：")
    amount = input_amount("请输入金额：")
    records.append({'type':'收入', 'amount':amount, 'desc':desc, 'month':month})
    save_records(records)

def add_cost_record(records):
    month = str(input_month("请输入月份："))
    desc = input("请输入描述：")
    amount = input_amount("请输入金额：")
    records.append({'type':'支出', 'amount':amount, 'desc':desc, 'month':month})
    save_records(records)

def sum_income(records):
    total_income = 0
    found = False

    for record in records:
        if record['type'] == "收入":
            total_income += record['amount']
            found = True
    if not found:
        print("暂无收入数据！")
        return

    print(f"总收入为:{total_income}")

def sum_cost(records):
    total_cost = 0
    found = False

    for record in records:
        if record['type'] == "支出":
            total_cost += record['amount']
            found = True
    if not found:
        print("暂无支出数据！")
        return

    print(f"总支出为:{total_cost}")

def balance_records(records):
    balance = 0
    if len(records) == 0:
        print("暂无数据！")
        return

    for record in records:
        if record['type'] == "收入":
            balance += record['amount']
        else :
            balance -= record['amount']
    print(f"总结余为{balance}")

def month_records(records):
    if len(records) == 0:
        print("暂无数据！")
        return
    
    month = {}
    for record in records:
        if record['month'] not in month:
            if record['type'] == '收入':
                month[record['month']] = {'收入':record['amount'], '支出':0, '结余':record['amount'] - 0}
            else:
                month[record['month']] = {'收入':0, '支出':record['amount'], '结余':0 - record['amount']}
        else:
            if record['type'] == '收入':
                month[record['month']]['收入'] += record['amount']
                month[record['month']]['结余'] += record['amount']
            else:
                month[record['month']]['支出'] += record['amount']
                month[record['month']]['结余'] -= record['amount']

    for each in month:
        print(f"{each}月的收入为{month[each]['收入']}, 支出为{month[each]['支出']},结余为{month[each]['结余']}")

def sort_records(records):
    if len(records) == 0:
        print("数据为空")
        return

    for i in range(len(records) - 1):
        for j in  range(len(records) - i - 1):
            if records[j]['amount'] < records[j+1]['amount']:
                temp = records[j]
                records[j] = records[j+1]
                records[j+1] = temp

    for record in records:
        print(f"{record['month']}月的{record['desc']}{record['type']}:{record['amount']}")

def main():
    records = load_records()
    save_records(records)

    while True:
        print("\n1. 查看所有记录")
        print("2. 添加收入记录")
        print("3. 添加支出记录")
        print("4. 查看总收入")
        print("5. 查看总支出")
        print("6. 查看总结余")
        print("7. 查看每月汇总")
        print("8. 按金额从高到低显示记录")
        print("9. 退出")

        choice = input("请输入操作：").strip()

        if choice == "1":
            show_all_records(records)
        elif choice == "2":
            add_income_record(records)
        elif choice == "3":
            add_cost_record(records)
        elif choice == "4":
            sum_income(records)
        elif choice == "5":
            sum_cost(records)
        elif choice == "6":
            balance_records(records)
        elif choice == "7":
            month_records(records)
        elif choice == "8":
            sort_records(records)
        elif choice == "9":
            save_records(records)
            print("程序已退出。")
            break
        else:
            print("请输入有效操作！")

main()