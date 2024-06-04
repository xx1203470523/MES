/*
 *creator: Karl
 *
 *describe: 生产汇总表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产汇总表仓储
    /// </summary>
    public partial class ManuSfcSummaryRepository : BaseRepository, IManuSfcSummaryRepository
    {

        public ManuSfcSummaryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuSfcSummaryEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcSummaryEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcSummaryEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcSummaryPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcSummaryEntity>> GetPagedInfoAsync(ManuSfcSummaryPagedQuery manuSfcSummaryPagedQuery)
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

            var offSet = (manuSfcSummaryPagedQuery.PageIndex - 1) * manuSfcSummaryPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcSummaryPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcSummaryPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcSummaryEntitiesTask = conn.QueryAsync<ManuSfcSummaryEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcSummaryEntities = await manuSfcSummaryEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcSummaryEntity>(manuSfcSummaryEntities, manuSfcSummaryPagedQuery.PageIndex, manuSfcSummaryPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcSummaryQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetManuSfcSummaryEntitiesAsync(ManuSfcSummaryQuery manuSfcSummaryQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcSummaryEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (manuSfcSummaryQuery.ProcedureIds != null && manuSfcSummaryQuery.ProcedureIds.Any())
            {
                sqlBuilder.Where("ProcedureId in @ProcedureIds");
            }
            if (manuSfcSummaryQuery.EquipmentId.HasValue)
            {
                sqlBuilder.Where("EquipmentId = @EquipmentId");
            }
            if (manuSfcSummaryQuery.EquipmentIds != null && manuSfcSummaryQuery.EquipmentIds.Length > 0)
            {
                sqlBuilder.Where("EquipmentId IN @EquipmentIds");
            }
            if (manuSfcSummaryQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }
            if (manuSfcSummaryQuery.SFCS != null && manuSfcSummaryQuery.SFCS.Any())
            {
                sqlBuilder.Where("SFC IN @SFCS");
            }
            if (manuSfcSummaryQuery.FirstQualityStatus.HasValue)
            {
                sqlBuilder.Where("FirstQualityStatus = @FirstQualityStatus");
            }
            if (manuSfcSummaryQuery.QualityStatus.HasValue)
            {
                sqlBuilder.Where("QualityStatus = @QualityStatus");
            }

            if (manuSfcSummaryQuery.IsReplenish.HasValue)
            {
                sqlBuilder.Where("IsReplenish = @IsReplenish");
            }
            if (manuSfcSummaryQuery.StartTime.HasValue)
            {
                sqlBuilder.Where($"CreatedOn >= '{manuSfcSummaryQuery.StartTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}'");
            }
            if (manuSfcSummaryQuery.EndTime.HasValue)
            {
                sqlBuilder.Where($"CreatedOn < '{manuSfcSummaryQuery.EndTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}'");
            }
            using var conn = GetMESDbConnection();
            var manuSfcSummaryEntities = await conn.QueryAsync<ManuSfcSummaryEntity>(template.RawSql, manuSfcSummaryQuery);
            return manuSfcSummaryEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcSummaryEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcSummaryEntity manuSfcSummaryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcSummaryEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcSummaryEntitys);
        }

        /// <summary>
        /// 批量新增记录表
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsRecordAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsRecordSql, manuSfcSummaryEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcSummaryEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcSummaryEntity manuSfcSummaryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcSummaryEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcSummaryEntitys);
        }

        /// <summary>
        /// 更新是否补料状态
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateIsReplenish(List<ManuSfcSummaryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateIsReplenishSql, entities);
        }

        /// <summary>
        /// 存在更新，不存在新增
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateRangeAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertOrUpdateSql, manuSfcSummaryEntitys);
        }

        /// <summary>
        /// 获取汇总数
        /// </summary>
        /// <param name="manuSfcSummaryQuery"></param>
        /// <returns></returns>
        public async Task<decimal> GetSumQtyAsync(ManuSfcSummaryQuery manuSfcSummaryQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcSummaryEntitiesSqlTemplate);
            sqlBuilder.Select("SUM(qty)");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (manuSfcSummaryQuery.ProcedureIds != null && manuSfcSummaryQuery.ProcedureIds.Any())
            {
                sqlBuilder.Where("ProcedureId in @ProcedureIds");
            }
            if (manuSfcSummaryQuery.EquipmentId.HasValue)
            {
                sqlBuilder.Where("EquipmentId = @EquipmentId");
            }
            if (manuSfcSummaryQuery.EquipmentIds != null && manuSfcSummaryQuery.EquipmentIds.Length > 0)
            {
                sqlBuilder.Where("EquipmentId IN @EquipmentIds");
            }
            if (manuSfcSummaryQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }
            if (manuSfcSummaryQuery.SFCS != null && manuSfcSummaryQuery.SFCS.Any())
            {
                sqlBuilder.Where("SFC IN @SFCS");
            }
            if (manuSfcSummaryQuery.FirstQualityStatus.HasValue)
            {
                sqlBuilder.Where("FirstQualityStatus = @FirstQualityStatus");
            }
            if (manuSfcSummaryQuery.QualityStatus.HasValue)
            {
                sqlBuilder.Where("QualityStatus = @QualityStatus");
            }

            if (manuSfcSummaryQuery.IsReplenish.HasValue)
            {
                sqlBuilder.Where("IsReplenish = @IsReplenish");
            }
            if (manuSfcSummaryQuery.StartTime.HasValue)
            {
                sqlBuilder.Where("EndTime >= @StartTime");
            }
            if (manuSfcSummaryQuery.EndTime.HasValue)
            {
                sqlBuilder.Where("EndTime < @EndTime");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstAsync<decimal>(template.RawSql, manuSfcSummaryQuery);
        }
        #endregion

    }

    public partial class ManuSfcSummaryRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_summary` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_summary` /**where**/ ";
        const string GetManuSfcSummaryEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_summary` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsRecordSql = "INSERT INTO `manu_sfc_summary_record`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string InsertOrUpdateSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted ) ON DUPLICATE KEY UPDATE" +
                                        " BeginTime=@BeginTime,EndTime=@EndTime,RepeatedCount=@RepeatedCount,Qty=@Qty,NgNum=@NgNum,FirstQualityStatus=@FirstQualityStatus,QualityStatus=@QualityStatus,UpdatedBy=@UpdatedBy,UpdatedOn=NOW(),IsDeleted=@IsDeleted ";

        const string UpdateSql = "UPDATE `manu_sfc_summary` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, BeginTime = @BeginTime, EndTime = @EndTime, RepeatedCount = @RepeatedCount, Qty = @Qty, NgNum = @NgNum, FirstQualityStatus = @FirstQualityStatus, QualityStatus = @QualityStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_summary` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, BeginTime = @BeginTime, EndTime = @EndTime, RepeatedCount = @RepeatedCount, Qty = @Qty, NgNum = @NgNum, FirstQualityStatus = @FirstQualityStatus, QualityStatus = @QualityStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateIsReplenishSql = "UPDATE `manu_sfc_summary` SET IsReplenish = @IsReplenish WHERE Id = @Id ";


        const string DeleteSql = "UPDATE `manu_sfc_summary` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_summary` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_summary`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_summary`  WHERE Id IN @Ids ";
        #endregion
    }
}
