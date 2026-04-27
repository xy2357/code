import pandas as pd
import numpy as np

# df_s = pd.Series(data=(1, 2, 3, 4), name='num')
# # print(df_s)
#
# df_f = pd.DataFrame({'A':[1, 2, 3, 4]})
# # print(df_f)
#
# df_m = df_f.join(df_s)
# print(df_m)

# a = [1, 2, 3]
# df_s = pd.Series(a)
# print(df_s.cumprod())

# data = [['Google', 10], ['Runoob', 12], ['Wiki', 13]]
#
# df = pd.DataFrame(data).rename(columns={0: 'Site', 1: 'age'})
# print(df)

# data = {'Site': ['Google', 'Runoob', 'Wiki'], 'Age': [10, 12, 13]}
# df = pd.DataFrame(data)
# print(df)

# 用户表
# df_users = pd.DataFrame({
#     'user_id': [101, 102, 103, 104, 105],
#     'user_name': ['A', 'B', 'C', 'D', 'E'],
#     'vip_level': [1, 2, 1, 3, 2]
# })
#
# new_row = {'user_id': 13, 'user_name': 14, 'vip_level': 16}
# df_users = pd.concat([df_users, pd.DataFrame([new_row])], ignore_index=True)
# print(df_users)
# print(df_users.dtypes)
# df_users['user_name'] = df_users['user_name'].astype('str')
# print(df_users.dtypes)