using System;

namespace ConsoleScheduleCreator
{
    public class JobType
    {
        /// <summary>
        /// ID Типа работ
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Названия типа работ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание типа работ
        /// </summary>
        public string Description { get; set; }
    }
}
