using ConsoleScheduleCreator.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void CreateScheduleTests_Proj1_FrontAlgorithm_FirstlyStratagy()
        {
            Job job1 = new Job("First", 1, 0, 20, 2);
            Job job2 = new Job("Second", 2, 0, 5, 2);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 5, 2);
            job3.AddPrevios(job2);
            Job job4 = new Job("Fourth", 4, 0, 5, 2);
            job4.AddPrevios(job1);
            Job job5 = new Job("Fifth", 5, 8, 5, 2);
            job5.AddPrevios(job4);
            Job job6 = new Job("Sixth", 6, 0, 20, 2);
            job6.AddPrevios(job5);
            Job job7 = new Job("Seventh", 7, 12, 20, 2);
            job7.AddPrevios(job4);
            Job job8 = new Job("Eighth", 8, 0, 20, 2);
            job8.AddPrevios(job3);
            job8.AddPrevios(job6);
            job8.AddPrevios(job7);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };
            string[] workersName = { "Vasya", "Petya", "Vova" };

            int[,] workersTime =
            {
                { 3,2,3,4,4,1,5,3},
                { 3,3,2,3,2,3,6,3},
                { 3,3,3,3,3,3,3,3}
            };

            Project project = new Project("Test", 0, 20, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new NextJobStratagies.FirstlyStratagy(), new GetWorkerStratagy.GreedyStatagy(), null, new PrinterToConsole()));

            Job[,] actual = project.Schedule.Planner;
            Job[,] expected =
            {
                {job1,job1,job1,job2,job2,null,null,null,null,null,job6,null,null,null,null,null,null,null,null,null },
                {null,null,null,job4,job4,job4,null,null,job5,job5,null,null,null,null,null,job8,job8,job8,null,null },
                {null,null,null,null,null,job3,job3,job3,null,null,null,null,job7,job7,job7,null,null,null,null,null }
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateScheduleTests_Proj2_FrontAlgorithm_FirstlyStratagy()
        {
            Job job1 = new Job("First", 1, 0, 1, 5);
            Job job2 = new Job("Second", 2, 0, 3, 8);
            Job job3 = new Job("Third", 3, 0, 3, 2);
            Job job4 = new Job("Fourth", 4, 0, 1, 5);
            Job job5 = new Job("Fifth", 5, 3, 5, 2);
            job5.AddPrevios(job1);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 3, 5, 2);
            job6.AddPrevios(job3);
            job6.AddPrevios(job4);
            Job job7 = new Job("Seventh", 7, 5, 7, 2);
            job7.AddPrevios(job5);
            job7.AddPrevios(job6);
            Job job8 = new Job("Eighth", 8, 0, 9, 2);
            job8.AddPrevios(job7);
            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };

            string[] workersName = { "Vasya", "Petya", "Vova" };
            int[,] workersTime =
            {
                { 1,3,2,3,4,3,3,3},
                { 2,5,8,1,2,2,2,2},
                { 1,4,2,1,3,2,2,2}
            };

            Project project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new NextJobStratagies.FirstlyStratagy(), new GetWorkerStratagy.GreedyStatagy(), null, new PrinterToConsole()));

            Job[,] actual = project.Schedule.Planner;
            Job[,] expected =
            {
                {job1,job4,job4,job4,null,null,null,null,null,null,null,null,null,null,null },
                {job3,job3,job3,job3,job3,job3,job3,job3,null,null,job7,job7,null,null,null },
                {job2,job2,job2,job2,job5,job5,job5,null,job6,job6,null,null,job8,job8,null }
            };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateScheduleTests_Proj2_FrontAlgorithm_GreedyStratagy()
        {
            Job job1 = new Job("First", 1, 0, 1, 5);
            Job job2 = new Job("Second", 2, 0, 3, 8);
            Job job3 = new Job("Third", 3, 0, 3, 2);
            Job job4 = new Job("Fourth", 4, 0, 1, 5);
            Job job5 = new Job("Fifth", 5, 3, 5, 2);
            job5.AddPrevios(job1);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 3, 5, 2);
            job6.AddPrevios(job3);
            job6.AddPrevios(job4);
            Job job7 = new Job("Seventh", 7, 5, 7, 2);
            job7.AddPrevios(job5);
            job7.AddPrevios(job6);
            Job job8 = new Job("Eighth", 8, 0, 9, 2);
            job8.AddPrevios(job7);
            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };

            string[] workersName = { "Vasya", "Petya", "Vova" };
            int[,] workersTime =
            {
                { 1,3,2,3,4,3,3,3},
                { 2,5,8,1,2,2,2,2},
                { 1,4,2,1,3,2,2,2}
            };

            Project project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new NextJobStratagies.GreedyStratagy(), new GetWorkerStratagy.GreedyStatagy(), null, new PrinterToConsole()));

            Job[,] actual = project.Schedule.Planner;
            Job[,] expected =
            {
                {job2,job2,job2,null,null,null,null,null,null,null,null,null,null,null,null },
                {job4,null,null,job5,job5,job7,job7,job8,job8,null,null,null,null,null,null },
                {job1,job3,job3,job6,job6,null,null,null,null,null,null,null,null,null,null }
            };

            Assert.Equal(expected, actual);
        }

    }
}
