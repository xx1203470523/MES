using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushActualDelivery.View;
using Hymson.MES.Data.Repositories.NIO.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.NIO
{
    /// <summary>
    /// 仓储（物料发货信息表）
    /// </summary>
    public partial class NioPushActualDeliveryRepository : BaseRepository, INioPushActualDeliveryRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public NioPushActualDeliveryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(NioPushActualDeliveryEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<NioPushActualDeliveryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(NioPushActualDeliveryEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<NioPushActualDeliveryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushActualDeliveryEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<NioPushActualDeliveryEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushActualDeliveryEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushActualDeliveryEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据niopushID获取数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushActualDeliveryEntity>> GetByPushIdAsync(long nioPushId)
        {
            string sql = "SELECT * FROM nio_push_ActualDelivery WHERE NioPushId = @NioPushId and IsDeleted = 0";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushActualDeliveryEntity>(sql, new { NioPushId = nioPushId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushActualDeliveryEntity>> GetEntitiesAsync(NioPushActualDeliveryQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushActualDeliveryEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushActualDeliveryView>> GetPagedListAsync(NioPushActualDeliveryPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("t1.*,t2.Status");
            sqlBuilder.OrderBy("t1.UpdatedOn DESC");
            sqlBuilder.Where("t1.IsDeleted = 0");
            sqlBuilder.InnerJoin("nio_push t2 on t1.NioPushId = t2.Id and t2.IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");
            if (string.IsNullOrEmpty(pagedQuery.MaterialName) == false)
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where(" t1.MaterialName like @MaterialName ");
            }
            if (string.IsNullOrEmpty(pagedQuery.MaterialCode) == false)
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where(" t1.MaterialCode like @MaterialCode ");
            }
            if (pagedQuery.Status != null)
            {
                sqlBuilder.Where(" t2.Status = @Status ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<NioPushActualDeliveryView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<NioPushActualDeliveryView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class NioPushActualDeliveryRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM nio_push_ActualDelivery t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM nio_push_ActualDelivery t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM nio_push_ActualDelivery /**where**/  ";

        const string InsertSql = "INSERT INTO nio_push_ActualDelivery(  `Id`, `NioPushId`, `PartnerBusiness`, `MaterialCode`, `MaterialName`, `ShippedQty`, `ActualDeliveryTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @NioPushId, @PartnerBusiness, @MaterialCode, @MaterialName, @ShippedQty, @ActualDeliveryTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO nio_push_ActualDelivery(  `Id`, `NioPushId`, `PartnerBusiness`, `MaterialCode`, `MaterialName`, `ShippedQty`, `ActualDeliveryTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @NioPushId, @PartnerBusiness, @MaterialCode, @MaterialName, @ShippedQty, @ActualDeliveryTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE nio_push_ActualDelivery SET   MaterialCode = @MaterialCode, MaterialName = @MaterialName, ShippedQty = @ShippedQty, ActualDeliveryTime = @ActualDeliveryTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE nio_push_ActualDelivery SET  MaterialCode = @MaterialCode, MaterialName = @MaterialName, ShippedQty = @ShippedQty, ActualDeliveryTime = @ActualDeliveryTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE nio_push_ActualDelivery SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE nio_push_ActualDelivery SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM nio_push_ActualDelivery WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM nio_push_ActualDelivery WHERE Id IN @Ids ";

    }
}
