namespace Question18;

class Program
{
    static void Main(string[] args)
    {
        Button button = new Button();
        button.Click += OnButtonClick;
        button.Press();
    }

    static void OnButtonClick(object? sender, EventArgs e)
    {
        Console.WriteLine("按钮被点击了！");
    }
}

class Button
{
    public event EventHandler? Click;

    public void Press()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }
}