/*
 *creator: Karl
 *
 *describe: 生产领料单明细 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-05 03:48:53
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单明细仓储
    /// </summary>
    public partial class ManuRequistionOrderDetailRepository :BaseRepository, IManuRequistionOrderDetailRepository
    {

        public ManuRequistionOrderDetailRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuRequistionOrderDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuRequistionOrderDetailEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRequistionOrderDetailEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuRequistionOrderDetailEntity>> GetPagedInfoAsync(ManuRequistionOrderDetailPagedQuery manuRequistionOrderDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (manuRequistionOrderDetailPagedQuery.PageIndex - 1) * manuRequistionOrderDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuRequistionOrderDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuRequistionOrderDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var manuRequistionOrderDetailEntitiesTask = conn.QueryAsync<ManuRequistionOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuRequistionOrderDetailEntities = await manuRequistionOrderDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuRequistionOrderDetailEntity>(manuRequistionOrderDetailEntities, manuRequistionOrderDetailPagedQuery.PageIndex, manuRequistionOrderDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuRequistionOrderDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetManuRequistionOrderDetailEntitiesAsync(ManuRequistionOrderDetailQuery manuRequistionOrderDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuRequistionOrderDetailEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SitId=@SiteId");
            sqlBuilder.Select("*");
            if (manuRequistionOrderDetailQuery.RequistionOrderIds!=null&&manuRequistionOrderDetailQuery.RequistionOrderIds.Any())
            {
                sqlBuilder.Where("RequistionOrderId IN @RequistionOrderIds");
            }
            sqlBuilder.AddParameters(manuRequistionOrderDetailQuery);
            using var conn = GetMESDbConnection();
            var manuRequistionOrderDetailEntities = await conn.QueryAsync<ManuRequistionOrderDetailEntity>(template.RawSql, manuRequistionOrderDetailQuery);
            return manuRequistionOrderDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuRequistionOrderDetailEntity manuRequistionOrderDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuRequistionOrderDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuRequistionOrderDetailEntity> manuRequistionOrderDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuRequistionOrderDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuRequistionOrderDetailEntity manuRequistionOrderDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuRequistionOrderDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuRequistionOrderDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuRequistionOrderDetailEntity> manuRequistionOrderDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuRequistionOrderDetailEntitys);
        }
        #endregion

    }

    public partial class ManuRequistionOrderDetailRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_requistion_order_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_requistion_order_detail` /**where**/ ";
        const string GetManuRequistionOrderDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_requistion_order_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_requistion_order_detail`(  `Id`, `RequistionOrderId`, `MaterialCode`, `Version`, `MaterialBarCode`, `Batch`, `Qty`, `WarehouseId`, `SupplierCode`, `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @RequistionOrderId, @MaterialCode, @Version, @MaterialBarCode, @Batch, @Qty, @WarehouseId, @SupplierCode, @ExpirationDate, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_requistion_order_detail`(  `Id`, `RequistionOrderId`, `MaterialCode`, `Version`, `MaterialBarCode`, `Batch`, `Qty`, `WarehouseId`, `SupplierCode`, `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @RequistionOrderId, @MaterialCode, @Version, @MaterialBarCode, @Batch, @Qty, @WarehouseId, @SupplierCode, @ExpirationDate, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `manu_requistion_order_detail` SET   RequistionOrderId = @RequistionOrderId, MaterialCode = @MaterialCode, Version = @Version, MaterialBarCode = @MaterialBarCode, Batch = @Batch, Qty = @Qty, WarehouseId = @WarehouseId, SupplierCode = @SupplierCode, ExpirationDate = @ExpirationDate, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_requistion_order_detail` SET   RequistionOrderId = @RequistionOrderId, MaterialCode = @MaterialCode, Version = @Version, MaterialBarCode = @MaterialBarCode, Batch = @Batch, Qty = @Qty, WarehouseId = @WarehouseId, SupplierCode = @SupplierCode, ExpirationDate = @ExpirationDate, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_requistion_order_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_requistion_order_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `RequistionOrderId`, `MaterialCode`, `Version`, `MaterialBarCode`, `Batch`, `Qty`, `WarehouseId`, `SupplierCode`, `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_requistion_order_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `RequistionOrderId`, `MaterialCode`, `Version`, `MaterialBarCode`, `Batch`, `Qty`, `WarehouseId`, `SupplierCode`, `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_requistion_order_detail`  WHERE Id IN @Ids ";
        #endregion
    }
}
