using Microsoft.Ajax.Utilities;
using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using static Rocket6w6wH.Controllers.StoresController;

namespace Rocket6w6wH.Controllers
{
    public class ReplyController : ApiController
    {
        private Model db = new Model();
        [HttpGet]
        [Route("api/getreply/{id}")]
        public IHttpActionResult GetReply(int Id)
        {
            try
            {
                using (var context = new Model())
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
                            var result = new
                            {
                                statusCode = 401,
                                status = false,
                                message = "Token 無效或已過期"
                            };
                            return Content(System.Net.HttpStatusCode.Unauthorized, result);
                        }
                    }
                    var replies = context.Reply.Where(c => c.CommentId == Id).Include(m=>m.Member).Include(r=>r.StoreComments).ToList();
                    var comment = context.StoreComments.FirstOrDefault(sc => sc.Id == Id);
                    var repliID= replies.Select(r => r.Id).ToList();
                    var replyLike = context.ReplyLike.ToList();
                    var like = replyLike.Where(l =>  repliID.Contains(l.ReplyId)).Count();
                    var searchCondition = context.SearchCondition.ToList();
                    var reply = replies.Select(r => new
                    {
                        replyId = r.Id,
                        userId = r.ReplyUserId,
                        userName = r.Member?.Name ?? null, // 空值處理
                        userPhoto = r.Member?.Photo ?? null, // 空值處理
                        comment = r.ReplyContent,
                        posterAt = r.CreateTime,
                        badge = r.Member?.Badge ?? null, // 空值處理
                        country = r.Member?.Country ?? null, // 空值處理
                        likeCount = like,
                        isLike = replyLike.Any(rl => rl.LikeUserId == memberId) ? true : false,
                    }).ToList();
                    var data = new
                    {
                        commentId = comment.Id,
                        userId = comment.MemberId,
                        userName = comment.Member.Name,
                        userPhoto = comment.Member.Photo,
                        photos = comment.CommentPictures,
                        starCount = comment.Stars,
                        comment = comment.Comment,
                        postAt = comment.CreateTime.ToString(),
                        likeCount = like,
                        isLike = comment.CommentLikes.Any(cl => cl.LikeUserId == memberId),
                        tags = searchCondition.Where(condition => comment.Label.Split(',')
                       .Select(tag => int.Parse(tag.Trim())).ToList()
                       .Contains(condition.Id)).Select(condition => condition.MVal)
                        .ToList(),
                        badge = comment.Member.Badge,
                        country = comment.Member.Country,
                        reply

                    };
                    var response = new
                    {
                        statusCode = 200,
                        status = true,
                        message = "資料取得成功",
                        data
                    };
                    if (data == null)
                    {
                        var result = new
                        {
                            statusCode = 200,
                            status = true,
                            message = "此評論沒有留言",
                            data
                        };
                        return Ok(result);
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
        [Route("api/comments/reply")]
        public IHttpActionResult Postreply([FromBody] forreply replyvalue)
        {
            try
            {
                var now = DateTime.Now;
                string withMilliseconds = now.ToString("yyyy-MM-dd!HH:mm:ss.fff");

                var ry = new Reply
                {
                    CommentId = replyvalue.commentID,
                    ReplyUserId = replyvalue.userID,
                    ReplyContent = replyvalue.comment,
                    CreateTime = DateTime.Now,
                    ReplyOnlyCode = withMilliseconds
                };
                db.Reply.Add(ry);
                db.SaveChanges();

                var repliesWithMembers = db.Reply.Where(m => m.ReplyOnlyCode == withMilliseconds).Include(m => m.Member).ToList();

                data datadictionary = null;
                foreach (var detail in repliesWithMembers)
                {
                    datadictionary = new data
                    {
                        replyID = detail.Id,
                        userID = detail.ReplyUserId,
                        userName = detail.Member.Name,
                        userPhoto = detail.Member.Photo,
                        comment = detail.ReplyContent,
                        postedAt = detail.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        badge = null,
                        likeCount = 0,
                        isLike = true
                    };
                }

                var response = new
                {
                    statusCode = 200,
                    status = true,
                    message = "評論成功",
                    data = datadictionary
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
        [Route("api/messages/delete")]
        public IHttpActionResult Deletemessages([FromBody] data replyvalue)
        {
            try
            {
                var messages = db.Reply.Find(replyvalue.replyID);

                if (messages == null)
                {
                    return Ok(new
                    {
                        statusCode = 404,
                        status = false,
                        message = "無此留言"
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

        public class forreply
        {
            public int commentID { get; set; }
            public int userID { get; set; }
            public string comment { get; set; }
        }

        public class data
        {
            public int replyID { get; set; }
            public int userID { get; set; }
            public string userName { get; set; }
            public string userPhoto { get; set; }
            public string comment { get; set; }
            public string postedAt { get; set; }
            public string badge { get; set; }
            public int likeCount { get; set; }
            public bool isLike { get; set; }
        }

    }
}
