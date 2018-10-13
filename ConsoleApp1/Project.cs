using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection; //чтение данных из файлов Excel
using Excel = Microsoft.Office.Interop.Excel;
namespace ConsoleScheduleCreator
{
    public class Project:IPrintable
    {    
        public string Name { get; private set; }
        public int Early { get; private set; }
        public int Late { get; private set; }
        internal List<Job> Jobs { get; private set; }
        internal List<Worker> Workers { get; private set; }

        //Конструктор
        public Project(string name, int early, int late, List<Job> jobs, string[] nameWorkers, int[,] workersTime)
        {
            //Проверка входных данных
            if (name == null) throw new ArgumentNullException("Name can't be null!");
            if (nameWorkers == null) throw new ArgumentNullException("nameWorkers can't be null!");
            if (workersTime == null) throw new ArgumentNullException("workersTime can't be null!");
            if (jobs == null) throw new ArgumentNullException("jobs can't be null!");
            if (workersTime.LongLength!=jobs.Count*nameWorkers.Length) throw new ArgumentOutOfRangeException("Size array workersTime is wrong");
            //Проверка на дубликаты Id работ
            var linq = from job in jobs
                       group job by job.Id into grouped
                       where grouped.Count() > 1
                       select grouped.Key;
            if (linq.Count() != 0) throw new DuplicateWaitObjectException("Jobs have equal ID");

            //Инициализируем поля класса
            Name = name;
            Early = early;
            Late = late;
            Workers = new List<Worker>();
            Jobs = jobs;

            //создаем исполнителей проекта
            for (int i = 0; i < nameWorkers.Length; i++)
            {
                int[] timeOfJob = new int[jobs.Count];

                //Выделяем массив времен выбранного работника
                for (int j = 0; j < jobs.Count; j++)
                {
                    timeOfJob[j] = workersTime[i, j];
                }
                //добавляем работника
                Workers.Add(new Worker(nameWorkers[i], jobs.Count, timeOfJob));
            }
        }

