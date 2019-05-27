using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.DTOs
{
    public class WorkerDto
    {
        public string Name { get; set; }

        public IDictionary<JobDto, int?> TimeOfWork { get; set; }
    }
}
