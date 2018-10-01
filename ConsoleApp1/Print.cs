﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    interface IPrinter
    {
        void Print(string msg);
    }
    interface IPrintable
    {
        void Print(IPrinter printer);
    }

    class PrinterToConsole:IPrinter
    {
        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
