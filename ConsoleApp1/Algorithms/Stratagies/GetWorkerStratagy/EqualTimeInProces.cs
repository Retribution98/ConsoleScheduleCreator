﻿using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms.Stratagies.GetWorkerStratagy
{
    class EqualTimeInProces: IGetWorkerStratagy
    {
        public Worker GetWorker(Job job, Plan plan, int time)
        {
            var freestWorkers = plan.Workers
                .Where(w => plan[w, time] == null)
                .OrderBy(w => w.TimeInProcess);
            var chanceArray = new List<Worker>();
            foreach (var worker in freestWorkers)
            {
                for (var i = 0; i<worker.Priority; i++)
                {
                    chanceArray.Add(worker);
                }
            }
            var random = new Random();
            return chanceArray
                .Skip(random.Next(chanceArray.Count))
                .Take(1)
                .FirstOrDefault();
        }
    }
}
