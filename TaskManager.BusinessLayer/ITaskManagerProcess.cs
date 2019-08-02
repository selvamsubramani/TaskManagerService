using System.Collections.Generic;
using TaskManager.Entities;

namespace TaskManager.BusinessLayer
{
    public interface ITaskManagerProcess
    {
        IEnumerable<Task> GetTasks();
        IEnumerable<Task> GetParentTasks(int id);
        Task GetTaskByTaskId(int id);
        bool AddTask(Task task);
        bool EditTask(Task task);
        bool EndTask(int id);
        bool DeleteTask(int id);
    }
}
