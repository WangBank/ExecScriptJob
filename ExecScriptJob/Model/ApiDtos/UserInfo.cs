using FreeSql.DataAnnotations;

namespace ExecScriptJob.Model.ApiDtos
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Table(Name = "User")]
    public class UserInfo
    {

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
        [Column(IsPrimary = true)]
        public string UserName { get; set; }
    }
}
