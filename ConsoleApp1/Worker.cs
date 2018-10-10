using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public class Worker:IPrintable
    {
        //Свойства
        public string Name { get; }
        public int NumJob { get; }
        public int[] TimeOfWork { get; }
        public int TimeInProcess { get; private set; }

        //Методы
        public Worker(string _name, int _num_Job, int[] _time_of_work)      // Конструктор
        {
            Name = _name;
            NumJob = _num_Job;
            TimeOfWork = new int[NumJob];
            TimeInProcess = 0;
            for (int i = 0; i < NumJob; i++)
            {
                TimeOfWork = _time_of_work;
            }
        }
        public override string ToString()
        {
            StringBuilder msg = new StringBuilder("Имя: " + Name + "\n");
            for (int job = 0; job < NumJob; job++)
                msg.AppendFormat("№" + job + ": " + TimeOfWork[job] + "   ");
            return msg.ToString();
        }
        public void Print(IPrinter printer)
        {
            printer.Print(this.ToString());
        }
        public void AddProcess (int time)
        {
            TimeInProcess += time;
        }
    }

}
