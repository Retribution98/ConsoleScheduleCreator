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
        public Dictionary<uint,int> TimeOfWork { get; }
        public int Priority { get; set; }                   // Приоритет для многопроходного алгоритма

        //Методы
        public Worker(string _name, uint[] id_jobs, int[] time_of_work)      // Конструктор
        {
            if (id_jobs.Length != time_of_work.Length) throw new ArgumentException("Length array id_jobs and time_of_work are different");
            Name = _name;
            NumJob = id_jobs.Length;
            TimeOfWork = new Dictionary<uint, int>();

            for (int i=0; i<NumJob; i++)
            {
                TimeOfWork.Add(id_jobs[i], time_of_work[i]);
            }
        }
        public override string ToString()
        {
            StringBuilder msg = new StringBuilder($"Имя: {Name}\n");
            foreach (KeyValuePair<uint,int> duo in TimeOfWork)
                msg.Append($"№{duo.Key}: {duo.Value}   ");
            return msg.ToString();
        }
        public void Print(IPrinter printer)
        {
            printer.PrintLn(this.ToString());
        }
    }

}
