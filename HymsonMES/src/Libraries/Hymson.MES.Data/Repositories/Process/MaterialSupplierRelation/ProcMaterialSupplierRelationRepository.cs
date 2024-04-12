/*
 *creator: Karl
 *
 *describe: 物料供应商关系 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-27 02:30:48
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料供应商关系仓储
    /// </summary>
    public partial class ProcMaterialSupplierRelationRepository : BaseRepository, IProcMaterialSupplierRelationRepository
    {
        public ProcMaterialSupplierRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

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
        /// <param name="param"></param>
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
        public async Task<ProcMaterialSupplierRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialSupplierRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialSupplierRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialSupplierRelationEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialSupplierRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialSupplierRelationEntity>> GetPagedInfoAsync(ProcMaterialSupplierRelationPagedQuery procMaterialSupplierRelationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            var offSet = (procMaterialSupplierRelationPagedQuery.PageIndex - 1) * procMaterialSupplierRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialSupplierRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialSupplierRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var procMaterialSupplierRelationEntitiesTask = conn.QueryAsync<ProcMaterialSupplierRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialSupplierRelationEntities = await procMaterialSupplierRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialSupplierRelationEntity>(procMaterialSupplierRelationEntities, procMaterialSupplierRelationPagedQuery.PageIndex, procMaterialSupplierRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procMaterialSupplierRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialSupplierRelationEntity>> GetProcMaterialSupplierRelationEntitiesAsync(ProcMaterialSupplierRelationQuery procMaterialSupplierRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcMaterialSupplierRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procMaterialSupplierRelationEntities = await conn.QueryAsync<ProcMaterialSupplierRelationEntity>(template.RawSql, procMaterialSupplierRelationQuery);
            return procMaterialSupplierRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcMaterialSupplierRelationEntity procMaterialSupplierRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procMaterialSupplierRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcMaterialSupplierRelationEntity> procMaterialSupplierRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procMaterialSupplierRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcMaterialSupplierRelationEntity procMaterialSupplierRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procMaterialSupplierRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcMaterialSupplierRelationEntity> procMaterialSupplierRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procMaterialSupplierRelationEntitys);
        }

        /// <summary>
        /// 批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueByMaterialIdsAsync(long[] materialIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueByMaterialIdsSql, new { materialIds = materialIds });
            
        }

        /// <summary>
        /// 通过物料Id查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialSupplierView>> GetByMaterialIdAsync(long materialId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialSupplierView>(GetByMaterialIdSql, new { materialId = materialId });
        }

        /// <summary>
        /// 通过物料Id查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<ProcMaterialSupplierView>> GetByMaterialIdsAsync(long[] materialIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialSupplierView>(GetByMaterialIdSql, new { materialId = materialIds });
        }

        /// <summary>
        /// 通过供应商Id查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<ProcMaterialSupplierView>> GetBySupplierIdsAsync(long[] supplierIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialSupplierView>(GetBySupplierIdsSql, new { supplierIds });
        }
    }

    public partial class ProcMaterialSupplierRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_material_supplier_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_material_supplier_relation` /**where**/ ";
        const string GetProcMaterialSupplierRelationEntitiesSqlTemplate = @"SELECT
        /**select**/
        FROM `proc_material_supplier_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_material_supplier_relation`(  `Id`, `MaterialId`, `SupplierId`, `CreatedBy`, `CreatedOn`) VALUES (   @Id, @MaterialId, @SupplierId, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `proc_material_supplier_relation`(  `Id`, `MaterialId`, `SupplierId`, `CreatedBy`, `CreatedOn`) VALUES (   @Id, @MaterialId, @SupplierId, @CreatedBy, @CreatedOn )  ";
        const string UpdateSql = "UPDATE `proc_material_supplier_relation` SET   MaterialId = @MaterialId, SupplierId = @SupplierId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_material_supplier_relation` SET   MaterialId = @MaterialId, SupplierId = @SupplierId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material_supplier_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_material_supplier_relation`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT
          `Id`, `MaterialId`, `SupplierId`, `CreatedBy`, `CreatedOn`
        FROM `proc_material_supplier_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT
          `Id`, `MaterialId`, `SupplierId`, `CreatedBy`, `CreatedOn`
        FROM `proc_material_supplier_relation`  WHERE Id IN @ids ";

        const string DeleteTrueByMaterialIdsSql = "DELETE From `proc_material_supplier_relation` WHERE  MaterialId IN @materialIds";
        const string GetByMaterialIdSql = @"Select 
                                    msr.`Id`, msr.`MaterialId`, msr.`SupplierId`, msr.`CreatedBy`, msr.`CreatedOn`,
                                    s.code, s.name
                                from proc_material_supplier_relation msr
                                LEFT join wh_supplier s on msr.SupplierId=s.Id
                                where msr.MaterialId=@materialId and s.IsDeleted = 0
            ";


        const string GetBySupplierIdsSql = @"Select *  from proc_material_supplier_relation  where SupplierId in @supplierIds";
    }
}
