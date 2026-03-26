cities = {
    'beijing': {
        'country': 'China',
        'population': '10',
        'fact': '1',
    },
    'shanghai': {
        'country': 'China',
        'population': '20',
        'fact': '2',
    },
    'tokyo':{
        'country': 'Japan',
        'population': '30',
        'fact': '3',
    }
}
for city, message in cities.items():
    print("city: " + city)
    print(message['country'])
    print(message['population'])
    print(message['fact'])