class Restaurant:
    def __init__(self, restaurant_name, cuisine_type):
        """初始化属性"""
        self.restaurant_name = restaurant_name
        self.cuisine_type = cuisine_type
        self.number_served = 0

    def describe_restaurant(self):
        print("The restaurant name is " + self.restaurant_name.title() + ".")
        print("The restaurant cuisine type is " + self.cuisine_type.title() + ".")
        print("The restaurant has " + str(self.number_served) + " people!")

    def open_restaurant(self):
        print(self.restaurant_name.title() + " is opening.")

    def set_number_served(self, number_served):
        self.number_served = number_served

class IceCreamStand(Restaurant):

    def __init__(self, restaurant_name, cuisine_type, flavors):
        super().__init__(restaurant_name, cuisine_type)
        self.flavors = flavors
        print(self.restaurant_name.title() + "' cuisine type is " + self.cuisine_type.title() + " that flavours is " + self.flavors)


my_restaurant = IceCreamStand('Tongfu', 'doufu', 'sweet')
