using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Ajax.Utilities;
using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using static Rocket6w6wH.Controllers.CommentController;

namespace Rocket6w6wH.Controllers
{
    public class MemberController : ApiController
    {
        private Model db = new Model();

        [HttpPost]
        [JwtAuthFilter]
        [Route("api/CollectShop/Collect")]
        public IHttpActionResult PostCollectStores([FromBody] CollectStore CollectSvalue)
        {
            try
            {
                var userId = (int)HttpContext.Current.Items["memberid"];

                var Store = db.Stores.Find(CollectSvalue.StoreId);
                if (Store == null)
                {
                    return Ok(new
                    {
                        statusCode = 404,
                        status = false,
                        message = "無此店家"
                    });
                }

                int collectId = db.CollectStore.Where(m => m.StoreId == CollectSvalue.StoreId && m.MemberId == userId).Select(x => x.Id).FirstOrDefault();
                if (collectId != 0) 
                {
                    var Collect = db.CollectStore.Find(collectId);
                    db.CollectStore.Remove(Collect);
                    db.SaveChanges();
                    return Ok(new
                    {
                        statusCode = 200,
                        status = true,
                        message = "收藏取消"
                    });
                }

                var cs = new CollectStore
                {
                    StoreId = CollectSvalue.StoreId,
                    MemberId = userId,
                    CreateTime = DateTime.Now,
                };
                db.CollectStore.Add(cs);
                db.SaveChanges();

                return Ok(new
                {
                    statusCode = 200,
                    status = true,
                    message = "收藏成功！"
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    statusCode = 500,
                    status = false,
                    message = "伺服器錯誤，請稍後再試",
                    error = ex.Message
                };
                return Content(System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
        }


    }
}