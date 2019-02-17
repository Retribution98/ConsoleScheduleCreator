using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleScheduleCreator
{
    public class User
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Login пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// Квалификации пользователя
        /// </summary>
        public List<Qualification> Qualifications { get; set; }

        /// <summary>
        /// Выполненные или выполняемые работы пользователем
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// Управляемые проекты
        /// </summary>
        public List<Project> ManagmentProjects { get; set; }

        /// <summary>
        /// Проекты, в которых пользователь является работником
        /// </summary>
        public List<Project> Projects { get; set; }
    }
}
