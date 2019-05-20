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
        private IDictionary<Job, int?> _timeOfWork;
        public int Priority { get; set; }                   // Приоритет для многопроходного алгоритма

        //Методы
        public Worker(string _name, IDictionary<Job, int?> time_of_work)      // Конструктор
        {
            Name = _name;
            _timeOfWork = new Dictionary<Job, int?>(time_of_work);
        }

        public override string ToString()
        {
            StringBuilder msg = new StringBuilder($"Имя: {Name}\n");
            foreach (var duo in _timeOfWork)
                msg.Append($"№{duo.Key}: {duo.Value}   ");
            return msg.ToString();
        }

        public int? GetTimeOfWork(Job job)
        {
            if (_timeOfWork.ContainsKey(job))
            {
                return _timeOfWork[job];
            }
            else
            {
                return null;
            }
        }

        public void AddJob(Job job, int? timeOfWork)
        {
            _timeOfWork.Add(job, timeOfWork);
        }

        public void Print(IPrinter printer)
        {
            printer.PrintLn(this.ToString());
        }
    }

}
