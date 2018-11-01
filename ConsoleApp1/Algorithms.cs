using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public struct Schedule: IPrintable
    {
        public readonly uint[,] _planner;
        public readonly Int64 _penalty;
        public Schedule(uint[,] planner, Int64 penalty)
        {
            _planner = planner;
            _penalty = penalty;
        }
        public void Print(IPrinter printer)
        {
            StringBuilder plan = new StringBuilder();
            for (int worker = 0; worker < _planner.GetLength(0); worker++)
            {
                for (int t = 0; t < _planner.GetLength(1); t++)
                    plan.AppendFormat("{0} ", _planner[worker, t].ToString());
                plan.AppendLine();
            }
            printer.Print(plan + "\n Penalty: " + _penalty.ToString());
        }
    }

    public interface IAlgorithm
    {
        Schedule CreateShedule(Project proj);
    }

    public class FrontAlgorithm : IAlgorithm
    {
        readonly IFrontStratagy stratagyNextJob;
        public FrontAlgorithm(IFrontStratagy stratagy)
        {
            stratagyNextJob = stratagy;
        }

        List<Job> GetReadyJobs(List<Job> jobs, int time)
        {
            if (jobs == null) return null;
            List<Job> readyJobs = new List<Job>();
            foreach (Job job in jobs)
            {
                if (job.Ready(time))
                {
                    readyJobs.Add(job);
                }
            }
            return readyJobs;
        }
        Job GetNextJob(List<Job> jobs)
        {
            return stratagyNextJob.GetJob(jobs);
        }
        Int64 AppointJob(Project proj, uint[,] plan, int time, Job job)
        {
            //Соотношение между Работником и порядковым номером в проекте
            Dictionary<Worker, int> numerationWorkers = new Dictionary<Worker, int>();
            for (int index = 0; index < proj.Workers.Count; index++)
            {
                numerationWorkers.Add(proj.Workers[index], index);
            }

            var sortByTime = from worker in proj.Workers
                             where plan[numerationWorkers[worker], time]==0
                             orderby worker.TimeOfWork[job.Id], worker.TimeInProcess
                             select worker;

            if (sortByTime.Count() == 0) throw new OperationCanceledException("Haven't ready worker");
            Worker bestWorker = sortByTime.First();

            for (int l = 0; l < bestWorker.TimeOfWork[job.Id]; l++)                                      //Заполняем план для исполнителя с минимальным временм исполнения
                plan[numerationWorkers[bestWorker], time + l] = job.Id;
            job.Complete(time, time + bestWorker.TimeOfWork[job.Id] - 1);             //Выполняем работу с такой-то по такой такт
            bestWorker.AddProcess(job.Id);                                                            //Добавляем нагрузку на исполнителя

            return job.FinalPenalty;
        }
        bool HaveNotCompletedJob(Project proj)
        {
            foreach (Job job in proj.Jobs)
            {
                if (!job.Completed) return true;
            }
            return false;
        }
        public Schedule CreateShedule(Project proj)
        {
            //Определяем расписание как график Ганта
            uint[,] plan = new uint[proj.Workers.Count, proj.Late];
            Int64 penalty = 0;

            //Строим расписание
            for (int time=0; HaveNotCompletedJob(proj); time++)
            {
                //Создаем фронт работ в данный момент времени
                List<Job> front = GetReadyJobs(proj.Jobs, time);
                //Назанчаем работы из фронта
                while (front.Count != 0)
                {
                    Job nextJob = GetNextJob(front);
                    try
                    {
                        penalty += AppointJob(proj, plan, time, nextJob);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    
                    front = GetReadyJobs(proj.Jobs, time);
                    //TODO добавить проверку на доступность ресурсов
                }
            }

            return new Schedule(plan, penalty);
        }
    }
}
