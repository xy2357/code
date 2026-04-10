# def recsum(x):
#     if x < 0:
#         return x
#     else:
#         return recsum(x-1) + recsum(x-2)
    
# print(recsum(3))

# def recsum(x):
#     if not x:
#         return 0
#     else:
#         return x[0] + recsum(x[1:])
# print(recsum([1, 2, 3, 4, 5]))

# x = [1, 2, 3, 4]
# print(x[3:])

# def sumtree(x):
#     total = 0
#     for i in x:
#         if type(i) is not list:
#                 total = total + i
#         else:
#             total = total + sumtree(i)
#     return total

# print(sumtree([1, 2, 3, [4, 5, [6, 7, 8, [9, 10]]]]))

# import time

# def fib(n):
#     a = 0
#     b = 1
#     for i in range(n):
#         # a = b
#         # b = a + b
#         a, b = b, a + b
#     return a
# start1_time = time.time()
# print(fib(40))
# end1_time = time.time()
# print(f"{end1_time-start1_time}")

# def fibRecur(n):
#     if n == 1 or n == 2:
#         return 1
#     else:
#         return fibRecur(n-1) + fibRecur(n-2)
# start1_time = time.time()
# print(fibRecur(40))
# end1_time = time.time()
# print(f"{end1_time-start1_time}")

# def tailFibRecur(n, x, y):
#     if n == 1 or n == 2:
#         return y
#     else:
#         return tailFibRecur(n-1, y, x+y)

# start1_time = time.time()
# print(tailFibRecur(40, 1, 1))
# end1_time = time.time()
# print(f"{end1_time-start1_time}")

import functools

# def power(base, exp):
#     return base ** exp

# square = functools.partial(power, exp=2)
# print(square(3))

# pr = functools.partial(print, sep="@", end="#")
# pr("I", "Love", "Myself!")

# def myfunc(func):
#     @functools.wraps(func)
#     def call_func():
#         func()
#     return call_func

# @myfunc
# def test():
#     print("test~")

# test()
# print(test.__name__)

# class C:
#     pass
# c = C()

# class C:
#     x = 250
#     def get_x(self):
#         return self.x
    
# c = C()
# print(c.get_x())

# d = C()
# d.x = 520
# print(d.get_x())


# class Person:
#     name = "Li"
#     age = 18

#     def set_name(self):
#         self.name = input("请输入你的名字：")
#     def set_age(self):
#         self.age = input("请输入你的年龄：")
#     def get_name(self):
#         return self.name
#     def get_age(self):
#         return self.age

# c = Person()
# c.set_name()
# c.set_age()
# print(c.get_name())
# print(c.get_age())

# class Rectangle:
#     length = None
#     width = None
    
#     def set_length(self):
#         self.length = float(input("请输入长："))
#     def set_width(self):
#         self.width = float(input("请输入宽："))
#     def get_perimeter(self):
#         if self.length == None:
#             self.set_length()
#         if self.width == None:
#             self.set_width()
#         return self.length * 2 + self.width * 2

#     def get_area(self):
#         if self.length == None:
#             self.set_length()
#         if self.width == None:
#             self.set_width()
#         return self.length * self.width

# c = Rectangle()
# c.set_width()
# print(c.get_area())
# print(c.get_perimeter())

# class A:
#     pass

# class B(A):
#     pass

# a = A()
# b = B()
# print(isinstance(a, A))
# print(isinstance(b, B))
# print(issubclass(B, A))

# class A:
#     x = 250
#     def hello(self):
#         print("你好，我是A")

# class B:
#     x = 520
#     def hello(self):
#         print("你好，我是B")

# class C(A, B):
#     pass

# c = C()
# print(c.x)
# c.hello()

# print(issubclass(bool, int))

# class C:
#     def myfunc(self):
#         print("C")

# class D:
#     x = 250
#     def myfunc(self):
#         print("D")

# class E(C, D):
#     x = 520

# e = E()
# print(e.x)
# e.myfunc()

# x = 1
# def test():
#     x = 2
#     print(x)

#     class C:
#         x = 3
#         print(x)

#         def m1(self):
#             print(x)
#             print(self.x)

