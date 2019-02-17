using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleApp.DataAccess.DTO
{
    public class UserDto
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
        public List<QualificationDto> Qualifications { get; set; }

        /// <summary>
        /// Выполненные или выполняемые работы пользователем
        /// </summary>
        public List<JobDto> Jobs { get; set; }

        /// <summary>
        /// Управляемые проекты
        /// </summary>
        public List<ProjectDto> ManagmentProjects { get; set; }

        /// <summary>
        /// Проекты, в которых пользователь является работником
        /// </summary>
        public List<ProjectDto> Projects { get; set; }
    }
}
