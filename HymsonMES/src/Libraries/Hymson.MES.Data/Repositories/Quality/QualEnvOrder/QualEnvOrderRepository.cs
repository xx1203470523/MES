/*
 *creator: Karl
 *
 *describe: 环境检验单 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.QualEnvOrder
{
    /// <summary>
    /// 环境检验单仓储
    /// </summary>
    public partial class QualEnvOrderRepository :BaseRepository, IQualEnvOrderRepository
    {

        public QualEnvOrderRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<QualEnvOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualEnvOrderEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualEnvOrderEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualEnvOrderPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvOrderEntity>> GetPagedInfoAsync(QualEnvOrderPagedQuery qualEnvOrderPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (qualEnvOrderPagedQuery.WorkCenterId.HasValue)
            {
                sqlBuilder.Where("WorkCenterId=@WorkCenterId");
            }
            if (qualEnvOrderPagedQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }
            if (qualEnvOrderPagedQuery.InspectionDate != null && qualEnvOrderPagedQuery.InspectionDate.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = qualEnvOrderPagedQuery.InspectionDate[0], EndTime = qualEnvOrderPagedQuery.InspectionDate[1].AddDays(1) });
                sqlBuilder.Where("T.UpdatedOn >= @StartTime AND T.UpdatedOn < @EndTime");
            }

            var offSet = (qualEnvOrderPagedQuery.PageIndex - 1) * qualEnvOrderPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = qualEnvOrderPagedQuery.PageSize });
            sqlBuilder.AddParameters(qualEnvOrderPagedQuery);

            using var conn = GetMESDbConnection();
            var qualEnvOrderEntitiesTask = conn.QueryAsync<QualEnvOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualEnvOrderEntities = await qualEnvOrderEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualEnvOrderEntity>(qualEnvOrderEntities, qualEnvOrderPagedQuery.PageIndex, qualEnvOrderPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="qualEnvOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderEntity>> GetQualEnvOrderEntitiesAsync(QualEnvOrderQuery qualEnvOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetQualEnvOrderEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var qualEnvOrderEntities = await conn.QueryAsync<QualEnvOrderEntity>(template.RawSql, qualEnvOrderQuery);
            return qualEnvOrderEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualEnvOrderEntity qualEnvOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, qualEnvOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualEnvOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualEnvOrderEntity> qualEnvOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, qualEnvOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualEnvOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualEnvOrderEntity qualEnvOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, qualEnvOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualEnvOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualEnvOrderEntity> qualEnvOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, qualEnvOrderEntitys);
        }
        #endregion

    }

    public partial class QualEnvOrderRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_env_order` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_env_order` /**where**/ ";
        const string GetQualEnvOrderEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_env_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `qual_env_order`(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkCenterId`, `ProcedureId`, `IsAbnormal`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @WorkCenterId, @ProcedureId, @IsAbnormal, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_env_order`(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkCenterId`, `ProcedureId`, `IsAbnormal`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @WorkCenterId, @ProcedureId, @IsAbnormal, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `qual_env_order` SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, WorkCenterId = @WorkCenterId, ProcedureId = @ProcedureId, IsAbnormal = @IsAbnormal, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_env_order` SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, WorkCenterId = @WorkCenterId, ProcedureId = @ProcedureId, IsAbnormal = @IsAbnormal, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `qual_env_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_env_order` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkCenterId`, `ProcedureId`, `IsAbnormal`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_env_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkCenterId`, `ProcedureId`, `IsAbnormal`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_env_order`  WHERE Id IN @Ids ";
        #endregion
    }
}
