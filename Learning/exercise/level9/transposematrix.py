def transpose_matrix(matrix):
    trans_matrix = []
    for i in range(len(matrix[0])):
        trans_row = []
        for j in range(len(matrix)):
            trans_row.append(matrix[j][i])
        trans_matrix.append(trans_row)
    return trans_matrix

matrix = [
    [1, 2, 3],
    [4, 5, 6]
]
print(transpose_matrix(matrix))