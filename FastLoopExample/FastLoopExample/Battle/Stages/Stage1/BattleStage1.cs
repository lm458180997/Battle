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

        Stages.BattleComponents battlecomponents;   //战斗的相关组件（血槽，能量槽，框架等） 


        Battle.Players.BattlePlayer player1;        // 玩家1
        Battle.Players.BattlePlayer player2;        // 玩家1

        bool IsServer = true;    //是否为服务器
        Battle.Net.ServerTool server; //服务器

        Sprite bksp = new Sprite();
        Renderer render = new Renderer();

        public BattleStage1(StateSystem sys, TextureManager _t, SoundManagerEx _s)
        {
            _system = sys;               //状态机
            _textureManager = _t;        //纹理管理器
            soundmanager = _s;           //声音管理器

            player1 = new Players.BattlePlayer(_t, 0);  //实例化玩家
            player2 = new Players.BattlePlayer(_t, 0);  //实例化玩家
            player2.SetPeopleKeyControl(false);         //player2不能通过键盘来控制

            if (IsServer)
            {
                server = new Battle.Net.ServerTool();
                server.Run();
            }
            bksp.Texture = _textureManager.Get("back");
            bksp.SetWidth(1400/2);
            bksp.SetHeight(1200/2);
            bksp.SetPosition(0, 100);

            battlecomponents = new Stages.BattleComponents(_textureManager);

        }

        //切换场景时会自动调用Start
        public void Start()
        {
            player1.Start();
        }

        //逻辑更新
        public void Update(double elapsedTime)
        {
            DealWithCommand(elapsedTime);//处理选项命令

            //-------------设置返送给客户端的信息-----------------//
            server.SetPlayerData(player1);
            //-------------设置返送给客户端的信息-----------------//

            //-------------绑定来自客户端的信息-------------------//
            //将从网络上获取的玩家数据同步绑定到目标角色上
            if (Battle.Net.ServerTool.HaveNewData)
            {
                player2.BindPeopleData(Battle.Net.ServerTool.data);
                Battle.Net.ServerTool.HaveNewData = false;
            }
            //-------------绑定来自客户端的信息-------------------//


            player1.Update(elapsedTime); //player的逻辑更新
            player2.Update(elapsedTime);

            MassageManager manager = new MassageManager();
            //player1 对 player2 造成的碰撞
            if (player1.Collision(player2, ref manager))
            {
                //根据筛选，找出Massage.SpaceCollision的分类信息
                Massage[] msgs = manager.SelectMessages(Massage.SpaceCollision);
                if (msgs != null)
                {
                    foreach (Massage m in msgs)
                        player2.CurrentCharactor.AddMassage(m);
                }
            }
            //player2 执行信息队列
            player2.DealWidthMassage();
            //清空队列
            manager.ClearMessage();

            //player2 对 player1 造成的碰撞
            if (player2.Collision(player1, ref manager))
            {
                //根据筛选，找出Massage.SpaceCollision的分类信息
                Massage[] msgs = manager.SelectMessages(Massage.SpaceCollision);
                if (msgs != null)
                {
                    foreach (Massage m in msgs)
                        player1.CurrentCharactor.AddMassage(m);
                }
            }
            //player1 执行信息队列
            player1.DealWidthMassage();
            //清空队列
            manager.ClearMessage();


            if (Input.getKeyDown("S"))
            {
                Datas.ShowCollisionRect = !Datas.ShowCollisionRect;
            }

            //执行组件的逻辑更新
            RunComponents(elapsedTime);

        }

        /// <summary>
        /// 更新各类组件
        /// </summary>
        /// <param name="elapsedTime">时间差值</param>
        public void RunComponents(double elapsedTime)
        {
            battlecomponents.Update(elapsedTime);    //战斗相关组件的更新
        }


        //处理各项命令
        public void DealWithCommand(double elapsedTime)
        {
            
        }

        //渲染函数
        public void Render()
        {
            #region 刷新背景（黑色填充）
            Gl.glShadeModel(Gl.GL_LINE_SMOOTH);
            Gl.glClearColor(0, 0, 0, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            #endregion

            //投影模式
            //Gl.glMatrixMode(Gl.GL_PROJECTION);

            render.DrawSprite(bksp);

            player2.Render();
            player1.Render();

            //正交模式
            //Gl.glMatrixMode(Gl.GL_MODELVIEW);

            //UI部分
            battlecomponents.Render();

            Gl.glFinish();
        }


    }

}
