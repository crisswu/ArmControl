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
        /// 型号  0 = V8 1=学士
        /// </summary>
        public int ModelType { get; set; }
        /// <summary>
        /// 消息类型  0=执行指定舵机角度  1=指定执行函数 
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

        /// <summary>
        /// 执行函数 
        /// 0 = 初始化
        /// </summary>
        [DataMember]
        public int MethodType { get; set; }

    }
}
