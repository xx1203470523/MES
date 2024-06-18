using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Equipment.View;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具绑定设备操作记录表）
    /// </summary>
    public partial class EquSparepartEquipmentBindRecordRepository : BaseRepository, IEquSparepartEquipmentBindRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparepartEquipmentBindRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparepartEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSparepartEquipmentBindRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparepartEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSparepartEquipmentBindRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
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
        public async Task<EquSparepartEquipmentBindRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparepartEquipmentBindRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartEquipmentBindRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparepartEquipmentBindRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartEquipmentBindRecordEntity>> GetEntitiesAsync(EquSparepartEquipmentBindRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.EquipmentId.HasValue)
            {
                sqlBuilder.Where(" EquipmentId=@EquipmentId ");
            }
            if (query.SparepartId.HasValue)
            {
                sqlBuilder.Where(" SparepartId=@SparepartId ");
            }
            if (query.OperationType.HasValue)
            {
                sqlBuilder.Where(" OperationType=@OperationType ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparepartEquipmentBindRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartEquipmentBindRecordView>> GetPagedListAsync(EquSparepartEquipmentBindRecordPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("es.code as SparepartCode,es.`Name`as SparepartName,est.`Code` as SparePartType,est.`Name` as SparePartTypeName,es.SparePartTypeId,ee.EquipmentCode,ee.EquipmentName,ese.Id, ese.CreatedBy,ese.CreatedOn,ese.Position,ese.OperationType,ese.UninstallReason,ese.UninstallBy,ese.UninstallOn");

            sqlBuilder.LeftJoin(" equ_equipment ee on ee.Id=ese.EquipmentId");
            sqlBuilder.LeftJoin(" equ_sparepart es on es.Id=ese.SparepartId");
            sqlBuilder.LeftJoin(" equ_sparepart_type est on est.Id=es.SparePartTypeId");

            sqlBuilder.OrderBy("ese.UpdatedOn DESC");
            sqlBuilder.Where("ese.IsDeleted = 0");
            sqlBuilder.Where("ese.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.SparepartCode))
            {
                pagedQuery.SparepartCode = $"%{pagedQuery.SparepartCode}%";
                sqlBuilder.Where(" es.code like @SparepartCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.SparepartName))
            {
                pagedQuery.SparepartName = $"%{pagedQuery.SparepartName}%";
                sqlBuilder.Where(" es.Name like @SparepartName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.SparePartType))
            {
                pagedQuery.SparePartType = $"%{pagedQuery.SparePartType}%";
                sqlBuilder.Where(" est.Code like @SparePartType");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Position))
            {
                pagedQuery.Position = $"%{pagedQuery.Position}%";
                sqlBuilder.Where(" ese.Position like @Position");
            }

            if (pagedQuery.OperationType.HasValue)
            {
                sqlBuilder.Where(" ese.OperationType= @OperationType");
            }

            if (pagedQuery.InstallTimeRange != null && pagedQuery.InstallTimeRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pagedQuery.InstallTimeRange[0], CreatedOnEnd = pagedQuery.InstallTimeRange[1].AddDays(1) });
                sqlBuilder.Where(" ese.CreatedOn >= @CreatedOnStart AND ese.CreatedOn < @CreatedOnEnd");
            }

            if (pagedQuery.UninstallTimeRange != null && pagedQuery.UninstallTimeRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { UninstallTimeStart = pagedQuery.UninstallTimeRange[0], UninstallTimeEnd = pagedQuery.UninstallTimeRange[1] });
                sqlBuilder.Where(" ese.UninstallOn >= @UninstallTimeStart AND ese.UninstallOn < @UninstallTimeEnd");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSparepartEquipmentBindRecordView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparepartEquipmentBindRecordView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquSparepartEquipmentBindRecordEntity> GetIsBindAsync(EquSparepartEquipmentBindRecordQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparepartEquipmentBindRecordEntity>(IsBindSql, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSparepartEquipmentBindRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_sparepart_equipment_bind_record ese /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_sparepart_equipment_bind_record ese /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_sparepart_equipment_bind_record /**where**/  ";

        const string InsertSql = "INSERT INTO equ_sparepart_equipment_bind_record(  `Id`, `SiteId`, `SparepartId`, `SparepartRecordId`, `EquipmentId`, `EquipmentRecordId`, `Position`, `OperationType`, `UninstallReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SparepartId, @SparepartRecordId, @EquipmentId, @EquipmentRecordId, @Position, @OperationType, @UninstallReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO equ_sparepart_equipment_bind_record(  `Id`, `SiteId`, `SparepartId`, `SparepartRecordId`, `EquipmentId`, `EquipmentRecordId`, `Position`, `OperationType`, `UninstallReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SparepartId, @SparepartRecordId, @EquipmentId, @EquipmentRecordId, @Position, @OperationType, @UninstallReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE equ_sparepart_equipment_bind_record SET  SparepartId = @SparepartId, SparepartRecordId = @SparepartRecordId, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, Position = @Position, OperationType = @OperationType, UninstallReason = @UninstallReason, Remark=@Remark,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,UninstallBy=@UninstallBy,UninstallOn=@UninstallOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_sparepart_equipment_bind_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_sparepart_equipment_bind_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_sparepart_equipment_bind_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_sparepart_equipment_bind_record WHERE Id IN @Ids ";

        const string IsBindSql = "SELECT Id,Position FROM equ_sparepart_equipment_bind_record WHERE IsDeleted= 0 AND SiteId=@SiteId AND EquipmentId=@EquipmentId and SparepartId=@SparepartId and OperationType=@OperationType";
    }
}
