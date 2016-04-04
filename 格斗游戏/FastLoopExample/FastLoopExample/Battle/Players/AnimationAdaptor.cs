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
        public const byte None = 0;
        public byte Type = 0;
        public byte currentComplete = 0;     //当前完成的数量
        public byte testTotle = 0;           //总共需要完成的数量
        public bool Isloop = false;          //是否一直循环
        public bool IsOver = false;                  //是否已经执行完毕

        public object Data1;                 //参数1
        public object Data2;                 //参数2

        public BattleAnimation animation;    //适配器对应的Animation

        public virtual void SetCurrentIndex(byte index)
        {
            animation.CurrentIndex = index;
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
        public virtual Sprite GetSprite()
        {
            return animation.Sprites[animation.CurrentIndex];
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


    }


}
