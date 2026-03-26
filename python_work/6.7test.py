people_1 = {'first_name': 'li', 'last_name': 'si', 'age': '10', 'city': 'shanghai'}
people_2 = {'first_name': 'wang', 'last_name': 'wu', 'age': '11', 'city': 'beijing'}
people_3 = {'first_name': 'zhang', 'last_name': 'liu', 'age': '12', 'city': 'hangzhou'}

peoples = [people_1, people_2, people_3]

for people in peoples:
       print(people['first_name'] + people['last_name'] + "'s age is: " + people['age'])