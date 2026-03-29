# scores = {
#     "小明": 85,
#     "小红": 92,
#     "小刚": 78,
#     "小美": 96
# }
# students = {}
# for name, score in scores.items():
#     students[score] = name
#
# score_list = []
# for score in students.keys():
#     score_list.append(score)
#
# for i in range(len(score_list) - 1):
#     for j in range(len(score_list) - i - 1):
#         if score_list[j] < score_list[j+1]:
#             x = score_list[j]
#             score_list[j] = score_list[j+1]
#             score_list[j+1] = x
#
# for each in score_list:
#     print(f"{students[each]}:{each}")

scores = {
    "小明": 85,
    "小红": 92,
    "小刚": 78,
    "小美": 96
}
student_list = []
for name, score in scores.items():
    student_list.append([name,score])

for i in range(len(student_list) - 1):
    for j in range(len(student_list) - i - 1):
        if student_list[j][1] < student_list[j+1][1]:
            x = student_list[j]
            student_list[j] = student_list[j+1]
            student_list[j+1] = x

for each in student_list:
    print(f"{each[0]}:{each[1]}")