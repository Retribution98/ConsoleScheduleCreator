using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleApp.DataAccess.DTO;


namespace ScheduleCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectDto proj = null;
            UserDto manager = null;
            UserDto worker1 = null, worker2 = null, worker3 = null;
            JobTypeDto firstJobType = null;
            QualificationDto firstQual = null;
            JobDto job1 = null, job2 = null, job3 = null;

            firstJobType = new JobTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "first"
            };
            firstQual = new QualificationDto
            {
                EffectivePercent = 100,
                Id = Guid.NewGuid(),
                Name = "first",
                JobType = firstJobType,
                Users = new List<UserDto>
                {
                    worker1,
                    worker2,
                    worker3
                }
            };
            
            manager = new UserDto {
                Id = Guid.NewGuid(),
                FirstName = "Mahager",
                LastName = "First",
                Login = "manager1",
                Password = "qwerty",
                Roles = new List<Role>
                {
                    Role.Manager
                },
                ManagmentProjects = new List<ProjectDto>
                {
                    proj
                }
            };

            proj = new ProjectDto
            {
                CreateDate = DateTime.Now,
                EarlyTime = DateTime.Now,
                LateTime = DateTime.Today,
                Name = "Test",
                Id = Guid.NewGuid(),
                Manager = manager,
                Workers = new List<UserDto>
                {
                    worker1,
                    worker2,
                    worker3
                }
            };
            new FrontAlgorithm(new FirstlyStratagy()).CreateShedule(proj, proj.EarlyTime.Value, proj.LateTime.Value, PeriodUnit.Minutes);

            //var printerToConsole = new PrinterToConsole();
            //proj.Print(printerToConsole);
            //proj.CreateSchedule(new FrontAlgorithm(new GreedyStratagy()));
            //proj.Schedule.Print(printerToConsole);
            
            Console.Read();
        }
 
    }
}
