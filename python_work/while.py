# current_number = 1
# while current_number <= 5:
#     print(current_number)
#     current_number += 1

# prompt = "\nTell me something, and I will repeat it back to you!"
# prompt += "\nEnter 'quit' to end the program."
# message = ""
# while message != 'quit':
#     message = input(prompt)
#     if message != 'quit':
#         print(message)

# prompt = "\nTell me something, and I will repeat it back to you!"
# prompt += "\nEnter 'quit' to end the program."
# active = True
# while active:
#     message = input(prompt)
#     if message == 'quit':
#         active == False
#     else:
#         print(message)

# prompt = "\nTell me something, and I will repeat it back to you!"
# prompt += "\nEnter 'quit' to end the program."
# message = ""
# while message != 'quit':
#     message = input(prompt)
#     if message == 'quit':
#         break
#     else:
#         print(message)

# current_number = 0
# while current_number < 10:
#     current_number += 1
#     if current_number % 2 ==0:
#         continue
#     print(current_number)

# x = 1
# while x < 5:
#     print(x)
#     x += 1

# active = True
# while active:
#     age = input("请输入你的年龄：")
#     if age == 'quit':
#         break
#     age = int(age)
#     if age < 3:
#         print("免费的。")
#     elif age > 3 and age < 12:
#         print("费用为10美元。")
#     elif age > 15:
#         print("费用为15美元。")

# unconfirmed_user = ['alice', 'brain', 'candace']
# confirmed_user = []
# while unconfirmed_user:
#     user = input("请输入你的名字：")
#
#     if user == "quit":
#         break
#
#     if user in unconfirmed_user:
#         unconfirmed_user.remove(user)
#         confirmed_user.append(user)
#         print("已验证用户")
#     else:
#         print("不存在该用户！")
# print(unconfirmed_user)
# print(confirmed_user)

# responses = {}
#
# # 设置标志判断循环是否继续
# polling_active = True
#
# while polling_active:
#     # 输入名字和回答
#     name = input("请输入你的名字：")
#     response = input("你想爬哪座山？")
#
#     # 将答案存储在字典里
#     responses[name] = response
#     # 判断是否还否有其他人回答
#     repeat = input("还有其他人回答吗？")
#     if repeat == 'no':
#         polling_active = False
# # 打印输出结果
# print("\n---Poll Result---\n")
# for name, response in responses.items():
#     print(responses[name] + "想爬的山是：" + responses[response])

# sandwich_orders = ['a', 'b', 'c']
# finished_sandwiches = []
# index = 0
#
# while index < len(sandwich_orders):
#     sandwich = sandwich_orders[index]
#     print("我给你做了个:" + sandwich)
#     sandwich_orders.remove(sandwich)
#     finished_sandwiches.append(sandwich)
#
#     if not sandwich_orders:
#         print(finished_sandwiches)
# for san in finished_sandwiches:
#     print("想在有这些：" + san)

# sandwich_orders = ['a', 'b', 'c', 'd', 'b']
# finished_sandwiches = []
# print("烟熏牛肉卖完了！")
# index = 0
#
# while index < len(sandwich_orders):
#
#     if sandwich_orders[index] != 'b':
#         finished_sandwiches.append(sandwich_orders[index])
#         index += 1
#     else:
#         index += 1
#
# print(finished_sandwiches)




















