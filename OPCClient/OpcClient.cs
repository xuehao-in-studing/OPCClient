using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OPCAutomation;

namespace OPCClient
{
    /// <summary>
    /// customized OPCItem class for OPC communication
    /// </summary>
    public class OPCItem
    {
        public OPCItem() { }

        public string ItemID;
        public object Value;
        public int Quality;
        public string TimeStamp;
    }
    public class OpcClient
    {
        private LogClass logger = new LogClass();

        // 服务器的名称或者ProgID
        private String serverIP;
        // 本机IP地址
        private String serverName;

        // group对象
        private OPCGroups clientGroups;
        private OPCGroup clientGroup;
        private OPCBrowser opcBrowser;

        // 存放item的列表
        private List<OPCItem> itemList = new List<OPCItem>();
        private Array itemIDs;

        // 客户端分配句柄
        private Array clientHandles;
        // 服务器分配的句柄
        private Array serverHanelds;
        private Array errors;
        private int cancellID = 0;
        private int TransactionID = 0;

        // 读取节点的数据属性
        private string[] tagTypes;

        public string[] TagTypes
        {
            get { return tagTypes; }
            set { tagTypes = value; }
        }
        private string[] tagNames;
        public string[] TagNames
        {
            get { return tagNames; }
            set { tagNames = value; }
        }

        private ManualResetEvent resetEvent = new ManualResetEvent(false);

        // 读取结束的阻塞事件
        public ManualResetEvent ResetEvent
        {
            get { return resetEvent; }
            set {
                if (value != null)
                    resetEvent = value;
                else
                    throw new ArgumentException("设置非引用类型");
            }
        }

        // opcserver
        private OPCServer opcServer = new OPCServer();

        public String ServerIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }

