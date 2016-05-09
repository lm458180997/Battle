using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{

    //所有实体类的基  entity
    public abstract class Entity : IGameObject
    {
        //坐标
        public Vector2D Position = new Vector2D();
        //运动方向
        public Vector2D Direction = new Vector2D();
        //旋转方向
        public Vector2D Rotation = new Vector2D();
        //缩放
        public Vector2D Scale = new Vector2D();
        //宽与高
        public float Width;
        public float Height;
        public Sprite sprite;
        public virtual void Start() { }
        public virtual void Update(double elapsedTime) { }
        public virtual void Render() { }
    }


    // 碰撞体结构  （含有碰撞判定的物体都必须实现此接口）  ， 
    // 碰撞判定的时候，尽量是用子弹来完成与玩家（敌人）的判定, 而非玩家来指向子弹，
    // 因为玩家和敌人的碰撞判定是比较简单的， 这样可以提高效率
    public interface Collider
    {
        bool Collision(Collider c);           //判断碰撞体之间的碰撞
    }

    public interface RectCollider : Collider   //矩形碰撞体，所不同的是它含有了坐标以及大小属性
    {
        double GetX();
        double GetY();
        double GetWidth();
        double GetHeight();
    }

    public interface RoundCollider : Collider   //圆形碰撞体，所不同的是它含有了具有圆的属性
    {
        double GetX();
        double GetY();
        double GetRadius();
    }

    public struct RecTangleF
    {
        public float left, top, right, bottom;
        public RecTangleF(float l, float t, float r, float b)
        {
            left = l; top = t; right = r; bottom = b;
        }

    }

}
