def merge_sorted_list(list_a, list_b):
    merge_list = []
    i = 0
    j = 0

    while i < len(list_a) and j < len(list_b):
        if list_a[i] < list_b[j]:
            merge_list.append(list_a[i])
            i += 1
        else:
            merge_list.append(list_b[j])
            j += 1

    while i < len(list_a):
        merge_list.append(list_a[i])
        i += 1

    while j < len(list_b):
        merge_list.append(list_b[j])
        j += 1

    return merge_list

a = [1, 3, 5]
b = [2, 4, 6]

print(merge_sorted_list(a, b))
