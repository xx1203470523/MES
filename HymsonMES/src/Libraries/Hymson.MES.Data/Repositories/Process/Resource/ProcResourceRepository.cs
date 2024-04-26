using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源维护表仓储层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcResourceRepository : BaseRepository, IProcResourceRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcResourceRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceView> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetResByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetResByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity> > GetResByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetResByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            var key = $"{CachedTables.PROC_RESOURCE}&{query.Site}&{query.Code}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetByCodeSql, query);
            });
        }

        /// <summary>
        /// 查询某些资源类型下关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByResTypeIdsAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetByResTypeIdsSql, new { SiteId = query.SiteId, Ids = query.IdsArr });
        }

        /// <summary>
        /// 查询要删除的资源列表是否有启用状态的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByIdsAndStatusAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetByIdsAndStatusSql, new { Ids = query.IdsArr, Status = query.Status });
        }

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetListByIdsAsync( IEnumerable<long>   ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据资源Code查询资源数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetResourceByResourceCodeAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetResourceByResourceCode, query);
        }

        /// <summary>
        /// 根据资源Code查询数据
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetByResourceCodeAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetByResourceCode, new { ResCode = query.ResCode, SiteId = query.SiteId });
        }

        /// <summary>
        /// 根据设备Code查询数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByEquipmentCodeAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetByEquipmentCode, new { EquipmentCode = query.EquipmentCode, SiteId = query.SiteId });
        }

        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(ProcResourceQuery query)
        {
            using var conn = GetMESDbConnection();
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
            if (string.IsNullOrEmpty(query.Sorting))
            {
                sqlBuilder.OrderBy("a.UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(query.Sorting);
            }

            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("a.ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("a.ResName like @ResName");
            }
            if (!string.IsNullOrWhiteSpace(query.ResType))
            {
                query.ResType = $"%{query.ResType}%";
                sqlBuilder.Where("b.ResType like @ResType");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("a.Status = @Status");
            }
            if (query.ResTypeId.HasValue && query.ResTypeId > 0)
            {
                sqlBuilder.Where(" a.ResTypeId=@ResTypeId");
            }

            // 这个是为了查询指定线体下的资源，大部分情况下这个 WorkCenterLineId 是不会有值的
            if (query.WorkCenterLineId.HasValue)
            {
                sqlBuilder.Where("EXISTS (SELECT IWCRR.ResourceId FROM inte_work_center_resource_relation IWCRR WHERE IWCRR.IsDeleted = 0 AND IWCRR.WorkCenterId = @WorkCenterLineId AND IWCRR.ResourceId = a.Id)");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceView>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 更具线体和工序查询资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEntity>> GetPageListBylineIdAndProcProcedureIdAsync(ProcResourcePagedlineIdAndProcProcedureIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Select("proc_resource.*");
            sqlBuilder.LeftJoin("inte_work_center_resource_relation  IWCRR ON IWCRR.IsDeleted = 0 AND  IWCRR.ResourceId = proc_resource.Id");
            sqlBuilder.Where("proc_resource.IsDeleted=0");
            sqlBuilder.Where("proc_resource.Status=1");
            sqlBuilder.Where("proc_resource.ResTypeId=@ResTypeId");
            if (string.IsNullOrEmpty(query.Sorting))
            {
                sqlBuilder.OrderBy("proc_resource.UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(query.Sorting);
            }

            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("proc_resource.ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("proc_resource.ResName like @ResName");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceView>(templateData.RawSql, templateData.Parameters);
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
        public async Task<PagedInfo<ProcResourceView>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoJoinDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoJoinCountSqlTemplate);
            sqlBuilder.Where("r.IsDeleted = 0");
            sqlBuilder.Where("r.SiteId = @SiteId");
            sqlBuilder.OrderBy("r.UpdatedOn DESC");
            sqlBuilder.Select("r.*,t.ResType,t.ResTypeName");
            sqlBuilder.LeftJoin("proc_resource_type t on r.ResTypeId = t.Id and t.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure p on p.ResourceTypeId =t.Id and p.IsDeleted=0");

            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("p.Id = @ProcedureId");
            }
            if (!string.IsNullOrWhiteSpace(query.ProcedureCode))
            {
                sqlBuilder.Where("p.code = @ProcedureCode");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("r.Status = @Status");
            }
            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("r.ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("r.ResName like @ResName");
            }
            if (!string.IsNullOrWhiteSpace(query.ResType))
            {
                query.ResName = $"%{query.ResType}%";
                sqlBuilder.Where("t.ResType like @ResType");
            }
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
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

            using var conn = GetMESDbConnection();
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

            if (query.Status.HasValue)
            {
                sqlBuilder.Where(" status=@Status ");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 查询工序关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetProcResourceListByProcedureIdAsync(long procedureId)
        {
            var key = $"{CachedTables.PROC_RESOURCE}&{CachedTables.PROC_PROCEDURE}&{procedureId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<ProcResourceEntity>(GetProcResourceListByProcedureIdSql, new { ProcedureId = procedureId });
            });
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="resourceEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcResourceEntity> resourceEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, resourceEntities);
        }

        /// <summary>
        /// 更新资源维护数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procResourceEntities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<ProcResourceEntity> procResourceEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procResourceEntities);
        }

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateResTypeAsync(ProcResourceUpdateCommand entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateResTypeSql, new
            {
                ResTypeId = entity.ResTypeId,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                Ids = entity.IdsArr
            });
        }

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> ResetResTypeAsync(ProcResourceUpdateCommand entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatedByResTypeSql, entity);
        }

        /// <summary>
        /// 清空资源的资源类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ClearResourceTypeIdsAsync(ClearResourceTypeIdsCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ClearResourceTypeIds, command);
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
        /// 根据设备Id查询关联的资源数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetByEquipmentIdsAsync(ProcResourceByEquipmentIdsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceEntity>(GetByEquipmentIdsSql, new { EquipmentIds = query.EquipmentIds, SiteId = query.SiteId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procResourceQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetProcResouceEntitiesAsync(ProcResourceQuery procResourceQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcResouceEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (procResourceQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status= @Status ");
            }
            if (procResourceQuery.ResTypeId.HasValue)
            {
                sqlBuilder.Where(" ResTypeId= @ResTypeId ");
            }

            sqlBuilder.AddParameters(procResourceQuery);
            using var conn = GetMESDbConnection();
            var procResources = await conn.QueryAsync<ProcResourceEntity>(template.RawSql, procResourceQuery);
            return procResources;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="procResourceQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEntity>> GetEntitiesAsync(ProcResourceQuery procResourceQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (procResourceQuery.ResCodes != null && procResourceQuery.ResCodes.Any())
            {
                sqlBuilder.Where(" ResCode in @ResCodes ");
            }
            using var conn = GetMESDbConnection();
            var procResourceEntities = await conn.QueryAsync<ProcResourceEntity>(template.RawSql, procResourceQuery);
            return procResourceEntities;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcResourceRepository
    {
        const string GetByIdSql = "SELECT a.*,b.ResType,b.ResType FROM proc_resource a left join proc_resource_type b on a.ResTypeId=b.Id and b.IsDeleted =0 where a.Id=@Id ";
        const string GetByResTypeIdsSql = "select * from proc_resource where SiteId=@SiteId and ResTypeId in @Ids and IsDeleted =0 ";
        const string GetByIdsAndStatusSql = "select * from proc_resource where  Id  in @Ids and Status=@Status";
        const string GetByIdsSql = "select * from proc_resource  WHERE Id IN @ids and IsDeleted=0";
        const string GetByCodeSql = "SELECT * FROM proc_resource WHERE `IsDeleted` = 0 AND SiteId = @Site AND ResCode = @Code LIMIT 1";
        const string GetByResourceCode = "SELECT Id, ResCode FROM proc_resource WHERE IsDeleted = 0 AND ResCode = @ResCode and SiteId =@SiteId ";
        const string GetByEquipmentCode = @"SELECT R.Id, R.ResCode FROM proc_resource_equipment_bind REB 
            LEFT JOIN equ_equipment E ON REB.EquipmentId = E.Id
            LEFT JOIN proc_resource R ON REB.ResourceId = R.Id
            WHERE E.IsDeleted = 0 AND R.IsDeleted = 0 AND E.EquipmentCode = @EquipmentCode AND E.SiteId =@SiteId";
        const string GetResByIdSql = "select * from proc_resource where  Id  =@Id";
        const string GetResByIdsSql = "select * from proc_resource where  Id  IN @Ids";
        const string ExistsSql = "SELECT Id FROM proc_resource WHERE `IsDeleted`= 0 AND ResCode=@ResCode and SiteId=@SiteId LIMIT 1";

        const string GetPagedInfoDataSqlTemplate = "SELECT a.*,b.ResType,b.ResTypeName  FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.Id and b.IsDeleted =0 /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT count(*) FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.Id  /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM  proc_resource /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetPagedInfoJoinDataSqlTemplate = @"SELECT /**select**/ FROM proc_resource r /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoJoinCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource r /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetListSqlTemplate = "SELECT /**select**/ FROM proc_resource  /**where**/  ";
        const string GetProcResourceListByProcedureIdSql = "SELECT R.* FROM proc_resource R INNER JOIN proc_procedure P ON R.ResTypeId = P.ResourceTypeId " +
            "WHERE R.IsDeleted = 0 AND P.IsDeleted = 0  AND P.Id = @ProcedureId ";

        const string InsertSql = "INSERT INTO `proc_resource`(`Id`, `SiteId`, `ResCode`, `ResName`,`Status`,`ResTypeId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResCode, @ResName,@Status,@ResTypeId,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted); ";
        const string UpdateSql = "UPDATE `proc_resource` SET ResName = @ResName,ResTypeId = @ResTypeId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource` SET `IsDeleted` = Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE `Id` in @Ids;";

        const string UpdateResTypeSql = "UPDATE `proc_resource` SET ResTypeId = @ResTypeId,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id in @Ids;";
        const string UpdatedByResTypeSql = "UPDATE `proc_resource` SET ResTypeId =0,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE ResTypeId = @ResTypeId;";
        const string ClearResourceTypeIds = "UPDATE proc_resource SET ResTypeId = 0, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE ResTypeId IN @ResourceTypeIds; ";

        const string GetResourceByResourceCode = "SELECT Id, ResCode FROM proc_resource WHERE IsDeleted = 0 AND Status=@Status AND ResCode = @ResCode and SiteId =@SiteId ";

        const string GetByEquipmentIdsSql = @"SELECT R.* 
            FROM proc_resource_equipment_bind REB 
            LEFT JOIN proc_resource R ON REB.ResourceId = R.Id
            WHERE R.IsDeleted = 0 
            AND REB.EquipmentId IN @EquipmentIds 
            AND REB.SiteId = @SiteId ";

        const string GetProcResouceEntitiesSqlTemplate = @"SELECT  /**select**/ FROM `proc_resource` /**where**/  ";

        const string UpdateStatusSql = "UPDATE `proc_resource` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        const string GetEntitiesSqlTemplate = "SELECT * FROM `proc_resource` /**where**/ ";
    }
}
