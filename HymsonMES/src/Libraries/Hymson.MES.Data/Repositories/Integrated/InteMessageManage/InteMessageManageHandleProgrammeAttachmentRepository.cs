using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（消息管理处理报告附件）
    /// </summary>
    public partial class InteMessageManageHandleProgrammeAttachmentRepository : BaseRepository, IInteMessageManageHandleProgrammeAttachmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteMessageManageHandleProgrammeAttachmentRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteMessageManageHandleProgrammeAttachmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteMessageManageHandleProgrammeAttachmentEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteMessageManageHandleProgrammeAttachmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteMessageManageHandleProgrammeAttachmentEntity> entities)
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
        public async Task<InteMessageManageHandleProgrammeAttachmentEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteMessageManageHandleProgrammeAttachmentEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageManageHandleProgrammeAttachmentEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageManageHandleProgrammeAttachmentEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageManageHandleProgrammeAttachmentEntity>> GetEntitiesAsync(InteMessageManageHandleProgrammeAttachmentQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageManageHandleProgrammeAttachmentEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteMessageManageHandleProgrammeAttachmentEntity>> GetPagedListAsync(InteMessageManageHandleProgrammeAttachmentPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<InteMessageManageHandleProgrammeAttachmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteMessageManageHandleProgrammeAttachmentEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteMessageManageHandleProgrammeAttachmentRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_message_manage_handle_programme_attachment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_message_manage_handle_programme_attachment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM inte_message_manage_handle_programme_attachment /**where**/  ";

        const string InsertSql = "INSERT INTO inte_message_manage_handle_programme_attachment(  `Id`, `MessageManageId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @MessageManageId, @AttachmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO inte_message_manage_handle_programme_attachment(  `Id`, `MessageManageId`, `AttachmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @MessageManageId, @AttachmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE inte_message_manage_handle_programme_attachment SET   MessageManageId = @MessageManageId, AttachmentId = @AttachmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_message_manage_handle_programme_attachment SET   MessageManageId = @MessageManageId, AttachmentId = @AttachmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_message_manage_handle_programme_attachment SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_message_manage_handle_programme_attachment SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_message_manage_handle_programme_attachment WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_message_manage_handle_programme_attachment WHERE Id IN @Ids ";

    }
}
