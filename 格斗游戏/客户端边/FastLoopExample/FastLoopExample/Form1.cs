using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;                          //添加后可使用OpenGL库
using Tao.DevIl;                           //添加后可使用资源
using Tao.Glfw;                       //另一种显示图形的方法
using Un4seen.Bass;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;                  //使用bass音频库

namespace FastLoopExample
{

    public partial class Form1 : Form
    {
       
        public const uint LMEM_ZEROINIT = 0x0040;

        //游戏状态
        enum GameState                 //每一个游戏状态实现IGameObeject接口
        {
            CompanySplash,
            TitleMenu,
            PlayingGame,
            SettingsMenu
        }

        bool _fullscreen = false;                          //是否全屏 

        FastLoop _fastloop;                                //循环事件（高效刷新）

        Random rand = new Random();                        //随机数生成器

        public static StateSystem _system = new StateSystem();    //状态机

        TextureManager _textureManager = new TextureManager();    //纹理管理器

        SoundManagerEx _soundManagerEx = new SoundManagerEx();    //功能完善版声音管理器

        Dictionary<string, Pac_boolean> keys = new Dictionary<string, Pac_boolean>(); //记录一些键位是否按下

        public FileManager filemanager ;      //本地资源管理器[1.0]

        public static List<WebEngion.NetworkManager> BackThreads = new List<WebEngion.NetworkManager>();
        
        public Form1()
        {
            InitializeComponent();
            //初始化bass库
            BassNet.Registration("leileimin@qq.com", "2X52314160022");
            this.Text = "Stay Night" ;
            _fastloop = new FastLoop(GameLoop);            //绑定循环
            _openGLControl.Dock = DockStyle.Fill;          //保持为父容器填充
            _openGLControl.InitializeContexts();           //OpenGL初始化（比c++方便了不少）
            filemanager = new FileManager();               //实例化资源管理器
            LoadingSetting();                              //读取用户设置
            InitializeDisplay();
            //读取资源
            InitializeSounds();
            InitializeTextures();
            //角色初始化
            InitializeGameCharactors();
            //游戏场景
            InitializeGameState();
                       Gl.glDepthMask(Gl.GL_TRUE);  //设置深度缓冲为可读可写
                      //glDepthMask(GL_FALSE);可将深度缓冲区设置为只读形式
            //键盘控制初始
            InitializeKeyManager();

            Gl.glEnable(Gl.GL_TEXTURE_2D);          //打开2D纹理着色
            _openGLControl.KeyDown += _openGLControl_KeyDown;
            _openGLControl.KeyUp += _openGLControl_KeyUp;
            _openGLControl.AutoFinish = true;             //自动finish
            Gl.glRenderMode(Gl.GL_DOUBLE | Gl.GL_RGB); //双缓冲
            _openGLControl.AutoSwapBuffers = true;
            _system.ChangeState("BattleStage1");    //切换进游戏场景

            this.FormClosing += CloseForm;
            
        }
        

