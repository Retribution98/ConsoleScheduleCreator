using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public interface IAlgorithm
    {
        Schedule CreateShedule(Project proj);
    }
}
