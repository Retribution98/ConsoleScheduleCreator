using ConsoleScheduleCreator.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleScheduleCreator.Entities.Project
{
    public class Pipeline
    {
        // Параметры для построителя
        private readonly int countJob;
        private const int maxLevel = 3;
        private const int minBranch = 1;
        private const int maxBranch = 3;
        private const int minDivarivation = 2;
        private const int maxDivarivation = 5;
        private const int minCombination = 2;
        private const int maxCombination = 5;
        private const int branchСhance = 40;
        private const int compositChance = 10;
        private const int divaricationChance = 50;

        // Используемые сущности
        private class Element
        {
            public Job Job { get; }
            public int Level { get; }
            public Element Parent { get; }
            public BuildStruct? WasBuild { get; }

            private Element(Job job, int level, Element parent, BuildStruct? wasBuild)
            {
                Job = job;
                Level = level;
                Parent = parent;
                WasBuild = wasBuild;
            }

            public static IEnumerable<Element> GetSomeInstance(int count, int level, Element parent = null, BuildStruct? wasBuild = null)
            {
                var elements = new List<Element>();
                var parentName = parent != null ? parent.Job.Name : "";
                var rand = new Random();
                for (var i = 0; i < count; i++)
                {
                    var mulct = rand.Next(100);
                    elements.Add(new Element(new Job($"{jobIndex++}/{mulct}", Guid.NewGuid(), 0, 1, mulct), level, parent, wasBuild));
                }
                return elements;
            }

            public bool IsDescendant(Element parent)
            {
                if (Parent == null)
                {
                    return false;
                }
                else
                if (Parent == parent)
                {
                    return true;
                }
                else
                    return Parent.IsDescendant(parent);
            }
        }
        private enum BuildStruct
        {
            Branch,
            Divarication,
            Combination
        }
        private class Combination
        {
            private Pipeline _builder;
            private List<Element> _elements;
            private int countForCombine;
            private Func<int> GetNextCount { get; }

            public Combination(Pipeline builder, int minCombination, int maxCombination)
            {
                _builder = builder;
                var random = new Random();
                this.GetNextCount = () => random.Next(minCombination, maxCombination);
                this._elements = new List<Element>();
                countForCombine = GetNextCount();
            }

            public void Add(Element element)
            {
                Add(new Element[] { element });
            }

            public void Add(IEnumerable<Element> elements)
            {
                _elements.AddRange(elements);
                if (_elements.Count() >= countForCombine)
                {
                    Combine();
                    countForCombine = GetNextCount();
                }
            }

            public void Combine()
            {
                if (_elements.Count == 0)
                {
                    return;
                }
                if (_elements.Count == 1)
                {
                    var element = _elements.First();
                    _builder.AddBranch(element, 1);
                    _elements.Remove(element);
                    _builder.lastElements.Remove(element);
                    return;
                }
                Element parent;
                int level;
                if (_builder.lastElements.Where(el => el.IsDescendant(_elements.First().Parent)).Any() ||
                    _builder.combinations.Where(c => c.Key.IsDescendant(_elements.First().Parent)).SelectMany(x => x.Value._elements).Any())
                {
                    parent = _elements.First().Parent;
                    level = _elements.First().Level;
                }
                else
                {
                    parent = _elements.First().Parent.Parent;
                    level = _elements.First().Parent.Level;
                }
                var newElement = Element.GetSomeInstance(
                    count: 1,
                    level: level,
                    parent: parent,
                    wasBuild: BuildStruct.Combination).First();
                // Добавление связей
                foreach (var element in _elements)
                {
                    newElement.Job.AddPrevios(element.Job);
                }
                // Добавление в множество
                _builder.allElements.Add(newElement);
                _builder.lastElements.Add(newElement);
                _elements.Clear();
            }
        }

        private List<Element> lastElements;
        private List<Element> allElements;
        private Dictionary<Element, Combination> combinations;
        private Dictionary<BuildStruct, uint> buildStructProbability = new Dictionary<BuildStruct, uint>
            {
                { BuildStruct.Branch, branchСhance },
                { BuildStruct.Divarication, divaricationChance },
                { BuildStruct.Combination, compositChance }
            };
        private static int jobIndex = 0;

        public Pipeline(uint count)
        {
            countJob = (int)count;
            var firstElement = Element.GetSomeInstance(count: 1, level: 0).First();
            allElements = new List<Element>() { firstElement };
            lastElements = new List<Element> { firstElement };
            combinations = new Dictionary<Element, Combination>();

            CreateJobs(countJob);
        }
        
        public IEnumerable<Job> GetJobs(string name = null)
        {
            return allElements.Select(el => el.Job);
        }

        private void CreateJobs(int countElement)
        {
            var random = new Random();
            while (lastElements.Any() && allElements.Count < countElement - 1)
            {
                var element = lastElements.First();
                var nextStruct = GetNextStruct(element);
                lastElements.Remove(element);
                switch (nextStruct)
                {
                    case BuildStruct.Branch:
                        AddBranch(element, random.Next(minBranch, maxBranch));
                        break;
                    case BuildStruct.Divarication:
                        AddDivarication(element, random.Next(minDivarivation, maxDivarivation));
                        break;
                    case BuildStruct.Combination:
                        combinations[element.Parent].Add(element);
                        break;
                    default:
                        throw new NotImplementedException("build struct");
                }
                if (!lastElements.Any())
                {
                    foreach (var combination in combinations)
                    {
                        combination.Value.Combine();
                    }
                }
            }
            foreach (var combination in combinations.OrderByDescending(x => x.Key.Level))
            {
                combination.Value.Combine();
                while (lastElements.Any())
                {
                    var element = lastElements.First();
                    lastElements.Remove(element);
                    if (element.Parent != null)
                    {
                        combinations[element.Parent].Add(element);
                    }
                }
            }
            lastElements.Clear();
        }

        private BuildStruct GetNextStruct(Element element)
        {
            var availableStructs = new List<BuildStruct>();
            if (element.WasBuild != BuildStruct.Branch)
            {
                availableStructs.Add(BuildStruct.Branch);
            }
            if (element.Level < maxLevel && element.WasBuild != BuildStruct.Divarication)
            {
                availableStructs.Add(BuildStruct.Divarication);
            }
            if (element.Parent != null && element.WasBuild != BuildStruct.Combination)
            {
                availableStructs.Add(BuildStruct.Combination);
            }
            return availableStructs.GetWithProbability(buildStructProbability);

        }

        private void AddBranch(Element element, int length)
        {
            if (!allElements.Contains(element))
            {
                allElements.Add(element);
            }
            var elements = Element.GetSomeInstance(count: length, level: element.Level, parent: element.Parent, wasBuild: BuildStruct.Branch).ToList();
            //Устаналвиваем связи
            elements[0].Job.AddPrevios(element.Job);
            for (var i = 1; i < length; i++)
            {
                elements[i].Job.AddPrevios(elements[i - 1].Job);
            }
            // Добавляем в множества
            allElements.AddRange(elements);
            lastElements.Add(elements.Last());
        }

        private void AddDivarication(Element element, int width)
        {
            if (!allElements.Contains(element))
            {
                allElements.Add(element);
            }
            var elements = Element.GetSomeInstance(count: width, level: element.Level + 1, parent: element, wasBuild: BuildStruct.Divarication);
            //Устанавлвиаются связи
            foreach (var el in elements)
            {
                el.Job.AddPrevios(element.Job);
            }
            // Добавляются в множества
            allElements.AddRange(elements);
            lastElements.AddRange(elements);

            combinations.Add(element, new Combination(
                this,
                Math.Min(minCombination, width),
                Math.Min(maxCombination, width)));
        }

    }
}
