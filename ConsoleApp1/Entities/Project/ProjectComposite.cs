using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleScheduleCreator.Algorithms;
using DotNetGraph;

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
            var graph = new DotGraph(Name, true);
            foreach (var job in Jobs)
            {
                var node = new DotNode($"\"{job.Id}\"") { Label = job.Name };
                graph.Add(node);
            };
            foreach (var job in Jobs)
            {
                job.Previos
                    .ForEach(prev => graph.Add(new DotArrow($"\"{prev.Id}\"", $"\"{job.Id}\"")));
            };
            printer.PrintGraph(graph);
        }
    }
}
