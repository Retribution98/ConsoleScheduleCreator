using ConsoleScheduleCreator.Algorithms;
using ConsoleScheduleCreator.Algorithms.SheduleClasses;
using ConsoleScheduleCreator.Algorithms.Stratagies.ModifyStratagy;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.Entities.Project;
using ConsoleScheduleCreator.GetWorkerStratagy;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class AnalisisTest
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

            var project = new Project("Test", 0, 20, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(new MinTimeOfWork(), new NextJobStratagies.FirstlyStratagy(), new PenaltyStratagy()), new PrinterToConsole()));
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

            var project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(new MinTimeOfWork(), new NextJobStratagies.FirstlyStratagy(), new PenaltyStratagy()), new PrinterToConsole()));

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

            var project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));

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

            var project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
            var specialShedule = project.Schedule;
            project.Reset();
            project.CreateSchedule(
                new FrontAlgorithm(
                    new JobsDelayMinimization(
                        new Algorithms.Stratagies.GetWorkerStratagy.FirstlyStratagy(),
                        new NextJobStratagies.FirstlyStratagy(),
                        new PenaltyStratagy()),
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

            var project = new Project("Test", 0, 15, jobs, workersName, workersTime);
            project.CreateSchedule(new FrontAlgorithm(new EqualWorkload(), new PrinterToConsole()));
            var specialShedule = project.Schedule;
            project.Reset();
            project.CreateSchedule(
                new FrontAlgorithm(
                    new EqualWorkload(
                        new Algorithms.Stratagies.GetWorkerStratagy.FirstlyStratagy(),
                        new NextJobStratagies.FirstlyStratagy(),
                        new CriticalWorkersStratagy()),
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

            var project = new Project("Test", 0, 10, jobs, workersName, workersTime);
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
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
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
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
            project.CreateSchedule(new FrontAlgorithm(new EqualWorkload(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            int numTrue = 0;
            for (var i=0; i < 100; i++)
            {
                project.MultiAlgorihm(new FrontAlgorithm(new EqualWorkload(), new PrinterToConsole()));
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
            project.CreateSchedule(new FrontAlgorithm(new EqualWorkload(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            project.Reset();
            int numTrue = 0;
            for (var i = 0; i < 100; i++)
            {
                project.MultiAlgorihm(new FrontAlgorithm(new EqualWorkload(), new PrinterToConsole()));
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
            var job1 = new Job("1", 1, 0, 4, 5);
            var job2 = new Job("2", 2, 0, 4, 5);
            job2.AddPrevios(job1);
            var job3 = new Job("3", 3, 0, 4, 5);
            job3.AddPrevios(job1);
            var job4 = new Job("4", 4, 0, 4, 5);
            job4.AddPrevios(job3);
            var job5 = new Job("5", 5, 0, 4, 5);
            job5.AddPrevios(job2);
            job5.AddPrevios(job4);
            var job6 = new Job("6", 6, 0, 3, 5);
            var job7 = new Job("7", 7, 0, 3, 5);
            job7.AddPrevios(job6);
            var job8 = new Job("8", 8, 0, 3, 5);
            job8.AddPrevios(job6);
            var job9 = new Job("9", 9, 0, 3, 5);
            job9.AddPrevios(job6);
            var job10 = new Job("10", 10, 0, 3, 5);
            job10.AddPrevios(job8);
            job10.AddPrevios(job9);

            var timeOfWork = new Dictionary<Job, int?>
            {
                {job1, 1 },
                {job2, 2 },
                {job3, 1 },
                {job4, 1 },
                {job5, 1 },
                {job6, 2 },
                {job7, 1 },
                {job8, 1},
                {job9, 1 },
                {job10, 1 }
            };
            var worker1 = new Worker("vova", timeOfWork);
            var worker2 = new Worker("petya", timeOfWork);
            var worker3 = new Worker("kolya", timeOfWork);
            var project1 = new Project(
                "first", 0, 4, 
                new List<Job> { job1, job2, job3, job4, job5 }, 
                new List<Worker> { worker1, worker2, worker3 });
            var project2 = new Project(
                "second", 0, 3,
                new List<Job> { job6, job7, job8, job9, job10 },
                new List<Worker> { worker1, worker2, worker3 });

            var project = new ProjectComposite(new List<IProject> { project1, project2 });
            project.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
            var singleAlgorithmSchedule = project.Schedule;
            var printer = new PrinterToDebug();
            var printerToFile = new PrinterToFile("TESTVIZ");
            project.PrintTimeLine(printer);
            project.MultiAlgorihm(new FrontAlgorithm(new JobsDelayMinimization(), new PrinterToConsole()));
            var multiAlgorithmSchedule = project.Schedule;
            project.PrintTimeLine(printer);
            project.PrintGraph(printerToFile);
            Assert.True(multiAlgorithmSchedule.Penalty < singleAlgorithmSchedule.Penalty);
        }
    }
}
