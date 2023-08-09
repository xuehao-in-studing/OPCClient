using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

// 作为client访问server

namespace OPC_Kepserver
{
    public class OPCItem
    {
        public OPCItem() { }

        public string ItemID;
        public object Value;
        public int Quality;
        public String TimeStamp;
    }

    public partial class Form2 : Form
    {
        // 服务器对象
        private OPCServer kepServer = new OPCServer();
        private OPCBrowser browser;
        private OPCGroups kepGroups;
        // OPC读取都是通过组来读取
        private OPCGroup kepGroup;
        private int timeInterval = 250;



        #region 一些进行opc异步读取的属性
        private int numItems = 1;
        private Array serverHanlde;
        private int transactionID = 0;
        private int cancelID = 0;
        //List<string> ItemIDs = new List<string>();
        //List<int> ClientHandles = new List<int>();
        // 存储什么东西？
        private List<OPCItem> OPCItemList = new List<OPCItem>();
        private Array error;

        #endregion
        public Form2()
        {
            InitializeComponent();
        }

        private void ServerName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region 获取服务器列表
        private void ServerNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ServerName.Items.Clear();
            try
            {
                if (kepServer != null)
                {
                    object opcServerList = kepServer.GetOPCServers(this.ServerNode.Text);
                    // 获取到的opcserver 名称进行显示，并加入数组中
                    foreach (var server in (Array)opcServerList)
                    {
                        if (!this.ServerName.Items.Contains(server))
                        {
                            this.ServerName.Items.Add(server);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 刷新列表
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            // 根据本机名获取host,获取连接时的IP地址
            IPHostEntry hostEntry = Dns.GetHostEntry(Environment.MachineName);

            // 清空servernode所有项
            this.ServerNode.Items.Clear();
            try
            {
                if (hostEntry.AddressList.Length > 0)
                    for (int i = 0; i < hostEntry.AddressList.Length; i++)
                    {
                        string hostname = Dns.GetHostEntry(hostEntry.AddressList[i].ToString()).HostName;
                        if (!this.ServerNode.Items.Contains(hostname))
                        {
                            this.ServerNode.Items.Add(hostname);
                        }
                    }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 连接服务器
        /// <summary>
        /// 连接服务器事件
        /// 连接到某个特定的主机的OPC服务器，由两个选择框决定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnBtn_Click(object sender, EventArgs e)
        {
            if (this.ConnBtn.Text == "Connect")
            {
                // 建立连接
                try
                {
                    // 连接OPC需要确定两个对象，一个是OPC的Porgid，一个是服务器节点
                    kepServer.Connect(ServerName.Text, ServerNode.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.ConnBtn.Text = "Disconnect";
                KepGroupInit();

                // 显示服务器中所有可访问的变量
                // 可读可写？
                browser = kepServer.CreateBrowser();
                browser.ShowBranches();
                browser.ShowLeafs(true);

                OpcList.Items.Clear();

                foreach (var opc in browser)
                {
                    if (!OpcList.Items.Contains(opc))
                    {
                        OpcList.Items.Add(opc);
                    }
                }

            }

            else if (ConnBtn.Text == "Disconnect")
            {
                kepServer.Disconnect();
                ConnBtn.Text = "Connect";
            }
        }
        #endregion
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Hello");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 初始化kepgroup对象，并绑定异步读取完成事件
        /// </summary>
        public void KepGroupInit()
        {
            kepGroups = kepServer.OPCGroups;
            kepGroups.DefaultGroupDeadband = 10;
            kepGroups.DefaultGroupUpdateRate = 250;
            kepGroups.DefaultGroupIsActive = true;

            kepGroup = kepGroups.Add("DefaultGroup");
            kepGroup.IsActive = true;
            kepGroup.IsSubscribed = true;
            kepGroup.DeadBand = 10;

            kepGroup.AsyncReadComplete += KepGroup_AsyncReadComplete;
            kepGroup.AsyncWriteComplete += KepGroup_AsyncWriteComplete;
        }

        private void KepGroup_AsyncWriteComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array Errors)
        {
            bool success = true;
            // 写入
            for (int i = 1; i <= numItems; i++)
            {
                // error 为0则表示写入成功，否则失败
                int error = (int)Errors.GetValue(i);
                if (error == 0)
                {
                }
                else
                {
                    success = false;
                }
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 点击按钮写一下数据
        /// 对items列表里的项进行修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void WriteBtn_Click(object sender, EventArgs e)
        {
            // 需要占位符
            object[] val = { 1,1 };
            var vallist = val.ToList();
            // 占位符
            vallist.Insert(0,null);
            Array Value = vallist.ToArray();
            if (serverHanlde != null)
            {
                try
                {
                    kepGroup.AsyncWrite(Value.Length-1, serverHanlde, Value, out error, transactionID, out cancelID);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                MessageBox.Show("服务器未连接!");
            }
            //throw new NotImplementedException();
        }

        private void KepGroup_AsyncReadComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps, ref Array Errors)
        {
            // 解析读取结果

            int count = NumItems;
            // 索引必需从一开始,是由OPC通讯的内置函数决定的
            for (int i = 1; i <= NumItems; i++)
            {
                object value = ItemValues.GetValue(i);
                if (value != null)
                {
                    int clientHandle = Convert.ToInt32(ClientHandles.GetValue(i));
                    for (int j = 0; j < OPCItemList.Count; j++)
                    {
                        // 通过匹配serverhandle来对opcitem进行赋值
                        if (j == clientHandle)
                        {
                            OPCItemList[j].Value = value;
                            OPCItemList[j].TimeStamp = TimeStamps.GetValue(i).ToString();
                            OPCItemList[j].Quality = (int)Qualities.GetValue(i);
                            // 实时刷新的同时仅在同一行进行更新，不添加多行
                            if (OpcItemViewer.RowCount < OPCItemList.Count)
                                OpcItemViewer.Rows.Add(OPCItemList[j].ItemID, OPCItemList[j].Value, OPCItemList[j].TimeStamp, OPCItemList[j].Quality);
                            else
                            {
                                OpcItemViewer.Rows[j].Cells[0].Value = OPCItemList[j].ItemID;
                                OpcItemViewer.Rows[j].Cells[1].Value = OPCItemList[j].Value;
                                OpcItemViewer.Rows[j].Cells[2].Value = OPCItemList[j].TimeStamp;
                                OpcItemViewer.Rows[j].Cells[3].Value = OPCItemList[j].Quality;
                            }
                        }
                    }
                }
            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 一定间隔从server读取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (kepServer != null)
            {
                if (kepGroup != null && this.OPCItemList.Count > 0)
                {
                    kepGroup.AsyncRead(this.OPCItemList.Count, serverHanlde, out error, transactionID, out cancelID);
                }
            }
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 触发事件，
        /// 事件功能：1.选择opcitem并将item加入到OPCItemList中去
        /// 2.显示加入的item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 防止重复添加已有的item
            if (OpcList.Items.Count > 0 && !OPCItemList.Exists(x => x.ItemID == OpcList.Text))
            {
                OPCItemList.Add(new OPCItem()
                {
                    ItemID = this.OpcList.Text,
                });

                // 每次都要新建吗，有没有其他方式
                List<string> ItemIDs = new List<string>();
                List<int> ClientHandles = new List<int>();
                // 占位符
                ItemIDs.Add("999");
                ClientHandles.Add(999);

                // 把选择的项加入列表
                for (int i = 0; i < OPCItemList.Count; i++)
                {
                    ItemIDs.Add(OPCItemList[i].ItemID);
                    ClientHandles.Add(i);
                }

                Array ItemID = ItemIDs.ToArray();
                Array ClientHandle = ClientHandles.ToArray();

                // 获取serverhandle
                kepGroup.OPCItems.AddItems(OPCItemList.Count, ref ItemID, ref ClientHandle, out serverHanlde, out error);
            }
        }

    }
}
