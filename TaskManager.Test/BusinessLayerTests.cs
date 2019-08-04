using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TaskManager.BusinessLayer;
using TaskManager.DataLayer;

namespace TaskManager.Test
{
    [TestClass]
    public class BusinessLayerTests
    {
        IQueryable<Task> tasks;
        [TestInitialize]
        public void Setup()
        {
            Task parent = new Task { ID = 1, Description = "Task-01", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 5 };
            tasks = new Task[]
            {
                parent,
                new Task { ID = 2, Description = "Task-02", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 10, ParentTask = parent },
                new Task { ID = 3, Description = "Task-03", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 15, ParentTask = parent }
            }.AsQueryable();
        }
        [TestMethod]
        public void ShouldGetTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            var process = new TaskManagerProcess(connector.Object);
            var result = process.GetTasks();
            Assert.IsNotNull(result);
            Assert.AreEqual(tasks.Count(), result.Count());
        }
        [TestMethod]
        public void ShouldGetParentTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllParentTasks(It.IsAny<int>())).Returns(tasks);
            var process = new TaskManagerProcess(connector.Object);
            var result = process.GetParentTasks(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(tasks.Count(), result.Count());
        }
        [TestMethod]
        public void ShouldGetTaskByTaskId()
        {
            var taskId = 1;
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetTaskById(It.IsAny<int>())).Returns<int>(
                id => { return tasks.First(t => t.ID == id); });
            var process = new TaskManagerProcess(connector.Object);
            var result = process.GetTaskByTaskId(taskId);
            Assert.IsNotNull(result);
            Assert.AreEqual(taskId, result.Id);
        }
        [TestMethod]
        public void ShouldNotGetTaskByTaskId()
        {
            var taskId = 0;
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetTaskById(It.IsAny<int>())).Returns<int>(
                id => { return tasks.FirstOrDefault(t => t.ID == id); });
            var process = new TaskManagerProcess(connector.Object);
            var result = process.GetTaskByTaskId(taskId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldAddTasks()
        {
            var newTaskId = new Random().Next(4, 10);
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.AddTask(It.IsAny<Task>())).Callback<Task>(t => { t.ID = newTaskId; });
            var process = new TaskManagerProcess(connector.Object);
            var result = process.AddTask(new Entities.Task { Id = 0, Name="New-Task" });
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldNotAddTasks()
        {
            var newTaskId = new Random().Next(4, 10);
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.AddTask(It.IsAny<Task>())).Callback<Task>(t => { t.ID = newTaskId; });
            var process = new TaskManagerProcess(connector.Object);
            var result = process.AddTask(new Entities.Task { Id = 1, Name = "New-Task" });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldUpdateTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.EditTask(It.IsAny<Task>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.EditTask(new Entities.Task { Id = 1, Name = "New-Task" });
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldNotUpdateTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.EditTask(It.IsAny<Task>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.EditTask(new Entities.Task { Id = 0 });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldEndTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.EndTask(It.IsAny<int>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.EndTask(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldNotEndTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.EndTask(It.IsAny<int>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.EndTask(0);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldDeleteTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.DeleteTask(It.IsAny<int>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.DeleteTask(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldNotDeleteTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            connector.Setup(m => m.GetAllTasks()).Returns(tasks);
            connector.Setup(m => m.DeleteTask(It.IsAny<int>()));
            var process = new TaskManagerProcess(connector.Object);
            var result = process.DeleteTask(0);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }
    }
}
