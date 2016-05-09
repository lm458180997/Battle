using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Players
{
    /// <summary>
    /// 动画适配器（用于将动画实现到具体的动作状态上）
    /// </summary>
    public class AnimationAdaptor :IGameObject
    {
        public const byte None = 0;          //类型种类
        public byte Type = 0;                //适配器类型[暂无作用]
        public byte currentComplete = 0;     //当前完成的数量
        public byte testTotle = 0;           //总共需要完成的数量
        public bool Isloop = false;          //是否一直循环
        public bool IsOver = false;          //是否已经执行完毕
        public Vector2D Position = new Vector2D(0,0);            //适配器需要对应的坐标

        public RectCollider collider= new RectCollider(0,0); //每一个适配器都有着一个自己的一个身体碰撞区域（身体主干的矩形）

        public object Data1;                 //参数1[暂无作用]
        public object Data2;                 //参数2[暂无作用]

        public BattleAnimation animation;    //适配器对应的Animation

        public virtual void SetCurrentIndex(byte index)
        {
            animation.CurrentIndex = index;
        }

        public void SetPosition(double x, double y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public virtual void Start()
        {
        }
        public virtual void Update(double elapsedTime)
        {
        }
        public virtual void Render()
        {
            
        }
        /// <summary>
        /// 获取动画中对应的Sprite
        /// </summary>
        /// <returns>返回对应Sprite</returns>
        public virtual Sprite GetSprite(bool reserve = false)
        {
            return GetSpriteWithPos(Position.X, Position.Y, reserve);
        }

        public virtual Sprite GetSprite()
        {
            return animation.GetSprite();
        }

        /// <summary>
        /// 获取纹理并设置坐标（根据具体纹理会进行具体的偏移改动）
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public virtual Sprite GetSpriteWithPos(double x, double y, bool reserve = false)
        {
            Sprite sp = animation.GetSprite();
            sp.SetPosition(x, y);
            return sp;
        }
        /// <summary>
        /// 获取碰撞器，若没有则返回null
        /// </summary>
        /// <returns>获取碰撞器，若没有则返回null</returns>
        public virtual RectCollider getcollider(double x, double y)
        {
            //+8的目的是去除脚下的空格
            collider.SetPosition((float)x, (float)y+8);
            return collider;
        }
        public virtual RectCollider getcollider(float x, float y)
        {
            collider.SetPosition(x, y);
            return collider;
        }
        /// <summary>
        /// 获得碰撞器
        /// </summary>
        /// <returns>自动匹配坐标</returns>
        public virtual RectCollider getcollider()
        {
            collider.SetPosition((float)Position.X, (float)Position.Y+8);
            return collider;
        }

        public virtual bool Collision(double x,double y, BattlePeoples people, ref MassageManager manager)
        {
            bool havemassage = false;
            RectCollider cld = people.GetBody();
            int collisionresult =0;
            collider.SetPosition((float)x,(float)y);
            collisionresult = collider.Collision(cld);
            if (collisionresult > 0)
            {
                havemassage = true;
                SpaceCollisionMassage msg;
                if (collisionresult > 2)
                {
                    msg = new SpaceCollisionMassage((float)x, (float)y, this);
                }
                else
                {
                    msg = new SpaceCollisionMassage((float)x, (float)y, this,true);
                }
                manager.AcceptMessage(msg);
            }
            return havemassage;
        }
        public virtual bool Collision(BattlePeoples people, ref MassageManager manager)
        {
            double x = Position.X; double y = Position.Y;
            bool havemassage = false;
            RectCollider cld = people.GetBody();
            int collisionresult = 0;
            collider.SetPosition((float)x, (float)y);
            collisionresult = collider.Collision(cld);
            if (collisionresult > 0)
            {
                havemassage = true;
                SpaceCollisionMassage msg;
                if (collisionresult > 2)
                {
                    msg = new SpaceCollisionMassage((float)x, (float)y, this);
                }
                else
                {
                    msg = new SpaceCollisionMassage((float)x, (float)y, this);
                }
            }
            return havemassage;
        }

    }


}
