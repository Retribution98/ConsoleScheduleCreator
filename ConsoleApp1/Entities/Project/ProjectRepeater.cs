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

        public string Name { get { return $"Repeat project ({Project.Name})"; } }

        public int Early { get { return Project.Early; } }

        public int Late { get { return Project.Late * (int)_numRepeat; } }

        public List<Job> Jobs { get { return _repeatedProjects.SelectMany(p => p.Jobs).ToList(); } }

        public List<Worker> Workers { get { return Project.Workers; } }

        public Schedule Schedule { get; private set; }

        public ProjectRepeater (IProject project, uint numRepeat)
        {
            Project = project;
            _numRepeat = numRepeat;
            _repeatedProjects = new List<IProject>();
            for (var i =0; i<_numRepeat; i++)
            {
                _repeatedProjects.Add(project.Clone() as IProject);
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
    }
}
