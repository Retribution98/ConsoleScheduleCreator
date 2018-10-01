using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleScheduleCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Project proj = Project.Open(@"D:\GIT\ConsoleScheduleCreator\Data.xlsx");
            if (proj == null)
            {
                System.GC.Collect();
                Console.Read();
                return;
            }
            PrinterToConsole printerToConsole = new PrinterToConsole();
            proj.Print(printerToConsole);

            int time = proj.Late;
            int[,] plan = proj.CreateSchedule(time);
            for (int worker = 0; worker < proj.Workers.Count; worker++)
            {
                for (int t = 0; t < time; t++)
                    Console.Write(plan[worker, t] + " ");
                Console.WriteLine();
            }
            Console.WriteLine("Полученный штраф: " + proj.PenaltyProject(time));
            System.GC.Collect();
            Console.Read();
        }
 
    }
}
