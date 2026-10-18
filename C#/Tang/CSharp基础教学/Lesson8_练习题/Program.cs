namespace Lesson8_练习题
{
    internal class Program
    {

        static bool CheckLogin(string name, string password, ref string info)
        {
            if (name == "admin")
            {
                if (password == "88888888")
                {
                    info = "登陆成功";
                    return true;
                }
                info = "密码错误";
                return false;
            }
            info = "用户名错误";
            return false;
        }
        static void Main(string[] args)
        {
            #region 练习一
            string info = "";
            Console.WriteLine("请输入用户名：");
            string name = Console.ReadLine();
            Console.WriteLine("请输入密码：");
            string password = Console.ReadLine();
            CheckLogin(name, password, ref info);
            Console.WriteLine(info);
            #endregion
        }
    }
}
