using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码流转表仓储
    /// </summary>
    public partial class ManuSfcCirculationRepository : BaseRepository, IManuSfcCirculationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcCirculationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcCirculationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcCirculationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据Location查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetByLocationAsync(LocationQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(GetByLocationSql, query);
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                sqlBuilder.Where("SFC = @Sfc");
            }

            if (query.CirculationTypes != null && query.CirculationTypes.Any())
            {
                sqlBuilder.Where("CirculationType IN @CirculationTypes");
            }

            if (query.IsDisassemble.HasValue)
            {
                sqlBuilder.Where("IsDisassemble = @IsDisassemble");
            }

            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId = @ProcedureId");
            }
            if (query.CirculationMainProductId.HasValue)
            {
                sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 根据SFCs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>  
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationBySfcsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (query.Sfc != null && query.Sfc.Any())
            {
                sqlBuilder.Where("SFC IN @Sfc");
            }

            if (!string.IsNullOrWhiteSpace(query.CirculationBarCode))
            {
                sqlBuilder.Where("CirculationBarCode = @CirculationBarCode");
            }

            if (query.CirculationBarCodes != null && query.CirculationBarCodes.Any())
            {
                sqlBuilder.Where("CirculationBarCode IN @CirculationBarCodes");
            }

            if (query.CirculationTypes != null && query.CirculationTypes.Length > 0)
            {
                sqlBuilder.Where("CirculationType IN @CirculationTypes");
            }

            if (query.IsDisassemble.HasValue)
            {
                sqlBuilder.Where("IsDisassemble = @IsDisassemble");
            }

            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId = @ProcedureId");
            }
            if (query.CirculationMainProductId.HasValue)
            {
                sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(GetListByStartWaterMarkIdSql, query);
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetListByStartWaterMarkTimeAsync(EntityByWaterMarkTimeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCirculationEntity>(GetListByStartWaterMarkTimeSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcCirculationEntity>> GetPagedInfoAsync(ManuSfcCirculationPagedQuery manuSfcCirculationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (manuSfcCirculationPagedQuery.PageIndex - 1) * manuSfcCirculationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcCirculationPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcCirculationPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, manuSfcCirculationPagedQuery.PageIndex, manuSfcCirculationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcCirculationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationEntitiesAsync(ManuSfcCirculationQuery manuSfcCirculationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");
            if (!string.IsNullOrEmpty(manuSfcCirculationQuery.Sfc))
            {
                sqlBuilder.Where("SFC = @Sfc");
            }

            if (manuSfcCirculationQuery.CirculationTypes != null && manuSfcCirculationQuery.CirculationTypes.Any())
            {
                sqlBuilder.Where("CirculationType IN @CirculationTypes");
            }

        

            if (manuSfcCirculationQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId = @ProcedureId");
            }
            sqlBuilder.AddParameters(manuSfcCirculationQuery);
            using var conn = GetMESDbConnection();
            var manuSfcCirculationEntities = await conn.QueryAsync<ManuSfcCirculationEntity>(template.RawSql, manuSfcCirculationQuery);
            return manuSfcCirculationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcCirculationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcCirculationEntity>? manuSfcCirculationEntitys)
        {
            if (manuSfcCirculationEntitys == null || !manuSfcCirculationEntitys.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcCirculationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 在制品拆解移除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DisassemblyUpdateAsync(DisassemblyCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DisassemblyUpdateSql, command);
        }


        /// <summary>
        /// 组件使用报告 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcCirculationEntity>> GetReportPagedInfoAsync(ComUsageReportPagedQuery queryParam)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetReportPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetReportPagedInfoCountSqlTemplate);
            sqlBuilder.Where(" IsDeleted=0 ");
            sqlBuilder.Select("*");

            sqlBuilder.Where(" IsDisassemble=0 "); //筛选出未拆解的

            sqlBuilder.Where(" SiteId=@SiteId ");

            //where sc.IsDeleted = 0
            //    and sc.SiteId = ''
            //    and sc.CirculationProductId = ''
            //    and sc.CreatedOn BETWEEN '' and ''-- 查询 时间
            //    and sc.CirculationBarCode like '%%'-- 查询组件车间作业 / 库存批次 
            //    -- and-- 查询 供应商编码
            //    and sc.ProcedureId = ''
            //    and sc.ResourceId-- 查询资源

            if (queryParam.CirculationProductId.HasValue)
            {
                sqlBuilder.Where(" CirculationProductId=@CirculationProductId ");
            }

            if (queryParam.CreatedOn != null && queryParam.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = queryParam.CreatedOn[0], CreatedOnEnd = queryParam.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where(" CreatedOn >= @CreatedOnStart AND CreatedOn < @CreatedOnEnd ");
            }

            if (!string.IsNullOrEmpty(queryParam.CirculationBarCode))
            {
                sqlBuilder.Where(" CirculationBarCode=@CirculationBarCode ");
            }

            if (queryParam.ProcedureId.HasValue)
            {
                sqlBuilder.Where(" ProcedureId=@ProcedureId ");
            }

            if (queryParam.ResourceId.HasValue)
            {
                sqlBuilder.Where(" ResourceId=@ResourceId ");
            }

            if (queryParam.CirculationMainSupplierId.HasValue)
            {
                sqlBuilder.Where(" CirculationMainSupplierId=@CirculationMainSupplierId ");
            }

            var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
            sqlBuilder.AddParameters(queryParam);

            using var conn = GetMESDbConnection();
            var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcCirculationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_circulation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_circulation` /**where**/ ";
        const string GetManuSfcCirculationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_circulation` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_circulation`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`,`Location`, `CirculationProductId`,`CirculationMainProductId`, `CirculationQty`, `CirculationType`,  `IsDisassemble`,`CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @FeedingPointId, @SFC, @WorkOrderId, @ProductId, @CirculationBarCode, @CirculationWorkOrderId,@Location,@CirculationProductId,@CirculationMainProductId,@CirculationQty, @CirculationType, @IsDisassemble,@CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        
        const string UpdateSql = "UPDATE `manu_sfc_circulation` SET ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, FeedingPointId = @FeedingPointId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, CirculationBarCode = @CirculationBarCode, CirculationWorkOrderId = @CirculationWorkOrderId, CirculationProductId = @CirculationProductId, CirculationMainProductId =@CirculationMainProductId,  CirculationQty=@CirculationQty,  CirculationType = @CirculationType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_circulation` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";

        const string GetByLocationSql = @"SELECT * FROM manu_sfc_circulation WHERE SiteId = @SiteId AND SFC = @SFC AND Location = @Location ";
        const string GetByIdSql = @"SELECT * FROM `manu_sfc_circulation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc_circulation`  WHERE Id IN @ids ";
        const string GetListByStartWaterMarkIdSql = @"SELECT * FROM `manu_sfc_circulation` WHERE Id > @StartWaterMarkId ORDER BY Id ASC LIMIT @Rows";
        const string GetListByStartWaterMarkTimeSql = @"SELECT * FROM `manu_sfc_circulation` WHERE UpdatedOn > @StartWaterMarkTime ORDER BY UpdatedOn ASC LIMIT @Rows";

        const string DisassemblyUpdateSql = "UPDATE manu_sfc_circulation SET " +
            "CirculationType = @CirculationType, IsDisassemble = @IsDisassemble," +
            "DisassembledBy = @UserId, DisassembledOn = @UpdatedOn, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE Id = @Id AND IsDisassemble <> @IsDisassemble ";

        const string GetReportPagedInfoDataSqlTemplate = @"
                SELECT 
                    /**select**/ 
                FROM `manu_sfc_circulation` 
                /**innerjoin**/ 
                /**leftjoin**/ 
                /**where**/ 
                LIMIT @Offset,@Rows 
                ";
        const string GetReportPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_circulation` /**where**/ ";
    }
}
