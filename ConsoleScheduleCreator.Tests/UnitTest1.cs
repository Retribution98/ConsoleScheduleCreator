using ConsoleScheduleCreator.Algorithms;
using ConsoleScheduleCreator.Algorithms.SheduleClasses;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.Entities.Project;
using ConsoleScheduleCreator.GetWorkerStratagy;
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
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(getWorkerStratagy: new MinTimeOfWork(), nextJobStratagy: new NextJobStratagies.FirstlyStratagy()), new PrinterToConsole()));
            var actual = project.Schedule.Planner;
            var expected = new Job[,]
            {
                {job1,job1,job1,job2,job2,job3,job3,job3,null,null,job6,null,null,null,null,job8,job8,job8,null,null },
                {null,null,null,job4,job4,job4,null,null,job5,job5,null,null,null,null,null,null,null,null,null,null },
                {null,null,null,null,null,null,null,null,null,null,null,null,job7,job7,job7,null,null,null,null,null }
            };

            for (var i = 0; i < project.Workers.Count; i++)
            {
                var worker = project.Workers[i];
                for (var t = 0; t < project.Late; t++)
                    Assert.Equal(expected[i, t], actual[worker, t]);
            }
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
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(getWorkerStratagy: new MinTimeOfWork(), nextJobStratagy: new NextJobStratagies.FirstlyStratagy()), new PrinterToConsole()));

            var actual = project.Schedule.Planner;
            var expected = new Job[,]
            {
                {job1,job4,job4,job4,null,null,null,null,null,null,null,null,null,null,null },
                {job3,job3,job3,job3,job3,job3,job3,job3,job6,job6,job7,job7,job8,job8,null },
                {job2,job2,job2,job2,job5,job5,job5,null,null,null,null,null,null,null,null }
            };

            for (var i =0; i < project.Workers.Count; i++)
            {
                var worker = project.Workers[i];
                for (var t = 0; t < project.Late; t++)
                    Assert.Equal(expected[i,t], actual[worker, t]);
            }
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
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));

            var actual = project.Schedule.Planner;
            var expected = new Job[,]
            {
                {job2,job2,job2,null,null,null,null,null,null,null,null,null,null,null,null },
                {job4,null,null,job5,job5,job7,job7,job8,job8,null,null,null,null,null,null },
                {job1,job3,job3,job6,job6,null,null,null,null,null,null,null,null,null,null }
            };

            for (var i = 0; i < project.Workers.Count; i++)
            {
                var worker = project.Workers[i];
                for (var t = 0; t < project.Late; t++)
                    Assert.Equal(expected[i, t], actual[worker, t]);
            }
        }


        [Fact]
        public void CreateScheduleTests_JobsDirectiveTime_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 1, 5);
            Job job2 = new Job("Second", 2, 0, -1, 5);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 1, 5);
            job3.AddPrevios(job1);
            Job job4 = new Job("Fourth", 4, 0, 1, 10);
            job4.AddPrevios(job1);
            Job job5 = new Job("Fifth", 5, 2, -1, 5);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 2, 3, 7);
            job6.AddPrevios(job3);
            Job job7 = new Job("Seventh", 7, 4, 5, 10);
            job7.AddPrevios(job4);
            Job job8 = new Job("Eighth", 8, 5, 6, 5);
            job8.AddPrevios(job5);
            job8.AddPrevios(job6);
            job8.AddPrevios(job7);
            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };

            string[] workersName = { "Vasya", "Petya", "Vova" };
            int[,] workersTime =
            {
                { 2,2,3,3,2,2,1,1},
                { 2,2,3,2,2,2,2,2},
                { 1,1,1,1,1,1,1,1}
            };

            Project project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var specialShedule = project.Schedule;
            project.Reset();
            project.CreateSchedule(
                new FrontAlgorithm(
                    new JobsDirectiveTime(
                        getWorkerStratagy: new Algorithms.Stratagies.GetWorkerStratagy.FirstlyStratagy(),
                        nextJobStratagy: new NextJobStratagies.FirstlyStratagy()),
                    new PrinterToConsole()));
            var defaultSchedule = project.Schedule;

            //var expected = new Job[,]
            //{
            //    {job2,job2,job2,null,null,null,null,null,null,null,null,null,null,null,null },
            //    {job4,null,null,job5,job5,job7,job7,job8,job8,null,null,null,null,null,null },
            //    {job1,job3,job3,job6,job6,null,null,null,null,null,null,null,null,null,null }
            //};

            Assert.True(specialShedule.Penalty < defaultSchedule.Penalty);
        }

        [Fact]
        public void CreateScheduleTests_EqualWorker_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 1, 5);
            Job job2 = new Job("Second", 2, 0, -1, 5);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 1, 5);
            job3.AddPrevios(job1);
            Job job4 = new Job("Fourth", 4, 0, 1, 10);
            job4.AddPrevios(job1);
            Job job5 = new Job("Fifth", 5, 2, -1, 5);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 2, 3, 7);
            job6.AddPrevios(job3);
            Job job7 = new Job("Seventh", 7, 4, 5, 10);
            job7.AddPrevios(job4);
            Job job8 = new Job("Eighth", 8, 5, 6, 5);
            job8.AddPrevios(job5);
            job8.AddPrevios(job6);
            job8.AddPrevios(job7);
            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8 };

            string[] workersName = { "Vasya", "Petya", "Vova" };
            int[,] workersTime =
            {
                { 2,2,3,3,2,2,1,1},
                { 2,2,3,2,2,2,2,2},
                { 1,1,1,1,1,1,1,1}
            };

            Project project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new EqualWorker(), new PrinterToConsole()));
            var specialShedule = project.Schedule;
            project.Reset();
            project.CreateSchedule(
                new FrontAlgorithm(
                    new EqualWorker(
                        getWorkerStratagy: new Algorithms.Stratagies.GetWorkerStratagy.FirstlyStratagy(),
                        nextJobStratagy: new NextJobStratagies.FirstlyStratagy()),
                    new PrinterToConsole()));
            var defaultSchedule = project.Schedule;

            Assert.True(specialShedule.Penalty < defaultSchedule.Penalty);
        }

        [Fact]
        public void CreateScheduleTests_MultiAlgorithm_JobsDirectiveTime()
        {
            Job job1 = new Job("First", 1, 0, 10, 2);
            Job job2 = new Job("Second", 2, 0, 2, 2);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 4, 2);
            job3.AddPrevios(job2);
            Job job4 = new Job("Fourth", 4, 0, 6, 2);
            job4.AddPrevios(job3);
            Job job5 = new Job("Fifth", 5, 0, 8, 2);
            job5.AddPrevios(job4);
            Job job6 = new Job("Sixth", 6, 0, 2, 6);
            job6.AddPrevios(job1);
            Job job7 = new Job("Seventh", 7, 0, 4, 6);
            job7.AddPrevios(job6);
            Job job8 = new Job("Eighth", 8, 0, 6, 6);
            job8.AddPrevios(job7);
            Job job9 = new Job("Ninth", 9, 0, 8, 6);
            job9.AddPrevios(job8);
            Job job10 = new Job("Tenth", 10, 0, 10, 6);
            job10.AddPrevios(job1);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8, job9, job10 };
            string[] workersName = { "Vasya", "Petya" };

            int[,] workersTime =
            {
                { 1,2,2,2,2,2,2,2,2,1},
                { 1,2,2,2,2,2,2,2,2,1}
            };

            Project project = new Project("Test", 0, 10, jobs, workersName, workersTime);
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var actual = project.Schedule.Planner;
            var expected = new Job[,]
            {
                {job1,job2,job2,job3,job3,job4,job4,job5,job5,job10 },
                {null,job6,job6,job7,job7,job8,job8,job9,job9,null }
            };

            for (var i = 0; i < project.Workers.Count; i++)
            {
                var worker = project.Workers[i];
                for (var t = 0; t < project.Late; t++)
                    Assert.Equal(expected[i, t], actual[worker, t]);
            }
        }

        [Fact]
        public void CreateScheduleTests_MultiAlgorithm_JobsDirectiveTime_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 10, 2);
            Job job2 = new Job("Second", 2, 0, 2, 2);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 4, 2);
            job3.AddPrevios(job2);
            Job job4 = new Job("Fourth", 4, 0, 6, 2);
            job4.AddPrevios(job3);
            Job job5 = new Job("Fifth", 5, 0, 8, 2);
            job5.AddPrevios(job4);
            Job job6 = new Job("Sixth", 6, 0, 2, 6);
            job6.AddPrevios(job1);
            Job job7 = new Job("Seventh", 7, 0, 4, 6);
            job7.AddPrevios(job6);
            Job job8 = new Job("Eighth", 8, 0, 6, 6);
            job8.AddPrevios(job7);
            Job job9 = new Job("Ninth", 9, 0, 8, 6);
            job9.AddPrevios(job8);
            Job job10 = new Job("Tenth", 10, 0, 10, 6);
            job10.AddPrevios(job1);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8, job9, job10 };
            string[] workersName = { "Vasya", "Petya" };

            int[,] workersTime =
            {
                { 1,2,2,2,2,2,2,2,2,1},
                { 1,2,2,2,2,2,2,2,2,1}
            };

            Project project = new Project("Test", 0, 10, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var multiAlgorithmSchedule = project.Schedule;

            Assert.True(multiAlgorithmSchedule.Penalty < singleAlgorithmSchedule.Penalty);
        }

        [Fact]
        public void CreateScheduleTests_MultiAlgorithm_EqualWorker_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 10, 2);
            Job job2 = new Job("Second", 2, 0, 2, 2);
            Job job3 = new Job("Third", 3, 0, 4, 2);
            job3.AddPrevios(job1);
            Job job4 = new Job("Fourth", 4, 0, 6, 2);
            job4.AddPrevios(job1);
            Job job5 = new Job("Fifth", 5, 0, 8, 2);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 0, 2, 6);
            job6.AddPrevios(job3);
            Job job7 = new Job("Seventh", 7, 0, 4, 6);
            job7.AddPrevios(job4);
            Job job8 = new Job("Eighth", 8, 0, 6, 6);
            job8.AddPrevios(job5);
            job8.AddPrevios(job7);
            Job job9 = new Job("Ninth", 9, 0, 8, 6);
            job9.AddPrevios(job6);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8, job9};
            string[] workersName = { "Vasya", "Petya", "Vova" };

            int[,] workersTime =
            {
                { 2,3,3,2,4,3,2,4,3},
                { 2,3,3,3,3,3,2,4,3},
                { 2,3,2,4,2,2,2,2,4}
            };

            var project = new Project("Test", 0, 10, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new EqualWorker(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            int numTrue = 0;
            for (var i=0; i < 100; i++)
            {
                project.MultiAlgorihm(new FrontAlgorithm(new EqualWorker(), new PrinterToConsole()));
                 if (project.Schedule.Penalty < singleAlgorithmSchedule.Penalty)
                {
                    numTrue++;
                }
                project.Reset();
            }
            
            Assert.True(numTrue > 15);
        }

        [Fact]
        public void CreateScheduleTests_ProjectComposite_MultiAlgorithm_EqualWorker_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 10, 2);
            Job job2 = new Job("Second", 2, 0, 2, 2);
            Job job3 = new Job("Third", 3, 0, 4, 2);
            job3.AddPrevios(job1);
            Job job4 = new Job("Fourth", 4, 0, 6, 2);
            job4.AddPrevios(job1);
            Job job5 = new Job("Fifth", 5, 0, 8, 2);
            job5.AddPrevios(job2);
            Job job6 = new Job("Sixth", 6, 0, 2, 6);
            job6.AddPrevios(job3);
            Job job7 = new Job("Seventh", 7, 0, 4, 6);
            job7.AddPrevios(job4);
            Job job8 = new Job("Eighth", 8, 0, 6, 6);
            job8.AddPrevios(job5);
            job8.AddPrevios(job7);
            Job job9 = new Job("Ninth", 9, 0, 8, 6);
            job9.AddPrevios(job6);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8, job9 };
            string[] workersName = { "Vasya", "Petya", "Vova" };

            int[,] workersTime =
            {
                { 2,3,3,2,4,3,2,4,3},
                { 2,3,3,3,3,3,2,4,3},
                { 2,3,2,4,2,2,2,2,4}
            };

            var projectProrotype = new Project("Test", 0, 10, jobs, workersName, workersTime);
            var project = new ProjectComposite(
                new List<IProject> {
                    projectProrotype,
                    projectProrotype.Clone() as IProject,
                    projectProrotype.Clone() as IProject
                });
            project.CreateSchedule(new FrontAlgorithm(new EqualWorker(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            int numTrue = 0;
            for (var i = 0; i < 100; i++)
            {
                project.MultiAlgorihm(new FrontAlgorithm(new EqualWorker(), new PrinterToConsole()));
                if (project.Schedule.Penalty < singleAlgorithmSchedule.Penalty)
                {
                    numTrue++;
                }
                project.Reset();
            }

            Assert.True(numTrue > 15);
        }

        [Fact]
        public void CreateScheduleTests_ProjectRepeater_MultiAlgorithm_JobsDirectiveTime_EffectiveTest()
        {
            Job job1 = new Job("First", 1, 0, 10, 2);
            Job job2 = new Job("Second", 2, 0, 2, 2);
            job2.AddPrevios(job1);
            Job job3 = new Job("Third", 3, 0, 4, 2);
            job3.AddPrevios(job2);
            Job job4 = new Job("Fourth", 4, 0, 6, 2);
            job4.AddPrevios(job3);
            Job job5 = new Job("Fifth", 5, 0, 8, 2);
            job5.AddPrevios(job4);
            Job job6 = new Job("Sixth", 6, 0, 2, 6);
            job6.AddPrevios(job1);
            Job job7 = new Job("Seventh", 7, 0, 4, 6);
            job7.AddPrevios(job6);
            Job job8 = new Job("Eighth", 8, 0, 6, 6);
            job8.AddPrevios(job7);
            Job job9 = new Job("Ninth", 9, 0, 8, 6);
            job9.AddPrevios(job8);
            Job job10 = new Job("Tenth", 10, 0, 10, 6);
            job10.AddPrevios(job1);

            List<Job> jobs = new List<Job>() { job1, job2, job3, job4, job5, job6, job7, job8, job9, job10 };
            string[] workersName = { "Vasya", "Petya" };

            int[,] workersTime =
            {
                { 1,2,2,2,2,2,2,2,2,1},
                { 1,2,2,2,2,2,2,2,2,1}
            };

            var projectProtorype = new Project("Test", 0, 10, jobs, workersName, workersTime);
            var project = new ProjectRepeater(projectProtorype, 100);
            project.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDirectiveTime(), new PrinterToConsole()));
            var multiAlgorithmSchedule = project.Schedule;

            Assert.True(multiAlgorithmSchedule.Penalty < singleAlgorithmSchedule.Penalty);
        }
    }
}
