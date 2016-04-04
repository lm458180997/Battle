using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace FastLoopExample.Battle
{

    public class BattleStage1 : IGameObject
    {
         
        Renderer _renderer = new Renderer();        // 着色器（普通着色器，以正中间计算）

        StateSystem _system;                        // 状态机(控制场景切换)

        TextureManager _textureManager;             // 纹理控制器

        SoundManagerEx soundmanager;                // 声音控制器

        Battle.Players.BattlePlayer player1;        // 玩家1

        bool IsServer = true;    //是否为服务器
        Battle.Net.ServerTool server;
        Battle.Net.Customer customer;


        Sprite bksp = new Sprite();
        Renderer render = new Renderer();


        public BattleStage1(StateSystem sys, TextureManager _t, SoundManagerEx _s)
        {
            _system = sys;               //状态机
            _textureManager = _t;        //纹理管理器
            soundmanager = _s;           //声音管理器

            player1 = new Players.BattlePlayer(_t, 0);  //实例化玩家

            customer = new Net.Customer();

            customer.Run();


            bksp.Texture = _textureManager.Get("back");
            bksp.SetWidth(1000);
            bksp.SetHeight(480);
            bksp.SetPosition(0, 100);

        }

        //切换场景时会自动调用Start
        public void Start()
        {
            player1.Start();
        }

        double time_caculate = 0;

        //逻辑更新
        public void Update(double elapsedTime)
        {
            //-------------发送信息给服务器端-----------------//
            Vector2D vct = player1.CurrentCharactor.Position;
            Net.PlayerData dt = new Net.PlayerData();
            player1.ClonePeopleData(dt);
            customer.SendData(dt.player_x, dt.player_y, dt.playerstate,
                dt.adaptorstate, dt.currentindex, dt.totleindex,dt.speed,dt.caculatetime);
            //-------------发送信息给服务器端-----------------//

            DealWithCommand(elapsedTime);//处理选项命令

            player1.Update(elapsedTime); //player的逻辑更新

        }

        //处理各项命令
        public void DealWithCommand(double elapsedTime)
        {
            
        }

        //渲染函数
        public void Render()
        {
            #region 刷新背景（黑色填充）
            //Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            //Gl.glClearColor(1, 1, 1, 1.0f);
            //Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            #endregion

            render.DrawSprite(bksp);

            player1.Render();

            Gl.glFinish();
        }


    }

}
