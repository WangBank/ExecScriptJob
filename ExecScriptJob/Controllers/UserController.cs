using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExecScriptJob.JWT;
using ExecScriptJob.Model.ApiDtos;
using ExecScriptJob.Model.FreeSqlHelper;
using ExecScriptJob.Model.Request;
using ExecScriptJob.Model.Response;
using JWTToken.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExecScriptJob.Controllers
{
    /// <summary>
    /// 登录时用
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// 获取配置文件类
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// tokenhelper
        /// </summary>
        private readonly ITokenHelper tokenHelper = null;
        /// <summary>
        /// 主db操作
        /// </summary>
        public IFreeSql _sqliteSql;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqliteSql"></param>
        /// <param name="_tokenHelper"></param>
        /// <param name="_configuration"></param>
        public UserController(IFreeSql<SqlLiteFlag> sqliteSql,ITokenHelper _tokenHelper, IConfiguration _configuration)
        {
            Configuration = _configuration;
            _sqliteSql = sqliteSql;
            tokenHelper = _tokenHelper;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginForm"></param>
        /// <returns>登录成功后返回token</returns>
        [HttpPost]
        public async  Task<CommonResponse> Login(LoginForm loginForm)
        {
            var nowUser =  _sqliteSql.Select<UserInfo>();
            var result = await nowUser.AnyAsync(u=>u.UserName == loginForm.username && u.Password == loginForm.password);

            return new CommonResponse
            {
                code = result ? 0 :-1 ,
                message = result ? "登录成功" :"登录失败，请检查用户名和密码",
                data = result?tokenHelper.CreateToken(new Dictionary<string, string>
                    {
                        { "username",loginForm.username}
                    }).Token:null
            };
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse> Info(string token)
        {
            var ss = HttpContext;
            string username = "";
            string Issuer = Configuration.GetSection("JWTConfig").GetSection("Issuer").Value;
            string Audience = Configuration.GetSection("JWTConfig").GetSection("Audience").Value;
            //验证jwt,同时取出来jwt里边的用户ID
            TokenType tokenType = tokenHelper.ValiTokenState(token, a => a["iss"] == Issuer && a["aud"] == Audience, action => { username = action["username"]; });
            if (tokenType == TokenType.Fail)
            {
                return new CommonResponse
                {
                    code = 50012,
                    message = "token验证失败",
                    data =  null
                };
            }
            if (tokenType == TokenType.Expired)
            {
                return new CommonResponse
                {
                    code = 50014,
                    message = "token已经过期",
                    data = null
                };
            }
            var tokenUser = await _sqliteSql.Select<UserInfo>().Where(u => u.UserName == username).ToOneAsync();
            return new CommonResponse
            {
                code = 0,
                data = new User
                {
                    Avatar = tokenUser.Avatar,
                    Introduction = tokenUser.Introduction,
                    UserName = username,
                    Roles = new string[] { "admin" }
                },
                message = "获取用户信息成功"
            };
        }
    }
}
