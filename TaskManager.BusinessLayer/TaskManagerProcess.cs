using System.Collections.Generic;
using System.Linq;
using TaskManager.DataLayer;
using System;
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.Task> GetTasks()
        {
            return _connector.GetAllTasks().ToArray().Select(x => ConvertToEntityTask(x));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Entities.Task> GetParentTasks(int id)
        {
            return _connector.GetAllParentTasks(id).ToArray().Select(x => ConvertToEntityTask(x));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Task GetTaskByTaskId(int id)
        {
            var task = _connector.GetTaskById(id);
            if (task != null)
            {
                return ConvertToEntityTask(task);
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public bool AddTask(Entities.Task task)
        {
            if (CheckTask(task.Id, task.Name))
                return false;
            else
            {
                var dataTask = ConvertToDataTask(task);
                _connector.AddTask(dataTask);
                task.Id = dataTask.ID;
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public bool EditTask(Entities.Task task)
        {
            if (CheckTask(task.Id, task.Name))
            {
                _connector.EditTask(ConvertToDataTask(task));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public bool EndTask(int id)
        {
            if (CheckTaskEligibleForClose(id))
            {
                _connector.EndTask(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteTask(int id)
        {
            if (CheckTask(id))
            {
                _connector.DeleteTask(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private Entities.Task ConvertToEntityTask(DataLayer.Task task)
        {
            return
                new Entities.Task
                {
                    Id = task.ID,
                    Name = task.Description,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    Status = task.Status,
                    Parent = task.ParentID.HasValue ? ConvertToEntityTask(task.ParentTask) : null
                };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private Task ConvertToDataTask(Entities.Task task)
        {
            return
                new Task
                {
                    ID = task.Id,
                    Description = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    Status = task.Status,
                    ParentID = task.Parent?.Id
                };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckTask(int id, string name = null)
        {
            var tasks = GetTasks();
            if (string.IsNullOrEmpty(name))
                return tasks.Any(task => task.Id == id);
            return tasks.Any(task => task.Id == id || task.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckTaskEligibleForClose(int id)
        {
            var tasks = GetTasks();
            return tasks != null && tasks.Any(task => task.Id == id && !task.Status);
        }
    }
}