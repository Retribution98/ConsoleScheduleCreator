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
        public uint Id { get; }                                              //ID работы
        public string Name { get; }                                         //Название работы
        public int EarlyTime { get; }                                       //Раннее начало выполнения
        public int LateTime { get; }                                        //Позднее окончание выполнения
        public int Mulct { get; set; }                                           //Штраф
        public Int64 FinalPenalty { get; private set; }                               //Итоговый штраф
        public  List<Job> Previos { get; set; }                    //Предшествующие работы
        public  bool Completed { get; private set; }                         //Флаг окончания выполнения работы
        public int TimeStart { get; set; }                                  //Время начала выполнения работы
        public int TimeEnd { get; set; }                                    //Время окончания выполения работы

        //Методы

        //Конструктор с заданным именем, временем раннего начала и познего окнчания выполнения, а так же штрафом
        public Job(string name, uint id, int early, int late, int mulct)      
        {
            //Инициализируем поля
            Id = id;
            Name = name ?? throw new ArgumentNullException("Name can't be null!");
            EarlyTime = early;
            LateTime = late;
            Mulct = mulct;
            Completed = false;
            TimeStart = -1;
            TimeEnd = -1;
            Previos = new List<Job>();
        }

        public void AddPrevios(Job NewPrevios)      // Добавление предшествующей работы
        {
            if (NewPrevios == null) throw new ArgumentNullException("Previos can't be null");
            Previos.Add(NewPrevios);          // Добавляем предшествующую работу
        }
        
        public bool Ready(int time)         // Работа готова к выполнению
        {
            if (Completed) return false; //Если работа завершена - не готова к выполнению
            if (EarlyTime > time) return false;     //если раннее время не наступило - не готова
            //Проверяем завершение выполнения предшественников
            foreach (Job prev in Previos)
            {
                if (prev.Completed == false) return false;
                if (prev.TimeEnd >= time) return false;
            }
            return true;
        }

        public void Complete(int timeStart, int timeEnd)        //Работа выполнена
        {
            Completed = true;          // Устанваливаем статус "завершен"
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            // Рассчитываем полученный штраф за работу
            if (timeEnd > LateTime) FinalPenalty = Mulct * (timeEnd - LateTime);
            else FinalPenalty = 0;
        }

        public Int64 GetPenaltyForTime(int time)        //Возвращает общий штраф на момент времени time
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
            StringBuilder msg = new StringBuilder("Id: " + Id + "\t Name: " + Name + "\t Раннее начало: " + EarlyTime + "\t Позднее окончание: " + LateTime + "\tПредшественники: ");
            if (Previos.Count != 0)
            {
                msg.Append(String.Join(", ", Previos.Select(j => j.Id)));
            }
            else msg.AppendFormat("отсутствуют");
            return msg.ToString();
        }

        public void Print(IPrinter printer)
        {
            printer.PrintLn(this.ToString());
        }

        public void Reset()                 //Срос планирования
        {
            //Убираем время начала и окончания выполнения работы
            TimeStart = -1;
            TimeEnd = -1;
            //Обнуляем полученные штрафы
            FinalPenalty = 0;
            //Снимаем рабочие статусы
            Completed = false;
        }

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
