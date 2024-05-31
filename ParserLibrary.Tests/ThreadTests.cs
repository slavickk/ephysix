/******************************************************************
 * File: ThreadTests.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParserLibrary.Tests;

[TestFixture]
public class ThreadTests
{

    [Test]
    public static async System.Threading.Tasks.Task TestWaiting()
   
    {
        int countSuc = 0, countNoSuc = 0;
        for (int i = 0; i < 20; i++)
        {
            int timeoutInMilliseconds = 3000;
            if(i%2 ==0)
                timeoutInMilliseconds= 1000;

            int milli = 2000;

            try
            {
                await tt(milli).WaitAsync(TimeSpan.FromMilliseconds(timeoutInMilliseconds));
                countSuc++;
            }
            catch (TimeoutException t)
            {
                countNoSuc++;
//                Console.WriteLine(t.ToString());
            }
        }
        Assert.AreEqual(countSuc, countNoSuc);
    }
    async static System.Threading.Tasks.Task tt(int milli)
    {
        await System.Threading.Tasks.Task.Delay(milli);
        return;

    }


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