using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArmControl
{
    [DataContract]
    public class Capm
    {
        /// <summary>
        /// 消息类型  0=修改指定舵机ID角度  1=指定执行函数 
        /// </summary>
        [DataMember]
        public int MsgType { get; set; }
        /// <summary>
        ///  舵机ID
        /// </summary>
        [DataMember]
        public int[] Ids { get; set; }
        /// <summary>
        /// 舵机角度
        /// </summary>
        [DataMember]
        public int[] Vals { get; set; }

    }
}
