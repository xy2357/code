from os import remove
from pathlib import Path
# from timeit import reindent

import pandas as pd
from functools import wraps
#
# ROOT = Path(__file__).resolve().parents[1]   # 指向 excel_py/
# in_file = ROOT / "data" / "input.xlsx"
# out_file = ROOT / "output" / "output.xlsx"
#
# df = pd.read_excel(in_file)
# df.to_excel(out_file, index=False)
from numpy.ma.core import inner, append

# class Power:
#     def __init__(self, exp):
#         self.exp = exp
#     def __call__(self, base):
#         return base ** self.exp
#
# square = Power(2)
# print(square(3))

# class C:
#     def __str__(self):
#         return "C"
#
# c = C()
# print(repr(c))

# class D:
#     def __init__(self):
#         self._X = 250
#     def __getattr__(self, name):
#         if name == "X":
#             return self._X
#         else:
#             return super().__getattr__(name)
#     def __setattr__(self, name, value):
#         if name == "X":
#             super().__setattr__("_X", value)
#         else:
#             super().__setattr__(name, value)
#     def __delattr__(self, name):
#         if name == "X":
#             super().__delattr__("_X")
#         else:
#             super().__delattr__(name)
#
# d = D()
# print(d.X)

# class E:
#     def __init__(self):
#         self._x = 250
#     @property
#     def x(self):
#         return self._x
#
#
# e = E()
# print(e.x)

# def funA():
#     x = 520
#     def funB():
#         print(x)
#     return funB
#
# funny = funA()
# del funA
# funny()

# def outter():
#     # x = 880
#     def innerA():
#         x = 100
#
#     def innerB():
#         nonlocal x
#         x = 250
#
#     def innerC():
#         global x
#         x = 520
#     x = 880
#
#     innerA()
#     print(f"调用完innerA()函数之后，x = {x}")
#
#     innerB()
#     print(f"调用完innerB()函数之后，x = {x}")
#
#     innerC()
#     print(f"调用完innerC()函数之后，x = {x}")
#
# outter()
# print(f"此时此刻，全局变量x = {x}")

# def funA(a):
#     def funB(b):
#         def funC(c):
#             return a * b * c
#         return funC
#     return funB
#
# print(funA(3)(4)(5))

# def maker(n):
#     def action(x):
#         return x ** n
#     return action
#
# f = maker(2)
#
# print(f(3))
# print(f(5))

# maker = lambda n: (lambda x: x ** n)
# print(maker(2)(3))

# def make_avg():
#     sum = 0
#     count = 0
#     def avg(x):
#         nonlocal sum, count
#         sum = sum + x
#         count += 1
#         return sum / count
#     return avg
#
# avg = make_avg()
#
# print(avg(5))
# print(avg(3))
# print(avg(7))
# print(avg(19))

# def fib():
#     a = 0
#     b = 1
#     def add():
#         nonlocal a, b
#         a, b = b, a + b
#         print(a)
#     return add
#
# f = fib()
# f()
# print(fib()())
# print(f())
# print(f())
# print(f())
# print(f())
# print(f())
# print(f())
# print(f())
# print(f())
# print(f())

# def make_counter():
#     counter = 0
#     def add():
#         nonlocal counter
#         counter += 1
#         return counter
#     return add
#
# c = make_counter()
# print(c())
# print(c())
# print(c())
# print(c())
# print(c())

# def make_adder(a):
#     def add(b):
#         return a + b
#     return add
#
# add5 = make_adder(5)
# print(add5(10))
# print(add5(-2))

# def make_product():
#     x = 1
#     def multiply(y):
#         nonlocal x
#         x = y * x
#         return x
#     return multiply
#
# p = make_product()
# print(p(2))
# print(p(3))

# def make_moving_avg(k):
#     list = []
#     total = 0
#     def moving_avg(x):
#         nonlocal total
#         list.append(x)
#         total += x
#
#         if len(list) > k:
#             total -= list.pop(0)
#
#         return total / len(list)
#
#     return moving_avg
#
# ma = make_moving_avg(3)
# print(ma(10))
# print(ma(20))
# print(ma(30))
# print(ma(40))
# print(ma(50))
# print(ma(60))


# def add(func):
#     def inner():
#         x = func()
#         return x + 1
#     return inner
#
# def cube(func):
#     def inner():
#         x = func()
#         return x * x * x
#     return inner
#
# def square(func):
#     def inner():
#         x = func()
#         return x * x
#     return inner
#
# # @add
# # @cube
# # @square
# def num():
#     return 2
#
# num = square(num)
# print(num)
# print(inner)
# num = cube(num)
# print(num)
# print(inner)
# num = add(num)
# print(num)
# print(inner)
#
# print(num())

# 题1
# def dec(func):
#     def wrapper():
#         print("A")
#         r = func()
#         print("B")
#         return r
#     return wrapper
#
# # @dec
# def f():
#     print("F")
#     return 10
#
# f = dec(f)
#
# print(f())
# 输出结果是：
# A
# F
# B
# 10  //为什么会有个10？

# 题2
# def d1(func):
#     def w():
#         print("d1 in")
#         r = func()
#         print("d1 out")
#         return r
#     return w
#
# def d2(func):
#     def w():
#         print("d2 in")
#         r = func()
#         print("d2 out")
#         return r
#     return w
#
# # @d1
# # @d2
# def f():
#     print("F")
#
# f = d2(f)
# f = d1(f)
#
# f()

