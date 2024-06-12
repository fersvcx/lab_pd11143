using System;
using System.Threading;

class Program
{
    static object lock1 = new object();
    static object lock2 = new object();

    static void Thread1()
    {
        bool lock1Taken = false;
        bool lock2Taken = false;

        try
        {
            Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(1000), ref lock1Taken);
            if (lock1Taken)
            {
                Console.WriteLine("Thread1 locked lock1");
                Thread.Sleep(100);
                Console.WriteLine("Thread 1 is waiting for lock2");

                try
                {
                    Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(1000), ref lock2Taken);
                    if (lock2Taken)
                    {
                        Console.WriteLine("Thread1 locked lock2");
                    }
                    else
                    {
                        Console.WriteLine("Thread1 could not lock lock2");
                    }
                }
                finally
                {
                    if (lock2Taken)
                    {
                        Monitor.Exit(lock2);
                    }
                }
            }
            else
            {
                Console.WriteLine("Thread1 could not lock lock1");
            }
        }
        finally
        {
            if (lock1Taken)
            {
                Monitor.Exit(lock1);
            }
        }
    }

    static void Thread2()
    {
        bool lock2Taken = false;
        bool lock1Taken = false;

        try
        {
            Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(1000), ref lock2Taken);
            if (lock2Taken)
            {
                Console.WriteLine("Thread2 locked lock2");
                Thread.Sleep(100);
                Console.WriteLine("Thread 2 is waiting for lock1");

                try
                {
                    Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(1000), ref lock1Taken);
                    if (lock1Taken)
                    {
                        Console.WriteLine("Thread2 locked lock1");
                    }
                    else
                    {
                        Console.WriteLine("Thread2 could not lock lock1");
                    }
                }
                finally
                {
                    if (lock1Taken)
                    {
                        Monitor.Exit(lock1);
                    }
                }
            }
            else
            {
                Console.WriteLine("Thread2 could not lock lock2");
            }
        }
        finally
        {
            if (lock2Taken)
            {
                Monitor.Exit(lock2);
            }
        }
    }

    static void Main()
    {
        Thread thread1 = new Thread(Thread1);
        Thread thread2 = new Thread(Thread2);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
        Console.WriteLine("End of program");
    }
}