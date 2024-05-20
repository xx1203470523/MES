using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;
using System.Text;

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
        public async Task<int> IgnoreRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(IgnoreSql, entities);
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
            if (entities == null || !entities.Any()) return 0;

            // 拼接删除SQL
            var stringBuilder = new StringBuilder();
            foreach (var item in entities)
            {
                // AND Status = @Status 不加状态是为了删除对应条码所有记录
                stringBuilder.AppendFormat(DeleteSql, item.SiteId, item.WorkOrderId, item.SFC.Replace("'", "''"));
            }

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(stringBuilder.ToString());

            /*
            using var conn = GetMESDbConnection();
            // AND Status = @Status 不加状态是为了删除对应条码所有记录
            return await conn.ExecuteAsync(DeletesSql, entities);
            */
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
        const string IgnoreSql = "INSERT IGNORE manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (@Id, @SiteId, @WorkOrderId, @SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";

#if DM
        const string InsertsSql = "MERGE INTO manu_workorder_sfc t " +
            "USING (SELECT @WorkOrderId AS WorkOrderId, @SFC AS SFC FROM dual) s " +
            "ON (t.WorkOrderId = s.WorkOrderId AND t.SFC = s.SFC) " +
            "WHEN NOT MATCHED THEN " +
              "INSERT (Id, SiteId, WorkOrderId, SFC, Status, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn) " +
              "VALUES (@Id, @SiteId, s.WorkOrderId, s.SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn); ";

        const string ReplacesSql = "MERGE INTO manu_workorder_sfc t " +
            "USING (SELECT @WorkOrderId AS WorkOrderId, @SFC AS SFC FROM dual) s " +
            "ON (t.WorkOrderId = s.WorkOrderId AND t.SFC = s.SFC) " +
            "WHEN MATCHED THEN " +
              "UPDATE SET " +
                "t.Id = @Id, " +
                "t.SiteId = @SiteId, " +
                "t.Status = @Status, " +
                "t.UpdatedBy = @UpdatedBy, " +
                "t.UpdatedOn = @UpdatedOn " +
            "WHEN NOT MATCHED THEN " +
              "INSERT (Id, SiteId, WorkOrderId, SFC, Status, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn) " +
              "VALUES (@Id, @SiteId, s.WorkOrderId, s.SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn); ";
#else

        const string ReplacesSql = "REPLACE INTO manu_workorder_sfc(  `Id`, `SiteId`, `WorkOrderId`, `SFC`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (@Id, @SiteId, @WorkOrderId, @SFC, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn) ";
#endif

        const string DeleteSql = "DELETE FROM manu_workorder_sfc WHERE SiteId = {0} AND WorkOrderId = {1} AND SFC = '{2}'; ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_workorder_sfc /**where**/  ";

    }
}
