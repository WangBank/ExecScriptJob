using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecScriptJob.Model.ApiDtos
{
    /// <summary>
    /// response user
    /// </summary>
    public class User
    {
        /// <summary>
        /// 权限组
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
    }
}
