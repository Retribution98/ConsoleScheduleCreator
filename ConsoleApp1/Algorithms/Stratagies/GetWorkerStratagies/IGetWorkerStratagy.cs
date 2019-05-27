using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;

namespace ConsoleScheduleCreator
{
    public interface IGetWorkerStratagy
    {
        Worker GetWorker(Job job, Plan plan, int time);
    }
}