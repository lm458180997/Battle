﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Players
{

    //状态说明：
    //状态分为多种状态，一种为行走状态，一种为攻击状态
    //所有的逻辑都是根据状态的组合来决定。  
    //例如 running + ATK_Normal , 解释为人为跑动时触发的攻击

    //人物的状态
    public class PeopleStates
    {

        public const byte Ready_L = 0;           //就绪状态
        public const byte Walking_L = 1;          //行走状态
        public const byte Ready_R = 100;            //就绪状态
        public const byte Walking_R = 101;          //行走状态
        public const byte Running = 2;         //跑动状态
        public const byte Jumping_Walking = 3;  //行走中的跳跃
        public const byte Jumping_Walking_R = 103;
        public const byte Running_Walking = 4;  //跑动中的跳跃
        public const byte Lay = 5;              //倒地状态

        public const byte ATK_None = 50;
        public const byte ATK_Normal = 51;

    }

    //战斗人物
    public class BattlePeoples : IGameObject
    {
        public Vector2D Position;               //人物的坐标
        public Vector2D Direction;              //人物的朝向   

        public TextureManager texturemanager;   //纹理管理器
        public const float per_512 = 0.015625f, per_256 = 0.03125f;       //用于将数据百分比化，从而便于制作渲染精灵
        protected string name = "default";       //人物的名字（便于在众多对象中需找到指定对象）
        public int level = 0;                    //人物的使用等级
        public float attack = 10;                //人物本身的攻击力（暂且归纳为基础攻击力）
        public float fastSpeed = 350;            //跑动时的速度
        public float slowSpeed = 175;            //走动时的速度
        public int Level = 0;                    //强化等级

        public byte BP_MoveState = PeopleStates.Ready_R;     //人物的行走类动画状态(默认为准备状态)
        public byte BP_ATKState = PeopleStates.ATK_None;   //人物的攻击类动画状态（默认为无）

        //动画适配器缓冲区（保存所有的动画数据，需要时再从中读取）
        public Dictionary<string, AnimationAdaptor> AdaptorsList = new Dictionary<string, AnimationAdaptor>();

        public AnimationAdaptor Adaptor;  //动画适配器（）

        public Renderer renderer = new Renderer();                //一般着色器（中心点居中）

        //攻击组件（每一个组件都能对敌人具有技能判定）
        //public List<CollisionComponent> AttackComponents = new List<CollisionComponent>();  //攻击组件
        //public List<CollisionComponent> AttackComponents_ToRemove = new List<CollisionComponent>(); //组件删除缓冲池

        /// <summary>
        /// 向peopleData中注册人物信息
        /// </summary>
        /// <param name="pd">peopleData</param>
        public virtual void ClonePeopleData(Net.PlayerData pd)
        {
            pd.player_x = (float)Position.X;
            pd.player_y = (float)Position.Y;
            pd.playerstate = 0;
            pd.adaptorstate = 0;
            pd.currentindex = Adaptor.currentComplete;
            pd.totleindex = Adaptor.testTotle;
        }

        /// <summary>
        /// 绑定人物数据（通过网络，实现人物状态的同步）
        /// </summary>
        /// <param name="pd"></param>
        public virtual void BindPeopleData(Net.PlayerData pd)
        {


        }

        public virtual void InitAdaptorBuffers()
        {

        }

        public virtual void Start()
        {
        }

        public virtual void Update(double elapsedTime)
        {
        }

        public virtual void KeyUpdate(double elapsedTime)
        {

        }

        /// <summary>
        /// 攻击的判定
        /// </summary>
        /// <param name="p">另一个Player</param>
        /// <param name="msg">信息管理机</param>
        /// <returns>是否有满足判定的实体存在</returns>
        //public virtual bool AttackCollision(Battle.Players.BattlePlayer p, ref MessageManager msg)
        //{
        //    return false;
        //}

        /// <summary>
        /// 攻击的判定
        /// </summary>
        /// <param name="b">另一个Player的飞行道具</param>
        /// <param name="msg">信息管理机</param>
        /// <returns>是否有满足判定的实体存在</returns>
        //public virtual bool AttackCollision(Bullet b, ref MessageManager msg)
        //{
        //    return false;
        //}

        public virtual void Render()
        {
        }


    }

    //蕾米利亚
    public class Remilia : BattlePeoples
    {
        public int height = 64;       //身高
        bool leftdown = false;        //是否左键被持续按下
        bool rightdown = false;       //是否右键被持续按下
        float speed_x = 180;          //横向移动速度（固定）

        //---------跳跃参数----------//
        bool insky = false;            //是否是在空中(浮空状态)
        float speed_y = 0;             //纵向移动速度（可变）
        float jumpspeed = 0;           //跳跃时候的瞬间速度
        float gravi = 0;               //纵向的重力加速度
        float jumphight = 150;         //跳跃的最高高度
        float jumptime = 0.8f;         //跳跃过程中所需要的时间
        public int Bottom = 0;         //人物在地面上的最低位置
        //--------------------------//
        /// <summary>
        /// 蕾米利亚
        /// </summary>
        /// <param name="t">纹理机</param>
        public Remilia(TextureManager t)
        {

            texturemanager = t;
            Position = new Vector2D(0, 0);
            Direction = new Vector2D(1, 0);  //默认朝右边
            InitAdaptorBuffers();                  //初始化适配器列表

            //初始化跳跃速率参数
            float jumpt2 = jumptime / 2;
            gravi = jumphight * 2 / (jumpt2 * jumpt2);
            jumpspeed = gravi * jumpt2;

        }
        //初始化适配器中的数据
        public override void InitAdaptorBuffers()
        {
            AdaptorsList.Add("remilia_stand",
                new Battle.CharactorsAnimations.remilia_stand_Adaptor(texturemanager));
            AdaptorsList.Add("remilia_walkFront",
                new Battle.CharactorsAnimations.remilia_walkFront_Adaptor(texturemanager));
            AdaptorsList.Add("remilia_jump",
                new Battle.CharactorsAnimations.remilia_jump_Adaptor(texturemanager));
            Adaptor = AdaptorsList["remilia_stand"];   //最初选择站立
            Adaptor.Start();
        }
        /// <summary>
        /// 注册（克隆）状态数据进PlayerData中，用于网络传递
        /// </summary>
        /// <param name="pd"></param>
        public override void ClonePeopleData(Net.PlayerData pd)
        {
            pd.player_x = (float)Position.X;
            pd.player_y = (float)Position.Y;
            pd.playerstate = BP_MoveState;
            pd.adaptorstate = 0;              //暂时没用上
            pd.currentindex = (byte)Adaptor.animation.CurrentIndex;
            pd.totleindex = 0;                //暂时没用上
            pd.speed = (int)speed_y;
            //if (pd.speed > 0)
            //{
            //    int i = 0;
            //    int k = 0;
            //    pd.speed = pd.speed;
            //}
            pd.caculatetime = (float)Adaptor.animation.caculatetime;
        }

        /// <summary>
        /// 绑定人物数据（动态情况）
        /// </summary>
        /// <param name="pd">人物数据</param>
        public override void BindPeopleData(Net.PlayerData pd)
        {
            //  <100为朝左的状态 ，>=100为朝右的状态
            if (pd.playerstate < 100)
            {
                if (pd.playerstate == PeopleStates.Ready_L)
                {
                    leftdown = false; rightdown = false;      //左右按下的状态刷新为无
                    Direction.X = -1; Direction.Y = 0;        //面向的方向修改
                    Adaptor = AdaptorsList["remilia_stand"];  //选择对应的适配器
                    //   Adaptor.SetCurrentIndex(pd.currentindex); //将适配器安置到合适的帧
                    //    Form1.DebugResult = "ReadyL";             //调试信息
                }
                else if (pd.playerstate == PeopleStates.Walking_L)
                {
                    leftdown = true; rightdown = false;       //标记为正在向左按键
                    Direction.X = -1; Direction.Y = 0;
                    Adaptor = AdaptorsList["remilia_walkFront"];
                    //   Adaptor.SetCurrentIndex(pd.currentindex);
                    //   Form1.DebugResult = "Walking_L";
                }
            }
            else
            {
                if (pd.playerstate == PeopleStates.Ready_R)
                {
                    leftdown = false; rightdown = false;
                    Direction.X = 1; Direction.Y = 0;
                    Adaptor = AdaptorsList["remilia_stand"];
                    // Adaptor.SetCurrentIndex(pd.currentindex);
                    //  Form1.DebugResult = "Ready_R";
                }
                else if (pd.playerstate == PeopleStates.Walking_R)
                {
                    leftdown = false; rightdown = true;
                    Direction.X = 1; Direction.Y = 0;
                    Adaptor = AdaptorsList["remilia_walkFront"];
                    // Adaptor.SetCurrentIndex(pd.currentindex);
                    // Form1.DebugResult = "Walking_R";
                }


            }

            if (pd.playerstate == PeopleStates.Jumping_Walking)
            {
                insky = true;
                speed_y = pd.speed;
                Adaptor = AdaptorsList["remilia_jump"];
                Adaptor.animation.CurrentIndex = pd.currentindex;
                Adaptor.animation.caculatetime = pd.caculatetime;
            }

            this.Position.X = pd.player_x;
            this.Position.Y = pd.player_y;
            this.BP_MoveState = pd.playerstate;
        }


        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);

            KeyUpdate(elapsedTime);          //键盘逻辑更新

            Adaptor.Update(elapsedTime);     //适配器逻辑更新

            DealWithAdaptor(elapsedTime);    //适配器逻辑操作


            double Posx = Position.X;

            if (leftdown)
                Posx -= elapsedTime * speed_x;
            if (rightdown)
                Posx += elapsedTime * speed_x;

            if (Posx < -300)
                Posx = -300;
            if (Posx > 300)
                Posx = 300;
            Position.X = Posx;

            //if (BP_MoveState == PeopleStates.Walking_L || BP_MoveState == PeopleStates.Walking_R)
            //{
            //    if (leftdown)
            //        Position.X -= elapsedTime * speed_x;
            //    if (rightdown)
            //        Position.X += elapsedTime * speed_x;
            //}

            //如果处于浮空状态，则重力相关的逻辑
            if (insky)
            {
                RunGravi(elapsedTime);
            }

        }

        /// <summary>
        /// 重力相关的逻辑更新，只要在空中就会受到重力的影响
        /// </summary>
        /// <param name="elapsedTime">deltatime</param>
        public void RunGravi(double elapsedTime)
        {
            speed_y -= (float)elapsedTime * gravi;      //速度向下方向加速
            double y = Position.Y + speed_y * elapsedTime;  //沿速度方向移动
            if (y <= Bottom)
            {
                Position.Y = Bottom;               //置于地面
                insky = false;                     //解除浮空标记
                speed_y = 0;                       //y轴移动速度置0
                //从跳跃状态结束
                if (BP_MoveState == PeopleStates.Jumping_Walking)
                {
                    BP_MoveState = PeopleStates.Ready_L;
                    if (leftdown || rightdown)
                    {
                        Adaptor = AdaptorsList["remilia_walkFront"];
                        if (leftdown)
                            BP_MoveState = PeopleStates.Walking_L;
                        else
                            BP_MoveState = PeopleStates.Walking_R;
                    }
                    else
                        Adaptor = AdaptorsList["remilia_stand"];
                    Adaptor.Start();
                }
            }
            else
            {
                //正常坐标移动
                Position.Y = y;
            }
        }

        public void DealWithAdaptor(double elapsedTime)
        {
            //如果动画结束了（完全执行）
            if (Adaptor.animation.IsOver)
            {

            }
        }

        public override void KeyUpdate(double elapsedTime)
        {
            //如果不处于跳跃状态，则可以对左右按键进行判断
            if (BP_MoveState != PeopleStates.Jumping_Walking)
            {
                if (Input.getKeyDown("Right"))
                {
                    leftdown = false;
                    Adaptor = AdaptorsList["remilia_walkFront"];
                    Adaptor.Start();
                    Direction.X = 1;
                    Direction.Y = 0;
                    rightdown = true;
                    BP_MoveState = PeopleStates.Walking_R;
                }
                if (Input.getKeyUp("Right"))
                {
                    if (rightdown)
                    {
                        Adaptor = AdaptorsList["remilia_stand"];
                        Adaptor.Start();
                        rightdown = false;
                        BP_MoveState = PeopleStates.Ready_R;
                    }
                }
                if (Input.getKeyDown("Left"))
                {
                    rightdown = false;
                    Adaptor = AdaptorsList["remilia_walkFront"]; //从列表中获取Adaptor
                    Adaptor.Start();                 //adptor中的数据进行重置
                    Direction.X = -1;
                    Direction.Y = 0;
                    leftdown = true;
                    BP_MoveState = PeopleStates.Walking_L;
                }
                if (Input.getKeyUp("Left"))
                {
                    if (leftdown)
                    {
                        Adaptor = AdaptorsList["remilia_stand"];
                        Adaptor.Start();
                        leftdown = false;
                        BP_MoveState = PeopleStates.Ready_L;
                    }
                }
            }
            if (Input.getKeyDown("Up"))
            {
                if (BP_MoveState != PeopleStates.Jumping_Walking)
                {
                    Adaptor = AdaptorsList["remilia_jump"]; //从列表中获取Adaptor
                    Adaptor.Start();                 //adptor中的数据进行重置
                    BP_MoveState = PeopleStates.Jumping_Walking;
                    insky = true;                    //进入浮空状态
                    speed_y = jumpspeed;
                }
            }
        }

        public override void Render()
        {
            base.Render();
            Sprite sp;
            //方向朝右
            if (Direction.X > 0)
            {
                //从动画中取出纹理，并设定属性值
                sp = Adaptor.GetSpriteWithPos(Position.X, Position.Y);
            }
            //方向朝左
            else
            {
                //从动画中取出纹理，并设定属性值
                sp = Adaptor.GetSpriteWithPos(Position.X, Position.Y, true);
            }
            //使用一般着色器进行着色
            renderer.DrawSprite(sp);

        }

    }


}
