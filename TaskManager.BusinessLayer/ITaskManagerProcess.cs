using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Entities;

namespace TaskManager.BusinessLayer
{
    public interface ITaskManagerProcess
    {
        IQueryable<Task> GetTasks();
    }
}
