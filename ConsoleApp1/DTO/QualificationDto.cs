using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleScheduleCreator
{
    public class Qualification
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
        public JobType JobType { get; set; }

        /// <summary>
        /// Процент эффективности работы
        /// </summary>
        public int EffectivePercent { get; set; }

        /// <summary>
        /// Исполнители, обладающие данной квалификацией
        /// </summary>
        public List<User> Users { get; set; }
    }
}
