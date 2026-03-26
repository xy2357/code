import pandas as pd
import tkinter as tk
from tkinter import filedialog


def compare_excel_columns(file1, sheet1, column1, file2, sheet2, column2):
    try:
        # 读取 Excel 文件
        df1 = pd.read_excel(file1, sheet_name=sheet1)
        df2 = pd.read_excel(file2, sheet_name=sheet2)

        # 重新设置列名
        df1.columns = [str(col) for col in df1.columns]
        df2.columns = [str(col) for col in df2.columns]

        # 提取指定列的数据
        data1 = df1[column1].tolist()
        data2 = df2[column2].tolist()

        # 检查两列数据是否相同
        if data1 == data2:
            result_label.config(text=f"The columns {column1} in {file1} and {column2} in {file2} are identical.")
        else:
            result_label.config(text=f"The columns {column1} in {file1} and {column2} in {file2} are different.")

            # 找出两列数据中不同的行
            diff_rows = df1[df1[column1] != df2[column2]]
            print("Different rows between the two columns:")
            print(diff_rows)

    except pd.errors.EmptyDataError:
        result_label.config(text="One or both of the specified columns are empty.")


def browse_file(entry):
    filename = filedialog.askopenfilename()
    entry.delete(0, tk.END)
    entry.insert(0, filename)


# 创建主窗口
root = tk.Tk()
root.title("Excel Column Comparison")

# 创建界面元素
file1_entry = tk.Entry(root, width=50)
file2_entry = tk.Entry(root, width=50)
sheet1_entry = tk.Entry(root, width=20)
sheet2_entry = tk.Entry(root, width=20)
column1_entry = tk.Entry(root, width=20)
column2_entry = tk.Entry(root, width=20)
result_label = tk.Label(root, text="Result will be displayed here.")

# 设置界面布局
file1_label = tk.Label(root, text="Excel File 1:")
file2_label = tk.Label(root, text="Excel File 2:")
sheet1_label = tk.Label(root, text="Sheet 1:")
sheet2_label = tk.Label(root, text="Sheet 2:")
column1_label = tk.Label(root, text="Column 1:")
column2_label = tk.Label(root, text="Column 2:")

compare_button = tk.Button(root, text="Compare", command=lambda: compare_excel_columns(
    file1_entry.get(), sheet1_entry.get(), column1_entry.get(),
    file2_entry.get(), sheet2_entry.get(), column2_entry.get()
))

file1_browse_button = tk.Button(root, text="Browse", command=lambda: browse_file(file1_entry))
file2_browse_button = tk.Button(root, text="Browse", command=lambda: browse_file(file2_entry))

# 将元素放置在窗口中
file1_label.grid(row=0, column=0, padx=5, pady=5)
file1_entry.grid(row=0, column=1, columnspan=2, padx=5, pady=5)
file1_browse_button.grid(row=0, column=3, padx=5, pady=5)

sheet1_label.grid(row=1, column=0, padx=5, pady=5)
sheet1_entry.grid(row=1, column=1, padx=5, pady=5)

column1_label.grid(row=2, column=0, padx=5, pady=5)
column1_entry.grid(row=2, column=1, padx=5, pady=5)

file2_label.grid(row=0, column=4, padx=5, pady=5)
file2_entry.grid(row=0, column=5, columnspan=2, padx=5, pady=5)
file2_browse_button.grid(row=0, column=7, padx=5, pady=5)

sheet2_label.grid(row=1, column=4, padx=5, pady=5)
sheet2_entry.grid(row=1, column=5, padx=5, pady=5)

column2_label.grid(row=2, column=4, padx=5, pady=5)
column2_entry.grid(row=2, column=5, padx=5, pady=5)

compare_button.grid(row=3, column=0, columnspan=8, pady=10)

result_label.grid(row=4, column=0, columnspan=8, pady=10)

# 运行主循环
root.mainloop()
