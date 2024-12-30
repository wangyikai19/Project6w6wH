using Rocket6w6wH.Models;
using Rocket6w6wH.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace Rocket6w6wH.Controllers
{
    public class ValuesController : ApiController
    {
        //private Models model = new Models.Member();
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("api/user/test")]
        public IHttpActionResult Test(string id)
        {
            string jwt = id;

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

                // 從 JWT 中提取一些信息
                if (jsonToken != null)
                {
                    var issuer = jsonToken.Issuer;
                    var audience = jsonToken.Audiences.FirstOrDefault();
                    var claims = jsonToken.Claims;
                    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                    string jwtToken = jwtAuthUtil.GenerateToken(1);
                    return Ok(new
                    {
                        Status = true,
                        Issuer = issuer,
                        Audience = audience,
                        Claims = claims.Select(c => new { c.Type, c.Value }),
                        JwtToken = jwtToken
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid token: " + ex.Message);
            }

            return Ok(new { Message = "OK" });
        }
        /// <summary>
        /// 驗證登入狀態
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //驗證登入
        // Post api/auth
        [HttpPost]
        [Route("api/v1/auth")]
        public IHttpActionResult Auth([FromBody] authRequest request)
        {
            // 模型驗證
            if (!ModelState.IsValid)
            {
                var errorStr = new
                {
                    statusCode = "400",
                    status = false,
                    message = "錯誤的請求"
                };

                return Ok(errorStr);
            }

            // 取出請求內容，解密 JwtToken 取出資料
            var userToken = JwtAuthUtil.GetToken(Request.Headers.Authorization.Parameter);

            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            // ExpRefreshToken() 生成刷新效期 JwtToken 用法

            string jwtToken = jwtAuthUtil.ExpRefreshToken(userToken);
            // 處理完請求內容後，順便送出刷新效期的 JwtToken

            var responseStr = new
            {
                statusCode = "200",
                status = true,
                message = "用戶已登入"
            };

            return Ok(responseStr);

        }
        public class authRequest
        {
            public string JWTtoken { get; set; }
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet]
        [Route("api/user/Email/{id}")]
        public IHttpActionResult MenberEmail(int id)
        {
            try
            {
                using (var context = new Model())
                {
                    var member = context.Member.FirstOrDefault(m => m.Id == id);
                    if (member == null)
                    {
                        return NotFound(); // 如果未找到會員，返回 404
                    }
                    return Ok(new { Message = member.Email });
                }
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回具體錯誤訊息
                return InternalServerError(new Exception("詳細錯誤訊息", ex));
            }
        }
        // POST api/value
        public void Post([FromBody] string value)
        {
            using (var context = new Model()) 
            {
                var newMember = new Member
                {
                    Email = value,
                                   
                };

                context.Member.Add(newMember);
                context.SaveChanges();
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
