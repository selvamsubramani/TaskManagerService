using Moq;
using NBench;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskManager.DataLayer;

namespace TaskManager.PerformanceTest
{
    public class DataLayerPerformanceTests
    {
        private List<Task> output = null;
        Mock<DbSet<Task>> mockTasks;
        List<Task> tasks;
        TaskDataConnector connector;
        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            Task parent = new Task { ID = 1, Description = "Task-01", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 5 };
            tasks = new List<Task>
            {
                parent,
                new Task { ID = 2, Description = "Task-02", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 10, ParentTask = parent },
                new Task { ID = 3, Description = "Task-03", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 15, ParentTask = parent }
            };
            var source = tasks.AsQueryable();
            mockTasks = new Mock<DbSet<Task>>();
            mockTasks.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(source.Expression);
            mockTasks.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(source.ElementType);
            mockTasks.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
            mockTasks.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(source.Provider);

            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            connector = new TaskDataConnector(model.Object);
        }

        [PerfBenchmark(Description = "Task Manager Performance Test", NumberOfIterations = 5,
            RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        public void GetTasks_MemoryMesaurement()
        {            
            output = connector.GetAllTasks().ToList();
        }

        [PerfBenchmark(Description = "Task Manager Performance Test", NumberOfIterations = 5,
            RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void GetTasks_GcMesaurement()
        {
            output = connector.GetAllTasks().ToList();
        }

        [PerfBenchmark(Description = "Task Manager Performance Test", NumberOfIterations = 1,
           RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 2000)]
        public void GetTasks_ElapsedTimeAssertion()
        {
            output = connector.GetAllTasks().ToList();
        }
        [PerfCleanup]
        public void Cleanup()
        {
            if (output != null)
                output = null;
        }
    }
}
