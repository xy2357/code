def find_in_matrix(matrix, target):
    found = False
    res_tup = ()
    for i in range(len(matrix)):
        if target in matrix[i]:
            row = i
            for j in range(len(matrix[i])):
                if matrix[i][j] == target:
                    col = j
                    res_tup = (row, col)
                    found = True
                    return res_tup
            if not found:
                return None
    return None


matrix = [
    [3, 5, 7],
    [1, 9, 2],
    [8, 4, 6]
]

print(find_in_matrix(matrix, 10))