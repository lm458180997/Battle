using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages
{
    /// <summary>
    ///  战斗场景组件，例如生命槽，能量槽，符卡，UI等
    /// </summary>
    public class BattleComponents :IGameObject
    {
        TextureManager texturemanager;

        UpComponents upcomponent;      //上半部分的组件
        DownComponents downcomponent;  //下半部分的组件
        Vector2D UpPostion = new Vector2D(0, 150);   //上半部分组件的容器
        Vector2D DownPosition = new Vector2D(0, -50);//下半部分组件的容器

        public BattleComponents(TextureManager t)
        {
            texturemanager = t;

            upcomponent = new UpComponents(t);
            upcomponent.SetParent(UpPostion);
            downcomponent = new DownComponents(t);
            downcomponent.SetParent(DownPosition);

        }

        public void Start()
        {
            upcomponent.Start();
            downcomponent.Start();
        }

        public void Update(double elapsedTime)
        {
            upcomponent.Update(elapsedTime);
            downcomponent.Update(elapsedTime);
        }

        public void Render()
        {
            upcomponent.Render();
            downcomponent.Render();
        }
    }
    /// <summary>
    /// 上面部分的组件
    /// </summary>
    public class UpComponents : IGameObject
    {
        TextureManager texturemanager;
        public Vector2D ParentPosition = new Vector2D();
        Renderer renderer = new Renderer();

        //每个人物对应的face
        Dictionary<string, Sprite> Faces= new Dictionary<string,Sprite>();
        Vector2D F_PositionL = new Vector2D(-240, 160);
        Vector2D F_PositionR = new Vector2D(240, 160);
        String LeftSelect = "Remilia";
        String RightSelect = "Remilia";

        //UI框架
        Sprite Table=new Sprite();
        Vector2D Table_Position = new Vector2D(0, 126);

        //血槽
        Sprite HelpTable= new Sprite() ;  //血槽的底层框架
        Sprite BackHolder = new Sprite(); //血槽底色
        Sprite ForeHolder = new Sprite(); //血槽前景色
        Vector2D H_PositionL = new Vector2D(-175,120);
        Vector2D H_PositionR = new Vector2D(175, 120);
        float HoderOff_x = 8, HoderOff_y =20;

        //胜点
        Sprite VictoryTable = new Sprite(); //胜利槽的框架
        Sprite V_Holder = new Sprite();    //胜利点纹理
        Vector2D V_PositionL = new Vector2D(-75, 110);  //左边的相对坐标
        Vector2D V_PositionR = new Vector2D(75, 110);   //右边的相对坐标
        int V_CountL = 2;                  //左边的胜点数量
        int V_CountR = 2;                  //右边的胜点数量
        float V_distance = 26;             //两个胜点间的间隔

        //天气
        Dictionary<String, Sprite> weather; //天气的纹理映射表
        String CurrentWeather = "";         //当前的天气

        /// <summary>
        /// 初始化人物的表情贴图
        /// </summary>
        void initFaces()
        {
            Sprite sp= new Sprite();
            sp.Texture = texturemanager.Get("remifc");  //蕾米利亚
            sp.SetWidth(256); sp.SetHeight(64);
            Faces.Add("Remilia", sp); 
        }
        /// <summary>
        /// 初始化各个组件的纹理
        /// </summary>
        void initSprites()
        {
            //框架
            Table.Texture = texturemanager.Get("centerObjectB");
            Table.SetWidth(1024); Table.SetHeight(128);
            //血槽
            HelpTable.Texture = texturemanager.Get("1PlifeGageB");
            HelpTable.SetWidth(512); HelpTable.SetHeight(64);
            BackHolder.Texture = texturemanager.Get("lifeUnderB");
            BackHolder.SetWidth(256); BackHolder.SetHeight(32);
            ForeHolder.Texture = texturemanager.Get("1PlifeBarB");
            ForeHolder.SetWidth(256); ForeHolder.SetHeight(32);
            //胜点
            VictoryTable.Texture = texturemanager.Get("winGaugeB");
            VictoryTable.SetWidth(32); VictoryTable.SetHeight(32);
            V_Holder.Texture = texturemanager.Get("winB");
            V_Holder.SetWidth(32); V_Holder.SetHeight(32);

        }

        public UpComponents(TextureManager t)
        {
            texturemanager = t;
            initFaces();
            initSprites();
        }
        /// <summary>
        /// 设定父容器
        /// </summary>
        /// <param name="vct">父容器的坐标数据</param>
        public void SetParent(Vector2D vct)
        {
            ParentPosition = vct;
        }

        public void Start()
        {
           
        }

        public void Update(double elapsedTime)
        {
           
        }

        public void Render()
        {
            //先画人物图像
            Sprite sp = Faces[LeftSelect];
            sp.SetPosition(ParentPosition.X + F_PositionL.X,
                ParentPosition.Y + F_PositionL.Y);
            sp.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(sp);
            sp = Faces[RightSelect];
            sp.SetPosition(ParentPosition.X + F_PositionR.X,
                ParentPosition.Y + F_PositionR.Y);
            sp.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(sp);
            //框架
            Table.SetPosition(ParentPosition.X + Table_Position.X,
                ParentPosition.Y + Table_Position.Y);
            renderer.DrawSprite(Table);
            //血槽
            HelpTable.SetPosition(ParentPosition.X + H_PositionL.X,
                ParentPosition.Y + H_PositionL.Y);
            HelpTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(HelpTable);
            HelpTable.SetPosition(ParentPosition.X + H_PositionR.X,
                ParentPosition.Y + H_PositionR.Y);
            HelpTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(HelpTable);

            BackHolder.SetPosition(ParentPosition.X + H_PositionL.X-HoderOff_x,
                ParentPosition.Y + H_PositionL.Y+HoderOff_y);
            BackHolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(BackHolder);
            BackHolder.SetPosition(ParentPosition.X + H_PositionR.X + HoderOff_x,
                ParentPosition.Y + H_PositionR.Y + HoderOff_y);
            BackHolder.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(BackHolder);

            ForeHolder.SetPosition(ParentPosition.X + H_PositionL.X - HoderOff_x,
                ParentPosition.Y + H_PositionL.Y + HoderOff_y);
            ForeHolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(ForeHolder);
            ForeHolder.SetPosition(ParentPosition.X + H_PositionR.X + HoderOff_x,
                ParentPosition.Y + H_PositionR.Y + HoderOff_y);
            ForeHolder.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(ForeHolder);
            //胜点
            VictoryTable.SetPosition(ParentPosition.X + V_PositionL.X,
                ParentPosition.Y + V_PositionL.Y);
            VictoryTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(VictoryTable);
            VictoryTable.SetPosition(ParentPosition.X + V_PositionL.X-V_distance,
                ParentPosition.Y + V_PositionL.Y);
            VictoryTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(VictoryTable);

            VictoryTable.SetPosition(ParentPosition.X + V_PositionR.X,
                ParentPosition.Y + V_PositionR.Y);
            VictoryTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(VictoryTable);
            VictoryTable.SetPosition(ParentPosition.X + V_PositionR.X+V_distance,
                ParentPosition.Y + V_PositionR.Y);
            VictoryTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(VictoryTable);

            V_Holder.SetPosition(ParentPosition.X + V_PositionL.X+2,
                ParentPosition.Y + V_PositionL.Y+3);
            V_Holder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(V_Holder);
            V_Holder.SetPosition(ParentPosition.X + V_PositionL.X - V_distance+2,
                ParentPosition.Y + V_PositionL.Y+3);
            V_Holder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(V_Holder);

            V_Holder.SetPosition(ParentPosition.X + V_PositionR.X-2,
                ParentPosition.Y + V_PositionR.Y+3);
            V_Holder.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(V_Holder);
            V_Holder.SetPosition(ParentPosition.X + V_PositionR.X + V_distance-2,
                ParentPosition.Y + V_PositionR.Y+3);
            V_Holder.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(V_Holder);
        }


    }

    /// <summary>
    /// 下面的组件
    /// </summary>
    public class DownComponents : IGameObject
    {
        TextureManager texturemanager;
        public Vector2D ParentPosition = new Vector2D();
        Renderer renderer = new Renderer();

        //符卡相关内容
        Sprite CardBottom = new Sprite();       //符卡的底座
        Vector2D CB_PositionL = new Vector2D(-240,-50);  //左边相对坐标
        Vector2D CB_PositionR = new Vector2D(240,-50);  //右边相对坐标

        Sprite CureentCardTable = new Sprite(); //当前符卡的框架
        Vector2D CT_PositionL = new Vector2D(-284,-26);
        Vector2D CT_PositionR = new Vector2D(284,-26);
        Sprite HideCardTable = new Sprite();    //小框架
        Vector2D HT_PositionL = new Vector2D(-250,-43);
        Vector2D HT_PositionR = new Vector2D(250,-43);
        float HT_distanceX = -21.5f;           //小符卡的X轴间距
        float HT_distanceY = -4;               //小符卡的y轴间距

        //能量槽相关内容
        Sprite PowerBolder = new Sprite();     //能量槽边框
        Sprite PowerFull = new Sprite();       //能量贴图（满）
        Sprite Power = new Sprite();           //能量（未满）
        Sprite PowerBreak = new Sprite();      //坏掉的能量槽
        Vector2D P_PositionL = new Vector2D(-150,-53);
        Vector2D P_PositionR = new Vector2D(150,-53);
        float P_distanceX = -28;
        float P_distanceY = -4;

        /// <summary>
        /// 初始化各个组件的纹理
        /// </summary>
        void initSprites()
        {
            //底座
            CardBottom.Texture = texturemanager.Get("underObjB");
            CardBottom.SetWidth(256); CardBottom.SetHeight(64);
            //符卡
            CureentCardTable.Texture = texturemanager.Get("cardGaugeBigB");
            CureentCardTable.SetWidth(64); CureentCardTable.SetHeight(128);
            HideCardTable.Texture = texturemanager.Get("cardGaugeSmallB");
            HideCardTable.SetWidth(32);
            HideCardTable.SetHeight(32);
            //能量
            PowerBolder.Texture = texturemanager.Get("borderGaugeB");
            PowerBolder.SetWidth(32); PowerBolder.SetHeight(32);
            PowerFull.Texture = texturemanager.Get("borderBarFullB");
            PowerFull.SetWidth(32); PowerFull.SetHeight(32);

        }

        public DownComponents(TextureManager t)
        {
            texturemanager = t;
            initSprites();
        }
        /// <summary>
        /// 设定父容器
        /// </summary>
        /// <param name="vct">父容器的坐标数据</param>
        public void SetParent(Vector2D vct)
        {
            ParentPosition = vct;
        }

        public void Start()
        {
           
        }

        public void Update(double elapsedTime)
        {
           
        }

        public void Render()
        {
            //底座框架
            CardBottom.SetPosition(ParentPosition.X + CB_PositionL.X,
                   ParentPosition.Y + CB_PositionL.Y);
            CardBottom.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(CardBottom);
            CardBottom.SetPosition(ParentPosition.X + CB_PositionR.X,
                ParentPosition.Y + CB_PositionR.Y);
            CardBottom.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(CardBottom);
            //符卡框架
            CureentCardTable.SetPosition(ParentPosition.X + CT_PositionL.X,
                ParentPosition.Y + CT_PositionL.Y);
            CureentCardTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(CureentCardTable);
            CureentCardTable.SetPosition(ParentPosition.X + CT_PositionR.X,
                ParentPosition.Y + CT_PositionR.Y);
            CureentCardTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(CureentCardTable);

            HideCardTable.SetPosition(ParentPosition.X + HT_PositionL.X,
               ParentPosition.Y + HT_PositionL.Y);
            HideCardTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionL.X - HT_distanceX,
                ParentPosition.Y + HT_PositionL.Y + HT_distanceY);
            HideCardTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionL.X - 2*HT_distanceX,
                ParentPosition.Y + HT_PositionL.Y + 2*HT_distanceY);
            HideCardTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionL.X - 3 * HT_distanceX,
                ParentPosition.Y + HT_PositionL.Y + 3 * HT_distanceY);
            HideCardTable.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(HideCardTable);

            HideCardTable.SetPosition(ParentPosition.X + HT_PositionR.X,
                ParentPosition.Y + HT_PositionR.Y);
            HideCardTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionR.X + HT_distanceX,
                ParentPosition.Y + HT_PositionR.Y + HT_distanceY);
            HideCardTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionR.X + 2*HT_distanceX,
                ParentPosition.Y + HT_PositionR.Y + 2*HT_distanceY);
            HideCardTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(HideCardTable);
            HideCardTable.SetPosition(ParentPosition.X + HT_PositionR.X + 3 * HT_distanceX,
                ParentPosition.Y + HT_PositionR.Y + 3 * HT_distanceY);
            HideCardTable.SetUVs(1, 0, 0, 1);
            renderer.DrawSprite(HideCardTable);

            //能量
               //框架L
            PowerBolder.SetPosition(ParentPosition.X + P_PositionL.X,
              ParentPosition.Y + P_PositionL.Y - P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionL.X - P_distanceX,
                ParentPosition.Y + P_PositionL.Y + P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionL.X - 2 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y -  P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionL.X - 3 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y + P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionL.X - 4 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y - P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
                //框架R
            PowerBolder.SetPosition(ParentPosition.X + P_PositionR.X,
              ParentPosition.Y + P_PositionR.Y - P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionR.X + P_distanceX,
                ParentPosition.Y + P_PositionR.Y + P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionR.X + 2 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y - P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionR.X + 3 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y + P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
            PowerBolder.SetPosition(ParentPosition.X + P_PositionR.X + 4 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y - P_distanceY);
            PowerBolder.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerBolder);
               //能量满：
            //框架L
            float P_offsety = 1;
            PowerFull.SetPosition(ParentPosition.X + P_PositionL.X,
              ParentPosition.Y + P_PositionL.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionL.X - P_distanceX,
                ParentPosition.Y + P_PositionL.Y + P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionL.X - 2 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionL.X - 3 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y + P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionL.X - 4 * P_distanceX,
                ParentPosition.Y + P_PositionL.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            //框架R
            PowerFull.SetPosition(ParentPosition.X + P_PositionR.X,
              ParentPosition.Y + P_PositionR.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionR.X + P_distanceX,
                ParentPosition.Y + P_PositionR.Y + P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionR.X + 2 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionR.X + 3 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y + P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
            PowerFull.SetPosition(ParentPosition.X + P_PositionR.X + 4 * P_distanceX,
                ParentPosition.Y + P_PositionR.Y - P_distanceY + P_offsety);
            PowerFull.SetUVs(0, 0, 1, 1);
            renderer.DrawSprite(PowerFull);
        }
    }


}
