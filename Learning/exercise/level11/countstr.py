with open("article.txt", 'r', encoding="utf-8") as file:
    text = file.read().strip().lower()

str_dict = {}
res_list = []
k = 0

for ch in [",", ".", "!", "?", ";", ":", '"', "'"]:
    text = text.replace(ch,' ')

for each in text.split():
    if each not in str_dict:
        str_dict[each] = 1
    else:
        str_dict[each] += 1

print(str_dict)
while k < 5 and len(str_dict) > 0:
    max_count = 0
    max_str = ""
    for each, num in str_dict.items():
        if num > max_count:
            max_count = num
            max_str = each
    del str_dict[max_str]
    res_list.append(max_str)
    k += 1

print(res_list)