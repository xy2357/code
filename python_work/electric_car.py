class Car:

    def __init__(self, make, model, year):
        """初始化汽车的属性"""
        self.make = make
        self.model = model
        self.year = year
        self.odometer_reading = 0

    def get_descriptive_name(self):
        """整洁的描述信息"""
        long_name = str(self.year) + ' ' + self.make + ' ' + self.model
        return long_name.title()

    def read_odometer(self):
        """打印一条指出汽车里程"""
        print("This car has " + str(self.odometer_reading) + " miles on it.")

    def update_odometer(self, mileage):
        """将里程数换成设置的
            禁止回调数值
        """
        if mileage >= self.odometer_reading:
            self.odometer_reading = mileage
        else:
            print("不能回调数值")

    def increment_odometer(self, miles):
        """将里程增加指定数值"""
        if miles >= 0:
            self.odometer_reading += miles
        else:
            print("不能输入负数")

    def fill_gas_tank(self):
        print("请加注油箱！")


class Battery:
    """对电动车电池的类定义"""

    def __init__(self, battery_size = 70):
        """初始化电动车电池的属性"""
        self.battery_size = battery_size

    def describe_battery_size(self):
        """打印电池的属性"""
        print("你的电池容量为：" + str(self.battery_size))

    def get_ranges(self):
        """指出电瓶的续航"""
        ranges = self.battery_size / 10

        message = "This car can go approximately " + str(ranges)
        message += " mile on a full charge!"
        print(message)

    def upgrade_battery(self):
        """升级电瓶"""
        if self.battery_size != 75:
            self.battery_size = 85
        else:
            self.battery_size = self.battery_size

class ElectricCar(Car):

    def __init__(self, make, model, age):
        super().__init__(make, model, age)
        self.battery = Battery()

    # def describe_battery_size(self):
    #     print("你的电池容量为：" + str(self.battery_size))

    def fill_gas_tank(self):
        print("这个车不需要加注油箱！")


my_tesla = ElectricCar('tesla', 'model s', 2016)
print(my_tesla.get_descriptive_name())
my_tesla.battery.describe_battery_size()
my_tesla.fill_gas_tank()
my_tesla.battery.get_ranges()
my_tesla.battery.upgrade_battery()
my_tesla.battery.get_ranges()