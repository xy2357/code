class Book:
    def __init__(self, name, author, stock):
        self.name = name
        self.author = author
        self.stock = stock

class BookManager:
    def show_all_book(self):