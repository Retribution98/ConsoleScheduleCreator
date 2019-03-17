using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;

namespace ConsoleScheduleCreator.Tests.AppointJobStratagyTests
{
    public class GreedyStratagyTest
    {
        [Fact]
        public void AppointJob_1Job_CompleteTrue()
        {
            var stratagy = new GetWorkerStratagy.GreedyStatagy();

            var job1 = new Job("Test", 1, 1, 10, 10);
            var plan = new Job[1, 5];

            var numAddProcess = 0;
            var worker = new Mock<Worker>();
            worker.SetupGet(w => w.Name).Returns("Vasya");
            worker.SetupGet(w => w.NumJob).Returns(1);
            worker.SetupGet(w => w.TimeOfWork).Returns(new Dictionary<uint, int> { { 1, 3 } });
            worker.SetupGet(w => w.TimeInProcess).Returns(0);
            worker.Setup(w => w.AddProcess(1)).Callback(() => numAddProcess++);

            var proj = new Mock<Project>();
            proj
                .SetupProperty(x => x.Workers, new List<Worker> { worker.Object })
                .SetupProperty(x => x.Jobs, new List<Job> { job1 });

            var actualPenalty = stratagy.AppointJob(proj.Object, plan, 1, job1);

            var expectedPenalty = 20L;

            Assert.True(job1.Completed);
            Assert.Equal(expectedPenalty, actualPenalty);
            Assert.Equal(1, numAddProcess);
            Assert.Equal(new Job[1, 5] {{ job1, job1, job1, null, null }}, plan);
        }

        [Fact]
        public void AppointJob_1Job_AddProcessForWorker()
        {
            var stratagy = new GetWorkerStratagy.GreedyStatagy();

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            var workersName = new string[3] { "Vasya", "Petya", "Vova" };
            var workersTime = new int[3, 1] { { 5 }, { 3 }, { 3 } };
            var proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            var plan = new Job[3, 5];

            var penalty = stratagy.AppointJob(proj, plan, 1, job1);

            int actual = proj.Workers[1].TimeInProcess;
            int expected = 3;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AppointJob_1Job_AddToPlan()
        {
            var stratagy = new GetWorkerStratagy.GreedyStatagy();

            var jobs = new List<Job>();
            var job1 = new Job("Test", 1, 1, 1, 10);
            jobs.Add(job1);
            var workersName = new string[1] { "Vasya" };
            var workersTime = new int[1, 1] { { 3 } };
            var proj = new Project("Test", 0, 100, jobs, workersName, workersTime);
            var plan = new Job[1, 5];

            var penalty = stratagy.AppointJob(proj, plan, 1, job1);

            var actual = plan;
            var expected = new Job[1, 5] { { null, job1, job1, job1, null } };

            Assert.Equal(expected, actual);
        }
    }
}
