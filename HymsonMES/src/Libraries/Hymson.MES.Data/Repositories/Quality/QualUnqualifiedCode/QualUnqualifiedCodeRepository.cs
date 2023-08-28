using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode;
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
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualUnqualifiedCodeEntity parm)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, parm);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualUnqualifiedCodeEntity> parm)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, parm);
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
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualUnqualifiedCodeEntity parm)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, parm);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualUnqualifiedCodeEntity> parm)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, parm);
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
        public async Task<QualUnqualifiedCodeEntity> GetByCodeAsync(QualUnqualifiedCodeByCodeQuery parm)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedCodeEntity>(GetByCodeSql, parm);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeEntity>> GetPagedInfoAsync(QualUnqualifiedCodePagedQuery parm)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrEmpty(parm.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(parm.Sorting);
            }

            if (!string.IsNullOrWhiteSpace(parm.UnqualifiedCode))
            {
                parm.UnqualifiedCode = $"%{parm.UnqualifiedCode}%";
                sqlBuilder.Where("UnqualifiedCode like @UnqualifiedCode");
            }
            if (!string.IsNullOrWhiteSpace(parm.UnqualifiedCodeName))
            {
                parm.UnqualifiedCodeName = $"%{parm.UnqualifiedCodeName}%";
                sqlBuilder.Where("UnqualifiedCodeName like @UnqualifiedCodeName");
            }

            if (parm.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }

            if (parm.Type.HasValue)
            {
                sqlBuilder.Where("Type=@Type");
            }

            var offSet = (parm.PageIndex - 1) * parm.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = parm.PageSize });
            sqlBuilder.AddParameters(parm);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedCodeEntitiesTask = conn.QueryAsync<QualUnqualifiedCodeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedCodeEntities = await qualUnqualifiedCodeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualUnqualifiedCodeEntity>(qualUnqualifiedCodeEntities, parm.PageIndex, parm.PageSize, totalCount);
        }

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeGroupRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long Id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedCodeGroupRelationView>(GetQualUnqualifiedCodeGroupRelationSqlTemplate, new { Id = Id });
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
            if (query.UnqualifiedGroupId.HasValue)
            {
                sqlBuilder.Where("gr.UnqualifiedGroupId=@UnqualifiedGroupId");
            }

            if (query.UnqualifiedGroupIds != null && query.UnqualifiedGroupIds.Any())
            {
                sqlBuilder.Where("gr.UnqualifiedGroupId in @UnqualifiedGroupIds");
            }

            if (query.StatusArr != null && query.StatusArr.Length > 0)
            {
                sqlBuilder.Where("uc.Status in @StatusArr");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedCodes = await conn.QueryAsync<QualUnqualifiedCodeEntity>(template.RawSql, query);
            return qualUnqualifiedCodes;
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
    }
}