# 题目3
# def log_call(func):
#     # @wraps(func)
#     def wrapper(*args, **kwargs):
#         print(f"name:, {func.__name__}")
#         print("args", args)
#         print("kwargs", kwargs)
#         return func(*args, **kwargs)
#     return wrapper
#
# # @log_call
# def add(a, b, c=0):
#     return a + b + c
# add = log_call(add)
#
# print(add(1, 2, c=3))
# print(add.__name__)

# def fun(*args, **kwargs):
#     print(args)
#     print(kwargs)
#
# fun(1 ,2 ,3 ,x=4, y=5)

# def f(a, b, *args):
#     print(a, b, args)
#
# f(10, 20, 30 ,40)

# def f(*args, b=99):
#     print(args, b)
#
# f(1, 2, 3, b=5)

# def f(a, *args, **kwargs):
#     print(a)
#     print(args)
#     print(kwargs)
#
# f(1, 2, 3, x=4, y=5)

# def f(a, b, *, c, d=0):
#     print(a, b, c, d)
#
# f(1, 2, c=3, d=4)


# def counter():
#     i = 0
#     while i <= 5:
#         yield i
#         i += 1
#     return print("YES")
#
# for each in counter():
#     print(each)

# y = (x ** 2 for x in range(1000000))
# list1 = []
# for each in y:
#     list1.append(each)
# print(len(list1))

# list((abs(x) for x in (-1, -2, -3, 4, -5)))

import functools

from pandas.core.algorithms import value_counts_arraylike


# def add(x, y):
#     return x + y
# print(functools.reduce(add, [1, 2, 3, 4, 5]))

# print(functools.reduce(lambda x,y:x*y,range(1,11)))

# class Bird():
#     pass
#
# class Peacock(Bird):
#     def say(self):
#         print("孔雀开屏了！")
#
# class Swan(Bird):
#     def say(self):
#         print("天鹅在游泳！")
#
# class Myna(Bird):
#     def say(self):
#         print("八哥在说话！")
#
# class Cat():
#     pass
#
# class Lion(Cat):
#     def say(self):
#         print("狮子在吼叫！")
#
# class Tiger(Cat):
#     def say(self):
#         print("老虎在咆哮！")
#
# class Leopard(Cat):
#     def say(self):
#         print("豹子在奔跑！")
#
# class Primate():
#     pass
#
# class Monkey(Primate):
#     def say(self):
#         print("猴子在吃香蕉！")
#
# class Chimpanzee(Primate):
#     def say(self):
#         print("黑猩猩在吃打架！")
#
# class Baboon(Primate):
#     def say(self):
#         print("狒狒在吃草！")
#
#
# class Zoo:
#     p1 = Peacock()
#     s1 = Swan()
#     m1 = Myna()
#     l1 = Lion()
#     t1 = Tiger()
#     le1 = Leopard()
#     mo1 = Monkey()
#     c1 = Chimpanzee()
#     b1 = Baboon()
#
#     def say(self):
#         self.p1.say()
#         self.s1.say()
#         self.m1.say()
#         self.l1.say()
#         self.t1.say()
#         self.le1.say()
#         self.mo1.say()
#         self.c1.say()
#         self.b1.say()
#
# z = Zoo()
# z.say()

# class C:
#     x = 100
#     def set_x(self, v):
#         self.x = v
#
# c1 = C()
# c2 = C()
# print(c1.x)
# c2.set_x(2)
# print(c1.x)
# print(c2.x)

# class C:
#     def hello(self):
#         print("Hello, Python!")
#
# c = C()
# def instance_hello():
#     print("Hello, C#!")
# c.hello = instance_hello
# c.hello()
# C.hello(c)
# x = 99
# class C:
#     x = 100
#     def get_x(self):
#         print(x)
#
# c = C()
# c.x = 250
# c.get_x()

# class C:
#     def f(self):
#         print("hello")
#
# c = C()
# print(type(C.f) == type(c.f))

# class C:
#     def __init__(self):
#         self.x = 100
#     def set_x(self, x):
#         self.x = x
#
# c = C()
# d = C()
# c.set_x(250)
# d.set_x(520)
# print(c.x)
# print(d.x)

# class C:
#     def __init__(self):
#         self.x = []
#     def add_x(self, x):
#         self.x.append(x)
#
# c = C()
# d = C()
# c.add_x(250)
# d.add_x(520)
# print(c.x)
# print(d.x)

# class Meet:
#     pass
#
# class Egg(Meet):
#     name = "鸡蛋"
#     price = 1
#
# class Beef(Meet):
#     name = "牛肉"
#     price = 25
#
# class Mutton(Meet):
#     name = "羊肉"
#     price = 30
#
# class Vegetable:
#     pass
#
# class Onion(Vegetable):
#     name = "洋葱"
#     price = 2
#
# class Tomato(Vegetable):
#     name = "番茄"
#     price = 2
#
# class Potato(Vegetable):
#     name = "土豆"
#     price = 3
#
# class Radish(Vegetable):
#     name = "萝卜"
#     price = 3
#
# class Menu:
#     def order(self):
#         self.x = []
#         dishes = input("1.洋葱炒牛肉；2.洋葱炒羊肉；3.煎蛋；4.番茄炒蛋；5.土豆萝卜炖羊肉：")
#         dishes = dishes.split()
#
#         while dishes:
#             dish = dishes.pop(0)
#
#             if dish == "1":
#                 onion = Onion()
#                 onion.num = 1
#                 beef = Beef()
#                 beef.num = 1
#                 self.x.extend([onion, beef])
#
#             if dish == "2":
#                 onion = Onion()
#                 onion.num = 1
#                 mutton = Mutton()
#                 mutton.num = 1
#                 self.x.extend([onion, mutton])
#
#             if dish == "3":
#                 egg = Egg()
#                 egg.num = 2
#                 self.x.append(egg)
#
#             if dish == "4":
#                 tomato = Tomato()
#                 tomato.num = 2
#                 egg = Egg()
#                 egg.num = 3
#                 self.x.extend([tomato, egg])
#
#             if dish == "5":
#                 potato = Potato()
#                 potato.num = 2
#                 radish = Radish()
#                 radish.num = 1
#                 mutton = Mutton()
#                 mutton.num = 2
#                 self.x.extend([potato, radish, mutton])
#
#     def pay(self):
#         total = 0
#         for each in self.x:
#             print(each.name, each.price, "*", each.num)
#             total += each.price * each.num
#         print(f"总价是：{total}元")
#
# menu = Menu()
# menu.order()
# menu.pay()