        public String ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }


        public OPCServer OpcServer
        {
            get { return opcServer; }
            set { opcServer = value; }
        }

        // 读取appSettings配置
        //string[] m_tags = ConfigurationManager.AppSettings.Get("m_tags").Split(';');
        //string[] m_types = ConfigurationManager.AppSettings.Get("m_types").Split(';');

        public OpcClient()
        {
        }


        public void Init()
        {
            ConnectToServer();
            SetGroups();
            ItemInit();
        }

        /// <summary>
        /// 连接到OPC服务器
        /// </summary>
        //public void ConnectToServer()
        //{
        //    try
        //    {
        //        object servers = opcServer.GetOPCServers(ServerNode);
        //        if (servers != null)
        //        {
        //            foreach (var server in (Array)servers)
        //            {
        //                opcServer.Connect(ServerName, ServerNode);
        //                logger.WriteLogFile("OPCClient connect");
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public void Close()
        {
            try
            {
                // 1 equals running
                // OPCServerState.OPCRunning equals 1
                if(opcServer.ServerState == 1)
                {
                    opcServer.Disconnect();
                    Console.WriteLine("OPC client已关闭");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


        public void ConnectToServer()
        {
            try
            {
                opcServer.Connect(ServerIP, ServerName);
                logger.WriteLogFile("OPCClient connect");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


        /// <summary>
        /// 设置group的一些属性
        /// </summary>
        public void SetGroups()
        {
            clientGroups = opcServer.OPCGroups;
            clientGroups.DefaultGroupDeadband = 10;
            clientGroups.DefaultGroupLocaleID = 0;
            clientGroups.DefaultGroupIsActive = true;
            clientGroups.DefaultGroupUpdateRate = 250;

            clientGroup = clientGroups.Add("DefaultGroup");
            clientGroup.IsActive = true;
            clientGroup.IsSubscribed = true;
            clientGroup.DeadBand = 0;

            clientGroup.DataChange += ClientGroup_DataChange;
            clientGroup.AsyncReadComplete += ClientGroup_AsyncReadComplete;
            clientGroup.AsyncWriteComplete += ClientGroup_AsyncWriteComplete;
        }

        private void ClientGroup_AsyncWriteComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array Errors)
        {
            Console.WriteLine("写完成调用回调");
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 订阅响应函数
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void ClientGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                if (NumItems != 0 && ItemValues != null)
                {
                    string splitString = "====================" + "====================";
                    Console.WriteLine(splitString + "\nWe are now DataChange Method\n" + splitString);
                    for (int i = 1; i <= NumItems; i++)
                    {
                        int listIdx = (int)ClientHandles.GetValue(i);
                        var val = ItemValues.GetValue(i);

                        itemList[listIdx].Value = val;
                        itemList[listIdx].Quality = (int)Qualities.GetValue(i);
                        itemList[listIdx].TimeStamp = TimeStamps.GetValue(i).ToString();
                        Console.WriteLine($"读取第{i}个数完毕,DataType:{tagTypes[listIdx]}");
                        Console.WriteLine($"value:{itemList[listIdx].Value},quality:{itemList[listIdx].Quality.ToString()},time:{itemList[listIdx].TimeStamp}");
                    }
                }

                // 50ms读取一次
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }



        private void ClientGroup_AsyncReadComplete(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps, ref Array Errors)
        {
            try
            {
                if (NumItems != 0 && ItemValues != null)
                {
                    string splitString = "====================" + "====================";
                    Console.WriteLine(splitString + "\nWe are now AsyncReadComplete Method\n" + splitString);
                    for (int i = 1; i <= NumItems; i++)
                    {
                        int listIdx = (int)ClientHandles.GetValue(i);
                        var val = ItemValues.GetValue(i);
                        //switch (tagTypes[i - 1])
                        //{
                        //    case "string": val = (string)val; break;
                        //    case "int": val = (int)val; break;
                        //    case "float": val = (float)val; break;
                        //    case "bool": val = (bool)val; break;
                        //}
                        itemList[listIdx].Value = val;
                        itemList[listIdx].Quality = (int)Qualities.GetValue(i);
                        itemList[listIdx].TimeStamp = TimeStamps.GetValue(i).ToString();

                        // 信号量置位
                        ResetEvent.Set();

                        Console.WriteLine($"读取第{i}个数完毕,DataType:{tagTypes[listIdx]}");
                        Console.WriteLine($"value:{itemList[listIdx].Value},quality:{itemList[listIdx].Quality.ToString()},time:{itemList[listIdx].TimeStamp}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ItemInit()
        {
            tagNames = (String[])ConfigurationManager.AppSettings.Get("m_tags").Split(';').Clone();
            tagTypes = (String[])ConfigurationManager.AppSettings.Get("m_types").Split(';').Clone();
            setItemsHandle();
        }

        #region 配置item
        /// <summary>
        /// 配置item对象
        /// </summary>
        private void setItemsHandle()
        {
            int count = TagNames.Count();
            List<String> ItemIDs = new List<String>();
            List<int> ClientHandles = new List<int>();
            // 占位符
            ItemIDs.Add("999");
            ClientHandles.Add(999);

            for (int i = 0; i < count; i++)
            {
                itemList.Add(new OPCItem()
                {
                    ItemID = TagNames[i],
                });
                ItemIDs.Add(TagNames[i]);
                ClientHandles.Add(i);
            }
            itemIDs = ItemIDs.ToArray();
            clientHandles = ClientHandles.ToArray();

            try
            {
                clientGroup.OPCItems.AddItems(count, ref itemIDs, ref clientHandles, out serverHanelds, out errors);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion


        public void BeginRead()
        {
            try
            {
                if (clientGroup != null && tagNames.Count() > 0)
                {
                    clientGroup.AsyncRead(tagNames.Length, serverHanelds, out errors, TransactionID, out cancellID);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void test()
        {

        }

        /// <summary>
        /// write a item accroding to idx
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="val"></param>
        public void writeitem(int idx, object val)
        {
            OPCItem itemvalue = new OPCItem();
            try
            {
                if (val != null)
                {
                    if (TagTypes[idx] == "bool")
                        itemvalue.Value = System.Convert.ToBoolean(System.Convert.ToInt32(val));
                    if (TagTypes[idx] == "int")
                        itemvalue.Value = System.Convert.ToInt32(val);
                    if (TagTypes[idx] == "float")
                        itemvalue.Value = System.Convert.ToDouble(val);
                    if (TagTypes[idx] == "string")
                        itemvalue.Value = val;
                }
                itemvalue.TimeStamp = DateTime.Now.ToString();
                // 测试异步写
                try
                {
                    Array Values = Array.CreateInstance(typeof(object), 2);
                    Values.SetValue(itemvalue.Value, 1);
                    Values.SetValue(null, 0);

                    List<int> Handles = new List<int>();
                    Handles.Add(0);
                    Handles.Add(int.Parse(serverHanelds.GetValue(idx + 1).ToString()));

                    clientGroup.AsyncWrite(1, Handles.ToArray(), Values, out errors, TransactionID, out cancellID);
                    //clientGroup.SyncWrite(1, Handles.ToArray(),Values,out errors);
                    Console.WriteLine($"item{idx} has changed,value is from {itemList[idx].Value} to {val}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }



        public void writeitems(int[] idx, object[] val)
        {

            var vallist = val.ToList();
            // 占位符
            vallist.Insert(0, null);
            Array Value = vallist.ToArray();
            if (serverHanelds != null && clientGroup != null)
            {
                try
                {
                    clientGroup.AsyncWrite(Value.Length - 1, serverHanelds, Value, out errors, TransactionID, out cancellID);
                    Console.WriteLine("多个数据项写完毕");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            else
            {
                Console.WriteLine("服务器未连接");
            }
        }

    }
}
