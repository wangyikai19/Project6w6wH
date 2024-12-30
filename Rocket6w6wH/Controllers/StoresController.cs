using Rocket6w6wH.Migrations;
using Rocket6w6wH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace Rocket6w6wH.Controllers
{
    public class StoresController : ApiController
    {
        // GET: api/Stores
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Stores/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Stores
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Stores/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Stores/5
        public void Delete(int id)
        {
        }
        [HttpGet]
        [Route("api/allstores")]
        public IHttpActionResult GetAllStores()
        {
            try
            {
                using (var context = new Model())
                {
                    // 查詢所有店家
                    var stores = context.Stores.ToList();
                    var result = stores.Select(store => new
                    {
                        store.StoreName,
                        store.AddressCh,
                        store.AddressEn,
                        store.PriceStart,
                        store.PriceEnd,
                        store.Phone,
                        store.ReserveUrl,
                        store.BusinessHours,
                        store.XLocation,
                        store.YLocation,
                    });
                    if (stores == null || stores.Count == 0)
                    {
                        return NotFound(); // 如果沒有店家資料，返回 404
                    }
                    return Ok(
                        new
                        {
                            result
                        });
                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }
        [HttpGet]
        [Route("api/stores/{id}")]
        public IHttpActionResult GetStores(int id)
        {
            try
            {
                using (var context = new Model())
                {
                    var store = context.Stores.FirstOrDefault(m => m.Id == id);
                  
                    if (store == null)
                    {
                        return NotFound(); //如果未找到店家，返回404
                    }
                    else
                    {
                        var storeStars = context.StoreStars.Where(m => m.StoreId == id).ToList();
                        if (!storeStars.Any())//如果沒有評星數
                        {
                            return Ok(new
                            {
                                store.StoreName,
                                store.AddressCh,
                                store.AddressEn,
                                store.PriceStart,
                                store.PriceEnd,
                                store.Phone,
                                store.ReserveUrl,
                                store.BusinessHours,
                                store.XLocation,
                                store.YLocation,
                                AverageStars = 0,//預設為0星
                            });
                        }
                        else
                        {
                            var averageStars = storeStars.Average(m => m.Stars);
                            return Ok(new
                            {
                                store.StoreName,
                                store.AddressCh,
                                store.AddressEn,
                                store.PriceStart,
                                store.PriceEnd,
                                store.Phone,
                                store.ReserveUrl,
                                store.BusinessHours,
                                store.XLocation,
                                store.YLocation,
                                AverageStars = averageStars,
                            });
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }
        [HttpGet]
        [Route("api/storesstars/{id}")]
        public IHttpActionResult GetStoresStars(int id)
        {
            try
            {
                using (var context = new Model())
                {
                    var storeStars = context.StoreStars.Where(m => m.StoreId == id).ToList();
                    if (!storeStars.Any())
                    {
                        return Ok(new
                        {
                            AverageStars = 0,
                        });
                    }
                    else
                    {
                        var averageStars = storeStars.Average(m => m.Stars);
                        return Ok(new
                        {
                            AverageStars = averageStars
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }
        [HttpGet]
        [Route("api/storescomments/{id}")]
        public IHttpActionResult GetStoresComments(int id)
        {
            try
            {
                using (var context = new Model())
                {
                    var StoreComments = context.StoreComments.Where(m => m.StoreId == id).Include(m => m.Member).ToList();
                    var result = StoreComments.Select(comment => new
                    {
                        comment.MemberId,
                        comment.Member.Name,
                        comment.Comment,

                    });
                    if (!StoreComments.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        
                        return Ok(new
                        {
                            result
                        });
                    }

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
