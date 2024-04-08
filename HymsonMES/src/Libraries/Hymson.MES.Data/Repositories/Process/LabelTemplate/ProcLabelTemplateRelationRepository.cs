using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准模板打印配置信息仓储
    /// </summary>
    public partial class ProcLabelTemplateRelationRepository :BaseRepository, IProcLabelTemplateRelationRepository
    {

        public ProcLabelTemplateRelationRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcLabelTemplateRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLabelTemplateRelationEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLabelTemplateRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLabelTemplateRelationEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLabelTemplateRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLabelTemplateRelationEntity>> GetPagedInfoAsync(ProcLabelTemplateRelationPagedQuery procLabelTemplateRelationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (procLabelTemplateRelationPagedQuery.PageIndex - 1) * procLabelTemplateRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLabelTemplateRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(procLabelTemplateRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var procLabelTemplateRelationEntitiesTask = conn.QueryAsync<ProcLabelTemplateRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLabelTemplateRelationEntities = await procLabelTemplateRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLabelTemplateRelationEntity>(procLabelTemplateRelationEntities, procLabelTemplateRelationPagedQuery.PageIndex, procLabelTemplateRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLabelTemplateRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLabelTemplateRelationEntity>> GetProcLabelTemplateRelationEntitiesAsync(ProcLabelTemplateRelationQuery procLabelTemplateRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLabelTemplateRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procLabelTemplateRelationEntities = await conn.QueryAsync<ProcLabelTemplateRelationEntity>(template.RawSql, procLabelTemplateRelationQuery);
            return procLabelTemplateRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLabelTemplateRelationEntity procLabelTemplateRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procLabelTemplateRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLabelTemplateRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLabelTemplateRelationEntity> procLabelTemplateRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procLabelTemplateRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLabelTemplateRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLabelTemplateRelationEntity procLabelTemplateRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLabelTemplateRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLabelTemplateRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLabelTemplateRelationEntity> procLabelTemplateRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procLabelTemplateRelationEntitys);
        }

        /// <summary>
        /// 根据labelTemplateId获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLabelTemplateRelationEntity> GetByLabelTemplateIdAsync(long labelTemplateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLabelTemplateRelationEntity>(GetByLabelTemplateIdSql, new { LabelTemplateId = labelTemplateId });
        }

        /// <summary>
        /// 根据labelTemplateId硬删除对应数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByLabelTemplateIdAsync(long labelTemplateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByLabelTemplateIdSql, new { LabelTemplateId = labelTemplateId });
        }
        #endregion

    }

    public partial class ProcLabelTemplateRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_label_template_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_label_template_relation` /**where**/ ";
        const string GetProcLabelTemplateRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_label_template_relation` /**where**/  "; 

        const string InsertSql = "INSERT INTO `proc_label_template_relation`(  `Id`, `LabelTemplateId`, `Remark`,`PrintDataModel`,`CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `PrintConfig`, `PrintTemplatePath`) VALUES (   @Id, @LabelTemplateId, @Remark,@PrintDataModel, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @PrintConfig, @PrintTemplatePath )  ";
        const string InsertsSql = "INSERT INTO `proc_label_template_relation`(  `Id`, `LabelTemplateId`, `Remark`, `PrintDataModel``CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `PrintConfig`, `PrintTemplatePath`) VALUES (   @Id, @LabelTemplateId, @Remark,@PrintDataModel, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @PrintConfig, @PrintTemplatePath )  ";

        const string UpdateSql = "UPDATE `proc_label_template_relation` SET   LabelTemplateId = @LabelTemplateId, Remark = @Remark,PrintDataModel=@PrintDataModel, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, PrintConfig = @PrintConfig, PrintTemplatePath = @PrintTemplatePath  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_label_template_relation` SET   LabelTemplateId = @LabelTemplateId, Remark = @Remark, PrintDataModel=@PrintDataModel,sCreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, PrintConfig = @PrintConfig, PrintTemplatePath = @PrintTemplatePath  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_label_template_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_label_template_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `LabelTemplateId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `PrintConfig`, `PrintTemplatePath`
                            FROM `proc_label_template_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `LabelTemplateId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `PrintConfig`, `PrintTemplatePath`
                            FROM `proc_label_template_relation`  WHERE Id IN @Ids ";

        const string GetByLabelTemplateIdSql = @"SELECT 
                               *
                            FROM `proc_label_template_relation`  WHERE LabelTemplateId = @LabelTemplateId ";
        const string DeleteByLabelTemplateIdSql = @"DELETE FROM `proc_label_template_relation` WHERE LabelTemplateId = @LabelTemplateId ";
        #endregion
    }
}
