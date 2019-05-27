using DotNetGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public interface IPrinter
    {
        void Print(string msg);
        void PrintLn(string msg);
        void PrintGraph(DotGraph graph);
    }

    public interface IPrintable
    {
        void Print(IPrinter printer);
    }

    public class PrinterToConsole:IPrinter
    {
        public void Print(string msg)
        {
            Console.Write(msg);
        }

        public void PrintGraph(DotGraph graph)
        {
            throw new NotImplementedException();
        }

        public void PrintLn(string msg)
        {
            Console.WriteLine(msg);
        }
    }
    public class PrinterToDebug : IPrinter
    {
        public void Print(string msg)
        {
            Debug.Write(msg);
        }

        public void PrintGraph(DotGraph graph)
        {
            throw new NotImplementedException();
        }

        public void PrintLn(string msg)
        {
            Debug.WriteLine(msg);
        }
    }
    public class PrinterToFile : IPrinter, IDisposable
    {
        private readonly string _path;
        private readonly StreamWriter sw;

        public PrinterToFile(string path)
        {
            _path = path;
            sw = new StreamWriter(_path);
        }

        public void Dispose()
        {
            sw.Dispose();
        }

        public void Print(string msg)
        {
             sw.Write(msg);
        }

        public void PrintGraph(DotGraph graph)
        {
            var dot = graph.Compile();
            Print(dot);
        }

        public void PrintLn(string msg)
        {
            sw.WriteLine(msg);
        }
    }
}
