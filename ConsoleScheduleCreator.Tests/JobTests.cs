using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests
{
    public class JobTests
    {
        [Fact]
        public void AddPrevios_Null_ArgumentNullException()
        {
            Job job = new Job("Test", 1, 0, 100, 10);

            Action testcode = () => job.AddPrevios(null);

            Assert.Throws<ArgumentNullException>(testcode);
        }

        [Fact]
        public void Ready_NotHavePrevios_True()
        {
            Job job = new Job("Test", 1, 0, 100, 10);

            bool actual = job.Ready(0);

            Assert.True(actual);
        }

        [Fact]
        public void Ready_notCompletedPrevios_False()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            Job previos = new Job("PreviosTest", 1, 0, 100, 10);
            job.AddPrevios(previos);

            bool actual = job.Ready(0);

            Assert.False(actual);
        }

        [Fact]
        public void Ready_CompletedPrevios_True()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            Job previos = new Job("PreviosTest", 1, 0, 100, 10);
            previos.Complete(0, 5);
            job.AddPrevios(previos);

            bool actual = job.Ready(10);

            Assert.True(actual);
        }

        [Fact]
        public void Ready_PreviosInProgress_False()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            job.Complete(0,10);

            bool actual = job.Ready(5);

            Assert.False(actual);
        }

        [Fact]
        public void Ready_Compleeted_False()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            job.Complete(0,5);

            bool actual = job.Ready(10);

            Assert.False(actual);
        }

        [Fact]
        public void Ready_TooEarlyTime_True()
        {
            Job job = new Job("Test", 1, 10, 100, 10);

            bool actual = job.Ready(0);

            Assert.False(actual);
        }

        [Fact]
        public void GetPenaltyForTime_0Time_0()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            Int64 actual = job.GetPenaltyForTime(0);
            Int64 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPenaltyForTime_5TimeLast_50()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            Int64 actual = job.GetPenaltyForTime(15);
            Int64 expected = 50;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPenaltyForTime_LastTime_0()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            Int64 actual = job.GetPenaltyForTime(10);
            Int64 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPenaltyForTime_CompletedBeforeLastTime_0()
        {
            Job job = new Job("Test", 1, 0, 10, 10);
            job.Complete(0,5);

            Int64 actual = job.GetPenaltyForTime(20);
            Int64 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPenaltyForTime_CompletedLaterLastTime_50()
        {
            Job job = new Job("Test", 1, 0, 10, 10);
            job.Complete(0,15);

            Int64 actual = job.GetPenaltyForTime(0);
            Int64 expected = 50;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Complete_BeforeLastTime_0()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            job.Complete(0, 5);
            Int64 actual = job.FinalPenalty;
            Int64 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Complete_5TimeLastTime_50()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            job.Complete(0, 15);
            Int64 actual = job.FinalPenalty;
            Int64 expected = 50;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Complete_LastTime_0()
        {
            Job job = new Job("Test", 1, 0, 10, 10);

            job.Complete(0, 10);
            Int64 actual = job.FinalPenalty;
            Int64 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Tostrnig_2Previos()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            Job previos1 = new Job("1PreviosTest", 2, 0, 100, 10);
            Job previos2 = new Job("2PreviosTest", 3, 0, 100, 10);
            job.AddPrevios(previos1);
            job.AddPrevios(previos2);

            string expected = "Id: 1\t Name: Test\t Раннее начало: 0\t Позднее окончание: 100\tПредшественники: 2, 3";
            string actual = job.ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Tostrnig_NotPrevios()
        {
            Job job = new Job("Test", 1, 0, 100, 10);
            
            string expected = "Id: 1\t Name: Test\t Раннее начало: 0\t Позднее окончание: 100\tПредшественники: отсутствуют";
            string actual = job.ToString();

            Assert.Equal(expected, actual);
        }
    }
}


