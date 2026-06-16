namespace Question29
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            inventory.ItemAdded += RefreshUI;
            inventory.ItemAdded += PlaySound;
            inventory.ItemAdded += CheckQuest;
            inventory.AddItem("铁剑");
        }

        static void RefreshUI(object? sender, ItemAddedEventArgs e)
        {
            Console.WriteLine($"刷新背包UI {e.Name}");
        }
        static void PlaySound(object? sender, ItemAddedEventArgs e)
        {
            Console.WriteLine($"播放{e.Name}音乐");
        }
        static void CheckQuest(object? sender, ItemAddedEventArgs e)
        {
            Console.WriteLine($"检查{e.Name}任务");
        }
    }

    class Inventory
    {
        public void AddItem(string name)
        {
            Console.WriteLine($"添加{name}");
            ItemAddedEventArgs itemAddedEventArgs = new ItemAddedEventArgs();
            itemAddedEventArgs.Name = name;
            ItemAdded?.Invoke(this, itemAddedEventArgs);
        }

        public event EventHandler<ItemAddedEventArgs>? ItemAdded;
    }

    public class ItemAddedEventArgs:EventArgs
    {
        public string Name { get; set; } = "";
    }
}
