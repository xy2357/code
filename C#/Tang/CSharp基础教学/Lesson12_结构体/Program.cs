using System.Formats.Asn1;
using System.Numerics;

namespace Lesson12_结构体
{
    enum E_Occupation
    {
        warrior = 0,
        hunter = 1,
        magic = 2,
    }
    internal class Program
    {
        struct Student
        {
            public string name;
            public bool sex;
            public int age;
            public int cls;
            public string major;

            public Student(string name, bool sex, int age, int cls, string major)
            {
                this.name = name;
                this.sex = sex;
                this.age = age;
                this.cls = cls;
                this.major = major;
            }

            public void Speak()
            {
                Console.WriteLine("姓名：{0}，性别：{1}，年龄：{2}，班级：{3}，专业：{4}", name, sex ? "女" : "男", age, cls, major);
            }
        }

        struct Rectangle
        {
            public int width;
            public int height;
            public float area;
            public float perimeter;

            public Rectangle(int width, int height)
            {
                this.width = width;
                this.height = height;
                area = width * height;
                perimeter = (height + width) * 2;
            }
        }

        struct Character
        {
            public string name;
            public string occupation;
            public string skill;
        }

        struct Monster
        {
            public string name;
            public string occupation;
            public string skill;
            public int atk;
        }

        struct OutMan
        {
            public string name;
            public int atk;
            public int def;
            public int hp;

            public OutMan(string name, int atk, int def, int hp)
            {
                this.name = name;
                this.atk = atk;
                this.def = def;
                this.hp = hp;
            }

            public void Atk(ref Boss boss)
            {
                //奥特曼打小怪兽
                boss.hp -= this.atk - boss.def;
                Console.WriteLine("{0}攻击了{1}, 造成了{2}伤害，剩余{3}血量", name, boss.name, this.atk - boss.def, boss.hp);
            }
        }
        
        struct Boss
        {
            public string name;
            public int atk;
            public int def;
            public int hp;

            public Boss(string name, int atk, int def, int hp)
            {
                this.name = name;
                this.atk = atk;
                this.def = def;
                this.hp = hp;
            }

            public void Atk(ref OutMan outMan)
            {
                //小怪兽打奥特曼
                outMan.hp -= this.atk - outMan.def;
                Console.WriteLine("{0}攻击了{1}, 造成了{2}伤害，剩余{3}血量", name, outMan.name, this.atk - outMan.def, outMan.hp);
            }
        }


        static void Main(string[] args)
        {
            #region 练习一
            //Student student1 = new Student();
            //student1.name = "小李";
            //student1.sex = "男";
            //student1.cls = 1;
            //student1.age = 18;
            //student1.professional = "数学";

            //Student student2 = new Student();
            //student2.name = "小王";
            //student2.sex = "女";
            //student2.cls = 2;
            //student2.age = 19;
            //student2.professional = "语文";

            //Console.WriteLine("{0}是{1}，在班级{2}，年龄是{3}，专业是{4}", student1.name, student1.sex, student1.cls, student1.age, student1.professional);
            //Console.WriteLine("{0}是{1}，在班级{2}，年龄是{3}，专业是{4}", student2.name, student2.sex, student2.cls, student2.age, student2.professional);

            Student s1 = new Student("小李", false, 18, 3, "数学");
            Student s2 = new Student("小明", false, 22, 2, "英语");
            s1.Speak();
            s2.Speak();
            #endregion

            #region 练习二
            //Rectangle rect1 = new Rectangle();
            //rect1.Width = 5;
            //rect1.Height = 10;
            //int rect1Area = rect1.Width * rect1.Height;
            //int rect1Perimeter = (rect1.Height + rect1.Width) * 2;
            //Console.WriteLine("{0}的面积是{1}，周长是{2}", nameof(rect1), rect1Area, rect1Perimeter);
            Rectangle rect1 = new Rectangle(5, 10);
            Console.WriteLine("{0}的面积是{1}，周长是{2}", nameof(rect1), rect1.area, rect1.perimeter);

            #endregion

            #region 练习三
            //Character character1 = new Character();
            //Console.WriteLine("请输入姓名");
            //character1.name = Console.ReadLine();
            //Console.WriteLine("请选择职业：1.战士（技能：冲锋）2.猎人（技能：假死）3.战士（技能：奥术冲击）");
            //int index = int.Parse(Console.ReadLine());
            //switch (index)
            //{
            //    case 1:
            //        character1.occupation = "战士";
            //        character1.skill = "冲锋";
            //        break;
            //    case 2:
            //        character1.occupation = "猎人";
            //        character1.skill = "假死";
            //        break;
            //    case 3:
            //        character1.occupation = "法师";
            //        character1.skill = "奥术冲击";
            //        break;
            //    default:
            //        break;
            //}
            //Console.WriteLine("{0}{1}释放了{2}", character1.occupation, character1.name, character1.skill);
            #endregion

            #region 练习四
            //Monster[] monsters = new Monster[10];
            //Random r = new Random();
            //for (int i = 0; i < monsters.Length; i++)
            //{
            //    monsters[i].name = "小怪兽" + i.ToString();
            //    monsters[i].atk = r.Next(1, 11);
            //}
            //foreach (var monster in monsters)
            //{
            //    Console.WriteLine("{0}的攻击力是{1}", monster.name, monster.atk);
            //}
            #endregion

            #region 练习五
            OutMan outMan = new OutMan("雷欧", 10, 5, 100);
            Boss boss = new Boss("哥斯拉", 8, 4, 100);

            while (true)
            {
                outMan.Atk(ref boss);
                if (boss.hp < 0) 
                {
                    Console.WriteLine("{0}胜利",outMan.name);
                    break;
                }
                boss.Atk(ref outMan);
                if (outMan.hp < 0) 
                {
                    Console.WriteLine("{1}胜利",boss.name);
                    break;
                }
                Console.WriteLine("按任意键继续");
                Console.ReadKey(true);

            }
            #endregion

        }
    }
}
