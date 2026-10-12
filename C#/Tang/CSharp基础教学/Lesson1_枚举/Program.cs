namespace Lesson1_枚举
{

    enum E_playerType
    {
        Mian = 1,
        Other = 2,
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            E_playerType playerType = E_playerType.Mian;

            int i = (int)playerType;
            Console.Write(i);

            playerType = 0;
            playerType = (E_playerType)1;
            playerType = (E_playerType)3;

            string str = playerType.ToString();
            Console.WriteLine(str);

            playerType = (E_playerType)Enum.Parse(typeof(E_playerType), "Other");
            Console.WriteLine(playerType);
        }
    }
}
