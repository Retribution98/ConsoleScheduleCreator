using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public class Job:IPrintable
    {
        //Свойства
        public int Id { get; }                                              //ID работы
        public string Name { get; }                                         //Название работы
        public int EarlyTime { get; }                                       //Раннее начало выполнения
        public int LateTime { get; }                                        //Позднее окончание выполнения
        internal List<Job> Previos { get; private set; }                    //Предшествующие работы
        public bool Completed { get; private set; }                         //Флаг окончания выполнения работы
        private bool InProgress { get;set; }                                //Флаг выполнения работы
        public int Mulct { get; }                                           //Штраф
        public int FinalPenalty { get; set; }                               //Итоговый штраф
        public int TimeStart { get; set; }                                  //Время начала выполнения работы
        public int TimeEnd { get; set; }                                    //Время окончания выполения работы

        //Методы

        //Конструктор с заданным именем, временем раннего начала и познего окнчания выполнения, а так же штрафом
        public Job(string name, int id, int early, int late, int mulct)      
        {
            //Инициализируем поля
            Id = id;
            Name = name;
            EarlyTime = early;
            LateTime = late;
            Mulct = mulct;
            Completed = false;
            InProgress = false;
            TimeStart = -1;
            TimeEnd = -1;
            Previos = new List<Job>();
        }

        public void AddPrevios(Job NewPrevios)      // Добавление предшествующей работы
        {
            Previos.Add(NewPrevios);          // Добавляем предшествующую работу
        }
        
        public bool Ready()         // Работа готова к выполнению
        {
            if ((Completed == true) || (InProgress == true)) return false; //Если работа завершена или в работе - не готова к выполнению

            //Проверяем завершение выполнения предшественников
            foreach (Job prev in Previos)
            {
                if (prev.Completed == false) return false;
            }
            return true;
        }

        public void Execut()        //Возвращает true, если в момент time рыбота находится в процессе выполнения, иначе false
        {
            InProgress = true;
        }

        public void Finish(int time)        //Работа выполнена
        {
            time++;
            InProgress = false;         // Снимаем стаус "в работе"
            Completed = true;          // Устанваливаем статус "завершен"
            Console.WriteLine("Работа №{0} закончила выполнение после {1} такта",this.Id+1,time);
            // Рассчитываем полученный штраф за работу
            if (time > LateTime) FinalPenalty = Mulct * (time - LateTime);
            else FinalPenalty = 0;
        }

        public int GetPenaltyForTime(int time)        //Возвращает общий штраф на момент времени time
        {
            if (Completed == true)             // Для заверешенных уже посчитано
            {
                return FinalPenalty;
            }
            else if (time > LateTime)           //Если просрочилось - добавляем штраф
            {
                return Mulct * (time - LateTime);
            }
            else return 0;
        }

        public override string ToString()
        {
            StringBuilder msg = new StringBuilder("ID: " + Id + "\t Name: " + Name + "\t Раннее начало: " + EarlyTime + "\t Позднее окончание: " + LateTime + "\nПредшественники: ");
            if (Previos.Count != 0)
                foreach (Job prev in Previos)
                {
                    msg.AppendFormat(prev.Id + ", ");
                }
            else msg.AppendFormat("отсутсвуют");
            return msg.ToString();
        }

        public void Print(IPrinter printer)
        {
            printer.Print(this.ToString());
        }

        /*public void Reset()                 //Срос планирования
        {
            //Убираем время начала и окончания выполнения работы
            start = -1;
            end = -1;
            //Обнуляем полученные штрафы
            penalty = 0;
            //Снимаем рабочие статусы
            inwork = false;
            finish = false;
        }*/

       /*public void AddPrevios(int amt, Job[] NewPrevios) // Добавление нескольких предшествующих работ
        {
            if (num_Prev == 0) previos = new Job[amt];
            else Array.Resize(ref previos, num_Prev + amt);
            for (int i = 0; i < amt; i++) previos[num_Prev + i] = NewPrevios[i];
            num_Prev = num_Prev + amt;
        }*/

        /*public void ChangePrevios(int amt, Job[] NewPrevios)    // Изменение множества предшествующих работ
        {
            Array.Copy(previos, NewPrevios, amt);
            num_Prev = amt;
        }*/
        
    }
}
