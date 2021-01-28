using System;
using System.Threading;

namespace ThreadProject
{
    class Printer
    {
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();

        public void PrintA(bool running)
        {
            Monitor.Enter(lock1);

            Console.Write('A');

            //Unlock lock2
            Monitor.Enter(lock2);
            Monitor.Pulse(lock2);
            Monitor.Exit(lock2);

            //Waitting lock1 to get unclocked
            Monitor.Wait(lock1);
        }

        public void PrintB(bool running)
        {
            Monitor.Enter(lock2);

            Console.Write('B');

            //Unlock lock3
            Monitor.Enter(lock3);
            Monitor.Pulse(lock3);
            Monitor.Exit(lock3);

            //Waitting lock2 to get unclocked
            Monitor.Wait(lock2);
        }

        public void PrintC(bool running)
        {
            Monitor.Enter(lock3);

            Console.Write('C');

            //Unlock lock1
            Monitor.Enter(lock1);
            Monitor.Pulse(lock1);
            Monitor.Exit(lock1);

            //Waitting lock3 to get unclocked
            Monitor.Wait(lock3);
        }

    }

    class MyThread
    {
        public static int count = 20;

        public Thread thread;
        Printer printer;

        public MyThread(string name, Printer prt)
        {
            thread = new Thread(this.Run);
            printer = prt;
            thread.Name = name;
            thread.Start();
        }

        void Run()
        {
            if (thread.Name == "A")
            {
                for (int i = 0; i < count; i++)
                    printer.PrintA(true);
                printer.PrintA(false);
            }
            else if (thread.Name == "B")
            {
                for (int i = 0; i < count; i++)
                    printer.PrintB(true);
                printer.PrintB(false);
            }
            else if (thread.Name == "C")
            {
                for (int i = 0; i < count; i++)
                    printer.PrintC(true);
                printer.PrintC(false);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Printer prt = new Printer();

            MyThread thread1 = new MyThread("A", prt);

            //Giving thread1 some time to start
            Thread.Sleep(50);

            MyThread thread2 = new MyThread("B", prt);

            Thread.Sleep(50);

            MyThread thread3 = new MyThread("C", prt);

            thread1.thread.Join();
            thread2.thread.Join();
            thread3.thread.Join();

            Console.WriteLine("\nEnd");
            Console.ReadLine();
        }
    }
}
