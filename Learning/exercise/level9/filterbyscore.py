def filter_by_score(students, min_score):
    filter_list = []
    for each in students:
        if each['score'] >= min_score:
            filter_list.append(each)
    return filter_list

students = [
    {"name": "小明", "score": 85},
    {"name": "小红", "score": 92},
    {"name": "小刚", "score": 78}
]

print(filter_by_score(students, 80))