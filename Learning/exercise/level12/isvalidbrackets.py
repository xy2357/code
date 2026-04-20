def is_valid_brackets(text):
    brackets_list = []
    brackets = ['(', ')', '[', ']', '{', '}']
    for each in text:
        if each in brackets:
            brackets_list.append(each)


    return brackets_list

print(is_valid_brackets("is_valid_brackets(text.['apple'])"))
