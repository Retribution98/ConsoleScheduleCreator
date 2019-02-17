using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleApp.DataAccess.DTO
{
    public class QualificationDto
    {
        /// <summary>
        /// ID квалификации
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование квалификации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип работы, на которой специализируется квалификация
        /// </summary>
        public JobTypeDto JobType { get; set; }

        /// <summary>
        /// Процент эффективности работы
        /// </summary>
        public int EffectivePercent { get; set; }

        /// <summary>
        /// Исполнители, обладающие данной квалификацией
        /// </summary>
        public List<UserDto> Users { get; set; }
    }
}
