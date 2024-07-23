using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>s
    /// 条码信息表仓储
    /// </summary>
    public partial class ManuSfcInfoRepository : BaseRepository, IManuSfcInfoRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSfcInfoRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuSfcInfoEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetBySFCIdWithIsUseAsync(long sfcId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetBySFCWithIsUseSql, new { SfcId = sfcId });
        }

        /// <summary>
        /// 根据SFCId获取数据
        /// </summary>
        /// <param name="sfcId"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetBySFCIdAsync(long sfcId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetBySFCIdSql, new { SFCId = sfcId });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetBySFCIdsWithIsUseAsync(IEnumerable<long> sfcIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetBySFCIdsWithIsUseSql, new { sfcIds });
        }

        /// <summary>
        /// 根据SFCIds获取数据
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetBySFCIdsAsync(IEnumerable<long> sfcIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetBySFCIdsSql, new { SFCIds = sfcIds });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfo1PagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInfoEntity>> GetPagedInfoAsync(ManuSfcInfo1PagedQuery manuSfcInfo1PagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            var offSet = (manuSfcInfo1PagedQuery.PageIndex - 1) * manuSfcInfo1PagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcInfo1PagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcInfo1PagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcInfo1EntitiesTask = conn.QueryAsync<ManuSfcInfoEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcInfo1Entities = await manuSfcInfo1EntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcInfoEntity>(manuSfcInfo1Entities, manuSfcInfo1PagedQuery.PageIndex, manuSfcInfo1PagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetManuSfcInfo1EntitiesAsync(ManuSfcInfo1Query manuSfcInfo1Query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfo1EntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcInfo1Entities = await conn.QueryAsync<ManuSfcInfoEntity>(template.RawSql, manuSfcInfo1Query);
            return manuSfcInfo1Entities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfoEntity ManuSfcInfoEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, ManuSfcInfoEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="ManuSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuSfcInfoEntity> ManuSfcInfoEntitys)
        {
            if (ManuSfcInfoEntitys == null || !ManuSfcInfoEntitys.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, ManuSfcInfoEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcInfoEntity ManuSfcInfoEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, ManuSfcInfoEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<ManuSfcInfoEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcInfoEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRangeSql, entities);
        }

        /// <summary>
        /// 批量更新 是否在用
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<int> UpdatesIsUsedAsync(ManuSfcInfoUpdateIsUsedCommand manuSfcInfoUpdateIsUsedCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesIsUsedSql, manuSfcInfoUpdateIsUsedCommand);
        }

        /// <summary>
        /// 马威获取工单数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WorkOrderQtyView>> GetWorkOrderQtyMavelAsync(WorkOrderQtyQuery query)
        {
            string sql = $@"
                select WorkOrderId ,count(*) Qty
                from manu_sfc_info mss 
                where SiteId  = {query.SiteId}
                and WorkOrderId in ({string.Join(",",query.OrderIdList)})
                group by WorkOrderId 
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WorkOrderQtyView>(sql);
        }
        #endregion

        /// <summary>
        /// 车间作业控制 报表分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkshopJobControlReportView>> GetPagedInfoWorkshopJobControlReportAsync(WorkshopJobControlReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoWorkshopJobControlReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoWorkshopJobControlReportCountSqlTemplate);

            sqlBuilder.Select(@"s.SFC,
                            s.`Status` as SFCStatus,
                            sp.`Status` as SFCProduceStatus,
                            CONCAT_WS('/',m.MaterialCode,m.Version) as MaterialCodeVersion,
                            m.MaterialName,
                            o.OrderCode,
                            o.Type as OrderType,
                            p.`Code` as ProcedureCode,
                            p.`Name` as ProcedureName,
                            CONCAT_WS('/',b.BomCode,b.Version) as BomCodeVersion,
                            b.BomName,
                            s.Qty ");
            //where si.IsDeleted = 0
            //AND si.IsUsed = 1

            //AND m.MaterialCode like '%%'
            //AND m.Version like '%%'
            //AND o.OrderCode like '%%'
            //AND s.SFC like '%%'
            //AND s.`Status` = ''

            //AND p.`Code` like '%%'
            //AND r.ResCode like '%%'

            sqlBuilder.Where(" si.SiteId = @SiteId ");
            sqlBuilder.Where(" si.IsDeleted = 0 ");
            sqlBuilder.Where(" si.IsUsed = 1 ");

            sqlBuilder.Where(" s.IsDeleted = 0 ");

            if (!string.IsNullOrEmpty(pageQuery.MaterialCode))
            {
                pageQuery.MaterialCode = $"%{pageQuery.MaterialCode}%";
                sqlBuilder.Where(" m.MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.MaterialVersion))
            {
                pageQuery.MaterialVersion = $"%{pageQuery.MaterialVersion}%";
                sqlBuilder.Where(" m.Version like @MaterialVersion ");
            }
            if (!string.IsNullOrEmpty(pageQuery.OrderCode))
            {
                pageQuery.OrderCode = $"%{pageQuery.OrderCode}%";
                sqlBuilder.Where(" o.OrderCode like @OrderCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.SFC))
            {
                pageQuery.SFC = $"%{pageQuery.SFC}%";
                sqlBuilder.Where(" s.SFC like @SFC ");
            }
            if (pageQuery.SFCStatus.HasValue)
            {
                sqlBuilder.Where(" s.`Status` =  @SFCStatus ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ProcedureCode))
            {
                pageQuery.ProcedureCode = $"%{pageQuery.ProcedureCode}%";
                sqlBuilder.Where(" p.`Code` like  @ProcedureCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ResourceCode))
            {
                pageQuery.ResourceCode = $"%{pageQuery.ResourceCode}%";
                sqlBuilder.Where(" r.ResCode like  @ResourceCode ");
            }

            if (pageQuery.SFCProduceStatus.HasValue)
            {
                sqlBuilder.Where(" sp.`Status` =  @SFCProduceStatus ");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var reportDataTask = conn.QueryAsync<WorkshopJobControlReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var reportData = await reportDataTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WorkshopJobControlReportView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 车间作业控制 报表分页查询
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkshopJobControlReportOptimizeView>> GetPagedInfoWorkshopJobControlReportOptimizeAsync(WorkshopJobControlReportOptimizePagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoWorkshopJobControlReportOptimizeDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoWorkshopJobControlReportOptimizeCountSqlTemplate);

            sqlBuilder.Where(" si.SiteId = @SiteId ");
            sqlBuilder.Where(" si.IsDeleted = 0 ");
            sqlBuilder.Where(" si.IsUsed = 1 ");

            if (pageQuery.MaterialId.HasValue)
            {
                sqlBuilder.Where(" si.ProductId = @MaterialId ");
            }
            if (pageQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where(" si.WorkOrderId = @WorkOrderId ");
            }
            if (!string.IsNullOrEmpty(pageQuery.SFC))
            {
                sqlBuilder.Where(" s.SFC = @SFC ");
            }
            if (pageQuery.SFCStatus.HasValue)
            {
                sqlBuilder.Where(" s.`Status` =  @SFCStatus ");
            }
            if (pageQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where(" sp.ProcedureId =  @ProcedureId ");
            }
            if (pageQuery.ResourceId.HasValue)
            {
                sqlBuilder.Where(" sp.ResourceId =  @ResourceId ");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var reportDataTask = conn.QueryAsync<WorkshopJobControlReportOptimizeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var reportData = await reportDataTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WorkshopJobControlReportOptimizeView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 根据SFC获取已经使用的
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetUsedBySFCAsync(string sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetUsedBySFC, new { sfc });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcInfoRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_info` /**where**/ ";
        const string GetManuSfcInfo1EntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `ProcessRouteId`,`ProductBOMId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @ProcessRouteId, @ProductBOMId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `ProcessRouteId`,`ProductBOMId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @ProcessRouteId, @ProductBOMId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_info` SET   SiteId = @SiteId, SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info` SET   SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, IsUsed = @IsUsed, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateRangeSql = "UPDATE manu_sfc_info SET WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id";

        const string DeleteSql = "UPDATE `manu_sfc_info` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_info` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND Id IN @ids ";
        const string GetBySFCIdSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SFCId = @SFCId ";
        const string GetBySFCWithIsUseSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SfcId = @SfcId  AND  IsUsed=1 ";
        const string GetBySFCIdsWithIsUseSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SfcId IN @sfcIds  AND  IsUsed=1";
        const string GetBySFCIdsSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SFCId IN @SFCIds";
        const string UpdatesIsUsedSql = "UPDATE `manu_sfc_info` SET  IsUsed =0 , UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE SfcId IN @SfcIds AND IsUsed=1";

        #endregion

        const string GetPagedInfoWorkshopJobControlReportDataSqlTemplate = @"
                        select 
                            /**select**/

                        from manu_sfc_info si
                        LEFT JOIN Manu_sfc s on s.id=si.SfcId -- 为了查询状态
                        LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                        LEFT join plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码
                        LEft join proc_bom b on b.id=o.ProductBOMId -- 为了查询bom

                        LEFT join manu_sfc_produce sp on sp.SFC=s.SFC and s.Status=1 -- 为了查询工序
                        LEFT JOIN proc_procedure p on p.Id=sp.ProcedureId -- 为了查询工序编码
                        LEFT join proc_resource r on r.id=sp.ResourceId  -- 为了查询资源
                        
                        /**leftjoin**/

                        /**where**/ 
                        Order by si.CreatedOn desc
                        LIMIT @Offset,@Rows 
        ";
        const string GetPagedInfoWorkshopJobControlReportCountSqlTemplate = @"
                        select count(1)
                        from manu_sfc_info si
                        LEFT JOIN Manu_sfc s on s.id=si.SfcId -- 为了查询状态
                        LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                        LEFT join plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码
                        LEft join proc_bom b on b.id=o.ProductBOMId -- 为了查询bom

                        LEFT join manu_sfc_produce sp on sp.SFC=s.SFC and s.Status=1 -- 为了查询工序
                        LEFT JOIN proc_procedure p on p.Id=sp.ProcedureId -- 为了查询工序编码
                        LEFT join proc_resource r on r.id=sp.ResourceId  -- 为了查询资源

                        /**leftjoin**/
                        /**where**/ 
        ";
        const string GetUsedBySFC = @"select si.* 
                                               from manu_sfc_info  si
                                               Left join manu_sfc s on s.id=si.SfcId
                                               where si.IsDeleted=0 and si.IsUsed=1 
                                               and s.sfc=@sfc ";

        const string GetPagedInfoWorkshopJobControlReportOptimizeDataSqlTemplate = @"
                        select 
                            s.SFC,
                            s.`Status` as SFCStatus,
                            si.ProductId,
                            si.WorkOrderId,
                            si.ProductBOMId,
                            si.ProcessRouteId,
                            sp.ProcedureId,
                            sp.ResourceId,
                            s.Qty
                        from manu_sfc_info si
                        LEFT JOIN Manu_sfc s on s.id=si.SfcId -- 为了查询状态
                        LEFT join manu_sfc_produce sp on sp.SFC=s.SFC 
                        /**where**/ 
                        Order by si.CreatedOn desc
                        LIMIT @Offset,@Rows 
        ";
        const string GetPagedInfoWorkshopJobControlReportOptimizeCountSqlTemplate = @"
                        select count(1)
                        from manu_sfc_info si
                        LEFT JOIN Manu_sfc s on s.id=si.SfcId -- 为了查询状态
                        LEFT join manu_sfc_produce sp on sp.SFC=s.SFC 
                        /**where**/ 
        ";

    }
}