# class C:
#     def __init__(self):
#         self.x = []
#     def add_x(self, x):
#         self.x.append(x)

# class A:
#     print("A")
#
# class B(A):
#     print("B")
#
# class C(B, A):
#     pass
#
# c = C()

# class A:
#     x = 250
#
# class B(A):
#     pass
#
# class C(B):
#     pass
#
# class D:
#     x = 520
#
# class E(C, D):
#     pass
#
# print(E.x)

# class A:
#     x = 250
#     def add_x(self):
#         self.x += 1
#
# class B(A):
#     x = 300
#     def add_x(self):
#         super().add_x()
#
# class C(A):
#     x = 500
#     def add_x(self):
#         super().add_x()
#
# class D(B,C):
#     pass
#
# d = D()
# d.add_x()
# print(d.x)
# print(D.mro())

# class A:
#     def ping(self):
#         print("A")
#
# class B(A):
#     def pong(self):
#         print("B")
#
# class C(A):
#     def pong(self):
#         print("C")
#
# class D(B,C):
#     def pingpong(self):
#         self.ping()
#         self.pong()
#
# d = D()
# d.pingpong()


# get_bases(tkinter.Text)
# print(tkinter.Text.mro())
# print(tkinter.Text.__bases__)
# a = 0
# b = 0
# while a < len(tkinter.Text.__bases__):
#         print(tkinter.Text.__bases__[a])
#         a += 1
    # else:
    #     if (len(tkinter.Text.__bases__[a][b]) > 1):
    #         print()
# import tkinter
#
# def get_bases(cls, count=0):
#     for i in range (count):
#         print("-", end=" ")
#     print(cls)
#
#     if cls.__bases__[0] is not object:
#         for each in cls.__bases__:
#             count += 1
#             get_bases(each, count)
#             count -= 1
#
# get_bases(tkinter.Text)

# from pathlib import Path
# from time import strftime, localtime
#
# class File:
#     def __init__(self, name, size, folder, ctime, mtime, atime):
#         self.name = name
#         self.size = size
#         self.folder = folder
#         self.ctime = ctime
#         self.mtime = mtime
#         self.atime = atime
#
#     def get_name(self):
#         return self.name
#
#     def get_size(self):
#         return f"{self.size}字节"
#
#     def get_folder(self):
#         return f"位置：{self.folder}"
#
#     def get_ctime(self):
#         return f"创建时间：{strftime('%Y-%m-%d %H:%M:%S', localtime(self.ctime))}"
#
#     def get_mtime(self):
#         return f"修改时间：{strftime('%Y-%m-%d %H:%M:%S', localtime(self.mtime))}"
#
#     def get_atime(self):
#         return f"访问时间：{strftime('%Y-%m-%d %H:%M:%S', localtime(self.atime))}"
#
# def get_file_msg(path):
#     p = Path(path)
#     paths = []
#     files = []
#
#     for each in p.glob("**/*"):
#         paths.append(each)
#         if each.is_file():
#             name = each.name
#             size = each.stat().st_size
#             folder = each.parent.resolve()
#             ctime = each.stat().st_ctime
#             mtime = each.stat().st_mtime
#             atime = each.stat().st_atime
#             files.append(File(name, size, folder, ctime, mtime, atime))
#
#     print("路径如下：")
#     for each in paths:
#         print(each)
#
#     return files
#
# def match_file(files):
#     count = 0
#     filename = input("\n请输入想要搜索的文件名：")
#
#     for each in files:
#         if filename in each.name:
#             count += 1
#             print(f"\n找到相关文件（{count} -> {each.get_name()}（{each.get_size()}））")
#             print(each.get_folder())
#             print(each.get_ctime())
#             print(each.get_mtime())
#             print(each.get_atime())
#         else:
#             print("找不到文件")
#
# files = get_file_msg("123")
# match_file(files)

