using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public struct Schedule : IPrintable
    {
        public Job[,] Planner { get; }
        public Int64 Penalty { get; }
        public Schedule(Job[,] planner, Int64 penalty)
        {
            this.Planner = planner;
            this.Penalty = penalty;
        }
        public void Print(IPrinter printer)
        {
            StringBuilder plan = new StringBuilder();
            for (int worker = 0; worker < Planner.GetLength(0); worker++)
            {
                for (int t = 0; t < Planner.GetLength(1); t++)
                    if (Planner[worker, t] != null)
                        plan.AppendFormat("{0} ", Planner[worker, t].Id.ToString());
                    else plan.AppendFormat("- ");
                plan.AppendLine();
            }
            printer.Print(plan + "\n Penalty: " + Penalty.ToString());
        }
    }
}
