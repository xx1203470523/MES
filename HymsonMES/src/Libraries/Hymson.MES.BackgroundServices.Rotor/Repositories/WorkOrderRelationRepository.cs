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
    /// 铁芯码和轴码绑定仓储
    /// </summary>
    public class WorkOrderRelationRepository : BaseRepository, IWorkOrderRelationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WorkOrderRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 获取绑定关系数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkOrderRelationDto>> GetList(string sql)
        {
            using var conn = GetRotorDbConnection();
            var dbList = await conn.QueryAsync<WorkOrderRelationDto>(sql);

            return dbList.ToList();
        }
    }
}
