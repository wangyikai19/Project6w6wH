using Newtonsoft.Json;
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
using static Google.Apis.Requests.BatchRequest;

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
                    var storeStars = context.StoreStars.Where(m => m.StoreId == id).ToList();
                    var tagIds = store.StoreTags.Split(',').Select(tag => int.Parse(tag.Trim())).ToList();
                    var searchConditions = context.SearchCondition.Where(sc => tagIds.Contains(sc.Id)).ToList();
                    var tags = searchConditions.Select(sc => new
                    {
                        tagId = sc.Id,
                        tagName = sc.Mavl,
                    }).ToList();
                    var averageStars = 0;
                    if (storeStars.Count > 0)
                    {
                        averageStars = (int)Math.Round(storeStars.Average(m => m.Stars));
                    }

                    var openingHours = JsonConvert.DeserializeObject<Dictionary<string, string>>(store.BusinessHours);
                    var data = new
                    {
                        advertise = new
                        {
                            photo = "", 
                            url = "",     
                            title = "", 
                            slogan = ""
                        },
                        starCount = averageStars,
                        tags,
                        isFavorited = "", 
                        placeId = "",
                        location = new { lat = store.XLocation, lng = store.YLocation }, 
                        displayName = store.StoreName, 
                        photos ="",
                        address = store.AddressCh,
                        enAddress = store.AddressEn,
                        ok = store.ReserveUrl,
                        budget = store.PriceStart+"+"+ store.PriceEnd,
                        phone = store.Phone,
                        url = "",
                        opening_hours = openingHours
                    };
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "資料取得成功",
                        data
                    };
                    if (store == null)
                    {
                        return NotFound(); //如果未找到店家，返回404
                    }
                    else
                    {
                        return Ok(response);

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
        [Route("api/stores/search")]
        public IHttpActionResult SearchStores([FromBody] SearchStoresRequest request)
        {
            try
            {
                using (var context = new Model())
                {
                    var locationType = request.LocationType;
                    var latitude = request.Latitude;
                    var longitude = request.Longitude;
                    var stationId = request.StationId;
                    var stores = context.Stores.ToList();
                    // 根据 locationType 进行过滤
                    if (locationType == "user")
                    {
                        // 使用经纬度进行过滤（假设存储了经纬度字段）
                        //stores = stores.Where(store => store.XLocation == latitude && store.YLocation == longitude);
                    }
                    else if (!string.IsNullOrEmpty(stationId))
                    {
                        // 根据 StationId 查找对应的车站或其他相关逻辑
                        //stores = stores.Where(store => store.StationId == stationId);
                    }
                    if (request.Category != null && request.Category.Count > 0)
                    {
                        var categoryIds = request.Category.Select(c => c.CategoryId).ToList();
                    }
                    if (request.Service != null && request.Service.Count > 0)
                    {
                        var serviceIds = request.Service.Select(s => s.ServiceId).ToList();
                        //stores = stores.Where(store => serviceIds.Contains(store.ServiceId));
                    }

                    var result = stores.ToList();
                    return Ok(stores);

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
                    var storeStars = context.StoreComments.Where(m => m.StoreId == id).ToList();
                    if (!storeStars.Any())
                    {
                        return Ok(new
                        {
                            AverageStars = 0,
                        });
                    }
                    else
                    {
                        var averageStars = (int)Math.Round(storeStars.Average(m => m.Stars));
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
                    var storeComments = context.StoreComments.Where(m => m.StoreId == id).Include(m => m.Member).ToList();
                    var result = storeComments.Select(comment => new
                    {
                        comment.MemberId,
                        comment.Member.Name,
                        comment.Comment,
                        comment.Stars,

                    });
                    if (!storeComments.Any())
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
        public class SearchStoresRequest
        {
            public string LocationType { get; set; } // 可选"user"或"station"
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public string StationId { get; set; }
            public List<Category> Category { get; set; } // 类别列表
            public List<Service> Service { get; set; } // 服务项目列表
        }
        public class Category
        {
            public int CategoryId { get; set; }
            public string Name { get; set; }
        }

        public class Service
        {
            public int ServiceId { get; set; }
            public string Name { get; set; }
        }
    }
}
