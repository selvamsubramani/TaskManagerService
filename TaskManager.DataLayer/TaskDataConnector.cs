using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DataLayer
{
    public class TaskDataConnector: ITaskDataConnector
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
    }
}
