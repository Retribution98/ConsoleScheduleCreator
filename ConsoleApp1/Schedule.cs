using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public struct Schedule : IPrintable
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
}
