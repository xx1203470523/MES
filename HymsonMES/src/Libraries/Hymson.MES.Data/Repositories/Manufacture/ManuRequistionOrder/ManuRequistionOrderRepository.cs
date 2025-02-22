/*
 *creator: Karl
 *
 *describe: 生产领料单 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-04 02:34:15
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单仓储
    /// </summary>
    public partial class ManuRequistionOrderRepository : BaseRepository, IManuRequistionOrderRepository
    {

        public ManuRequistionOrderRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuRequistionOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuRequistionOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRequistionOrderEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderEntity>> GetByOrderCodeAsync(long orderId, long siteId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRequistionOrderEntity>(GetByOrderIdSql, new { WorkOrderId = orderId, SiteId = siteId });
        }

        public async Task<IEnumerable<ManuRequistionOrderEntity>> GetByOrderCodeByScwAsync(long orderId, long siteId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRequistionOrderEntity>(GetByOrderIdByScwSql, new { WorkOrderId = orderId, SiteId = siteId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuRequistionOrderEntity>> GetPagedInfoAsync(ManuRequistionOrderPagedQuery manuRequistionOrderPagedQuery)
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

            var offSet = (manuRequistionOrderPagedQuery.PageIndex - 1) * manuRequistionOrderPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuRequistionOrderPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuRequistionOrderPagedQuery);

            using var conn = GetMESDbConnection();
            var manuRequistionOrderEntitiesTask = conn.QueryAsync<ManuRequistionOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuRequistionOrderEntities = await manuRequistionOrderEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuRequistionOrderEntity>(manuRequistionOrderEntities, manuRequistionOrderPagedQuery.PageIndex, manuRequistionOrderPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuRequistionOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderEntity>> GetManuRequistionOrderEntitiesAsync(ManuRequistionQueryByWorkOrders manuRequistionOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuRequistionOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            if (manuRequistionOrderQuery.WorkOrderIds != null && manuRequistionOrderQuery.WorkOrderIds.Any())
            {
                sqlBuilder.Where("WorkOrderId IN @WorkOrderIds");
            }
            if (!string.IsNullOrWhiteSpace(manuRequistionOrderQuery.ReqOrderCode))
            {
                sqlBuilder.Where("ReqOrderCode=@ReqOrderCode");
            }
            sqlBuilder.AddParameters(manuRequistionOrderQuery);
            using var conn = GetMESDbConnection();
            var manuRequistionOrderEntities = await conn.QueryAsync<ManuRequistionOrderEntity>(template.RawSql, manuRequistionOrderQuery);
            return manuRequistionOrderEntities;
        }
        
        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderEntity>> GetEntitiesAsync(ManuRequistionOrderQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuRequistionOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }

            if (!string.IsNullOrWhiteSpace(query.ReqOrderCode))
            {
                sqlBuilder.Where("ReqOrderCode = @ReqOrderCode");
            }

            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            var manuRequistionOrderEntities = await conn.QueryAsync<ManuRequistionOrderEntity>(template.RawSql, query);
            return manuRequistionOrderEntities;
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuRequistionOrderEntity> GetByCodeAsync(ManuRequistionOrderQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuRequistionOrderEntity>(GetByCodeSql, new { ReqOrderCode = query.ReqOrderCode, SiteId = query.SiteId });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuRequistionOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuRequistionOrderEntity manuRequistionOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuRequistionOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuRequistionOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuRequistionOrderEntity> manuRequistionOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuRequistionOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuRequistionOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuRequistionOrderEntity manuRequistionOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuRequistionOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuRequistionOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuRequistionOrderEntity> manuRequistionOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuRequistionOrderEntitys);
        }
        #endregion

    }

    public partial class ManuRequistionOrderRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_requistion_order` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_requistion_order` /**where**/ ";
        const string GetManuRequistionOrderEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_requistion_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_requistion_order`(  `Id`, `ReqOrderCode`, `WorkOrderId`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Warehouse`) VALUES (   @Id, @ReqOrderCode, @WorkOrderId, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @Warehouse )  ";

        const string UpdateSql = "UPDATE `manu_requistion_order` SET   ReqOrderCode = @ReqOrderCode, WorkOrderId = @WorkOrderId, Type = @Type, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_requistion_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_requistion_order` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `ReqOrderCode`, `WorkOrderId`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_requistion_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `ReqOrderCode`, `WorkOrderId`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_requistion_order`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT *  FROM `manu_requistion_order` WHERE IsDeleted = 0 AND ReqOrderCode = @ReqOrderCode and SiteId=@SiteId ";
        const string GetByOrderIdSql = @"SELECT *  FROM `manu_requistion_order` WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId and SiteId=@SiteId ";
        const string GetByOrderIdByScwSql = @"SELECT t1.*  FROM `manu_requistion_order` t1 WHERE t1.IsDeleted = 0 AND t1.Status != 5 AND t1.WorkOrderId = @WorkOrderId and t1.SiteId=@SiteId ";
        #endregion
    }
}
