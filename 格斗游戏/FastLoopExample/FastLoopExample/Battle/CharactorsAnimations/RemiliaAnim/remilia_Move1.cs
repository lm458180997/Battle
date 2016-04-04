using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.CharactorsAnimations
{
    /// <summary>
    /// 纹理帮助工具（用于快速部署纹理资源）
    /// </summary>
    public class BindTextureTools
    {
        /// <summary>
        /// 给BattleAnimation快速部署纹理资源
        /// </summary>
        /// <param name="_t">纹理管理器</param>
        /// <param name="name">纹理的普遍头命名</param>
        /// <param name="Sprites">需要部署的sprite数组</param>
        /// <param name="startindex">第一张纹理的序号</param>
        /// <param name="count">部署的纹理张数</param>
        public static void BindTexures(TextureManager _t, string name, Sprite[] Sprites, 
            int startindex,int count,int width = 32, int height = 32)
        {
            string head = name;
            string middle = "";
            string num;
            Sprite sp;
            for (int i = 0; i < count; i++)
            {
                num = i.ToString();
                if (num.Length == 2)
                {
                    middle = "0" + num;
                }
                else
                    middle = "00" + num;
                sp = new Sprite();
                sp.Texture = _t.Get(head + middle);
                sp.SetWidth(width);
                sp.SetHeight(height);
                Sprites[i+startindex] = sp;
            }
        }
    }


    /// <summary>
    /// 蕾米利亚的一般站立动画
    /// </summary>
    public class remilia_stand : BattleAnimation 
    {
        public remilia_stand(TextureManager texturemanager)
        {
            //站立动画一般都为循环式的
            needloop = true;
            //实例化动画数组
            Sprites = new Sprite[8];
            //设定起始索引，以及最后索引（这里举例为0-7）
            startIndex = 0;
            lastIndex = 7;
            name = "data_character_remilia_stand";
            //调用工具函数快速部署8张纹理至Sprites
            BindTextureTools.BindTexures(texturemanager, name, Sprites, 0, 8);
        }
        /// <summary>
        /// 动画更新
        /// </summary>
        /// <param name="ElapsedTime">帧间时间</param>
        public override void AnimUpdate(double ElapsedTime)
        {
            caculatetime += ElapsedTime;
            //每过一个时间[changeinterval]间隔更新一帧图片
            if (caculatetime >= changeinterval)
            {
                caculatetime -= changeinterval;
                if (CurrentIndex < lastIndex)
                {
                    CurrentIndex++;
                }
                else
                {
                    if (needloop)
                    {
                        CurrentIndex = startIndex;
                    }
                    else
                    {
                        CurrentIndex = lastIndex;
                        IsOver = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <returns>返回当前sprite</returns>
        public override Sprite GetSprite()
        {
           Sprite sp= Sprites[CurrentIndex];
           sp.SetWidth(128);
           sp.SetHeight(128);
           return sp;
        }
    }
    /// <summary>
    /// 蕾米利亚的一般站立动画对应的适配器
    /// </summary>
    public class remilia_stand_Adaptor : Players.AnimationAdaptor
    {
        public remilia_stand_Adaptor(TextureManager texturemanager)
        {
            animation =new remilia_stand(texturemanager);
        }
        public override void Start()
        {
            //调用Start以后，列表重置
            animation.CurrentIndex = animation.startIndex;
        }
        public override void Update(double elapsedTime)
        {
            animation.Update(elapsedTime);
        }
        public override Sprite GetSprite()
        {
            return animation.GetSprite();
        }

        //不同图片在x，y上的偏移量
        //                          0    1   2   3   4   5   6   7  
        int[] offsetx = new int[] { -8, -8, -8, -8, -8, -8, -8, -8 };
        int[] offsety = new int[] { 24, 24, 24, 24, 24, 24, 24, 24 };

        /// <summary>
        /// 获取纹理，并设置坐标（自动完成纹理的对齐）
        /// </summary>
        /// <param name="x">纹理对应的x坐标</param>
        /// <param name="y">纹理对应的y坐标</param>
        /// <param name="reserve">是否横轴反向</param>
        /// <returns>获取适配器纹理</returns>
        public override Sprite GetSpriteWithPos(double x, double y, bool reserve = false)
        {
            int index = animation.CurrentIndex;
            Sprite sp = animation.GetSprite();
            if (reserve)
            {
                sp.SetPosition(x - offsetx[index], y + offsety[index]);
                sp.SetUVs(1, 0, 0, 1);
            }
            else
            {
                sp.SetPosition(x + offsetx[index], y + offsety[index]);
                sp.SetUVs(0, 0, 1, 1);
            }
            sp.SetWidth(128);
            sp.SetHeight(128);
            return sp;
        }

    }

    public class remilia_walkFront : BattleAnimation
    {
        public remilia_walkFront(TextureManager texturemanager)
        {
            changeinterval = 0.08;
            //站立动画一般都为循环式的
            needloop = true;
            //实例化动画数组
            Sprites = new Sprite[8];
            //设定起始索引，以及最后索引（这里举例为0-7）
            startIndex = 0;
            lastIndex = 7;
            name = "data_character_remilia_walkFront";
            //调用工具函数快速部署8张纹理至Sprites
            BindTextureTools.BindTexures(texturemanager, name, Sprites, 0, 8);
        }
        /// <summary>
        /// 动画更新
        /// </summary>
        /// <param name="ElapsedTime">帧间时间</param>
        public override void AnimUpdate(double ElapsedTime)
        {
            caculatetime += ElapsedTime;
            //每过一个时间[changeinterval]间隔更新一帧图片
            if (caculatetime >= changeinterval)
            {
                caculatetime -= changeinterval;
                if (CurrentIndex < lastIndex)
                {
                    CurrentIndex++;
                }
                else
                {
                    if (needloop)
                    {
                        CurrentIndex = startIndex;
                    }
                    else
                    {
                        CurrentIndex = lastIndex;
                        IsOver = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <returns>返回当前sprite</returns>
        public override Sprite GetSprite()
        {
            Sprite sp = Sprites[CurrentIndex];
            sp.SetWidth(128);
            sp.SetHeight(128);
            return sp;
        }

    }

    public class remilia_walkFront_Adaptor : Players.AnimationAdaptor
    {
        public remilia_walkFront_Adaptor(TextureManager texturemanager)
        {
            animation = new remilia_walkFront(texturemanager);
        }
        public override void Start()
        {
            //调用Start以后，列表重置
            animation.CurrentIndex = animation.startIndex;
        }
        public override void Update(double elapsedTime)
        {
            animation.Update(elapsedTime);
        }
        public override Sprite GetSprite()
        {
            return animation.GetSprite();
        }
        //不同图片在x，y上的偏移量
        //                          0    1   2   3   4   5   6   7  
        int[] offsetx = new int[] { -8, -8, -8, -8, -8, -8, -8, -8 };
        int[] offsety = new int[] { 24, 24, 24, 24, 24, 24, 24, 24 };

        /// <summary>
        /// 获取纹理，并设置坐标（自动完成纹理的对齐）
        /// </summary>
        /// <param name="x">纹理对应的x坐标</param>
        /// <param name="y">纹理对应的y坐标</param>
        /// <param name="reserve">是否横轴反向</param>
        /// <returns>获取适配器纹理</returns>
        public override Sprite GetSpriteWithPos(double x, double y, bool reserve = false)
        {
            int index = animation.CurrentIndex;
            Sprite sp = animation.GetSprite();
            if (reserve)
            {
                sp.SetPosition(x - offsetx[index], y + offsety[index]);
                sp.SetUVs(1, 0, 0, 1);
            }
            else
            {
                sp.SetPosition(x + offsetx[index], y + offsety[index]);
                sp.SetUVs(0, 0, 1, 1);
            }
            sp.SetWidth(128);
            sp.SetHeight(128);
            return sp;
        }
    }

    public class remilia_jump : BattleAnimation
    {
        public remilia_jump(TextureManager texturemanager)
        {
            needloop = false;
            changeinterval = 0.05;
            //站立动画一般都为循环式的
            needloop = true;
            //实例化动画数组
            Sprites = new Sprite[17];
            //设定起始索引，以及最后索引（这里举例为0-7）
            startIndex = 0;
            lastIndex = 14;
            name = "data_character_remilia_jump";
            //调用工具函数快速部署17张纹理至Sprites
            BindTextureTools.BindTexures(texturemanager, name, Sprites, 0, 17);
        }
        /// <summary>
        /// 动画更新
        /// </summary>
        /// <param name="ElapsedTime">帧间时间</param>
        public override void AnimUpdate(double ElapsedTime)
        {
            caculatetime += ElapsedTime;
            //每过一个时间[changeinterval]间隔更新一帧图片
            if (caculatetime >= changeinterval)
            {
                caculatetime -= changeinterval;
                if (CurrentIndex < lastIndex)
                {
                    CurrentIndex++;
                }
                else
                {
                    if (needloop)
                    {
                        CurrentIndex = startIndex;
                    }
                    else
                    {
                        CurrentIndex = lastIndex;
                        IsOver = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取Sprite
        /// </summary>
        /// <returns>返回当前sprite</returns>
        public override Sprite GetSprite()
        {
            Sprite sp = Sprites[CurrentIndex];
            sp.SetWidth(128);
            sp.SetHeight(128);
            return sp;
        }

    }

    public class remilia_jump_Adaptor : Players.AnimationAdaptor
    {
        public remilia_jump_Adaptor(TextureManager texturemanager)
        {
            animation = new remilia_jump(texturemanager);
            animation.needloop = false;
            animation.changeinterval = 0.05f;
        }
        public override void Start()
        {
            //调用Start以后，列表重置
            animation.CurrentIndex = animation.startIndex;
            //结束标志重置
            IsOver = false;
            //动画重置
            animation.IsOver = false;
        }
        public override void Update(double elapsedTime)
        {
            if (IsOver)
                return;
            animation.Update(elapsedTime);
            //如果动画执行结束了，则适配器也执行结束
            if (animation.IsOver)
            {
                this.IsOver = true;
            }
        }
        public override Sprite GetSprite()
        {
            return animation.GetSprite();
        }

        //不同图片在x，y上的偏移量
        //                          0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16
        int[] offsetx = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0 };
        int[] offsety = new int[] { 24, 24, 24, -16, -16, 16, 8, 8, 16, 16, 8, -16, 16, 16, 16, 24-8, 24-8 };

        /// <summary>
        /// 获取纹理，并设置坐标（自动完成纹理的对齐）
        /// </summary>
        /// <param name="x">纹理对应的x坐标</param>
        /// <param name="y">纹理对应的y坐标</param>
        /// <param name="reserve">是否横轴反向</param>
        /// <returns>获取适配器纹理</returns>
        public override Sprite GetSpriteWithPos(double x, double y, bool reserve = false)
        {
            int index = animation.CurrentIndex;
            Sprite sp = animation.GetSprite();
            if (reserve)
            {
                sp.SetPosition(x -offsetx[index], y + offsety[index]);
                sp.SetUVs(1, 0, 0, 1);
            }
            else
            {
                sp.SetPosition(x + offsetx[index], y + offsety[index]);
                sp.SetUVs(0, 0, 1, 1);
            }
            sp.SetWidth(128);
            sp.SetHeight(128);
            return sp;
        }

    }


}
