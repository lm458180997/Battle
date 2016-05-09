using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Players
{

    public class BattlePlayer : Entity
    {

        public BattlePeoples CurrentCharactor;    //当前角色

        public int PlayerType = 0;         //人物的类型，0——player1 ，1——player2

        Net.PlayerData playerdata = new Net.PlayerData();          //玩家数据    
       
        public BattlePlayer(TextureManager texturemanager,int type = 0)
        {
            //玩家类型（为Player1，或是为Player2）
            PlayerType = type;

            //生成一个人物数据实例
            BattlePeoples p = new Battle.Players.Remilia(texturemanager);

            //注册人物
            RegisteredRCharactors(p);

        }

        /// <summary>
        /// 注册人物
        /// </summary>
        /// <param name="fc">人物</param>
        public void RegisteredRCharactors(BattlePeoples fc)
        {
            CurrentCharactor = fc;
        }

        /// <summary>
        /// 设置人物的键盘控制情况
        /// </summary>
        /// <param name="able">能否处于玩家控制</param>
        public void SetPeopleKeyControl(bool able)
        {
            CurrentCharactor.InControl = able;
        }

        /// <summary>
        /// Start初始化
        /// </summary>
        public override void Start()
        {
            base.Start();
            CurrentCharactor.Start();
        }
        /// <summary>
        /// 绑定人物数据
        /// </summary>
        /// <param name="dt"></param>
        public void BindPeopleData(Net.PlayerData dt)
        {
            CurrentCharactor.BindPeopleData(dt);
        }
        /// <summary>
        /// 复制人物数据
        /// </summary>
        /// <param name="dt"></param>
        public void ClonePeopleData(Net.PlayerData dt)
        {
            CurrentCharactor.ClonePeopleData(dt);
        }

        public override void Render()
        {
            base.Render();
            CurrentCharactor.Render();

        }

        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);

            // CurrentCharactor  Update
            CurrentCharactor.Update(elapsedTime);

        }

        /// <summary>
        /// 被击中后的处理
        /// </summary>
        /// <returns>返回是否成功打击</returns>
        public bool Hitted()
        {

            return false;
        }

        //更新连击（产生攻击间隔判定）
        void UpdateAttack(double elapsedTime)
        {
            
        }

        //攻击连击
        public void Attack()
        {
            
        }

        /// <summary>
        /// 人物的对人物的碰撞判定
        /// </summary>
        /// <param name="people">敌方人物</param>
        /// <param name="manager">信息管理器</param>
        /// <returns>是有具有有效碰撞</returns>
        public virtual bool Collision(BattlePeoples people, ref MassageManager manager)
        {
            return CurrentCharactor.Collision(people, ref manager);
        }

        /// <summary>
        /// 玩家的对玩家的碰撞判定
        /// </summary>
        /// <param name="people">敌人</param>
        /// <param name="manager">信息管理器</param>
        /// <returns>是有具有有效碰撞</returns>
        public virtual bool Collision(BattlePlayer player, ref MassageManager manager)
        {
            return CurrentCharactor.Collision(player.CurrentCharactor, ref manager);
        }

        public void AddMassage(Massage msg)
        {
            CurrentCharactor.AddMassage(msg);
        }
        public void DealWidthMassage()
        {
            CurrentCharactor.DealWidthMassages();
        }

    }

}
