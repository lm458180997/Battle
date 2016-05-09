using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle.Stages.Menu
{
    /// <summary>
    /// Menu下的背景的所有有关内容
    /// </summary>
    public class BackGround
    {
        TextureManager texturemanager;
        Renderer render = new Renderer();       //着色器
        Sprite weather;            //weather层的纹理
        Vector2D weatherPosition = new Vector2D();
        Sprite bk1;                //1层纹理
        Vector2D bk1Position = new Vector2D();
        float bk1width = 512;
        float bk1_offset_speed = 3;
        Sprite bk2;                //2层纹理
        Vector2D bk2Position = new Vector2D();
        float bk2width = 640;
        float bk2_offset_speed = 7;
        Sprite bk3;                //3层纹理
        Vector2D bk3Position = new Vector2D();
        float bk3width = 768;
        float bk3_offset_speed = 14;
        Sprite robot;              //机器人（人型影子）
        Vector2D robotPosition = new Vector2D();
        double robotangle = 0;          //图像旋转角
        float robot_rotatespeed = 3f;   //每秒旋转1.5度
        float robot_angle_MAX = 3;    //最大偏移角度
        bool robot_toleft = true;      //是否向左边偏转（否则向右）
        double robot_offsetY_max = 5;  //最大高度偏移（产生走路时上移的效果）
        

        public BackGround(TextureManager mng)
        {
            texturemanager = mng;
            initSprites();
        }

        public void Start()
        {
        }
        public void initSprites()
        {
            weather = new Sprite();
            bk1 = new Sprite();
            bk2 = new Sprite();
            bk3 = new Sprite();
            robot = new Sprite();
            weather.Texture = texturemanager.Get("15_back");
            weather.SetWidth(1024); weather.SetHeight(1024);
            bk1.Texture = texturemanager.Get("14_back");
            bk1.SetWidth(512); bk1.SetHeight(512);
            bk1.SetUVs(0, 0.01f, 1, 1);
            bk2.Texture = texturemanager.Get("12_back");
            bk2.SetWidth(1024); bk2.SetHeight(1024);
            bk2.SetUVs(0, 0.01f, 1, 1);
            bk3.Texture = texturemanager.Get("11_back");
            bk3.SetWidth(1024); bk3.SetHeight(1024);
            bk3.SetUVs(0, 0.01f, 1, 1);        //去除素材上的黑线[并没有特殊意义，单纯的素材优化]
            robot.Texture = texturemanager.Get("13_back");
            robot.SetWidth(128); robot.SetHeight(128);

            weatherPosition.Y = 100;
            bk1Position.Y = -100;
            bk1Position.X = 100;
            bk2Position.X = 100;
            bk3Position.X = 100;
            bk2Position.Y = -100;
            bk3Position.Y = -60;
            robotPosition.Y = -90;
            robotPosition.X = 170;

        }
        /// <summary>
        /// 逻辑更新
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(double elapsedTime)
        {
            bk1Position.X += elapsedTime * bk1_offset_speed;
            bk2Position.X += elapsedTime * bk2_offset_speed;
            bk3Position.X += elapsedTime * bk3_offset_speed;

            if (bk1Position.X > bk1width)
                bk1Position.X -= bk1width;
            if (bk2Position.X > bk2width)
                bk2Position.X -= bk2width;
            if (bk3Position.X > bk3width)
                bk3Position.X -= bk3width;

            if (robot_toleft)
            {
                robotangle -= elapsedTime * robot_rotatespeed;
                if (robotangle < -robot_angle_MAX)
                {
                    robotangle = -robot_angle_MAX;
                    robot_toleft = false;
                }
            }
            else
            {
                robotangle += elapsedTime * robot_rotatespeed;
                if (robotangle > robot_angle_MAX)
                {
                    robotangle = robot_angle_MAX;
                    robot_toleft = true;
                }
            }


        }
        /// <summary>
        /// 通过这个函数使得每个图层都是连续的（左右各重画一次）
        /// </summary>
        /// <param name="width">图层原宽度</param>
        /// <param name="pos">图层的原坐标</param>
        /// <param name="sp">图层对应的纹理精灵</param>
        public void BackRender(float width, Vector2D pos, Sprite sp)
        {
            sp.SetPosition(pos.X, pos.Y);
            render.DrawSprite(sp);
            sp.SetPosition(pos.X - width, pos.Y);
            render.DrawSprite(sp);
            sp.SetPosition(pos.X + width, pos.Y);
            render.DrawSprite(sp);
        }
        /// <summary>
        /// 渲染
        /// </summary>
        public void Render()
        {
            weather.SetPosition(weatherPosition.X, weatherPosition.Y);
            render.DrawSprite(weather);
            BackRender(bk1width, bk1Position, bk1);
            //机器人会旋转（产生走动的效果）
            double offsety = 0;
            if (robotangle < 0)
            {
                double percent = (-robotangle)/robot_angle_MAX ;
                percent = 1-percent;
                percent = percent * percent;
                offsety = percent * robot_offsetY_max;
            }
            else
            {
                double percent = robotangle / robot_angle_MAX;
                percent = 1 - percent;
                percent = percent * percent;
                offsety = percent * robot_offsetY_max;
            }

            robot.SetPosition(robotPosition.X, robotPosition.Y+offsety);
            render.DrawSprite(robot,robotPosition.X ,robotPosition.Y+offsety,(float)robotangle);
            BackRender(bk2width, bk2Position, bk2);
            BackRender(bk3width, bk3Position, bk3);
        }

    }
}
