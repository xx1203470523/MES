/*
 *creator: Karl
 *
 *describe: 马威FQC检验 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualFqcInspectionMaval;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验仓储
    /// </summary>
    public partial class QualFqcInspectionMavalRepository : BaseRepository, IQualFqcInspectionMavalRepository
    {
        public QualFqcInspectionMavalRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<QualFqcInspectionMavalEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcInspectionMavalEntity>(GetByIdSql, new { Id = id });
        }


        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<QualFqcInspectionMavalEntity> GetBySFCAsync(QualFqcInspectionMavalQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcInspectionMavalEntity>(GetBySFCSql, param);
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcInspectionMavalEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcInspectionMavalEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualFqcInspectionMavalPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcInspectionMavalEntity>> GetPagedInfoAsync(
            QualFqcInspectionMavalPagedQuery qualFqcInspectionMavalPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            if (string.IsNullOrEmpty(qualFqcInspectionMavalPagedQuery.Remark) == false)
            {
                qualFqcInspectionMavalPagedQuery.Remark = $"%{qualFqcInspectionMavalPagedQuery.Remark}%";
                sqlBuilder.Where("Remark LIKE @Remark");
            }

            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("CreatedOn DESC");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

            var offSet = (qualFqcInspectionMavalPagedQuery.PageIndex - 1) * qualFqcInspectionMavalPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = qualFqcInspectionMavalPagedQuery.PageSize });
            sqlBuilder.AddParameters(qualFqcInspectionMavalPagedQuery);

            using var conn = GetMESDbConnection();
            var qualFqcInspectionMavalEntitiesTask =
                conn.QueryAsync<QualFqcInspectionMavalEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualFqcInspectionMavalEntities = await qualFqcInspectionMavalEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcInspectionMavalEntity>(qualFqcInspectionMavalEntities,
                qualFqcInspectionMavalPagedQuery.PageIndex, qualFqcInspectionMavalPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcInspectionMavalEntity>> GetQualFqcInspectionMavalEntitiesAsync(
            QualFqcInspectionMavalQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetQualFqcInspectionMavalEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.SFCs != null && query.SFCs.Any())
            {
                sqlBuilder.Where("Sfc IN @SFCs");
            }

            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            var qualFqcInspectionMavalEntities =
                await conn.QueryAsync<QualFqcInspectionMavalEntity>(template.RawSql, query);
            return qualFqcInspectionMavalEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcInspectionMavalEntity qualFqcInspectionMavalEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, qualFqcInspectionMavalEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualFqcInspectionMavalEntity> qualFqcInspectionMavalEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, qualFqcInspectionMavalEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcInspectionMavalEntity qualFqcInspectionMavalEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, qualFqcInspectionMavalEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualFqcInspectionMavalEntity> qualFqcInspectionMavalEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, qualFqcInspectionMavalEntitys);
        }

        #endregion
    }

    public partial class QualFqcInspectionMavalRepository
    {
        #region

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_fqc_inspection_maval` /**innerjoin**/ /**leftjoin**/ /**where**/  /**orderby**/  LIMIT @Offset,@Rows ";
        
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_fqc_inspection_maval` /**where**/ ";

        const string GetQualFqcInspectionMavalEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_fqc_inspection_maval` /**where**/  ";

        const string InsertSql =
            "INSERT INTO `qual_fqc_inspection_maval`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `SFC`, `Qty`, `JudgmentResults`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @SFC, @Qty, @JudgmentResults, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark )  ";

        const string InsertsSql =
            "INSERT INTO `qual_fqc_inspection_maval`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `SFC`, `Qty`, `JudgmentResults`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @SFC, @Qty, @JudgmentResults, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark )  ";

        const string UpdateSql =
            "UPDATE `qual_fqc_inspection_maval` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, SFC = @SFC, Qty = @Qty, JudgmentResults = @JudgmentResults, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark  WHERE Id = @Id ";

        const string UpdatesSql =
            "UPDATE `qual_fqc_inspection_maval` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, SFC = @SFC, Qty = @Qty, JudgmentResults = @JudgmentResults, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `qual_fqc_inspection_maval` SET IsDeleted = Id WHERE Id = @Id ";

        const string DeletesSql =
            "UPDATE `qual_fqc_inspection_maval` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `SFC`, `Qty`, `JudgmentResults`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_fqc_inspection_maval`  WHERE Id = @Id ";

        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `SFC`, `Qty`, `JudgmentResults`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_fqc_inspection_maval`  WHERE Id IN @Ids ";

        const string GetBySFCSql = @"SELECT  
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `SFC`, `Qty`, `JudgmentResults`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_fqc_inspection_maval`  WHERE SFC = @SFC AND SiteId=@SiteId";

        #endregion
    }
}