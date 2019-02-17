using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public interface IAlgorithm
    {
        /// <summary>
        /// Создать расписание
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        Schedule CreateShedule(Project proj);
    }
}
