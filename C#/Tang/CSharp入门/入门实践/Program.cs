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
                        //当前选择文字的编号
                        int nowSelIndex = 0;
                        //死循环处理开始场景逻辑
                        while (true)
                        {
                            bool isQuitWhile = false;
                            //显示内容
                            //先设置光标位置 再显示内容
                            Console.SetCursorPosition(w / 2 - 4, 13);
                            Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("开始游戏");
                            Console.SetCursorPosition(w / 2 - 4, 15);
                            Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("退出游戏");
                            //检测输入
                            char input = Console.ReadKey(true).KeyChar;
                            switch (input)
                            {
                                case 'W':
                                case 'w':
                                    --nowSelIndex;
                                    if (nowSelIndex < 0)
                                    {
                                        nowSelIndex = 0;
                                    }
                                    break;
                                case 'S':
                                case 's':
                                    ++nowSelIndex;
                                    if (nowSelIndex > 1)
                                    {
                                        nowSelIndex = 1;
                                    }
                                    break;
                                case 'J':
                                case 'j':
                                    if (nowSelIndex == 0)
                                    {
                                        //1.改变当前学则的场景ID
                                        //2.要退出内层while循环
                                        nowSceneID = 2;
                                        isQuitWhile = true;
                                    }
                                    else
                                    {
                                        //关闭控制台
                                        Environment.Exit(0);
                                    }
                                        break;
                            }
                            if (isQuitWhile)
                            {
                                break;
                            }
                        }
                        break;
                    //游戏场景
                    case 2:
                        Console.Clear();

                        #region 不变的红墙
                        Console.ForegroundColor = ConsoleColor.Red;
                        //画墙
                        for (int i = 0; i < w; i += 2)
                        {
                            //上方墙
                            Console.SetCursorPosition(i, 0);
                            Console.Write("■");
                            //下方墙
                            Console.SetCursorPosition(i, h - 1);
                            Console.Write("■");
                            //中间墙
                            Console.SetCursorPosition(i, h - 6);
                            Console.Write("■");
                        }
                        for (int i = 0; i < h; i++)
                        {
                            //左边墙
                            Console.SetCursorPosition(0, i);
                            Console.Write("■");
                            //右边墙
                            Console.SetCursorPosition(w - 2, i);
                            Console.Write("■");
                        }

                        #endregion

                        #region boss属性相关
                        int bossX = 24;
                        int bossY = 15;
                        int bossAtkMin = 7;
                        int bossAtkMax = 13;
                        int bossHp = 100;
                        string bossIcon = "■";
                        ConsoleColor bossColor = ConsoleColor.Green;
                        #endregion

                        #region 玩家属性相关
                        int playerX = 4;
                        int playerY = 5;
                        int playerAtkMin = 8;
                        int playerAtkMax = 12;
                        int playerHp = 100;
                        string playerIcon = "◆";
                        ConsoleColor playerColor = ConsoleColor.Yellow;
                        char playerInput;
                        #endregion

                        while (true)
                        {
                            //boss血量大于0才显示
                            if (bossHp > 0)
                            {
                                //绘制boss图标
                                Console.SetCursorPosition(bossX, bossY);
                                Console.ForegroundColor = bossColor;
                                Console.Write(bossIcon);
                            }

                            #region 玩家移动相关
                            //画出玩家
                            Console.SetCursorPosition(playerX, playerY);
                            Console.ForegroundColor = playerColor;
                            Console.Write(playerIcon);

                            //读取按键，但不在控制台显示输入的字符
                            playerInput = Console.ReadKey(true).KeyChar;
                            //擦除原本的位置
                            Console.SetCursorPosition(playerX, playerY);
                            Console.Write("  ");
                            //改到新位置
                            switch (playerInput)
                            {
                                case 'W':
                                case 'w':
                                    --playerY;
                                    break;
                                case 'A':
                                case 'a':
                                    playerX -= 2;
                                    break;
                                case 'S':
                                case 's':
                                    ++playerY;
                                    break;
                                case 'D':
                                case 'd':
                                    playerX += 2;
                                    break;
                            }
                            #endregion
                        }
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
