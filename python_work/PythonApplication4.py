# cars = ['bmw', 'audi', 'toyota', 'subaru']
#
# for car in cars:
#     if car == 'audi':
#         print(car.upper())
#     else:
#         print(car.title())

# banned_users = ['andrew', 'carolina', 'david']
# user = 'marie'
#
# if user not in banned_users:
#     print(user.title() + ",you can post a response if you wish!")
#
# car = 'subaru'
# print("Is car == 'subaru'? I predict True.")
# print(car == 'subaru')
#
# print("\nIs car == 'audi'? I predict False.")
# print(car == 'subaru')

# age = int(input("Please input a number:"))
# if age < 4:
#     print("Your admission cost is $0!")
# elif age < 18:
#     print("Your admission cost is $5!")
# else:
#     print("Your admission cost is $10!")

users = ['alice', 'admin', 'bob', 'cali', 'dav', '']
for user in users:
    if len(user) == 0:
        print("We need to find some users!")
        users.remove(user)
    else:
        if user == 'admin':
            print("Hello admin ,would you like to see a statues report?")
        else:
            print("Hello Eric,thank you for logging in again.")
print(users)


current_users = ['alice', 'bob', 'cali', 'dav', 'ela']
new_users = ['dav', 'ela', 'four', 'glory', 'hap']
for new_user in new_users:
    if new_user in current_users:
        print("Please enter another name:")
    else:
        print("Thank you for joining!")



