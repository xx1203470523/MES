using Dapper;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Entity;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 过站仓储
    /// </summary>
    public class WorkProcessRepository : BaseRepository, IWorkProcessRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WorkProcessRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 获取过站信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkProcessDto>> GetList(string sql)
        {
            using var conn = GetRotorDbConnection();
            var dbList = await conn.QueryAsync<WorkProcessDto>(sql);

            return dbList.ToList();
        }
    }
}
