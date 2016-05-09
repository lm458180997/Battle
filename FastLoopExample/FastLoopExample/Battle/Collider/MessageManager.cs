using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample.Battle
{
    /// <summary>
    /// 信息管理器
    /// </summary>
    public class MassageManager
    {
        public List<Massage> Buffers;        //信息缓冲区（用于过滤信息并且返回[通过Tag]）
        public List<Massage> Messages;       //信息队列（所有的信息放于其中）
        public MassageManager()
        {
            Messages = new List<Massage>();
            Buffers = new List<Massage>();
        }
        /// <summary>
        /// 获得信息
        /// </summary>
        /// <param name="msg"></param>
        public void AcceptMessage(Massage msg)
        {
            Messages.Add(msg);
        }
        /// <summary>
        /// 清空信息
        /// </summary>
        public void ClearMessage()
        {
            Messages.Clear();
        }
        /// <summary>
        /// 是否信息队列为空
        /// </summary>
        /// <returns>是否信息队列为空</returns>
        public bool IsEmpty()
        {
            return Messages.Count == 0;
        }
        /// <summary>
        /// 根据标签筛选信息
        /// </summary>
        /// <param name="Tag">Tag标签</param>
        /// <returns>返回筛选出的信息</returns>
        public Massage[] SelectMessages(int Tag, bool deleteDatas = true)
        {
            foreach (Massage msg in Messages)
            {
                Buffers.Add(msg);
            }
            int count = Buffers.Count;
            if (count < 1)
                return null;
            Massage[] msgs = new Massage[count];
            for (int i = 0; i < count; i++)
            {
                msgs[i] = Buffers[i];
                //如果需要删除读取过的信息，则从缓冲区删除[当信息种类很多的时候能有效提高效率]
                if (deleteDatas)
                {
                    Messages.Remove(Buffers[i]);
                }
            }
            Buffers.Clear();
            return msgs;
        }

    }

}
