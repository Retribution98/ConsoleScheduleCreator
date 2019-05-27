using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace ConsoleScheduleCreator
{
    public static class Utils
    {

        public static Guid ToGuid(this int id)
        {
            return new Guid(id, 0, 0, new byte[] { 0,0,0,0,0,0,0,0});
        }

        public static List<T> AddElement<T>(this List<T> elements, T element, uint count)
        {
            for(var i =0; i< count; i++)
            {
                elements.Add(element);
            }
            return elements;
        }

        public static T GetWithProbability<T>(this List<T> elements, Dictionary<T, uint> probability)
        {
            if (probability == null)
            {
                //Если не задан - устанавливаем равную вероятность
                probability = new Dictionary<T, uint>();
                foreach (var elem in elements)
                {
                    probability.Add(elem, 1);
                }
            } else {
                if (probability.Where(pr => elements.Contains(pr.Key)).All(pr => pr.Value == 0))
                {
                    throw new ArgumentException("no available element");
                }
            }
            var arrayProbability = new List<T>();
            foreach (var elem in elements)
            {
                if (!probability.ContainsKey(elem))
                {
                    throw new ArgumentException("invalid probability");
                }
                arrayProbability.AddElement(elem, probability[elem]);
            }

            var random = new Random();
            return arrayProbability
                .Skip(random.Next(arrayProbability.Count))
                .Take(1)
                .First();
        }

        public static void Print(this Excel.Worksheet worksheet, int numRow, int numCol, object value)
        {
            var retry = true;
            do
            {
                try
                {
                    var excelcells = (Excel.Range)worksheet.Cells[numRow, numCol];
                    //Выводим координаты ячеек
                    excelcells.Value2 = value;
                    retry = false;
                }
                catch (Exception exp)
                {
                    System.Threading.Thread.Sleep(10);
                }
            } while (retry);

        }
    }
}
