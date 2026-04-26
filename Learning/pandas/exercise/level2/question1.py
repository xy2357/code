import pandas as pd
import numpy as np

pd.set_option('display.max_columns', None)

# 订单表
df_orders = pd.DataFrame({
    'order_id': [1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008],
    'user_id': ['101', '102', '101', '103', '104', '102', '105', '101'],
    'order_time': [
        '2026-01-05', '2026-01-11', '2026-02-03', '2026-02-17',
        '2026-03-01', '2026-03-11', '2026-03-21', '2026-03-25'
    ],
    'status': ['paid', 'unpaid', 'paid', 'paid', 'cancel', 'paid', 'paid', 'paid'],
    'final_amount': [120, 80, 150, 200, 60, 130, 300, 170]
})

# 用户表
df_users = pd.DataFrame({
    'user_id': [101, 102, 103, 104, 105],
    'user_name': ['A', 'B', 'C', 'D', 'E'],
    'vip_level': [1, 2, 1, 3, 2]
})

# 推塔表
df_stage = pd.DataFrame({
    'b_role_id': [5590000000001, 5590000000001, 5610000000002, 5610000000002, 5610000000003],
    'b_zone_id': [1302, 1302, 1305, 1305, 1307],
    'flag': [False, True, False, True, False],
    'properties_stage_id': [12, 15, 20, 22, 18]
})

# print(df_orders)
# print()
# print(df_users)
# print()
# print(df_stage)

# ## 题目1
df_orders['order_time'] = pd.to_datetime(df_orders['order_time'])
# print(df_orders.dtypes)
#
# ## 题目2
filt = df_orders['status'] == 'paid'
# df_paid = df_orders[filt][['order_id', 'user_id', 'order_time', 'final_amount']].reset_index(drop=True)
# print(df_paid['user_id'])
#
# ## 题目3
# print(df_paid['order_id'].count())
# print(df_paid['final_amount'].sum())
# print(df_paid['user_id'].nunique())
#
## 题目4
# month_report = df_orders[filt].groupby(df_orders[filt]['order_time'].dt.to_period('M')).agg(
#     order_count=('order_id', 'count'),
#     total_amount=('final_amount', 'sum'),
#     paid_user=('user_id', 'nunique')
# )
# month_report['order_avg'] = month_report['total_amount'] / month_report['order_count']
# month_report['user_avg'] = month_report['total_amount'] / month_report['paid_user']
#
# print(month_report)
#
# ## 题目5
# print(month_report['total_amount'].idxmax())
# print(month_report['order_count'].idxmax())
# print(month_report['user_avg'].idxmax())
#
# ## 题目6
# user_report = df_orders[filt].groupby('user_id').agg(
#     paid_count=('order_id', 'count'),
#     total_amount=('final_amount', 'sum')
# )
# filter_user = user_report['paid_count'] >= 2
# print(user_report[filter_user])
#
# ## 题目7
# # dt.month可以会只想不同年份的同一个月
# # 改成dt.to_period()
#
# ## 题目8
# filter_paid = df_orders['status'] == 'paid'
# print(df_orders[filter_paid].index)
# print(pd.to_datetime(df_orders['order_time']).dt.month.index)
# # 第一个数据是筛选后的表，使用的索引是原表的索引
# # 第二个是直接使用的原表
#
# ## 题目9
# # 数据类型不一样，df_paid['user_id']是str类型，df_users['user_id']是Int64类型
# df_paid['user_id'] = df_paid['user_id'].astype('Int64')
# df_paid.merge(df_users, on='user_id', how='left')
# print(df_paid)
#
# ## 题目10
# df_paid['user_id'] = df_paid['user_id'].astype('int')
# df_paid_user = df_paid.merge(df_users, on='user_id', how='left')
# print(df_paid_user[['order_id', 'user_id', 'user_name', 'vip_level', 'final_amount']])
#
# ## 题目11
# print(df_paid['user_id'].map(type).value_counts())
# print(df_paid['user_id'].dtype)
#
# ## 题目12
# # user= (df_stage.groupby(['b_role_id', 'b_zone_id', 'flag']))['properties_stage_id'].max().unstack().rename(columns={False: '塔1', True: '塔2'})
# # user[['塔1', '塔2']] = user[['塔1', '塔2']].astype('Int64')
#
# user = df_stage.pivot_table(
#     index=(['b_role_id', 'b_zone_id']),
#     columns='flag',
#     values='properties_stage_id',
#     aggfunc='max'
# ).rename(columns={False: '塔1', True: '塔2'}).reset_index()
# user[['塔1', '塔2']] = user[['塔1', '塔2']].astype('Int64')
# print(user)
#
# ## 题目13
# # 'True'应该为True
#
# ## 题目14
# user= (df_stage.groupby(['b_role_id', 'b_zone_id', 'flag']))['properties_stage_id'].max().unstack().rename(columns={False: '塔1', True: '塔2'})
# print(user)
#
# user1= (df_stage.groupby(['b_role_id', 'b_zone_id', 'flag']))['properties_stage_id'].max().unstack().rename(columns={False: '塔1', True: '塔2'}).reset_index()
# print(user1)
#
# ## 题目15
# pd.set_option('display.float_format', '{:.0f}'.format)
#
# ## 题目16
# #题目16和15不是一个意思？
#
## 题目17
# filter_paid = df_orders['status'] == 'paid'
# df_orders = df_orders[filter_paid]
# df_orders['user_id'] = df_orders['user_id'].astype('Int64')
# user_report = df_orders.groupby('user_id').agg(
#     paid_order_count=('order_id', 'count'),
#     paid_total_amount=('final_amount', 'sum')
# )
#
# final_report = user_report.merge(
#     df_users,
#     on='user_id',
#     how='left'
# ).sort_values('paid_total_amount', ascending=False).reset_index(drop=True)
#
# final_report['paid_avg_amount'] = final_report['paid_total_amount'] / final_report['paid_order_count']
#
# print(final_report)
#
# ## 题目18
# filter_user = final_report['paid_order_count'] >= 2
# print(final_report[filter_user])

## 题目20
filter_paid = df_orders['status'] == 'paid'
df_paid = df_orders[filter_paid].copy()
df_paid['user_id'] = df_paid['user_id'].astype('int')
# print(df_paid)
df_paid['user_name'] = df_paid['user_id'].map(df_users.set_index('user_id')['user_name'])
df_paid['vip_level'] = df_paid['user_id'].map(df_users.set_index('user_id')['vip_level'])
print(df_paid)
