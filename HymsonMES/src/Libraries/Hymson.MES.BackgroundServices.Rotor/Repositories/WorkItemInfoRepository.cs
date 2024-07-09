using Dapper;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Entity;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using IdGen;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 上料信息仓储
    /// </summary>
    public class WorkItemInfoRepository : BaseRepository, IWorkItemInfoRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WorkItemInfoRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkItemInfoDto>> GetList(string sql)
        {
            using var conn = GetRotorDbConnection();
            var dbList = await conn.QueryAsync<WorkItemInfoDto>(sql);

            return dbList.ToList();
        }
    }
}
