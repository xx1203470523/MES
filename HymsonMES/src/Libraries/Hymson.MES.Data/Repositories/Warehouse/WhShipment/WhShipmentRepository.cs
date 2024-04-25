using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhShipment;
using Hymson.MES.Core.Domain.WhShipmentBarcode;
using Hymson.MES.Core.Domain.WhShipmentMaterial;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhShipment.View;
using Hymson.MES.Data.Repositories.WhShipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WhShipment
{
    /// <summary>
    /// 仓储（出货单）
    /// </summary>
    public partial class WhShipmentRepository : BaseRepository, IWhShipmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhShipmentRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhShipmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhShipmentEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// INSERT DETAIL
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhShipmentMaterialEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsDetailSql, entities);
        }

        /// <summary>
        /// INSERT BARCORDS
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhShipmentBarcodeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsBarcordSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhShipmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhShipmentEntity> entities)
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
        /// 删除Details
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesDetailByIdAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesDetailByIdSql, new { Ids = ids });
        }

        /// <summary>
        /// 删除Bacords
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesBarcodeByDetailIdAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesDetailByDetailIdSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhShipmentEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhShipmentEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhShipmentEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhShipmentEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhShipmentView> GetEntityWithCodeByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhShipmentView>(GetEntityWithCodeByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WhShipmentView> GetEntityAsync(WhShipmentQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
            sqlBuilder.LeftJoin("inte_custom cus ON T.CustomerId = cus.Id");
            sqlBuilder.Select("T.*,cus.Code AS CustomerCode");
            sqlBuilder.Where("T.IsDeleted = 0");
            if (query.ShipmentId.HasValue)
            {
                sqlBuilder.Where("T.Id = @ShipmentId");
            }
            if (!string.IsNullOrWhiteSpace(query.ShipmentNum))
            {
                query.ShipmentNum = $"%{query.ShipmentNum}%";
                sqlBuilder.Where("T.ShipmentNum LIKE @ShipmentNum");
            }

            if (!string.IsNullOrWhiteSpace(query.ShipmentNumNoLike))
            {
                sqlBuilder.Where("T.ShipmentNum = @ShipmentNumNoLike");
            }

            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhShipmentView>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhShipmentEntity>> GetEntitiesAsync(WhShipmentQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("T.*");
            sqlBuilder.Where("T.IsDeleted = 0");
            if (query.ShipmentId.HasValue)
            {
                sqlBuilder.Where("T.Id = @ShipmentId");
            }
            if (!string.IsNullOrWhiteSpace(query.ShipmentNum))
            {
                query.ShipmentNum = $"%{query.ShipmentNum}%";
                sqlBuilder.Where("T.ShipmentNum LIKE @ShipmentNum");
            }

            if (query.Ids != null && query.Ids.Any()) {
                sqlBuilder.Where("T.Id IN @Ids");
            }

            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhShipmentEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhShipmentEntity>> GetPagedListAsync(WhShipmentPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.ShipmentNum != null)
            {
                sqlBuilder.Where("ShipmentNum = @ShipmentNum");
            }

            if (pagedQuery.TimeStamp.Any())
            { 
                sqlBuilder.AddParameters(new { CreatedOnStart = pagedQuery.TimeStamp[0], CreatedOnEnd = pagedQuery.TimeStamp[1].AddDays(1) });
                sqlBuilder.Where("CreatedOn >= @CreatedOnStart and CreatedOn < @CreatedOnEnd");
            }

            if (pagedQuery.PlanShipmentTimeStart.HasValue)
            {
                sqlBuilder.Where("PlanShipmentTime >= @PlanShipmentTimeStart");
            }

            if (pagedQuery.PlanShipmentTimeEnd.HasValue)
            {
                sqlBuilder.Where("PlanShipmentTime <= @PlanShipmentTimeEnd");
            }

            if (pagedQuery.NotInIds != null && pagedQuery.NotInIds.Any()) {
                sqlBuilder.Where("Id NOT IN @NotInIds");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhShipmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhShipmentEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhShipmentRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_shipment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM wh_shipment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM wh_shipment T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM wh_shipment T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT 1";

        const string InsertSql = "INSERT INTO wh_shipment(  `Id`, `SiteId`, `ShipmentNum`, `CustomerId`, `PlanShipmentTime`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ShipmentNum, @CustomerId, @PlanShipmentTime, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO wh_shipment(  `Id`, `SiteId`, `ShipmentNum`, `CustomerId`, `PlanShipmentTime`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ShipmentNum, @CustomerId, @PlanShipmentTime, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string InsertsDetailSql = "INSERT INTO wh_shipment_material(`Id`, `ShipmentId`, `MaterialId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES(@Id, @ShipmentId, @MaterialId, @Qty, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted)";
        const string InsertsBarcordSql = "INSERT INTO wh_shipment_barcode(`Id`, `ShipmentDetailId`, `BarCode`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES(@Id, @ShipmentDetailId, @BarCode, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, 0)";

        const string UpdateSql = "UPDATE wh_shipment SET   SiteId = @SiteId, ShipmentNum = @ShipmentNum, CustomerId = @CustomerId, PlanShipmentTime = @PlanShipmentTime, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_shipment SET   SiteId = @SiteId, ShipmentNum = @ShipmentNum, CustomerId = @CustomerId, PlanShipmentTime = @PlanShipmentTime, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_shipment SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_shipment SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string DeletesDetailByIdSql = "DELETE FROM wh_shipment_material WHERE ShipmentId IN @Ids";

        const string DeletesDetailByDetailIdSql = "DELETE FROM wh_shipment_barcode WHERE ShipmentDetailId IN @Ids";

        const string GetByIdSql = @"SELECT * FROM wh_shipment WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM wh_shipment WHERE Id IN @Ids ";
        const string GetEntityWithCodeByIdSql = @"SELECT ws.*,cus.Code AS CustomerCode FROM wh_shipment ws LEFT JOIN inte_custom cus ON ws.CustomerId = cus.Id WHERE ws.Id = @Id ";

    }
}