        //需要把alut.dll, ILU.dll,OpenAL32.dll 复制到debug和release目录里
        void InitializeDisplay()
        {
            this.ClientSizeChanged += (s, e) =>
            {
                int dw = (int)(this.ClientSize.Height * (4 / 3.0f));
                Gl.glViewport((this.ClientSize.Width - dw) / 2, 0, dw, this.ClientSize.Height);
                //使OpenGL窗体与winform保持一致,坐标原点在左下角
                Setup2DGraphics(640, 480,false);           //游戏场景的大小
            };                                        // 游戏是640*480
            this.ClientSize = new Size(640, 480);     //800 600(4:3)  1280 720(16:9)
            //资源初始化
            Il.ilInit();
            Ilu.iluInit();
            Ilut.ilutInit();
            Ilut.ilutRenderer(Ilut.ILUT_OPENGL);

        }
        void ReadMusicFile(string path)
        {
            filemanager.ClearFiles();
            filemanager.ReadFileClip(path);
            string name = "";
            if (filemanager.FilesDatas != null)
                foreach (FileData fdt in filemanager.FilesDatas)
                {
                    name = fdt.Name;
                    name = name.Substring(0, name.Length - 4);
                    unsafe
                    {
                        byte[] test = fdt.filedata;
                        GCHandle hObject = GCHandle.Alloc(test, GCHandleType.Pinned);
                        IntPtr pObject = hObject.AddrOfPinnedObject();
                        _soundManagerEx.AddSound(name, pObject, test.LongLength);
                    }
                }
            filemanager.ClearFiles();
        }
        void InitializeSounds()
        {
            
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_CPSPEAKERS, this.Handle))
            {
                MessageBox.Show("Bass 初始化出错！" + Bass.BASS_ErrorGetCode().ToString());
            }
            Datas.SoundManager_Static = _soundManagerEx;                 //将其保存至共享资源内
            #region oldloadmusic
            // _soundManager.LoadSound("mymusic1", "BadApple.wav");
           // _soundManagerEx.AddSound("mymusic1", "BadApple.wav");
           // _soundManagerEx.AddSound("rj", "res\\溶解.mp3");
           // _soundManagerEx.AddSound("Accept Bloody Fate", "res\\Audio\\BGM\\Accept Bloody Fate.mp3");
            //_soundManagerEx.AddSound("Graze","res\\Audio\\SE\\se_graze.wav");
            #endregion
           // ReadMusicFile("msc.tg");

            #region ME

            //unsafe
            //{
            //    byte[] test = FastLoopExample.Properties.Resources.竹取飞翔_静_;
            //    GCHandle hObject = GCHandle.Alloc(test, GCHandleType.Pinned);
            //    IntPtr pObject = hObject.AddrOfPinnedObject();
            //    _soundManagerEx.AddSound("victor01", pObject, test.LongLength);
            //}
            
            #endregion

