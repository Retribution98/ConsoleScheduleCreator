using ConsoleScheduleCreator.Algorithms;
using ConsoleScheduleCreator.Entities.Project;
using System;

namespace ConsoleScheduleCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var proj = Project.Open(@"D:\GIT\ConsoleScheduleCreator\Data.xlsx");
            if (proj == null)
            {
                System.GC.Collect();
                Console.Read();
                return;
            }

            PrinterToConsole printerToConsole = new PrinterToConsole();
            proj.Print(printerToConsole);
            proj.CreateSchedule(new FrontAlgorithm(new JobsDirectiveTime(), printerToConsole));
            proj.Schedule.Print(printerToConsole);
            Console.WriteLine("_______________________________________________________");
            proj.Reset();
            proj.MultiAlgorihm(new FrontAlgorithm(new JobsDirectiveTime(), printerToConsole));
            proj.Schedule.Print(printerToConsole);

            Console.Read();
        }
 
    }
}
