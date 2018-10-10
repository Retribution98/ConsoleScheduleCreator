using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public interface IPrinter
    {
        void Print(string msg);
    }
    public interface IPrintable
    {
        void Print(IPrinter printer);
    }

    public class PrinterToConsole:IPrinter
    {
        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
