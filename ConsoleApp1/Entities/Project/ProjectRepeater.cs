using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleScheduleCreator.Algorithms;

namespace ConsoleScheduleCreator.Entities.Project
{
    public class ProjectRepeater : IProject
    {
        public IProject Project { get; }

        private readonly List<IProject> _repeatedProjects;

        private readonly uint _numRepeat;

        private Schedule _schedule;

        public string Name { get { return $"Repeat project ({Project.Name})"; } }

        public int Early { get { return Project.Early; } }

        public int Late { get { return Project.Late * (int)_numRepeat; } }

        public List<Job> Jobs { get { return _repeatedProjects.SelectMany(p => p.Jobs).ToList(); } }

        public List<Worker> Workers { get { return Project.Workers; } }

        public Schedule Schedule {
            get { return _schedule; }
            set {
                _schedule = value;
                _repeatedProjects.ForEach(pr => pr.Schedule = value);
            }
        }

        public ProjectRepeater (IProject project, uint numRepeat)
        {
            Project = project;
            _numRepeat = numRepeat;
            _repeatedProjects = new List<IProject>();
            for (var i =0; i<_numRepeat; i++)
            {
                var newPorject = project.Clone() as IProject;
                var lastProject = _repeatedProjects.LastOrDefault();
                if (lastProject != null)
                {
                    var separatingJob = new Job("Separator", Guid.NewGuid(), lastProject.Early, lastProject.Late, 0);
                    foreach (var job in lastProject.Jobs)
                    {
                        separatingJob.AddPrevios(job);
                    }
                    foreach(var job in newPorject.Jobs)
                    {
                        job.AddPrevios(separatingJob);
                    }
                    var timeOfWork = new Dictionary<Worker, int>();
                    foreach (var worker in lastProject.Workers)
                    {
                        timeOfWork.Add(worker, 0);
                    }
                    lastProject.AddJob(separatingJob, timeOfWork);
                }
                _repeatedProjects.Add(newPorject);
            }
        }

        public void CreateSchedule(IAlgorithm algorithm)
        {
            Schedule = algorithm.CreateShedule(this);
        }

        public void MultiAlgorihm(IAlgorithm algorithm)
        {
            Schedule = algorithm.MultiAlgorihm(this);
        }

        public void Reset()
        {
            _repeatedProjects.ForEach(p => p.Reset());
        }

        public object Clone()
        {
            return new ProjectRepeater(Project.Clone() as IProject, _numRepeat);
        }

        public void AddJob(Job newJob, Dictionary<Worker, int> timeOfWorker)
        {
            _repeatedProjects.Last().AddJob(newJob, timeOfWorker);
        }

        public void PrintTimeLine(IPrinter printer)
        {
            _repeatedProjects.ForEach(p => p.PrintTimeLine(printer));
        }
    }
}
