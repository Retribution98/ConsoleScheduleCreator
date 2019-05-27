using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.DTOs
{
    public class ProjectDto
    {
        public string Name { get; set; }

        public int Early { get; set; }

        public int Late { get; set; }

        public List<JobDto> Jobs { get; set; }

        public List<WorkerDto> Workers { get; set; }
    }
}
