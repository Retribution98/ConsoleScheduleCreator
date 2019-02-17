using System;

namespace ScheduleApp.DataAccess.DTO
{
    public class JobTypeDto
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
