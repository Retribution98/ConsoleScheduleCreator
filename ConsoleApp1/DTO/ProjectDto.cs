using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleScheduleCreator
{
    public class Project
    {
        /// <summary>
        /// ID проекта
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название проекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Руководитель проекта
        /// </summary>
        public User Manager { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Ограничение на раннее время начала
        /// </summary>
        public DateTime? EarlyTime { get; set; }

        /// <summary>
        /// Ограничение на позднее время окончания
        /// </summary>
        public DateTime? LateTime { get; set; }

        /// <summary>
        /// Коллекция работ в проекте
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Исполнители участвующие в проекте
        /// </summary>
        public List<User> Workers { get; set; }
    }
}
