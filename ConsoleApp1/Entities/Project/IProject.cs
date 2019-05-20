using ConsoleScheduleCreator.Algorithms;
using System;
using System.Collections.Generic;

namespace ConsoleScheduleCreator.Entities
{
    public interface IProject: ICloneable
    {
        string Name { get; }
        int Early { get; }
        int Late { get; }
        List<Job> Jobs { get; }
        List<Worker> Workers { get; }
        Schedule Schedule { get; set;  }

        void AddJob(Job newJob, Dictionary<Worker,int> timeOfWorker);

        void CreateSchedule(IAlgorithm algorithm);
        void MultiAlgorihm(IAlgorithm algorithm);
        void Reset();
        void PrintTimeLine(IPrinter printer);
    }
}
