import random
import tkinter as tk


class RandomNumberGenerator:
    def __init__(self, master):
        self.master = master
        master.title("Random Number Generator")

        self.label = tk.Label(master, text="How many numbers do you want to generate?")
        self.label.pack()

        self.num_input = tk.Entry(master)
        self.num_input.pack()

        self.button = tk.Button(master, text="Generate Numbers", command=self.generate_numbers)
        self.button.pack()

        self.results_label = tk.Label(master, text="")
        self.results_label.pack()

    def generate_numbers(self):
        num_numbers = int(self.num_input.get())

        values = [40, 70, 100, 0, 200]
        probabilities = [0.4, 0.4, 0.1, 0.05, 0.05]

        random_numbers = [str(random.choices(values, probabilities)[0]) for i in range(num_numbers)]
        results_text = "\n".join(random_numbers)

        self.results_label.config(text=results_text)


root = tk.Tk()
my_gui = RandomNumberGenerator(root)
root.mainloop()
