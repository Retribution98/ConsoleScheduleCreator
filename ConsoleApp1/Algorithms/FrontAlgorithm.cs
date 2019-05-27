using ConsoleScheduleCreator.Algorithms.SheduleClasses;
using ConsoleScheduleCreator.Entities;
using ConsoleScheduleCreator.NextJobStratagies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms
{
    public class FrontAlgorithm : IAlgorithm
    {
        private readonly ICriterion _criterion;
        private readonly IPrinter _printer;

        public FrontAlgorithm(ICriterion sheduleClass, IPrinter printer = null)
        {
            _criterion = sheduleClass;
            _printer = printer ?? new PrinterToDebug();
        }

        private bool HaveDidntCompleteJob(IEnumerable<Job> jobs)
        {
            foreach (Job job in jobs)
            {
                if (!job.IsCompleted) return true;
            }
            return false;
        }

        public Schedule CreateShedule(IProject proj)
        {
            //Определяем расписание как график Ганта
            var plan = new Plan(proj.Workers, proj.Late);

            //Строим расписание
            for (int time = 0; HaveDidntCompleteJob(proj.Jobs); time++)
            {
                //Создаем фронт работ в данный момент времени
                Front front = new Front(proj.Jobs, time);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                _printer.Print($"Front in {time}: ");
                _printer.PrintLn(String.Join(", ", front.Jobs.Select(j => j.Id)));
                Console.ResetColor();
                //Назанчаем работы из фронта
                while ((front.Jobs.Count != 0) && plan.HaveFreeWorker(time))
                {
                    var job = front.GetNextJob(_criterion);
                    var worker = _criterion.GetWorker(job, plan, time);
                    plan.AppointJob(job, worker, time);
                    //нельзя просто удалить установившуюся работу, так как могут появиться новые работы во фронте
                    front = new Front(proj.Jobs, time);
                }
            }
            var penalty = _criterion.GetCriterion(proj, plan);
            proj.Reset();
            return new Schedule(plan, penalty);
        }

        public Schedule MultiAlgorihm(IProject proj)
        {
            Schedule newSchedule, schedule;
            if (proj.Schedule.Planner == null)
            {
                schedule = CreateShedule(proj);
                newSchedule = schedule;
            }
            else
            {
                schedule = proj.Schedule;
                newSchedule = proj.Schedule;
            }
            do
            {
                schedule = newSchedule;
                
                _criterion.ModifyProject(proj, schedule.Planner);
                proj.Reset();
                newSchedule = CreateShedule(proj);
                _printer.PrintLn("//////////");
                newSchedule.Print(_printer);
                _printer.PrintLn("\\\\\\\\\\");
            }
            while (schedule.Penalty > newSchedule.Penalty);
            proj.Reset();
            return schedule;
        }
    }
}
