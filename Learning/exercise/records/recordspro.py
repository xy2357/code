import json
from pathlib import Path

FILE_PATH = Path("records.json")


def load_records():
    if not FILE_PATH.exists():
        return []

    try:
        with FILE_PATH.open("r", encoding="utf-8") as file:
            data = json.load(file)
        if isinstance(data, list):
            return data
        print("文件格式不对，已重置为空数据！")
        return []
    except json.JSONDecodeError:
        print("文件损坏，已重置为空数据！")
        return []


def save_records(records):
    with FILE_PATH.open("w", encoding="utf-8") as file:
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
            if month < 1 or month > 12:
                print("请输入正确的月份！")
                continue
            return f"{month}月"
        except ValueError:
            print("请输入月份数字！")


def show_all_records(records):
    if not records:
        print("暂无数据！")
        return

    for i, record in enumerate(records, start=1):
        print(
            f"{i}. {record['month']} - {record['desc']} - {record['type']} - {record['amount']}"
        )


def add_record(records, record_type):
    month = input_month("请输入月份：")
    desc = input("请输入描述：").strip()
    if not desc:
        print("描述不能为空！")
        return

    amount = input_amount("请输入金额：")
    records.append({
        "month": month,
        "type": record_type,
        "amount": amount,
        "desc": desc
    })
    save_records(records)
    print("添加成功。")


def build_month_summary(records):
    summary = {}

    for record in records:
        month = record["month"]
        if month not in summary:
            summary[month] = {"收入": 0, "支出": 0, "结余": 0}

        if record["type"] == "收入":
            summary[month]["收入"] += record["amount"]
        else:
            summary[month]["支出"] += record["amount"]

        summary[month]["结余"] = summary[month]["收入"] - summary[month]["支出"]

    return summary


def show_total_income(records):
    total = 0
    for record in records:
        if record["type"] == "收入":
            total += record["amount"]
    print(f"总收入为：{total}")


def show_total_cost(records):
    total = 0
    for record in records:
        if record["type"] == "支出":
            total += record["amount"]
    print(f"总支出为：{total}")


def show_balance(records):
    balance = 0
    for record in records:
        if record["type"] == "收入":
            balance += record["amount"]
        else:
            balance -= record["amount"]
    print(f"总结余为：{balance}")


def show_month_summary(records):
    if not records:
        print("暂无数据！")
        return

    summary = build_month_summary(records)
    for month, info in summary.items():
        print(
            f"{month} 的收入为 {info['收入']}，支出为 {info['支出']}，结余为 {info['结余']}"
        )


def sort_records_by_amount(records):
    if not records:
        print("暂无数据！")
        return

    for i in range(len(records) - 1):
        for j in range(len(records) - 1 - i):
            if records[j]["amount"] < records[j + 1]["amount"]:
                records[j], records[j + 1] = records[j + 1], records[j]

    print("已按金额从高到低排序：")
    show_all_records(records)


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
            add_record(records, "收入")
        elif choice == "3":
            add_record(records, "支出")
        elif choice == "4":
            show_total_income(records)
        elif choice == "5":
            show_total_cost(records)
        elif choice == "6":
            show_balance(records)
        elif choice == "7":
            show_month_summary(records)
        elif choice == "8":
            sort_records_by_amount(records)
        elif choice == "9":
            save_records(records)
            print("程序已退出。")
            break
        else:
            print("请输入有效操作！")


if __name__ == "__main__":
    main()