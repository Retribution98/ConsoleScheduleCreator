using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    //public class ProjectTests
    //{
    //    
    //    [Fact]
    //    public void ReadyJobsTests_1Job_1Job()
    //    {
    //        List<Job> jobs = new List<Job>();
    //        jobs.Add(new Job("TestJob", 1, 0, 100, 10));
    //        string[] workersName = new string[0];
    //        int[,] workersTime = new int[1, 0];
    //        Project proj = new Project("Test", 0, 100, jobs, workersName, workersTime);

    //        Assert.Equal(new Job("TestJob", 1, 0, 100, 100).ToString(), proj.ReadyJobs(10)[0].ToString());

    //        List<Job> actual = proj.ReadyJobs(10);

    //        Assert.Single(actual);
    //    }

    //    [Fact]
    //    public void PenaltyProjectTest_CompletedJobs()
    //    {
    //        Job job1 = new Job("First", 1, 0, 5, 2);
    //        job1.Complete(0, 10);
    //        Job job2 = new Job("Second", 2, 0, 5, 2);
    //        job2.Complete(0, 10);
    //        Job job3 = new Job("Third", 3, 0, 5, 2);
    //        job3.Complete(0, 10);
    //        List<Job> jobs = new List<Job>() { job1, job2, job3 };
    //        string[] workersName = new string[0];
    //        int[,] workersTime = new int[0,0];
    //        Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);

    //        Int64 actual = project.PenaltyProject(20);
    //        Int64 expected = 30;

    //        Assert.Equal(expected, actual);
    //    }

    //    [Fact]
    //    public void PenaltyProjectTest_1JobsNotCompleted()
    //    {
    //        Job job1 = new Job("First", 1, 0, 5, 2);
    //        job1.Complete(0, 10);
    //        Job job2 = new Job("Second", 2, 0, 5, 2);
    //        job2.Complete(0, 10);
    //        Job job3 = new Job("Third", 3, 0, 5, 2);
    //        List<Job> jobs = new List<Job>() { job1, job2, job3 };
    //        string[] workersName = new string[0];
    //        int[,] workersTime = new int[0, 0];
    //        Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);

    //        Int64 actual = project.PenaltyProject(20);
    //        Int64 expected = 50;

    //        Assert.Equal(expected, actual);
    //    }

    //    [Fact]
    //    public void PenaltyProjectTest_StartTime()
    //    {
    //        Job job1 = new Job("First", 1, 0, 5, 2);
    //        Job job2 = new Job("Second", 2, 0, 5, 2);
    //        Job job3 = new Job("Third", 3, 0, 5, 2);
    //        List<Job> jobs = new List<Job>() { job1, job2, job3 };
    //        string[] workersName = new string[0];
    //        int[,] workersTime = new int[0, 0];
    //        Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);

    //        Int64 actual = project.PenaltyProject(0);
    //        Int64 expected = 0;

    //        Assert.Equal(expected, actual);
    //    }

    //    [Fact]
    //    public void PenaltyProjectTest_HaveNotJobs()
    //    {
    //        List<Job> jobs = new List<Job>();
    //        string[] workersName = new string[0];
    //        int[,] workersTime = new int[0, 0];
    //        Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);

    //        Int64 actual = project.PenaltyProject(20);
    //        Int64 expected = 0;

    //        Assert.Equal(expected, actual);
    //    }

    //    [Fact]
    //    public void CreateScheduleTests_Proj1()
    //    {
    //        Job job1 = new Job("First", 1, 0, 20, 2);
    //        Job job2 = new Job("Second", 2, 0, 5, 2);
    //        job2.AddPrevios(job1);
    //        Job job3 = new Job("Third", 3, 0, 5, 2);
    //        job3.AddPrevios(job2);
    //        Job job4 = new Job("Fourth", 4, 0, 5, 2);
    //        job4.AddPrevios(job1);
    //        Job job5 = new Job("Fifth", 5, 8, 5, 2);
    //        job5.AddPrevios(job4);
    //        Job job6 = new Job("Sixth", 6, 0, 20, 2);
    //        job6.AddPrevios(job5);
    //        Job job7 = new Job("Seventh", 7, 12, 20, 2);
    //        job7.AddPrevios(job4);
    //        Job job8 = new Job("Eighth", 8, 0, 20, 2);
    //        job8.AddPrevios(job3);
    //        job8.AddPrevios(job6);
    //        job8.AddPrevios(job7);

    //        List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };
    //        string[] workersName = { "Vasya", "Petya", "Vova" };

    //        int[,] workersTime =
    //        {
    //            { 3,2,3,4,4,1,5,3},
    //            { 3,3,2,3,2,3,6,3},
    //            { 3,3,3,3,3,3,3,3}
    //        };

    //        Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);
    //        int time = 20;

    //        uint[,] actual = project.CreateSchedule(time);
    //        uint[,] expected =
    //        {
    //            {1,1,1,2,2,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0 },
    //            {0,0,0,4,4,4,0,0,5,5,0,0,0,0,0,8,8,8,0,0 },
    //            {0,0,0,0,0,3,3,3,0,0,0,0,7,7,7,0,0,0,0,0 }
    //        };

    //        Assert.Equal(expected, actual);
    //        //for (int w = 0; w < workersName.Length; w++)
    //        //    for (int t = 0; t < time; t++)
    //        //        Assert.True(actual[w, t] == expected[w, t],
    //        //            String.Format("Expected: '{0}', Actual: '{1}' at worker = {2} and time = {3}.", expected[w,t], actual[w,t],w,t));
    //    }

    //    [Fact]
    //    public void CreateScheduleTests_Proj2()
    //    {
    //        Job job1 = new Job("First", 1, 0, 1, 2);
    //        Job job2 = new Job("Second", 2, 0, 3, 2);
    //        Job job3 = new Job("Third", 3, 0, 3, 2);
    //        Job job4 = new Job("Fourth", 4, 0, 1, 2);
    //        Job job5 = new Job("Fifth", 5, 3, 5, 2);
    //        job5.AddPrevios(job1);
    //        job5.AddPrevios(job2);
    //        Job job6 = new Job("Sixth", 6, 3, 5, 2);
    //        job6.AddPrevios(job3);
    //        job6.AddPrevios(job4);
    //        Job job7 = new Job("Seventh", 7, 5, 7, 2);
    //        job7.AddPrevios(job5);
    //        job7.AddPrevios(job6);
    //        Job job8 = new Job("Eighth", 8, 0, 9, 2);
    //        job8.AddPrevios(job7);
    //        List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };

    //        string[] workersName = { "Vasya", "Petya", "Vova" };
    //        int[,] workersTime =
    //        {
    //            { 1,3,2,3,4,3,3,3},
    //            { 2,5,8,1,2,2,2,2},
    //            { 1,4,2,1,3,2,2,2}
    //        };

    //        Project project = new Project("Test", 0, 15, jobs, workersName, workersTime);
    //        int time = 15;

    //        uint[,] actual = project.CreateSchedule(time);
    //        uint[,] expected =
    //        {
    //            {1,4,4,4,0,0,0,0,0,0,0,0,0,0,0 },
    //            {3,3,3,3,3,3,3,3,0,0,7,7,0,0,0 },
    //            {2,2,2,2,5,5,5,0,6,6,0,0,8,8,0 }
    //        };

    //        Assert.Equal(expected, actual);
    //    }
    //}
}
