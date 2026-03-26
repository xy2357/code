# alien_0 = {'color': 'green', 'points': 5}
# print(alien_0['color'])
# print(alien_0['points'])

# alien_0 = {'color': 'green', 'points': 5}
# print(alien_0)
#
# alien_0['x_position'] = 0
# alien_0['y_position'] = 25
# print(alien_0)

# alien_0 = {'x_position': 0, 'y_position': 25, 'speed': 'medium'}
# print("Original x_position: " + str(alien_0['x_position']))
# # 向右移动外星人
# # 根据移动速度决定将其移动多远
# if alien_0['speed'] == 'slow':
#     x_increment = 1
# elif alien_0['speed'] == 'medium':
#     x_increment = 2
# else:
#     x_increment = 3
#
# alien_0['x_position'] = alien_0['x_position'] + x_increment
# print("New position: " + str(alien_0['x_position']))
#
# alien_0['speed'] = 'fast'
#
# if alien_0['speed'] == 'slow':
#     x_increment = 1
# elif alien_0['speed'] == 'medium':
#     x_increment = 2
# else:
#     x_increment = 3
#
# alien_0['x_position'] = alien_0['x_position'] + x_increment
# print("New position: " + str(alien_0['x_position']))

# favorite_language = {
#     'jen': 'python',
#     'sarah': 'c',
#     'edward': 'ruby',
#     'phil': 'python',
# }
#
# print("Sarah's favorite language is " + favorite_language['sarah'].title() + ".")

# message = {'first_name': 'jianing', 'last_name': 'li', 'age': '24', 'city': 'shanghai'}
# print(message['first_name'])
# print(message['last_name'])
# print(message['age'])
# print(message['city'])

# user_0 = {
#     'username': 'efermi',
#     'first': 'enrico',
#     'last': 'fermi',
# }
# for key,value in user_0.items():
#     print("\nKey: " + key)
#     print("Value: " + value)

# favorite_language = {
#     'jen': 'python',
#     'sarah': 'c',
#     'edward': 'ruby',
#     'phil': 'python',
# }
#
# for name,language in favorite_language.items():
#     print(name.title() + "'s favorite language is " + language.title() + ".")

# favorite_language = {
#     'jen': 'python',
#     'sarah': 'c',
#     'edward': 'ruby',
#     'phil': 'python',
# }
#
# for name, language in favorite_language.items():
#     if language == 'python':
#         print(name.title() + "'s favorite language is " + language.title() + ".")

# favorite_languages = {
#     'jen': 'python',
#     'sarah': 'c',
#     'edward': 'ruby',
#     'phil': 'python',
# }
#
# friends = ['phil', 'sarah']
# for name in favorite_languages.keys():
#     print(name.title())
#
#     if name in friends:
#         print("Hi " + name.title() + " I see you favorite language is " + favorite_languages[name].title() + "!")

# rivers = {'nile': 'egypt', 'yangtze': 'china', 'amazon': 'south america'}
# for river, country in rivers.items():
#     print("The " + river.title() + "runs through " + country.title())
#
# for river in rivers.keys():
#     print(river)
#
# for country in rivers.values():
#     print(country)

# favorite_languages = {
#     'jen': 'python',
#     'sarah': 'c',
#     'edward': 'ruby',
#     'phil': 'python',
# }
# users = ['jen', 'phil']
#
# for user in favorite_languages.keys():
#     if user in users:
#         print("Thank you !" + user.title() + " .")
#     else:
#         print(user.title() + ",please take our poll !")

# alien_0 = {'color': 'green', 'point': '5'}
# alien_1 = {'color': 'yellow', 'point': '10'}
# alien_2 = {'color': 'red', 'point': '15'}
#
# aliens = [alien_2, alien_1, alien_0]
# for alien in aliens:
#     print(alien)

# aliens = []
#
# for alien_number in range(30):
#     new_alien = {'color': 'green', 'point': 5, 'speed': 'slow'}
#     aliens.append(new_alien)
#
# for alien in aliens[:3]:
#     if alien['color'] == 'green':
#         alien['color'] = 'yellow'
#         alien['speed'] = 'medium'
#         alien['points'] = 10
#
# for alien in aliens[:5]:
#     print(alien)
# print("...")
#
# print("Total number of aliens:" + str(len(aliens)))
#
# pizza = {
#     'crust': 'thick',
#     'topping': ['mushrooms', 'extra chess'],
# }
#
# print("You ordered a " + pizza['crust'] + "-crust pizza " +
#       "with the following topping:")
#
# for topping in pizza['topping']:
#     print("\t" + topping)

# import random
#
# # 获取用户输入
# start = int(input("请输入范围的起点："))
# end = int(input("请输入范围的终点："))
#
# # 生成随机数
# random_num = random.randint(start, end)
# # print(random_num)

favorite_languages = {
    'jen': ['python', 'ruby'],
    'sarah': ['c'],
    'edward': ['ruby', 'go'],
    'phil': ['python','haskell'],
}
for name, languages in favorite_languages.items():
    print("\n" + name.title() + "'s favorite languages are:")
    for language in languages:
        print("\t" + language.title())















