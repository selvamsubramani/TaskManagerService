using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using TaskManager.BusinessLayer;
using TaskManager.DataLayer;

namespace TaskManager.Test
{
    [TestClass]
    public class BusinessLayerTests
    {
        [TestMethod]
        public void ShouldGetTasks()
        {
            var connector = new Mock<ITaskDataConnector>();
            var taskInput = new Task[] { new Task { }, new Task { } };
            connector.Setup(m => m.GetAllTasks()).Returns(taskInput.AsQueryable());
            var process = new TaskManagerProcess(connector.Object);
            var result = process.GetTasks();
            Assert.IsNotNull(result);
            Assert.AreEqual(taskInput.Length, result.Count());
        }
    }
}
