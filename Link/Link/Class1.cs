using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Link.Net
{
    [Serializable]
    public class Message
    {
        public byte adaptor_state;
        public byte currentindex;
        public byte player_state;
        public float player_x1;
        public float player_x2;
        public byte totoleindex;
        public int speed;
        public float caculatetime;
    }
}
