﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public struct Schedule: IPrintable
    {
        public readonly Job[,] _planner;
        public readonly Int64 _penalty;
        public Schedule(Job[,] planner, Int64 penalty)
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
                    if (_planner[worker, t] != null)
                        plan.AppendFormat("{0} ", _planner[worker, t].Id.ToString());
                else plan.AppendFormat("- ");
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
        class Front
        {
            public List<Job> Jobs { get;  }

            public Front(List<Job> jobs, int time)
            {
                if (jobs != null)
                {
                    Jobs = new List<Job>();
                    foreach (Job job in jobs)
                    {
                        if (job.Ready(time))
                        {
                            Jobs.Add(job);
                        }
                    }
                }
            }
            public Job GetNextJob(IFrontStratagy stratagyNextJob)
            {
                return stratagyNextJob.GetJob(Jobs);
            }
        }

        readonly IFrontStratagy stratagyNextJob;

        public FrontAlgorithm(IFrontStratagy stratagy)
        {
            stratagyNextJob = stratagy;
        }
        
        Int64 AppointJob(Project proj, Job[,] plan, int time, Job job)
        {
            //Соотношение между Работником и порядковым номером в проекте
            Dictionary<Worker, int> numerationWorkers = new Dictionary<Worker, int>();
            for (int index = 0; index < proj.Workers.Count; index++)
            {
                numerationWorkers.Add(proj.Workers[index], index);
            }

            var sortByTime = from worker in proj.Workers
                             where plan[numerationWorkers[worker], time]==null
                             orderby worker.TimeOfWork[job.Id], worker.TimeInProcess
                             select worker;

            if (sortByTime.Count() == 0) throw new OperationCanceledException("Haven't ready worker");
            Worker bestWorker = sortByTime.First();

            for (int l = 0; l < bestWorker.TimeOfWork[job.Id]; l++)                                      //Заполняем план для исполнителя с минимальным временм исполнения
                plan[numerationWorkers[bestWorker], time + l] = job;
            job.Complete(time, time + bestWorker.TimeOfWork[job.Id] - 1);             //Выполняем работу с такой-то по такой такт
            bestWorker.AddProcess(job.Id);                                                            //Добавляем нагрузку на исполнителя

            return job.FinalPenalty;
        }
        bool HaveDidntCompleteJob(Project proj)
        {
            foreach (Job job in proj.Jobs)
            {
                if (!job.Completed) return true;
            }
            return false;
        }
        bool HaveFreeWorker (Job[,] plan, int time)
        {
            for (int worker=0; worker<plan.GetLength(0); worker++)
            {
                if (plan[worker, time] == null)
                    return true;
            }
            return false;
        }
        public Schedule CreateShedule(Project proj)
        {
            //Определяем расписание как график Ганта
            Job[,] plan = new Job[proj.Workers.Count, proj.Late];
            Int64 penalty = 0;

            //Строим расписание
            for (int time=0; HaveDidntCompleteJob(proj); time++)
            {
                //Создаем фронт работ в данный момент времени
                Front front = new Front(proj.Jobs, time);
                //Назанчаем работы из фронта
                while ((front.Jobs.Count != 0) && HaveFreeWorker(plan, time))
                {
                    try
                    {
                        penalty += AppointJob(proj, plan, time, front.GetNextJob(stratagyNextJob));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    
                    front = new Front(proj.Jobs, time);
                }
            }

            return new Schedule(plan, penalty);
        }
    }
}