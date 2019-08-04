using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskManager.DataLayer;

namespace TaskManager.Test
{
    [TestClass]
    public class DataLayerTests
    {
        Mock<DbSet<Task>> mockTasks;
        List<Task> tasks;
        [TestInitialize]
        public void Setup()
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

            mockTasks.As<IDbSet<Task>>().Setup(m => m.Add(It.IsAny<Task>())).Returns<Task>(t =>
            {
                t.ID = new Random().Next(4, 10);
                tasks.Add(t);
                return t;
            });

            mockTasks.As<IDbSet<Task>>().Setup(m => m.Remove(It.IsAny<Task>())).Returns<Task>(t =>
            {
                tasks.Remove(t);
                return t;
            });
        }
        [TestMethod]
        public void ShouldGetAllTasks()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            var result = connector.GetAllTasks();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void ShouldGetAllParentTasks()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            var result = connector.GetAllParentTasks(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldGetTaskById()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            var result = connector.GetTaskById(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ShouldAddTask()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            connector.AddTask(new Task { Description = "New Task" });
            var newtask = model.Object.Tasks.FirstOrDefault(t => t.Description == "New Task");
            Assert.IsNotNull(newtask);
        }

        [TestMethod]
        public void ShouldEditTask()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            var editTask = model.Object.Tasks.FirstOrDefault(t => t.ID == 1);
            editTask.Description = "Edited - Task";
            connector.EditTask(editTask);
            var task = model.Object.Tasks.FirstOrDefault(t => t.ID == 1);
            Assert.IsNotNull(task);
            Assert.AreEqual(editTask.Description, task.Description);
        }

        [TestMethod]
        public void ShouldEndTask()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            connector.EndTask(1);
            var task = model.Object.Tasks.FirstOrDefault(t => t.ID == 1);
            Assert.IsNotNull(task);
            Assert.AreEqual(true, task.Status);
        }

        [TestMethod]
        public void ShouldDeleteTask()
        {
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            connector.DeleteTask(1);
            var task = model.Object.Tasks.FirstOrDefault(t => t.ID == 1);
            Assert.IsNull(task);
        }
    }
}
