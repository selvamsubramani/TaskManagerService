using System;
using System.Linq;

namespace TaskManager.DataLayer
{
    public class TaskDataConnector : ITaskDataConnector
    {
        private readonly TaskManagerDataModel _model;
        //public TaskDataConnector() : this(new TaskManagerDataModel()) { }
        public TaskDataConnector(TaskManagerDataModel model)
        {
            _model = model;
        }

        private static readonly Lazy<TaskDataConnector> lazy = new Lazy<TaskDataConnector>(() => new TaskDataConnector(new TaskManagerDataModel()));
        public static TaskDataConnector Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public IQueryable<Task> GetAllTasks()
        {
            return _model.Tasks;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task GetTaskById(int id)
        {
            return GetAllTasks().FirstOrDefault(task => task.ID == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            _model.Tasks.Add(task);
            _model.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void EditTask(Task task)
        {
            var editedTask = GetTaskById(task.ID);
            if (editedTask != null)
            {
                editedTask.Description = task.Description;
                editedTask.StartDate = task.StartDate;
                editedTask.EndDate = task.EndDate;
                editedTask.Status = task.Status;
                editedTask.Priority = task.Priority;
                editedTask.ParentID = task.ParentID;
                _model.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void EndTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.EndDate = DateTime.Now;
                task.Status = true;
                _model.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _model.Tasks.Remove(task);
                _model.SaveChanges();
            }
        }
    }
}
