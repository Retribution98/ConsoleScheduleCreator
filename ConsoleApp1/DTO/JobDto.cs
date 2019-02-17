using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleScheduleCreator
{
    public class Job
    {
        /// <summary>
        /// ID работы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название работы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание работы
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Тип работы
        /// </summary>
        public JobType JobType { get; set; }

        /// <summary>
        /// Время выполнения работы
        /// </summary>
        public TimeSpan LeadTime { get; set; }

        /// <summary>
        /// Проект, включающий данную работу
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Ограничение на время раннего начала
        /// </summary>
        public DateTime? EarlyTime { get; set; }

        /// <summary>
        /// Ограничение на время позднего окончания
        /// </summary>
        public DateTime? LateTime { get; set; }

        /// <summary>
        /// Работник ,выполня
        /// </summary>
        public User Worker { get; set; }

        /// <summary>
        /// Время начала выполнения работы
        /// </summary>
        public DateTime? TimeStart { get; set; }

        /// <summary>
        /// Время окончания выполнения работы
        /// </summary>
        public DateTime? TimeEnd { get; set; }

        /// <summary>
        /// Предшествующие рыботы по технологическим ограничениям
        /// </summary>
        public List<Job> Parents { get; set; }
    }
}
