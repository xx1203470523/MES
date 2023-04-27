/*
 *creator: Karl
 *
 *describe: 容器条码表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器条码表仓储
    /// </summary>
    public partial class ManuContainerBarcodeRepository :BaseRepository, IManuContainerBarcodeRepository
    {

        public ManuContainerBarcodeRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuContainerBarcodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerBarcodeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerBarcodeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuContainerBarcodeEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuContainerBarcodePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerBarcodeQueryView>> GetPagedInfoAsync(ManuContainerBarcodePagedQuery manuContainerBarcodePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("barcode.IsDeleted=0");
            sqlBuilder.Select("barcode.id,barcode.SiteId,barcode.ProductId,barcode.BarCode,barcode.ContainerId,barcode.Status,barcode.CreatedBy," +
                "barcode.CreatedOn,material.MaterialCode as ProductCode,material.MaterialName as ProductName,container.`Level`,container.Maximum,container.Minimum");
            sqlBuilder.LeftJoin("proc_material material on material.Id=barcode.ProductId and material.IsDeleted=0");
            sqlBuilder.LeftJoin("inte_container container on container.Id=barcode.ContainerId and container.IsDeleted=0");
            if (!string.IsNullOrWhiteSpace(manuContainerBarcodePagedQuery.BarCode))
            {
                sqlBuilder.Where("barcode.BarCode=@BarCode");
            }
            if (manuContainerBarcodePagedQuery.Level.HasValue)
            {
                sqlBuilder.Where("container.Level=@Level");
            }
            if (!string.IsNullOrWhiteSpace(manuContainerBarcodePagedQuery.ProductName))
            {
                sqlBuilder.Where("material.MaterialName=@ProductName");
            }

            if (!string.IsNullOrWhiteSpace(manuContainerBarcodePagedQuery.ProductCode))
            {
                sqlBuilder.Where("material.MaterialCode=@ProductCode");
            }

            var offSet = (manuContainerBarcodePagedQuery.PageIndex - 1) * manuContainerBarcodePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuContainerBarcodePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuContainerBarcodePagedQuery);

            using var conn = GetMESDbConnection();
            var manuContainerBarcodeEntitiesTask = conn.QueryAsync<ManuContainerBarcodeQueryView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuContainerBarcodeEntities = await manuContainerBarcodeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuContainerBarcodeQueryView>(manuContainerBarcodeEntities, manuContainerBarcodePagedQuery.PageIndex, manuContainerBarcodePagedQuery.PageSize, totalCount);
        }

        public async Task<PagedInfo<ManuContainerBarcodeEntity>> GetPagedListAsync(ManuContainerBarcodePagedQuery manuContainerBarcodePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("barcode.IsDeleted=0");
            sqlBuilder.Where(" barcode.SiteId=@SiteId ");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(manuContainerBarcodePagedQuery.BarCode))
            {
                sqlBuilder.Where("barcode.BarCode=@BarCode");
            }
            if (manuContainerBarcodePagedQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("barcode.WorkOrderId=@WorkOrderId");
            }

            var offSet = (manuContainerBarcodePagedQuery.PageIndex - 1) * manuContainerBarcodePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuContainerBarcodePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuContainerBarcodePagedQuery);

            using var conn = GetMESDbConnection();
            var manuContainerBarcodeEntitiesTask = conn.QueryAsync<ManuContainerBarcodeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuContainerBarcodeEntities = await manuContainerBarcodeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuContainerBarcodeEntity>(manuContainerBarcodeEntities, manuContainerBarcodePagedQuery.PageIndex, manuContainerBarcodePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuContainerBarcodeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerBarcodeEntity>> GetManuContainerBarcodeEntitiesAsync(ManuContainerBarcodeQuery manuContainerBarcodeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuContainerBarcodeEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuContainerBarcodeEntities = await conn.QueryAsync<ManuContainerBarcodeEntity>(template.RawSql, manuContainerBarcodeQuery);
            return manuContainerBarcodeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuContainerBarcodeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerBarcodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuContainerBarcodeEntity> manuContainerBarcodeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuContainerBarcodeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuContainerBarcodeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuContainerBarcodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuContainerBarcodeEntity> manuContainerBarcodeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuContainerBarcodeEntitys);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, manuContainerBarcodeEntity);
        }

        public async Task<ManuContainerBarcodeEntity> GetByCodeAsync(ManuContainerBarcodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerBarcodeEntity>(GetByCodeSql, new { BarCode = query.BarCode, SiteId= query.SiteId });
        }

        public async Task<ManuContainerBarcodeEntity> GetByProductIdAsync(long pid, int status,int level)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerBarcodeEntity>(GetByProductIdSql, new { ProductId = pid, Status =status,PackLevel=level });
        }
        #endregion

    }

    public partial class ManuContainerBarcodeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_container_barcode` barcode /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_container_barcode` barcode /**leftjoin**/ /**where**/ ";
        const string GetManuContainerBarcodeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_container_barcode` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_container_barcode`(  `Id`, `SiteId`, `ProductId`, `WorkOrderId`,`BarCode`, `ContainerId`,`PackLevel`,`MaterialVersion`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProductId,@WorkOrderId, @BarCode, @ContainerId,@PackLevel,@MaterialVersion, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_container_barcode`(  `Id`, `SiteId`, `ProductId`,`WorkOrderId`, `BarCode`, `ContainerId`,`PackLevel`,`MaterialVersion`,  `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProductId,@WorkOrderId, @BarCode, @ContainerId,@PackLevel,@MaterialVersion, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_container_barcode` SET   SiteId = @SiteId, ProductId = @ProductId, BarCode = @BarCode, ContainerId = @ContainerId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_container_barcode` SET   SiteId = @SiteId, ProductId = @ProductId, BarCode = @BarCode, ContainerId = @ContainerId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `manu_container_barcode` SET   Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";


        const string DeleteSql = "UPDATE `manu_container_barcode` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_container_barcode` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProductId`,`WorkOrderId`, `BarCode`, `ContainerId`,`PackLevel`,`MaterialVersion`,  `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_barcode`  WHERE Id = @Id ";
        const string GetByCodeSql = @"SELECT 
                               `Id`, `SiteId`, `ProductId`, `BarCode`,`WorkOrderId`, `ContainerId`,`PackLevel`,`MaterialVersion`,  `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_barcode`  WHERE BarCode = @BarCode and SiteId=@SiteId ";
        const string GetByProductIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProductId`,`WorkOrderId`, `BarCode`, `ContainerId`,`PackLevel`,`MaterialVersion`,  `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_barcode`  WHERE IsDeleted =0 and ProductId = @ProductId and Status=@Status and PackLevel=@PackLevel ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProductId`,`WorkOrderId`, `BarCode`, `ContainerId`,`PackLevel`,`MaterialVersion`,  `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_barcode`  WHERE Id IN @Ids ";
        #endregion
    }
}
