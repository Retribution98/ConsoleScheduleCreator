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
        public IDictionary<Job,int> TimeOfWork { get; }
        public int Priority { get; set; }                   // Приоритет для многопроходного алгоритма

        //Методы
        public Worker(string _name, IDictionary<Job, int> time_of_work)      // Конструктор
        {
            Name = _name;
            TimeOfWork = time_of_work;
        }

        public override string ToString()
        {
            StringBuilder msg = new StringBuilder($"Имя: {Name}\n");
            foreach (KeyValuePair<Job,int> duo in TimeOfWork)
                msg.Append($"№{duo.Key}: {duo.Value}   ");
            return msg.ToString();
        }

        public void Print(IPrinter printer)
        {
            printer.PrintLn(this.ToString());
        }
    }

}
