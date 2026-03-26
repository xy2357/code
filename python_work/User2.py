class User:

    def __init__(self, first_name, last_name, age, login_attempts):
        self.first_name = first_name
        self.last_name = last_name
        self.age = age
        self.login_attempts = login_attempts

    def describe_user(self):
        print("First name is " + self.first_name.title() + " , last name is " + self.last_name.title() + ".")

    def greet_user(self):
        print("Welcome " + self.first_name.title() + " you age is " + str(self.age))

    def increment_login_attempts(self):
        self.login_attempts = self.login_attempts + 1
        print("登陆次数为" + str(self.login_attempts))

    def reset_login_attempt(self):
        self.login_attempts = 0
        print("重置次数为" + str(self.login_attempts))

one_user = User('Li', 'Wu', 18, 0)
one_user.describe_user()
one_user.greet_user()
one_user.increment_login_attempts()
one_user.increment_login_attempts()
one_user.increment_login_attempts()
one_user.reset_login_attempt()
