using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    public enum GameLevel
    {
        easy =0, 
        normal ,
        hard , 
        lunatic,
        extra
    }
    //记录当前的全局数据
    public class Datas
    {
        static Datas()
        {
            GameRandom = new GRandom();
        }

        //核心随机器
        public static GRandom GameRandom;

        public static bool ReFre = false ;     //是否处于重播模式、
        public static Random LooselyRandom = new Random();   //使用时机不严谨的随机数生成器（可随时调用，不会影响系统逻辑）     

        public static List<string> Commands;              //从磁盘上读取的命令队列(或者是在运行游戏的过程中所记录的命令队列)

        public static SoundManagerEx SoundManager_Static; //共享的声音处理器

        public static string Adress = "192.168.191.1";                //服务器使用的IP地址

        public static bool ShowCollisionRect = true;

        public static int TestX, TestY;      //测试地点
        public static String SelectGear = "";//测试齿轮种类
        public static int Width, Height;     //测试的类型的宽度和高度

        public static void WirteData(string command)
        {
            Commands.Add(command);
        }


    }

    public class GRandom
    {
        public Random random;
        int seed;
        public GRandom()
        {
            long s = DateTime.Now.Ticks;
            seed = (int)(s % 100000000);
            random = new Random(seed);
        }
        public GRandom(int seed)
        {
            this.seed = seed;
            random = new Random(seed);
        }

        public int Seed
        {
            get { return seed; }
            set 
            {
                seed = value;
                random = new Random(seed);
            }
        }
        public double NextDouble()
        {
            return random.NextDouble();
        }

    }



}


