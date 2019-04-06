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
        private readonly ISheduleClass _sheduleClass;
        private readonly IPrinter _printer;

        public FrontAlgorithm(ISheduleClass sheduleClass, IPrinter printer)
        {
            _sheduleClass = sheduleClass;
            _printer = printer;
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
                    var job = front.GetNextJob(_sheduleClass);
                    var worker = _sheduleClass.GetWorker(job, plan, time);
                    plan.AppointJob(job, worker, time);
                    front.RemoveJob(job);
                }
            }
            var penalty = _sheduleClass.GetPenalty(proj, plan);
            return new Schedule(plan, penalty);
        }

        public Schedule MultiAlgorihm(IProject proj)
        {
            var schedule = CreateShedule(proj);
            var newSchedule = schedule;
            do
            {
                schedule = newSchedule;
                
                _sheduleClass.ModifyProject(proj);
                proj.Reset();
                newSchedule = CreateShedule(proj);
                _printer.PrintLn("//////////");
                newSchedule.Print(_printer);
                _printer.PrintLn("\\\\\\\\\\");
            }
            while (schedule.Penalty > newSchedule.Penalty);
            return schedule;
        }
    }
}