        public static Project Open(string FileName)      //Открываем проект, сохраненный как файл Excel
        {
            string proj_name;       //Имя проекта
            int proj_start;         //Раннее время старта работы над проектом
            int proj_end;           //Позднее время окончания работы над проектом
            int NumJobs;            //Количество работ
            int NumWorkers;         //Количество исполнителей
            string[] Name_Workers;  //Именя исполнителей
            int[,] Workers;         //Временные затраты на выполнение работ
            Project proj = null;

            Excel.Application app = new Excel.Application();            // Приложение Excel
            Excel.Workbook workbook = null;                                    // Книга данныъх
            Excel.Worksheet worksheet;                                  // Страница данных
            Excel.Range range;                                          // Ячейки данных
            try
            {
                //Открываем книгу
                workbook = app.Workbooks.Open(FileName, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //Выбираем страницу с характеристиками проекта
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(1);
                range = worksheet.get_Range("B1", Type.Missing);        //Считываем название проекта
                proj_name = Convert.ToString(range.Value2);
                range = worksheet.get_Range("B2", Type.Missing);        //Считываем раннее время старта работы над проектом
                proj_start = (int)range.Value2;
                range = worksheet.get_Range("B3", Type.Missing);        //Считываем позднее время окончания работы над проектом
                proj_end = (int)range.Value2;
                range = worksheet.get_Range("B4", Type.Missing);        //Считываем количество работ, включенных в проект
                NumJobs = (int)range.Value2;
                range = worksheet.get_Range("B5", Type.Missing);        //Считываем количество работ, включенных в проект
                NumWorkers = (int)range.Value2;

                //Создаем множества временных затрат и имен исполнителей
                Workers = new int[NumWorkers, NumJobs];
                Name_Workers = new string[NumWorkers];

                //Добавляем исполнителй
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(3);       //Открываем страницу исполнителей проекта
                range = worksheet.UsedRange;
                for (int i = 0; i < NumWorkers; i++)
                {
                    Name_Workers[i] = (range.Cells[2, 2 + i] as Excel.Range).Value2.ToString();       //Считываем название исполниетля
                    for (int j = 0; j < NumJobs; j++)
                    {
                        Workers[i, j] = (int)(range.Cells[4 + j, 2 + i] as Excel.Range).Value2;            //Считаываем время выполнения работ
                    }
                }

                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(2);       //Открываем страницу работ проекта
                range = worksheet.UsedRange;                                    //Задаем множество ячеек для работы - изменных ранее

                List<Job> jobs = new List<Job>();
                //Добавляем работу проекта
                for (int Rnum = 2; Rnum < NumJobs + 2; Rnum++)
                {
                    //считываем данные из файла
                    uint job_id = (uint)(range.Cells[Rnum, 1] as Excel.Range).Value2;
                    string job_name = (range.Cells[Rnum, 2] as Excel.Range).Value2.ToString();
                    int job_start = (int)(range.Cells[Rnum, 3] as Excel.Range).Value2;
                    int job_end = (int)(range.Cells[Rnum, 4] as Excel.Range).Value2;
                    int mulct = (int)(range.Cells[Rnum, 5] as Excel.Range).Value2;
                    int NumPrevios = (int)(range.Cells[Rnum, 6] as Excel.Range).Value2;
                    int[] Previos = new int[NumPrevios];

                    if (job_start < 0) job_start = proj_start;
                    if (job_end < 0) job_end = proj_end;

                    Job newJob = new Job(job_name, job_id, job_start, job_end, mulct);
                    for (int p = 0; p < NumPrevios; p++)
                    {
                        int idPrevios = (int)(range.Cells[Rnum, 7 + p] as Excel.Range).Value2;
                        newJob.AddPrevios(jobs.Find( x => x.Id == idPrevios));
                    }
                    jobs.Add(newJob);
                }
                //Создаем проект
                proj = new Project(proj_name, proj_start, proj_end, jobs, Name_Workers, Workers);
            }
            catch (Exception exc)
            {
                Console.WriteLine("!!!"+exc.Message);
            }
            finally
            {
                if (workbook != null)
                    workbook.Close(false, Missing.Value, Missing.Value);
                //Закрываем Excel файл
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                workbook = null;
                worksheet = null;
                range = null;
                System.GC.Collect();
            }
            return proj;
        }

        public List<Job> ReadyJobs(int time)            //Возвращает фронт работ проекта в момент времени time
        {
            //Определяем множество работ, готовых к выполнению - фронт работ
            List<Job> Front = new List<Job>();

            //Проверяем условия готовности работ, подходящие добавляются во фронт
            foreach(Job job in Jobs)
            {
                if ((job.EarlyTime <= time) && (job.Ready(time)))       //ранее время старта работы прошло и работа готова к выполнению
                {
                    Front.Add(job);                        //Добавляем работу
                }
            }

            //Возвращаем сформированный фронт работ
            return Front;
        }

        private List<Worker> ReadyWorkers(uint[,] plan, int time)
        {
            List<Worker> waitingWorkers = new List<Worker>();
            // формируем множетсво свободных исполнителей
            for (int number = 0; number < this.Workers.Count; number++)
                if (plan[number, time] == 0)                                               //Если исполнитель свободен
                {
                    waitingWorkers.Add(Workers[number]);                               //Добавляем свободного исполнителя
                }
            return waitingWorkers;
        }

        public Int64 PenaltyProject(int time)     //Общая сумма штрафа работ проекта на момент времени time
        {
            Int64 penalty = 0;
            foreach(Job job in Jobs)
            {
                penalty += job.GetPenaltyForTime(time);        //Прибавляем штраф очередной работы
                Console.WriteLine("ID: " + job.Id + "\tШтраф:" + job.Mulct + "\tОбщий штраф: " + job.FinalPenalty);
            }
            return penalty;
        }

        public void Print(IPrinter printer)
        {
            string msg = "\nНазвание проекта: " + Name + "\nКоличество работ: " + Jobs.Count + "\tКоличество работников: " + Workers.Count + "\nРаботы: \n";
            printer.Print(msg);
            foreach (Job job in Jobs)
            {
                job.Print(printer);
            }
            Console.WriteLine("Работники: ");
            foreach (Worker worker in Workers)
            {
                worker.Print(printer);
            }
        }

        public uint[,] CreateSchedule(int time)      //Создатель расписания выполнения проекта за промежуток времени длиною time
        {
            //Определяем расписание как график Ганта
            uint[,] plan = new uint[this.Workers.Count, time];

            //Проходим все такты планирования
            for (int time_now = 0; time_now < time; time_now++)
            {
                //Создаем фронт работ в данный момент времени
                List<Job> front = this.ReadyJobs(time_now);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Front in {0}:\t",time_now);
                foreach (Job job in front)
                {
                    Console.Write(job.Id + ", ");
                }
                Console.ResetColor();
                Console.WriteLine();

                //Массив свободных исполнителей
                List<Worker> waitingWorkers = this.ReadyWorkers(plan, time_now);               

                //Соотношение между Работником и порядковым номером в проекте
                Dictionary<Worker, int> numerationWorkers = new Dictionary<Worker, int>();
                for (int index = 0; index < Workers.Count; index++)
                {
                    numerationWorkers.Add(Workers[index], index);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("FreeWorkers:\t");
                foreach (Worker worker in waitingWorkers)
                {
                    Console.Write(worker.Name + ", ");
                }
                Console.ResetColor();
                Console.WriteLine();

                while ((waitingWorkers.Count > 0) && (front.Count != 0))
                {
                    var sortByTime = from worker in waitingWorkers
                                     orderby worker.TimeOfWork[front.First().Id - 1], worker.TimeInProcess 
                                     select worker;
                    Worker bestWorker = sortByTime.First();

                    //Формируем план, если работа будет выполнена до конца 
                    if (time_now + bestWorker.TimeOfWork[front.First().Id - 1] <= time)
                    {
                        for (int l = 0; l < bestWorker.TimeOfWork[front.First().Id - 1]; l++)                                      //Заполняем план для исполнителя с минимальным временм исполнения
                            plan[numerationWorkers[bestWorker], time_now + l] = front.First().Id;
                        front.First().Complete(time_now, time_now + bestWorker.TimeOfWork[front.First().Id - 1] - 1);             //Выполняем работу с такой-то по такой такт
                        bestWorker.AddProcess(bestWorker.TimeOfWork[front.First().Id - 1]);                                     //Добавляем нагрузку на исполнителя

                        front.Remove(front.First());                                                                            //Убираем работу из фронта
                    }

                    //Убираем выбранного исполнителя из массива свободных
                    waitingWorkers.Remove(bestWorker);
                }
            }

            return plan;
        }
        
        /*public void Reset()                         //Сброс планирования проекта
        {
            foreach(Job job in Jobs)
            {
                job.Reset();     //Сбрасывает параметры выполнения у работы
            }
        }*/

        //public void AddJob(string name, int early, int late, int mulct, int numPrevios, int[] previos)         //Добавляем множество работ в проект
        //{
        //    // Устанваливаем временные ограничения для работы, если отсутсвует ограничения или они выходят за рамки ограничений проекта - заменяются значениями проекта
        //    if ((early == -1) || (early < Early)) early = this.Early;
        //    if ((late == -1) || (late > Late)) late = this.Late;

        //    IdJobs++; // увеличиваем счетчик id работ проекта
        //    // Создаем новую работу в массиве работ проекта
        //    Jobs.Add(new Job(name, IdJobs, early, late, mulct));

        //    //Добавляем для новой работы множество предшестующих
        //    for (int i = 0; i < numPrevios; i++)
        //    {
        //        Jobs.Last().AddPrevios(Jobs[previos[i] - 1]);
        //    }
        //}
    }
}
