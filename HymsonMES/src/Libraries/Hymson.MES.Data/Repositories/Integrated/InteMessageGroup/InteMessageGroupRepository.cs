using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（消息组）
    /// </summary>
    public partial class InteMessageGroupRepository : BaseRepository, IInteMessageGroupRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public InteMessageGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteMessageGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteMessageGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteMessageGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteMessageGroupEntity> entities)
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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteMessageGroupEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteMessageGroupEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteMessageGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteMessageGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageGroupEntity>> GetEntitiesAsync(EntityBySiteIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteMessageGroupView>> GetPagedListAsync(InteMessageGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("inte_work_center IWC ON IWC.Id = T.WorkShopId");
            sqlBuilder.Select("T.*, IWC.Name AS WorkShopName");
            sqlBuilder.OrderBy("T.UpdatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("T.Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("T.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("T.Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkShopName))
            {
                pagedQuery.WorkShopName = $"%{pagedQuery.WorkShopName}%";
                sqlBuilder.Where("IWC.Name LIKE @WorkShopName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<InteMessageGroupView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteMessageGroupView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteMessageGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_message_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_message_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/  FROM inte_message_group /**where**/  ";
#if DM
        const string InsertSql = "INSERT  inte_message_group(`Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT  inte_message_group(`Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
#else
const string InsertSql = "INSERT IGNORE inte_message_group(`Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT IGNORE inte_message_group(`Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

#endif
        const string UpdateSql = "UPDATE inte_message_group SET SiteId = @SiteId, Code = @Code, Name = @Name, WorkShopId = @WorkShopId, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_message_group SET SiteId = @SiteId, Code = @Code, Name = @Name, WorkShopId = @WorkShopId, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_message_group SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_message_group SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByCodeSql = "SELECT * FROM inte_message_group WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetByIdSql = @"SELECT * FROM inte_message_group WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_message_group WHERE Id IN @Ids ";

    }
}
