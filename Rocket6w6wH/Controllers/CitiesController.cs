using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rocket6w6wH.Controllers
{
    public class CitiesController : ApiController
    {
        // GET: api/Cities
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cities/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cities
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cities/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cities/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/getcity")]
        public IHttpActionResult GetCity()
        {
            try
            {
                using (var context = new Model())
                {
                    // 查詢所有店家
                    var cities = context.City.ToList();
                    var result = cities.Select(city => new
                    {
                        city.Id,
                        city.Area,
                        city.County,
                        city.CountyName,
                    });
                    if (cities == null || cities.Count == 0)
                    {
                        return NotFound(); // 如果沒有店家資料，返回 404
                    }
                    return Ok(new
                    {
                        result
                    });

                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                //return InternalServerError(new Exception("詳細錯誤訊息", ex));
                return InternalServerError(new Exception($"伺服器處理請求時發生錯誤: {ex.Message}", ex));
            }
        }
    }
}
