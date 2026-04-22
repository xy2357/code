import pandas as pd
from unicodedata import category

orders_basic = pd.DataFrame([
    [2001, 1, 'Electronics', 'Keyboard', 199, 2, 'paid', 'China'],
    [2002, 2, 'Electronics', 'Mouse', 99, 1, 'paid', 'USA'],
    [2003, 1, 'Furniture', 'Desk', 899, 1, 'paid', 'China'],
    [2004, 3, 'Electronics', 'Monitor', 1299, 1, 'cancelled', 'China'],
    [2005, 4, 'Stationery', 'Notebook', 15, 10, 'paid', 'UK'],
    [2006, 2, 'Electronics', 'Keyboard', 199, 1, 'refunded', 'USA'],
    [2007, 5, 'Furniture', 'Chair', 499, 2, 'paid', 'China'],
    [2008, 6, 'Stationery', 'Pen', 5, 20, 'paid', 'USA'],
    [2009, 3, 'Electronics', 'Mouse', 99, 3, 'paid', 'China'],
    [2010, 4, 'Furniture', 'Desk', 899, 1, 'paid', 'UK'],
    [2011, 5, 'Electronics', 'Monitor', 1299, 1, 'paid', 'China'],
    [2012, 6, 'Stationery', 'Notebook', 15, 5, 'paid', 'USA'],
], columns=['order_id', 'user_id', 'category', 'product_name', 'price', 'quantity', 'status', 'country'])

# # 题目1
# orders_basic['amount'] = orders_basic['price'] * orders_basic['quantity']
# filter_condition_1 = (orders_basic['status'] == 'paid')
# paid_report = orders_basic[filter_condition_1].groupby('country').agg(
#     order_count=('order_id', 'count'),
#     total_amount=('amount', 'sum')
# )
# paid_report['avg_amount'] = paid_report['total_amount'] / paid_report['order_count']
# print(paid_report)
#
# # 题目2
# category_report = orders_basic.groupby('category').agg(
#     total_quantity=('quantity', 'sum'),
#     total_amount=('amount', 'sum'),
#     unique_user=('user_id', 'nunique')
# )
# print(category_report)
#
# #题目3
# product_report = orders_basic.groupby('product_name').agg(
#     total_amount=('amount', 'sum'),
#     total_quantity=('quantity', 'sum'),
#     unique_user=('user_id', 'nunique')
# )
# print(product_report)
# print(product_report.nlargest(3, 'total_amount'))
# print(product_report.nlargest(3, 'total_quantity'))
# print(product_report.nlargest(3, 'unique_user'))
#
# # 题目4
# # country_report = orders_basic.pivot_table(index='product_name', columns='country',values='quantity')
# # print(country_report.idxmax())
# country_popular_product = orders_basic.groupby(['country', 'product_name']).agg(
#     total_quantity=('quantity', 'sum')
# ).reset_index()
# result = country_popular_product.loc[
#     country_popular_product.groupby('country')['total_quantity'].idxmax()
# ]
# print(result)
#
# # 题目5
# double_report = orders_basic.groupby(['country', 'category']).agg(
#     total_amount=('amount', 'sum')
# ).sort_values('total_amount', ascending=False)
# print(double_report)