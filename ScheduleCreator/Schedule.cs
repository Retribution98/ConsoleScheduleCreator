using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleCreator
{
    public class Schedule
    {
        public Plan Plan { get; }
        public Int64 Penalty { get; }

        public Schedule(Plan plan)
        {
            Plan = plan;
            Penalty = GetPenalty(plan);
        }
        
        public static Int64 GetPenalty(Plan plan)
        {
            throw new NotImplementedException();
        }
        //public void Print(IPrinter printer)
        //{
        //    StringBuilder plan = new StringBuilder();
        //    for (int worker = 0; worker < _planner.GetLength(0); worker++)
        //    {
        //        for (int t = 0; t < _planner.GetLength(1); t++)
        //            plan.AppendFormat("{0} ", _planner[worker, t].ToString());
        //        plan.AppendLine();
        //    }
        //    printer.Print(plan + "\n Penalty: " + _penalty.ToString());
        //}
    }
}
