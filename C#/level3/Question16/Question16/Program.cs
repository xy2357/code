namespace Question16;

class Program
{
    static void Main(string[] args)
    {
        Button button = new Button();
        button.Click += () =>
        {
            Console.WriteLine("Button clicked");
        };
        button.Press();
    }
}

class Button
{
    public event Action? Click;

    public void Press()
    {
        Click?.Invoke();
    }
}