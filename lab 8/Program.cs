using System;
using System.Threading;

class Program
{
    static Random random = new Random();
    static object lockObj = new object();
    static int randomNumber;
    static bool complete = false;

    static void Thread1()
    {
        for (int i = 0; i < 100; i++)
        {
            lock (lockObj)
            {
                randomNumber = random.Next(1, 11);
                Console.WriteLine($"Thread 1: sinh so ngau nhien: {randomNumber}");
                Monitor.Pulse(lockObj);
                Monitor.Wait(lockObj);
            }
            Thread.Sleep(2000);
        }
        complete = true;
    }

    static void Thread2()
    {
        for (int i = 0; i < 100; i++)
            lock (lockObj)
            {
                Monitor.Wait(lockObj);
                int squaredNumber = randomNumber * randomNumber;
                Console.WriteLine($"Thread 2: binh phuong cua so: is {squaredNumber}");
                Monitor.Pulse(lockObj);
            }
        Thread.Sleep(1000);

    }


    static void Main(string[] args)
    {   
        Thread thread1 = new Thread(new ThreadStart(Thread1));
        Thread thread2 = new Thread(new ThreadStart(Thread2));

        thread1.Start();
        thread2.Start();
        while (!complete)
        {
            Thread.Sleep(1000);
        }
        Console.WriteLine("ket thuc chuong trinh");
    }
}