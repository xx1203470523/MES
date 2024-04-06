using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓库标签模板仓储
    /// </summary>
    public partial class ProcLabelTemplateRepository : BaseRepository, IProcLabelTemplateRepository
    {
        public ProcLabelTemplateRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLabelTemplateEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLabelTemplateEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLabelTemplateEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLabelTemplateEntity>(GetByIdsSql, new { ids = ids });
        }

        public async Task<ProcLabelTemplateEntity> GetByNameAsync(ProcLabelTemplateByNameQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLabelTemplateEntity>(GetBynameSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLabelTemplatePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLabelTemplateEntity>> GetPagedInfoAsync(ProcLabelTemplatePagedQuery procLabelTemplatePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");


            if (!string.IsNullOrWhiteSpace(procLabelTemplatePagedQuery.Name))
            {
                procLabelTemplatePagedQuery.Name = $"%{procLabelTemplatePagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            var offSet = (procLabelTemplatePagedQuery.PageIndex - 1) * procLabelTemplatePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLabelTemplatePagedQuery.PageSize });
            sqlBuilder.AddParameters(procLabelTemplatePagedQuery);

            using var conn = GetMESDbConnection();
            var procLabelTemplateEntitiesTask = conn.QueryAsync<ProcLabelTemplateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLabelTemplateEntities = await procLabelTemplateEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLabelTemplateEntity>(procLabelTemplateEntities, procLabelTemplatePagedQuery.PageIndex, procLabelTemplatePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLabelTemplateQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLabelTemplateEntity>> GetProcLabelTemplateEntitiesAsync(ProcLabelTemplateQuery procLabelTemplateQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLabelTemplateEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procLabelTemplateEntities = await conn.QueryAsync<ProcLabelTemplateEntity>(template.RawSql, procLabelTemplateQuery);
            return procLabelTemplateEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLabelTemplateEntity procLabelTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procLabelTemplateEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcLabelTemplateEntity> procLabelTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procLabelTemplateEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLabelTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLabelTemplateEntity procLabelTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLabelTemplateEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLabelTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<ProcLabelTemplateEntity> procLabelTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procLabelTemplateEntitys);
        }

    }

    public partial class ProcLabelTemplateRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_label_template` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_label_template` /**where**/ ";
        const string GetProcLabelTemplateEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_label_template` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_label_template`(  `Id`, `Name`, `Path`, `Content`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Name, @Path, @Content, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `proc_label_template`(  `Id`, `Name`, `Path`, `Content`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Name, @Path, @Content, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `proc_label_template` SET   Name = @Name, Path = @Path, Content = @Content, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_label_template` SET   Name = @Name, Path = @Path, Content = @Content, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_label_template` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_label_template` SET IsDeleted = Id WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `Name`, `Path`, `Content`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `proc_label_template`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Name`, `Path`, `Content`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `proc_label_template`  WHERE Id IN @ids ";
        const string GetBynameSql = @"SELECT 
                               `Id`, `Name`, `Path`, `Content`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `proc_label_template`  WHERE Name = @Name and IsDeleted=0 and SiteId=@SiteId";
    }
}
