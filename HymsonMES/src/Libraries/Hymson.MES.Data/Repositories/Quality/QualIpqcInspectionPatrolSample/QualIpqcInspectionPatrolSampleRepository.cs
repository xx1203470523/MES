using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（巡检检验单样本）
    /// </summary>
    public partial class QualIpqcInspectionPatrolSampleRepository : BaseRepository, IQualIpqcInspectionPatrolSampleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIpqcInspectionPatrolSampleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIpqcInspectionPatrolSampleEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIpqcInspectionPatrolSampleEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIpqcInspectionPatrolSampleEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIpqcInspectionPatrolSampleEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualIpqcInspectionPatrolSampleEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIpqcInspectionPatrolSampleEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionPatrolSampleEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionPatrolSampleEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionPatrolSampleEntity>> GetEntitiesAsync(QualIpqcInspectionPatrolSampleQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }
            if (query.InspectionOrderId.HasValue)
            {
                sqlBuilder.Where("IpqcInspectionPatrolId = @InspectionOrderId");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionPatrolSampleEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionPatrolSampleView>> GetPagedListAsync(QualIpqcInspectionPatrolSamplePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("qual_ipqc_inspection_parameter qiip ON T.IpqcInspectionParameterId = qiip.Id");
            sqlBuilder.LeftJoin("proc_parameter pp ON T.ParameterId = pp.Id");
            sqlBuilder.Select("T.*, pp.ParameterCode, pp.ParameterName, pp.ParameterUnit, pp.DataType, qiip.UpperLimit, qiip.LowerLimit, qiip.CenterValue, qiip.EnterNumber, qiip.IsDeviceCollect, qiip.Sequence");
            sqlBuilder.OrderBy("T.CreatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            if (pagedQuery.SiteId.HasValue)
            {
                sqlBuilder.Where("T.SiteId = @SiteId");
            }
            if (pagedQuery.InspectionOrderId.HasValue)
            {
                sqlBuilder.Where("T.IpqcInspectionPatrolId = @InspectionOrderId");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIpqcInspectionPatrolSampleView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIpqcInspectionPatrolSampleView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取检验样本数量
        /// </summary>
        /// <param name="ipqcInspectionHeadId"></param>
        /// <returns></returns>
        public async Task<int> GetCountByIpqcInspectionId(long ipqcInspectionId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteScalarAsync<int>(GetCountByIpqcInspectionIdsql, new { ipqcInspectionId = ipqcInspectionId });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIpqcInspectionPatrolSampleRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_ipqc_inspection_patrol_sample T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_ipqc_inspection_patrol_sample T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM qual_ipqc_inspection_patrol_sample /**where**/  ";
        const string GetCountByIpqcInspectionIdsql = @"SELECT 
                                           COUNT( DISTINCT Barcode) 
                                           FROM qual_ipqc_inspection_patrol_sample  WHERE IpqcInspectionPatrolId = @ipqcInspectionId AND IsDeleted=0  ";

        const string InsertSql = "INSERT INTO qual_ipqc_inspection_patrol_sample(  `Id`, `SiteId`, `IpqcInspectionPatrolId`, `IpqcInspectionParameterId`, `InspectionParameterGroupDetailId`, `ParameterId`, `Barcode`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @IpqcInspectionPatrolId, @IpqcInspectionParameterId, @InspectionParameterGroupDetailId, @ParameterId, @Barcode, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_ipqc_inspection_patrol_sample(  `Id`, `SiteId`, `IpqcInspectionPatrolId`, `IpqcInspectionParameterId`, `InspectionParameterGroupDetailId`, `ParameterId`, `Barcode`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @IpqcInspectionPatrolId, @IpqcInspectionParameterId, @InspectionParameterGroupDetailId, @ParameterId, @Barcode, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_ipqc_inspection_patrol_sample SET   SiteId = @SiteId, IpqcInspectionPatrolId = @IpqcInspectionPatrolId, IpqcInspectionParameterId = @IpqcInspectionParameterId, InspectionParameterGroupDetailId = @InspectionParameterGroupDetailId, ParameterId = @ParameterId, Barcode = @Barcode, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_ipqc_inspection_patrol_sample SET   SiteId = @SiteId, IpqcInspectionPatrolId = @IpqcInspectionPatrolId, IpqcInspectionParameterId = @IpqcInspectionParameterId, InspectionParameterGroupDetailId = @InspectionParameterGroupDetailId, ParameterId = @ParameterId, Barcode = @Barcode, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_ipqc_inspection_patrol_sample SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_ipqc_inspection_patrol_sample SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_ipqc_inspection_patrol_sample WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_ipqc_inspection_patrol_sample WHERE Id IN @Ids ";

    }
}