# class Employee:
#     def __init__(self, name, job, grade, year, uid):
#         self.name = name
#         self.job = job
#         self.grade = grade
#         self.year = year
#         self.uid = uid
#
#     def get_uid(self):
#         return self.uid
#
#     def get_name(self):
#         return self.name
#
#     def get_job(self):
#         return self.job
#
#     def get_grade(self):
#         return self.grade
#
#     def set_grade(self, grade):
#         self.grade = grade
#
#     def get_year(self):
#         return self.year
#
#     def salary(self):
#         return 3000 + 500 * self.grade + 50 * self.year
#
#
# class Teamleader(Employee):
#     def salary(self):
#         return 4000 + 800 * self.grade + 100 * self.year
#
# class Manager(Employee):
#     def salary(self):
#         return 5000 + 1000 * (self.grade + self.year)
#
# def main():
#     mems = {}
#     jobs = {'E':[], 'T':[], 'M':[]}
#     uid = 10000
#     MAX_E_GRADE = 10
#     MAX_T_GRADE = 6
#     MAX_M_GRADE = 3
#
#     while True:
#         ins = input("\n1.录入;2.查询;3.升级;4.降级;5.退出：")
#
#         if ins == "1":
#             name = input("姓名：")
#             job = input("职位（E.普通员工;T.组长;M.经理）：")
#             year = int(input("工龄："))
#             grade = int(input("级别："))
#
#             if job == 'E':
#                 while grade > MAX_E_GRADE:
#                     grade = int(input(f"该职位最高级别为{MAX_E_GRADE}，请重新输入："))
#
#                 e = Employee(name, job, grade, year, uid)
#                 mems[uid] = e
#                 jobs['E'].append(e)
#
#             if job == 'T':
#                 while grade > MAX_T_GRADE:
#                     grade = int(input(f"该职位最高级别为{MAX_T_GRADE}，请重新录入级别："))
#
#                 e = Teamleader(name, job, grade, year, uid)
#                 mems[uid] = e
#                 jobs['T'].append(e)
#
#             if job == 'M':
#                 while grade > MAX_M_GRADE:
#                     grade = int(input(f"该职位最高级别为{MAX_M_GRADE}，请重新录入级别："))
#
#                 e = Manager(name, job, grade, year, uid)
#                 mems[uid] = e
#                 jobs['M'].append(e)
#
#             print(f"录入成功！姓名：{name}，工号：{uid}，薪资：{e.salary()}")
#         uid += 1
#
#         if ins == '2':
#             op = input("1.员工查询；2.职位查询：")
#
#             if op == "1":
#                 uid = int(input("请输入工号！"))
#
#                 if mems.get(uid):
#                     e = mems[uid]
#                     print(f"姓名：{e.get_name()}")
#                     print(f"职位：{e.get_job()}")
#                     print(f"级别：{e.get_grade()}")
#                     print(f"工龄：{e.get_year()}")
#                     print(f"薪资：{e.salary()}")
#                 else:
#                     print("该工号不存在！")
#
#             if op == "2":
#                 job = input("职位（E.普通员工;T.组长;M.经理）:")
#
#                 if job == 'E':
#                     if jobs['E']:
#                         print(f"目前普通员工有{len(jobs['E'])}人：")
#                         for each in jobs['E']:
#                             print(f"{each.get_uid()} -> {each.get_name()}")
#                     else:
#                         print("目前没有普通员工")
#
#                 if job == 'T':
#                     if jobs['T']:
#                         print(f"目前普通员工有{len(jobs['T'])}人：")
#                         for each in jobs['T']:
#                             print(f"{each.get_uid()} -> {each.get_name()}")
#                     else:
#                         print("目前没有组长")
#
#                 if job == 'M':
#                     if jobs['M']:
#                         print(f"目前普通员工有{len(jobs['M'])}人：")
#                         for each in jobs['M']:
#                             print(f"{each.get_uid()} -> {each.get_name()}")
#                     else:
#                         print("目前没有经理")
#
#         if ins == '3':
#             uid = int(input("请输入工号："))
#
#             if mems.get(uid):
#                 e = mems[uid]
#                 print(f"姓名：{e.get_name()}，职位：{e.get_job()}，级别：{e.get_grade()}，工龄：{e.get_year()}，薪资：{e.salary()}")
#
#                 name = e.get_name()
#                 job = e.get_job()
#                 year = e.get_year()
#                 grade = e.get_grade()
#                 old_salary = e.salary()
#                 n = int(input("请输入升级的级别："))
#
#                 if job == 'E':
#                     if grade + n > MAX_E_GRADE:
#                         job = 'T'
#                         grade = 1
#                         jobs['E'].remove(e)
#                         e = Teamleader(name, job, grade, year, uid)
#                         jobs['T'].append(e)
#                         mems[uid] = e
#                     else:
#                         e.set_grade(grade + n)
#
#                 elif job == 'T':
#                     if grade + n > MAX_T_GRADE:
#                         job = 'M'
#                         grade = 1
#                         jobs['T'].remove(e)
#                         e = Teamleader(name, job, grade, year, uid)
#                         jobs['M'].append(e)
#                         mems[uid] = e
#                     else:
#                         e.set_grade(grade + n)
#
#                 elif job == 'M':
#                     if grade + n > MAX_M_GRADE:
#                         print("已达最高级别！")
#                         grade = MAX_M_GRADE
#                     else:
#                         e.set_grade(grade + n)
#
#                 new_salary = e.salary()
#                 print("升级成功！")
#                 print(
#                     f"{e.get_name()}，工号：{e.get_uid()}，升级后职位：{e.get_job()}{e.get_grade()}，升级后薪资：{e.salary()}(+{new_salary - old_salary})")
#             else:
#                 print("该工号不存在！")
#
#         if ins == '4':
#             uid = int(input("请输入工号："))
#             if mems.get(uid):
#                 e = mems[uid]
#                 print(f"{e.get_name()}，工号：{e.get_uid()}，当前职位：{e.get_job()}{e.get_grade()}，当前薪资：{e.salary()}")
#
#                 name = e.get_name()
#                 job = e.get_job()
#                 grade = e.get_grade()
#                 year = e.get_year()
#                 old_salary = e.salary()
#                 n = int(input("请输入需要减少的级数："))
#
#                 if job == 'M':
#                     if grade - n <= 0:
#                         job = 'T'
#                         grade = MAX_T_GRADE
#                         jobs['M'].remove(e)  # 将jobs中相应职位的对象删除
#                         e = Teamleader(name, job, grade, year, uid)  # 生成新的对象
#                         jobs['T'].append(e)  # 添加到jobs中的新职位中
#                         mems[uid] = e  # mems也要相应的旧对象也要被覆盖
#                     else:
#                         e.set_grade(grade - n)
#
#                 elif job == 'T':  # 注意，这里要用elif，否则上面M变成T之后，还会在这里跑多一遍
#                     if grade - n <= 0:
#                         job = 'E'
#                         grade = MAX_E_GRADE
#                         jobs['T'].remove(e)
#                         e = Employee(name, job, grade, year, uid)
#                         jobs['E'].append(e)
#                         mems[uid] = e
#                     else:
#                         e.set_grade(grade - n)
#
#                 elif job == 'E':  # 注意，这里要用elif，否则上面T变成E之后，还会在这里跑多一遍
#                     if grade - n <= 0:
#                         print("已达最低级别！")
#                         grade = 1
#
#                 new_salary = e.salary()
#
#                 print("降级成功！")
#                 print(
#                     f"{e.get_name()}，工号：{e.get_uid()}，降级后职位：{e.get_job()}{e.get_grade()}，降级后薪资：{e.salary()}({new_salary - old_salary})")
#             else:
#                 print("该工号不存在！")
#
#             if ins == '5':
#                 break
#
# main()

