
namespace Question31
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            player.LevelChanged += OnLevelChanged;

            player.Level = 3;
            player.Level = 2;
            player.Level = 2;
            player.Level = 4;
        }

        static void OnLevelChanged(object? sender, LevelChangedEventArgs e)
        {
            Console.WriteLine($"等级从{e.oldLevel}变成{e.newLevel}");
        }
    }

    class Player
    {
        private int _level = 1;
        public int Level
        {
            get { return _level; }
            set 
            {
                if (value == _level) { return; }
                if (value < _level)
                {
                    Console.WriteLine("等级不能小于之前的等级");
                    return;
                }
                int OldLevel = _level;
                _level = value;
                LevelChangedEventArgs levelChangedEventArgs = new LevelChangedEventArgs();
                levelChangedEventArgs.oldLevel = OldLevel;
                levelChangedEventArgs.newLevel = _level;
                LevelChanged?.Invoke(this, levelChangedEventArgs);
            }
        }

        public event EventHandler<LevelChangedEventArgs>? LevelChanged;
    }

    public class LevelChangedEventArgs:EventArgs
    {
        public int oldLevel { get; set; }
        public int newLevel { get; set; }
    }
}
