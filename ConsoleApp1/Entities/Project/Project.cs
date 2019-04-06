using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection; //чтение данных из файлов Excel
using Excel = Microsoft.Office.Interop.Excel;
using ConsoleScheduleCreator.Algorithms;
using ConsoleScheduleCreator.Entities;

namespace ConsoleScheduleCreator
{
    public class Project: IProject, IPrintable, ICloneable
    {    
        public string Name { get; private set; }
        public int Early { get; private set; }
        public int Late { get; private set; }
        public  List<Job> Jobs { get; private set; }
        public  List<Worker> Workers { get; private set; }
        public Schedule Schedule { get; private set; }

        private Project() { }
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
                var timeOfJob = new Dictionary<Job, int?>();

                //Выделяем массив времен выбранного работника
                for (int j = 0; j < jobs.Count; j++)
                {
                    timeOfJob.Add(jobs[j], workersTime[i, j]);
                }
                //добавляем работника
                Workers.Add(new Worker(nameWorkers[i], timeOfJob));
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
                    var job_id = (int)(range.Cells[Rnum, 1] as Excel.Range).Value2;
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
                        newJob.AddPrevios(jobs.Find( x => x.Id == idPrevios.ToGuid()));
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

        public void Print(IPrinter printer)
        {
            printer.PrintLn($"Название проекта: {Name}");
            printer.PrintLn($"Количество работ: {Jobs.Count}");
            printer.PrintLn($"Количество работников: {Workers.Count}");
            printer.PrintLn("Работы:");
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

        public void CreateSchedule(IAlgorithm algorithm)
        {
            Schedule = algorithm.CreateShedule(this);
        }
        public void MultiAlgorihm(IAlgorithm algorithm)
        {
            Schedule = algorithm.MultiAlgorihm(this);
        }
        public void Reset()                         //Сброс планирования проекта
        {
            foreach (Job job in Jobs)
            {
                job.Reset();     //Сбрасывает параметры выполнения у работы
            }
        }

        public object Clone()
        {
            var newJobs = this.Jobs
                .ToDictionary(j => j, j => new Job(j.Name, Guid.NewGuid(), j.EarlyTime, j.LateTime, j.Mulct));
            foreach(var job in Jobs)
            {
                job.Previos
                    .ForEach(p => newJobs[job].AddPrevios(newJobs[p]));
                Workers.ForEach(w => w.AddJob(newJobs[job], w.GetTimeOfWork(job)));
            }
            return new Project
            {
                Name = this.Name,
                Early = this.Early,
                Late = this.Late,
                Jobs = newJobs.Values.ToList(),
                Workers = this.Workers
            };
        }

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
