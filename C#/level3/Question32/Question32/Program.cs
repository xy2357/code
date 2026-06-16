namespace Question32
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChatWindow window = new ChatWindow();
            ChatRoom room = new ChatRoom();
            room.MessageReceive += window.ShowMessage;
            room.SendMessage("你好");

            room.MessageReceive -= window.ShowMessage;
            room.SendMessage("你还在吗？");
        }
    }

    class ChatRoom
    {
        public event EventHandler<MessageReceiveEventArgs>? MessageReceive;

        public void SendMessage(string message) 
        {
            MessageReceiveEventArgs messageReceiveEventArgs = new MessageReceiveEventArgs();
            messageReceiveEventArgs.Message = message;
            MessageReceive?.Invoke(this, messageReceiveEventArgs);
        }
    }

    class ChatWindow
    {
        public void ShowMessage(object? sender, MessageReceiveEventArgs e)
        {
            Console.WriteLine($"聊天窗口显示：{e.Message}");
        }
    }

    class MessageReceiveEventArgs:EventArgs
    {
        public string Message { get; set; } = "";
    }
}
