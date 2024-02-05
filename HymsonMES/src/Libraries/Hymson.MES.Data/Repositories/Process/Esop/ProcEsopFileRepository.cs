using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// esop 文件仓储
    /// </summary>
    public partial class ProcEsopFileRepository :BaseRepository, IProcEsopFileRepository
    {

        public ProcEsopFileRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcEsopFileEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcEsopFileEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEsopFileEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcEsopFileEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEsopFilePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEsopFileEntity>> GetPagedInfoAsync(ProcEsopFilePagedQuery procEsopFilePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (procEsopFilePagedQuery.PageIndex - 1) * procEsopFilePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procEsopFilePagedQuery.PageSize });
            sqlBuilder.AddParameters(procEsopFilePagedQuery);

            using var conn = GetMESDbConnection();
            var procEsopFileEntitiesTask = conn.QueryAsync<ProcEsopFileEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procEsopFileEntities = await procEsopFileEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcEsopFileEntity>(procEsopFileEntities, procEsopFilePagedQuery.PageIndex, procEsopFilePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procEsopFileQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEsopFileEntity>> GetProcEsopFileEntitiesAsync(ProcEsopFileQuery procEsopFileQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcEsopFileEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");
            if (procEsopFileQuery.EsopId.HasValue)
            {
                sqlBuilder.Where("EsopId = @EsopId");
            }

            if (procEsopFileQuery.EsopIds != null && procEsopFileQuery.EsopIds.Any()) {
                sqlBuilder.Where("EsopId IN @EsopIds");
            }
            using var conn = GetMESDbConnection();
            var procEsopFileEntities = await conn.QueryAsync<ProcEsopFileEntity>(template.RawSql, procEsopFileQuery);
            return procEsopFileEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEsopFileEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcEsopFileEntity procEsopFileEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procEsopFileEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEsopFileEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcEsopFileEntity> procEsopFileEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procEsopFileEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEsopFileEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcEsopFileEntity procEsopFileEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procEsopFileEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procEsopFileEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcEsopFileEntity> procEsopFileEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procEsopFileEntitys);
        }
        #endregion

    }

    public partial class ProcEsopFileRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_esop_file` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_esop_file` /**where**/ ";
        const string GetProcEsopFileEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_esop_file` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_esop_file`(  `Id`, `SiteId`, `EsopId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EsopId, @AttachmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_esop_file`(  `Id`, `SiteId`, `EsopId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EsopId, @AttachmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_esop_file` SET  EsopId = @EsopId,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_esop_file` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_esop_file` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `EsopId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_esop_file`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `EsopId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_esop_file`  WHERE Id IN @Ids ";
        #endregion
    }
}
