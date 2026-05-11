//using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/list")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ListController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<TODOList> GetLists()
        {
            return Ok(_db.tfList.ToList());
        }

        //[HttpGet("Filter_listId")]
        //public ActionResult<TODOList> FilterListId([FromQuery] int? Max_id , [FromQuery] int? Min_id)
        //{
        //    var list = _db.tfList.Where(l => l.Id >=Min_id && l.Id <= Max_id).ToList();
        //    return Ok(list);
        //}


        [HttpGet("{id:int}")]
        public ActionResult<TODOList> getOneList(int id)
        {
            var list = _db.tfList.FirstOrDefault(l => l.Id == id);
            return Ok(list);
        }

        [HttpPost]
        public ActionResult<TODOList> listForTask([FromBody]TODOList list) 
        {
            _db.tfList.Add(list);
            _db.SaveChanges();
            return Ok(list);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TODOList> listUpdate([FromBody]TODOList list , int id)
        {
            var updating_list = _db.tfList.FirstOrDefault(l => l.Id == id);
            updating_list.Name = list.Name;
            _db.SaveChanges();
            return Ok(updating_list);
        }


        [HttpDelete("{id:int}")]
        public ActionResult<TODOList> listDelete(int id)
        {
            var list = _db.tfList.FirstOrDefault(l => l.Id == id);
            _db.tfList.Remove(list);
            _db.SaveChanges();
            return Ok(list);
        }
        

    }
}
