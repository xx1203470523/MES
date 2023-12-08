using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（工单条码记录表）
    /// </summary>
    public partial class ManuWorkOrderSFCRepository : BaseRepository, IManuWorkOrderSFCRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuWorkOrderSFCRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> RepalceRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ReplacesSql, entities);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuWorkOrderSFCEntity>> GetEntitiesAsync(EntityByWorkOrderIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuWorkOrderSFCEntity>(template.RawSql, query);
        }


    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuWorkOrderSFCRepository
    {
        const string InsertsSql = "INSERT IGNORE manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `SFCStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (  @Id, @SiteId, @WorkOrderId, @SFC, @SFCStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
        const string ReplacesSql = "REPLACE INTO manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `SFCStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (  @Id, @SiteId, @WorkOrderId, @SFC, @SFCStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_workorder_sfc /**where**/  ";

    }
}
