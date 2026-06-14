namespace Question21;

class Program
{
    static void Main(string[] args)
    {
        Button button = new Button("开始");
        button.Click += OnButtonClick;
        button.Press();
    }

    static void OnButtonClick(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Console.WriteLine($"{button.Name}被点击了");
        }
    }
}

class Button
{
    private string _name;

    public Button(string name)
    {
        _name = name;
    }

    public string Name
    {
        get { return _name; }
    }

    public event EventHandler? Click;

    public void Press()
    {
        
        Click?.Invoke(this, EventArgs.Empty);
    }
}
