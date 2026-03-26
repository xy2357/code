# def make_pizza(size, *toppings):
#     """概述要制作的披萨"""
#     print('\nMaking a ' + size + '-inch pizza with the following toppings')
#     for topping in toppings:
#         print('-' + topping)
#
#
# make_pizza('16', 'pepperoni', 'mushroom')

def build_profile(first, last, **user_info):
    """创建一个字典"""
    profile = {'first name': first, 'last name': last}
    for key, value in user_info.items():
        profile[key] = value
    return profile


user_profile = build_profile('albert', 'einstein', location='princeton', filed='physics')
print(user_profile)
