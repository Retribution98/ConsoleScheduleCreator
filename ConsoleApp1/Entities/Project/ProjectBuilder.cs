using ConsoleScheduleCreator.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleScheduleCreator.Entities.Project
{
    public class ProjectBuilder
    {
        public string Name { get; set; }
        public int Early { get; set; }
        public int Late { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Worker> Workers { get; set; }

        public ProjectBuilder()
        {
            Name = "";
            Jobs = new List<Job>();
            Early = 0;
            Late = 1;
            Workers = new List<Worker>();
        }
        public ProjectBuilder(ProjectDto dto)
        {
            var jobs = dto.Jobs.ToDictionary(x => x, x => new Job(x.Name, x.Id, x.EarlyTime, x.LateTime, x.Mulct));
            foreach(var job in dto.Jobs)
            {
                job.Previos.ForEach(x => jobs[job].AddPrevios(jobs[x]));
            }
            var workers = dto.Workers.Select(w => new Worker(w.Name, w.TimeOfWork.ToDictionary(x => jobs[x.Key], x => x.Value)));

            Name = dto.Name;
            Early = dto.Early;
            Late = dto.Late;
            Jobs = jobs.Values.ToList();
            Workers = workers.ToList();
        }

        public Project Please()
        {
            return new Project(Name, Early, Late, Jobs, Workers);
        }

        public void AddJobPipeline(uint count)
        {
            var pipeline = new Pipeline(count);
            Jobs.AddRange(pipeline.GetJobs());
        }

        public void AddWorker(uint count)
        {
            for (var i=0; i< count; i++)
            {
                var worker = new Worker($"Worker{Workers.Count}", Jobs.ToDictionary(x => x, x => (int?)1));
                Workers.Add(worker);
            }
        }
    }
}
