using Rocket6w6wH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rocket6w6wH.Controllers
{
    public class SearchConditionsController : ApiController
    {
        // GET: api/SearchConditions
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SearchConditions/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SearchConditions
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SearchConditions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SearchConditions/5
        public void Delete(int id)
        {
        }
        [HttpGet]
        [Route("api/searchconditions/{group}")]
        public IHttpActionResult GetSearchConditions(string group)
        {
            try
            {
                using (var context = new Model())
                {
                    var Conditions = context.SearchCondition.Where(s=>s.Group== group).ToList();

                    if (Conditions == null || Conditions.Count == 0)
                    {
                        return NotFound();
                    }
                    return Ok(Conditions.Select(store => new
                    {
                        store.Id,
                        store.Group,
                        store.PVal,
                        store.Mavl,
                    }));
                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }
    }
}
