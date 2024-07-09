using Dapper;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.BackgroundServices.Stator.Repositories
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class OPRepository<TEntity> : BaseRepository, IOPRepository<TEntity> where TEntity : BaseOPEntity
    {
        /// <summary>
        /// 水位查询SQL
        /// </summary>
        const string QuerySql = @"SELECT * FROM {0} WHERE Id > @StartWaterMarkId ORDER BY Id ASC LIMIT @Rows";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public OPRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<TEntity>(string.Format(QuerySql, typeof(TEntity).Name), query);
        }

    }
}