#         def m2(self):
#             x = 4
#             print(x)
#             self.x = 5
        
#         def m3(self):
#             print(x)
#             print(self.x)

#     c = C()
#     c.m1()
#     c.m2()
#     c.m3()

# test()

# class A:
#     x = 250

# class B:
#     x = 520

# class C(B):
#     pass

# class D(A, B):
#     pass

# class F(D):
#     pass

# class G(D):
#     pass

# class E():
#     pass

# class F(D):
#     pass

# class G(D):
#     pass

# class H(E, F, G):
#     pass

# h = H()
# print(h.x)

class C:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def add(self):
        return self.x + self.y

    def mul(self):
        return self.x * self.y
    
c = C(2, 3)
# print(c.add())
# print(c.mul())

# class C:
#     x = None
#     y = None

#     def set(self, x, y):
#         self.x = x
#         self.y = y

#     def add(self):
#         return self.x + self.y

#     def mul(self):
#         return self.x * self.y
    
# c = C()
# c.set(2, 3)
# print(c.add())
# print(c.mul())

# class D(C):
#     def __init__(self, x, y, z):
#         C.__init__(self, x, y)
#         self.z = z

#     def add(self):
#         return self.x * self.y + self.z
    
#     def mul(self):
#         return C.mul(self) * self.z + self.x

# d = D(3, 4, 5)
# print(d.mul())

# class A:
#     def __init__(self):
#         print("A")

# class B1(A):
#     def __init__(self):
#         A.__init__(self)
#         print("B1")

# class B2(A):
#     def __init__(self):
#         A.__init__(self)
#         print("B2")

# class C(B1, B2):
#     def __init__(self):
#         super().__init__()
#         B2.__init__(self)
#         print("C")

# c = C()
# print(C.mro())
# print(C.__mro__)

# class A:
#     pass

# class B1(A):
#     pass

# class B2:
#     pass

# class C(B1, B2):
#     pass

# c = C()
# print(C.mro())

# class A:
#     def say(self):
#         print("A")

# class B:
#     def say(self):
#         print("B")

# class C(A, B):
#     def say(self):
#         super().say()

# c = C()
# c.say()

# x = 1 
# def test(): 
#     x = 2 
#     print(x)

#     class C: 
#         x = 3 
#         print(x)

#         def m1(self): 
#             print(x) 
#             print(self.x)

#         def m2(self): 
#             x = 4 
#             print(x) 
#             self.x = 5

#         def m3(self): 
#                 print(x) 
#                 print(self.x)

#     c = C() 
#     c.m1() 
#     c.m2() 
#     c.m3()

# test()

# class A:
#     def __init__(self):
#         self.x = 1
#     def __add__(self,other):
#         print("a")

# a = A()
# # b = A()
# # a + 1
# # print(a.x)
# print(a.x + a.x)
# a + a

# from pathlib import Path

# class Displayer:
#     def display(self, message):
#         print(message)

# class LoggerMixin:
#     def log(self, message, filename="logfile.txt"):
#         with open (filename, "a") as f:
#             f.write(message)

#     def display(self, message):
#         super().display(message)
#         self.log(message)

# class MySubClass(LoggerMixin, Displayer):
#     def log(self, message):
#         super().log(message, filename="subclasslog.txt")

# # subclass = MySubClass()
# # subclass.display("This is a test.")
# # print("当前工作目录:", Path.cwd())
# logger = LoggerMixin()
# logger.log("Hello")

# class A:
#     def say_hi(self):
#         print("hi from A")

#     def say(self):
#         super().say_hi()

# class B1(A):
#     def say_hi(self):
#         print("Hi from B1")

#     def say(self):
#         super().say()

# class B2:
#     def say_hi(self):
#         print("Hi from B2")

#     def say(self):
#         super().say()

# class C(B1, B2):
#     def say(self):
#         super().say()

# c = C()
# c.say()
# print(C.mro())

# class A():
#     def say():
#         print("A")

# class B1(A):
#     def say():
#         print("B1")

# class B2(A):
#     def say():
#         print("B2")

# class C(B1, B2):
#     B1.say()
#     B2.say()
#     print("C")

# c = C()

# class A:
#     def __init__(self):
#         print("A")

