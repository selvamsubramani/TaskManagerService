using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using TaskManager.API.Controllers;
using TaskManager.BusinessLayer;
using TaskManager.Entities;

namespace TaskManager.Test
{
    [TestClass]
    public class APITests
    {
        [TestMethod]
        public void ShouldGetAllTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            var taskInput = new Task[] { new Task { }, new Task { } };
            process.Setup(m => m.GetTasks()).Returns(taskInput.AsEnumerable());
            var controller = new TaskController(process.Object);
            var tasks = controller.GetAllTasks();
            Assert.IsNotNull(tasks);
            Assert.IsInstanceOfType(tasks, typeof(OkNegotiatedContentResult<Task[]>));
            var result = tasks as OkNegotiatedContentResult<Task[]>;
            Assert.AreEqual(taskInput.Length, result.Content.Count());
        }

        [TestMethod]
        public void ShouldGetNoTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            var taskInput = new Task[] { };
            process.Setup(m => m.GetTasks()).Returns(taskInput.AsQueryable());
            var controller = new TaskController(process.Object);
            var tasks = controller.GetAllTasks();
            Assert.IsNotNull(tasks);
            Assert.IsInstanceOfType(tasks, typeof(ResponseMessageResult));
            var result = tasks as ResponseMessageResult;
            Assert.IsNull(result.Response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, result.Response.StatusCode);
        }
        [TestMethod]
        public void ShouldGetErrorOnGetAllTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            IQueryable<Task> taskInput = null;
            process.Setup(m => m.GetTasks()).Returns(taskInput);
            var controller = new TaskController(process.Object);
            var tasks = controller.GetAllTasks();
            Assert.IsNotNull(tasks);
            Assert.IsInstanceOfType(tasks, typeof(ExceptionResult));
            var result = tasks as ExceptionResult;
            Assert.AreEqual("Value cannot be null.\r\nParameter name: source", result.Exception.Message);
        }
    }
}
