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
            proj.CreateSchedule(new FrontAlgorithm(new GredyStratagy()));
            proj.Schedule.Print(printerToConsole);
            
            Console.Read();
        }
 
    }
}
