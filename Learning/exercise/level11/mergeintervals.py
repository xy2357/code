def merge_intervals(intervals):
    for i in range(len(intervals) - 1):
        for j in range(len(intervals) - i - 1):
            if intervals[j][0] > intervals[j+1][0]:
                intervals[j], intervals[j+1] = intervals[j+1], intervals[j]

    # k = 0
    # while k < len(intervals) - 1:
    #     if intervals[k][1] >= intervals[k+1][0]:
    #         intervals[k][1] = max(intervals[k][1], intervals[k+1][1])
    #         del intervals[k+1]
    #     k += 1

    merge= [intervals[0]]

    for i in range(len(intervals)):
        last = merge[-1]
        current = intervals[i]

        if last[1] > current[0]:
            last[1] = max(last[1], current[1])
        else:
            merge.append(intervals[i])


    return merge

intervals = [[1, 3], [2, 6], [8, 10], [15, 18]]

print(merge_intervals(intervals))