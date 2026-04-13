def longest_unique_substring(s):
    s_list = []
    max_len = 0
    for i in range(len(s)):
        while s[i]  in s_list:
            del s_list[0]
        s_list.append(s[i])

        if len(s_list) > max_len:
            max_len = len(s_list)

    return max_len

s = "abcabcbb"
print(longest_unique_substring(s))