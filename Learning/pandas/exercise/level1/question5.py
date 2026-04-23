import pandas as pd

pd.set_option('display.max_columns', None)

df_all = pd.DataFrame([
    [4001, 1, 'Alice', 'China', 'Electronics', 'Keyboard', 2, 398, 'paid',      '2024-01-03 10:15', '2023-12-20'],
    [4002, 2, 'Bob',   'USA',   'Electronics', 'Monitor',  1, 1299,'paid',      '2024-01-05 14:20', '2023-12-28'],
    [4003, 1, 'Alice', 'China', 'Stationery',  'Notebook', 5, 75,  'paid',      '2024-01-08 09:00', '2023-12-20'],
    [4004, 3, 'Cathy', 'china', 'Electronics', 'Mouse',    3, 297, 'cancelled', '2024-01-08 21:10', '2024-01-05'],
    [4005, 4, 'David', 'UK',    'Furniture',   'Desk',     1, 849, 'paid',      '2024-02-01 11:30', '2024-01-10'],
    [4006, 5, 'Eva',   'China', 'Furniture',   'Chair',    2, 998, 'paid',      '2024-02-03 16:45', '2024-01-18'],
    [4007, 2, 'Bob',   'USA',   'Electronics', 'Keyboard', 1, 199, 'refunded',  '2024-02-10 08:00', '2023-12-28'],
    [4008, 6, 'Frank', 'USA',   'Stationery',  'Notebook',10, 145, 'paid',      '2024-02-12 19:20', '2024-02-01'],
    [4009, 1, 'Alice', 'China', 'Electronics', 'Monitor',  1, 1099,'paid',      '2024-02-15 13:00', '2023-12-20'],
    [4010, 3, 'Cathy', 'China', 'Furniture',   'Chair',    1, 499, 'paid',      '2024-03-01 10:10', '2024-01-05'],
    [4011, 5, 'Eva',   'China', 'Electronics', 'Keyboard', 2, 378, 'paid',      '2024-03-03 12:00', '2024-01-18'],
    [4012, 2, 'Bob',   'USA',   'Furniture',   'Desk',     1, 899, 'paid',      '2024-03-05 18:30', '2023-12-28'],
    [4013, 2, 'Bob',   'USA',   'Stationery',  'Notebook', 4, 60,  'paid',      '2024-03-12 09:40', '2023-12-28'],
    [4014, 4, 'David', 'UK',    'Electronics', 'Mouse',    2, 198, 'paid',      '2024-03-18 20:15', '2024-01-10'],
    [4015, 6, 'Frank', 'USA',   'Furniture',   'Chair',    1, 499, 'paid',      '2024-04-02 11:05', '2024-02-01'],
    [4016, 1, 'Alice', 'China', 'Furniture',   'Desk',     1, 899, 'paid',      '2024-04-06 15:30', '2023-12-20'],
    [4017, 3, 'Cathy', 'China', 'Electronics', 'Monitor',  1, 1299,'paid',      '2024-04-09 14:00', '2024-01-05'],
    [4018, 5, 'Eva',   'China', 'Stationery',  'Notebook', 8, 120, 'paid',      '2024-04-11 08:25', '2024-01-18'],
    [4019, 4, 'David', 'UK',    'Furniture',   'Desk',     1, 899, 'cancelled', '2024-04-20 17:45', '2024-01-10'],
    [4020, 6, 'Frank', 'USA',   'Electronics', 'Keyboard', 1, 199, 'paid',      '2024-04-25 09:10', '2024-02-01'],
], columns=[
    'order_id', 'user_id', 'name', 'country', 'category', 'product_name',
    'quantity', 'final_amount', 'status', 'order_time', 'register_date'
])

# ## 题目1
filter_paid = df_all['status'] == 'paid'
# # print(df_all[filter_paid])
# month_report = df_all[filter_paid].groupby((pd.to_datetime(df_all['order_time'])).dt.month).agg(
#     order_count=('order_id', 'count'),
#     total_amount=('final_amount', 'sum'),
#     paid_user=('user_id', 'nunique')
# )
# month_report['order_avg'] = month_report['total_amount'] / month_report['order_count']
# month_report['user_avg'] = month_report['total_amount'] / month_report['paid_user']
# print(month_report['total_amount'].idxmax())
# print(month_report['order_count'].idxmax())
# print(month_report['user_avg'].idxmax())
# print(month_report )

## 题目2
# user_report = df_all[filter_paid].groupby('user_id').agg(
#     again_user_order_count = ('order_id', 'count'),
#     again_user_total_amount=('final_amount', 'sum')
# )
# # print(user_report)
#
# filter_again = user_report['again_user_order_count'] >= 2
# print(user_report[filter_again])
# print(user_report[filter_again].count())
# print(user_report[filter_again]['again_user_total_amount'].sum())
# print(df_all['final_amount'].sum() - user_report[filter_again]['again_user_total_amount'].sum())


## 题目3
# country_report = df_all[filter_paid].groupby(['country','name']).agg(
#     total_sales=('final_amount', 'sum')
# )
# country_report = country_report.pivot_table(index='country', columns='name', values='total_sales').fillna(0)
# print(country_report)

## 题目4
# user_report = df_all[filter_paid].groupby('user_id').agg(
#     user_name=('name', 'first'),
#     first_order=('order_time', 'first'),
#     first_product=('product_name' , 'first'),
#     first_amount=('final_amount', 'first'),
#     register_date=('register_date', 'first')
# )
# # df_all = df_all.set_index('user_id')
# # print(df_all)
# user_report['day'] = (pd.to_datetime(user_report['first_order']) - pd.to_datetime(user_report['register_date']))
# print(user_report)

## 题目5
country_report_new = df_all[filter_paid].groupby('country').agg(
    user_count=('user_id', 'nunique'),
    paid_count=('quantity', 'sum'),
    total_amount=('final_amount', 'sum')
)