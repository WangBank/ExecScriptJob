using System.Threading.Tasks;
using ExecScriptJob.Model.ApiDtos;
using ExecScriptJob.Model.FreeSqlHelper;
using ExecScriptJob.Model.Response;
using Microsoft.AspNetCore.Mvc;


namespace ExecScriptJob.Controllers
{
    /// <summary>
    /// 任务明细
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TasksDetailController : ControllerBase
    {

        /// <summary>
        /// 主db操作
        /// </summary>
        public IFreeSql _sqliteSql;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqliteSql"></param>
        public TasksDetailController(IFreeSql<SqlLiteFlag> sqliteSql)
        {
           
            _sqliteSql = sqliteSql;
        }

        /// <summary>
        /// 任务明细信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse> Info(string code = "")
        {
            var items = string.IsNullOrEmpty(code)
              ? await _sqliteSql.Select<TasksDetail>().ToListAsync()
              : await _sqliteSql.Select<TasksDetail>().Where(o => o.ScriptCode== int.Parse(code) || o.OrgCode == int.Parse(code)|| o.TaskCode == int.Parse(code)
          ).ToListAsync();
            return new CommonResponse
            {
                code = 0,
                data = new TasksDetailList
                {
                    Items = items,
                    Total = items.Count
                },
                message = ""
            };
        }


        /// <summary>
        /// 新增任务明细信息
        /// </summary>
        /// <param name="tasksDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse> Insert(TasksDetail tasksDetail)
        {
            var insetresult = await _sqliteSql.Insert(new TasksDetail
            {
                ScriptCode = tasksDetail.ScriptCode,
                OrgCode = tasksDetail.OrgCode,
                TaskCode = tasksDetail.TaskCode
            }).ExecuteAffrowsAsync();


            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "插入失败"
            };
        }


        /// <summary>
        /// 修改任务明细信息
        /// </summary>
        /// <param name="tasksDetail"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<CommonResponse> Update(TasksDetail tasksDetail)
        {
            var insetresult = await _sqliteSql.Update<TasksDetail>().SetSource(tasksDetail).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "更新失败!"
            };
        }


        /// <summary>
        /// 删除任务明细信息
        /// </summary>
        /// <param name="taskCode"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<CommonResponse> Delete(int taskCode)
        {
            var insetresult = await _sqliteSql.Delete<TasksDetail>().Where(o => o.TaskCode == taskCode).ExecuteAffrowsAsync();

            return new CommonResponse
            {
                code = insetresult == 1 ? 0 : -1,
                message = insetresult == 1 ? "" : "删除失败!"
            };
        }
    }
}
