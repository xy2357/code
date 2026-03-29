scores = {
    "小明": 85,
    "小红": 92,
    "小刚": 78,
    "小美": 96,
    "小华": 88
}

total = 0
for score in scores.values():
    total += score
average = total / len(scores)
print(f"平均分是{average}")

for name, score in scores.items():
    if score > average:
        print(f"{name}:{score}")