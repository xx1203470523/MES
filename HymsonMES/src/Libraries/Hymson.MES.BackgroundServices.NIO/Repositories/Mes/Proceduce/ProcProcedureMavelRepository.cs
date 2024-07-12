using Dapper;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce.View;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Microsoft.Extensions.Options;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce
{
    /// <summary>
    /// 工序表仓储
    /// </summary>
    public class ProcProcedureMavelRepository : BaseRepository, IProcProcedureMavelRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcProcedureMavelRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 获取站点下的所有工序
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetList(MavelProducreQuery param)
        {
            string sql = $@"
                select *
                from proc_procedure pp 
                where IsDeleted  = 0
                and SiteId  = {param.SiteId}
            ";

            using var conn = GetMESDbConnection();
            var dbList = await conn.QueryAsync<ProcProcedureEntity>(sql);

            return dbList;
        }
    }
}
