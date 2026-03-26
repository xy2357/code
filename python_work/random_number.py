import random
import tkinter as tk

# 创建窗口
window = tk.Tk()
window.title("随机数生成器")
window.geometry("300x200")

# 创建标签
label = tk.Label(window, text="请设置随机数的范围：")
label.pack()

# 创建输入框
start_entry = tk.Entry(window)
start_entry.pack()
end_entry = tk.Entry(window)
end_entry.pack()

# 创建按钮
def generate_random_num():
    start = int(start_entry.get())
    end = int(end_entry.get())
    random_num = random.randint(start, end)
    result_label.config(text=f"生成的随机数为：{random_num}")

button = tk.Button(window, text="生成随机数", command=generate_random_num)
button.pack()

# 创建结果标签
result_label = tk.Label(window, text="")
result_label.pack()

# 运行窗口
window.mainloop()
