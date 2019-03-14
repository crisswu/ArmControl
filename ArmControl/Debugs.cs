using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ArmControl
{
    public partial class Debugs : Form
    {
        public Debugs()
        {
            InitializeComponent();
        }

       

        private void Debugs_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 封装模型为JSON格式 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string getJson(Capm c)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Capm));
            System.IO.MemoryStream Mes = new MemoryStream();
            serializer.WriteObject(Mes, c);
            System.IO.StreamReader reader = new StreamReader(Mes);
            Mes.Position = 0;
            string strRes = reader.ReadToEnd();
            reader.Close();
            Mes.Close();
            return strRes;
        }

        private void btnDJ1_Click(object sender, EventArgs e)
        {
            Capm c = new Capm();
            c.MsgType = 1;
            c.Ids = new int[] { 5 };
            c.Vals = new int[] { Convert.ToInt32(txtDJ1.Value) };

            string data =  TcpManager.Client(txtIP.Text, 12345, getJson(c));
            txtMessage.Text += data + "\n";
           
        }
    }
}
