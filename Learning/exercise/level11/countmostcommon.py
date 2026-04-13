def count_most_common(nums):
    nums_dict = {}
    k = 0
    nums_list = []
    for i in range(len(nums)):
        if nums[i] not in nums_dict:
            nums_dict[nums[i]] = 1
            continue
        nums_dict[nums[i]] += 1

    while k < 2 and len(nums_dict) > 0:
        max_num = 0
        max_count = 0
        for num, count in nums_dict.items():
            if count > max_count:
                max_num = num
                max_count = count
        del nums_dict[max_num]
        nums_list.append((max_num, max_count))
        k += 1
    return nums_list

nums = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4]

print(count_most_common(nums))