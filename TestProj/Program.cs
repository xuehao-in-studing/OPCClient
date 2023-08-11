using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProj
{

    class Productor
    {
        public object MyProperty { get; set; }
        public Productor()
        {

        }

        public void Produce(Buffer buffer)
        {
            while (true)
            {
                Buffer.semaphore.WaitOne();
                lock (buffer.LockObj)
                {
                    if (buffer.BufferCount < buffer.MaxCount)
                        // 向缓冲区生产一个产品
                        buffer.BufferCount++;

                    Console.WriteLine($"{Thread.CurrentThread.Name} is product and count of buffer is {buffer.BufferCount}");
                    Thread.Sleep(10);
                    //else
                    //    break;
                    // 缓冲区满退出
                    Buffer.semaphore.Release();
                }
            }
        }
    }


    class Consumer
    {
        public Consumer()
        {

        }

        public void Consume(Buffer buffer)
        {
            while (true)
            {
                Buffer.semaphore.WaitOne();
                lock (buffer.LockObj)
                {
                    // 从缓冲区消耗一个产品
                    if (buffer.BufferCount > 0)
                        buffer.BufferCount--;
                    Console.WriteLine($"{Thread.CurrentThread.Name} is consume and count of buffer is {buffer.BufferCount}");
                    Thread.Sleep(10);
                    //else
                    //    break;
                    Buffer.semaphore.Release();
                }
            }
        }
    }

    class Buffer
    {
        public static Semaphore semaphore = new Semaphore(1, 1);

        public object LockObj { get; set; }

        public Buffer(int maxcount)
        {
            LockObj = new object();
            MaxCount = maxcount;
        }

        public int bufferCount = 0;
        public int BufferCount
        {
            get
            {
                return bufferCount;
            }
            set
            {
                if (value >= 0)
                    bufferCount = value;
                else
                    throw new Exception("must above 0");
            }
        }

        public int MaxCount { get; set; }

    }


    class DataBase
    {
        public int ReaderCount { get; set; }
        // 写者互斥锁
        public static Semaphore writeSema = new Semaphore(1, 1);
        // 读者数量访问锁
        public static object rCountMutex = new object();

        public DataBase()
        {
            ReaderCount = 0;
        }
    }

    class Reader
    {
        public Reader()
        {

        }

        public void Read(DataBase dataBase)
        {
            while (true)
            {
                lock (DataBase.rCountMutex)
                {
                    dataBase.ReaderCount++;
                }
                // 等待写入互斥锁释放
                DataBase.writeSema.WaitOne();
                // 无写者执行逻辑
                Console.WriteLine($"读者数量{dataBase.ReaderCount}，{Thread.CurrentThread.Name}正在读");
                Thread.Sleep(20);
                // 读完，离开
                lock (DataBase.rCountMutex)
                {
                    dataBase.ReaderCount--;
                }
                DataBase.writeSema.Release();
            }
        }
    }

    class Writer
    {
        public Writer()
        {

        }


        public void Write(DataBase dataBase)
        {
            while (true)
            {
                lock (DataBase.rCountMutex)
                {
                    if (dataBase.ReaderCount == 0)
                    {
                        // 读者写着互斥逻辑
                        DataBase.writeSema.WaitOne();
                        Console.WriteLine($"{Thread.CurrentThread.Name}正在写");
                        Thread.Sleep(20);
                        DataBase.writeSema.Release();
                    }
                }
            }
        }
    }


    class Program
    {
        static Semaphore semaphore = new Semaphore(1, 3); // 创建一个初始值为 3 的信号量
        static int ticketCount = 50;

        static object lockobj = new object();
        static void Worker()
        {
            while (true)
            {
                semaphore.WaitOne(); // 获取信号量，如果计数器为零，则线程会阻塞等待
                                     // 访问共享资源
                                     // 模拟客人购票，多个窗口同时售出这么多票
                                     //Console.WriteLine("Thread {0} is accessing the shared resource.", Thread.CurrentThread.Name);
                if (ticketCount > 0)
                {
                    ticketCount -= 1;
                    Console.WriteLine($"窗口{Thread.CurrentThread.Name}卖出去一张票，余票:{ticketCount}");
                    // 模拟线程访问共享资源的耗时操作
                    Thread.Sleep(20);
                    semaphore.Release(); // 释放信号量，将计数器加一
                }
                else
                    break;
            }

        }
        /// <summary>
        /// 自旋锁简单实例
        /// </summary>
        static void show()
        {
            while (true)
            {
                lock (lockobj)
                {
                    if (ticketCount > 0)
                    {
                        // 模拟客人购票，多个窗口同时售出这么多票
                        //Console.WriteLine("Thread {0} is accessing the shared resource.", Thread.CurrentThread.Name);
                        ticketCount -= 1;
                        Console.WriteLine($"窗口{Thread.CurrentThread.Name}卖出去一张票，余票:{ticketCount}");
                        // 模拟线程访问共享资源的耗时操作
                        Thread.Sleep(20);
                    }
                    else
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            Reader[] readers = new Reader[10];
            Writer[] writers = new Writer[10];

            for (int i = 0; i < readers.Length; i++)
            {
                readers[i] = new Reader();
                writers[i] = new Writer();
            }

            DataBase data = new DataBase();

            Thread[] threads = new Thread[20];

            for (int i = 0; i < readers.Length; i++)
            {
                int index = i;

                threads[readers.Length + index] = new Thread(() =>
                {
                    writers[index].Write(data);
                });
                threads[readers.Length + index].Name = $"writer {index + 1}";
                threads[readers.Length + index].Start();

                threads[index] = new Thread(() =>
                {
                    readers[index].Read(data);
                });
                threads[index].Name = $"reader {index + 1}";
                threads[index].Start();
                Console.WriteLine(index);
            }


            //等待所有线程结束
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            Console.ReadLine();
        }
    }
}
