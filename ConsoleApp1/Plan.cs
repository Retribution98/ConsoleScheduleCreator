using ScheduleApp.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleCreator
{
    public class Plan
    {
        private JobDto[,] _jobs;
        public int Time { get; }
        public List<UserDto> Users { get; }
        public PeriodUnit TimeUnit { get; }

        public Plan(List<UserDto> users, PeriodUnit unit, int time)
        {
            TimeUnit = unit;
            if (time > 0)
                Time = time;
            else throw new ArgumentException("Wrong time");
            Users = new List<UserDto>();
            foreach (var user in users)
            {
                if (Users.Contains(user))
                    continue;
                else Users.Add(user);
            }
            _jobs = new JobDto[Users.Count, Time];
        }

        public JobDto this[UserDto user, int time]
        {
            get
            {
                return _jobs[Users.IndexOf(user),time];
            }
            set
            {
                _jobs[Users.IndexOf(user), time] = value;
            }
        }
    }
}
