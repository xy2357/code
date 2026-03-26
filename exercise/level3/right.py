nums = [1, 2, 3, 4, 5, 6, 7, 8, 9]
last = nums[len(nums) - 1]
for i in range(len(nums) - 1, 0, -1):
    # print(i)
    nums[i] = nums[i - 1]
nums[0] = last

print(nums)