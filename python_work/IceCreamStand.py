class Restaurant:
    """定义一个餐馆类"""

    def __init__(self, restaurant_name, cuisine_type):
        """初始化name和type"""
        self.restaurant_name = restaurant_name
        self.cuisine_type = cuisine_type
        self.number_served = 0

    """介绍餐馆"""

    def describe(self):
        print("餐馆的名字：" + self.restaurant_name.title())
        print("餐馆菜的类型：" + self.cuisine_type)

    """指出餐馆正在营业"""

    def open(self):
        print(self.restaurant_name + "餐馆正在营业．")

    def set_number_served(self, number):
        """设置人数"""
        if number >= 0:
            self.number_served = number
            print("请输入就餐人数：" + str(self.number_served))
        else:
            print("不能输入负值！")

    def increment_number_served(self, increment_number):
        """修改人数"""
        if abs(increment_number) >= self.number_served and increment_number < 0:
            print("请输入正确的数值！")
        else:
            self.number_served += increment_number
            print("请输入修改后的人数：" + str(self.number_served))


class IceCreamStand(Restaurant):

    def __init__(self, restaurant_name, cuisine_type, flavors):
        super().__init__(restaurant_name, cuisine_type)
        self.flavors = flavors
        print(self.restaurant_name.title() + "' cuisine type is " + self.cuisine_type.title() + " that flavours is " + self.flavors)

