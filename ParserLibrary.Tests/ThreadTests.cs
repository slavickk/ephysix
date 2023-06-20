using NUnit.Framework;
using System;
using System.Threading;

namespace ParserLibrary.Tests;

[TestFixture]
public class ThreadTests
{
    [Test]
    public static void ScheduleManyTasks()
    {
        // Set the maximum number of threads in the thread pool to 10
        ThreadPool.SetMaxThreads(10, 10);

        // Schedule 10000 tasks to the thread pool
        for (int i = 0; i < 10000; i++)
        {
            Console.WriteLine("Scheduling task {0}");
            ThreadPool.QueueUserWorkItem(new WaitCallback(Task), i);
        }

        Console.ReadLine();
    }

    static void Task(object state)
    {
        // Perform the task
        Console.WriteLine("Task {0} executed", state);
        
        // The the number of threads in the thread pool
        ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
    }
}