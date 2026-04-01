# def compress_string(text):
#     text_dict = {}
#     comp_str = ""
#     i = 0
#     while i in range(len(text)):
#         if i > 0:
#             if text[i] == text[i-1]:
#                 text_dict[text[i-1]] += 1
#             else:
#                 text_dict[text[i]] = 1
#         else:
#             text_dict[text[i]] = 1
#         i += 1
#
#
#     for alph, value in text_dict.items():
#         comp_str += alph
#         comp_str += str(value)
#
#     return comp_str
#
# string = input("请输入字符串：")
# print(compress_string(string))

def compress_string(text):
    if text == "":
        return ""
    result = ""
    current_char = text[0]
    count = 1

    for i in range(1, len(text)):
        if text[i] == current_char:
            count += 1
        else:
            result += current_char + str(count)
            current_char = text[i]
            count = 1

    result += current_char + str(count)
    return result

text = input("请输入字符串：")
print(compress_string(text))