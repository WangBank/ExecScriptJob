using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecScriptJob.Model.Request
{
    /// <summary>
    /// 登录入参
    /// </summary>
    public class LoginForm
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
