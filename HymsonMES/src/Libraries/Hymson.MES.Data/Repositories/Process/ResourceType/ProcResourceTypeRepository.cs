using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeView>> GetPageListAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            if (string.IsNullOrEmpty(procResourceTypePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("a.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procResourceTypePagedQuery.Sorting);
            }

            sqlBuilder.Where("a.SiteId = @SiteId");
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
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResCode))
            {
                procResourceTypePagedQuery.ResCode = $"%{procResourceTypePagedQuery.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResName))
            {
                procResourceTypePagedQuery.ResName = $"%{procResourceTypePagedQuery.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }

            var offSet = (procResourceTypePagedQuery.PageIndex - 1) * procResourceTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procResourceTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procResourceTypePagedQuery);

            using var conn = GetMESDbConnection();
            var procResourceTypeEntitiesTask = conn.QueryAsync<ProcResourceTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceTypeEntities = await procResourceTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceTypeView>(procResourceTypeEntities, procResourceTypePagedQuery.PageIndex, procResourceTypePagedQuery.PageSize, totalCount);
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
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }
    }

    public partial class ProcResourceTypeRepository
    {
        const string GetByIdSql = "select * from proc_resource_type where Id =@Id and IsDeleted =0 ";
        const string GetByCodeSql = "select * from proc_resource_type where SiteId =@SiteId and ResType =@ResType and IsDeleted =0 ";

        const string GetPagedInfoDataSqlTemplate = "SELECT a.*,b.ResCode,b.ResName  FROM proc_resource_type a left join proc_resource b on a.Id =b.ResTypeId and b.IsDeleted=0 /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT count(*) FROM proc_resource_type a left join proc_resource b on a.Id =b.ResTypeId  /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource_type /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_type`(`Id`, `SiteId`, `ResType`, `ResTypeName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResType, @ResTypeName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `proc_resource_type` SET ResTypeName = @ResTypeName, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource_type` SET `IsDeleted` = Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE `Id` in @Ids";
    }
}
