namespace 入门实践
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 1 控制台基础设置

            //隐藏光标
            Console.CursorVisible = false;

            int w = 50;
            int h = 30;
            //设置舞台（控制台）的大小
            Console.SetWindowSize(w, h);
            Console.SetBufferSize(w, h);
            #endregion

            #region 2 多个场景

            //当前所在场景的编号
            int nowSceneID = 1;
            while (true) 
            {
                //不同的场景ID 进行不同的逻辑处理
                switch (nowSceneID)
                {
                    //开始场景
                    case 1:
                        Console.Clear();

                        Console.SetCursorPosition(w / 2 - 7, 8);
                        Console.WriteLine("唐老师营救公主");

                        //死循环处理开始场景逻辑
                        while (true)
                        {
                            //显示内容
                            //先设置光标位置 再显示内容
                            Console.SetCursorPosition(w / 2 - 4, 13);
                            Console.Write("开始游戏");
                            Console.SetCursorPosition(w / 2 - 4, 15);
                            Console.Write("退出游戏");
                            //检测输入
                            char input = Console.ReadKey(true).KeyChar;
                            switch (input)
                            {
                                case 'W':
                                case 'w':
                                    break;
                                case 'S':
                                case 's':
                                    break;
                            }
                        }
                        break;
                    //游戏场景
                    case 2:
                        Console.Clear();
                        Console.WriteLine("游戏场景");
                        break;
                    //结束场景
                    case 3:
                        Console.Clear();
                        Console.WriteLine("结束场景");
                        break;

                }
            }

            #endregion

        }
    }
}
