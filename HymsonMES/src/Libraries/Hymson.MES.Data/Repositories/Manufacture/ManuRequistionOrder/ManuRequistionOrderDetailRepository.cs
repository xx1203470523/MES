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
using Hymson.MES.Data.Repositories.QualFqcInspectionMaval;
using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder
{
    /// <summary>
    /// 生产领料单明细仓储
    /// </summary>
    public partial class ManuRequistionOrderDetailRepository : BaseRepository, IManuRequistionOrderDetailRepository
    {

        public ManuRequistionOrderDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
            return await conn.QueryFirstOrDefaultAsync<ManuRequistionOrderDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRequistionOrderDetailEntity>(GetByIdsSql, new { Ids = ids });
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
        /// 根据生产领料单ID，查询生产领料单明细信息（manu_requistion_order_detail）
        /// </summary>
        /// <param name="manuRequistionOrderDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRequistionOrderDetailEntity>> GetManuRequistionOrderDetailEntitiesAsync(ManuRequistionOrderDetailQuery manuRequistionOrderDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuRequistionOrderDetailEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            if (manuRequistionOrderDetailQuery.RequistionOrderIds != null && manuRequistionOrderDetailQuery.RequistionOrderIds.Any())
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
            return await conn.ExecuteAsync(InsertSql, manuRequistionOrderDetailEntitys);
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
            return await conn.ExecuteAsync(UpdateSql, manuRequistionOrderDetailEntitys);
        }

        /// <summary>
        /// 仓库分组名称
        /// </summary>
        /// <returns></returns>
        public async Task<List<ManuRequistionOrderGroupDto>> GetManuRequistionOrderGroupListAsync()
        {
            string sql = "select Warehouse from manu_requistion_order where Warehouse is not null GROUP BY Warehouse ";
            using var conn = GetMESDbConnection();
            var dbListTask = conn.QueryAsync<ManuRequistionOrderGroupDto>(sql);
            var dbList = await dbListTask;
            var list = dbList as List<ManuRequistionOrderGroupDto>;
            list.Insert(0, new ManuRequistionOrderGroupDto());
            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportRequistionOrderResultDto>> GetReportPagedInfoAsync(ReportRequistionOrderQueryDto param)
        {
            string whereSql = string.Empty;
            if(string.IsNullOrEmpty(param.OrderCode) == false)
            {
                whereSql += $" and t3.OrderCode like '%{param.OrderCode}%' ";
            }
            if(string.IsNullOrEmpty(param.Warehouse) == false)
            {
                whereSql += $" and t2.Warehouse like '%{param.Warehouse}%' ";
            }
            if(string.IsNullOrEmpty(param.MaterialCode) == false)
            {
                whereSql += $" and t5.MaterialCode like '%{param.MaterialCode}%' ";
            }
            if (string.IsNullOrEmpty(param.MaterialName) == false)
            {
                whereSql += $" and t5.MaterialName like '%{param.MaterialName}%' ";
            }
            if (param.ReqDate != null && param.ReqDate.Count() == 2)
            {
                whereSql += $" and t1.CreatedOn > '{((DateTime)param.ReqDate[0]).ToString("yyyy-MM-dd HH:mm:ss")}' and t1.CreatedOn < '{((DateTime)param.ReqDate[1]).ToString("yyyy-MM-dd HH:mm:ss")}' ";
            }
            if(param.Status != null)
            {
                whereSql += $" and t2.Status  = {(int)param.Status} ";
            }
            if(string.IsNullOrEmpty(param.CreatedBy) == false)
            {
                whereSql += $" and t1.CreatedBy like '%{param.CreatedBy}%' ";
            }

            string sql = $@"
                select t1.CreatedOn ReqDate,t1.UpdatedOn OutWmsDate,t3.OrderCode ,t2.ReqOrderCode,t5.MaterialCode ,t5.MaterialName , t5.Specifications ,t6.Name Unit ,
	                t3.Qty OrderQty,t1.Qty ReqQty, t4.WorkPlanCode , t2.Warehouse , t1.CreatedBy ,t2.Status ,  t2.ReqOrderCode ,t2.`Type` 
                from manu_requistion_order_detail t1
                inner join manu_requistion_order t2 on t1.RequistionOrderId = t2.Id and t2.IsDeleted = 0
                inner join plan_work_order t3 on t3.Id = t2.WorkOrderId and t3.IsDeleted = 0
                inner join plan_work_plan t4 on t4.Id = t3.WorkPlanId and t4.IsDeleted = 0
                inner join proc_material t5 on t5.Id = t1.MaterialId and t5.IsDeleted  = 0
                left join inte_unit t6 on t6.Code = t5.Unit and t6.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
                order by t1.CreatedOn desc
                 limit {(param.PageIndex - 1) * param.PageSize},{param.PageSize}
            ";

            string countSql = $@"
                select count(*)
                from manu_requistion_order_detail t1
                inner join manu_requistion_order t2 on t1.RequistionOrderId = t2.Id and t2.IsDeleted = 0
                inner join plan_work_order t3 on t3.Id = t2.WorkOrderId and t3.IsDeleted = 0
                inner join plan_work_plan t4 on t4.Id = t3.WorkPlanId and t4.IsDeleted = 0
                inner join proc_material t5 on t5.Id = t1.MaterialId and t5.IsDeleted  = 0
                left join inte_unit t6 on t6.Code = t5.Unit and t6.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
            ";

            using var conn = GetMESDbConnection();
            var dbListTask = conn.QueryAsync<ReportRequistionOrderResultDto>(sql);
            var totalCountTask = conn.ExecuteScalarAsync<int>(countSql);
            var dbList = await dbListTask;
            var totalCount = await totalCountTask;
            
            return new PagedInfo<ReportRequistionOrderResultDto>(dbList, param.PageIndex, param.PageSize, totalCount);
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuRequistionOrderDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_requistion_order_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_requistion_order_detail` /**where**/ ";
        const string GetManuRequistionOrderDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_requistion_order_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_requistion_order_detail`(  `Id`, `RequistionOrderId`, ProductionOrderComponentID, `MaterialId`, `Qty`, `WarehouseId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @RequistionOrderId, @ProductionOrderComponentID, @MaterialId, @Qty, @WarehouseId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `manu_requistion_order_detail` SET RequistionOrderId = @RequistionOrderId, ProductionOrderComponentID = @ProductionOrderComponentID, MaterialId = @MaterialId,  Qty = @Qty, WarehouseId = @WarehouseId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_requistion_order_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_requistion_order_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `manu_requistion_order_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_requistion_order_detail`  WHERE Id IN @Ids ";

    }
}
