using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rocket6w6wH.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace Rocket6w6wH.Controllers
{
    public class CommentController : ApiController
    {
        private Model db = new Model();

        [HttpPost]
        [Route("api/reviews")]
        public IHttpActionResult PostComments()
        {

            var httpRequest = HttpContext.Current.Request; //從HttpRequest.Files取得前端傳來圖片

            string storeid = httpRequest.Form["placeID"];
            int memberid = int.Parse(httpRequest.Form["userID"]);
            string comment = httpRequest.Form["comment"];
            int starcount = int.Parse(httpRequest.Form["starCount"]);
            string[] tags = JsonConvert.DeserializeObject<string[]>(httpRequest.Form["tags"]);

            int storeId = db.Stores.Where(m => m.StoreGoogleId == storeid).Select(x => x.Id).FirstOrDefault();
            //var duplicateComment = db.Stores.FirstOrDefault(m => m.StoreGoogleId == storeid).StoreComments.Select(x => x.MemberId); 盡量不要將FirstOrDefault()放在where位置
            var duplicateComment = db.Stores.Where(m => m.StoreGoogleId == storeid).SelectMany(m => m.StoreComments.Select(x => x.MemberId));
            if (duplicateComment.Contains(memberid))
            {
                int commentIdForCommentPictures = db.StoreComments.Where(m => m.StoreId == storeId && m.MemberId == memberid).Select(x => x.Id).FirstOrDefault();
                List<int> tagslist = new List<int>();
                var searchConditionTable = db.SearchCondition.ToList();
                foreach (var tag in tags)
                {
                    int labelId = searchConditionTable.Where(m => m.Mavl == tag).Select(x => x.Id).FirstOrDefault();
                    tagslist.Add(labelId);
                };
                string labelstr = JsonConvert.SerializeObject(tagslist);

                var reSC = db.StoreComments.FirstOrDefault(s => s.Id == commentIdForCommentPictures);
                if (reSC != null)
                {
                    reSC.Comment = comment;
                    reSC.ModifyTime = DateTime.Now;
                    reSC.Stars = starcount;
                    reSC.Label = labelstr;
                    db.SaveChanges();
                }

                //var picturesToDelete = db.CommentPictures.Where(c => c.CommentId == commentIdForCommentPictures).ToList();
                //db.CommentPictures.RemoveRange(picturesToDelete);
                //db.SaveChanges();
                var response = new
                {
                    statusCode = 200,
                    status = true,
                    message = "評論修改成功"
                };

                return Ok(response);

            }
            else
            {
                List<int> tagslist = new List<int>();
                var searchConditionTable = db.SearchCondition.ToList();
                foreach (var tag in tags)
                {
                    int labelId = searchConditionTable.Where(m => m.Mavl == tag).Select(x => x.Id).FirstOrDefault();
                    tagslist.Add(labelId);
                };
                string labelstr = JsonConvert.SerializeObject(tagslist);//將標籤string陣列轉換為標籤DB的id再存成list存入DB


                var sc = new StoreComments
                {
                    StoreId = storeId,
                    MemberId = memberid,
                    Comment = comment,
                    CreateTime = DateTime.Now,
                    Stars = starcount,
                    Label = labelstr
                };
                db.StoreComments.Add(sc);
                db.SaveChanges();

                if (httpRequest.Files.Count > 0)
                {
                    int commentIdForCommentPictures = db.StoreComments.Where(m => m.StoreId == storeId && m.MemberId == memberid).Select(x => x.Id).FirstOrDefault();
                    var uploadedFiles = httpRequest.Files;
                    string uploadPath = ConfigurationManager.AppSettings["UploadPath"];
                    string[] allowedExtensions = { ".jpg", ".png" };
                    foreach (string fileKey in uploadedFiles)
                    {
                        var file = uploadedFiles[fileKey];
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileExtension = Path.GetExtension(file.FileName).ToLower();
                            if (!allowedExtensions.Contains(fileExtension)) continue; //檢查檔案類型

                            const int maxFileSizeInBytes = 1 * 1024 * 1024; // 1MB
                            if (file.ContentLength > maxFileSizeInBytes)
                            {
                                continue; // 檔案太大，跳過
                            }

                            string fileName = Path.GetFileName(file.FileName);
                            string savePath = Path.Combine(uploadPath, fileName);

                            if (File.Exists(savePath))
                            {
                                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                fileName = $"{fileNameWithoutExtension}_{timestamp}{fileExtension}";
                                savePath = Path.Combine(uploadPath, fileName);
                            }

                            file.SaveAs(savePath);

                            var cp = new CommentPictures
                            {
                                CommentId = commentIdForCommentPictures,
                                PictureUrl = fileName,
                                CreateTime = DateTime.Now
                            };
                            db.CommentPictures.Add(cp);
                        }
                    }
                }
                db.SaveChanges();

                var response = new
                {
                    statusCode = 200,
                    status = true,
                    message = "評論成功"
                };

                return Ok(response);
            }
        }



        [HttpPost]
        [Route("api/comments/delete")]
        public IHttpActionResult CommentsDelete([FromBody] forComment Commentvalue)
        {
            var Comment = db.StoreComments.Find(Commentvalue.commentID);
            var PictureUrllist = db.CommentPictures.Where(m => m.CommentId == Commentvalue.commentID).Select(x => x.PictureUrl).ToList();
            if (Comment == null)
            {
                return Ok(new
                {
                    statusCode = 404,
                    status = false,
                    message = "無此評論"
                });
            }
            else if (PictureUrllist.Any()) 
            {
                string uploadPath = ConfigurationManager.AppSettings["UploadPath"];
                foreach (string PictureName in PictureUrllist) 
                {
                    string savePath = Path.Combine(uploadPath, PictureName);
                    if (File.Exists(savePath))
                    {
                        File.Delete(savePath); // 刪除舊檔案
                    }
                }
            }

            db.StoreComments.Remove(Comment);
            db.SaveChanges();
            return Ok(new
            {
                statusCode = 200,
                status = true,
                message = "刪除成功！"
            });

        }

        [HttpPost]
        [Route("api/comments/repeat")]
        public IHttpActionResult Commentrepeat([FromBody] forComment Commentvalue)
        {

            int storeId = db.Stores.Where(m => m.StoreGoogleId == Commentvalue.placeID).Select(x => x.Id).FirstOrDefault();
            var duplicateComment = db.Stores.Where(m => m.StoreGoogleId == Commentvalue.placeID).SelectMany(m => m.StoreComments.Where(x => x.MemberId == Commentvalue.userID)).ToList();
            //Select(x => new { x.Label, x.Comment })

            if (!duplicateComment.Any())
            {
                return Ok(new
                {
                    statusCode = 404,
                    status = false,
                    message = "用戶未評論"
                });
            }

            int Commentid = duplicateComment.FirstOrDefault().Id;
            var cps = db.CommentPictures.Where(c => c.CommentId == Commentid).Select(x => x.PictureUrl).ToList();
            var searchConditionTable = db.SearchCondition.ToList();
            List<string> tagslist = new List<string>();
            int[] LabelIDlist = JsonConvert.DeserializeObject<int[]>(duplicateComment.FirstOrDefault().Label);
            foreach (var Labe in LabelIDlist)
            {
                string labelstr = searchConditionTable.Where(m => m.Id == Labe).Select(x => x.Mavl).FirstOrDefault();
                tagslist.Add(labelstr);
            };

            var fc = new forComment
            {
                placeID = Commentvalue.placeID,
                storeID = storeId,
                userID = Commentvalue.userID,
                comment = duplicateComment.FirstOrDefault()?.Comment,
                starCount = duplicateComment.FirstOrDefault().Stars,
                tags = tagslist,
                Pictureslist = cps.Any() ? cps : null
            };
            
            return Ok(fc);
        }




        public class forComment
        {
            public int commentID { get; set; }
            public string placeID { get; set; }
            public int storeID { get; set; }
            public int userID { get; set; }
            public string comment { get; set; }
            public int starCount { get; set; }
            public List<string> tags { get; set; }
            public List<string> Pictureslist { get; set; }
        }
    }
}
