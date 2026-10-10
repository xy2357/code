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

                        #region 公主相关属性
                        int princessX = 24;
                        int princessY = 5;
                        string princessIcon = "★";
                        ConsoleColor princessColor = ConsoleColor.Blue;
                        #endregion

                        //判断玩家是否处于战斗
                        bool isFight = false;

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
                            else
                            {
                                //绘制公主
                                Console.SetCursorPosition(princessX, princessY);
                                Console.ForegroundColor = princessColor;
                                Console.Write(princessIcon);
                            }

                            #region 玩家移动相关
                            //画出玩家
                            Console.SetCursorPosition(playerX, playerY);
                            Console.ForegroundColor = playerColor;
                            Console.Write(playerIcon);

                            //读取按键，但不在控制台显示输入的字符
                            playerInput = Console.ReadKey(true).KeyChar;

                            //战斗状态
                            if (isFight)
                            {
                                if (playerInput == 'J' || playerInput == 'j')
                                {
                                    //在这判断 玩家或者怪物是否死亡
                                    if (playerHp <= 0)
                                    {
                                        //游戏结束
                                        //输掉直接显示游戏结束界面
                                        nowSceneID = 3;
                                        break;
                                    }
                                    else if (bossHp <= 0)
                                    {
                                        //去营救公主
                                        //boss擦除
                                        Console.SetCursorPosition(bossX, bossY);
                                        Console.Write("  ");
                                        isFight = false;
                                    }
                                    else
                                    {
                                        //玩家打怪物
                                        //玩家随机攻击力
                                        Random r = new Random();
                                        int atk = r.Next(playerAtkMin, playerAtkMax);
                                        //boss掉血
                                        bossHp -= atk;
                                        //打印信息
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        //先擦除上次打印的信息
                                        Console.SetCursorPosition(2, h - 4);
                                        Console.Write("                                         ");
                                        //再打印新的信息
                                        Console.SetCursorPosition(2, h - 4);
                                        Console.Write("你对boss造成了{0}伤害，boss还剩余{1}血量", atk, bossHp);
                                        //怪物打玩家
                                        if (bossHp > 0)
                                        {
                                            //boss随机攻击力
                                            atk = r.Next(playerAtkMin, playerAtkMax);
                                            //玩家掉血
                                            playerHp -= atk;
                                            //打印信息
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            //先擦除上次打印的信息
                                            Console.SetCursorPosition(2, h - 3);
                                            Console.Write("                                         ");
                                            //再打印新的信息
                                            if (playerHp <= 0)
                                            {
                                                Console.SetCursorPosition(2, h - 3);
                                                Console.Write("你死了，你未能通过boss的试炼");
                                            }
                                            else
                                            {
                                                Console.SetCursorPosition(2, h - 3);
                                                Console.Write("boss对你造成了{0}伤害，玩家还剩余{1}血量", atk, playerHp);
                                            }
                                        }
                                        else
                                        {
                                            //擦除之前的信息
                                            Console.SetCursorPosition(2, h - 5);
                                            Console.Write("                                         ");
                                            Console.SetCursorPosition(2, h - 4);
                                            Console.Write("                                         ");
                                            Console.SetCursorPosition(2, h - 3);
                                            Console.Write("                                         ");
                                            //显示胜利的信息
                                            Console.SetCursorPosition(2, h - 5);
                                            Console.Write("你战胜了boss，快去营救公主");
                                            Console.SetCursorPosition(2, h - 4);
                                            Console.Write("前往公主身边按J键继续");
                                        }
                                    }
                                }
                            }
                            else //非战斗状态
                            {
                                //擦除原本的位置
                                Console.SetCursorPosition(playerX, playerY);
                                Console.Write("  ");
                                //改到新位置
                                switch (playerInput)
                                {
                                    case 'W':
                                    case 'w':
                                        --playerY;
                                        if (playerY < 1)
                                        {
                                            playerY = 1;
                                        }
                                        else if (playerX == bossX && playerY == bossY && bossHp > 0)
                                        {
                                            ++playerY;
                                        }
                                        break;
                                    case 'A':
                                    case 'a':
                                        playerX -= 2;
                                        if (playerX < 2)
                                        {
                                            playerX = 2;
                                        }
                                        else if (playerX == bossX && playerY == bossY && bossHp > 0)
                                        {
                                            playerX += 2;
                                        }
                                        break;
                                    case 'S':
                                    case 's':
                                        ++playerY;
                                        if (playerY > h - 7)
                                        {
                                            playerY = h - 7;
                                        }
                                        else if (playerX == bossX && playerY == bossY && bossHp > 0)
                                        {
                                            --playerY;
                                        }
                                        break;
                                    case 'D':
                                    case 'd':
                                        playerX += 2;
                                        if (playerX > w - 4)
                                        {
                                            playerX = w - 4;
                                        }
                                        else if (playerX == bossX && playerY == bossY && bossHp > 0)
                                        {
                                            playerX -= 2;
                                        }
                                        break;
                                    case 'J':
                                    case 'j':
                                        //开始战斗
                                        if ((playerX == bossX && playerY == bossY - 1 ||
                                            playerX == bossX && playerY == bossY + 1 ||
                                            playerX == bossX - 2 && playerY == bossY ||
                                            playerX == bossX + 2 && playerY == bossY) && bossHp > 0)
                                        {
                                            isFight = true;
                                            Console.SetCursorPosition(2, h - 5);
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.Write("开始和boss战斗,按J键继续");
                                            Console.SetCursorPosition(2, h - 4);
                                            Console.Write("玩家的血量为{0}", playerHp);
                                            Console.SetCursorPosition(2, h - 3);
                                            Console.Write("Boss的血量为{0}", bossHp);
                                        }
                                        //玩家不能移动
                                        //下方显示战斗信息
                                        break;
                                }
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
