# def count_words(text):
#     count = 0
#     for each in text.split(" "):
#         count += 1
#     return count
#
# print(count_words("I love python"))

def count_words(text):
    return len(text.split())

text = input("请输入一句英文：")
print(count_words(text))