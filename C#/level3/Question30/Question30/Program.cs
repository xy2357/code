namespace Question30
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Monster monster = new Monster("史莱姆", 20);
            QuestSystem querySystem = new QuestSystem();
            monster.Died += querySystem.OnMonsterDied;
            monster.TakeDamage(50);
        }
    }

    class Monster
    {
        private string _name;
        private int _hp;
        private bool _isDead = false;
        public Monster(string name, int hp)
        {
            _name = name;
            _hp = hp;
        }
        public event EventHandler<MonsterDiedEventArgs>? Died;
        public void TakeDamage(int damage)
        {
            if (_isDead) return;
            _hp -= damage;
            if (_hp <= 0)
            {
                _isDead = true;
                MonsterDiedEventArgs monsterDiedEventArgs = new MonsterDiedEventArgs();
                monsterDiedEventArgs.Name = _name;
                Died?.Invoke(this, monsterDiedEventArgs);
            }
        }
    }

    class QuestSystem
    {
        public void OnMonsterDied(object? sender, MonsterDiedEventArgs e)
        {
            Console.WriteLine($"任务系统收到消息：{e.Name}死亡");
            Console.WriteLine($"击杀{e.Name}任务进度 +1");
        }
    }

    public class MonsterDiedEventArgs:EventArgs
    {
        public string Name { get; set; } = "";
    }
}
