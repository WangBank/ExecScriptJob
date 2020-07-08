using System.Threading.Tasks;
using ExecScriptJob.Model.ApiDtos;
using ExecScriptJob.Model.FreeSqlHelper;
using ExecScriptJob.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace ExecScriptJob.Controllers
{
    /// <summary>
    /// 账套api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        /// <summary>
        /// 主db操作
        /// </summary>
        public IFreeSql _sqliteSql;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqliteSql"></param>
        public OrganizationController( IFreeSql<SqlLiteFlag> sqliteSql)
        {
            _sqliteSql = sqliteSql;
        }

        /// <summary>
        /// 获取账套信息
        /// </summary>
        /// <param name="code">账套编码</param>
        /// <param name="name">账套名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse> Info(string code="",string name="")
        {
          
            var items = string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name)
                ? await _sqliteSql.Select<Organization>().ToListAsync() 
                : await _sqliteSql.Select<Organization>().Where(o => o.Name.Contains(name) || o.Code==int.Parse(code)
            ).ToListAsync();

            return new CommonResponse
            {
                code = 0,
                data = new OrganizationList
                {
                    Items = items,
                    Total = items.Count
                },
                message = ""
            };
        }

        /// <summary>
        /// 新增账套信息
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse> Insert(Organization organization)
        {
            var insetresult  = await _sqliteSql.Insert(new Organization { 
                ServerName = organization.ServerName,
                ConnectingString = organization.ConnectingString,
                DataBaseName = organization.DataBaseName,
                DbType = organization.DbType,
                Description = organization.Description,
                Name = organization.Name,
                Password = organization.Password,
                UserName = organization.UserName

            }).ExecuteAffrowsAsync();

           
            return new CommonResponse
            {
                code = insetresult==1?0: -1,
                message = insetresult == 1 ? "" : "插入失败"
            };
        }


        /// <summary>
        /// 修改账套信息
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResponse> Update(Organization organization)
        {
            var insetresult = await _sqliteSql.Update<Organization>().SetSource(organization).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "更新失败!"
            };
        }


        /// <summary>
        /// 删除账套信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResponse> Delete(int code)
        {
            var insetresult = await _sqliteSql.Delete<Organization>().Where(o=>o.Code == code).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "删除失败!"
            };
        }
    }
}
