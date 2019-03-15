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

        DataTable dt;

        private void Debugs_Load(object sender, EventArgs e)
        {
            cboMethod.SelectedIndex = 0;
            grd.AutoGenerateColumns = false;
            cboModel.SelectedIndex = 0;

            dt = new DataTable();
            dt.Columns.Add("Name", Type.GetType("System.String"));
            dt.Columns.Add("col1",Type.GetType("System.Int32"));
            dt.Columns.Add("col2", Type.GetType("System.Int32"));
            dt.Columns.Add("col3", Type.GetType("System.Int32"));
            dt.Columns.Add("col4", Type.GetType("System.Int32"));
            dt.Columns.Add("col5", Type.GetType("System.Int32"));
            dt.Columns.Add("col6", Type.GetType("System.Int32"));
            dt.Columns.Add("btnExecute", Type.GetType("System.String"));
            dt.Columns.Add("btnDel", Type.GetType("System.String"));

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
        /// <summary>
        /// 根据机号位 返回 角度值
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetValuesByNum(int num)
        {
            switch (num)
            {
                case 1:
                    return Convert.ToInt32(txtDJ1.Value);
                case 2:
                    return Convert.ToInt32(txtDJ2.Value);
                case 3:
                    return Convert.ToInt32(txtDJ3.Value);
                case 4:
                    return Convert.ToInt32(txtDJ4.Value);
                case 5:
                    return Convert.ToInt32(txtDJ5.Value);
                default:
                    return Convert.ToInt32(txtDJ6.Value);
            }
        }
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDJ1_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(((Button)sender).Tag);//机号位

            Capm c = new Capm();
            c.ModelType = cboModel.SelectedIndex;//型号
            c.MsgType = 0;//消息类型
            c.Ids = new int[] { num };
            c.Vals = new int[] { GetValuesByNum(num) };

            string data =  TcpManager.Client(txtIP.Text, 12345, getJson(c));
            ExecuteResponse(data);
        }
        /// <summary>
        /// 添加动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();
            dr["Name"] = txtName.Text;
            dr["col1"] = txtDJ1.Value;
            dr["col2"] = txtDJ2.Value;
            dr["col3"] = txtDJ3.Value;
            dr["col4"] = txtDJ4.Value;
            dr["col5"] = txtDJ5.Value;
            dr["col6"] = txtDJ6.Value;
            dr["btnExecute"] = "执行";
            dr["btnDel"] = "删除";
            dt.Rows.Add(dr);
            grd.DataSource = dt;
            txtName.Text = "";
        }

        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <param name="data"></param>
        public void ExecuteResponse(string data)
        {
            try
            {
                data = data.Replace("\0", "");
                ResaultModel rm = JsonDeserialize(data);
                if (rm.Code == "200")
                {
                    txtMessage.Text += "执行完成!\n";
                }
                else if (rm.Code == "100")//初始化
                {
                    for (int i = 0; i < rm.Ids.Length; i++)
                    {
                        if (rm.Ids[i] == 1)
                            txtDJ1.Value = rm.Vals[i];
                        if (rm.Ids[i] == 2)
                            txtDJ2.Value = rm.Vals[i];
                        if (rm.Ids[i] == 3)
                            txtDJ3.Value = rm.Vals[i];
                        if (rm.Ids[i] == 4)
                            txtDJ4.Value = rm.Vals[i];
                        if (rm.Ids[i] == 5)
                            txtDJ5.Value = rm.Vals[i];
                        if (rm.Ids[i] == 6)
                            txtDJ6.Value = rm.Vals[i];
                    }
                }
                else if (rm.Code == "201")// 自定义内容
                {
                    txtMessage.Text += rm.Msg + "\n";
                }
                else if (rm.Code == "500")// python错误信息
                {
                    txtMessage.Text += rm.Msg + "\n";
                }
                else
                {
                    txtMessage.Text += "返回结果出现异常\n";
                }
            }
            catch (Exception ep)
            {
                txtMessage.Text += ep.Message + "\n";
            }
            
        }
        //JSON反序列化
        public static ResaultModel JsonDeserialize(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ResaultModel));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            ResaultModel obj = (ResaultModel)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                int i = e.RowIndex;
                //执行
                Capm c = new Capm();
                c.ModelType = cboModel.SelectedIndex;//型号
                c.MsgType = 0;//消息类型
                c.Ids = new int[] { 1,2,3,4,5,6};

                c.Vals = new int[] { Convert.ToInt32(dt.Rows[i]["col1"]),
                                     Convert.ToInt32(dt.Rows[i]["col2"]),
                                     Convert.ToInt32(dt.Rows[i]["col3"]),
                                     Convert.ToInt32(dt.Rows[i]["col4"]),
                                     Convert.ToInt32(dt.Rows[i]["col5"]),
                                     Convert.ToInt32(dt.Rows[i]["col6"])};

                string data = TcpManager.Client(txtIP.Text, 12345, getJson(c));
                ExecuteResponse(data);
            }
            else if (e.ColumnIndex == 7)
            {
                //删除
                dt.Rows.RemoveAt(e.RowIndex);
                grd.DataSource = dt;
            }
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMethod_Click(object sender, EventArgs e)
        {
            Capm c = new Capm();
            c.ModelType = cboModel.SelectedIndex;//型号
            c.MsgType = 1;//消息类型
            c.MethodType = cboMethod.SelectedIndex;

            string data = TcpManager.Client(txtIP.Text, 12345, getJson(c));
            ExecuteResponse(data);
        }
        /// <summary>
        /// PMW端口测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            Capm c = new Capm();
            c.ModelType = cboModel.SelectedIndex;//型号
            c.MsgType = 2;//消息类型
            c.Ids = new int[] { Convert.ToInt32(txtPMWid.Value) };
            c.Vals = new int[] { Convert.ToInt32(txtPMWVal.Value) };

            string data = TcpManager.Client(txtIP.Text, 12345, getJson(c));
            ExecuteResponse(data);
        }
    }
}
