namespace Question7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Button button = new Button();
            button.Click += button.OnClicked;
            button.Press();
        }
    }

    class Button
    {
        public event Action Click;

        public void Press()
        {
            this.Click?.Invoke();
        }

        public void OnClicked()
        {
            Console.WriteLine("按钮被点击了");
        }
    }

}
