using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Entities
{
    public class Plan
    {
        private IDictionary<Worker, IDictionary<int,Job>> _jobs;
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
            _jobs = new Dictionary<Worker, IDictionary<int, Job>>();
            foreach (var worker in Workers)
            {
                _jobs.Add(worker, new Dictionary<int, Job>());
            }
        }

        public Job this[Worker worker, int time]
        {
            get
            {
                return _jobs[worker].ContainsKey(time) 
                    ? _jobs[worker][time] 
                    : null;
            }
            set
            {
                if (_jobs[worker].ContainsKey(time))
                {
                    _jobs[worker][time] = value;
                }
                else
                {
                    _jobs[worker].Add(time, value);
                }
            }
        }

        public void AppointJob(Job job, Worker worker, int time)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write($"Appoint job in {time}: {job.Id}");
            Console.ResetColor();
            Console.WriteLine();
            var leadTime = worker.TimeOfWork[job];
            //job.FinalPenalty = job.GetPenaltyForTime(time + leadTime - 1);
            job.Complete(time, time + leadTime - 1);
            for (var delta = 0; delta < leadTime; delta++)
            {
                this[worker, time + delta] = job;
            }
        }

        public bool HaveFreeWorker(int time)
        {
            foreach(var worker in Workers)
            {
                if (!_jobs[worker].ContainsKey(time))
                    return true;
            }
            return false;
        }

        public int GetTimeInProcess(Worker worker)
        {
            return _jobs[worker].Count();
        }

        public IEnumerable<int> GetTimesJobExecute(Job job)
        {
            var listTime = new List<int>();
            foreach (var worker in Workers)
            {
                listTime.AddRange(_jobs[worker].Where(d => d.Value == job).Select(p => p.Key));
            }
            return listTime;
        }

    }
}
