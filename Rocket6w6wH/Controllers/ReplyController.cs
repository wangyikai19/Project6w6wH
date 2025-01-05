using Microsoft.Ajax.Utilities;
using Rocket6w6wH.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Rocket6w6wH.Controllers.StoresController;

namespace Rocket6w6wH.Controllers
{
    public class ReplyController : ApiController
    {
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
                    var replies = context.Reply.Where(c => c.CommentId == commentid).Include(m=>m.Member).ToList();
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
                        isLike = true,
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
    }
}