# class B1(A):
#     def __init__(self):
#         A.__init__(self)
#         print("B1")

# class B2(A):
#     def __init__(self):
#         A.__init__(self)
#         print("B2")
    
# class C(B1, B2):
#     def __init__(self):
#         B1.__init__(self)
#         B2.__init__(self)
#         print("C")

# c = C()

# class A:
#     def say(self):
#         super().say()
#         print("A")

# class B():
#     def say(self):
#         print("B")

# class C(A):
#     def say(self):
#         super().say()
#         print("C")

# class D(B):
#     def say(self):
#         super().say()
#         print("D")

# class E(C, D):
#     def say(self):
#         super().say()
#         print("E")

# e = E()
# print(E.mro())
# e.say()


# import time
# import hashlib

# class Member:
#     def __init__(self, id, name, password, point, regdate):
#         self.id = id
#         self.name = name
#         self.password = password
#         self.point = point
#         self.regdate = regdate

# class PasswordMixin:
#     def is_tooshort(self,password,require=6):
#         if len(password) < require:
#             password = input("密码长度不能小于6位，请重新输入：")
#         return password
    
#     def to_md5(self,password):
#         bstr = bytes(password, "utf-8")
#         password = hashlib.md5(bstr).hexdigest()
#         return password

# class LoggerMixin():
#     def log(self,message,filename="log.txt"):
#         with open (filename, "a") as f:
#             f.write(message)

# class Manage(PasswordMixin, LoggerMixin):
#     def __init__(self):
#         self.members = {}
#         self.id = 10000

#     def welcome(self):
#         ins = 0
#         print("欢迎来到超市管理系统：")
#         while ins != "5":
#             ins = input("\n1.创建新卡;2.修改密码;3.商品支付;4.积分查询;5.退出程序：")
#             if ins == "1":
#                 self.create_member()
#             if ins == "2":
#                 self.modify_password()
#             if ins == "3":
#                 self.pay_money()
#             if ins == "4":
#                 self.view_point()
#             if ins == "5":
#                 print("感谢使用！")

#     def confirm_password(self):
#         id = int(input("请输入卡号："))
#         while not self.members.get(id):
#             id = int(input("请输入卡号："))

#         password = input("请输入密码")
#         password = self.to_md5(password)
#         while not self.members.get(id).password == password:
#             password = input("请输入密码")
#             password = self.to_md5(password)

#         return id

        
#     def create_member(self):
#         name = input("请输入名字：")
#         password = input("请输入密码：")
#         password = self.is_tooshort(password)
#         password = self.to_md5(password)
#         point = 0
#         regdate = time.localtime()
#         member = Member(self.id, name, password, point, regdate)
#         self.members[self.id] = member
#         print(f"创建成功，卡号为{self.id},关联用户{name}")
#         self.log(f"开卡成功：{self.id} -> {name},时间：{time.strftime('%Y-%m-%d %H:%M:%S',regdate)}\n")
#         self.id += 1

#     def modify_password(self):
#         id = self.confirm_password()
#         newpassword = input("请输入新密码：")
#         newpassword = self.is_tooshort(newpassword)
#         newpassword = self.to_md5(newpassword)
#         self.members[id].password = newpassword
#         self.log(f"修改密码：卡号 -> {self.id},时间：{time.strftime('%Y-%m-%d %H:%M:%S',time.localtime())}\n")

#     def pay_money(self):
#         id = self.confirm_password()
#         money = int(input("请输入支付金额："))
#         self.members[id].point += money
#         self.log(f"积分累计：卡号 -> {self.id},时间：{time.strftime('%Y-%m-%d %H:%M:%S',time.localtime())}\n")

#     def view_point(self):
#         id = self.confirm_password()
#         print(f"卡号 -> {self.id}当前积分为：{self.members[id].point}")

# def main():
#     m = Manage()
#     m.welcome()

# # main()
# print(Manage.mro())

# class A:
#     print("A")

# class B:
#     a =A()

# b =B()

# class Shape:
#     def __init__(self, name):
#         self.name = name
#     def area(self):
#         pass

