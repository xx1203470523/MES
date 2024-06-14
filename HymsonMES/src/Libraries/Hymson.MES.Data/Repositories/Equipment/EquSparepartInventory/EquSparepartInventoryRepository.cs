/*
 *creator: Karl
 *
 *describe: 备件库存 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSparepartInventory;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSparepartInventory
{
    /// <summary>
    /// 备件库存仓储
    /// </summary>
    public partial class EquSparepartInventoryRepository : BaseRepository, IEquSparepartInventoryRepository
    {

        public EquSparepartInventoryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparepartInventoryEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparepartInventoryEntity>(GetByIdSql, new { Id = id });
        }


        /// <summary>
        /// 根据SparepartId获取数据
        /// </summary>
        /// <param name="SparepartId"></param>
        /// <returns></returns>
        public async Task<EquSparepartInventoryEntity> GetBySparepartIdAsync(EquSparepartInventoryQuery equSparepartInventoryQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparepartInventoryEntity>(GetBySparepartIdSql, equSparepartInventoryQuery);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartInventoryEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparepartInventoryEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparepartInventoryPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartInventoryPageView>> GetPagedInfoAsync(EquSparepartInventoryPagedQuery equSparepartInventoryPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.LeftJoin("equ_sparepart es ON es.Id=esi.SparepartId");
            sqlBuilder.LeftJoin("equ_sparepart_type est ON est.Id=es.SparePartTypeId");

            sqlBuilder.Where("esi.IsDeleted=0");
            sqlBuilder.Where("esi.SiteId=@SiteId");
            sqlBuilder.Select("esi.Id,esi.SparepartId,es.Code AS SparepartCode,es.Name AS SparepartName,est.Code AS SparepartGroupCode,est.Name AS SparepartGroupName,esi.Qty,es.Specifications,esi.UpdatedBy,esi.UpdatedOn");

            if (!string.IsNullOrWhiteSpace(equSparepartInventoryPagedQuery.SparepartCode))
            {
                equSparepartInventoryPagedQuery.SparepartCode = $"%{equSparepartInventoryPagedQuery.SparepartCode}%";
                sqlBuilder.Where("es.Code LIKE @SparepartCode");
            }
            if (!string.IsNullOrWhiteSpace(equSparepartInventoryPagedQuery.SparepartName))
            {
                equSparepartInventoryPagedQuery.SparepartName = $"%{equSparepartInventoryPagedQuery.SparepartName}%";
                sqlBuilder.Where("es.Name LIKE @SparepartName");
            }
            if (!string.IsNullOrWhiteSpace(equSparepartInventoryPagedQuery.SparePartsGroupCode))
            {
                equSparepartInventoryPagedQuery.SparePartsGroupCode = $"%{equSparepartInventoryPagedQuery.SparePartsGroupCode}%";
                sqlBuilder.Where("est.Code LIKE @SparePartsGroupCode");
            }
            if (!string.IsNullOrWhiteSpace(equSparepartInventoryPagedQuery.Specifications))
            {
                equSparepartInventoryPagedQuery.Specifications = $"%{equSparepartInventoryPagedQuery.Specifications}%";
                sqlBuilder.Where("es.Specifications LIKE @Specifications");
            }

            var offSet = (equSparepartInventoryPagedQuery.PageIndex - 1) * equSparepartInventoryPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSparepartInventoryPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSparepartInventoryPagedQuery);

            using var conn = GetMESDbConnection();
            var equSparepartInventoryEntitiesTask = conn.QueryAsync<EquSparepartInventoryPageView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSparepartInventoryEntities = await equSparepartInventoryEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparepartInventoryPageView>(equSparepartInventoryEntities, equSparepartInventoryPagedQuery.PageIndex, equSparepartInventoryPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSparepartInventoryQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartInventoryEntity>> GetEquSparepartInventoryEntitiesAsync(EquSparepartInventoryQuery equSparepartInventoryQuery)
        {
            var sqlBuilder = new SqlBuilder();

            var template = sqlBuilder.AddTemplate(GetEquSparepartInventoryEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            if (equSparepartInventoryQuery.SparepartIds != null && equSparepartInventoryQuery.SparepartIds.Any())
            {
                sqlBuilder.Where("SparepartId IN @SparepartIds");
            }

            using var conn = GetMESDbConnection();
            var equSparepartInventoryEntities = await conn.QueryAsync<EquSparepartInventoryEntity>(template.RawSql, equSparepartInventoryQuery);
            return equSparepartInventoryEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparepartInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparepartInventoryEntity equSparepartInventoryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSparepartInventoryEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSparepartInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSparepartInventoryEntity> equSparepartInventoryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSparepartInventoryEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparepartInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparepartInventoryEntity equSparepartInventoryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSparepartInventoryEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSparepartInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSparepartInventoryEntity> equSparepartInventoryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSparepartInventoryEntitys);
        }
        #endregion

    }

    public partial class EquSparepartInventoryRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_sparepart_inventory` esi /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_sparepart_inventory` esi /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEquSparepartInventoryEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart_inventory` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_sparepart_inventory`(  `Id`, `SiteId`, `SparepartId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SparepartId, @Qty, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_sparepart_inventory`(  `Id`, `SiteId`, `SparepartId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SparepartId, @Qty, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_sparepart_inventory` SET   SiteId = @SiteId, SparepartId = @SparepartId, Qty = @Qty, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_sparepart_inventory` SET   SiteId = @SiteId, SparepartId = @SparepartId, Qty = @Qty, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_sparepart_inventory` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_sparepart_inventory` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SparepartId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_sparepart_inventory`  WHERE Id = @Id ";
        const string GetBySparepartIdSql = @"SELECT 
                               `Id`, `SiteId`, `SparepartId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_sparepart_inventory`  WHERE SparepartId = @SparepartId AND  SiteId = @SiteId ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SparepartId`, `Qty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_sparepart_inventory`  WHERE Id IN @Ids ";
        #endregion
    }
}
