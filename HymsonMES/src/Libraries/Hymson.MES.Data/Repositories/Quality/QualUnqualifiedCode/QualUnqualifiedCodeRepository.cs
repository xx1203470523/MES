using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合代码仓储
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class QualUnqualifiedCodeRepository : IQualUnqualifiedCodeRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public QualUnqualifiedCodeRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualUnqualifiedCodeEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, param);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualUnqualifiedCodeEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, param);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteRangSql, param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualUnqualifiedCodeEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, param);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualUnqualifiedCodeEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedCodeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedCodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedCodeEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedCodeEntity> GetByCodeAsync(QualUnqualifiedCodeByCodeQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedCodeEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeEntity>> GetPagedInfoAsync(QualUnqualifiedCodePagedQuery pram)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where(" SiteCode= @SiteCode");
            if (!string.IsNullOrWhiteSpace(pram.UnqualifiedCode))
            {
                sqlBuilder.Where("UnqualifiedCode like '%@UnqualifiedCode%'");
            }

            if (!string.IsNullOrWhiteSpace(pram.UnqualifiedCodeName))
            {
                sqlBuilder.Where("UnqualifiedCodeName like '%@UnqualifiedCodeName%'");
            }

            if (!string.IsNullOrWhiteSpace(pram.Status))
            {
                sqlBuilder.Where("Status=@Status");
            }

            if (pram.Type != null)
            {
                sqlBuilder.Where("Type=@Type");
            }

            var offSet = (pram.PageIndex - 1) * pram.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pram.PageSize });
            sqlBuilder.AddParameters(pram);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedCodeEntitiesTask = conn.QueryAsync<QualUnqualifiedCodeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedCodeEntities = await qualUnqualifiedCodeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualUnqualifiedCodeEntity>(qualUnqualifiedCodeEntities, pram.PageIndex, pram.PageSize, totalCount);
        }

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeGroupRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedCodeGroupRelationView>(GetQualUnqualifiedCodeGroupRelationSqlTemplate, new { Id = id });
        }
    }

    /// <summary>
    /// 不合格代码sql模板
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class QualUnqualifiedCodeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_unqualified_code` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `qual_unqualified_code` /**where**/ ";
      
        const string GetQualUnqualifiedCodeGroupRelationSqlTemplate = @"SELECT  QUCGR.Id,QUCGR.UnqualifiedGroupId,QUG.UnqualifiedGroup,QUG.UnqualifiedGroupName,QUCGR.CreatedBy,QUCGR.CreatedOn,QUCGR.UpdatedBy,QUCGR.UpdatedOn
                                                                        FROM qual_unqualified_code QUC 
                                                                        LEFT JOIN qual_unqualified_code_group_relation QUCGR ON QUC.Id=QUCGR.UnqualifiedCodeId AND QUCGR.IsDeleted=0
                                                                        LEFT JOIN qual_unqualified_group QUG on QUCGR.UnqualifiedGroupId=QUG.Id AND QUG.IsDeleted=0
                                                                        WHERE QUC.Id=@Id AND QUC.IsDeleted=0";
        const string InsertSql = "INSERT INTO `qual_unqualified_code`(  `Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @UnqualifiedCode, @UnqualifiedCodeName, @Status, @Type, @Degree, @ProcessRouteId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_unqualified_code`(  `Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @UnqualifiedCode, @UnqualifiedCodeName, @Status, @Type, @Degree, @ProcessRouteId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `qual_unqualified_code` SET    UnqualifiedCode = @UnqualifiedCode, UnqualifiedCodeName = @UnqualifiedCodeName, Status = @Status, Type = @Type, Degree = @Degree, ProcessRouteId = @ProcessRouteId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string UpdatesSql = "UPDATE `qual_unqualified_code` SET   UnqualifiedCode = @UnqualifiedCode, UnqualifiedCodeName = @UnqualifiedCodeName, Status = @Status, Type = @Type, Degree = @Degree, ProcessRouteId = @ProcessRouteId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string DeleteRangSql = "UPDATE `qual_unqualified_code` SET IsDeleted = '1', UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_unqualified_code`  WHERE Id = @Id AND IsDeleted=0";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_unqualified_code`  WHERE Id IN @ids AND IsDeleted=0  ";
        const string GetByCodeSql = @"SELECT 
                               `Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_unqualified_code`  WHERE UnqualifiedCode = @UnqualifiedCode  AND Site=@Site AND IsDeleted=0 ";
    }
}