# class Square(Shape):
#     def __init__(self, length):
#         super().__init__("正方形")
#         self.length = length
#     def area(self):
#         return self.length * self.length
    
# s = Square(5,8)
# print(s.area())
# print(s.id)
# print(s.name)
    
# class Cat:
#     def __init__(self, name, age):
#         self.name = name
#         self.age = age
#     def intro(self):
#         print(f"我是一只沙雕猫咪，我叫{self.name}，今年{self.age}岁~")
#     def say(self):
#         print("mua~")

# class Dog:
#     def __init__(self, name, age):
#         self.name = name
#         self.age = age
#     def intro(self):
#         print(f"我是一只小狗，我叫{self.name}，今年{self.age}岁~")
#     def say(self):
#         print("哟吼~")

# class Pig:
#     def __init__(self, name, age):
#         self.name = name
#         self.age = age
#     def intro(self):
#         print(f"我是一只小猪，我叫{self.name}，今年{self.age}岁~")
#     def say(self):
#         print("oink~") # 不要问我猪为什么这么叫，我是跟隔壁幼儿园的小朋友学的，oink~

# c = Cat("web", 4)
# d = Dog("布布", 7)
# p = Pig("大肠", 5)
# def animal(x):
#     x.intro()
#     x.say()
    
# class Member:
#     def __init__(self, name, job, grade, year, uid):
#         self.name =name
#         self.job = job
#         self.grade = grade
#         self.year = year
#         self.uid = uid
        
#     def get_uid(self):
#         return self.uid
    
#     def get_name(self):
#         return self.name
    
#     def get_job(self):
#         return self.job
    
#     def get_grade(self):
#         return self.grade
    
#     def get_year(self):
#         return self.year
    
#     def salary(self):
#         return 3000 + 500 * self.grade + 50 * self.year
    
# class Teamleader(Member):
#     def salary(self):
#         return 4000 + 800 * self.grade + 100 * self.year
    
# class Manager(Member):
#     def salary(self):
#         return 5000 + 1000 * (self.grade + self.year)



# e = Employee(1, 2, 3, 4, 5)


# class C:
#     def __init__(self, x):
#         self.__x = x

#     def set_x(self, x):
#         self.__x = x

#     def get_x(self):
#         print(self.__x)

# c = C(1)
# d = C(2)
# d.set_x(3)
# print(c._C__x)
# print(d._C__x)
# print(d.__dict__)

# class A:
#     __slots__ = ['__x', 'y']
#     def __init__(self, x, y):
#         self.__x = x
#         self.y = y

# class B(A):
#     pass
# a = A(1,2)
# b = B(3,4)
# # a.__x = 5
# a.y = 10
# # print(a.__x)
# print(b.y)
# # print(a.__dict__)

# class C:
#     __FishC = 250

# c = C()
# print(c._C__FishC)

# class C:
#     __slots__ = ['x']

# c = C()
# c.x = 250
# print(c.x)
# print(c.__dict__)
# print(c.__slots__)
# print(c.__tupe__)

# class C:
#     __slots__ = ['x', 'y']
#     def __init__(self, x, y):
#         self.x = x
#         self.y = y

# class D(C):
#     def __init__(self, x, y, z):
#         super().__init__(x, y)
#         self.z = z

# d = D(2,3,4)
# print(d.__slots__)
# print(d.__dict__)

# class C:
#     __slots__ = ["x", "y"]

# class D(C):
#     __slots__ = ["x", "y"]
#     def __init__(self, x, y):
#         self.x = x
#         self.y = y

# d = D(3,4)
# d.z = 5
# # print(c.__dict__)
# print(dir(d))

class Car:
    def __init__(self, brand, model, platenum, dayrent, carid):
        self.brand = brand
        self.model = model
        self.platenum = platenum
        self.dayrent = dayrent
        self.carid = carid

    def get_brand(self):
        return self.brand
    
    def get_model(self):
        return self.model
    
    def get_platenum(self):
        return self.platenum
    
    def get_dayrent(self):
        return self.dayrent
    
    def get_carid(self):
        return self.carid
    
    def calc_rent(self, days, discount):
        return (days * self.dayrent) * discount
    
