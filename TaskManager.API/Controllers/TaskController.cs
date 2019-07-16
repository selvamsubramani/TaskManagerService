using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManager.BusinessLayer;

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
        // GET: api/Task
        public IHttpActionResult GetAllTasks()
        {
            try
            {
                var result = _process.GetTasks();
                if (result.Any())
                    return Ok(result);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// GET: api/Task/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Task
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Task/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Task/5
        //public void Delete(int id)
        //{
        //}
    }
}
