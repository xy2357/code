scores = {
    "小明": 85,
    "小红": 92,
    "小刚": 78,
    "小美": 96,
    "小华": 88
}
total_score = 0
average = 0
max_score = 0

for score in scores.values():
    total_score +=score

average = total_score / len(scores)

print(f"总分为{total_score},平均分为{average}")

for student, score in scores.items():
    if max_score < score:
        max_score = score
        max_student = student
print(f"{max_student}为最高分，分数为{max_score}")