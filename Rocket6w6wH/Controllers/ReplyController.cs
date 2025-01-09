using Microsoft.Ajax.Utilities;
using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Text.Json;
using static Rocket6w6wH.Controllers.StoresController;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Rocket6w6wH.Controllers
{
    public class ReplyController : ApiController
    {
        private Model db = new Model();
        [HttpGet]
        [Route("api/getreply")]
        public IHttpActionResult GetReply([FromBody] SearchReply request)
        {
            try
            {
                using (var context = new Model())
                {
                    var commentid = request.CommentId;
                    var userid = request.UserId;
                    var replies = context.Reply.Where(c => c.CommentId == commentid).Include(m => m.Member).ToList();
                    var repliID = replies.Select(r => r.Id).ToList();
                    var like = context.ReplyLike.Where(l => l.LikeUserId == userid && repliID.Contains(l.ReplyId)).Count();
                    var data = replies.Select(r => new
                    {
                        replyID = r.Id,
                        userID = r.ReplyUserId,
                        userName = r.Member?.Name ?? null, // 空值處理
                        userPhoto = r.Member?.Photo ?? null, // 空值處理
                        comment = r.ReplyContent,
                        posterAt = r.CreateTime,
                        badge = r.Member?.Badge ?? null, // 空值處理
                        country = r.Member?.Country ?? null, // 空值處理
                        likeCount = 1,
                        isLike = like,
                    }).ToList();
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "資料取得成功",
                        data
                    };
                    if (data == null || data.Count == 0)
                    {
                        return NotFound(); // 如果沒有店家資料，返回 404
                    }
                    return Ok(response);

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"伺服器處理請求時發生錯誤: {ex.Message}", ex));
            }
        }
        public class SearchReply
        {
            public int CommentId { get; set; }
            public int UserId { get; set; }
        }

        [HttpPost]
        [JwtAuthFilter]
        [Route("api/comments/reply")]
        public IHttpActionResult Postreply([FromBody] Forreply replyvalue)
        {
            try
            {
                int userId = (int)HttpContext.Current.Items["memberid"];
                var now = DateTime.Now;
                string withMilliseconds = now.ToString("yyyy-MM-dd!HH:mm:ss.fff");

                var Comment = db.StoreComments.Find(replyvalue.CommentId);
                if (Comment == null)
                {
                    return Ok(new
                    {
                        statusCode = 404,
                        status = false,
                        message = "無此評論"
                    });
                }

                var ry = new Reply
                {
                    CommentId = replyvalue.CommentId,
                    ReplyUserId = userId,
                    ReplyContent = replyvalue.Comment,
                    CreateTime = DateTime.Now,
                    ReplyOnlyCode = withMilliseconds
                };
                db.Reply.Add(ry);
                db.SaveChanges();

                var repliesWithMembers = db.Reply.Where(m => m.ReplyOnlyCode == withMilliseconds).Include(m => m.Member).ToList();

                var dataList = new List<object>();
                foreach (var detail in repliesWithMembers)
                {
                    string savePath = null;
                    if (detail.Member.Photo != null)
                    {
                        string userPath = ConfigurationManager.AppSettings["UserPhoto"];
                        savePath = Path.Combine(userPath, detail.Member.Photo);
                    }

                    var datadictionary = new 
                    {
                        replyId = detail.Id,
                        userId = detail.ReplyUserId,
                        userName = detail.Member.Name,
                        userPhoto = savePath == null ? null : savePath,
                        comment = detail.ReplyContent,
                        postedAt = detail.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        badge = "level1",
                        likeCount = 0,
                        isLike = false,
                    };
                    dataList.Add(datadictionary);
                }

                var response = new
                {
                    statusCode = 200,
                    status = true,
                    message = "留言成功",
                    data = dataList[0]
                };

                return Ok(response);
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


        [HttpPost]
        [JwtAuthFilter]
        [Route("api/messages/delete")]
        public IHttpActionResult Deletemessages([FromBody] Forreply replyvalue)
        {
            try
            {
                int userId = (int)HttpContext.Current.Items["memberid"];
                var messages = db.Reply.Find(replyvalue.ReplyId);

                if (messages == null)
                {
                    return Ok(new
                    {
                        statusCode = 404,
                        status = false,
                        message = "無此留言"
                    });
                }

                int rId = db.Reply.Where(m => m.Id == replyvalue.ReplyId).Select(x => x.ReplyUserId).FirstOrDefault();
                if (userId != rId)
                {
                    return Ok(new
                    {
                        statusCode = 404,
                        status = false,
                        message = "用戶無此留言"
                    });
                }

                db.Reply.Remove(messages);
                db.SaveChanges();
                return Ok(new
                {
                    statusCode = 200,
                    status = true,
                    message = "刪除成功！"
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

        public class Forreply
        {
            public int CommentId { get; set; }
            public string Comment { get; set; }
            public int ReplyId { get; set; }
        }
    }
}
