using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample.Battle.Stages.Menu
{
    public interface KeyFocusTest
    {
        void RunKey(int type);
    }

    public class MenuStage : IGameObject
    {
        Renderer _renderer = new Renderer();        // 着色器（普通着色器，以正中间计算）

        StateSystem _system;                        // 状态机(控制场景切换)

        Form1 form;

        TextureManager _textureManager;             // 纹理控制器

        SoundManagerEx soundmanager;                // 声音

        BackGround background;  //背景组件

        SelectComponent selectcomponent;  //选择框组件

        Shade shade;                      //墙组件

        ConnectComponent connectcomponent;          //连接组件

        DefaultComponent defaultcomponent;          //默认组件（测试用）

        Sprite Logo = new Sprite();                  
        Renderer renderer = new Renderer();

        public static int CurrentFocus = 0;          //当前焦点在哪个组件上 
        public const int SelectComponent=0;          //焦点在SelectComponent上
        public const int ConnectComponent = 1;       //焦点在connectComponent上
        public const int DefaultComponent = 10;       //焦点在connectComponent上
        public static bool NeedShowShade = false;    //静态字段，等待组件通知是否需要开启或关闭遮挡
        public static bool NeedHideShade = false;
        public static bool NeedStageChange = false;  //是否需要切换场景
        public static String ChangeStageName = "";   //需要切换的对应场景的名称

        public MenuStage(Form1 form, StateSystem sys, TextureManager _t, SoundManagerEx _s)
        {
            this.form = form;
            _system = sys;               //状态机
            _textureManager = _t;        //纹理管理器
            soundmanager = _s;           //声音管理器
            background = new BackGround(_t);
            selectcomponent = new SelectComponent(_t);
            shade = new Shade(_textureManager);
            connectcomponent = new ConnectComponent(_textureManager);
            defaultcomponent = new Menu.DefaultComponent(_t);

            Logo.Texture = _textureManager.Get("2_title");
            Logo.SetWidth(512); Logo.SetHeight(512);
            Logo.SetPosition(100, 170);

        }

        //切换场景时会自动调用Start
        public void Start()
        {

        }

        //逻辑更新
        public void Update(double elapsedTime)
        {
            background.Update(elapsedTime);
            selectcomponent.Update(elapsedTime);
            shade.Update(elapsedTime);
            connectcomponent.Update(elapsedTime);
            if (CurrentFocus == ConnectComponent)
            {
                //接手键盘命令
                connectcomponent.RunKey(ConnectComponent);
            }

            String Item = selectcomponent.SelectItem;
            if (Input.getKeyDown("Z"))
            {
                if (Item == "VsNetwork")
                {
                    //如果已经获得焦点则将信息传递至下一层
                    if (CurrentFocus == ConnectComponent)
                    {
                        //使连接组件捕捉到“Z”
                        connectcomponent.KeyCatch("Z");
                    }
                    //如果没有获得焦点。则转移焦点。并触发动画
                    if (CurrentFocus == SelectComponent)
                    {
                        NeedShowShade = true;
                        connectcomponent.Show();
                        CurrentFocus = ConnectComponent;
                    }
                }
                else if (Item == "Exit")
                {
                    form.Close();
                }
                else if (Item == "Config" || Item == "Replay"
                    || Item == "Musicroom")
                {
                    NeedShowShade = true;
                    defaultcomponent.Show();
                    defaultcomponent.SetName(Item);
                    CurrentFocus = DefaultComponent;
                }
            }
            if (Input.getKeyDown("X"))
            {
                if (Item == "VsNetwork")
                {
                    //使连接组件捕捉到“X” (需要已经获得了焦点)
                    if (CurrentFocus == ConnectComponent)
                    {
                        connectcomponent.KeyCatch("X");
                    }
                }
                else if (Item == "Config" || Item == "Replay"
                    || Item == "Musicroom")
                {
                    NeedShowShade = true;
                    if (CurrentFocus == DefaultComponent)
                       defaultcomponent.KeyCatch("X");
                }
                
            }

            if (NeedShowShade)
            {
                shade.Show();
                NeedShowShade = false;
            }
            if (NeedHideShade)
            {
                shade.Hide();
                NeedHideShade = false;
            }
            if (NeedStageChange)
            {
                NeedStageChange = false;
                _system.ChangeState(ChangeStageName);
            }

        }

        //渲染函数
        public void Render()
        {
            #region 刷新背景（黑色填充）
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            Gl.glClearColor(0, 0, 0, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            #endregion

            background.Render();                //背景要首先进行绘制

            renderer.DrawSprite(Logo);

            selectcomponent.Render();           //选择框组件进行绘制

            shade.Render();                     //墙进行渲染

            connectcomponent.Render();          //组件渲染

            defaultcomponent.Render();          //默认组件渲染

            Gl.glFinish();
        }

    }



}
