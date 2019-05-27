using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator.DTOs
{
    public class JobDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int EarlyTime { get; set; }

        public int LateTime { get; set; }

        public int Mulct { get; set; }

        public List<JobDto> Previos { get; set; }
    }
}
