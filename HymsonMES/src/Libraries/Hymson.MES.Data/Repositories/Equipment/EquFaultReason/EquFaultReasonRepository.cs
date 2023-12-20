using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表仓储
    /// </summary>
    public partial class EquFaultReasonRepository : BaseRepository, IEquFaultReasonRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquFaultReasonRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equFaultReasonEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquFaultReasonEntity equFaultReasonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equFaultReasonEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equFaultReasonEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquFaultReasonEntity> equFaultReasonEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equFaultReasonEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equFaultReasonEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquFaultReasonEntity equFaultReasonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equFaultReasonEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equFaultReasonEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<EquFaultReasonEntity> equFaultReasonEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equFaultReasonEntitys);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
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
        public async Task<EquFaultReasonEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquFaultReasonEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultReasonEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquFaultReasonEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquFaultReasonEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquFaultReasonEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultReasonEntity>> GetPagedInfoAsync(EquFaultReasonPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquFaultReasonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquFaultReasonEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquFaultReasonQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultReasonEntity>> GetListAsync(EquFaultReasonQuery EquFaultReasonQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquFaultReasonEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (EquFaultReasonQuery.SiteId != 0)
            {
                sqlBuilder.Where(" SiteId=@SiteId ");
            }

            if (!string.IsNullOrWhiteSpace(EquFaultReasonQuery.Code))
            {
                sqlBuilder.Where(" Code = @Code ");
            }

            if (EquFaultReasonQuery.Ids != null && EquFaultReasonQuery.Ids.Any())
            {
                sqlBuilder.Where(" Id = @Ids ");
            }

            using var conn = GetMESDbConnection();
            var EquFaultReasonEntities = await conn.QueryAsync<EquFaultReasonEntity>(template.RawSql, EquFaultReasonQuery);
            return EquFaultReasonEntities;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquFaultReasonRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEquFaultReasonEntitiesSqlTemplate = @"SELECT /**select**/ FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_fault_reason`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_fault_reason`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        
        const string UpdateSql = "UPDATE `equ_fault_reason` SET Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_fault_reason` SET SiteId = @SiteId, Code = @Code, Name = @Name, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE equ_fault_reason SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = UpdatedOn WHERE Id = @Id; ";

        const string DeleteSql = "UPDATE `equ_fault_reason` SET IsDeleted = Id WHERE Id = @Id  ";
        const string DeletesSql = "UPDATE `equ_fault_reason` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `equ_fault_reason`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `equ_fault_reason`  WHERE Id IN @ids ";
        const string GetByCodeSql = "SELECT * FROM equ_fault_reason WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";

    }
}
