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
        /// 根据编码批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetByCodesAsync(QualUnqualifiedCodeByCodesQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedCodeEntity>(GetByCodesSql, param);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeEntity>> GetPagedInfoAsync(QualUnqualifiedCodePagedQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrEmpty(param.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(param.Sorting);
            }

            if (!string.IsNullOrWhiteSpace(param.UnqualifiedCode))
            {
                param.UnqualifiedCode = $"%{param.UnqualifiedCode}%";
                sqlBuilder.Where("UnqualifiedCode like @UnqualifiedCode");
            }
            if (!string.IsNullOrWhiteSpace(param.UnqualifiedCodeName))
            {
                param.UnqualifiedCodeName = $"%{param.UnqualifiedCodeName}%";
                sqlBuilder.Where("UnqualifiedCodeName like @UnqualifiedCodeName");
            }

            if (param.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }

            if (param.Type.HasValue)
            {
                sqlBuilder.Where("Type=@Type");
            }

            var offSet = (param.PageIndex - 1) * param.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = param.PageSize });
            sqlBuilder.AddParameters(param);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedCodeEntitiesTask = conn.QueryAsync<QualUnqualifiedCodeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedCodeEntities = await qualUnqualifiedCodeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualUnqualifiedCodeEntity>(qualUnqualifiedCodeEntities, param.PageIndex, param.PageSize, totalCount);
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

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetListByGroupIdAsync(QualUnqualifiedCodeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetListByGroupIdTemplate);
            sqlBuilder.Select("uc.*");
            sqlBuilder.Where("uc.IsDeleted=0");
            sqlBuilder.Where($"uc.SiteId =@SiteId");
            sqlBuilder.LeftJoin("qual_unqualified_code_group_relation gr on uc.Id =gr.UnqualifiedCodeId and gr.IsDeleted =0  ");
            sqlBuilder.Where("gr.UnqualifiedGroupId=@UnqualifiedGroupId");
            if (query.StatusArr != null && query.StatusArr.Length > 0)
            {
                sqlBuilder.Where("uc.Status in @StatusArr");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedCodes = await conn.QueryAsync<QualUnqualifiedCodeEntity>(template.RawSql, query);
            return qualUnqualifiedCodes;
        }

        /// <summary>
        /// 列表拆线呢
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeEntity>> GetListAsync(QualUnqualifiedCodeQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedCodeEntity>(GetListSql, query);
        }
    }

    /// <summary>
    /// 不合格代码sql模板
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class QualUnqualifiedCodeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_unqualified_code` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_unqualified_code` /**where**/ ";

        const string GetListByGroupIdTemplate = @"SELECT /**select**/ FROM `qual_unqualified_code` uc  /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/  ";

        const string GetQualUnqualifiedCodeGroupRelationSqlTemplate = @"SELECT  QUCGR.Id,QUCGR.UnqualifiedGroupId,QUG.UnqualifiedGroup,QUG.UnqualifiedGroupName,QUCGR.CreatedBy,QUCGR.CreatedOn,QUCGR.UpdatedBy,QUCGR.UpdatedOn
                                                                        FROM qual_unqualified_code QUC 
                                                                        LEFT JOIN qual_unqualified_code_group_relation QUCGR ON QUC.Id=QUCGR.UnqualifiedCodeId AND QUCGR.IsDeleted=0
                                                                        LEFT JOIN qual_unqualified_group QUG on QUCGR.UnqualifiedGroupId=QUG.Id AND QUG.IsDeleted=0
                                                                        WHERE QUC.Id=@Id AND QUC.IsDeleted=0";
        const string InsertSql = "INSERT INTO `qual_unqualified_code`(  `Id`, `SiteId`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @UnqualifiedCode, @UnqualifiedCodeName, @Status, @Type, @Degree, @ProcessRouteId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_unqualified_code`(  `Id`, `SiteId`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @UnqualifiedCode, @UnqualifiedCodeName, @Status, @Type, @Degree, @ProcessRouteId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `qual_unqualified_code` SET    UnqualifiedCodeName = @UnqualifiedCodeName, Status = @Status, Type = @Type, Degree = @Degree, ProcessRouteId = @ProcessRouteId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string UpdatesSql = "UPDATE `qual_unqualified_code` SET   UnqualifiedCode = @UnqualifiedCode, UnqualifiedCodeName = @UnqualifiedCodeName, Status = @Status, Type = @Type, Degree = @Degree, ProcessRouteId = @ProcessRouteId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string DeleteRangSql = "UPDATE `qual_unqualified_code` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                             Id, SiteId, UnqualifiedCode, UnqualifiedCodeName, Status, Type, Degree, ProcessRouteId, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                            FROM `qual_unqualified_code`  WHERE Id = @Id AND IsDeleted=0";
        const string GetByIdsSql = @"SELECT 
                                          Id, SiteId, UnqualifiedCode, UnqualifiedCodeName, Status, Type, Degree, ProcessRouteId, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                            FROM `qual_unqualified_code`  WHERE Id IN @ids AND IsDeleted=0  ";
        const string GetByCodeSql = @"SELECT 
                               Id, SiteId, UnqualifiedCode, UnqualifiedCodeName, Status, Type, Degree, ProcessRouteId, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                            FROM `qual_unqualified_code`  WHERE UnqualifiedCode = @Code  AND SiteId=@Site AND IsDeleted=0 ";

        const string GetByCodesSql = @"SELECT 
                               Id, SiteId, UnqualifiedCode, UnqualifiedCodeName, Status, Type, Degree, ProcessRouteId, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                            FROM `qual_unqualified_code`  WHERE UnqualifiedCode IN @Codes  AND SiteId=@Site AND IsDeleted=0 ";

        const string GetListSql = "SELECT * FROM `qual_unqualified_code` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
    }
}
