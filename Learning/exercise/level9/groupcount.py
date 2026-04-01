def group_count(items, key_name):
    key_dict = {}
    for each in items:
        if each[key_name] in key_dict:
            key_dict[each[key_name]] += 1
        else:
            key_dict[each[key_name]] = 1
    return key_dict

logs = [
    {"user": "A", "action": "login"},
    {"user": "B", "action": "login"},
    {"user": "A", "action": "view"},
    {"user": "A", "action": "logout"}
]

print(group_count(logs, "action"))
