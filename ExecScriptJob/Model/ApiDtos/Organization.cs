using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace ExecScriptJob.Model.ApiDtos
{
    /// <summary>
    /// 账套信息列表
    /// </summary>
    public class OrganizationList
    {
        /// <summary>
        /// 账套信息列表
        /// </summary>
        public List<Organization> Items { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Total { get; set; }

    }

    /// <summary>
    /// 账套信息dto
    /// </summary>
    [Table(Name = "Organization")]
    public class Organization
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库类型
        /// 0 oracle,1 sqlserver,2 mysql
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库地址
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public string ConnectingString { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

}
