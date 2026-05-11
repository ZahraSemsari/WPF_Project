//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/Task")]
    [ApiController]
    public class TaskListControllers : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TaskListControllers(ApplicationDbContext db) 
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<TODOTask> getTasks()
        {
            return Ok(_db.tfTask.ToList());
        }

        [HttpGet("Filter_if_Not_checked")]
        public ActionResult<TODOTask> Get_ifNOt_Checked()
        {
            var task_Not_checked = _db.tfTask.Where(t => t.Checked == false).ToList();
            return Ok(task_Not_checked);
        }

        [HttpGet("Filter_if_Starred")]
        public ActionResult<TODOTask> Get_if_Checked()
        {
            var task_Starred = _db.tfTask.Where(t => t.Starred == true).ToList();
            return Ok(task_Starred);
        }

        [HttpGet("Filter_listId")]
        public ActionResult<List<TODOTask>> FilterListId([FromQuery] int listId)
        {
            var tasks = _db.tfTask.Where(t => t.ListId == listId).ToList();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TODOTask> getOneTask(int id)
        {
            var task = _db.tfTask.FirstOrDefault(t  => t.Id == id);
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TODOTask> CreateTask([FromBody] TODOTask task)
        {
            _db.tfTask.Add(task);   
            _db.SaveChanges();
            return Ok(task);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TODOTask> UpdateTask([FromBody] TODOTask task, int id)
        {
            
            if (task == null || task.Id != id)
            {
                return BadRequest("Task data is invalid or does not match the provided ID.");
            }

            var updating_task = _db.tfTask.FirstOrDefault(t => t.Id == id);
            if (updating_task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            updating_task.Name = task.Name;
            updating_task.StartTime = task.StartTime;
            updating_task.EndTime = task.EndTime;
            updating_task.Starred = task.Starred;
            updating_task.Checked = task.Checked;
            updating_task.ListId = task.ListId;

            _db.SaveChanges();

            return Ok(updating_task);
        }


        [HttpDelete("{id:int}")]
        public ActionResult<TODOTask> DeleteTask(int id)
        {
            var task = _db.tfTask.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            _db.tfTask.Remove(task);
            _db.SaveChanges();

            return Ok(task);
        }

    }
}
