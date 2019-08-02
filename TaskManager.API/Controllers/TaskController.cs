using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManager.BusinessLayer;
using TaskManager.Entities;

namespace TaskManager.API.Controllers
{
    public class TaskController : ApiController
    {
        private readonly ITaskManagerProcess _process;
        public TaskController() : this(new TaskManagerProcess()) { }
        public TaskController(ITaskManagerProcess process)
        {
            _process = process;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllTasks()
        {
            try
            {
                var result = _process.GetTasks().ToArray();
                if (result.Any())
                    return Ok(result);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllParentTasks(int id)
        {
            try
            {
                var result = _process.GetParentTasks(id).ToArray();
                if (result.Any())
                    return Ok(result);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult GetTaskByTaskId(int id)
        {
            try
            {
                var result = _process.GetTaskByTaskId(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreateTask(Task task)
        {
            try
            {
                if (_process.AddTask(task))
                    return Created(new Uri(Request.RequestUri, $"GetTaskByTaskId/{task.Id}"), task);
                return BadRequest("Task is already available.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateTask(Task task)
        {
            try
            {
                if (_process.EditTask(task))
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Accepted));
                return BadRequest("Task is not found to update.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult CloseTask(int id)
        {
            try
            {
                if (_process.EndTask(id))
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Accepted));
                return BadRequest("Task is not found or already closed.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteTask(int id)
        {
            try
            {
                if (_process.DeleteTask(id))
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Accepted));
                return BadRequest("Task is not found or already deleted.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
