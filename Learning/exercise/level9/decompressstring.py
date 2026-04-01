def decompress_string(text):
    str1 = ""
    for each in text:
        if each.isalpha():
            alpha = each
        else:
            nums = each
            str1 += alpha * int(nums)
    return str1

print(decompress_string("a2c1b3"))