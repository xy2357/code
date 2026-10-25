namespace 实践项目
{
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景
        /// </summary>
        Begin,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 结束场景
        /// </summary>
        End,
    }

    enum E_GridType
    {
        /// <summary>
        /// 表示正常状态。
        /// </summary>
        Normal,
        Boom,
        Pause,
        Tunnel,
    }

    struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    struct Grid
    {
        //格子的类型
        E_GridType type;
        //格子的位置
        public Vector2 pos;

        public Grid(int x, int y, E_GridType type)
        {
            this.pos.x = x;
            this.pos.y = y;
            this.type = type;
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            switch (type)
            {
                case E_GridType.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("□");
                    break;
                case E_GridType.Boom:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("●");
                    break;
                case E_GridType.Pause:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("‖");
                    break;
                case E_GridType.Tunnel:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("◆");
                    break;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            #region 控制台初始化
            int weight = 50;
            int height = 30;
            ConsoleInit(weight, height);
            #endregion

            #region 场景选择相关
            E_SceneType nowSceneType = E_SceneType.Begin;
            while (true)
            {
                switch (nowSceneType)
                {
                    case E_SceneType.Begin:
                        //开始场景逻辑
                        Console.Clear();
                        BeginScene(weight, height, ref nowSceneType);
                        break;
                    case E_SceneType.Game:
                        //游戏场景逻辑
                        Console.Clear();
                        GameScene(weight, height);
                        break;
                    case E_SceneType.End:
                        //结束场景逻辑
                        Console.Clear();
                        break;
                }
            }
            #endregion
        }


        static void ConsoleInit(int weight, int height)
        {
            //基础设置
            //光标的隐藏
            Console.CursorVisible = false;
            //舞台的大小
            Console.SetWindowSize(weight, height);
            Console.SetBufferSize(weight, height);
        }

        #region 开始场景逻辑
        static void BeginScene(int weight, int height, ref E_SceneType nowSceneType)
        {
            Console.SetCursorPosition(weight / 2 - 3, 8);
            Console.WriteLine("飞行棋");

            //选项的标记
            int nowSelIndex = 0;
            bool isQuitBegin;
            //开始场景逻辑循环
            while (true)
            {
                isQuitBegin = false;
                Console.SetCursorPosition(weight / 2 - 4, 13);
                Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("开始游戏");
                Console.SetCursorPosition(weight / 2 - 4, 15);
                Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine("退出游戏");

                //通过ReadKey可以获得一个输入的枚举
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        --nowSelIndex;
                        if (nowSelIndex < 0)
                        {
                            nowSelIndex = 0;
                        }
                        break;
                    case ConsoleKey.S:
                        ++nowSelIndex;
                        if (nowSelIndex > 1)
                        {
                            nowSelIndex = 1;
                        }
                        break;
                    case ConsoleKey.J:
                        if (nowSelIndex == 0)
                        {
                            //改变当前场景ID
                            nowSceneType = E_SceneType.Game;
                            //退出开始场景的主循环
                            isQuitBegin = true;
                        }
                        else if(nowSelIndex == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
                if (isQuitBegin)
                {
                    break;
                }
            }
        }
        #endregion

        #region 游戏场景逻辑
        static void GameScene(int weight, int height)
        {
            //绘制不变的基本信息
            DrawWall(weight, height);
            //绘制地图
            Grid grid = new Grid(5, 5, E_GridType.Normal);
            grid.Draw();
            //绘制玩家

            //游戏场景主循环
            while (true)
            {

            }
        }
        #endregion

        #region 绘制不变内容（红墙 提示等）
        static void DrawWall(int weight, int height)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //横着的墙
            //最上方的墙
            for (int i = 0; i < weight; i += 2)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");

            //最下方的墙
                Console.SetCursorPosition(i, height - 1);
                Console.Write("■");

            //中间的墙
                Console.SetCursorPosition(i, height - 6);
                Console.Write("■");
                Console.SetCursorPosition(i, height - 11);
                Console.Write("■");
            }
            //竖着的墙
            for (int i = 0; i < height; i++)
            {
                //最左边
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                //最右边
                Console.SetCursorPosition(weight - 2, i);
                Console.Write("■");
            }

            //文字信息
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, height - 10);
            Console.WriteLine("□：普通格子");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(2, height - 9);
            Console.WriteLine("‖：暂停，一回合不动");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(26, height - 9);
            Console.WriteLine("●：炸弹，倒退5格");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, height - 8);
            Console.WriteLine("◆：时空隧道：随即倒退，暂停，换位置");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(2, height - 7);
            Console.WriteLine("★：玩家");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(12, height - 7);
            Console.WriteLine("▲：电脑");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(22, height - 7);
            Console.WriteLine("◎：玩家和电脑重合");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, height - 5);
            Console.WriteLine("按任意键仍色子");
        }
        #endregion
    }
}
