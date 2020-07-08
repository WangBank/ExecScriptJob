using System.Threading.Tasks;
using ExecScriptJob.Model.ApiDtos;
using ExecScriptJob.Model.FreeSqlHelper;
using ExecScriptJob.Model.Response;
using Microsoft.AspNetCore.Mvc;


namespace ExecScriptJob.Controllers
{

    /// <summary>
    /// 脚本Api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
        /// <summary>
        /// 主db操作
        /// </summary>
        public IFreeSql _sqliteSql;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqliteSql"></param>
        public ScriptController( IFreeSql<SqlLiteFlag> sqliteSql)
        {
            _sqliteSql = sqliteSql;
        }

        /// <summary>
        /// 获取脚本信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse> Info(string code = "", string name = "")
        {
           var items = string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name)
                ? await _sqliteSql.Select<Script>().ToListAsync()
                : await _sqliteSql.Select<Script>().Where(o => o.Name.Contains(name) || o.Code == int.Parse(code)
            ).ToListAsync();

            return new CommonResponse
            {
                code = 0,
                data = new ScriptList
                {
                    Items = items,
                    Total = items.Count
                },
                message = ""
            };
        }


        /// <summary>
        /// 新增脚本信息
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse> Insert(Script script)
        {
            var insetresult = await _sqliteSql.Insert(new Script
            {
                SqlString = script.SqlString,
                Description = script.Description,
                Name = script.Name
            }).ExecuteAffrowsAsync();


            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "插入失败"
            };
        }

        /// <summary>
        /// 修改脚本信息
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResponse> Update(Script script)
        {
            var insetresult = await _sqliteSql.Update<Script>().SetSource(script).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "更新失败!"
            };
        }


        /// <summary>
        /// 删除脚本信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResponse> Delete(int code)
        {
            var insetresult = await _sqliteSql.Delete<Script>().Where(o => o.Code == code).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "删除失败!"
            };
        }
    }
}

