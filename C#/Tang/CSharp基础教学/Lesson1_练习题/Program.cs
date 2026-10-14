namespace Lesson1_练习题
{
    /// <summary>
    /// QQ枚举说明
    /// </summary>
    enum E_QQType
    {

        Online,
        Leave,
        Busy,
        Invisible,
    }

    enum E_CafeSize
    {
        Middle,
        Large,
        Ultra,
    }

    enum E_Sex
    {
        Man,
        Woman,
    }

    enum E_Occupation
    {
        Warrior,
        Hunter,
        Magic,
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("枚举练习题");
            #region 练习题一
            //try
            //{
            //    Console.WriteLine("请输入QQ的状态（0在线，1离开，2忙碌，3隐身）");
            //    int type = int.Parse(Console.ReadLine());
            //    E_QQType qqType = (E_QQType)type;
            //    Console.WriteLine(qqType);
            //}
            //catch
            //{
            //    Console.WriteLine("请输入数字");
            //}
            #endregion

            #region 练习题二
            //try
            //{
            //    Console.WriteLine("请输入咖啡的大小（中杯0 大杯1 超大杯2）");
            //    int size = int.Parse(Console.ReadLine());
            //    E_CafeSize cafeSize = (E_CafeSize)size;
            //    switch (cafeSize)
            //    {
            //        case E_CafeSize.Middle:
            //            Console.WriteLine("你购买了中杯咖啡，花费了35元");
            //            break;
            //        case E_CafeSize.Large:
            //            Console.WriteLine("你购买了大杯咖啡，花费了40元");
            //            break;
            //        case E_CafeSize.Ultra:
            //            Console.WriteLine("你购买了超大杯咖啡，花费了43元");
            //            break;
            //        default:
            //            Console.Write("请输入正确的类型");
            //            break;
            //    }

            //}
            //catch
            //{

            //    Console.Write("请输入数字");
            //}
            #endregion

            #region 练习题三
            try
            {
                Console.WriteLine("请选择性别（男0 女1）");
                E_Sex sex = (E_Sex)int.Parse(Console.ReadLine());
                string sexStr = "";
                string occupationStr = "";
                string skill = "";
                int atk = 0;
                int def = 0;
                switch (sex)
                {
                    case E_Sex.Man:
                        sexStr = "男性";
                        atk += 50;
                        def += 100;
                        break;
                    case E_Sex.Woman:
                        sexStr = "女性";
                        atk += 150;
                        def += 20;
                        break;
                }

                Console.WriteLine("请选择职业（战士0 猎人1 法师2）");
                E_Occupation o = (E_Occupation)int.Parse(Console.ReadLine());
                switch (o)
                {
                    case E_Occupation.Warrior:
                        atk += 20;
                        def += 100;
                        skill = "冲锋";
                        occupationStr = "战士";
                        break;
                    case E_Occupation.Hunter:
                        atk += 120;
                        def += 30;
                        skill = "假死";
                        occupationStr = "猎人";
                        break;
                    case E_Occupation.Magic:
                        atk += 200;
                        def += 10;
                        skill = "奥数冲击";
                        occupationStr = "法师";
                        break;
                    default:
                        break;
                }

                Console.WriteLine("你选择了\"{0}{1}\"，攻击力：{2}，防御力：{3}，职业技能：{4}", sexStr, occupationStr, atk, def, skill);
            }
            catch (Exception)
            {

                throw;
            }
            #endregion
        }
    }
}
