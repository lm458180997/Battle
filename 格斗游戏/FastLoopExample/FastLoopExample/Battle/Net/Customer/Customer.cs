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

namespace FastLoopExample.Battle.Net
{

    public class Customer
    {
        
        Server.PlayerDataManager clientPeer;

        public void Run()
        {
            //实例
            clientPeer = new Server.PlayerDataManager();

            //注册文件传输协议
            clientPeer.AddHandler("FileCallResult", OnCall);
            //上传文件结果返回
            clientPeer.AddHandler("FileSaveResult", OnSave);

            clientPeer.Start("172.21.8.74", 21555);

        }

        static string result="";

        public void SendData(float x, float y, byte pl_st,byte ap_st,byte cu_index,byte tt_inedx)
        {
            Link.Net.Message pu = new Link.Net.Message();
            pu.player_x1 = x;
            pu.player_x2 = y;
            pu.player_state = pl_st;
            pu.adaptor_state = ap_st;
            pu.currentindex = cu_index;
            pu.totoleindex = tt_inedx;

            //包体封装
            NetPacket p = new NetPacket();
            p.BeginWrite("C_player_Data_Send");                        //展开文件储存协议
            p.WriteObject<Link.Net.Message>(pu);
            p.EncodeHeader();

            if (clientPeer.Send(p)){ }
            else
               result = "发送失败，服务器断开";
        }


        //处理存储
        public void OnCall(NetPacket packet)
        {
            result = "Success";

            //从返回结果中获取任务ID
            //Link.Chat.CallResult rs = packet.ReadObject<Link.Chat.CallResult>();
            //包体封装
            NetPacket p = new NetPacket(512);
            p.BeginWrite("C_Player_Data");   //客户端数据传输

            //p.WriteObject<Link.Chat.FileProto>(proto);
            //p.packageId = id;
            //p.EncodeHeader();
            if (clientPeer.Send(p))
            {

            }
            else
                result = "发送失败，服务器断开";
        }

        public void OnSave(NetPacket packet)
        {
            result = "Save Success";
        }
    }

}
