class BankAccount:
    def __init__(self, name, balance):
        if balance < 0:
            raise ValueError("初始余额不能小于0")
        self.name = name
        self.balance = balance

    @staticmethod
    def check_amount(amount):
        if amount <= 0:
            print("金额必须大于0")
            return False
        return True

    def deposit(self, amount):
        if not self.check_amount(amount):
            return
        self.balance += amount
        print(f"存钱成功！当前余额{self.balance}")

    def withdraw(self, amount):
        if not self.check_amount(amount):
            return
        if amount > self.balance:
            print("余额不足！")
            return
        self.balance -= amount
        print(f"取钱成功！剩余余额{self.balance}")

    def show_balance(self):
        print(f"当前余额：{self.balance}")

account = BankAccount("小明",100)
account.deposit(50)
account.withdraw(30)
account.show_balance()