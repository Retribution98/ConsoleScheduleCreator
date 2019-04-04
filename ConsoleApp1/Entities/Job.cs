﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public class Job:IPrintable, ICloneable
    {
        //Свойства
        public uint Id { get; }                                              //ID работы
        public string Name { get; }                                         //Название работы
        public int EarlyTime { get; }                                       //Раннее начало выполнения
        public int LateTime { get; }                                        //Позднее окончание выполнения
        public int Mulct { get; set; }                                           //Штраф
        public  List<Job> Previos { get; set; }                    //Предшествующие работы
        public  bool IsCompleted { get; private set; }                         //Флаг окончания выполнения работы
        public int TimeStart { get; set; }                                  //Время начала выполнения работы
        public int TimeEnd { get; set; }                                    //Время окончания выполения работы
        public int Priority { get; set; }                       // Приоритет для многпороходного алгоритма
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
            IsCompleted = false;
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
            if (IsCompleted) return false; //Если работа завершена - не готова к выполнению
            if (EarlyTime > time) return false;     //если раннее время не наступило - не готова
            //Проверяем завершение выполнения предшественников
            foreach (Job prev in Previos)
            {
                if (prev.IsCompleted == false) return false;
                if (prev.TimeEnd >= time) return false;
            }
            return true;
        }

        public void Complete(int timeStart, int timeEnd)        //Работа выполнена
        {
            IsCompleted = true;          // Устанваливаем статус "завершен"
            TimeStart = timeStart;
            TimeEnd = timeEnd;
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
            //Снимаем рабочие статусы
            IsCompleted = false;
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
