using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ConsoleScheduleCreator;
using ConsoleScheduleCreator.Entities.Project;
using Shields.GraphViz.Models;
using Shields.GraphViz.Services;
using Shields.GraphViz.Components;
using System.IO;
using System.Threading;

namespace ConsoleScheduleCreator.Tests.Projects
{
    public class ProjectCompositeTests
    {
        //[Fact]
        //public void Constructor_Test()
        //{
        //    var job1 = new Job("1", Guid.NewGuid(), 0, 10, 0);
        //    var job2 = new Job("2", Guid.NewGuid(), 0, 10, 0);
        //    var job3 = new Job("3", Guid.NewGuid(), 0, 10, 0);
        //    var job4 = new Job("4", Guid.NewGuid(), 0, 10, 0);
        //    var job5 = new Job("5", Guid.NewGuid(), 0, 10, 0);

        //    var worker1 = new Worker()

        //    var firstProject = new Project(
        //        "",
        //        0,
        //        100,
        //        new List<Job> { job1, job2, job3, job4, job5},
        //        new string[] { "Petya", "Vasya", "Vova" },
        //        new int[1,1] { { 1} });

        //    new Project()
        //}

        [Fact]
        public async void GraphViz()
        {
            Graph graph = Graph.Directed                
                .Add(EdgeStatement.For("a", "b"))
                .Add(EdgeStatement.For("a", "c"));
            IRenderer renderer = new Renderer("C:\\Users\\kiril\\Desktop\\Paterns Старостин\\ConsoleScheduleCreator\\graphviz\\bin\\");
            using (var file = File.Create("graph.png"))
            {
                await renderer.RunAsync(
                    graph, file,
                    RendererLayouts.Dot,
                    RendererFormats.Png,
                    CancellationToken.None);
            }
        }
    }
}
