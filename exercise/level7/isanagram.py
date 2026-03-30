def is_anagram(s1, s2):
    s1_dict = {}
    s2_dict = {}
    for each in s1:
        if each not in s1_dict:
            s1_dict[each] = 1
        else:
            s1_dict[each] += 1

    for each in s2:
        if each not in s2_dict:
            s2_dict[each] = 1
        else:
            s2_dict[each] += 1

    return s1_dict == s2_dict

print(is_anagram("listen", "silent"))