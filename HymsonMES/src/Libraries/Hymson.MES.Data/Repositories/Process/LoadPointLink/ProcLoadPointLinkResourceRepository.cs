using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点关联资源表仓储
    /// </summary>
    public partial class ProcLoadPointLinkResourceRepository : BaseRepository, IProcLoadPointLinkResourceRepository
    {
        public ProcLoadPointLinkResourceRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据LoadPointId批量真删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesByLoadPointIdTrueAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByLoadPointIdTrueSql, new { ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointLinkResourceEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointLinkResourceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetByResourceIdAsync(long resourceId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(GetByResourceIdSql, new { ResourceId = resourceId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceView>> GetLoadPointLinkResourceAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointLinkResourceView>(GetLoadPointLinkResourceByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointLinkResourcePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointLinkResourceEntity>> GetPagedInfoAsync(ProcLoadPointLinkResourcePagedQuery procLoadPointLinkResourcePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (procLoadPointLinkResourcePagedQuery.PageIndex - 1) * procLoadPointLinkResourcePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLoadPointLinkResourcePagedQuery.PageSize });
            sqlBuilder.AddParameters(procLoadPointLinkResourcePagedQuery);

            using var conn = GetMESDbConnection();
            var procLoadPointLinkResourceEntitiesTask = conn.QueryAsync<ProcLoadPointLinkResourceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLoadPointLinkResourceEntities = await procLoadPointLinkResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLoadPointLinkResourceEntity>(procLoadPointLinkResourceEntities, procLoadPointLinkResourcePagedQuery.PageIndex, procLoadPointLinkResourcePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLoadPointLinkResourceQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetProcLoadPointLinkResourceEntitiesAsync(ProcLoadPointLinkResourceQuery procLoadPointLinkResourceQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLoadPointLinkResourceEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");

            if (procLoadPointLinkResourceQuery.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }
            if (procLoadPointLinkResourceQuery.LoadPointId.HasValue)
            {
                sqlBuilder.Where("LoadPointId = @LoadPointId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(template.RawSql, procLoadPointLinkResourceQuery);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procLoadPointLinkResourceEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procLoadPointLinkResourceEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLoadPointLinkResourceEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointLinkResourceEntitys);
        }

        #region 顷刻

        /// <summary>
        /// 根据上料点编码获取关联的资源
        /// </summary>
        /// <param name="loadPoint"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetByCodeAsync(ProcLoadPointCodeLinkResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(GetByCodeSql, query);
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcLoadPointLinkResourceRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_load_point_link_resource` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_load_point_link_resource` /**where**/ ";
        const string GetProcLoadPointLinkResourceEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_load_point_link_resource` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_load_point_link_resource`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @ResourceId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_load_point_link_resource`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @ResourceId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_load_point_link_resource` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, ResourceId = @ResourceId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_load_point_link_resource` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, ResourceId = @ResourceId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_load_point_link_resource` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_load_point_link_resource` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string DeletesByLoadPointIdTrueSql = "DELETE  FROM `proc_load_point_link_resource` WHERE LoadPointId in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_resource`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_resource`  WHERE Id IN @ids ";
        const string GetByResourceIdSql = "SELECT * FROM proc_load_point_link_resource WHERE ResourceId = @ResourceId";
        const string GetLoadPointLinkResourceByIdsSql = @"SELECT 
                                           a.Id,  a.ResourceId, b.ResCode, b.ResName 
                            FROM `proc_load_point_link_resource` a
                            Inner JOIN proc_resource b on a.ResourceId = b.Id
                            WHERE a.LoadPointId IN @ids 
                            Order by a.CreatedOn ";
        #region 顷刻
        const string GetByCodeSql = @"
            select t2.*
            from proc_load_point t1
            inner join proc_load_point_link_resource t2 on t1.Id = t2.LoadPointId and t2.IsDeleted = 0
            where t1.LoadPoint = @LoadPoint
            and t1.IsDeleted = 0
        ";
        #endregion
    }
}
