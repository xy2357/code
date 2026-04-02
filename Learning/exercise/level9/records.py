records = [
    {"month": "1月", "type": "收入", "amount": 5000},
    {"month": "1月", "type": "支出", "amount": 1200},
    {"month": "2月", "type": "收入", "amount": 5200},
    {"month": "2月", "type": "支出", "amount": 1800},
    {"month": "1月", "type": "支出", "amount": 300}
]

res_dict = {}
for record in records:
    if record['month'] in res_dict:
        if record['type'] == "收入":
            res_dict[record['month']]['收入'] += record['amount']
        elif record['type'] == "支出":
            res_dict[record['month']]['支出'] += record['amount']
        res_dict[record['month']]["结余"] = res_dict[record['month']]["收入"] - res_dict[record['month']]["支出"]
    else:
        if record['type'] == "收入":
            res_dict[record['month']] = {"收入": record['amount'], "支出": 0, "结余": 0}
        elif record['type'] == "支出":
            res_dict[record['month']] = {"收入": 0, "支出": record['amount'], "结余": 0}
        res_dict[record['month']]["结余"] = res_dict[record['month']]["收入"] - res_dict[record['month']]["支出"]

print(res_dict)