# class Car:
#     def __init__(self, brand, model, platenum, dayrent, carid):
#         self.brand = brand
#         self.model = model
#         self.platenum = platenum
#         self.dayrent = dayrent
#         self.carid = carid
#         self.is_rented = False
#
#     def get_brand(self):
#         return self.brand
#
#     def get_model(self):
#         return self.model
#
#     def get_platenum(self):
#         return self.platenum
#
#     def get_dayrent(self):
#         return self.dayrent
#
#     def get_carid(self):
#         return self.carid
#
#     def calc_rent(self, days, discount):
#         return (days * self.dayrent) * discount
#
#
# class EconomyCar(Car):
#     def __init__(self, brand, model, platenum, dayrent, carid, allowance):
#         super().__init__(brand, model, platenum, dayrent, carid)
#         self.allowance = allowance
#
#     def calc_rent(self, days, discount):
#         return (days * self.dayrent) * discount - self.allowance
#
#
# class LuxuryCar(Car):
#     def __init__(self, brand, model, platenum, dayrent, carid, insurancecost):
#         super().__init__(brand, model, platenum, dayrent, carid)
#         self.insurancecost = insurancecost
#
#     def calc_rent(self, days, discount):
#         return (days * self.dayrent) * discount + self.insurancecost
#
#
# class SportCar(Car):
#     def __init__(self, brand, model, platenum, dayrent, carid, losscost):
#         super().__init__(brand, model, platenum, dayrent, carid)
#         self.losscost = losscost
#
#     def calc_rent(self, days, discount):
#         return (days * self.dayrent) * discount + self.losscost
#
#
# class SUV(Car):
#     def __init__(self, brand, model, platenum, dayrent, carid):
#         super().__init__(brand, model, platenum, dayrent, carid)
#
#     def calc_rent(self, days, discount):
#         if days > 7:
#             return ((days * self.dayrent) * discount) * 0.7
#         else:
#             return (days * self.dayrent) * discount
#
#
# class CarOperation:
#     def __init__(self):
#         self.cars = {}
#         self.stocks = {'EconomyCar': [], 'LuxuryCar': [], 'SportCar': [], 'SUV': []}
#
#     def operate(self):
#         id = 10000
#         while True:
#             ins = int(input(f"\n1.录入汽车，2.获取库存，3.租车服务，4.还车服务，5.退出程序："))
#             # if ins not in [1, 2, 3, 4, 5]:
#             #     ins = int(input(f"\n1.录入汽车，2.获取库存，3.租车服务，4.还车服务，5.退出程序："))
#             #     continue
#             if ins == 1:
#                 classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
#                 # while classic not in [1, 2, 3, 4]:
#                 #     classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
#                 brand = input(f"\n品牌：")
#                 model = input(f"\n型号：")
#                 platenum = input(f"\n车牌：")
#                 dayrent = int(input(f"\n每日租金："))
#                 carid = id
#                 id += 1
#
#                 if classic == 1:
#                     allowance = int(input(f"\n请输入经济车型的补贴金额："))
#                     car = EconomyCar(brand, model, platenum, dayrent, carid, allowance)
#                     self.cars[carid] = car
#                     self.stocks['EconomyCar'].append(carid)
#                 if classic == 2:
#                     insurancecost = int(input(f"\n请输入豪华车型的保险费用："))
#                     car = LuxuryCar(brand, model, platenum, dayrent, carid, insurancecost)
#                     self.cars[carid] = car
#                     self.stocks['LuxuryCar'].append(carid)
#                 if classic == 3:
#                     losscost = int(input(f"\n请输入跑车型的损失费用："))
#                     car = SportCar(brand, model, platenum, dayrent, carid, losscost)
#                     self.cars[carid] = car
#                     self.stocks['SportCar'].append(carid)
#                 if classic == 4:
#                     car = SUV(brand, model, platenum, dayrent, carid)
#                     self.cars[carid] = car
#                     self.stocks['SUV'].append(carid)
#
#             if ins == 2:
#                 classic = int(input(f"\n1.经济车型，2.豪华车，3.跑车，4.SUV："))
#                 if classic == 1:
#                     if len(self.stocks['EconomyCar']) != 0:
#                         print(f"经济车型数量：{len(self.stocks['EconomyCar'])}")
#                         for each in self.stocks['EconomyCar']:
#                             print(
#                                 f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
#                     else:
#                         print(f"经济车型数量为0")
#
#                 if classic == 2:
#                     if len(self.stocks['LuxuryCar']) != 0:
#                         print(f"豪华车型数量：{len(self.stocks['LuxuryCar'])}")
#                         for each in self.stocks['LuxuryCar']:
#                             print(
#                                 f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
#                     else:
#                         print(f"豪华车型数量为0")
#
#                 if classic == 3:
#                     if len(self.stocks['SportCar']) != 0:
#                         print(f"跑车型数量：{len(self.stocks['SportCar'])}")
#                         for each in self.stocks['SportCar']:
#                             print(
#                                 f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
#                     else:
#                         print(f"跑车型数量为0")
#
#                 if classic == 4:
#                     if len(self.stocks['SUV']) != 0:
#                         print(f"SUV型数量：{len(self.stocks['SUV'])}")
#                         for each in self.stocks['SUV']:
#                             print(
#                                 f"ID：{self.cars[each].get_carid()}，品牌：{self.cars[each].get_brand()}， 型号:{self.cars[each].get_model()}， 车牌:{self.cars[each].get_platenum()}， 日租金:{self.cars[each].get_dayrent()}")
#                     else:
#                         print(f"SUV数量为0")
#
#             if ins == 3:
#                 id = int(input(f"\n请输入要租车辆的id："))
#                 if id in self.cars and self.cars[id].is_rented == False:
#                     days = int(input(f"\n请输入要租的天数："))
#                     print(f"\n租车费用为：{self.cars[id].calc_rent(days, discount=0.9)}")
#                     self.cars[id].is_rented = True
#                 else:
#                     print(f"\n没有该id的车辆！")
#
#             if ins == 4:
#                 id = int(input(f"请输入车辆编号："))
#                 if id in self.cars and self.cars[id].is_rented == True:
#                     self.cars[id].is_rented = False
#                     print("还车成功！")
#                 else:
#                     print(f"\n没有该id的车辆！")
#
#             if ins == 5:
#                 print("退出程序！")
#                 break
#
#
# c = CarOperation()
# c.operate()

