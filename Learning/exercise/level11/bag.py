class Item:
    def __init__(self, name, weight, value):
        self.name = name
        self.weight = weight
        self.value = value

class Bag:
    def __init__(self, capacity):
        self.capacity = capacity
        self.items = []

    def add_items(self, item):
        if self.total_weight() + item.weight > self.capacity:
            print("超过载重！")
            return
        self.items.append(item)

    def show_items(self):
        for item in self.items:
            print(f"{item.name}的重量是{item.weight},价值是{item.value}")

    def total_weight(self):
        total_weight = 0
        for item in self.items:
            total_weight += item.weight

        return total_weight

    def total_value(self):
        total_value = 0
        for item in self.items:
            total_value += item.value
        return total_value

item1 = Item('手机', 100, 10000)
item2 = Item('冰箱', 10000, 10000)
item3 = Item('耳机', 10, 1000)
bag =Bag(1000)
bag.add_items(item1)
bag.add_items(item3)
bag.show_items()
print(f"总重量是：{bag.total_weight()}")
print(f"总价值是：{bag.total_value()}")
# bag.add_items(item2)