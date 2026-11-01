using System.Collections;
using System.Numerics;

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
        /// 表示格子类型。
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

    #region 格子结构体
    struct Grid
    {
        //格子的类型
        public E_GridType type;
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
    #endregion

    #region 地图结构体
    struct Map
    {
        public Grid[] grids;

        //初始化了各个格子的类型和位置
        public Map(int x, int y, int num)
        {
            this.grids = new Grid[num];

            //用于位置改变计数的变量
            //用于x变化的计数
            int indexX = 0;
            //用于y变化的计数
            int indexY = 0;

            int stepNum = 2;

            Random r = new Random();
            int randomNum;
            for (int i = 0; i < num; i++)
            {
                //初始化格子类型
                randomNum = r.Next(1, 101);

                //设置类型 普通格子
                //85%几率是普通格子 首位两个格子必为普通格子
                if (randomNum < 85 || i == 0 || i == num - 1)
                {
                    grids[i].type = E_GridType.Normal;
                }
                //5%几率是炸弹
                else if (randomNum >= 85 && randomNum < 90)
                {
                    grids[i].type = E_GridType.Boom;
                }
                //5%几率是暂停
                else if (randomNum >= 90 && randomNum < 95)
                {
                    grids[i].type = E_GridType.Pause;
                }
                //5%几率是时空隧道
                else
                {
                    grids[i].type = E_GridType.Tunnel;
                }

                //设置位置
                grids[i].pos = new Vector2(x, y);

                //加十次
                if (indexX == 10)
                {
                    y += 1;
                    //加一次Y记一次数
                    ++indexY;
                    if (indexY == 2)
                    {
                        //y加了2次之后，把x,y次数置0
                        indexX = 0;
                        indexY = 0;

                        //x反向
                        stepNum = -stepNum;
                    }
                }
                else
                {
                    x += stepNum;
                    //加一次X记一次数
                    ++indexX;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i].Draw();
            }
        }
    }
    #endregion

    #region 玩家枚举和玩家结构体
    enum E_PlayerType
    {
        Player,
        Computer,
    }

    struct Player
    {
        public E_PlayerType type;
        //当前所在地图哪一个位置
        public int nowIndex;
        //判断玩家是否处于暂停状态
        public bool isPause;

        public Player(int index, E_PlayerType type)
        {
            nowIndex = index;
            this.type = type;
            isPause = false;
        }

        public void Draw(Map mapInfo)
        {
            //设置位置
            //必须要先得到地图 知道在地图哪一个格子
            Grid grid = mapInfo.grids[nowIndex];
            //设置位置
            Console.SetCursorPosition(grid.pos.x, grid.pos.y);
            //画 设置颜色 设置图标
            switch (type)
            {
                case E_PlayerType.Player:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("★");
                    break;
                case E_PlayerType.Computer:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("▲");
                    break;
            }
        }
    }
    #endregion

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
                        BeginOrEndScene(weight, height, ref nowSceneType);
                        break;
                    case E_SceneType.Game:
                        //游戏场景逻辑
                        Console.Clear();
                        GameScene(weight, height, ref nowSceneType);
                        break;
                    case E_SceneType.End:
                        //结束场景逻辑
                        Console.Clear();
                        BeginOrEndScene(weight, height, ref nowSceneType);
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

        #region 开始场景逻辑 + 结束场景逻辑
        static void BeginOrEndScene(int weight, int height, ref E_SceneType nowSceneType)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(nowSceneType == E_SceneType.Begin ? weight / 2 - 3: weight / 2 - 4, 8);
            Console.WriteLine(nowSceneType == E_SceneType.Begin ? "飞行棋" : "游戏结束");

            //选项的标记
            int nowSelIndex = 0;
            bool isQuitBegin;
            //开始场景逻辑循环
            while (true)
            {
                isQuitBegin = false;
                Console.SetCursorPosition(nowSceneType == E_SceneType.Begin ? weight / 2 - 4 : weight / 2 - 5, 13);
                Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine(nowSceneType == E_SceneType.Begin ? "开始游戏" : "回到主菜单");

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
                            nowSceneType = nowSceneType == E_SceneType.Begin ? E_SceneType.Game : E_SceneType.Begin;
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
        static void GameScene(int weight, int height, ref E_SceneType nowSecneType)
        {
            //绘制不变的基本信息
            DrawWall(weight, height);
            //绘制地图
            Map map = new Map(14, 3, 82);
            map.Draw();
            //绘制玩家
            Player player = new Player(0, E_PlayerType.Player);
            Player computer = new Player(0, E_PlayerType.Computer);
            DrawPlayer(player, computer, map);

            //游戏场景主循环
            while (true)
            {
                //玩家扔色子逻辑
                if (PlayerRandomMove(weight, height, ref player, ref computer, map, ref nowSecneType))
                {
                    break;
                }

                //电脑扔色子逻辑
                if (PlayerRandomMove(weight, height, ref computer, ref player, map, ref nowSecneType))
                {
                    break;
                }
            }
        }
        #endregion

        #region 玩家移动逻辑
        static bool PlayerRandomMove(int weight, int height, ref Player p, ref Player otherP, Map map, ref E_SceneType nowSecneType)
        {
            //检测输入
            Console.ReadKey(true);
            //扔色字的逻辑
            bool isGameOver = RandomMove(weight, height, ref p, ref otherP, map);
            //绘制地图
            map.Draw();
            //绘制玩家
            DrawPlayer(p, otherP, map);
            //判断是否要结束游戏
            if (isGameOver)
            {
                //卡住程序，让玩家按任意键
                Console.ReadKey(true);
                //改变场景ID
                nowSecneType = E_SceneType.End;
                //跳出场景
            }
            return isGameOver;
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

        #region 绘制玩家
        static void DrawPlayer(Player player,Player computer, Map map)
        {
            //重合时
            if (player.nowIndex == computer.nowIndex)
            {
                //得到重合的位置
                Grid grid = map.grids[player.nowIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("◎");
            }
            //不重合时
            else
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }
        #endregion

        #region 扔色子

        //擦除函数
        static void ClearInfo(int weight, int height)
        {
            Console.SetCursorPosition(2, height - 5);
            Console.WriteLine("                                  ");
            Console.SetCursorPosition(2, height - 4);
            Console.WriteLine("                                  ");
            Console.SetCursorPosition(2, height - 3);
            Console.WriteLine("                                  ");
            Console.SetCursorPosition(2, height - 2);
            Console.WriteLine("                                  ");
        }
        static bool RandomMove(int weight, int height, ref Player p,ref Player otherP, Map map)
        {
            //擦除之前显示的提示信息
            ClearInfo(weight, height);

            //根据玩家类型显示提示文字的颜色
            Console.ForegroundColor = p.type == E_PlayerType.Player ? ConsoleColor.Cyan : ConsoleColor.Magenta;

            //扔色子之前判断玩家是否处于暂停状态
            if (p.isPause)
            {
                Console.SetCursorPosition(2, height - 5);
                Console.WriteLine("处于暂停点，{0}需要暂停一回合！", p.type == E_PlayerType.Player ? "你" : "电脑");
                Console.SetCursorPosition(2, height - 4);
                Console.WriteLine("请按任意键，让{0}继续游戏", p.type == E_PlayerType.Player ? "电脑" : "你");
                p.isPause = false;
                return false;
            }

            //扔色子目的是改变玩家位置

            //扔色子，随机一个数加上去
            Random r = new Random();
            int randomNum = r.Next(1, 7);
            p.nowIndex += randomNum;

            //打印扔的点数
            Console.SetCursorPosition(2, height - 5);
            Console.WriteLine("{0}扔出的点数是：{1}", p.type == E_PlayerType.Player ? "你" : "电脑", randomNum);


            //首先判断是否到终点
            if (p.nowIndex >= map.grids.Length - 1)
            {
                p.nowIndex = map.grids.Length - 1;

                if (p.type == E_PlayerType.Player)
                {
                    Console.SetCursorPosition(2, height - 4);
                    Console.WriteLine("恭喜，你先到达了终点！");
                }
                else
                {
                    Console.SetCursorPosition(2, height - 4);
                    Console.WriteLine("电脑先到达了终点！");
                }
                Console.SetCursorPosition(2, height - 3);
                Console.WriteLine("请按任意键结束游戏");
                return true;
            }
            else
            {
                //没到终点
                Grid grid = map.grids[p.nowIndex];
                switch (grid.type)
                {
                    case E_GridType.Normal:
                        Console.SetCursorPosition(2, height - 4);
                        Console.WriteLine("{0}到了一个安全位置", p.type == E_PlayerType.Player ? "你" : "电脑");
                        Console.SetCursorPosition(2, height - 3);
                        Console.WriteLine("请按任意键，让{0}继续游戏", p.type == E_PlayerType.Player ? "电脑" : "你");
                        break;
                    case E_GridType.Boom:
                        //炸弹退格
                        p.nowIndex -= 5;
                        if (p.nowIndex < 0)
                        {
                            p.nowIndex = 0;
                        }
                        Console.SetCursorPosition(2, height - 4);
                        Console.WriteLine("{0}踩到了炸弹，退后5格", p.type == E_PlayerType.Player ? "你" : "电脑");
                        Console.SetCursorPosition(2, height - 3);
                        Console.WriteLine("请按任意键，让{0}继续游戏", p.type == E_PlayerType.Player ? "电脑" : "你");
                        break;
                    case E_GridType.Pause:
                        //暂停一回合
                        p.isPause = true;
                        Console.SetCursorPosition(2, height - 4);
                        Console.WriteLine("{0}踩到了暂停点，需要暂停一回合", p.type == E_PlayerType.Player ? "你" : "电脑");
                        Console.SetCursorPosition(2, height - 3);
                        Console.WriteLine("请按任意键，让{0}继续游戏", p.type == E_PlayerType.Player ? "电脑" : "你");
                        break;
                    case E_GridType.Tunnel:
                        //随机
                        Console.SetCursorPosition(2, height - 4);
                        Console.WriteLine("{0}踩到了时空隧道", p.type == E_PlayerType.Player ? "你" : "电脑");
                        randomNum = r.Next(1, 91);
                        if (randomNum <= 30)
                        {
                            p.nowIndex -= 5;
                            if (p.nowIndex < 0)
                            {
                                p.nowIndex = 0;
                            }
                            Console.SetCursorPosition(2, height - 3);
                            Console.WriteLine("触发了倒退5格");
                        }
                        else if(randomNum <= 30)
                        {
                            p.isPause = true;
                            Console.SetCursorPosition(2, height - 3);
                            Console.WriteLine("触发暂停一回合");
                        }
                        else
                        {
                            int temp = p.nowIndex;
                            p.nowIndex = otherP.nowIndex;
                            otherP.nowIndex = temp;
                            Console.SetCursorPosition(2, height - 3);
                            Console.WriteLine("双方交换位置");
                        }
                        break;
                }
            }
            //默认没有结束
            return false;
        }
        #endregion
    }
}
