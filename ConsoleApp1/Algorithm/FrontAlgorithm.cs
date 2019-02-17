using ScheduleApp.DataAccess.DTO;
using ScheduleCreator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleCreator
{
    public class FrontAlgorithm : IAlgorithm
    {
        readonly IFrontStratagy stratagyNextJob;
        public FrontAlgorithm(IFrontStratagy stratagy)
        {
            stratagyNextJob = stratagy;
        }

        private List<JobDto> GetReadyJobs(List<JobDto> jobs, int time)
        {
            if ((jobs == null) || (jobs.Any()))
                return new List<JobDto>();
            var readyJobs = new List<JobDto>();
            foreach (var job in jobs)
            {
                if (job.IsReady(time))
                {
                    readyJobs.Add(job);
                }
            }
            return readyJobs;
        }

        private JobDto GetNextJob(List<JobDto> jobs)
        {
            return stratagyNextJob.GetJob(jobs);
        }

        private UserDto AppointJob(ProjectDto proj, Plan plan, int time, JobDto job)
        {
            var workers = from worker in proj.Workers
                          where plan[worker, time] == null
                          orderby worker.Qualifications.Where(q => q.JobType == job.JobType).Select(q => q.EffectivePercent)
                          select worker;

            if (!workers.Any())
                return null;
            var bestWorker = workers.First();
            var timeWork = bestWorker.Qualifications.Where(q => q.JobType == job.JobType).Select(q => q.EffectivePercent).FirstOrDefault() * job.LeadTime;
            var numPeriods = GetNumPeriods(timeWork, plan.TimeUnit);
            for (int l = 0; l < numPeriods; l++)                                      //Заполняем план для исполнителя с минимальным временм исполнения
                plan[bestWorker, time + l] = job;

            return bestWorker;
        }

        private bool HaveNotCompletedJob(ProjectDto proj)
        {
            foreach (var job in proj.Jobs)
            {
                if (!job.TimeEnd.HasValue) return true;
            }
            return false;
        }

        private TimeSpan SetTime(PeriodUnit periodUnit, int numPeriod)
        {
            switch (periodUnit)
            {
                case PeriodUnit.Milliseconds:
                    return TimeSpan.FromMilliseconds(numPeriod);
                case PeriodUnit.Seconds:
                    return TimeSpan.FromSeconds(numPeriod);
                case PeriodUnit.Minutes:
                    return TimeSpan.FromMinutes(numPeriod);
                case PeriodUnit.Hours:
                    return TimeSpan.FromHours(numPeriod);
                case PeriodUnit.Days:
                    return TimeSpan.FromDays(numPeriod);
                default:
                    throw new NotImplementedException("This periodUnit not implemented");
            }
        }

        private int GetNumPeriods(TimeSpan time, PeriodUnit unit)
        {
            switch (unit)
            {
                case PeriodUnit.Milliseconds:
                    return time.Milliseconds;
                case PeriodUnit.Seconds:
                    return time.Seconds;
                case PeriodUnit.Minutes:
                    return time.Minutes;
                case PeriodUnit.Hours:
                    return time.Hours;
                case PeriodUnit.Days:
                    return time.Days;
                default:
                    throw new NotImplementedException("Schedule for this periodUnit not implemented");
            }
        }

        public Schedule CreateShedule(ProjectDto proj, DateTime timeStart, DateTime timeEnd, PeriodUnit periodUnit)
        {
            if (proj == null) throw new ArgumentNullException("Project is null");
            if (timeEnd <= timeStart) throw new ArgumentException("EarlyTime more LateTime");

            var employmentWorker = new Dictionary<UserDto, int>();
            foreach (var worker in proj.Workers)
            {
                employmentWorker[worker] = 0;
            }

            var timeSpan = timeEnd - timeStart;
            int numPeriods = GetNumPeriods(timeSpan, periodUnit);
            

            //Определяем расписание как график Ганта
            var plan = new Plan(proj.Workers, periodUnit, numPeriods);

            //Строим расписание
            for (int period = 0; HaveNotCompletedJob(proj) && period < numPeriods; period++)
            {
                //Создаем фронт работ в данный момент времени
                List<JobDto> front = GetReadyJobs(proj.Jobs, period);
                //Назанчаем работы из фронта
                while (front.Count != 0)
                {
                    var nextJob = GetNextJob(front);
                    var worker = AppointJob(proj, plan, period, nextJob);
                    if (worker == null)
                        break;
                    else
                    {
                        nextJob.TimeStart = timeStart + SetTime(periodUnit, period);
                        var timeWork = worker.Qualifications.Where(q => q.JobType == nextJob.JobType).Select(q => q.EffectivePercent).FirstOrDefault() * nextJob.LeadTime;
                        nextJob.TimeEnd = nextJob.TimeStart + timeWork;
                        employmentWorker[worker] += GetNumPeriods(timeWork, periodUnit);                                                            //Добавляем нагрузку на исполнителя
                    }

                    front = GetReadyJobs(proj.Jobs, period);
                    //TODO добавить проверку на доступность ресурсов
                }
            }

            return new Schedule(plan);
        }
    }
}
