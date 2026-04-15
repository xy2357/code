def is_valid_brackets(text):
    brackets_list = []

    for each in text:
        if each in "([{" :
            brackets_list.append(each)
        else:
            if not brackets_list:
                return False

            top = brackets_list.pop()

            if each == ")" and top == "(":
                return True
            if each == "]" and top == "[":
                return True
            if each == "}" and top == "{":
                return True


    return len(brackets_list) == 0

print(is_valid_brackets("is_valid_brackets(text.['apple'])"))
