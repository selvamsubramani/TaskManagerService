using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
        IEnumerable<Task> tasks;
        [TestInitialize]
        public void Setup()
        {
            Task parent = new Task { Id = 1, Name = "Task-01", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 5 };
            tasks = new Task[]
            {
                parent,
                new Task { Id = 2, Name = "Task-02", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 10, Parent = parent },
                new Task { Id = 3, Name = "Task-03", StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue, Priority = 15, Parent = parent }
            };
        }

        [TestMethod]
        public void ShouldGetAllTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.GetTasks()).Returns(tasks);
            var controller = new TaskController(process.Object);
            var output = controller.GetAllTasks();
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(OkNegotiatedContentResult<Task[]>));
            var result = output as OkNegotiatedContentResult<Task[]>;
            Assert.AreEqual(tasks.Count(), result.Content.Count());
        }

        [TestMethod]
        public void ShouldGetNoTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            var taskInput = new Task[] { };
            process.Setup(m => m.GetTasks()).Returns(taskInput.AsQueryable());
            var controller = new TaskController(process.Object);
            var output = controller.GetAllTasks();
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ResponseMessageResult));
            var result = output as ResponseMessageResult;
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
            var output = controller.GetAllTasks();
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Value cannot be null.\r\nParameter name: source", result.Exception.Message);
        }

        [TestMethod]
        public void ShouldGetAllParentTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.GetParentTasks(0)).Returns(tasks);
            var controller = new TaskController(process.Object);
            var output = controller.GetAllParentTasks(0);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(OkNegotiatedContentResult<Task[]>));
            var result = output as OkNegotiatedContentResult<Task[]>;
            Assert.AreEqual(tasks.Count(), result.Content.Count());
        }

        [TestMethod]
        public void ShouldGetNoParentTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            var taskInput = new Task[] { };
            process.Setup(m => m.GetParentTasks(0)).Returns(taskInput.AsQueryable());
            var controller = new TaskController(process.Object);
            var output = controller.GetAllParentTasks(0);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ResponseMessageResult));
            var result = output as ResponseMessageResult;
            Assert.IsNull(result.Response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, result.Response.StatusCode);
        }
        [TestMethod]
        public void ShouldGetErrorOnGetParentTasks()
        {
            var process = new Mock<ITaskManagerProcess>();
            IQueryable<Task> taskInput = null;
            process.Setup(m => m.GetParentTasks(0)).Returns(taskInput);
            var controller = new TaskController(process.Object);
            var output = controller.GetAllParentTasks(0);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Value cannot be null.\r\nParameter name: source", result.Exception.Message);
        }

        [TestMethod]
        public void ShouldGetTaskByTaskId()
        {
            var process = new Mock<ITaskManagerProcess>();
            var task = tasks.FirstOrDefault(t => t.Id == 1);
            process.Setup(m => m.GetTaskByTaskId(1)).Returns(task);
            var controller = new TaskController(process.Object);
            var output = controller.GetTaskByTaskId(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(OkNegotiatedContentResult<Task>));
            var result = output as OkNegotiatedContentResult<Task>;
            Assert.AreEqual(task.Id, result.Content.Id);
        }

        [TestMethod]
        public void ShouldGetNoTaskById()
        {
            var process = new Mock<ITaskManagerProcess>();
            var task = tasks.FirstOrDefault(t => t.Id == 0);
            process.Setup(m => m.GetTaskByTaskId(0)).Returns(task);
            var controller = new TaskController(process.Object);
            var output = controller.GetTaskByTaskId(0);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(NotFoundResult));
        }
        [TestMethod]
        public void ShouldGetErrorOnGetTaskById()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.GetTaskByTaskId(0)).Throws(new Exception("Server error"));
            var controller = new TaskController(process.Object);
            var output = controller.GetTaskByTaskId(0);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Server error", result.Exception.Message);
        }
        [TestMethod]
        public void ShouldCreateTask()
        {
            var taskId = new Random().Next(2, 100);
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.AddTask(It.IsAny<Task>())).Returns<Task>(t =>
            {
                t.Id = taskId;
                return true;
            });
            var controller = new TaskController(process.Object);
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/createtask"),
                Method = System.Net.Http.HttpMethod.Post
            };
            var output = controller.CreateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(CreatedNegotiatedContentResult<Task>));
            var result = output as CreatedNegotiatedContentResult<Task>;
            Assert.AreEqual(taskId, result.Content.Id);
        }

        [TestMethod]
        public void ShouldNotCreateTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.AddTask(It.IsAny<Task>())).Returns(false);
            var controller = new TaskController(process.Object);
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/createtask"),
                Method = System.Net.Http.HttpMethod.Post
            };
            var output = controller.CreateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void ShouldGetErrorOnCreateTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.AddTask(It.IsAny<Task>())).Throws(new Exception("Internal Error"));
            var controller = new TaskController(process.Object);
            var output = controller.CreateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Internal Error", result.Exception.Message);
        }

        [TestMethod]
        public void ShouldUpdateTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EditTask(It.IsAny<Task>())).Returns(true);
            var controller = new TaskController(process.Object);
            var output = controller.UpdateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ResponseMessageResult));
            var result = output as ResponseMessageResult;
            Assert.IsNull(result.Response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.Accepted, result.Response.StatusCode);
        }

        [TestMethod]
        public void ShouldNotUpdateTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EditTask(It.IsAny<Task>())).Returns(false);
            var controller = new TaskController(process.Object);
            var output = controller.UpdateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void ShouldGetErrorOnUpdateTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EditTask(It.IsAny<Task>())).Throws(new Exception("Internal Error"));
            var controller = new TaskController(process.Object);
            var output = controller.UpdateTask(new Task { });
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Internal Error", result.Exception.Message);
        }

        [TestMethod]
        public void ShouldCloseTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EndTask(It.IsAny<int>())).Returns(true);
            var controller = new TaskController(process.Object);
            var output = controller.CloseTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ResponseMessageResult));
            var result = output as ResponseMessageResult;
            Assert.IsNull(result.Response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.Accepted, result.Response.StatusCode);
        }

        [TestMethod]
        public void ShouldNotCloseTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EndTask(It.IsAny<int>())).Returns(false);
            var controller = new TaskController(process.Object);
            var output = controller.CloseTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void ShouldGetErrorOnCloseTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.EndTask(It.IsAny<int>())).Throws(new Exception("Internal Error"));
            var controller = new TaskController(process.Object);
            var output = controller.CloseTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Internal Error", result.Exception.Message);
        }

        [TestMethod]
        public void ShouldDeleteTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.DeleteTask(It.IsAny<int>())).Returns(true);
            var controller = new TaskController(process.Object);
            var output = controller.DeleteTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ResponseMessageResult));
            var result = output as ResponseMessageResult;
            Assert.IsNull(result.Response.Content);
            Assert.AreEqual(System.Net.HttpStatusCode.Accepted, result.Response.StatusCode);
        }

        [TestMethod]
        public void ShouldNotDeleteTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.DeleteTask(It.IsAny<int>())).Returns(false);
            var controller = new TaskController(process.Object);
            var output = controller.DeleteTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void ShouldGetErrorOnDeleteTask()
        {
            var process = new Mock<ITaskManagerProcess>();
            process.Setup(m => m.DeleteTask(It.IsAny<int>())).Throws(new Exception("Internal Error"));
            var controller = new TaskController(process.Object);
            var output = controller.DeleteTask(1);
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(ExceptionResult));
            var result = output as ExceptionResult;
            Assert.AreEqual("Internal Error", result.Exception.Message);
        }
    }
}
