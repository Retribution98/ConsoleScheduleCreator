using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleScheduleCreator.Algorithms;
using Shields.GraphViz.Models;

namespace ConsoleScheduleCreator.Entities.Project
{
    public class ProjectComposite : IProject
    {
        private List<IProject> _projects;
        private Schedule _schedule;

        public ProjectComposite(IEnumerable<IProject> projects)
        {
            _projects = projects.ToList();
        }

        public string Name { get { return $"Composite project ({String.Join(", ", _projects.Select(p => p.Name))})"; } }

        public int Early { get { return _projects.Min(p => p.Early); } }

        public int Late { get { return _projects.Max(p => p.Late); } }

        public List<Job> Jobs { get { return _projects.SelectMany(p => p.Jobs).Distinct().ToList(); } }

        public List<Worker> Workers { get { return _projects.SelectMany(p => p.Workers).Distinct().ToList(); } }

        public Schedule Schedule {
            get { return _schedule; }
            set {
                _schedule = value;
                _projects.ForEach(pr => pr.Schedule = value);
            }
        }
 

        public void AddJob(Job newJob, Dictionary<Worker, int> timeOfWorker)
        {
            _projects.ForEach(p => p.AddJob(newJob, timeOfWorker));
        }

        public object Clone()
        {
            var projectsClones = _projects.Select(p => p.Clone() as IProject);
            return new ProjectComposite(projectsClones);
        }

        public void CreateSchedule(IAlgorithm algorithm)
        {
            Schedule = algorithm.CreateShedule(this);
        }

        public void MultiAlgorihm(IAlgorithm algorithm)
        {
            Schedule = algorithm.MultiAlgorihm(this);
        }

        public void PrintTimeLine(IPrinter printer)
        {
            _projects.ForEach(p => p.PrintTimeLine(printer));
        }

        public void Reset()
        {
            _projects.ForEach(p => p.Reset());
        }

        public void PrintGraph(IPrinter printer)
        {
            
            var stats = new List<Statement>();
            foreach (var job in Jobs)
            {
                stats.AddRange(job.Previos.Select(pr => EdgeStatement.For(pr.Name, job.Name)));
            };
            var graph = Graph.Directed
                .Add(EdgeStatement.For("a", "b"))
                .AddRange(stats);
            printer.PrintGraph(graph, "TEST");
        }
    }
}
