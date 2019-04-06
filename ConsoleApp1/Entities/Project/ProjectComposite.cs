using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleScheduleCreator.Algorithms;

namespace ConsoleScheduleCreator.Entities.Project
{
    public class ProjectComposite : IProject
    {
        private List<IProject> _projects;

        public ProjectComposite(IEnumerable<IProject> projects)
        {
            _projects = projects.ToList();
        }

        public string Name { get { return $"Composite project ({String.Join(", ", _projects.Select(p => p.Name))})"; } }

        public int Early { get { return _projects.Min(p => p.Early); } }

        public int Late { get { return _projects.Max(p => p.Late); } }

        public List<Job> Jobs { get { return _projects.SelectMany(p => p.Jobs).Distinct().ToList(); } }

        public List<Worker> Workers { get { return _projects.SelectMany(p => p.Workers).Distinct().ToList(); } }

        public Schedule Schedule {get; private set;}

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

        public void Reset()
        {
            _projects.ForEach(p => p.Reset());
        }
    }
}
