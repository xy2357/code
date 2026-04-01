students = [
    {"name": "小明", "score": 85},
    {"name": "小红", "score": 92},
    {"name": "小刚", "score": 78},
    {"name": "小美", "score": 96},
    {"name": "小华", "score": 88}
]
total_score = 0
average_score = 0
max_score = 0
max_student = ""
min_score = 100
min_student = ""
high_average = []
counts = {"A":0, "B":0, "C":0, "D":0, "E":0}
for each in students:
    total_score += each['score']
    if each['score'] > max_score:
        max_score = each['score']
        max_student = each['name']
    elif each['score'] < min_score:
        min_score = each['score']
        min_student = each['name']

average_score = total_score / len(students)

print(f"平均分为{average_score}")
print(f"最高分为{max_student}:{max_score}")
print(f"最低分为{min_student}:{min_score}")

for each in students:
    if each['score'] > average_score:
        print(f"{each['name']}比平均分高！")


for each in students:
    if 90 <= each['score'] <= 100:
        counts['A'] += 1
    elif 80 <= each['score'] <= 89:
        counts['B'] += 1
    elif 70 <= each['score'] <= 79:
        counts['C'] += 1
    elif 60 <= each['score'] <= 69:
        counts['D'] += 1
    else:
        counts['E'] += 1
print(counts)
