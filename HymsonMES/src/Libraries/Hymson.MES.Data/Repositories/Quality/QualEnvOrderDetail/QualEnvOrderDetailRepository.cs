/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.QualEnvParameterGroupDetailSnapshoot;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细仓储
    /// </summary>
    public partial class QualEnvOrderDetailRepository : BaseRepository, IQualEnvOrderDetailRepository
    {

        public QualEnvOrderDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<QualEnvOrderDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualEnvOrderDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualEnvOrderDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualEnvOrderDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvOrderDetailEntity>> GetPagedInfoAsync(QualEnvOrderDetailPagedQuery qualEnvOrderDetailPagedQuery)
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

            var offSet = (qualEnvOrderDetailPagedQuery.PageIndex - 1) * qualEnvOrderDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = qualEnvOrderDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(qualEnvOrderDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var qualEnvOrderDetailEntitiesTask = conn.QueryAsync<QualEnvOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualEnvOrderDetailEntities = await qualEnvOrderDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualEnvOrderDetailEntity>(qualEnvOrderDetailEntities, qualEnvOrderDetailPagedQuery.PageIndex, qualEnvOrderDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="qualEnvOrderDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderDetailEntity>> GetQualEnvOrderDetailEntitiesAsync(QualEnvOrderDetailQuery qualEnvOrderDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetQualEnvOrderDetailEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var qualEnvOrderDetailEntities = await conn.QueryAsync<QualEnvOrderDetailEntity>(template.RawSql, qualEnvOrderDetailQuery);
            return qualEnvOrderDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualEnvOrderDetailEntity qualEnvOrderDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, qualEnvOrderDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualEnvOrderDetailEntity> qualEnvOrderDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, qualEnvOrderDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualEnvOrderDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualEnvOrderDetailEntity qualEnvOrderDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, qualEnvOrderDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualEnvOrderDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualEnvOrderDetailEntity> qualEnvOrderDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, qualEnvOrderDetailEntitys);
        }
        #endregion

        #region 
        /// <summary>
        /// 根据检验单ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvOrderDetailEntity>> GetByEnvOrderIdAsync(long envOrderId)
        {
            using var conn = GetMESDbConnection();  
            return await conn.QueryAsync<QualEnvOrderDetailEntity>(GetByEnvOrderIdSql, new { EnvOrderId = envOrderId });
        }


        /// <summary>
        /// 根据ID获取快照明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvParameterGroupDetailSnapshootEntity>> GetGroupDetailSnapshootByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualEnvParameterGroupDetailSnapshootEntity>(GetGroupDetailSnapshootSql, new { Ids = ids });
        }
        #endregion

    }

    public partial class QualEnvOrderDetailRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_env_order_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_env_order_detail` /**where**/ ";
        const string GetQualEnvOrderDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_env_order_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `qual_env_order_detail`(  `Id`, `SiteId`, `EnvOrderId`, `GroupDetailSnapshootId`, `StartTime`, `EndTime`, `RealTime`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EnvOrderId, @GroupDetailSnapshootId, @StartTime, @EndTime, @RealTime, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_env_order_detail`(  `Id`, `SiteId`, `EnvOrderId`, `GroupDetailSnapshootId`, `StartTime`, `EndTime`, `RealTime`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EnvOrderId, @GroupDetailSnapshootId, @StartTime, @EndTime, @RealTime, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `qual_env_order_detail` SET   SiteId = @SiteId, EnvOrderId = @EnvOrderId, GroupDetailSnapshootId = @GroupDetailSnapshootId, StartTime = @StartTime, EndTime = @EndTime, RealTime = @RealTime, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_env_order_detail` SET   SiteId = @SiteId, EnvOrderId = @EnvOrderId, GroupDetailSnapshootId = @GroupDetailSnapshootId, StartTime = @StartTime, EndTime = @EndTime, RealTime = @RealTime, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `qual_env_order_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_env_order_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `EnvOrderId`, `GroupDetailSnapshootId`, `StartTime`, `EndTime`, `RealTime`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_env_order_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `EnvOrderId`, `GroupDetailSnapshootId`, `StartTime`, `EndTime`, `RealTime`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_env_order_detail`  WHERE Id IN @Ids ";


        const string GetByEnvOrderIdSql = @"SELECT 
                               `Id`, `SiteId`, `EnvOrderId`, `GroupDetailSnapshootId`, `StartTime`, `EndTime`, `RealTime`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_env_order_detail`  WHERE EnvOrderId = @EnvOrderId ";

        const string GetGroupDetailSnapshootSql = @"SELECT  *  FROM `qual_env_parameter_group_detail_snapshoot`  WHERE Id in @Ids ";
        #endregion
    }
}
