using Shields.GraphViz.Components;
using Shields.GraphViz.Models;
using Shields.GraphViz.Services;
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
        void PrintGraph(Graph graph, string name);
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

        public void PrintGraph(Graph graph, string name)
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

        public void PrintGraph(Graph graph, string name)
        {
            throw new NotImplementedException();
        }

        public void PrintLn(string msg)
        {
            Debug.WriteLine(msg);
        }
    }
    public class PrinterToFile : IPrinter
    {
        private readonly string _path;

        public PrinterToFile(string path)
        {
            _path = path;
        }

        public void Print(string msg)
        {
            using (var sw = new StreamWriter(_path))
            {
                sw.Write(msg);
            }
        }

        public void PrintGraph(Graph graph, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "graph";
            }
            var renderer = new Renderer("C:\\Users\\kiril\\Desktop\\Paterns Старостин\\ConsoleScheduleCreator\\graphviz\\bin\\");
            using (var file = File.Create(name+".png"))
            {
                renderer.RunAsync(
                    graph, file,
                    RendererLayouts.Dot,
                    RendererFormats.Png,
                    CancellationToken.None);
            }
        }

        public void PrintLn(string msg)
        {
            using (var sw = new StreamWriter(_path))
            {
                sw.Write(msg);
            }
        }
    }
}
