using ConsoleScheduleCreator.Entities.Project;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConsoleScheduleCreator.Tests.ProjectTests
{
    public class ProjectBuilderTest
    {
        [Fact]
        public void GetGraph()
        {
            var projBuilder = new ProjectBuilder();
            projBuilder.Name = "TestJobs";
            projBuilder.AddJobPipeline(1000);
            var proj = projBuilder.Please();

            var printerToFile = new PrinterToFile("VIZTestJobs");
            proj.PrintGraph(printerToFile);
        }
    }
}
