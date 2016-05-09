using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle
{
    /// <summary>
    /// 信息基类
    /// </summary>
    public class Massage 
    {
        public const byte NONE = 0;   //无内容信息
        public const byte SpaceCollision = 1;  //人物不能重叠，若重叠则会引发的的空间挤撞信息（信息由空间信息接管）
        public const byte CollisionMassege = 2;//碰撞相关的一类信息（信息由碰撞信息接管）
        public int Tag = 0;        //报告信息标签[表示应该由哪种信息来接管]
        public object sender;      //信息的发送对象
        public String msg;         //信息的携带信息（字符串）        
    }

    public class SpaceCollisionMassage : Massage
    {
        public float X, Y;         //碰撞的原对象的坐标
        float speed = 300;    //碰撞对自己的影响（每秒应该偏移多少的距离）
        int direction = 1;  //1代表向右碰撞，-1代表向左碰撞
        /// <summary>
        /// 空间挤压信息
        /// </summary>
        /// <param name="x">原对象的x值</param>
        /// <param name="y">原对象的y值</param>
        /// <param name="sd">信息的发送对象</param>
        public SpaceCollisionMassage(float x, float y, object sd ,bool toleft = false)
        {
            Tag = SpaceCollision;     //空间挤压碰撞信息
            X = x; Y = y;             //碰撞的原对象的坐标
            sender = sd;
            msg = "SpaceCollisionMassege from " + sd.ToString();
            if (toleft)
                direction = -1;
        }
        public void SetSpeed(float spd)
        {
            this.speed = spd;
        }
        public float GetSpeed()
        {
            return speed;
        }
        public float GetDirection()
        {
            return direction;
        }
        public float Getoffset()
        {
            return direction * speed;
        }
        public void Setdirection(bool isleft)
        {
            if (isleft)
                direction = -1;
            else
                direction = 1;
        }
    }

    /// <summary>
    /// 碰撞信息
    /// </summary>
    public class ColliderMassages : Massage
    {
        //PeopleType  人物类型
        public byte PeopleType = 0;
        //AttackType  攻击类型
        public byte AttackType = 0;
    }

    /// <summary>
    /// 蕾米利亚适配器的传递信息
    /// </summary>
    public class RemiliaAdaptorMessage : ColliderMassages
    {

        public RemiliaAdaptorMessage()
        {

        }

    }


}
