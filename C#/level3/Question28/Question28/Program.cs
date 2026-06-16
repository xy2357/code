namespace Question28
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Button startButton = new Button("开始游戏");
            Game game = new Game();
            startButton.Click += game.Start;
            startButton.Press();
        }
    }

    class Button
    {
        private string _name;

        public Button(string name)
        {
            _name = name;
        }

        public event EventHandler? Click;

        public void Press()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    class Game
    { 
        public void Start(object? sender, EventArgs e)
        {
            Console.WriteLine("游戏开始");
        }
    }
}
