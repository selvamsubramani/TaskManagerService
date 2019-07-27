using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DataLayer
{
    public interface ITaskDataConnector
    {
        IQueryable<Task> GetAllTasks();
        Task GetTaskById(int id);
        void AddTask(Task task);
        void EditTask(Task task);
        void EndTask(int id);
        void DeleteTask(int id);
    }
}
