def binary_search(nums, target):
    found = False
    x = len(nums)
    while found:
        if target == nums[x]:
            index = x
            break
        else:
            if target in nums[:x]:
                x = x / 2
            elif target in nums[x:len(nums)]:
                x = (x+len(nums)) / 2
    return index
nums = [1, 3, 5, 7, 9, 11, 13]
print(binary_search(nums, 2))