            #region SE
            //_soundManagerEx.AddSound("002-System02.ogg", "res\\Audio\\SE\\002-System02.ogg");
            #endregion
        }

        #region 纹理初始化（读取资源文件）

        void ReadTextureFile(string path)
        {
            filemanager.ClearFiles();
            filemanager.ReadFileClip(path);
            string name = "";
            string extens = "";
            if(filemanager.FilesDatas!=null)
            foreach (FileData fdt in filemanager.FilesDatas)
            {
                name = fdt.Name;
                extens = Path.GetExtension(name);
                name = Path.GetFileNameWithoutExtension(name);
                switch (extens)
                {
                    case ".png":
                        _textureManager.LoadTexture(name, Il.IL_PNG, fdt.filedata, fdt.bodyLength + 1);
                        break;
                    case ".jpg":
                        _textureManager.LoadTexture(name, Il.IL_JPG, fdt.filedata, fdt.bodyLength + 1);
                        break;
                    case ".bmp":
                        _textureManager.LoadTexture(name, Il.IL_BMP, fdt.filedata, fdt.bodyLength + 1);
                        break;
                }
            }
            filemanager.ClearFiles();
        }

        //初始化纹理
        void InitializeTextures()
        {
            ReadTextureFile("res\\Remilia\\data_character_remilia_stand.tg");
            ReadTextureFile("res\\Remilia\\data_character_remilia_walkFront.tg");
            ReadTextureFile("res\\Remilia\\data_character_remilia_jump.tg");
            _textureManager.LoadTexture("back", "back.jpg");
            // _textureManager.LoadTexture("Name", "Path");
            // Texture texture = _textureManager.Get("Name");
            // Sprite sp = new Sprite();
            // sp.SetPosition(0, 0); sp.SetWidth(32);
            // sp.Texture = texture;
            // Renderer render = new Renderer();
            // render.DrawSprite(sprite);
        }

        #endregion

        //初始化游戏角色。（这里是预留一个函数开辟一个角色缓冲区，用于提高性能[暂时不用考虑]）
        void InitializeGameCharactors()
        {
            
        }

        //初始化游戏状态（生成一个当前格斗游戏的场景[现只有一个]）
        void InitializeGameState()
        {
            _system.AddState("BattleStage1",new Battle.BattleStage1(_system,_textureManager,_soundManagerEx));//BattleStage1注册
        }

        //初始化键盘管理系统（实现Input相关的内容）
        void InitializeKeyManager()
        {
            for (int i = 0; i < 26; i++)
            {
                char c = (char)('A' + i);
                keys.Add(c.ToString(), new Pac_boolean(false));
            }
            keys.Add("Left", new Pac_boolean(false));
            keys.Add("Right", new Pac_boolean(false));
            keys.Add("Up", new Pac_boolean(false));
            keys.Add("Down", new Pac_boolean(false));
            keys.Add("Escape", new Pac_boolean(false));
            keys.Add("Space", new Pac_boolean(false));
            keys.Add("Leftshift", new Pac_boolean(false));
            keys.Add("Rightshift", new Pac_boolean(false));
            keys.Add("Shift", new Pac_boolean(false));

            _openGLControl.PreviewKeyDown += keydown;       
        }

        //读取进入游戏时候的设置信息（声音大小，标题等内容）
        void LoadingSetting()
        {
            if (File.Exists("opst.dat"))
            {
                System.IO.StreamReader reader = new StreamReader("opst.dat");
                while (!reader.EndOfStream)
                {
                    string data = reader.ReadLine();
                    string[] arr = data.Split('$');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = arr[i].Replace("$", "");
                        char[] carr = arr[i].ToCharArray();
                        for (int j = 0; j < carr.Length; j++)
                        {
                            if (carr[j] == '#')
                            {
                                string name = arr[i].Substring(0, j);
                                if (name == "volume")
                                {
                                    string value = arr[i].Substring(j + 1, carr.Length - j - 1);
                                    int v = Convert.ToInt32(value);
                                    if (0 <= v && v < 10)
                                    {
                                        _soundManagerEx.Volume = 0;
                                    }
                                    else if (v <= 100)
                                    {
                                        _soundManagerEx.Volume = v+1;        //这里需要+1 才能获取正确的结果
                                    }
                                }
                                if (name == "title")
                                {
                                    string value = arr[i].Substring(j + 1, carr.Length - j - 1);
                                    this.Text = "憧萤ヒマワリ";
                                }
                                if (name == "Replay")
                                {
                                    string value = arr[i].Substring(j + 1, carr.Length - j - 1);
                                    int v = Convert.ToInt32(value);
                                    if (v == 1)
                                    {
                                        Datas.ReFre = true;
                                    }
                                }
                                if (name == "adress")
                                {
                                    string value = arr[i].Substring(j + 1, carr.Length - j - 1);
                                    value.Trim();
                                    if(value !="")
                                    Datas.Adress = value;
                                }
                                break;
                            }
                        }
                    }

                }

                reader.Dispose();
            }
        }

        //关闭窗体的时候需要关闭后台线程
        void CloseForm(object sender, EventArgs e)
        {
            foreach (WebEngion.NetworkManager t in BackThreads)
            {
                t.runUpdate = false;
            }
        }
        
        //-----------------------------------游戏系统的主要循环函数--------------------------------------
        #region  实现帧循环（GameLoop）所需要使用上的数字变量
        double timecaculate = 0;
        const double fps = 120;
        const double per_fps = 0.01666666;
        long refreshtick = 0;
        int tick = 0;
        int defaultdivide = 5;
        #endregion
        public void GameLoop(double elapsedTime)
        {
            if (elapsedTime > 0.02)
                return;
            #region  normalloop
            _system.Update(elapsedTime);
            System.Diagnostics.Debug.Print(elapsedTime.ToString());
            timecaculate += elapsedTime;
            tick++;
            if (tick == defaultdivide)
            {
                _system.Render();
                _openGLControl.Refresh();
                tick = 0;
                if (timecaculate <= 0.2)
                {
                    refreshtick++;
                }
                else
                {
                    double fps = refreshtick / timecaculate;
                    //System.Diagnostics.Debug.Print("fps: " + fps.ToString() + "   defaultdivide: " + defaultdivide.ToString());
                    refreshtick = 0;
                    timecaculate = 0;
                    if (fps > 120)
                    {
                        defaultdivide++;
                    }
                    else
                    {
                        defaultdivide--;
                        if (defaultdivide < 1)
                            defaultdivide = 1;
                    }
                }
            }
            #endregion

            #region  固定帧率算法，有行走锯齿缺陷
            //timc += elapsedTime;
            //timecaculate += elapsedTime;
            //System.Diagnostics.Debug.Print(elapsedTime.ToString());
            //if (timecaculate > per_fps)
            //{
            //    workingcount++;
            //    double sc = timecaculate - per_fps;
            //    timecaculate -= per_fps;
            //    _system.Render();
            //    _openGLControl.Refresh();
            //    if (sc > 0.0001)
            //    {
            //        errorcount++;
            //        timecaculate = 0;
            //    }
            //    else
            //    {
            //        _system.Update(per_fps);
            //    }
            //    this.Text = sc.ToString("f9");
            //    _soundManagerEx.Update();        //声音控制器逻辑更新
                //if (timc <= 0.1)
                //    refreshtick++;
                //else
                //{
                //    refreshtick++;
                //    double fpps = refreshtick / timc;
                //    //System.Diagnostics.Debug.Print("count: " + refreshtick.ToString() + "time:" + timc.ToString()
                //    //+ "fps:" + fpps.ToString());
                //    fpps = (double)errorcount / workingcount;
                //    //System.Diagnostics.Debug.Print("处理落率：" + fpps.ToString());
                //    fpps = 0;
                //    timc = 0;
                //    refreshtick = 0;
                //}
           // }
            #endregion
        }
        //-----------------------------------游戏系统的主要循环函数--------------------------------------


        //这里是将坐标原点位于中心。  如果要使原点位于左上角，可以使用  Gl.glOrtho(0,width,-height,0,-100,100)
        private void Setup2DGraphics(double width, double height, bool iscenter = true)   //
        {
            double halfWidth = width / 2;
            double halfHeight = height / 2;
            Gl.glMatrixMode(Gl.GL_PROJECTION);  //GL_PROJECTION会修改OpenGL状态
            Gl.glLoadIdentity();                //glLoadIdentity会清除当前的投影信息
            if (iscenter)
                Gl.glOrtho(-halfWidth, halfWidth, -halfHeight, halfHeight, -100, 100);
            else
            {
                Gl.glOrtho(-halfWidth, halfWidth, -halfHeight + 100, halfHeight + 100, -100, 100);
            }
            //建立了正射投影矩阵,最后两个值是近平面和远平面
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        public bool FullScreen                 //全屏属性
        {
            get { return _fullscreen; }
            set
            {
                _fullscreen = value;
                if (value == true)
                {
                    FormBorderStyle = FormBorderStyle.None;       //无框
                    WindowState = FormWindowState.Maximized;      //最大化
                }
            }
        }

        //OnLoad[无]
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region   键盘命令的相关内容[实现Input.GetKeyDown("Left")等的使用功能]

        //OpenGLControler所绑定的函数，用于实现Input功能
        void keydown(object o, PreviewKeyDownEventArgs args)
        {
            _openGLControl_KeyDown(o, new KeyEventArgs(args.KeyData));     //将所有数据都转交给_openGLControl_KeyDown处理
        }


        void _openGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                string temp = e.KeyCode.ToString();
                if (keys[temp].value == false)              //escape是否已经按下过
                {
                    Input.SetKeyUp(temp, false);
                    Input.SetKeyDown(temp, true);
                    keys[temp].value = true;                //已经按下了
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode==Keys.Down)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == false)             
                {
                    Input.SetKeyUp(str, false);
                    Input.SetKeyDown(str, true);
                    keys[str].value = true;                //已经按下了
                }
            }
            Keys ks = Keys.A;
            for (; ks <= Keys.Z; ks++)
                if (e.KeyCode == ks)
                {
                    string str = ks.ToString();
                    if (keys[str].value == false)             
                    {
                        Input.SetKeyUp(str, false);
                        Input.SetKeyDown(str, true);
                        keys[str].value = true;                //已经按下了
                    }
                }

            if (e.KeyCode == Keys.ShiftKey)
            {
                if (keys["Shift"].value == false)              //escape是否已经按下过
                {
                    Input.SetKeyUp("Shift", false);
                    Input.SetKeyDown("Shift", true);
                    keys["Shift"].value = true;                //已经按下了
                }
            }

        }

        void _openGLControl_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == true)               //escape是否已经按下过
                {
                    Input.SetKeyUp(str, true);
                    Input.SetKeyDown(str, false);
                    keys[str].value = false;               //没有按下
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == true)              //escape是否已经按下过
                {
                    Input.SetKeyUp(str, true);
                    Input.SetKeyDown(str, false);
                    keys[str].value = false;               //没有按下
                }
            }
            //判断A——Z
            Keys ks = Keys.A;
            for (; ks <= Keys.Z; ks++)
                if (e.KeyCode == ks)
                {
                    string str = ks.ToString();
                    if (keys[str].value == true)             
                    {
                        Input.SetKeyUp(str, true);
                        Input.SetKeyDown(str, false);
                        keys[str].value = false;               //没有按下
                    }
                }
            if (e.KeyCode == Keys.ShiftKey)
            {
                if (keys["Shift"].value == true)
                {
                    Input.SetKeyUp("Shift", true);
                    Input.SetKeyDown("Shift", false);
                    keys["Shift"].value = false;               //没有按下
                }
            }
        }

        //重写这个函数使得方向键可以使用 （像Tab , Return , Esc 和箭头键这些键原本是控件自身要使用的，可以自己添加来覆盖功能）
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right
                || keyData == Keys.Up || keyData == Keys.Down)
            {
                return false;
            }
            else
                return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                string temp = e.KeyCode.ToString();
                if (keys[temp].value == false)              //escape是否已经按下过
                {
                    Input.SetKeyUp(temp, false);
                    Input.SetKeyDown(temp, true);
                    keys[temp].value = true;                //已经按下了
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == false)              //escape是否已经按下过
                {
                    Input.SetKeyUp(str, false);
                    Input.SetKeyDown(str, true);
                    keys[str].value = true;                //已经按下了
                }
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                if (keys["Shift"].value == false)              //escape是否已经按下过
                {
                    Input.SetKeyUp("Shift", false);
                    Input.SetKeyDown("Shift", true);
                    keys["Shift"].value = true;                //已经按下了
                }
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == true)               //escape是否已经按下过
                {
                    Input.SetKeyUp(str, true);
                    Input.SetKeyDown(str, false);
                    keys[str].value = false;               //没有按下
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                string str = e.KeyCode.ToString();
                if (keys[str].value == true)              //escape是否已经按下过
                {
                    Input.SetKeyUp(str, true);
                    Input.SetKeyDown(str, false);
                    keys[str].value = false;               //没有按下
                }
            }

            //判断A——Z
            Keys ks = Keys.A;
            for (; ks <= Keys.Z;ks++ )
                if (e.KeyCode == ks)
                {
                    string str = ks.ToString();
                    if (keys[str].value == true)              //escape是否已经按下过
                    {
                        Input.SetKeyUp(str, true);
                        Input.SetKeyDown(str, false);
                        keys[str].value = false;               //没有按下
                    }
                }

        }

        #endregion

        /* - - - - - - - - - - - - - - - - - - - - - - - -  
 * Stream 和 byte[] 之间的转换 
 * - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        /* - - - - - - - - - - - - - - - - - - - - - - - -  
         * Stream 和 文件之间的转换 
         * - - - - - - - - - - - - - - - - - - - - - - - */
        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary> 
        /// 从文件读取 Stream 
        /// </summary> 
        public Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        } 
    }

}