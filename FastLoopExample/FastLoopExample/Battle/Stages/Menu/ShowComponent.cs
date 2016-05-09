using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages.Menu
{
    /// <summary>
    /// 展示详细内容的组件
    /// </summary>
    public class ShowComponent :IGameObject,KeyFocusTest
    {
        public TextureManager texturemanager;
        public Sprite Logo = new Sprite();

        public virtual void Start()
        {
        }
        public virtual void Show()
        {
        }
        public virtual void Hide()
        {
        }

        public virtual void Update(double elapsedTime)
        {
        }

        public virtual void Render()
        {
        }
        /// <summary>
        /// 捕捉键盘命令（可以捕捉来自Menu的命令）
        /// </summary>
        /// <param name="Key"></param>
        public virtual void KeyCatch(String Key)
        {
        }
        /// <summary>
        /// 控制键盘命令（为自身调用，不用捕捉Menu）
        /// </summary>
        /// <param name="type"></param>
        public void RunKey(int type)
        {
           
        }
    }

    public class DefaultComponent : ShowComponent
    {
        bool show = false;            //是否显示
        Renderer renderer;
        public Vector2D IpPosition= new Vector2D(-200,100+50);

        public DefaultComponent(TextureManager _t)
        {
            texturemanager = _t;
            renderer = new Renderer();
            Logo.Texture = texturemanager.Get("Network");
            Logo.SetWidth(256);
            Logo.SetHeight(256); Logo.SetPosition(-200, 280);
        }

        public void SetName(String name)
        {
            Logo.Texture = texturemanager.Get(name);
            Logo.SetWidth(256);
            Logo.SetHeight(256);
            Logo.SetPosition(-200, 280);
            if (name == "Musicroom")
            {
                Logo.SetWidth(512);
                Logo.SetHeight(512);
                Logo.SetPosition(-150, 280);
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Show()
        {
            show = true;
        }

        public override void Hide()
        {
            show = false;
        }

        public override void KeyCatch(string Key)
        {
            if (Key == "X")
            {
                MenuStage.NeedHideShade = true;
                show = false;
                MenuStage.CurrentFocus = MenuStage.SelectComponent;   //返回至主界面。归还焦点
            }
        }

        public override void Render()
        {
            if (show)
            {
               
                renderer.DrawSprite(Logo);
            }
        }
    }
    
    /// <summary>
    /// Connect显示组件
    /// </summary>
    public class ConnectComponent : ShowComponent,KeyFocusTest
    {
        bool show = false;            //是否显示
        Renderer renderer;
        NumBerMaker nummaker;         //数字纹理生成器
        public String Ip = "000.000.000.000";         //显示的IP地址
        public String Port = "21555";
        public Vector2D IpPosition= new Vector2D(-200,100+50);

        int Itemscount = 0;//选项的数量
        Dictionary<String, Sprite> ItemSprites;    //标签对应的纹理
        Dictionary<int, String> Map;               //索引值映射上的标签
        public String SelectItem = "StoryMode";     //选中项的名字
        int SelectIndex = 0;            //选中项的索引值
        /// <summary>
        /// 初始化映射表
        /// </summary>
        void initMap()
        {
            Map = new Dictionary<int, string>();
            int i = 0;
            Map.Add(i, "Server"); i++;
            Map.Add(i, "IP"); i++;
            Map.Add(i, "Exit"); i++;
            Itemscount = i;
        }
        /// <summary>
        /// 初始化纹理
        /// </summary>
        void initSprites()
        {
            ItemSprites = new Dictionary<string, Sprite>();
            Sprite sp = new Sprite();
            sp.Texture = texturemanager.Get("00a_Server");
            sp.SetWidth(256); sp.SetHeight(32);
            ItemSprites.Add("Server", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("01a_IP");
            sp.SetWidth(256); sp.SetHeight(32);
            ItemSprites.Add("IP", sp);

            sp = new Sprite();
            sp.Texture = texturemanager.Get("06a_Exit");
            sp.SetWidth(32); sp.SetHeight(32);
            ItemSprites.Add("Exit", sp);
        }

        public ConnectComponent(TextureManager _t)
        {
            texturemanager = _t;
            nummaker = new NumBerMaker(_t);
            renderer = new Renderer();
            Ip = Datas.Adress;
            Logo.Texture = texturemanager.Get("Network");
            Logo.SetWidth(256);
            Logo.SetHeight(256); Logo.SetPosition(-200, 280);
            initMap(); //初始化映射表
            initSprites();  //初始化纹理
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Show()
        {
            show = true;
        }

        public override void Hide()
        {
            show = false;
        }

        void KeyFocusTest.RunKey(int type)
        {
            //控制命令
            if (type == MenuStage.ConnectComponent)
            {
                if (Input.getKeyDown("Down"))
                {

                }
                if (Input.getKeyDown("Up"))
                {

                }
                if (Input.getKeyDown("Left"))
                {

                }
                if (Input.getKeyDown("Right"))
                {

                }
            }
        }

        public override void KeyCatch(string Key)
        {
            if (Key == "Z")
            {
                MenuStage.NeedStageChange = true;
                MenuStage.ChangeStageName = "BattleStage1";
            }

            if (Key == "X")
            {
                MenuStage.NeedHideShade = true;
                show = false;
                MenuStage.CurrentFocus = MenuStage.SelectComponent;   //返回至主界面。归还焦点
            }
        }

        public override void Update(double elapsedTime)
        {
            //根据当前的焦点来判断是否可以执行RunKey
            RunKey(MenuStage.CurrentFocus);
        }

        public override void Render()
        {
            if (show)
            {
                Sprite sp;
                int w = nummaker.GetWidth();
                int h = nummaker.GetHeight();
                int offset =0;
                for (int i = 0; i < Ip.Length; i++)
                {
                    sp = nummaker.GetSprite(Ip.Substring(i, 1));
                    sp.SetWidth(w); sp.SetHeight(h);
                    sp.SetPosition(IpPosition.X + offset, IpPosition.Y);
                    renderer.DrawSprite(sp);
                    offset += (w + 3);
                }
                offset += 20;
                for (int i = 0; i < Port.Length; i++)
                {
                    sp = nummaker.GetSprite(Port.Substring(i, 1));
                    sp.SetWidth(w); sp.SetHeight(h);
                    sp.SetPosition(IpPosition.X + offset, IpPosition.Y);
                    renderer.DrawSprite(sp);
                    offset += (w + 3);
                }
                renderer.DrawSprite(Logo);

                int y = 50;
                for (int i = 0; i < Itemscount-1; i++)
                {
                    Sprite sp2 = ItemSprites[Map[i]];
                    sp2.SetPosition(-100, y);
                    renderer.DrawSprite(sp2);
                    y -= 36;
                }
                Sprite sp3 = ItemSprites[Map[Itemscount - 1]];
                sp3.SetPosition(-211, y);
                renderer.DrawSprite(sp3);
                y -= 36;

            }
        }

    }
    /// <summary>
    /// connect组件的零件（显示和设置IP的模块）
    /// </summary>
    public class connect_Box : IGameObject
    {

        TextureManager texturemanager;
        Sprite box;
        public connect_Box(TextureManager tmg)
        {
            texturemanager = tmg;
            box = new Sprite();


        }
        

        public void Start()
        {
           
        }

        public void Update(double elapsedTime)
        {
            
        }

        public void Render()
        {
           
        }
    }



    /// <summary>
    /// 数字生产工具
    /// </summary>
    public class NumBerMaker
    {
        public const int Default = 2;
        public int Type = Default;
        public int[] Width = new int[10];
        public int[] Height = new int[10];
        public TextureManager texturemanager;
        public Dictionary<String, Sprite> Sprites;
        public const float per_256 = 0.03125f;
        public NumBerMaker(TextureManager _t)
        {
            texturemanager = _t;
            Sprites = new Dictionary<string, Sprite>();
            initTypes();
            initSprites();
        }
        void initTypes()
        {
            Width[0] = 48;
            Height[0] = 40;
            Width[1] = 12;
            Height[1] = 16;
            Width[2] = 12;
            Height[2] = 16;
        }
        void initSprites()
        {
            Sprite sp = new Sprite();
            float offset = per_256*1.368f;
            float offsetx = 0.005f;
            for (int i = 0; i < 10; i++)
            {
                sp = new Sprite();
                sp.Texture = texturemanager.Get("0000");
                sp.SetUVs(offsetx + i * offset, 1 - 3.4f * per_256,offsetx+ (i + 1) * offset, 1 - 1.66f * per_256);
                sp.SetWidth(Width[Type]); sp.SetHeight(Height[Type]);
                Sprites.Add(i.ToString(), sp);
            }
            sp = new Sprite();
            sp.Texture = texturemanager.Get("0000");
            sp.SetUVs(offsetx+0.002f + 10 * offset, 1 - 3.4f * per_256, offsetx+0.002f + 11 * offset, 1 - 1.5f * per_256);
            sp.SetWidth(Width[Type]); sp.SetHeight(Height[Type]);
            Sprites.Add(".", sp);

            //for (int i = 0; i < 10; i++)
            //{
            //    sp = new Sprite();
            //    sp.Texture = texturemanager.Get("0000");
            //    sp.SetUVs(i * offset, 1 - 3.4f * per_256, (i + 1) * offset, 1 - 1.66f * per_256);
            //    sp.SetWidth(Width[Type]); sp.SetHeight(Height[Type]);
            //    Sprites.Add(i.ToString(), sp);
            //}
            //sp = new Sprite();
            //sp.Texture = texturemanager.Get("0000");
            //sp.SetUVs(10 * offset, 1 - 3.5f * per_256, 11 * offset, 1 - 1.5f * per_256);
            //sp.SetWidth(Width[Type]); sp.SetHeight(Height[Type]);
            //Sprites.Add(".", sp);

        }
        public void BindType(int tp)
        {
            Type = tp;
        }
        public int GetWidth()
        {
            return Width[Type];
        }
        public int GetHeight()
        {
            return Height[Type];
        }
        public Sprite GetSprite(String key)
        {
            return Sprites[key];
        }

    }




}
