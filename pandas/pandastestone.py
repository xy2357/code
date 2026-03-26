import pandas as pd

df = pd.read_excel('Test1.xlsx')

# 检测缺失值
null_values = df.isnull()

for index, row in null_values.iterrows():
    for column, value in row.items():
        if value:  # 如果是缺失值
            print(f'在第 {index + 1} 行、第 {column} 列发现缺失值')