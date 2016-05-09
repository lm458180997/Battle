using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages.Menu
{
    public class Shade :IGameObject
    {
        public bool show = false;          //是否显示
        
        DoorL doorL;     //"左门"组件
        DoorR doorR;     //"右门"组件
        TopGears topGears;   //"上齿轮" 组件
        BottomGears bottomGears; //"下齿轮" 组件

        TextureManager texturemanager;

        public Shade(TextureManager _t)
        {
            texturemanager = _t;
            doorL = new DoorL(_t);
            doorR = new DoorR(_t);
            topGears = new TopGears(_t);
            bottomGears = new BottomGears(_t);
        }

        public void Show()
        {
            doorL.Show();
            doorR.Show();
            topGears.Show();
            bottomGears.Show();
            show = true;
        }

        public void Hide()
        {
            doorL.Hide();
            doorR.Hide();
            topGears.Hide();
            bottomGears.Hide();
            show = false;
        }

        public void Start()
        {
            doorL.Start();
            doorR.Start();
            topGears.Start();
            bottomGears.Start();
        }

        public void Update(double elapsedTime)
        {
            doorL.Update(elapsedTime);
            doorR.Update(elapsedTime);
            topGears.Update(elapsedTime);
            bottomGears.Update(elapsedTime);
        }

        public void Render()
        {
            doorL.Render();
            doorR.Render();
            topGears.Render();
            bottomGears.Render();
        }
    }

    /// <summary>
    /// 左边的门
    /// </summary>
    public class DoorL : IGameObject
    {
        TextureManager texturemanager;
        bool show = false;               //是否处于显示状态
        Vector2D Position = new Vector2D();   //坐标
        Animation animation=null;             //动画
        Renderer renderer = new Renderer();   //着色器
        Sprite door = new Sprite();           //着色精灵
        int X = -430;                         //初始（隐藏）时候的坐标
        int Aim_X = -88;                      //目标坐标
        double anitime = 0.3;                 //动画的执行时间
        public DoorL(TextureManager _t)
        {
            texturemanager = _t;
            Position.X = X;
            Position.Y = 100;
            door.Texture = texturemanager.Get("door_l");
            door.SetWidth(512); door.SetHeight(512);
            door.SetUVs(0.01f, 0.01f, 0.99f, 0.99f);
        }

        public void Start() {}

        /// <summary>
        /// 触发进入展示状态
        /// </summary>
        public void Show()
        {
            show = true;
            animation = new MoveAnimation(Position, new Vector2D(Aim_X, 100), anitime);
        }
        /// <summary>
        /// 触发进入隐藏状态
        /// </summary>
        public void Hide()
        {
            show = false;
            animation = new MoveAnimation(Position, new Vector2D(X, 100), anitime);
        }

        public void Update(double elapsedTime)
        {
            if (animation != null)
            {
                if (animation.working)
                {
                    animation.Update(elapsedTime);
                }
            }
        }

        public void Render()
        {
            door.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(door);
        }
    }

    /// <summary>
    /// 右边的门
    /// </summary>
    public class DoorR : IGameObject
    {
        TextureManager texturemanager;
        public bool show = false;               //是否处于显示状态
        Vector2D Position = new Vector2D();   //坐标
        Animation animation = null;             //动画
        Renderer renderer = new Renderer();   //着色器
        Sprite door = new Sprite();           //着色精灵
        int X = 430;                         //初始（隐藏）时候的坐标
        int Aim_X = 88;                      //目标坐标
        double anitime = 0.3;                 //动画的执行时间
        public DoorR(TextureManager _t)
        {
            texturemanager = _t;
            Position.X = X;
            Position.Y = 100;
            door.Texture = texturemanager.Get("door_r");
            door.SetWidth(512); door.SetHeight(512);
            door.SetUVs(0.01f, 0.01f, 0.99f, 0.99f);
        }

        public void Start() { }

        /// <summary>
        /// 触发进入展示状态
        /// </summary>
        public void Show()
        {
            show = true;
            animation = new MoveAnimation(Position, new Vector2D(Aim_X, 100), anitime);
        }
        /// <summary>
        /// 触发进入隐藏状态
        /// </summary>
        public void Hide()
        {
            show = false;
            animation = new MoveAnimation(Position, new Vector2D(X, 100), anitime);
        }

        public void Update(double elapsedTime)
        {
            if (animation != null)
            {
                if (animation.working)
                {
                    animation.Update(elapsedTime);
                }
            }
        }

        public void Render()
        {
            door.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(door);
        }
    }

    /// <summary>
    /// 齿轮[作为一个整体组件的一个小构件，或是作为独立工作的一个组件]
    /// </summary>
    public class Gear : IGameObject
    {
        public int Tag = 0;        //标签（用于标识种类）
        public String Name="Gear"; //名字（用于标识种类）
        public Sprite sprite = new Sprite();
        public Vector2D Position = new Vector2D(); //坐标
        public Vector2D FatherPostion = new Vector2D();//父容器的Vector2D组件（产生相对坐标）,默认为自己
        double startangle = 0;              //初始旋转角
        double angle = 0;                   //旋转角
        public double rotate_speed = 20;           //旋转速度  /s
        Renderer renderer = new Renderer(); //着色器
        double showx = 0;   //渲染的世界坐标x
        double showy = 0;   //渲染的世界坐标y

        /// <summary>
        /// 通过构造注入信息，完成齿轮的构建
        /// </summary>
        /// <param name="texture">齿轮对应的纹理</param>
        /// <param name="width">齿轮对应的宽度</param>
        /// <param name="height">齿轮对应的高度</param>
        /// <param name="Pos">齿轮的所在坐标</param>
        /// <param name="ag">初始的旋转角</param>
        public Gear(Texture texture, int width, int height, Vector2D Pos,double ag)
        {
            startangle = ag;
            angle = ag;
            sprite.Texture = texture;
            sprite.SetWidth(width);
            sprite.SetHeight(height);
            Position.X = Pos.X;
            Position.Y = Pos.Y;
        }
        /// <summary>
        /// 设定父容器[坐标会变为相对于父容器的相对坐标]
        /// </summary>
        /// <param name="vct"></param>
        public void SetParent(Vector2D vct)
        {
            FatherPostion = vct;
        }
        /// <summary>
        /// 设定名字
        /// </summary>
        /// <param name="name"></param>
        public void SetName(String name)
        {
            Name = name;
        }
        /// <summary>
        /// 设定标签
        /// </summary>
        /// <param name="tag">标签</param>
        public void SetTag(int tag)
        {
            Tag = tag;
        }
        /// <summary>
        /// 设置齿轮的旋转速度
        /// </summary>
        /// <param name="spd">旋转速度</param>
        public void SetSpeed(double spd)
        {
            rotate_speed = spd;
        }
        /// <summary>
        /// Start()接口
        /// </summary>
        public void Start()
        {
            angle = startangle;
        }
        /// <summary>
        /// 逻辑更新
        /// </summary>
        /// <param name="elapsedTime">帧间差</param>
        public void Update(double elapsedTime)
        {
            angle += elapsedTime * rotate_speed;
        }
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render()
        {
            showx = FatherPostion.X + Position.X;
            showy = FatherPostion.Y + Position.Y;
            sprite.SetPosition(showx,showy);
            renderer.DrawSprite(sprite,showx,showy,(float)angle);
        }
    }

    /// <summary>
    /// 顶部的一系列齿轮
    /// </summary>
    public class TopGears : IGameObject
    {
        public bool show = false;
        public int Y = 540;       
        public int Aim_Y = 300;
        public Vector2D Position = new Vector2D();
        double anitime = 0.3;
        Renderer renderer = new Renderer();
        
        Sprite back = new Sprite();   //底色图片
        double back_x = 0, back_y = 0;
        Sprite frame = new Sprite();  //底部的框架
        double frame_x=0, frame_y=10;      //框架的相对坐标

        TextureManager texturemanager;
        Animation animation = null;
        //----------设计用变量----------//
        Sprite flag = new Sprite();  //标识指针（确定当前制定位置）
        List<Gear> Gears = new List<Gear>();  //调试用的轮子
        bool turnleft = true;
        Gear showGear;
        double showx = 0, showy = 100;
        bool showCurrent = false; //指标
        bool showPro = false;     //例子
        //----------设计用变量----------//

        //----------齿轮-----------
        Gear[] WorkingGears= new Gear[25];//齿轮缓冲区
        int count = 0;                    //当前已有齿轮数量
        String[] Names = new String[14];  //名字
        int[] width = new int[14];        //纹理宽度
        int[] realwidth = new int[14];    //真实宽度
        int selectKind = 0;               //当前选择的种类的索引值
        //建立映射关系（索引值--名字--宽度--真实宽度）
        void initKinds()
        {
            Names[0] = "2L-front_L";
            width[0] = 128;
            realwidth[0] = 128;
            Names[1] = "2L-front_M";
            width[1] = 128;
            realwidth[1] = 64;
            Names[2] = "2L-front_S";
            width[2] = 32;
            realwidth[2] = 32;
            Names[3] = "2U-front_L";
            width[3] = 128;
            realwidth[3] = 128;
            Names[4] = "2U-front_M";
            width[4] = 128;
            realwidth[4] = 64;
            Names[5] = "2U-front_S";
            width[5] = 32;
            realwidth[5] = 32;
            Names[6] = "4L-mid_L";
            width[6] = 512;
            realwidth[6] = 320;
            Names[7] = "4L-mid_M";
            width[7] = 256;
            realwidth[7] = 160;
            Names[8] = "4L-mid_S";
            width[8] = 128;
            realwidth[8] = 80;
            Names[9] = "4U-mid_M";
            width[9] = 256;
            realwidth[9] = 160;
            Names[10] = "4U-mid_S";
            width[10] = 128;
            realwidth[10] = 80;
            Names[11] = "5L-back_L";
            width[11] = 512;
            realwidth[11] = 400;
            Names[12] = "5L-back_M";
            width[12] = 256;
            realwidth[12] = 160;
            Names[13] = "5U-back_M";
            width[13] = 256;
            realwidth[13] = 160;
            Datas.SelectGear = Names[selectKind];
        }
        //初始化有效齿轮
        void initgears()
        {
            Create(0, "2U-front_L", 3, new Vector2D(-300, -5), 0, 16);
            Create(1, "2U-front_M", 4, new Vector2D(-300, -5), 0, 32);
            Create(2, "4U-mid_M", 9, new Vector2D(-216, 21), 0, -16);
            Create(3, "2L-front_L", 3, new Vector2D(-216, 21), 0, -16);

            Create(4, "2U-front_S", 5, new Vector2D(-143, 30), 18, 64);
            Create(5, "2U-front_S", 5, new Vector2D(-118, 38), 0, -64);
            Create(6, "2U-front_S", 5, new Vector2D(-115, 12), 0, 64);
            Create(7, "2L-front_M", 1, new Vector2D(-75, 0), 12, -32);
            Create(8, "2L-front_L", 0, new Vector2D(13, 8), 20, 16);
            Create(9, "2L-front_M", 1, new Vector2D(13, 8), 4, 32);
            Create(10, "2L-front_M", 1, new Vector2D(65, 28), 0, -32);
            Create(11, "2U-front_L", 3, new Vector2D(154, 13), 4, 16);
            Create(12, "2L-front_M", 1, new Vector2D(154, 13), 0, 32);
            Create(13, "2L-front_L", 0, new Vector2D(273, -7), 4, -16);
            Create(14, "2L-front_M", 1, new Vector2D(273, -7), 4, -64);
            Create(15, "2L-front_M", 1, new Vector2D(240, 39), 0, 64);
            Create(16, "4L-mid_S", 8, new Vector2D(-106, -4), 0, 64);
            Create(17, "5U-back_M", 13, new Vector2D(-44, 21), 0, -34);
            Create(18, "4L-mid_S", 8, new Vector2D(92, -4), 0, 32);
            Create(19, "5L-back_M", 12, new Vector2D(259, 25), 0, -20);
            Create(20, "4L-mid_S", 8, new Vector2D(-44, 21), 0, -64);
            count = 21;
        }
        //initgears的辅助函数
        void Create(int index, String name, int kind, Vector2D pos, int ag,double speed)
        {
            WorkingGears[index] = new Gear(texturemanager.Get(name), width[kind], width[kind],
                pos, ag);
            WorkingGears[index].rotate_speed = speed;
            WorkingGears[index].SetParent(Position);
        }

        public TopGears(TextureManager _t)
        {
            texturemanager = _t;
            Position.Y = Y;

            frame.Texture = _t.Get("3U-frame");
            frame.SetWidth(1024); frame.SetHeight(1024);
            back.Texture = _t.Get("6U_pattern");
            back.SetWidth(1024); back.SetHeight(1024);

            //初始化显示内容（映射表）
            initKinds();
            //设计时使用的展示组件
            showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                    width[selectKind], new Vector2D(showx, showy), 0);

            //初始化齿轮（齿轮序列）
            initgears();

            flag.Texture = texturemanager.Get("bg_cursor0");
            flag.SetWidth(15);
            flag.SetHeight(15);
        }

        public void Show()
        {
            show = true;
            animation = new MoveAnimation(Position, new Vector2D(0, Aim_Y), anitime);
        }
        public void Hide()
        {
            show = false;
            animation = new MoveAnimation(Position, new Vector2D(0, Y), anitime);
        }
        public void Start()
        {

        }

        public void Update(double elapsedTime)
        {
            if (animation != null)
            {
                if (animation.working)
                {
                    animation.Update(elapsedTime);
                }
            }
            #region 设计逻辑
            //设计时使用的参考齿轮
            showGear.Update(elapsedTime);
            //设计下的齿轮序列
            foreach (Gear g in Gears)
            {
                g.Update(elapsedTime);
            }
            //设计前翻
            if (Input.getKeyDown("A"))
            {
                int s = selectKind - 1;
                if (s < 0)
                    s = 13;
                selectKind = s;
                Datas.SelectGear = Names[selectKind];
                showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                    width[selectKind], new Vector2D(showx, showy), 0);
            }
            //切换设计模式下的齿轮旋转方向
            if (Input.getKeyDown("C"))
            {
                turnleft = !turnleft;
            }
            //设计后翻
            if (Input.getKeyDown("S"))
            {
                int s = selectKind + 1;
                if (s >= 14)
                    s = 0;
                selectKind = s;
                Datas.SelectGear = Names[selectKind];
                showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                    width[selectKind], new Vector2D(showx, showy), 0);
            }
            //根据当前信息，添加一个齿轮进设计序列
            if (Input.getKeyDown("D"))
            {
                Add();
            }
            //删除最近的一个设计齿轮
            if (Input.getKeyDown("Q"))
            {
                if (Gears.Count != 0)
                    Gears.RemoveAt(Gears.Count - 1);
            }
            //将设计记录保存至Data.txt中
            if (Input.getKeyDown("R"))
            {
                WriteData();
            }
            if (Input.getKeyDown("I")) { Datas.TestY += 1; }
            if (Input.getKeyDown("K")) { Datas.TestY -= 1; }
            if (Input.getKeyDown("J")) { Datas.TestX -= 1; }
            if (Input.getKeyDown("L")) { Datas.TestX += 1; }
            if (Input.getKeyDown("M")) { showCurrent = !showCurrent; }
            if (Input.getKeyDown("N")) { showPro = !showPro; }
            #endregion
            //对所有有效齿轮进行逻辑更新
            for (int i = 0; i < count; i++)
            {
                WorkingGears[i].Update(elapsedTime);
            }

        }

        /// <summary>
        /// 给定参数后添加一个齿轮进设计列表（仅在设计时使用）[这里已经放弃使用本函数]
        /// </summary>
        /// <param name="select">选择项</param>
        /// <param name="_x">坐标x</param>
        /// <param name="_y">坐标y</param>
        /// <param name="flag">旋转方向</param>
        public void Add(int select ,int _x, int _y,int flag)
        {
            Gear g = new Gear(texturemanager.Get(Names[select]), width[select],
               width[select], new Vector2D(_x, _y), 0);

            g.rotate_speed = flag * 64 / (realwidth[select] / (float)32);
            g.SetParent(Position);
            g.SetName(Names[select]);
            Gears.Add(g);
        }
        /// <summary>
        /// 设计逻辑中需要调用的函数[用于添加一个测试点]
        /// </summary>
        public void Add()
        {
            double _x = Datas.TestX - Position.X;
            double _y = Datas.TestY - Position.Y;
            Gear g = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                width[selectKind], new Vector2D(_x, _y), 0);
            if (turnleft)
            {
                g.rotate_speed = 64 / (realwidth[selectKind] / 32);
            }
            else
            {
                g.rotate_speed = -64 / (realwidth[selectKind] / 32);
            }
            g.SetParent(Position);
            g.SetName(Names[selectKind]);
            g.SetTag(selectKind);
            Gears.Add(g);
        }
        //设计逻辑中用于记录添加点信息
        public void WriteData()
        {
            StreamWriter wrd = new StreamWriter("Datas.txt", false); //记录齿轮的位置的信息
            foreach (Gear g in Gears)
            {
                wrd.WriteLine(g.Name + "  X:" + g.Position.X + "  Y:" + g.Position.Y + " speed:"
                    + g.rotate_speed.ToString() + " selectindex:" + g.Tag.ToString());
            }
            wrd.Close();
        }
         
        public void Render()
        {
            //底色
            back.SetPosition(Position.X + back_x, Position.Y + back_y);
            renderer.DrawSprite(back);

            //后层
            WorkingGears[2].Render();
            WorkingGears[16].Render();
            WorkingGears[17].Render();
            WorkingGears[20].Render();
            WorkingGears[18].Render();
            WorkingGears[19].Render();
          
            //框架
            frame.SetPosition(Position.X + frame_x, Position.Y + frame_y);
            renderer.DrawSprite(frame);
            //前层
            WorkingGears[0].Render();
            WorkingGears[1].Render();
            WorkingGears[3].Render();
            WorkingGears[4].Render();
            WorkingGears[5].Render();
            WorkingGears[6].Render();
            WorkingGears[7].Render();
            WorkingGears[8].Render();
            WorkingGears[9].Render();
            WorkingGears[10].Render();
            WorkingGears[11].Render();
            WorkingGears[12].Render();
            WorkingGears[14].Render();
            WorkingGears[15].Render();
            WorkingGears[13].Render();



            //设计列表中的齿轮
            foreach (Gear g in Gears)
            {
                g.Render();
            }
          //------------设计逻辑----------------//
            //设计时使用的齿轮
            if(showPro)
            showGear.Render();
            if (showCurrent)
            {
                flag.SetPosition(Datas.TestX, Datas.TestY);
                renderer.DrawSprite(flag);
            }
          //------------设计逻辑----------------//
        }
    }

    /// <summary>
    /// 底部的一系列齿轮
    /// </summary>
    public class BottomGears : IGameObject
    {
        public bool show = false;
        public int Y = -250;
        public int Aim_Y = -100;
        public Vector2D Position = new Vector2D();
        double anitime = 0.3;
        Renderer renderer= new Renderer();

        Sprite back = new Sprite();   //底色图片
        double back_x = 0, back_y = -20;
        Sprite frame = new Sprite();  //底部的框架
        double frame_x=0, frame_y=-10;      //框架的相对坐标

        TextureManager texturemanager;
        Animation animation = null;
        //----------设计用变量----------//
        Sprite flag = new Sprite();  //标识指针（确定当前制定位置）
        List<Gear> Gears = new List<Gear>();  //调试用的轮子
        bool turnleft = true;
        Gear showGear;
        double showx = 0, showy = 100;
        //----------设计用变量----------//

        //----------齿轮-----------
        Gear[] WorkingGears= new Gear[15];//齿轮缓冲区
        int count = 0;                    //当前已有齿轮数量
        String[] Names = new String[14];  //名字
        int[] width = new int[14];        //纹理宽度
        int[] realwidth = new int[14];    //真实宽度
        int selectKind = 0;               //当前选择的种类的索引值
        //建立映射关系（索引值--名字--宽度--真实宽度）
        void initKinds()
        {
            Names[0] = "2L-front_L";
            width[0] = 128;
            realwidth[0] = 128;
            Names[1] = "2L-front_M";
            width[1] = 128;
            realwidth[1] = 64;
            Names[2] = "2L-front_S";
            width[2] = 32;
            realwidth[2] = 32;
            Names[3] = "2U-front_L";
            width[3] = 128;
            realwidth[3] = 128;
            Names[4] = "2U-front_M";
            width[4] = 128;
            realwidth[4] = 64;
            Names[5] = "2U-front_S";
            width[5] = 32;
            realwidth[5] = 32;
            Names[6] = "4L-mid_L";
            width[6] = 512;
            realwidth[6] = 320;
            Names[7] = "4L-mid_M";
            width[7] = 256;
            realwidth[7] = 160;
            Names[8] = "4L-mid_S";
            width[8] = 128;
            realwidth[8] = 80;
            Names[9] = "4U-mid_M";
            width[9] = 256;
            realwidth[9] = 160;
            Names[10] = "4U-mid_S";
            width[10] = 128;
            realwidth[10] = 80;
            Names[11] = "5L-back_L";
            width[11] = 512;
            realwidth[11] = 400;
            Names[12] = "5L-back_M";
            width[12] = 256;
            realwidth[12] = 160;
            Names[13] = "5U-back_M";
            width[13] = 256;
            realwidth[13] = 160;
            Datas.SelectGear = Names[selectKind];
        }
        //初始化有效齿轮
        void initgears()
        {
            Create(0, "2U-front_L", 3, new Vector2D(-104, -38), 0, -16);
            Create(1, "2U-front_S", 5, new Vector2D(-42, 3), 0, 64);
            Create(2, "2L-front_M", 1, new Vector2D(-4, -17),11, -32);
            Create(3, "2U-front_M", 4, new Vector2D(77, -35), 0, -32);
            Create(4, "4U-mid_S", 10, new Vector2D(278, -9), 1, 32);
            Create(5, "4L-mid_M", 7, new Vector2D(182, -65), 0, -16);
            Create(7, "2U-front_L", 3, new Vector2D(182, -65), 0, -16);
            Create(6, "5L-back_L", 11, new Vector2D(-104, -168), 0, -5);
            Create(8, "2U-front_L", 0, new Vector2D(-293, -48), 0, -16);
            Create(9, "4L-mid_S", 8, new Vector2D(-302, -14), 0, 64);
            Create(10, "4L-mid_L", 6, new Vector2D(-152, -141), 0, -16);
            Create(11, "5L-back_M", 12, new Vector2D(107, -76), 0, 24);
            Create(12, "5L-back_M", 12, new Vector2D(265, -65), 0, -32);
            count = 13;
        }
        //initgears的辅助函数
        void Create(int index, String name, int kind, Vector2D pos, int ag,double speed)
        {
            WorkingGears[index] = new Gear(texturemanager.Get(name), width[kind], width[kind],
                pos, ag);
            WorkingGears[index].rotate_speed = speed;
            WorkingGears[index].SetParent(Position);
        }
        
        public BottomGears(TextureManager _t)
        {
            texturemanager = _t;
            Position.Y = Y;

            frame.Texture = _t.Get("3L-frame");
            frame.SetWidth(1024); frame.SetHeight(1024);
            back.Texture = _t.Get("6L_pattern");
            back.SetWidth(1024); back.SetHeight(1024);

            //初始化显示内容（映射表）
            initKinds();
            //设计时使用的展示组件
            showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                    width[selectKind], new Vector2D(showx, showy), 0);

            //初始化齿轮（齿轮序列）
            initgears();

            flag.Texture = texturemanager.Get("bg_cursor0");
            flag.SetWidth(15);
            flag.SetHeight(15);
        }

        public void Show()
        {
            show = true;
            animation = new MoveAnimation(Position, new Vector2D(0, Aim_Y), anitime);
        }
        public void Hide()
        {
            show = false;
            animation = new MoveAnimation(Position, new Vector2D(0, Y), anitime);
        }
        public void Start()
        {

        }

        public void Update(double elapsedTime)
        {
            if (animation != null)
            {
                if (animation.working)
                {
                    animation.Update(elapsedTime);
                }
            }
            #region 设计逻辑
            ////设计时使用的参考齿轮
            //showGear.Update(elapsedTime);
            ////设计下的齿轮序列
            //foreach (Gear g in Gears)
            //{
            //    g.Update(elapsedTime);
            //}
            ////设计前翻
            //if (Input.getKeyDown("A"))
            //{
            //    int s = selectKind - 1;
            //    if (s < 0)
            //        s = 13;
            //    selectKind = s;
            //    Datas.SelectGear = Names[selectKind];
            //    showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
            //        width[selectKind], new Vector2D(showx, showy), 0);
            //}
            ////切换设计模式下的齿轮旋转方向
            //if (Input.getKeyDown("C"))
            //{
            //    turnleft = !turnleft;
            //}
            ////设计后翻
            //if (Input.getKeyDown("S"))
            //{
            //    int s = selectKind + 1;
            //    if (s >= 14)
            //        s = 0;
            //    selectKind = s;
            //    Datas.SelectGear = Names[selectKind];
            //    showGear = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
            //        width[selectKind], new Vector2D(showx, showy), 0);
            //}
            ////根据当前信息，添加一个齿轮进设计序列
            //if (Input.getKeyDown("D"))
            //{
            //    Add();
            //}
            ////删除最近的一个设计齿轮
            //if (Input.getKeyDown("Q"))
            //{
            //    if(Gears.Count !=0)
            //        Gears.RemoveAt(Gears.Count - 1);
            //}
            ////将设计记录保存至Data.txt中
            //if (Input.getKeyDown("R"))
            //{
            //    WriteData();
            //}
            //if (Input.getKeyDown("I")) { Datas.TestY += 1; }
            //if (Input.getKeyDown("K")) { Datas.TestY -= 1; }
            //if (Input.getKeyDown("J")) { Datas.TestX -= 1; }
            //if (Input.getKeyDown("L")) { Datas.TestX += 1; }
            #endregion
            //对所有有效齿轮进行逻辑更新
            for (int i = 0; i < count; i++)
            {
                WorkingGears[i].Update(elapsedTime);
            }

        }

        /// <summary>
        /// 给定参数后添加一个齿轮进设计列表（仅在设计时使用）[这里已经放弃使用本函数]
        /// </summary>
        /// <param name="select">选择项</param>
        /// <param name="_x">坐标x</param>
        /// <param name="_y">坐标y</param>
        /// <param name="flag">旋转方向</param>
        public void Add(int select ,int _x, int _y,int flag)
        {
            Gear g = new Gear(texturemanager.Get(Names[select]), width[select],
               width[select], new Vector2D(_x, _y), 0);

            g.rotate_speed = flag * 64 / (realwidth[select] / (float)32);
            g.SetParent(Position);
            g.SetName(Names[select]);
            Gears.Add(g);
        }
        /// <summary>
        /// 设计逻辑中需要调用的函数[用于添加一个测试点]
        /// </summary>
        public void Add()
        {
            double _x = Datas.TestX - Position.X;
            double _y = Datas.TestY - Position.Y;
            Gear g = new Gear(texturemanager.Get(Names[selectKind]), width[selectKind],
                width[selectKind], new Vector2D(_x, _y), 0);
            if (turnleft)
            {
                g.rotate_speed = 64 / (realwidth[selectKind] / 32);
            }
            else
            {
                g.rotate_speed = -64 / (realwidth[selectKind] / 32);
            }
            g.SetParent(Position);
            g.SetName(Names[selectKind]);
            g.SetTag(selectKind);
            Gears.Add(g);
        }
        //设计逻辑中用于记录添加点信息
        public void WriteData()
        {
            StreamWriter wrd = new StreamWriter("Datas.txt", false); //记录齿轮的位置的信息
            foreach (Gear g in Gears)
            {
                wrd.WriteLine(g.Name + "  X:" + g.Position.X + "  Y:" + g.Position.Y + " speed:"
                    + g.rotate_speed.ToString() + " selectindex:" + selectKind);
            }
            wrd.Close();
        }
         
        public void Render()
        {
            //底色
            back.SetPosition(Position.X + back_x, Position.Y + back_y);
            renderer.DrawSprite(back);

            //后层
            WorkingGears[6].Render();
            WorkingGears[10].Render();
            WorkingGears[9].Render();
            WorkingGears[11].Render();
            WorkingGears[12].Render();
            WorkingGears[5].Render();
            WorkingGears[4].Render();
            //框架
            frame.SetPosition(Position.X + frame_x, Position.Y + frame_y);
            renderer.DrawSprite(frame);
            //前层
            WorkingGears[0].Render();
            WorkingGears[1].Render();
            WorkingGears[2].Render();
            WorkingGears[3].Render();
            WorkingGears[7].Render();
            WorkingGears[8].Render();
            //设计列表中的齿轮
            foreach (Gear g in Gears)
            {
                g.Render();
            }
          //------------设计逻辑----------------//
            //设计时使用的齿轮
            //showGear.Render();
          //  flag.SetPosition(Datas.TestX, Datas.TestY);
          //  renderer.DrawSprite(flag);
          //------------设计逻辑----------------//
        }
    }

}
