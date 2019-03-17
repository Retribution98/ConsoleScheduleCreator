using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Entities
{
    public class Plan
    {
        private Job[,] _jobs;
        public int Time { get; }
        public List<Worker> Workers { get; }

        public Plan(List<Worker> workers, int time)
        {
            if (time > 0)
                Time = time;
            else throw new ArgumentException("Wrong time");
            Workers = new List<Worker>();
            foreach (var worker in workers)
            {
                if (Workers.Contains(worker))
                    continue;
                else Workers.Add(worker);
            }
            _jobs = new Job[Workers.Count, Time];
        }

        public Job this[Worker worker, int time]
        {
            get
            {
                return _jobs[Workers.IndexOf(worker), time];
            }
            set
            {
                _jobs[Workers.IndexOf(worker), time] = value;
            }
        }

        public void AppointJob(Job job, Worker worker, int time)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Appoint job in {time}: {job.Id}");
            Console.ResetColor();
            Console.WriteLine();
            var leadTime = worker.TimeOfWork[job.Id];
            //job.FinalPenalty = job.GetPenaltyForTime(time + leadTime - 1);
            job.Complete(time, time + leadTime - 1);
            worker.AddProcess(job.Id);
            for (var delta = 0; delta < leadTime; delta++)
            {
                this[worker, time + delta] = job;
            }
        }

        public bool HaveFreeWorker(int time)
        {
            for (int worker = 0; worker < Workers.Count; worker++)
            {
                if (_jobs[worker, time] == null)
                    return true;
            }
            return false;
        }
    }
}
