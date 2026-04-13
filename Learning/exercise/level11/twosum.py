def two_sum(nums, target):
    res = []
    for i in range(len(nums)):
        for j in range(i+1, len(nums)):
            nums1 = nums[i]
            nums2 = target - nums1
            if nums[j] == nums2:
                res = [i, j]
    return res

nums = [2, 7, 11, 15]
target = 4

print(two_sum(nums, target))