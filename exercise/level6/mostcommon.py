def most_common(nums):
    global max_num
    most_dict = {}
    max_count = 0
    for each in nums:
        if each in most_dict:
            most_dict[each] += 1
        else:
            most_dict[each] = 1
    print(most_dict)

    for num, count in most_dict.items():
        if count > max_count:
            max_count = count
            max_num = num
    return max_num

print(most_common([1, 2, 2, 3, 3, 3, 4]))