using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages.Menu
{
    /// <summary>
    /// 左边的选择框组件，各种选择的内容（）
    /// </summary>
    public class SelectComponent : IGameObject , KeyFocusTest
    {
        public bool hide = false;         //是否处于隐藏状态
        public const float per_512 = 0.015625f;
        TextureManager texturemanager;
        Renderer renderer = new Renderer();

        Sprite testback = new Sprite();
        Sprite testshow = new Sprite();

        int Itemscount = 0;//选项的数量
        Pointer pointer;        //指示指针
        int pointer_Y = 272;    //指针位置
        int pointer_X = -320;   //指针位置
        int Items_Offset = 24;  //每个项之间的间距
        int Items_X = -245;     //顶部项的X坐标
        int Items_Y = 232;      //顶部项的Y坐标

        Dictionary<String, Sprite> SpritesNormal;  //没有被选中的标签的纹理映射表
        Dictionary<String, Sprite> SpritesSelect;  //选中了的纹理映射表
        Dictionary<int, String> Map;               //索引值映射上的标签

        public String SelectItem = "StoryMode";     //选中项的名字
        int SelectIndex = 0;            //选中项的索引值

        bool changing = false;    //是否处于渐变
        double totletime = 0.3;   //渐变的总时间
        double caculatetime = 0;  //渐变的计时器
        double percent = 1;       //渐变的百分比

        public SelectComponent(TextureManager _t)
        {
            texturemanager = _t;
            testback.Texture = _t.Get("2_menu_black");
            testback.SetWidth(512); testback.SetHeight(512);
            testshow.Texture = _t.Get("2_menu_moji1");
            testshow.SetWidth(512); testshow.SetHeight(512);
            testback.SetPosition(-240, 100);
            testshow.SetPosition(-260, 100);
            pointer = new Pointer(texturemanager);    //指示指针
            pointer.Position.Y = pointer_Y;
            pointer.Position.X = pointer_X;

            initMap();
            initSprites();
        }
        /// <summary>
        /// 初始化映射表
        /// </summary>
        void initMap()
        {
            Map = new Dictionary<int, string>();
            int i=0;
            Map.Add(i, "StoryMode");i++;
            Map.Add(i, "ArgadeMode");i++;
            Map.Add(i, "VsCom");i++;
            Map.Add(i, "VsPlayer"); i++;
            Map.Add(i, "VsNetwork"); i++;
            Map.Add(i, "Practice"); i++;
            Map.Add(i, "Replay"); i++;
            Map.Add(i, "Musicroom"); i++;
            Map.Add(i, "Result"); i++;
            Map.Add(i, "Profile"); i++;
            Map.Add(i, "Config"); i++;
            Map.Add(i, "Exit"); i++;
            Itemscount = i;
        }
        void initSprites()
        {
            SpritesNormal = new Dictionary<string, Sprite>();
            SpritesSelect = new Dictionary<string, Sprite>();
            Sprite sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28*per_512,14*per_512,39*per_512,17*per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("StoryMode", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 17 * per_512, 39 * per_512, 20 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("ArgadeMode", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 20 * per_512, 39 * per_512, 23 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("VsCom", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 23 * per_512, 39 * per_512, 26 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("VsPlayer", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 26 * per_512, 39 * per_512, 29 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("VsNetwork", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 29 * per_512, 39 * per_512, 32 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Practice", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 32 * per_512, 39 * per_512, 35 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Replay", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 35 * per_512, 39 * per_512, 38 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Musicroom", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 38 * per_512, 39 * per_512, 41 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Result", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 41 * per_512, 39 * per_512, 44 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Profile", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 44 * per_512, 39 * per_512, 47 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Config", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji1");
            sp.SetUVs(28 * per_512, 47 * per_512, 39 * per_512, 50 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesNormal.Add("Exit", sp);
            initSprites2();
        }
        void initSprites2()
        {
            Sprite sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 14 * per_512, 39 * per_512, 17 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("StoryMode", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 17 * per_512, 39 * per_512, 20 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("ArgadeMode", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 20 * per_512, 39 * per_512, 23 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("VsCom", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 23 * per_512, 39 * per_512, 26 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("VsPlayer", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 26 * per_512, 39 * per_512, 29 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("VsNetwork", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 29 * per_512, 39 * per_512, 32 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Practice", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 32 * per_512, 39 * per_512, 35 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Replay", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 35 * per_512, 39 * per_512, 38 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Musicroom", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 38 * per_512, 39 * per_512, 41 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Result", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 41 * per_512, 39 * per_512, 44 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Profile", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 44 * per_512, 39 * per_512, 47 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Config", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("2_menu_moji2");
            sp.SetUVs(28 * per_512, 47 * per_512, 39 * per_512, 50 * per_512);
            sp.SetWidth(11 * 8); sp.SetHeight(3 * 8);
            SpritesSelect.Add("Exit", sp);
        }

        public void Start()
        {
        }

        public void Update(double elapsedTime)
        {
            pointer.Update(elapsedTime);     //指针逻辑更新
            RunKey(MenuStage.CurrentFocus);  //判断是否获取焦点再进行键盘任务
            //选择项的发光效果渐变
            if (changing)
            {
                caculatetime += elapsedTime;
                if (caculatetime > totletime)
                {
                    caculatetime = totletime;
                    percent = 1;
                    changing = false;
                }
                else
                {
                    percent = caculatetime / totletime;
                }
            }

        }

        public void Render()
        {
           // pointer.Render();
            pointer.Drawgear();
            renderer.DrawSprite(testback);
            pointer.DrawPointer();
            for (int i = 0; i < Map.Keys.Count; i++)
            {
                //是否正在选择
                if (i != SelectIndex)
                {
                    Sprite sp = SpritesNormal[Map[i]];
                    sp.SetPosition(Items_X, Items_Y - i * Items_Offset);
                    sp.SetColor(new Color(1, 1, 1, 1));
                    renderer.DrawSprite(sp);
                }
                else
                {
                    //是否处于渐变
                    if (changing)
                    {
                        Sprite sp1 = SpritesNormal[Map[i]];
                        Sprite sp2 = SpritesSelect[Map[i]];
                        sp1.SetPosition(Items_X, Items_Y - i * Items_Offset);
                        sp2.SetPosition(Items_X, Items_Y - i * Items_Offset);
                        sp1.SetColor(new Color(1, 1, 1, 1 ));
                        sp2.SetColor(new Color(1, 1, 1, 0.3f+ 0.7f*(float)percent));
                        renderer.DrawSprite(sp1);
                        renderer.DrawSprite(sp2);
                    }
                    else
                    {
                        Sprite sp = SpritesSelect[Map[i]];
                        sp.SetPosition(Items_X, Items_Y - i * Items_Offset);
                        sp.SetColor(new Color(1, 1, 1, 1));
                        renderer.DrawSprite(sp);
                    }
                }
            }

        }

        public void RunKey(int type)
        {
            if (type == MenuStage.SelectComponent)
            {
                if (Input.getKeyDown("Up"))
                {
                    int c = SelectIndex - 1;
                    if (c < 0)
                        c = Itemscount - 1;
                    SelectIndex = c;
                    SelectItem = Map[SelectIndex];
                    pointer.MoveTo(pointer_X, pointer_Y - SelectIndex * Items_Offset);
                    //更新渐变
                    changing = true;
                    caculatetime = 0;
                    percent = 0;
                }
                if (Input.getKeyDown("Down"))
                {
                    int c = SelectIndex + 1;
                    if (c >= Itemscount)
                        c = 0;
                    SelectIndex = c;
                    SelectItem = Map[SelectIndex];
                    pointer.MoveTo(pointer_X, pointer_Y - SelectIndex * Items_Offset);
                    //更新渐变
                    changing = true;
                    caculatetime = 0;
                    percent = 0;
                }
            }
        }
    }

    /// <summary>
    /// 旋转信息的一个封装类
    /// </summary>
    public class RotateData
    {
        public double Angle=0;        //旋转角度
        public double rate = 1;       // 角度/距离 的比例
    }
    /// <summary>
    /// 指针组件
    /// </summary>
    public class Pointer : IGameObject
    {
        TextureManager texturemanager;
        Sprite gear = new Sprite();                //齿轮
        Sprite gearback = new Sprite();            //齿轮的影子
        RotateData rotatedata= new RotateData();   //旋转的角度(旋转信息)
        public Vector2D Position= new Vector2D();          //坐标
        public int aim_X, aim_Y;                           //目标坐标，只能为整数
        Renderer renderer= new Renderer();
        Sprite pointer = new Sprite();             //指针

        MoveAnimation moveanimation;               //位移动画
        RoatateAnimation rotateanimation;          //旋转动画

        public Pointer(TextureManager _t)
        {
            texturemanager = _t;
            gear.Texture = texturemanager.Get("2_menu_gear");
            gear.SetWidth(128); gear.SetHeight(128);
            gearback.Texture = texturemanager.Get("gearback");
            gearback.SetWidth(128); gearback.SetHeight(128);

            pointer.Texture = texturemanager.Get("2_menu_hari");
            pointer.SetWidth(128); pointer.SetHeight(128);
            //初始化动画，但不执行
            moveanimation = new MoveAnimation(Position, Position, 1);
            moveanimation.working = false;
            rotateanimation = new RoatateAnimation(rotatedata, 0, 0);
            rotateanimation.working =false;
        }

        public void Start()
        {
        }

        public void setAimPosition(int x, int y)
        {
            aim_X = x;
            aim_Y = y;
        }

        public void MoveTo(int x, int y)
        {
            aim_X = x;
            aim_Y = y;
            if ((Position.X != x) || (Position.Y != y))
            {
                //产生新的移动信息后，更新动画
                moveanimation = new MoveAnimation(Position, new Vector2D(x, y), 0.2);
                rotateanimation = new RoatateAnimation(rotatedata, Position.Y-y, 0.2);
            }
        }

        public void Update(double elapsedTime)
        {
            if(moveanimation.working)
              moveanimation.Update(elapsedTime);
            if (rotateanimation.working)
                rotateanimation.Update(elapsedTime);
                ////坐标的移动
                //Vector2D dir = new Vector2D(aim_X - Position.X, aim_Y - Position.Y);
                //dir.Normalize();
                ////距离目标的距离
                //double distance = (aim_X - Position.X) * (aim_X - Position.X)
                //    + (aim_Y - Position.Y) * (aim_Y - Position.Y);
                ////本次的位移距离
                //double offsetlength = run_speed * elapsedTime;       
                //if (offsetlength >= distance)
                //{
                //    Position.X = aim_X;
                //    Position.Y = aim_Y;
                //    arrive = true;
                //}
                //else
                //{
                //    Position.X += offsetlength * dir.X;
                //    Position.Y += offsetlength * dir.Y;
                //}

        }

        public void Drawgear()
        {
            gearback.SetPosition(Position.X + 2, Position.Y - 5);
            renderer.DrawSprite(gearback, Position.X + 2, Position.Y - 5, (float)(-rotatedata.Angle));
            gear.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(gear, Position.X, Position.Y, (float)(-rotatedata.Angle));
        }
        public void DrawPointer()
        {
            pointer.SetPosition(Position.X + 30, Position.Y - 15);
            renderer.DrawSprite(pointer);
        }

        public void Render()
        {
            gearback.SetPosition(Position.X + 2, Position.Y-5);
            renderer.DrawSprite(gearback, Position.X+2, Position.Y-5, (float)(-rotatedata.Angle));

            gear.SetPosition(Position.X, Position.Y);
            renderer.DrawSprite(gear, Position.X, Position.Y, (float)(-rotatedata.Angle));

            pointer.SetPosition(Position.X+30 , Position.Y-15);
            renderer.DrawSprite(pointer);

        }
    }
    public delegate double ChangePercent(double cal,double totle);
    public class Animation
    {
        public bool working = false;      //是否正常工作
        public bool disabled = false;     //是否已经注销
        public bool end = false;          //动画是否结束
        //计时器
        public double caculatetime = 0;
        //总时间
        public double totletime = 1;
        public virtual void Update(double elapsedTime) 
        {
        }
        public static double Getpercent(ChangePercent cg,double cal,double totle)
        {
            return cg(cal, totle);
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="cal"></param>
        /// <param name="totle"></param>
        /// <returns></returns>
        public static double Line(double cal, double totle)
        {
            double res = cal / totle;
            if (res > 1) res = 1;
            return res;
        }
        /// <summary>
        /// 先快后慢的插值
        /// </summary>
        /// <param name="cal"></param>
        /// <param name="totle"></param>
        /// <returns></returns>
        public static double Line2(double cal, double totle)
        {
            double res = cal / totle;
            if (res > 1) res = 1;
            res = 1 - (1 - res) * (1 - res);
            return res;
        }
        /// <summary>
        /// 先慢后快的插值
        /// </summary>
        /// <param name="cal"></param>
        /// <param name="totle"></param>
        /// <returns></returns>
        public static double Line3(double cal, double totle)
        {
            double res = cal / totle;
            if (res > 1) res = 1;
            res = res * res;
            return res;
        }
    }
    public class MoveAnimation :Animation
    {
        double offsetX = 0;
        double offsetY = 0;
        //原点坐标
        Vector2D oriPosition = new Vector2D();
        //目标坐标
        Vector2D aimPosition = new Vector2D();
        //插值坐标
        Vector2D Position;
        /// <summary>
        /// 插值位移动画
        /// </summary>
        /// <param name="pos">插值坐标</param>
        /// <param name="aimpos">目标坐标</param>
        /// <param name="time">插值时间</param>
        public MoveAnimation(Vector2D pos,Vector2D aimpos, double time)
        {
            Position = pos;
            oriPosition.X = pos.X;
            oriPosition.Y = pos.Y;
            aimPosition.X = aimpos.X;
            aimPosition.Y = aimpos.Y;
            totletime = time;
            working = true;
            offsetX = aimpos.X - oriPosition.X;
            offsetY = aimpos.Y - oriPosition.Y;
        }
        public override void Update(double elapsedTime)
        {
            if (working)
            {
                caculatetime += elapsedTime;
                double percent = caculatetime / totletime;
                if (caculatetime >= totletime)
                {
                    percent = 1;
                    working = false;
                    end = true;
                    Position.X = aimPosition.X;
                    Position.Y = aimPosition.Y;
                }
                else
                {
                    //选用先快后慢的插值方式
                    percent = Getpercent(Line2,caculatetime,totletime);
                    Position.X = oriPosition.X + percent * offsetX;
                    Position.Y = oriPosition.Y + percent * offsetY;
                }
            }
        }


    }
    public class RoatateAnimation : Animation
    {
        RotateData rotatedata;
        double oriAngle = 0;
        double changeAngle = 0;
        /// <summary>
        /// 旋转动画
        /// </summary>
        /// <param name="data">用于接受数据的旋转信息</param>
        /// <param name="length">旋转动画需要位移的长度（动画本身并不影响位移，但需要根据位移换算旋转角）</param>
        /// <param name="t">动画播放的总时间</param>
        public RoatateAnimation(RotateData data, double length,double t)
        {
            rotatedata = data;
            totletime = t;
            working = true;
            oriAngle = data.Angle;
            //计算出需要的变化角
            changeAngle = data.rate * length; 
        }

        public override void Update(double elapsedTime)
        {
            if (working)
            {
                caculatetime += elapsedTime;
                double percent =0;
                if (caculatetime >= totletime)
                {
                    percent = 1;
                    working = false;
                    end = true;
                    rotatedata.Angle = oriAngle + changeAngle;
                }
                else
                {
                    //选用先快后慢的插值方式
                    percent = Getpercent(Line2, caculatetime, totletime);
                    rotatedata.Angle = oriAngle + changeAngle * percent;
                }
            }
        }

    }

}
