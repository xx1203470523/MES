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
        /// 删除（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities)
        {
            using var conn = GetMESDbConnection();

            // AND Status = @Status 不加状态是为了删除对应条码所有记录
            return await conn.ExecuteAsync(DeletesSql, entities);
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
#if DM
        const string InsertsSql = "INSERT  manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (@Id, @SiteId, @WorkOrderId, @SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
#else
        const string InsertsSql = "INSERT IGNORE manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (@Id, @SiteId, @WorkOrderId, @SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
#endif
        const string ReplacesSql = "REPLACE INTO manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (@Id, @SiteId, @WorkOrderId, @SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
        const string DeletesSql = "DELETE FROM manu_workorder_sfc WHERE SiteId = @SiteId AND WorkOrderId = @WorkOrderId AND SFC = @SFC";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_workorder_sfc /**where**/  ";

    }
}