class EconomyCar(Car):
    def __init__(self, brand, model, platenum, dayrent, carid, allowance):
        super().__init__(brand, model, platenum, dayrent, carid)
        self.allowance = allowance

    def clac_rent(self,days, discount):
        return (days * self.dayrent) * discount - self.allowance
    
class LuxuryCar(Car):
    def __init__(self, brand, model, platenum, dayrent, carid, insurancecost):
        super().__init__(brand, model, platenum, dayrent, carid)
        self.insurancecost = insurancecost

    def calc_rent(self, days, discount):
        return (days * self.dayrent) * discount + self.insurancecost

class SportCar(Car):
    def __init__(self, brand, model, platenum, dayrent, carid, losscost):
        super().__init__(brand, model, platenum, dayrent, carid)
        self.losscost = losscost

    def calc_rent(self, days, discount):
        return (days * self.dayrent) * discount + self.losscost

class SUV(Car):
    def __init__(self, brand, model, platenum, dayrent, carid):
        super().__init__(brand, model, platenum, dayrent, carid)
    
    def calc_rent(self, days, discount):
        while days > 7:
            return ((days * self.dayrent) * discount) * 0.7
        else:
            return (days * self.dayrent) * discount

class CarOperation:
    cars = {}
    stocks = {'EconomyCar': [], 'LuxuryCar': [], 'SportCar': [], 'SUV': []}

    def operate(self):
        id = 10000
        while True:
            ins = int(input(f"\n1.录入汽车，2.获取库存，3.租车服务，4.还车服务，5.退出程序："))
            # if ins not in [1, 2, 3, 4, 5]:
            #     ins = int(input(f"\n1.录入汽车，2.获取库存，3.租车服务，4.还车服务，5.退出程序："))
            #     continue
            if ins == 1:
                classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
                # while classic not in [1, 2, 3, 4]:
                #     classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
                brand = input(f"\n品牌：")
                model = input(f"\n型号：")
                platenum = input(f"\n车牌：")
                dayrent = int(input(f"\n每日租金："))
                carid = id
                id += 1

                if classic == 1:
                    car = EconomyCar(brand, model, platenum, dayrent, carid)
                    self.cars[carid] = car
                    self.stocks['EconomyCar'].append(carid)
                if classic == 2:
                    car = LuxuryCar(brand, model, platenum, dayrent, carid)
                    self.cars[carid] = car
                    self.stocks['LuxuryCar'].append(carid)
                if classic == 3:
                    car = SportCar(brand, model, platenum, dayrent, carid)
                    self.cars[carid] = car
                    self.stocks['SportCar'].append(carid)
                if classic == 4:
                    car = SUV(brand, model, platenum, dayrent, carid)
                    self.cars[carid] = car
                    self.stocks['SUV'].append(carid)

            if ins == 2:
                classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
                if classic == 1:
                    if len(self.stocks['EconomyCar']) != 0:
                        print(f"经济车型数量：{len(self.stocks['EconomyCar'])}")
                        for each in self.stocks['EconomyCar']:
                            print(f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
                    else:
                        print(f"经济车型数量为0")
                
                if classic == 2:
                    if len(self.stocks['LuxuryCar']) != 0:
                        print(f"豪华车型数量：{len(self.stocks['LuxuryCar'])}")
                        for each in self.stocks['LuxuryCar']:
                            print(f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
                    else:
                        print(f"豪华车型数量为0")
                
                if classic == 3:
                    if len(self.stocks['SportCar']) != 0:
                        print(f"跑车型数量：{len(self.stocks['SportCar'])}")
                        for each in self.stocks['SportCar']:
                            print(f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
                    else:
                        print(f"跑车型数量为0")
                
                if classic == 4:
                    if len(self.stocks['SUV']) != 0:
                        print(f"SUV型数量：{len(self.stocks['SUV'])}")
                        for each in self.stocks['SUV']:
                            print(f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
                    else:
                        print(f"SUV数量为0")

            if ins == 3:
                id = int(input(f"\n请输入要租车辆的id："))
                if id in self.cars:
                    days = int(input(f"\n请输入要租的天数："))
                    print(f"租车费用为：{self.cars[id].calc_rent(days, discount = 0.9)}")





c = CarOperation()
c.operate()