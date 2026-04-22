import pandas as pd

users_merge = pd.DataFrame([
    [1, 'Alice', 'China', '2023-12-20'],
    [2, 'Bob', 'USA', '2023-12-28'],
    [3, 'Cathy', 'China', '2024-01-05'],
    [4, 'David', 'UK', '2024-01-10'],
    [5, 'Eva', 'China', '2024-01-18'],
    [6, 'Frank', 'USA', '2024-02-01'],
], columns=['user_id', 'name', 'country', 'register_date'])

products_merge = pd.DataFrame([
    [101, 'Keyboard', 'Electronics', 199],
    [102, 'Mouse', 'Electronics', 99],
    [103, 'Monitor', 'Electronics', 1299],
    [104, 'Desk', 'Furniture', 899],
    [105, 'Chair', 'Furniture', 499],
    [106, 'Notebook', 'Stationery', 15],
], columns=['product_id', 'product_name', 'category', 'price'])

orders_merge = pd.DataFrame([
    [3001, 1, 101, 2, '2024-01-03 10:15', 'paid'],
    [3002, 2, 103, 1, '2024-01-05 14:20', 'paid'],
    [3003, 1, 106, 5, '2024-01-08 09:00', 'paid'],
    [3004, 3, 102, 3, '2024-01-08 21:10', 'cancelled'],
    [3005, 4, 104, 1, '2024-02-01 11:30', 'paid'],
    [3006, 5, 105, 2, '2024-02-03 16:45', 'paid'],
    [3007, 2, 101, 1, '2024-02-10 08:00', 'refunded'],
    [3008, 6, 106, 10, '2024-02-12 19:20', 'paid'],
    [3009, 1, 103, 1, '2024-02-15 13:00', 'paid'],
    [3010, 3, 105, 1, '2024-03-01 10:10', 'paid'],
    [3011, 5, 101, 2, '2024-03-03 12:00', 'paid'],
    [3012, 2, 104, 1, '2024-03-05 18:30', 'paid'],
], columns=['order_id', 'user_id', 'product_id', 'quantity', 'order_time', 'status'])

df_all = orders_merge.merge(
    users_merge[['user_id', 'name', 'country']],
    on='user_id',
    how='left'
).merge(
    products_merge,
    on='product_id',
    how='left'
)
print(df_all)