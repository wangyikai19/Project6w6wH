using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static Google.Apis.Requests.BatchRequest;

namespace Rocket6w6wH.Controllers
{
    public class StoresController : ApiController
    {
        private Model db = new Model();
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

        [HttpGet]
        [Route("api/addstore")]
        public IHttpActionResult Post([FromBody] AddRequest request)
        {
            if (request == null)
            {
                var response = new
                {
                    statusCode = 404,
                    status = false,
                    message = "送出失敗，請再試一次",
                };
                return Ok(response);
            }

            try
            {
                using (var context = new Model())
                {
                    var addStore = new Stores
                    {
                        StoreGoogleId = request.PlaceId,
                        StoreName = request.DisplayName,
                        AddressCh = request.Address,
                        XLocation = request.XLocation,
                        YLocation = request.YLocation,
                        StoreTags = request.Tags,
                        CreateTime = DateTime.Now
                    };

                    context.Stores.Add(addStore);
                    context.SaveChanges();
                }
                var response = new
                {
                    statusCode = 200,
                    status = true,
                    message = "成功送出",
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // 處理異常，並記錄詳細錯誤資訊
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }

        }

        // PUT: api/Stores/5
        public void Put([FromBody] string value)
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
        [Route("api/stores/{storeid}")]
        public IHttpActionResult GetStores(int storeid)
        {
            try
            {
                var request = Request;
                int? memberId = null;
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {
                    memberId = 0;
                }
                else
                {
                    try
                    {
                        var jwtObject = JwtAuthUtil.GetToken(request.Headers.Authorization.Parameter);
                        memberId = int.Parse(jwtObject["Id"].ToString());
                    }
                    catch
                    {
                        var response = new
                        {
                            statusCode = 401,
                            status = false,
                            message = "Token 無效或已過期"
                        };
                        return Content(System.Net.HttpStatusCode.Unauthorized, response);
                    }
                }
                using (var context = new Model())
                {
                    var store = context.Stores.FirstOrDefault(m => m.Id == storeid);
                    var mid = context.CollectStore.Where(c => c.StoreId == storeid & c.MemberId == memberId);
                    if (store != null)
                    {
                        var storeStars = context.StoreComments.Where(m => m.StoreId == storeid).ToList();
                        var comments = context.StoreComments.ToList();
                        string idr = context.Configs.FirstOrDefault(i => i.Group == "IDR").MVal;
                        int IDR = int.Parse(idr);
                        var averageStars = 0;
                        if (storeStars.Count > 0)
                        {
                            averageStars = (int)Math.Round(storeStars.Average(m => m.Stars));
                        }
                        var openingHours = JsonConvert.DeserializeObject<Dictionary<string, string>>(store.BusinessHours);
                        var tags = comments
                                    .Where(c => !string.IsNullOrEmpty(c.Label) && c.StoreId == store.Id)
                                    .SelectMany(c => c.Label.Split(','))
                                    .GroupBy(label => label.Trim())
                                    .ToDictionary(group => group.Key, group => group.Count());

                        // 如果 tags 為空，回傳空陣列
                        if (tags.Count == 0)
                        {
                            tags = new Dictionary<string, int>(); // 空的字典
                        }
                        var data = new
                        {
                            advertise = new
                            {
                                photo = "",
                                url = "",
                                title = "",
                                slogan = ""
                            },
                            id=store.Id,
                            starCount = averageStars,
                            tags = comments
                                    .Where(c => !string.IsNullOrEmpty(c.Label) && c.StoreId == store.Id)
                                    .SelectMany(c => c.Label.Split(','))
                                    .GroupBy(label => label.Trim())
                                    .ToDictionary(group => group.Key, group => group.Count()),
                            isFavorited = mid.Count() > 0 ? true : false,
                            placeId = store.StoreGoogleId,
                            location = new { lat = store.XLocation, lng = store.YLocation },
                            displayName = store.StoreName,
                            photos = store.StorePictures,
                            address = store.AddressCh,
                            enAddress = store.AddressEn,
                            book = store.ReserveUrl,
                            budget = "NTD " + store.PriceStart + "~" + store.PriceEnd + " / Rp " + store.PriceStart * IDR + "~" + store.PriceEnd * IDR,
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
                        return Ok(response);
                    }
                    else
                    {
                        var response = new
                        {
                            statusCode = 200,
                            status = true,
                            message = "查無店家資料",
                        };
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
        [HttpPost]
        [Route("api/stores/search")]
        public IHttpActionResult SearchStores([FromBody] SearchRequest request)
        {
            try
            {
                using (var context = new Model())
                {
                    const double radiusInKm = 3;
                    var memberID = request.memberID;
                    var locationType = request.LocationType;
                    var cityId = request.CityId;
                    var cityName = context.City.FirstOrDefault(m => m.Id == cityId)?.CountryName ?? string.Empty;
                    var tag = string.Join(",", request.Tags ?? new List<int>());
                    var comments = context.StoreComments.ToList();
                    var stores = context.Stores.ToList();
                    var replies = context.Reply.ToList();
                    var collects = context.CollectStore.ToList();

                    double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
                    {
                        const double earthRadius = 6371.0;
                        double dLat = (lat2 - lat1) * (Math.PI / 180.0);
                        double dLon = (lon2 - lon1) * (Math.PI / 180.0);

                        lat1 *= Math.PI / 180.0;
                        lat2 *= Math.PI / 180.0;

                        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                        return earthRadius * c;
                    }

                    if (locationType == "user")
                    {
                        var latitude = request.Location.Lat;
                        var longitude = request.Location.Lng;
                        stores = stores.Where(s => CalculateDistance(latitude, longitude, (double)s.XLocation, (double)s.YLocation) <= radiusInKm).ToList();
                    }
                    else
                    {
                        stores = stores.Where(store => store.AddressCh != null && store.AddressCh.Contains(cityName)).ToList();
                    }
                    if (stores.Count > 0)
                    {
                        var data = stores.Select(store => new
                        {
                            id=store.Id,
                            placeId = store.StoreGoogleId,
                            displayName = store.StoreName,
                            photos = store.StorePictures,
                            starCount = comments
                            .Where(m => m.StoreId == store.Id)
                            .Select(m => (double?)m.Stars)
                            .DefaultIfEmpty(0)
                            .Average(),
                            tags = comments
                                    .Where(c => !string.IsNullOrEmpty(c.Label) && c.StoreId == store.Id)
                                    .SelectMany(c => c.Label.Split(','))
                                    .GroupBy(label => label.Trim())
                                    .ToDictionary(group => group.Key, group => group.Count()),
                            isAdvertise = store.IsAdvertise,
                            isFavorited = collects.Any(c => c.StoreId == store.Id && c.MemberId == memberID),
                            reviewCount = comments.Where(c => c.StoreId == store.Id).Count(),
                            comments = comments.Where(c => c.StoreId == store.Id).ToList(),
                        }).ToList();
                        var result = new
                        {
                            statusCode = 200,
                            status = true,
                            message = "取得店家列表成功",
                            data
                        };

                        return Ok(result);
                    }
                    else
                    {
                        var randomStores = context.Stores.OrderBy(s => Guid.NewGuid()).Take(5).ToList();
                        var data = randomStores.Select(store => new
                        {
                            placeId = store.StoreGoogleId,
                            displayName = store.StoreName,
                            photos = store.StorePictures,
                            starCount = comments
                            .Where(m => m.StoreId == store.Id)
                            .Select(m => (double?)m.Stars)
                            .DefaultIfEmpty(0)
                            .Average(),
                            tags = comments
                                    .Where(c => !string.IsNullOrEmpty(c.Label) && c.StoreId == store.Id)
                                    .SelectMany(c => c.Label.Split(','))
                                    .GroupBy(label => label.Trim())
                                    .ToDictionary(group => group.Key, group => group.Count()),
                            isAdvertise = store.IsAdvertise,
                            isFavorited = collects.Any(c => c.StoreId == store.Id && c.MemberId == memberID),
                            reviewCount = comments.Where(c => c.StoreId == store.Id).Count(),
                            comments = comments.Where(c => c.StoreId == store.Id).ToList(),
                        }).ToList();
                        var result = new
                        {
                            statusCode = 200,
                            status = true,
                            message = "沒有店家",
                            data
                        };

                        return Ok(result);
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
        [Route("collectshop/get")]
        public IHttpActionResult collectshop()
        {
            try
            {
                var request = Request;
                int? memberId = null;
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "尚未登入",
                    };
                    return Ok(response);
                }
                try
                {
                    var jwtObject = JwtAuthUtil.GetToken(request.Headers.Authorization.Parameter);
                    memberId = int.Parse(jwtObject["Id"].ToString());
                }
                catch
                {
                    var response = new
                    {
                        statusCode = 401,
                        status = false,
                        message = "Token 無效或已過期"
                    };
                    return Content(System.Net.HttpStatusCode.Unauthorized, response);
                }
                var stores = db.CollectStore.Where(c => c.MemberId == memberId).Include(c => c.Stores).ToList();
                var data = stores.Select(store => new
                {
                    placeId = store.Stores.StoreGoogleId,
                    displayName = store.Stores.StoreName,
                    photos = store.Stores.StorePictures,
                    starCount = store.Stores.StoreComments
                                            .Where(m => m.StoreId == store.Id)
                                            .Select(m => (double?)m.Stars)
                                            .DefaultIfEmpty(0)
                                            .Average(),
                    tags = store.Stores.StoreComments
                                                    .Where(c => !string.IsNullOrEmpty(c.Label) && c.StoreId == store.Id)
                                                    .SelectMany(c => c.Label.Split(','))
                                                    .GroupBy(label => label.Trim())
                                                    .ToDictionary(group => group.Key, group => group.Count()),
                    isAdvertise = store.Stores.IsAdvertise,
                    isFavorited = store.Stores.CollectStores.Any(c => c.StoreId == store.Id && c.MemberId == memberId),
                    reviewCount = store.Stores.StoreComments.Where(c => c.StoreId == store.Id).Count(),
                    comments = store.Stores.StoreComments.Where(c => c.StoreId == store.Id).ToList(),
                }).ToList();

                if (data == null || data.Count == 0)
                {
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "用戶無收藏店家",
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "資料取得成功",
                        data
                    };
                    return Ok(response);
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
                var request = Request;
                int? memberId = null;
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {
                    memberId = 0;
                }
                else
                {
                    try
                    {
                        var jwtObject = JwtAuthUtil.GetToken(request.Headers.Authorization.Parameter);
                        memberId = int.Parse(jwtObject["Id"].ToString());
                    }
                    catch
                    {
                        var response = new
                        {
                            statusCode = 401,
                            status = false,
                            message = "Token 無效或已過期"
                        };
                        return Content(System.Net.HttpStatusCode.Unauthorized, response);
                    }
                }
                using (var context = new Model())
                {
                    var storeComments = context.StoreComments.Where(m => m.StoreId == id).Include(m => m.Member).Include(m => m.CommentPictures).Include(m => m.CommentLikes).ToList();
                    var data = storeComments.Select(comment => new
                    {
                        commentID = comment.Id,
                        userID = comment.MemberId,
                        userName = comment.Member.Name,
                        userPhoto = comment.Member.Photo,
                        country = comment.Member.Country,
                        comment = comment.Comment,
                        photo = comment.CommentPictures,
                        replyCount = storeComments.Count(),
                        starCount = (int)Math.Round(storeComments.Select(m => m.Stars).DefaultIfEmpty(0).Average(), MidpointRounding.AwayFromZero),
                        postedAt = comment.CreateTime.ToString(),
                        likeCount = storeComments.Count(),
                        isLike = comment.CommentLikes.Any(sc => sc.LikeUserId == memberId) ? true : false,
                        tags = comment.Label,
                        badge = comment.Member.Badge,


                    });
                    var result = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "資料取得成功",
                        data
                    };
                    if (!storeComments.Any())
                    {
                        result = new
                        {
                            statusCode = 200,
                            status = true,
                            message = "無評論",
                            data
                        };
                        return Ok(result);
                    }
                    else
                    {

                        return Ok(result);
                    }

                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }

        public class SearchRequest
        {
            public int memberID { get; set; }
            public string LocationType { get; set; }
            public Location Location { get; set; }
            public int CityId { get; set; }
            public List<int> Tags { get; set; }
        }

        public class Location
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }
        public class AddRequest
        {
            public string PlaceId { get; set; }
            public string DisplayName { get; set; }
            public string Photo { get; set; }
            public string Address { get; set; }
            public decimal XLocation { get; set; }
            public decimal YLocation { get; set; }
            public string Tags { get; set; }
        }
    }
}
