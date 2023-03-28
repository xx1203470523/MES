using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Resource;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源维护表仓储层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcResourceRepository : IProcResourceRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcResourceRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceView> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询某些资源类型下关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByResTypeIdsAsync(ProcResourceQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEntity>(GetByResTypeIdsSql, new { SiteId = query.SiteId, Ids = query.IdsArr });
        }

        /// <summary>
        /// 查询要删除的资源列表是否有启用状态的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByIdsAsync(ProcResourceQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEntity>(GetByIdsSql, new { Ids = query.IdsArr, Status = query.Status });
        }

        /// <summary>
        /// 根据资源Code查询数据
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByResourceCodeAsync(string resourceCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEntity>(GetByResourceCode, new { resourceCode });
        }

        /// <summary>
        /// 根据设备Code查询数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByEquipmentCodeAsync(string equipmentCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEntity>(GetByEquipmentCode, new { equipmentCode });
        }

        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(ProcResourceQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResource = await conn.QueryAsync<ProcResourceEntity>(ExistsSql, new { ResCode = query.ResCode, SiteId = query.SiteId });
            return procResource != null && procResource.Any();
        }

        /// <summary>
        /// 获取资源分页列表(关联资源类型)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceView>> GetPageListAsync(ProcResourcePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            sqlBuilder.Where("a.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }
            if (!string.IsNullOrWhiteSpace(query.ResType))
            {
                query.ResType = $"%{query.ResType}%";
                sqlBuilder.Where("ResType like @ResType");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceView>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEntity>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoJoinDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoJoinCountSqlTemplate);
            sqlBuilder.Where("r.IsDeleted = 0");
            sqlBuilder.Where("r.SiteId = @SiteId");
            sqlBuilder.OrderBy("r.UpdatedOn DESC");
            sqlBuilder.Select("r.*");
            sqlBuilder.LeftJoin("proc_resource_type t on r.ResTypeId = t.Id");
            sqlBuilder.LeftJoin("proc_procedure p on p.ResourceTypeId =t.Id");

            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("p.Id = @ProcedureId");
            }
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceEntity>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEntity>> GetListAsync(ProcResourcePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceEntity>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询资源维护表列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetListForGroupAsync(ProcResourcePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where($"SiteId = {query.SiteId}");
            sqlBuilder.Select("*");

            if (query.ResTypeId != null)
            {
                if (query.ResTypeId == 0)
                {
                    sqlBuilder.Where("ResTypeId=0");
                }
                else
                {
                    var resTypeId = query.ResTypeId.Value;
                    sqlBuilder.Where($" (ResTypeId=0 or ResTypeId={resTypeId})");
                }
            }

            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新资源维护数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateResTypeAsync(ProcResourceUpdateCommand entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateResTypeSql, new
            {
                ResTypeId = entity.ResTypeId,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                Ids = entity.IdsArr
            }); ;
        }

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> ResetResTypeAsync(ProcResourceUpdateCommand entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatedByResTypeSql, entity); ;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcResourceRepository
    {
        const string GetByIdSql = "SELECT a.*,b.ResType,b.ResType FROM proc_resource a left join proc_resource_type b on a.ResTypeId=b.Id and b.IsDeleted =0 where a.Id=@Id ";
        const string GetByResTypeIdsSql = "select * from proc_resource where SiteId=@SiteId and ResTypeId in @Ids and IsDeleted =0 ";
        const string GetByIdsSql = "select * from proc_resource where  Id  in @Ids and Status=@Status";
        const string GetByResourceCode = "SELECT Id, ResCode FROM proc_resource WHERE IsDeleted = 0 AND ResCode = @resourceCode";
        const string GetByEquipmentCode = "SELECT R.Id, R.ResCode FROM proc_resource_equipment_bind REB " +
            "LEFT JOIN equ_equipment E ON REB.EquipmentId = E.Id " +
            "LEFT JOIN proc_resource R ON REB.ResourceId = R.Id " +
            "WHERE E.IsDeleted = 0 AND R.IsDeleted = 0 AND E.EquipmentCode = @equipmentCode";

        const string ExistsSql = "SELECT Id FROM proc_resource WHERE `IsDeleted`= 0 AND ResCode=@ResCode and SiteId=@SiteId LIMIT 1";

        const string GetPagedInfoDataSqlTemplate = "SELECT a.*,b.ResType,b.ResTypeName  FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.Id and b.IsDeleted =0 /**where**/ ORDER BY a.UpdatedOn DESC LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT count(*) FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.Id  /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource /**where**/";

        const string GetPagedInfoJoinDataSqlTemplate = @"SELECT /**select**/ FROM proc_resource r /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoJoinCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource r /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetListSqlTemplate = "SELECT /**select**/ FROM proc_resource  /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_resource`(`Id`, `SiteId`, `ResCode`, `ResName`,`Status`,`ResTypeId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResCode, @ResName,@Status,@ResTypeId,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted); ";
        const string UpdateSql = "UPDATE `proc_resource` SET ResName = @ResName,ResTypeId = @ResTypeId,Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource` SET `IsDeleted` = Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE `Id` in @Ids;";

        const string UpdateResTypeSql = "UPDATE `proc_resource` SET ResTypeId = @ResTypeId,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id in @Ids;";
        const string UpdatedByResTypeSql = "UPDATE `proc_resource` SET ResTypeId =0,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE ResTypeId = @ResTypeId;";
    }
}
