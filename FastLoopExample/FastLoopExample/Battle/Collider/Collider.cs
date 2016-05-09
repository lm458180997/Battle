using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle
{
    /// <summary>
    /// 碰撞器
    /// </summary>
    public class Collider
    {
        public int Type = _RectCollider;
        public const int _RectCollider = 0;  //矩形碰撞器（底边上中心点，高，宽）
        /// <summary>
        /// 碰撞判定，返回值为0及其上数，描述碰撞的信息
        /// </summary>
        /// <param name="collider">对方的碰撞器</param>
        /// <returns>描述碰撞的信息</returns>
        public virtual byte Collision(Collider collider)
        {
            return 0;
        }

    }

    public class Rect
    {

    }

    public class RectCollider : Collider
    {
        public Vector2D Position = new Vector2D();   //矩形碰撞器的底边中心坐标
        public float width,height;                   //宽度以及高度

        public RectCollider(float w, float h)
        {
            width = w;
            height = h;
        }
        /// <summary>
        /// 设置碰撞器所在坐标
        /// </summary>
        /// <param name="x">所在x值</param>
        /// <param name="y">所在y值</param>
        public void SetPosition(float x, float y)
        {
            Position.X = x;
            Position.Y = y;
        }
        /// <summary>
        /// 设定大小
        /// </summary>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        public void SetSize(int w, int h)
        {
            width = w;
            height = h;
        }
        /// <summary>
        /// 获取碰撞器的属性
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        public void GetProperties(out float x, out float y, out float w, out float h)
        {
            x = (float)Position.X; y = (float)Position.Y;
            w = width; h = height;
        }
        /// <summary>
        /// 碰撞判定，返回值为-1及以上数，描述碰撞的信息
        /// </summary>
        /// <param name="collider">对方的碰撞器</param>
        /// <returns>描述碰撞的信息</returns>
        public override byte Collision(Collider collider)
        {
            if(collider.Type == Collider._RectCollider)
            {
                RectCollider rt = collider as RectCollider;
                float x, y, w, h;
                //从对方碰撞器中获取坐标以及大小信息
                rt.GetProperties(out x, out y, out w, out h);
                y+= (h/2);          //中心点平移至矩形中心
                float orix = (float)Position.X; float oriy = (float)Position.Y;
                oriy += height/2;   //中心点平移至矩形中心
                float offsetx = x - orix;
                float offsety = y - oriy;
                bool isright = true, isup = true;     //打中的是目标矩形的哪一个区域
                if (offsetx > 0)
                    isright = false;
                if (offsety > 0)
                    isup = false;
                //获取绝对值
                offsetx = offsetx < 0 ? -offsetx : offsetx;
                offsety = offsety < 0 ? -offsety : offsety;
                bool collision = false;
                //是否满足了碰撞
                if ((offsetx < (w / 2 + width / 2)) && (offsety < (h / 2 + height / 2)))
                    collision = true;
                if (!collision)
                    return 0;
                //返回4个区域  右上 1  ， 右下 2 ， 左上 3 ， 左下4 
                if (isright)
                {
                    if (isup)
                        return 1;
                    else
                        return 2;
                }
                else
                {
                    if (isup)
                        return 3;
                    else
                        return 4;
                }
            }
            return 0;
        }
    }

}
