using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;


namespace ArmControl
{
    public class TcpManager
    {
        /// <summary>
        /// 客户端发送Client("192.168.1.100", 10086, "mzwu.com");发送数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public static string Client(string ip, int port, string message)
        {
            try
            {
                //1.发送数据                
                TcpClient client = new TcpClient(ip, port);
                IPEndPoint ipendpoint = client.Client.RemoteEndPoint as IPEndPoint;
                NetworkStream stream = client.GetStream();
                byte[] messages = System.Text.Encoding.Default.GetBytes(message); //HexManage.strToToHexByte(message);//字符串转换成十六进制数组
                stream.Write(messages, 0, messages.Length);
                Console.WriteLine("{0:HH:mm:ss}->发送数据(to {1})：{2}:{3}", DateTime.Now, ip, port, message);

                //2.接收状态,长度<1024字节
                byte[] bytes = new Byte[10240];
                string data = string.Empty;
                int length = stream.Read(bytes, 0, bytes.Length);
                if (length > 0)
                {
                    byte[] newA = bytes.ToArray();//获取返回协议
                     // Console.WriteLine(BitConverter.ToString(newA).Replace("-", ""));
                    data = System.Text.Encoding.Default.GetString(newA); //HexManage.byteToHexStr(newA);//把返回的十六进制数组转换为字符串
                }

                //3.关闭对象
                stream.Close();
                client.Close();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0:HH:mm:ss}->{1}", DateTime.Now, ex.Message);
                return ex.Message;
            }
        }

    }
}
