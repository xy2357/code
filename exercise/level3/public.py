a = [1, 2, 3, 4, 5]
b = [3, 4, 5, 6, 7]

new = []
for each in a:
    if each in b:
        new.append(each)

print(new)