# import random
# import string
#
# class ShortURL:
#     def __init__(self):
#         self.url_dict = {}          #存储短链接和长链接的对应关系
#         self.click_dict = {}        #存储短链接的点击次数
#         self.long_to_short = {}     #存储长链接到短链接的映射
#
#     def __getitem__(self, short_url):
#         # 当短链接被访问，增加点击次数，并返回对应的长链接
#         if short_url in self.url_dict:
#             self.click_dict[short_url] = self.click_dict.get(short_url,0) + 1
#             return self.url_dict[short_url]
#         raise KeyError(f"短链接{short_url}不存在！")
#
#     def __setitem__(self, short_url, long_url):
#         #存储长链接和短链接的对应关系
#         self.url_dict[short_url] = long_url
#
#     def generate_short_url(self, long_url):
#         #如果长链接存在，直接返回短链接，否则生成短链接
#         if long_url in self.long_to_short:
#             return self.long_to_short[long_url]
#
#         while True:
#             #生产一个随机短链接，长度为8
#             short_url =''.join(random.choices(string.ascii_letters + string.digits, k=8))
#             if short_url not in self.url_dict:
#                 break
#
#         self.long_to_short[long_url] = short_url
#         return short_url
#
#     def get_clicks(self, short_url):
#         #返回短链接的点击次数，如果短链接不存在，返回0
#         return self.click_dict.get(short_url, 0)
#
# def main():
#     url_gen = ShortURL()
#
#     long_url = input(f"请输入：")
#     short_url = url_gen.generate_short_url(long_url)
#     print(f"短链接 -> {short_url}")
#
#     url_gen[short_url] = long_url
#     redirect_url = url_gen[short_url]
#     print(f"{short_url}指向 -> ", redirect_url)
#
#     for _ in range(10):
#         redirect_url = url_gen[short_url]
#         print(f"短链接（{short_url}）被点击的次数：", url_gen.get_clicks(short_url))
#
#     print(url_gen.url_dict)
#
# main()


# class A:
#     def __init__(self):
#         self.a_to_b ={}
#
#     def __getitem__(self, a_to_b):
#         return self.key
#
#     def __setitem__(self, a_to_b, value):
#         self.key = value
#
# a = A()
# key1 = "name"
# value1 = "li"
# a_to_b = {key1:value1}
# a[a_to_b(key1)] = value1
# print(dir(a))

# class A:

