import pandas as pd

employees = pd.DataFrame([
    [101, 'Tom', '25', 12000, 'Tech', ' Beijing', '2023-01-15'],
    [102, 'Jerry', '31', 18000, 'Tech', 'beijing', '2022-11-03'],
    [103, 'Linda', 'twenty-eight', 15000, 'HR', 'Shanghai ', '2023-03-20'],
    [104, 'Mike', None, None, 'Finance', 'shanghai', '2021-07-11'],
    [105, 'Nina', '29', 9500, 'Tech', 'Shenzhen', '2023-08-09'],
    [106, 'Oscar', '41', 22000, 'Finance', ' Guangzhou ', '2020-05-18'],
    [107, 'Penny', '35', None, 'HR', 'Beijing', '2023-12-01'],
    [108, 'Queen', '27', 13000, 'Tech', 'Shenzhen ', '2023-04-25'],
    [108, 'Queen', '27', 13000, 'Tech', 'Shenzhen ', '2023-04-25'],  # 重复行
    [109, 'Rick', '30', 16000, 'Tech', 'BEIJING', '2023-09-30'],
], columns=['emp_id', 'name', 'age', 'salary', 'department', 'city', 'join_date'])

#题目1
# filter_condition_1 = employees['city'].str.contains(' ')
filter_condition_1 = employees['city'].str.strip()
print(employees[filter_condition_1])

# filter_condition_2 = employees['city'].str.title() != employees['city']
filter_condition_2 = employees['city'].str.strip().str.title() != employees['city'].str.strip()
print(employees[filter_condition_2])

filter_condition_3 = pd.to_numeric(employees['age'], errors='coerce').isna()
print(employees[filter_condition_3])

print(employees[employees.duplicated(keep=False)])

#题目2
employees['city'] = employees['city'].str.strip().str.title()
print(employees)

employees['age'] = pd.to_numeric(employees['age'], errors='coerce')
print(employees)

employees['salary'] = employees['salary'].fillna(0)
print(employees)

employees['join_date'] = pd.to_datetime(employees['join_date'], errors='coerce')
print(employees)

# 题目3：
employees.drop_duplicates(keep='first', inplace=True)
print(employees)

# 题目4：
filter_condition_1 = (employees['join_date'].dt.year == 2023)
print(employees[filter_condition_1])

filter_condition_2 = employees['salary'] > 10000
print(employees[filter_condition_2])

filter_condition_3 = (employees['department'] == 'Tech') & (employees['city'].str.strip().str.title() == 'Beijing')
print(employees[filter_condition_3])

# 题目5：
employees['work_year'] = employees['join_date'].dt.year
employees['salary_level'] = employees['salary'].apply(lambda x: 'High' if x >= 15000 else 'Normal')
print(employees)