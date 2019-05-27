using AutoMapper;
using ConsoleScheduleCreator.Algorithms;
using ConsoleScheduleCreator.DTOs;
using ConsoleScheduleCreator.Entities.Project;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;

namespace ConsoleScheduleCreator
{
    class Program
    {
        private static Excel.Worksheet boolSheet;
        private static Excel.Worksheet singleSheet;
        private static Excel.Worksheet multiSheet;
        private static Excel.Worksheet singleTimeSheet;
        private static Excel.Worksheet multiTimeSheet;

        static void Main(string[] args)
        {
            //var proj = Project.Open(@"D:\GIT\ConsoleScheduleCreator\Data.xlsx");
            //if (proj == null)
            //{
            //    System.GC.Collect();
            //    Console.Read();
            //    return;
            //}


            Excel.Application excelapp = new Excel.Application();            // Приложение Excel
            Excel.Workbook excelappworkbook = null;                                    // Книга данныъх
            Excel.Worksheet worksheet;                                  // Страница данных

            excelapp = new Excel.Application();
            excelapp.Visible = true;
            excelapp.SheetsInNewWorkbook = 5;
            excelapp.Workbooks.Add(Type.Missing);
            excelappworkbook = excelapp.Workbooks[1];

            var excelsheets = excelappworkbook.Worksheets;
            boolSheet = (Excel.Worksheet)excelsheets.get_Item(1);
            singleSheet = (Excel.Worksheet)excelsheets.get_Item(2);
            multiSheet = (Excel.Worksheet)excelsheets.get_Item(3);
            singleTimeSheet = (Excel.Worksheet)excelsheets.get_Item(4);
            multiTimeSheet = (Excel.Worksheet)excelsheets.get_Item(5);
            try
            {
                Res();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Algorihm Error: " + ex.Message);
            }

            //save
            try
            {
                excelappworkbook.Saved = true;
                excelapp.DisplayAlerts = false;
                //excelapp.DefaultSaveFormat = Excel.XlFileFormat.xlExcel12;
                excelappworkbook.SaveAs(@"D:\schedulies2\schedulies.xls",
                Excel.XlFileFormat.xlWorkbookDefault,
                Type.Missing,                                //Пароль для доступа на запись 
                Type.Missing,                                //Пароль для открытия документа
                Type.Missing, Type.Missing,
                Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                excelapp.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExcelError: " + ex.Message);
                excelapp.Quit();
            }

            

            Console.WriteLine("Succes");
            Console.Read();
        }

        static void Res()
        {
            var counts = new List<uint> { 10000, 100000 };
            for (var c=0; c < counts.Count; c++)
            {
                var count = counts[c];
                var startRow = c * 50 + 2;
                for (var n = 0; n < 50; n++)
                {
                    var filename = $"\\schedulies2\\{count}\\project{count}-{n}";
                    var builder = new ProjectBuilder();
                    builder.AddJobPipeline(count);
                    var proj = builder.Please();
                    using (var printerToFile = new PrinterToFile($"{filename}.dot"))
                    {
                        proj.PrintGraph(printerToFile);
                    }
                    var startTime = Stopwatch.StartNew();
                    using (var printer = new PrinterToFile($"{filename}.txt"))
                    {
                        using (var printerTime = new PrinterToFile($"{filename}Time.txt"))
                        {
                            for (uint i = 1; i < 30; i++)
                            {
                                builder.Workers.Clear();
                                builder.AddWorker(i);
                                proj = builder.Please();

                                startTime.Restart();
                                proj.CreateSchedule(new FrontAlgorithm(new JobsDelayMinimization()));
                                startTime.Stop();

                                
                                var firstTime = startTime.Elapsed;
                                var firstRes = proj.Schedule.Penalty;
                                singleTimeSheet.Print(startRow + (int)i, n + 2, firstTime.TotalMilliseconds);
                                singleSheet.Print(startRow + (int)i, n + 2, firstRes);

                                startTime.Restart();
                                proj.MultiAlgorihm(new FrontAlgorithm(new JobsDelayMinimization()));
                                startTime.Stop();

                                var secondTime = startTime.Elapsed;
                                var secondRes = proj.Schedule.Penalty;
                                multiTimeSheet.Print(startRow + (int)i, n + 2, secondTime.TotalMilliseconds);
                                multiSheet.Print(startRow + (int)i, n + 2, secondRes);

                                printer.PrintLn($"{i}: {firstRes} / {secondRes}");
                                if (secondRes < firstRes)
                                {
                                    boolSheet.Print(startRow + (int)i, n + 2, 1);
                                }
                                else
                                {
                                    boolSheet.Print(startRow + (int)i, n + 2, 0);
                                }
                                printerTime.PrintLn($"{(secondTime.TotalMilliseconds / firstTime.TotalMilliseconds).ToString()}");
                                using (var jsonPrinter = new PrinterToFile($"{filename}-{i}.json"))
                                {
                                    var sw = new StringWriter();
                                    using (var writer = new JsonTextWriter(sw))
                                    {
                                        writer.Formatting = Formatting.Indented;
                                        proj.SaveToJson(writer);
                                    }
                                    jsonPrinter.Print(sw.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        
        static void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Worker, WorkerDto>();
                cfg.CreateMap<Job, JobDto>();
                cfg.CreateMap<Project, ProjectDto>();
            });
            
        }
    }
}