#
# class Student:
#     def __init__(self, name, score):
#         self.name = name
#         self.score = score
#
#     def __eq__(self, other):
#         # 请补充代码
#         if isinstance(other, Student):
#             return self.score == other.score
#         return False
#
#     def __ne__(self, other):
#         # 请补充代码
#         return not self.__eq__(other)
#
#     def __lt__(self, other):
#         # 请补充代码
#         if isinstance(other, Student):
#             return self.score < other.score
#         return False
#
#     def __le__(self, other):
#         # 请补充代码
#         if isinstance(other, Student):
#             return self.score <= other.score
#         return False
#
#     def __gt__(self, other):
#         # 请补充代码
#         if isinstance(other, Student):
#             return self.score > other.score
#         return False
#
#     def __ge__(self, other):
#         # 请补充代码
#         if isinstance(other, Student):
#             return self.score >= other.score
#         return False
#
#
# class StudentManager:
#     def __init__(self, init_id=100):
#         self.students = {}
#         self.stu_id = init_id
#
#     def add_student(self, student):
#         if student not in self.students.values():
#             self.students[self.stu_id] = student
#             print(f"添加成功，{student.name} -> ID：{self.stu_id}")
#             self.stu_id += 1
#
#     def update_score(self, stu_id, new_score):
#         if stu_id in self.students:
#             self.students[stu_id].score = new_score
#
#     def find_student(self, name):
#         for student in self.students.values():
#             if student.name == name:
#                 return student
#
#     def delete_student(self, stu_id):
#         if stu_id in self.students:
#             student = self.students.pop(stu_id)
#             print(f"删除成功，{student.name} -> ID：{self.stu_id}")
#             del student
#
#     def __iter__(self):
#         # 请补充代码
#         self.values = list(self.students.values())
#         self.index = 0
#         return self
#
#     def __next__(self):
#         # 请补充代码
#         if self.index < len(self.values):
#             student = self.values[self.index]
#             self.index += 1
#             return student
#         else:
#             raise StopIteration
#
#     def __contains__(self, name):
#         # 请补充代码
#         for student in self.students.values():
#             if student.name == name:
#                 return student
#
#     def __len__(self):
#         # 请补充代码
#         return len(self.students)
#
# if __name__ == "__main__":
#     # 初始学号ID设置为100
#     manager = StudentManager(100)
#
#     # 添加学生
#     s1 = Student("小甲鱼", 666)
#     s2 = Student("不二如是", 888)
#     s3 = Student("张三李四", 233)
#     manager.add_student(s1)
#     manager.add_student(s2)
#     manager.add_student(s3)
#
#     # 更新学生成绩
#     manager.update_score(100, 999)
#
#     # 查找学生
#     target = manager.find_student("小甲鱼")
#     print(f"查找 -> {target.name}，成绩: {target.score}")
#
#     # 删除学生
#     manager.delete_student(102)
#
#     # 迭代学生列表
#     for student in manager:
#         print(f"迭代 -> 姓名：{student.name}，成绩: {student.score}")
#
#     # 检查学生是否在列表中
#     print("小甲鱼" in manager)
#
#     # 获取学生数量
#     print(len(manager))
#
#     # 学生之间的成绩PK
#     print(s1 > s2)

# import pickle
# import time
# from pathlib import Path
#
# from unicodedata import category
#
#
# class Reminder:
#     def __init__(self, storage_file='reminders.pkl'):
#         self.storage_file = Path(storage_file)
#         self.reminders = self.load_reminders()
#
#     def __call__(self, action, *args, **kwargs):
#         actions = {
#             '添加':self.add_reminder,
#             '查看':self.list_reminders,
#             '删除':self.delete_reminder,
#             '完成':self.complete_reminder,
#             '搜索':self.search_reminders,
#         }
#
#         if action in actions:
#             actions[action](*args, **kwargs)
#         else:
#             print("无效操作，请使用‘添加’、‘查看’、‘删除’、‘完成’或者‘搜索’。")
#
#     def __getitem__(self, index):
#         return self.reminders[index]
#
#     def __repr__(self):
#         return f'Reminder(reminders={self.reminders})'
#
#     def __str__(self):
#         return f'提醒事项共有{len(self.reminders)}条'
#
#     def load_reminders(self):
#         if self.storage_file.exists():
#             with self.storage_file.open('rb') as file:
#                 return pickle.load(file)
#         return []
#
#     def save_reminders(self):
#         with self.storage_file.open('wb') as file:
#             pickle.dump(self.reminders, file)
#
#     def add_reminder(self, reminder, category, color, priority=0, delay=None):
#         reminder_item = {'text':reminder, 'category':category, 'color':color,
#                          'completed':False, 'priority':priority}
#         self.reminders.append(reminder_item)
#         self.save_reminders()
#         print(f"已添加提醒事项：{reminder}, 分类：{category}, 优先级：{priority}")
#
#         if delay is not None:
#             time.sleep(delay)
#             self.show_reminder(reminder)
#
#     def show_reminder(self, reminder):
#         print(f"提醒：{reminder}")
#
#     def list_reminders(self):
#         print('提醒事项：')
#         for i, reminder in enumerate(self.reminders):
#             status = '已完成' if reminder['completed'] else '未完成'
#             print(f'{i + 1}. {reminder["text"]} ({reminder["category"]}, {reminder["color"]})'
#                   f' ({status})，优先级：{reminder["priority"]}')
#
#     def delete_reminder(self, index):
#         try:
#             removed = self.reminders.pop(index - 1)
#             self.save_reminders()
#             print(f'已删除提醒事项:{removed["text"]}')
#         except IndexError:
#             print('索引无效！')
#
#     def complete_reminder(self, index):
#         try:
#             self.reminders[index - 1]['completed'] = True
#             self.save_reminders()
#             print(f'已完成事项: {self.reminders[index - 1]['text']}')
#         except IndexError:
#             print('索引无效！')
#
#     def search_reminders(self, keyword):
#         results = [r for r in self.reminders if keyword.lower() in r['text'].lower()]
#         if results:
#             print('搜索结果:')
#             for reminder in results:
#                 status = '已完成' if reminder['completed'] else '未完成'
#                 print(f'{reminder["text"]}({reminder['category']},{reminder['color']})'
#                       f'({status}),优先级：{reminder['priority']}')
#         else:
#             print('未找到与关键字匹配的提醒事项！')
#
#
# if __name__ == "__main__":
#     # 测试阶段为了防止数据重复添加
#     # 在每次启动时自动删除pickle文件
#     p = Path.cwd() / "reminders.pkl"
#     p.unlink(missing_ok=True)
#
#     reminder = Reminder("reminders.pkl")
#
#     # 添加提醒
#     print("添加提醒 >>>")
#     reminder('添加', '买牛奶', '购物', '黄色', 1)
#     reminder('添加', '发邮件', '工作', '蓝色', 2)
#     reminder('添加', '学编程', '学习', '红色', 1)
#     reminder('添加', '打游戏', '娱乐', '紫色', 4)
#     reminder('添加', '谈恋爱', '娱乐', '粉色', 1)
#     reminder('添加', '打酱油', '购物', '黄色', 3)
#
#     # 删除提醒
#     print("\n删除提醒 >>>")
#     reminder.delete_reminder(6)
#
#     # 列举提醒
#     print("\n列举提醒 >>>")
#     reminder.list_reminders()
#
#     # 完成提醒
#     print("\n完成提醒 >>>")
#     reminder.complete_reminder(5)
#
#     print("\n列举提醒 >>>")
#     reminder.list_reminders()
#
#     # 搜索提醒
#     print("\n搜索提醒 >>>")
#     reminder.search_reminders("编程")

