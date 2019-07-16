using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.DataLayer;
using TaskManager.Entities;

namespace TaskManager.BusinessLayer
{
    public class TaskManagerProcess : ITaskManagerProcess
    {
        private readonly ITaskDataConnector _connector;
        public TaskManagerProcess() : this(TaskDataConnector.Instance) { }
        public TaskManagerProcess(ITaskDataConnector connector)
        {
            _connector = connector;
        }

        public IQueryable<Entities.Task> GetTasks()
        {
            return _connector.GetAllTasks().Select(x =>
            new Entities.Task { });
        }
    }
}
