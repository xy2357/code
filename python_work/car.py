class Car:

    def __init__(self, make, model, year):
        """初始化汽车的属性"""
        self.make = make
        self.model = model
        self.year = year
        self.odometer_reading = 0

    def get_descriptive_name(self):
        """整洁的描述信息"""
        long_name = self.year + ' ' + self.make + ' ' + self.model
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


my_new_car = Car('audi', 'a4', '2016')
print(my_new_car.get_descriptive_name())
my_new_car.update_odometer(24)
my_new_car.update_odometer(20)
my_new_car.increment_odometer(-100)
my_new_car.read_odometer()
my_new_car.fill_gas_tank()


