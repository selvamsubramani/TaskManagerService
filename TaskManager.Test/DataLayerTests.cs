using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using TaskManager.DataLayer;

namespace TaskManager.Test
{
    [TestClass]
    public class DataLayerTests
    {
        [TestMethod]
        public void ShouldGetAllTasks()
        {
            var mockTasks = new Mock<DbSet<Task>>();
            var model = new Mock<TaskManagerDataModel>();
            model.Setup(x => x.Tasks).Returns(mockTasks.Object);
            var connector = new TaskDataConnector(model.Object);
            var result =connector.GetAllTasks();
            Assert.IsNotNull(result);
        }
    }
}
