using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ArmControl
{
    [DataContract]
    public class ResaultModel
    {
        /// <summary>
        /// 消息代码： 200 正常 100=初始化  500 = python错误  201=自定义内容
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        ///  消息文本
        /// </summary>
        [DataMember]
        public string Msg { get; set; }

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
