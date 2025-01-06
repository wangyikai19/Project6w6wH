using Newtonsoft.Json;
using Rocket6w6wH.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Rocket6w6wH
{
    public class often
    {
    }

    //    int storeId = db.Stores.Where(m => m.StoreGoogleId == commentData.placeID).Select(x => x.Id).FirstOrDefault();
    //    var duplicateComment = db.StoreComments.Where(m => m.StoreId == storeId).Select(x => x.MemberId).ToList();

    //            if (duplicateComment.Contains(commentData.userID))
    //            {
    //                var badRresponse = new
    //                {
    //                    statusCode = 404,
    //                    status = false,
    //                    message = "使用者對店家重複評論"
    //                };
    //                return Ok(badRresponse);
    //}//使用storeId與MemberId先確認使用者是否有重複評論


    //List<int> tagslist = new List<int>();
    //var searchConditionTable = db.SearchCondition.ToList();
    //foreach (var tag in commentData.tags)
    //{
    //    int labelId = searchConditionTable.Where(m => m.Mavl == tag).Select(x => x.Id).FirstOrDefault();
    //    tagslist.Add(labelId);
    //};
    //string labelstr = JsonConvert.SerializeObject(tagslist);//將標籤string陣列轉換為標籤DB的id再存成list存入DB


    //var sc = new StoreComments
    //{
    //    StoreId = storeId,
    //    MemberId = commentData.userID,
    //    Comment = commentData.comment,
    //    CreateTime = DateTime.Now,
    //    Stars = commentData.starCount,
    //    Label = labelstr
    //};
    //db.StoreComments.Add(sc);
    //db.SaveChanges();

    //if (httpRequest.Files.Count > 0)
    //{
    //    int commentIdForCommentPictures = db.StoreComments.Where(m => m.StoreId == storeId && m.MemberId == commentData.userID).Select(x => x.Id).FirstOrDefault();
    //    var uploadedFiles = httpRequest.Files;
    //    string uploadPath = ConfigurationManager.AppSettings["UploadPath"];
    //    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    //    foreach (string fileKey in uploadedFiles)
    //    {
    //        var file = uploadedFiles[fileKey];
    //        if (file != null && file.ContentLength > 0)
    //        {
    //            string fileExtension = Path.GetExtension(file.FileName).ToLower();
    //            if (!allowedExtensions.Contains(fileExtension)) continue;

    //            string fileName = Path.GetFileName(file.FileName);
    //            string savePath = Path.Combine(uploadPath, fileName);
    //            file.SaveAs(savePath);

    //            var cp = new CommentPictures
    //            {
    //                CommentId = commentIdForCommentPictures,
    //                PictureUrl = fileName,
    //                CreateTime = DateTime.Now
    //            };
    //            db.CommentPictures.Add(cp);
    //        }
    //    }
    //}
    //db.SaveChanges();
    //var response = new
    //{
    //    statusCode = 200,
    //    status = true,
    //    message = "評論成功"
    //};

    //return Ok(response);


    //public class CatchCommentData
    //{
    //    public string placeID { get; set; }
    //    public int userID { get; set; }
    //    public string comment { get; set; }
    //    public int starCount { get; set; }

    //    public string[] tags { get; set; }
    //}

}