using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question11
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Button button = new Button();
            button.Click += button.OnClick;
            button.Click += button.PlaySound;
            button.Click += button.ShowText;
            button.Click -= button.PlaySound;
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

        public void OnClick()
        {
            Console.WriteLine("按钮被点击了");
        }

        public void PlaySound()
        {
            Console.WriteLine("播放音效");
        }

        public void ShowText()
        {
            Console.WriteLine("显示点击文字");
        }
    }
}
