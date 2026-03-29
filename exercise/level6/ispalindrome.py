def is_palindrome(text):
    for i in range(0, len(text)-1):
        if text[i] != text[len(text)-i-1]:
            return False
    return True

text = input("请输入字符串：")
print(is_palindrome(text))