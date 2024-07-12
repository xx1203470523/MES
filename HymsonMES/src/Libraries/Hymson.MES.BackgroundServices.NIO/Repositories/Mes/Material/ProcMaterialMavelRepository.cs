using Dapper;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material.View;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护仓储
    /// </summary>
    public class ProcMaterialMavelRepository : BaseRepository, IProcMaterialMavelRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcMaterialMavelRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            
        }

        /// <summary>
        /// 获取自制品列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProcMaterialEntity>> GetSelfControlListAsync(MavelMaterialQuery param)
        {
            string sql = $@"
                select * 
                from proc_material pm 
                where IsDeleted  = 0
                and SiteId  = {param.SiteId}
                and BuyType = 1
            ";
            using var conn = GetMESDbConnection();
            var dbList = await conn.QueryAsync<ProcMaterialEntity>(sql);

            return dbList;
        }
    }

}
