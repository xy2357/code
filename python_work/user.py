class User:
    """创建一个User的类"""

    def __init__(self, first_name, last_name, age):
        """初始化User"""
        self.first_name = first_name
        self.last_name = last_name
        self.age = age
        self.login_attempts = 0

    def describe_user(self):
        """打印用户的信息摘要"""
        print(self.first_name.title() + ' ' + self.last_name.title() + " 的年龄是 " + str(self.age))

    def greet_user(self):
        """向用户发出问候"""
        print("你好！" + self.first_name.title() + ' ' + self.last_name.title())

    def increment_login_attempts(self):
        self.login_attempts += 1
        print("你已经登陆" + str(self.login_attempts) + "次！")

    def reset_login_attempts(self):
        self.login_attempts = 0
        print("你的登陆次数已经重置!" + str(self.login_attempts))


new_user = User('albert', 'einstein', 24)
new_user.describe_user()
new_user.greet_user()
new_user.increment_login_attempts()
new_user.increment_login_attempts()
new_user.increment_login_attempts()
new_user.increment_login_attempts()
new_user.increment_login_attempts()
new_user.reset_login_attempts()
