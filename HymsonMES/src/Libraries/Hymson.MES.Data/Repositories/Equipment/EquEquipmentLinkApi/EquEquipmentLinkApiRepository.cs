﻿using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi
{
    /// <summary>
    /// 设备链接
    /// </summary>
    public partial class EquEquipmentLinkApiRepository : BaseRepository,IEquEquipmentLinkApiRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentLinkApiRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// equipmentUnitEntity
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentLinkApiEntity equipmentUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equipmentUnitEntity);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquEquipmentLinkApiEntity> entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentLinkApiEntity equipmentUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equipmentUnitEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquEquipmentLinkApiEntity> entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(IEnumerable<long> idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(SoftDeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByEquipmentIdSql, new { EquipmentId=equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] equipmentIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByEquipmentIdSql, new { EquipmentIds= equipmentIds });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkApiEntity>(GetByIdSql, new { id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiEntity> GetByEquipmentIdAsync(long equipmentId, string apiType)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkApiEntity>(GetByEquipmentIdSql, new { equipmentId, apiType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkApiEntity>> GetListAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentLinkApiEntity>(GetByEquipmentSql, new { equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkApiEntity>> GetEntitiesAsync(EquEquipmentLinkApiQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equipmentUnitEntities = await conn.QueryAsync<EquEquipmentLinkApiEntity>(template.RawSql, query);
            return equipmentUnitEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiEntity>> GetPagedListAsync(EquEquipmentLinkApiPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<EquEquipmentLinkApiEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentLinkApiEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentLinkApiRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_equipment_link_api`(  `Id`, `EquipmentId`, `ApiUrl`, `ApiType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @EquipmentId, @ApiUrl, @ApiType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_equipment_link_api` SET   EquipmentId = @EquipmentId, ApiUrl = @ApiUrl, ApiType = @ApiType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string SoftDeleteSql = "UPDATE `equ_equipment_link_api` SET `IsDeleted` = 1 WHERE `Id` = @Id;";
        const string DeleteByEquipmentIdSql = "DELETE FROM equ_equipment_link_api WHERE EquipmentId=@EquipmentId ;";
        const string DeletesByEquipmentIdSql = "DELETE FROM equ_equipment_link_api WHERE EquipmentId in @EquipmentIds ;";
        const string GetByIdSql = "SELECT * FROM `equ_equipment_link_api` WHERE `Id` = @Id;";
        const string GetByEquipmentIdSql = "SELECT * FROM `equ_equipment_link_api` WHERE `EquipmentId` = @EquipmentId;";
        const string GetByEquipmentSql = "SELECT * FROM `equ_equipment_link_api` WHERE `EquipmentId` = @EquipmentId;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_equipment_link_api` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_link_api` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
