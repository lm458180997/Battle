using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebEngion;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

namespace FastLoopExample.Battle.Net
{
    public class ServerTool
    {
        ServerManager server;

        public static bool HaveNewData = false;                //是否取得了数据
        public static PlayerData data = new PlayerData();  //人物数据

        public void Run()
        {
            server = new ServerManager();
            //打开服务器
            server.StartServer(Datas.Adress, 21555);
            //在Form1中记录下当前的进程，用于退出时自动中断网络线程
            Form1.BackThreads.Add(server); 
        }

    }

    /// <summary>
    /// 每个连接的Socket的具体内容
    /// </summary>
    public class PlayerData
    {
        public  float player_x;
        public  float player_y;
        public  byte playerstate;
        public  byte adaptorstate;
        public  byte currentindex;
        public  byte totleindex;
        public int speed;
        public float caculatetime;
        public void SetData(float x, float y, byte pl_st, byte adpst, byte curindex, byte ttindex
            ,int sp , float cal)
        {
            player_x = x;
            player_y = y;
            playerstate = pl_st;
            adaptorstate = adpst;
            currentindex = curindex;
            totleindex = ttindex;
            speed = sp;
            caculatetime = cal;
        }
    }


    public class ServerManager : NetworkManager
    {
        //保存所有的客户端连接
        List<Socket> peerList;
        //清除无效的socket列表
        List<Socket> peerList_ToRemove = new List<Socket>();
        //服务器
        TCPPeer server;

        public ServerManager()
        {
            //创建一个列表保存每个客户端的Socket
            peerList = new List<Socket>();
        }

        //启动服务器
        public void StartServer(string ip, int port)
        {
            //注册事件，此处只有一个聊天消息
            AddHandler("C_Player_Data", DataRecive);           // send

            server = new TCPPeer(this);
            server.Listen(ip, port);
            //启动另一个线程处理消息队列
            this.StartThreadUpdate();
        }

        //处理服务器接受客户端的连接
        public override void OnAccepted(NetPacket packet)
        {
            Debug.Print("接受新的连接");
            peerList.Add(packet.socket);
        }
        //处理丢失连接
        public override void OnLost(NetPacket packet)
        {
            Debug.Print("丢失连接");
            peerList.Remove(packet.socket);
        }
        //处理存储消息
        public void DataRecive(NetPacket packet)
        {
            Link.Net.Message proto = packet.ReadObject<Link.Net.Message>();
            if (proto != null)
            {
                ServerTool.HaveNewData = true;
                ServerTool.data.SetData(proto.player_x1,
                    proto.player_x2,
                    proto.player_state,
                    proto.adaptor_state,
                    proto.currentindex,
                    proto.totoleindex,
                    proto.speed,
                    proto.caculatetime);
            }

            foreach (Socket sk in peerList)
            {
                if (!sk.Connected)
                    peerList_ToRemove.Add(sk);
            }

            //删除无效连接
            foreach (Socket sk in peerList_ToRemove)
            {
                peerList.Remove(sk);
            }
            peerList_ToRemove.Clear();

            //发送一个返回信息
            NetPacket npt = new NetPacket();
            npt.packageId = 0;
            npt.BeginWrite("S_Player_Data");
            Link.Net.Message rs = new Link.Net.Message();
            npt.WriteObject<Link.Net.Message>(rs);
            npt.EncodeHeader();
            if (packet.socket.Connected)
            {
                server.Send(packet.socket, npt);
            }


        }

        public void OnCall(NetPacket packet)
        {
            Link.Net.Message proto = packet.ReadObject<Link.Net.Message>();
        }
    }

}