# class Device:
#     def __init__(self, name):
#         self.name = name
#         self._status = False
#
#     @property
#     def status(self):
#         return self._status
#
#     @status.setter
#     def status(self, value):
#         self._status = value
#         print(f"[{self.name}]的开启状态是：{self.status}")
#
# class Light(Device):
#     def __init__(self, name, brightness=50):
#         super().__init__(name)
#         self._brightness = brightness
#
#     @property
#     def brightness(self):
#         return self._brightness
#
#     @brightness.setter
#     def brightness(self, value):
#         self.status = True
#         self._brightness = value
#         print(f"现在的亮度是{self.brightness}")
#
# class AirConditioner(Device):
#     def __init__(self, name, temperature=22):
#         super().__init__(name)
#         self._temperature = temperature
#
#     @property
#     def temperature(self):
#         return self._temperature
#
#     @temperature.setter
#     def temperature(self, value):
#         self.status = True
#         self._temperature = value
#         print(f"现在的温度是{self.temperature}")
#
# class SecurityCamera(Device):
#     def __init__(self, name, recording=True):
#         super().__init__(name)
#         self._recording = recording
#
#     @property
#     def recording(self):
#         return self._recording
#
#     @recording.setter
#     def recording(self, value):
#         if value and not self.status:
#             print(f"没有找到设备[{self.name}]")
#         else:
#             self._recording = value
#             print(f"[{self.name}]的录制状态是：{self._recording}")
#
# class SmartHomeSystem:
#     def __init__(self):
#         self.devices = {}
#
#     def add_device(self, device):
#         self.devices[device.name] = device
#         print(f"设备[{device.name}]已添加")
#
#     def remove_device(self, device):
#         del self.devices[device.name]
#
#     def get_device(self, device):
#         return self.devices[device.name]
#
# home_system = SmartHomeSystem()
# light = Light("客厅灯光", brightness=50)
# airconditioner = AirConditioner("客厅空调", temperature=22)
# camera = SecurityCamera("前门摄像头", recording=True)
#
# home_system.add_device(light)
# home_system.add_device(airconditioner)
# home_system.add_device(camera)
#
# light.brightness = 75
# airconditioner.temperature = 24
# camera.status = True  # 没有这一句的话应该抛出异常
# camera.recording = True
# light.status = False

# class C:
#     def funA(self):
#         return self
#
#     @classmethod
#     def funB(cls):
#         return cls
#
# c = C()
# print(isinstance(c.funA(), c.funB()))

# class C:
#     @classmethod
#     def funA(cls):
#         print(cls.__name__)
#
# class D(C):
#     pass
#
# d = D()
# d.funA()

# class C:
#     count = 0
#     def __init__(self):
#         C.count += 1
#
#     @classmethod
#     def get_count(cls):
#         return cls.count
#
# c = C()
# c.count = 10
# print(c.get_count())

# class Calculator:
#     @staticmethod
#     def add(a, b):
#         return a + b
#
# c = Calculator()
# print(c.add(1,2))

# class Settings:
#     config = {"theme": "dark", "language": "en"}
#
#     @classmethod
#     def update_config(cls, key, value):
#         cls.config[key] = value
#
#     @staticmethod
#     def reset_config():
#         Settings.config = {}
#
# # 更新配置
# Settings.update_config("theme", "light")
# print(Settings.config)
#
# # 重置配置
# Settings.reset_config()
# print(Settings.config)

class Logger:
    log_count = 0

    @classmethod
    def log(cls, message):
        cls.log_count += 1
        print(f"{cls.__name__} Log #{cls.log_count}: {message}")

class FileLogger(Logger):
    pass

Logger.log("系统启动。")
FileLogger.log("打开文件。")
Logger.log("出错！")
FileLogger.log("关闭文件。")


































