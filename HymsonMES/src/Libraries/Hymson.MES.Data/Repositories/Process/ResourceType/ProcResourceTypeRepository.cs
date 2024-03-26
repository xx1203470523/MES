using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Process.ResourceType.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源类型维护表仓储层处理
    /// @tableName proc_resource_type
    /// @author zhaoqing
    /// @date 2023-02-06
    /// </summary>
    public partial class ProcResourceTypeRepository : BaseRepository, IProcResourceTypeRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcResourceTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询资源类型是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ProcResourceTypeEntity> GetByCodeAsync(ProcResourceTypeEntity param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceTypeEntity>(GetByCodeSql, param);
        }

        /// <summary>
        ///  查询资源类型维护表列表(关联资源：一个类型被多个资源关联就展示多条)
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeView>> GetPageListNewAsync(ProcResourceTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("a.*,b.ResCode,b.ResName");
            sqlBuilder.LeftJoin("proc_resource b ON a.Id = b.ResTypeId AND b.IsDeleted = 0");
            sqlBuilder.Where("a.IsDeleted = 0");
            sqlBuilder.Where("a.SiteId = @SiteId");
            sqlBuilder.OrderBy("a.UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ResType))
            {
                pagedQuery.ResType = $"%{pagedQuery.ResType}%";
                sqlBuilder.Where("ResType LIKE @ResType");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ResTypeName))
            {
                pagedQuery.ResTypeName = $"%{pagedQuery.ResTypeName}%";
                sqlBuilder.Where("ResTypeName LIKE @ResTypeName");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ResCode))
            {
                pagedQuery.ResCode = $"%{pagedQuery.ResCode}%";
                sqlBuilder.Where("ResCode LIKE @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ResName))
            {
                pagedQuery.ResName = $"%{pagedQuery.ResName}%";
                sqlBuilder.Where("ResName LIKE @ResName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var procResourceTypeEntitiesTask = conn.QueryAsync<ProcResourceTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceTypeEntities = await procResourceTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceTypeView>(procResourceTypeEntities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取资源类型分页列表
        /// </summary>
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeEntity>> GetListAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResType))
            {
                procResourceTypePagedQuery.ResType = $"%{procResourceTypePagedQuery.ResType}%";
                sqlBuilder.Where("ResType like @ResType");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResTypeName))
            {
                procResourceTypePagedQuery.ResTypeName = $"%{procResourceTypePagedQuery.ResTypeName}%";
                sqlBuilder.Where("ResTypeName like @ResTypeName");
            }
            var offSet = (procResourceTypePagedQuery.PageIndex - 1) * procResourceTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procResourceTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procResourceTypePagedQuery);

            using var conn = GetMESDbConnection();
            var procResourceTypeEntitiesTask = conn.QueryAsync<ProcResourceTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceTypeEntities = await procResourceTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceTypeEntity>(procResourceTypeEntities, procResourceTypePagedQuery.PageIndex, procResourceTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="resourceTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceTypeAddCommand addCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, addCommand);
        }

        /// <summary>
        /// 更新资源类型维护数据
        /// </summary>
        /// <param name="updateCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceTypeUpdateCommand updateCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, updateCommand);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="resourceTypeEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcResourceTypeEntity> resourceTypeEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, resourceTypeEntities);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="resourceTypeEntities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<ProcResourceTypeEntity> resourceTypeEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, resourceTypeEntities);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="resourceTypeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceTypeEntity>> GetEntitiesAsync(ProcResourceTypeQuery resourceTypeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (resourceTypeQuery.ResTypes != null && resourceTypeQuery.ResTypes.Any())
            {
                sqlBuilder.Where(" ResType in @ResTypes ");
            }
            using var conn = GetMESDbConnection();
            var resourceTypeEntities = await conn.QueryAsync<ProcResourceTypeEntity>(template.RawSql, resourceTypeQuery);
            return resourceTypeEntities;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcResourceTypeRepository
    {
        const string GetByIdSql = "select * from proc_resource_type where Id =@Id and IsDeleted =0 ";
        const string GetByCodeSql = "select * from proc_resource_type where SiteId =@SiteId and ResType = @ResType and IsDeleted =0 ";
        const string GetEntitiesSqlTemplate = "select * from `proc_resource_type` /**where**/ ";

        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM proc_resource_type a /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset, @Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource_type a /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource_type /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_type`(`Id`, `SiteId`, `ResType`, `ResTypeName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResType, @ResTypeName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `proc_resource_type` SET ResTypeName = @ResTypeName, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource_type` SET `IsDeleted` = Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE `Id` in @Ids";
    }
}
