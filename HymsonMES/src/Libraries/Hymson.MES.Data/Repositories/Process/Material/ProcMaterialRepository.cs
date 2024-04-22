using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护仓储
    /// </summary>
    public partial class ProcMaterialRepository : BaseRepository, IProcMaterialRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcMaterialRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
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
        /// <param name="param"></param>
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
        public async Task<ProcMaterialView> GetByIdAsync(long id, long SiteId)
        {
            //var key = $"proc_material&proc_material_group&proc_process_route&proc_bom&{id}";
            //return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            //{
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ProcMaterialView>(GetViewByIdSql, new { Id = id, SiteId = SiteId });
            //});
        }

        /// <summary>
        ///  根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetByIdAsync(long id)
        {
            var key = $"proc_material&{id}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetMaterialByIdSql, new { Id = id });
            });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(long[] ids)
        {
            if (ids.Length <= 0)
            {
                return new List<ProcMaterialEntity>();
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 批量获取数据
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetBySiteIdAsync(long siteId)
        {
            var key = $"proc_material&all&{siteId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<ProcMaterialEntity>(GetBySiteIdSql, new { SiteId = siteId });
            });

        }

        /// <summary>
        /// 根据物料组ID查询物料
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetByGroupIdsAsync(long[] groupIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialEntity>(GetByGroupIdSql, new { groupIds = groupIds });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC, Id DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
            {
                procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
            {
                procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
                sqlBuilder.Where(" MaterialName like @MaterialName ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
            {
                procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            if (procMaterialPagedQuery.GroupId != null)
            {
                sqlBuilder.Where(" GroupId = @GroupId ");
            }
            if (procMaterialPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status = @Status ");
            }
            if (procMaterialPagedQuery.Origin.HasValue)
            {
                sqlBuilder.Where(" Origin = @Origin ");
            }
            if (procMaterialPagedQuery.BuyTypes != null && procMaterialPagedQuery.BuyTypes.Length > 0)
            {
                sqlBuilder.Where(" BuyType IN @BuyTypes ");
            }

            var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialEntities = await procMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoForGroupAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
            {
                procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
            {
                procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
                sqlBuilder.Where(" MaterialName like %@MaterialName% ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
            {
                procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            if (procMaterialPagedQuery.GroupId != null)
            {
                if (procMaterialPagedQuery.GroupId == 0)
                {
                    sqlBuilder.Where(" GroupId = 0 ");
                }
                else
                {
                    sqlBuilder.Where(" ( GroupId = 0 or GroupId =@GroupId ) ");
                }
            }
            if (procMaterialPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status = @Status ");
            }
            if (procMaterialPagedQuery.Origin.HasValue)
            {
                sqlBuilder.Where(" Origin = @Origin ");
            }

            var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialEntities = await procMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcMaterialEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(procMaterialQuery.MaterialCode))
            {
                procMaterialQuery.MaterialCode = $"%{procMaterialQuery.MaterialCode}%";
                sqlBuilder.Where(" MaterialCode LIKE @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialQuery.MaterialName))
            {
                procMaterialQuery.MaterialName = $"%{procMaterialQuery.MaterialName}%";
                sqlBuilder.Where(" MaterialName LIKE @MaterialName ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialQuery.Version))
            {
                procMaterialQuery.Version = $"%{procMaterialQuery.Version}%";
                sqlBuilder.Where(" Version LIKE @Version ");
            }
            if (procMaterialQuery.MaterialCodes != null && procMaterialQuery.MaterialCodes.Any())
            {
                sqlBuilder.Where(" MaterialCode in @MaterialCodes ");
            }
            sqlBuilder.AddParameters(procMaterialQuery);

            using var conn = GetMESDbConnection();
            var procMaterialEntities = await conn.QueryAsync<ProcMaterialEntity>(template.RawSql, template.Parameters);
            return procMaterialEntities;
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetByCodeAsync(ProcMaterialQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetByCodeAndVersionSql, query);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcMaterialEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            var key = $"proc_material&Site-{query.Site}&Code-{query.Code}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetByCodeSql, query);
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procMaterialEntity);  
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcMaterialEntity> procMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procMaterialEntitys);
        }

        /// <summary>
        /// 更新 同编码的其他物料设置为非当前版本
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateSameMaterialCodeToNoVersionAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSameMaterialCodeToNoVersionSql, procMaterialEntity);
        }

        /// <summary>
        /// 更新物料的物料组
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcMaterialGroupAsync(IEnumerable<ProcMaterialEntity> procMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProcMaterialGroupSql, procMaterialEntitys);
        }

        /// <summary>
        /// 更新某物料组下的物料为未绑定的
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcMaterialUnboundAsync(long groupId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProcMaterialUnboundSql, new { GroupId = groupId });
        }

        /// <summary>
        /// 更新某物料 的状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 根据编码获取物料信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialEntity>> GetByCodesAsync(ProcMaterialsByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialEntity>(GetByCodesSql, param);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcMaterialEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_material` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_material` /**where**/ ";
        const string GetProcMaterialEntitiesSqlTemplate = @"SELECT /**select**/ FROM `proc_material` /**where**/  ";
        const string GetByCodeAndVersionSql = "SELECT * FROM proc_material WHERE `IsDeleted` = 0 AND SiteId = @SiteId AND MaterialCode= @MaterialCode AND Version =@Version LIMIT 1";
        const string GetByCodeSql = "SELECT * FROM proc_material WHERE `IsDeleted` = 0 AND SiteId = @Site AND MaterialCode = @Code LIMIT 1";
        const string InsertSql = "INSERT INTO `proc_material`(`Id`, `SiteId`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `BomId`, `Batch`, PackageNum, `Unit`, `SerialNumber`, `BaseTime`, `ConsumptionTolerance`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `ConsumeRatio`,`MaskCodeId`, QuantityLimit, Specifications,ProductModel,MaterialType,ValidTime) VALUES (   @Id, @SiteId, @GroupId, @MaterialCode, @MaterialName, @Status, @Origin, @Version, @IsDefaultVersion, @Remark, @BuyType, @ProcessRouteId, @BomId, @Batch, @PackageNum, @Unit, @SerialNumber, @BaseTime, @ConsumptionTolerance, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @ConsumeRatio, @MaskCodeId, @QuantityLimit, @Specifications,@ProductModel,@MaterialType,@ValidTime)";
        const string UpdateSql = "UPDATE `proc_material` SET  GroupId = @GroupId, MaterialName = @MaterialName, Origin = @Origin, Version = @Version, Remark = @Remark, BuyType = @BuyType, ProcessRouteId = @ProcessRouteId, BomId = @BomId, Batch = @Batch, PackageNum = @PackageNum, Unit = @Unit, SerialNumber = @SerialNumber, BaseTime = @BaseTime, ConsumptionTolerance = @ConsumptionTolerance, IsDefaultVersion=@IsDefaultVersion, MaskCodeId=@MaskCodeId, QuantityLimit=@QuantityLimit, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn , ConsumeRatio=@ConsumeRatio,Specifications=@Specifications,ProductModel=@ProductModel,ValidTime = @ValidTime,MaterialType=@MaterialType  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetViewByIdSql = @"SELECT 
                                        g.`Id`,
                                        g.`SiteId`,
                                        o.GroupName,
                                        g.MaterialCode,
                                        g.MaterialName,
                                        g.Status, 
                                        g.Origin, 
                                        g.Version, 
                                        g.Remark, 
                                        g.BuyType, 
                                        p.Id AS ProcessRouteId, 
                                        p.Code As ProcessRouteCode,
                                        p.Name as ProcessRouteName,
                                        p.Version as ProcessRouteVersion, 
                                        q.Id as BomId, 
                                        q.BomCode as BomCode,
                                        q.BomName as BomName, 
                                        q.Version as BomVersion, 
                                        g.Batch as Batch, 
                                        g.PackageNum as PackageNum,
                                        g.Unit as Unit, 
                                        g.SerialNumber,
                                        g.BaseTime,
                                        g.ConsumptionTolerance,
                                        g.Specifications,
                                        g.ProductModel,
                                        g.MaterialType,
                                        g.IsDefaultVersion,
                                        g.UpdatedBy,
                                        g.UpdatedOn,
                                        g.ConsumeRatio,
                                        g.MaskCodeId,
                                        g.QuantityLimit,
                                        g.ValidTime
                            FROM `proc_material` g 
                            LEFT JOIN proc_material_group o on o.Id=g.GroupId
                            LEFT JOIN proc_process_route p on g.ProcessRouteId = p.Id
                            LEFT JOIN proc_bom q on g.BomId = q.Id 
                            WHERE g.Id = @Id and g.SiteId=@SiteId ";

        const string GetMaterialByIdSql = @"SELECT * FROM proc_material WHERE Id = @Id";
        const string GetByIdsSql = @"SELECT * FROM proc_material WHERE Id IN @ids AND IsDeleted = 0 ";
        const string GetBySiteIdSql = @"SELECT * FROM proc_material WHERE SiteId = @SiteId AND IsDeleted = 0 ";

        /// <summary>
        /// 根据物料组ID查询物料
        /// </summary>
        const string GetByGroupIdSql = @"SELECT * FROM proc_material WHERE IsDeleted = 0 AND GroupId IN @groupIds ";
        const string UpdateSameMaterialCodeToNoVersionSql = "UPDATE proc_material SET IsDefaultVersion = 0 WHERE MaterialCode = @MaterialCode AND IsDeleted = 0 ";

        /// <summary>
        /// 更改物料的物料组
        /// </summary>
        const string UpdateProcMaterialGroupSql = "UPDATE proc_material SET GroupId = @GroupId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        /// <summary>
        /// 更新某物料组下的物料为未绑定物料组
        /// </summary>
        const string UpdateProcMaterialUnboundSql = "UPDATE proc_material SET GroupId = 0 WHERE GroupId = @GroupId ";

        const string UpdateStatusSql = "UPDATE proc_material SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string GetByCodesSql = @"SELECT * FROM proc_material WHERE MaterialCode IN @MaterialCodes AND SiteId = @SiteId AND IsDeleted = 0 ";
    }
}
