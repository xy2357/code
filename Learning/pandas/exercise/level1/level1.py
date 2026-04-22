import pandas as pd
import numpy as np

students = pd.DataFrame([
    [1, 'Alice', 'female', 'Class A', 88, 92, 'Shanghai'],
    [2, 'Bob', 'male', 'Class A', 76, 81, 'Beijing'],
    [3, 'Cathy', 'female', 'Class B', 95, 87, 'Shanghai'],
    [4, 'David', 'male', 'Class B', 64, 73, 'Guangzhou'],
    [5, 'Eva', 'female', 'Class A', 82, 78, 'Shenzhen'],
    [6, 'Frank', 'male', 'Class C', 91, 89, 'Beijing'],
    [7, 'Grace', 'female', 'Class C', 58, 66, 'Shanghai'],
    [8, 'Henry', 'male', 'Class A', 84, np.nan, 'Hangzhou'],
    [9, 'Ivy', 'female', 'Class B', 77, 80, 'Beijing'],
    [10, 'Jack', 'male', 'Class C', 69, 72, 'Shanghai'],
], columns=['student_id', 'name', 'gender', 'class_name', 'math', 'english', 'city'])

# print(students)

# 题目一：
# print(students.head())
# print(students.shape)
# print(students.dtypes)
# print(students.isna().sum())
filt = students[3] == 'Class A'
print(students[filt])