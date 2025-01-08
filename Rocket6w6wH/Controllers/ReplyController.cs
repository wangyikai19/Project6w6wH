﻿using Microsoft.Ajax.Utilities;
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
                    var replies = context.Reply.Where(c => c.CommentId == commentid).Include(m=>m.Member).ToList();
                    var repliID= replies.Select(r => r.Id).ToList();
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
