﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OPCClient
{
    public class LogClass
    {

        string filename = "LogFile.txt";
        public LogClass()
        {

        }
        public LogClass(string file)
        {
            filename = file;
        }
        /**/
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        public void WriteLogFile(string input)
        {
            /**/
            ///指定日志文件的目录
            string fname = Directory.GetCurrentDirectory() + "\\" + filename;
            /**/
            ///定义文件信息对象
            try
            {
                FileInfo finfo = new FileInfo(fname);

                if (!finfo.Exists)
                {
                    FileStream fs;
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }

                /**/
                ///判断文件是否存在以及是否大于2K
                if (finfo.Length > 1024 * 1024 * 100)
                {

                    /**/
                    ///文件超过10MB则重命名
                    File.Move(Directory.GetCurrentDirectory() + "\\" + filename, Directory.GetCurrentDirectory() + "\\LogFile" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt");
                    /**/
                    ///删除该文件
                    finfo.Delete();
                }
                //finfo.AppendText();
                /**/
                ///创建只写文件流

                using (FileStream fs = finfo.OpenWrite())
                {

                    /**/
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter w = new StreamWriter(fs);

                    /**/
                    ///设置写数据流的起始位置为文件流的末尾
                    w.BaseStream.Seek(0, SeekOrigin.End);

                    /**/
                    ///写入“Log Entry : ”
                    w.Write("\n\rLog Entry : ");

                    /**/
                    ///写入当前系统时间并换行
                    w.Write("{0} {1} \n\r", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());

                    /**/
                    ///写入日志内容并换行
                    w.Write(input + "\n\r");

                    /**/
                    ///写入------------------------------------“并换行
                    //w.Write("------------------------------------\n\r");

                    /**/
                    ///清空缓冲区内容，并把缓冲区内容写入基础流
                    w.Flush();

                    /**/
                    ///关闭写数据流
                    w.Close();
                }
            }
            catch { }
        }

        /**/
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="input"></param>
        public void WritetxtFile(string input, string filename)
        {
            /**/
            ///指定日志文件的目录
            string fname = Directory.GetCurrentDirectory() + "\\" + filename;
            /**/
            ///定义文件信息对象
            try
            {
                FileInfo finfo = new FileInfo(fname);

                if (!finfo.Exists)
                {
                    FileStream fs;
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }

                /**/
                ///判断文件是否存在以及是否大于2K
                if (finfo.Length > 1024 * 1024 * 10)
                {

                    /**/
                    ///文件超过10MB则重命名
                    File.Move(Directory.GetCurrentDirectory() + "\\" + filename, Directory.GetCurrentDirectory() + DateTime.Now.TimeOfDay + "\\" + filename);
                    /**/
                    ///删除该文件
                    //finfo.Delete();
                }
                //finfo.AppendText();
                /**/
                ///创建只写文件流

                using (FileStream fs = finfo.OpenWrite())
                {
                    //清除文本内容
                    fs.SetLength(0);
                    /**/
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter w = new StreamWriter(fs, Encoding.UTF8);

                    /**/
                    ///设置写数据流的起始位置为文件流的末尾
                    w.BaseStream.Seek(0, SeekOrigin.End);



                    /**/
                    ///写入日志内容
                    w.Write(input);

                    /**/
                    ///写入------------------------------------“并换行
                    //w.Write("------------------------------------\n\r");

                    /**/
                    ///清空缓冲区内容，并把缓冲区内容写入基础流
                    w.Flush();

                    /**/
                    ///关闭写数据流
                    w.Close();
                }
            }
            catch { }
        }

        /**/
        /// <summary>
        /// 读日志文件
        /// </summary>
        /// <param name="input"></param>
        public string ReadTxtFile(string input, string file)
        {
            string s = "";
            try
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\" + file, Encoding.Default);
                s = sr.ReadToEnd();
            }
            catch { }
            return s;
        }
    }
}
