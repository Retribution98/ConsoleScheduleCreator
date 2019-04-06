using ConsoleScheduleCreator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.Algorithms
{
    public interface IAlgorithm
    {
        Schedule CreateShedule(IProject proj);
        Schedule MultiAlgorihm(IProject proj);
    }
}
