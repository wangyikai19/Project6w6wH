using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Rocket6w6wH.Models;

namespace Rocket6w6wH.Controllers
{
    public class CommentController : ApiController
    {
        private Model db = new Model();

        //GET: api/Comment
        //public IQueryable<StoreComments> GetStoreComments()
        //{
        //    return db.StoreComments;
        //}

        [HttpPost]
        [Route("api/test")]
        public IHttpActionResult GetStoreComments([FromBody] testData Value)
        {
            var sortedNumbers = Value.numberlist.OrderBy(n => n);

            var td = new
            {
                numberlist = sortedNumbers
            };

            return Ok(td);
        }

        // PUT: api/Comment/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutStoreComments(int id, StoreComments storeComments)
        //{
        //}

        [HttpPost]
        [Route("api/reviews")]
        public IHttpActionResult PostComments([FromBody] CatchCommentData commentData)
        {
            int storeId = db.Stores.Where(m => m.StoreGoogleId == commentData.placeID).Select(x => x.Id).FirstOrDefault();
            var duplicateComment = db.StoreComments.Where(m => m.StoreId == storeId).Select(x => x.MemberId).ToList();

            if (duplicateComment.Contains(commentData.userID))
            {
                var badRresponse = new
                {
                    statusCode = 404,
                    status = false,
                    message = "使用者對店家重複評論"
                };
                return Ok(badRresponse);
            }


            List<int> tagslist = new List<int>();
            var searchConditionTable = db.SearchCondition.ToList();
            foreach (var tag in commentData.tags)
            {
                int labelId = searchConditionTable.Where(m => m.Mavl == tag).Select(x => x.Id).FirstOrDefault();
                tagslist.Add(labelId);
            };
            string labelstr = JsonConvert.SerializeObject(tagslist);
            

            var sc = new StoreComments
            {
                StoreId = storeId,
                MemberId = commentData.userID,
                Comment = commentData.comment,
                CreateTime = DateTime.Now,
                Stars = commentData.starCount,
                Label = labelstr
            };
            db.StoreComments.Add(sc);
            db.SaveChanges();

            var response = new
            {
                statusCode = 200,
                status = true,
                message = "評論成功"
            };

            return Ok(response);
        }

        // DELETE: api/Comment/5
        //[ResponseType(typeof(StoreComments))]
        //public IHttpActionResult DeleteStoreComments(int id)
        //{
        //    StoreComments storeComments = db.StoreComments.Find(id);
        //    if (storeComments == null)
        //    {
        //        return NotFound();
        //    }

        //    db.StoreComments.Remove(storeComments);
        //    db.SaveChanges();

        //    return Ok(storeComments);
        //}


    }
}