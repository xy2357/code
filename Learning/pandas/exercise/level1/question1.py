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

# 题目1：
print(students.head())
print(students.shape)
print(students.dtypes)
print(students.isnull().sum())

# 题目2
filter_condition_1 = students['class_name'] == 'Class A'
print(students[filter_condition_1])

filter_condition_2 = ((students['math'] > 80) & (students['english'] > 80))
print(students[filter_condition_2])

filter_condition_3 = (students['city'] == 'Shanghai') & (students['gender'] == 'female')
print(students[filter_condition_3])

# 题目3
print(students[['name', 'math', 'english']].sort_values(by='math',ascending=False))

# 题目4
students['total_score'] = students['math'] + students['english']
# students['is_pass'] = students['total_score'].apply(lambda x: True if x >= 120 else False)
students['is_pass'] = students['total_score'] >= 120
print(students)

## 题目5：
# print(students.groupby('city')['student_id'].count())
print(students['city'].value_counts())
print(students['math'].mean())
# print(students.loc[students['total_score']==students['total_score'].max(), 'name'])
print(students.loc[students['total_score'].idxmax(), 'name'])