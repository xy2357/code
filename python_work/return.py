# def get_formatted_name(first_name, last_name):
#     """返回整洁的名字"""
#     full_name = first_name + ' ' + last_name
#     return full_name.title()
#
#
# musician = get_formatted_name('jimi', 'hendrix')
#
# print(musician)
import magicians


# def get_formatted_name(first_name, last_name, middle_name=''):
#     """返回整洁的名字"""
#     if middle_name:
#         full_name = first_name + ' ' + middle_name + ' ' + last_name
#     else:
#         full_name = first_name + ' ' + last_name
#     return full_name.title()
#
#
# musician = get_formatted_name('jimi', 'hendrix')
# print(musician)
#
# musician = get_formatted_name('jimi', 'hooker', 'hendrix')
# print(musician)


# def build_person(first_name, last_name, age=''):
#     """返回一个字典"""
#     person = {'first': first_name, 'last': last_name}
#     if age:
#         person['age'] = age
#     return person
#
#
# musician = build_person('jimi', 'hendrix', age='27')
# print(musician)

# def get_formatted_name(first_name, last_name):
#     """返回整洁的姓名"""
#     full_name = first_name + ' ' + last_name
#     return  full_name
#
# while True:
#     print("\n请输入你的名字！")
#     f_name = input('请输入你的名：')
#     if f_name == 'q':
#         break
#     l_name = input('请输入你的姓：')
#     if l_name == 'q':
#         break
#
#     formatted_name = get_formatted_name(f_name, l_name)
#     print(formatted_name)

# def city_country(city, country):
#
#     full_city_country = city + ', ' + country
#     return full_city_country
#
# while True:
#     i_city = input('请输入城市：')
#     if i_city == 'q':
#         break
#
#     i_country = input('请输入国家：')
#     if i_country == 'q':
#         break
#
#     city_and_country = city_country(i_city, i_country)
#     print(city_and_country)

# def greet_users(names):
#     """向列表中的每位用户都发出简单的问候"""
#     for name in names:
#         msg = "Hello, " + name.title() + "!"
#         print(msg)
#
#
# usernames = ['hannah', 'ty', 'margot']
# greet_users(usernames)


# def print_models(unprinted_designs, completed_models):
#     """
#     模拟打印每个设计，直到没有未打印的设计为止
#     打印每个设计后都将其移动到completed_models中
#     """
#     while unprinted_designs:
#         current_design = unprinted_designs.pop()
#
#         print("Printing model: " + current_design)
#         completed_models.append(current_design)
#
#
# def show_completed_models(completed_model):
#     """显示打印好的所有模型"""
#     print("\nThe following models have been printed:")
#     for completed_model in completed_models:
#         print(completed_model)
#
#
# unprinted_designs = ['iphone case', 'robot pendant', 'dodecahedron']
# completed_models = []
#
# print_models(unprinted_designs[:], completed_models)
# show_completed_models(completed_models)
# print(unprinted_designs)
# print(completed_models)

# magicians = ['li', 'wang']
#
#
# def show_magicians(magician):
#     for magician in magicians:
#         print(magician)
#
#
# show_magicians(magicians)

# now_magicians = ['li', 'wang']
# modified_magicians = now_magicians[:]
#
#
# def make_great(magicians_member):
#     modified_magicians = magicians_member[:]  # 使用切片法创建副本
#     for i in range(len(modified_magicians)):
#         modified_magicians[i] += ' the Great'
#     return modified_magicians
#
#
# def show_magicians(magicians_member):
#     for magician in magicians_member:
#         print(magician)
#
#
# magicians_list = ['Harry', 'David']
#
# """调用原始的魔术师列表"""
# print('\nOriginal Magicians')
# show_magicians(magicians_list)
#
# """调用make_great对列表进行修改"""
# modified_list = make_great(magicians_list)
#
# """打印修改之后的魔术师列表"""
# print('\nModified Magicians')
# show_magicians(modified_list)

def add_numbers(a, b):
    """计算两个数字的和并返回结果"""
    result = a + b
    return result


# 调用函数并接收返回值
sum_result = add_numbers(3, 4)
print(sum_result)  # 输出：7
