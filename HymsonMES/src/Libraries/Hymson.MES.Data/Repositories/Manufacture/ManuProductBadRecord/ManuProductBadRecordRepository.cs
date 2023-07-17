using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产品不良录入仓储
    /// </summary>
    public partial class ManuProductBadRecordRepository : IManuProductBadRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuProductBadRecordRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductBadRecordEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuProductBadRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordView>> GetBadRecordsBySfcAsync(ManuProductBadRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("br.SiteId = @SiteId");
            sqlBuilder.Where("br.IsDeleted =0");

            sqlBuilder.Select("br.Id,br.Status,uc.UnqualifiedCode,uc.UnqualifiedCodeName,br.UnqualifiedId,pr.ResCode,pr.ResName,uc.ProcessRouteId,uc.Type");
            sqlBuilder.LeftJoin("qual_unqualified_code uc on br.UnqualifiedId =uc.Id");
            sqlBuilder.LeftJoin("proc_resource pr on pr.Id =br.FoundBadResourceId");

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where("br.SFC=@SFC");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("br.Status =@Status");
            }
            if (query.Type.HasValue)
            {
                sqlBuilder.Where("uc.Type =@Type");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntities = await conn.QueryAsync<ManuProductBadRecordView>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuProductBadRecordEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuProductBadRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordEntity>> GetPagedInfoAsync(ManuProductBadRecordPagedQuery manuProductBadRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (manuProductBadRecordPagedQuery.PageIndex - 1) * manuProductBadRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuProductBadRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuProductBadRecordPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntitiesTask = conn.QueryAsync<ManuProductBadRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuProductBadRecordEntities = await manuProductBadRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductBadRecordEntity>(manuProductBadRecordEntities, manuProductBadRecordPagedQuery.PageIndex, manuProductBadRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuProductBadRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordEntity>> GetManuProductBadRecordEntitiesBySFCAsync(ManuProductBadRecordBySfcQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("br.*");
            sqlBuilder.Where("br.SiteId = @SiteId");
            sqlBuilder.Where("br.IsDeleted =0");
            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }
            if (query.Sfcs != null && query.Sfcs.Any())
            {
                sqlBuilder.Where("SFC in @Sfcs");
            }
            var manuProductBadRecordEntities = await conn.QueryAsync<ManuProductBadRecordEntity>(template.RawSql, query);
            return manuProductBadRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuProductBadRecordEntity manuProductBadRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuProductBadRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuProductBadRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuProductBadRecordEntity manuProductBadRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuProductBadRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuProductBadRecordEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 关闭条码不合格标识和缺陷
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusRangeAsync(List<ManuProductBadRecordCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusSql, commands);
        }

        /// <summary>
        /// 根据ID关闭条码不合格标识和缺陷
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusByIdRangeAsync(List<ManuProductBadRecordUpdateCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusByIdSql, commands);
        }

        /// <summary>
        /// 报表分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordReportView>> GetPagedInfoReportAsync(ManuProductBadRecordReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoReportCountSqlTemplate);

            //where rbr.IsDeleted = 0
            //        AND m.MaterialCode = ''
            //        AND m.Version = ''
            //        AND o.OrderCode = ''
            //        AND rbr.SFC = ''
            //        AND p.`Code`= ''
            //        AND rbr.CreatedOn BETWEEN '' and ''

            sqlBuilder.Where(" rbr.IsDeleted = 0 ");
            sqlBuilder.Where(" rbr.SiteId=@SiteId ");

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
                sqlBuilder.Where(" rbr.SFC like @SFC ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ProcedureCode))
            {
                pageQuery.ProcedureCode = $"%{pageQuery.ProcedureCode}%";
                sqlBuilder.Where(" p.`Code` like  @ProcedureCode ");
            }
            if (pageQuery.CreatedOn != null && pageQuery.CreatedOn.Length >= 2)
            {
                    sqlBuilder.AddParameters(new { CreatedOnStart = pageQuery.CreatedOn[0], CreatedOnEnd = pageQuery.CreatedOn[1].AddDays(1) });
                    sqlBuilder.Where(" rbr.CreatedOn >= @CreatedOnStart rbr.CreatedOn < @CreatedOnEnd ");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntitiesTask = conn.QueryAsync<ManuProductBadRecordReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuProductBadRecordEntities = await manuProductBadRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductBadRecordReportView>(manuProductBadRecordEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 获取 前多少的不良记录
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordReportView>> GetTopNumReportAsync(ManuProductBadRecordReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoReportDataSqlTemplate);

            sqlBuilder.Where(" rbr.IsDeleted = 0 ");
            sqlBuilder.Where(" rbr.SiteId=@SiteId ");

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
                sqlBuilder.Where(" rbr.SFC like @SFC ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ProcedureCode))
            {
                pageQuery.ProcedureCode = $"%{pageQuery.ProcedureCode}%";
                sqlBuilder.Where(" p.`Code` like  @ProcedureCode ");
            }
            //if (pageQuery.CreatedOnS.HasValue || pageQuery.CreatedOnE.HasValue)
            //{
            //    if (pageQuery.CreatedOnS.HasValue && pageQuery.CreatedOnE.HasValue)
            //        sqlBuilder.Where(" rbr.CreatedOn BETWEEN @CreatedOnS AND @CreatedOnE ");
            //    else
            //    {
            //        if (pageQuery.CreatedOnS.HasValue) sqlBuilder.Where("rbr.CreatedOn >= @CreatedOnS");
            //        if (pageQuery.CreatedOnE.HasValue) sqlBuilder.Where("rbr.CreatedOn < @CreatedOnE");
            //    }
            //}
            if (pageQuery.CreatedOn != null && pageQuery.CreatedOn.Length > 0)
            {
                if (pageQuery.CreatedOn.Length >= 2)
                {
                    sqlBuilder.AddParameters(new { CreatedOnStart = pageQuery.CreatedOn[0], CreatedOnEnd = pageQuery.CreatedOn[1].AddDays(1) });
                    sqlBuilder.Where(" rbr.CreatedOn >= @CreatedOnStart AND rbr.CreatedOn < @CreatedOnEnd ");
                }
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntitiesTask = conn.QueryAsync<ManuProductBadRecordReportView>(templateData.RawSql, templateData.Parameters);
            return await manuProductBadRecordEntitiesTask;
        }

        /// <summary>
        /// 不合格日志报表分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordLogReportView>> GetPagedInfoLogReportAsync(ManuProductBadRecordLogReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoLogReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoLogReportCountSqlTemplate);

            //where rbr.IsDeleted=0
            //    AND m.MaterialCode like '%%'
            //    AND m.Version like '%%'
            //    AND o.OrderCode like '%%'
            //    AND p.`Code` like '%%'
            //    AND r.ResCode like '%%'
            //    AND uc.UnqualifiedCode like '%%'
            //    AND uc.Type =''
            //    AND rbr.`Status` =''
            //    AND rbr.SFC like '%%'
            //    AND CreatedBy BETWEEN '' and ''

            sqlBuilder.Where(" rbr.IsDeleted = 0 ");
            sqlBuilder.Where(" rbr.SiteId=@SiteId ");

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
            if (!string.IsNullOrEmpty(pageQuery.UnqualifiedCode))
            {
                pageQuery.UnqualifiedCode = $"%{pageQuery.UnqualifiedCode}%";
                sqlBuilder.Where(" uc.UnqualifiedCode like  @UnqualifiedCode ");
            }
            if (pageQuery.UnqualifiedType.HasValue)
            {
                sqlBuilder.Where(" uc.Type =  @UnqualifiedType ");
            }
            if (pageQuery.BadRecordStatus.HasValue)
            {
                sqlBuilder.Where(" rbr.`Status` =  @BadRecordStatus ");
            }
            if (!string.IsNullOrEmpty(pageQuery.SFC))
            {
                pageQuery.SFC = $"%{pageQuery.SFC}%";
                sqlBuilder.Where(" rbr.SFC like @SFC ");
            }
            //if (pageQuery.CreatedOnS.HasValue || pageQuery.CreatedOnE.HasValue)
            //{
            //    if (pageQuery.CreatedOnS.HasValue && pageQuery.CreatedOnE.HasValue)
            //        sqlBuilder.Where(" rbr.CreatedOn BETWEEN @CreatedOnS AND @CreatedOnE ");
            //    else
            //    {
            //        if (pageQuery.CreatedOnS.HasValue) sqlBuilder.Where("rbr.CreatedOn >= @CreatedOnS");
            //        if (pageQuery.CreatedOnE.HasValue) sqlBuilder.Where("rbr.CreatedOn < @CreatedOnE");
            //    }
            //}
            if (pageQuery.CreatedOn != null && pageQuery.CreatedOn.Length > 0)
            {
                if (pageQuery.CreatedOn.Length >= 2)
                {
                    sqlBuilder.AddParameters(new { CreatedOnStart = pageQuery.CreatedOn[0], CreatedOnEnd = pageQuery.CreatedOn[1].AddDays(1) });
                    sqlBuilder.Where(" rbr.CreatedOn >= @CreatedOnStart AND  rbr.CreatedOn < @CreatedOnEnd ");
                }
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntitiesTask = conn.QueryAsync<ManuProductBadRecordLogReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuProductBadRecordEntities = await manuProductBadRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductBadRecordLogReportView>(manuProductBadRecordEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductBadRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_product_bad_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_product_bad_record` /**where**/ ";
        const string GetManuProductBadRecordEntitiesSqlTemplate = @"SELECT 
                                           `Id`, `SiteId`, `FoundBadOperationId`, `OutflowOperationId`, `UnqualifiedId`, `SFC`, `SfcInfoId`,`Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_product_bad_record`  WHERE SFC = @SFC AND  Status=@Status AND SiteId=@SiteId AND IsDeleted=0";

        const string GetEntitiesSqlTemplate = @"SELECT /**select**/  FROM `manu_product_bad_record` br  /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string InsertSql = "INSERT INTO `manu_product_bad_record`(  `Id`, `SiteId`, `FoundBadOperationId`, `FoundBadResourceId`,`OutflowOperationId`, `UnqualifiedId`,`SFC`,`SfcInfoId`,`Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FoundBadOperationId,@FoundBadResourceId, @OutflowOperationId, @UnqualifiedId, @SFC,@SfcInfoId,@Qty, @Status, @Source, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_product_bad_record` SET   FoundBadOperationId = @FoundBadOperationId, OutflowOperationId = @OutflowOperationId, UnqualifiedId = @UnqualifiedId, SFC = @SFC, Qty = @Qty, Status = @Status, Source = @Source, Remark = @Remark, DisposalResult = @DisposalResult, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_product_bad_record` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FoundBadOperationId`, `OutflowOperationId`, `UnqualifiedId`, `SFC`, `SfcInfoId`,`Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_product_bad_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FoundBadOperationId`, `OutflowOperationId`, `UnqualifiedId`, `SFC`,`SfcInfoId`, `Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_product_bad_record`  WHERE Id IN @ids ";

        const string UpdateStatusSql = "UPDATE `manu_product_bad_record` SET Remark = @Remark,Status=@Status,DisposalResult=@DisposalResult,UpdatedBy=@UserId,UpdatedOn=@UpdatedOn WHERE SFC=@Sfc  AND UnqualifiedId=@UnqualifiedId  and  Status!=@Status ";

        const string UpdateStatusByIdSql = "UPDATE `manu_product_bad_record` SET Remark = @Remark,Status=@Status,DisposalResult=@DisposalResult,UpdatedBy=@UserId,UpdatedOn=@UpdatedOn WHERE Id=@Id  AND  Status!=@Status ";

        const string GetPagedInfoReportDataSqlTemplate = @"
                    select 
                        rbr.UnqualifiedId, Sum(rbr.Qty)  as num
                    from manu_product_bad_record rbr
                    LEFT JOIN proc_procedure p on p.Id=rbr.OutflowOperationId -- 为了查询工序编码

                    -- LEFT join manu_sfc s on s.SFC=rbr.SFC
                    left join manu_sfc_info si on si.Id= rbr.SfcInfoId -- 为了获取关联信息

                    LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                    LEFT join plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码

                    /**where**/
                    GROUP BY rbr.UnqualifiedId 
                    ORDER BY num desc

                    LIMIT @Offset,@Rows 
                     ";
        const string GetPagedInfoReportCountSqlTemplate = @" 
                    select 
                        COUNT(DISTINCT rbr.UnqualifiedId )
                    from manu_product_bad_record rbr
                    LEFT JOIN proc_procedure p on p.Id=rbr.OutflowOperationId -- 为了查询工序编码

                    -- LEFT join manu_sfc s on s.SFC=rbr.SFC
                    left join manu_sfc_info si on si.Id= rbr.SfcInfoId -- 为了获取关联信息

                    LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                    LEFT join plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码
                    /**where**/   ";

        const string GetPagedInfoLogReportDataSqlTemplate = @"
                SELECT  rbr.SFC,  
                        m.MaterialCode,m.MaterialName,
                        o.OrderCode,
                        p.`Code` as ProcedureCode,
                        r.ResCode,
                        uc.UnqualifiedCode,
                        uc.Type as UnqualifiedType,
                        rbr.`Status` as BadRecordStatus,
                        rbr.Qty,
                        rbr.CreatedBy,
                        rbr.CreatedOn
                
                FROM manu_product_bad_record rbr
                LEFT JOIN proc_procedure p on p.Id=rbr.OutflowOperationId -- 为了查询工序编码
                LEFT JOIN proc_resource r on r.id=rbr.FoundBadResourceId  -- 为了查询资源
                LEFT JOIN qual_unqualified_code uc on uc.id=rbr.UnqualifiedId -- 为了查询不合格代码

                -- LEFT JOIN manu_sfc s on s.SFC=rbr.SFC
                LEFT JOIN manu_sfc_info si on si.id= rbr.SfcInfoId -- 为了获取关联信息

                LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                LEFT JOIN plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码
                /**where**/
                ORDER BY rbr.CreatedOn desc

                LIMIT @Offset,@Rows 
        ";
        const string GetPagedInfoLogReportCountSqlTemplate = @" 
                SELECT  COUNT(1) 
                FROM manu_product_bad_record rbr
                LEFT JOIN proc_procedure p on p.Id=rbr.OutflowOperationId -- 为了查询工序编码
                LEFT JOIN proc_resource r on r.id=rbr.FoundBadResourceId  -- 为了查询资源
                LEFT JOIN qual_unqualified_code uc on uc.id=rbr.UnqualifiedId -- 为了查询不合格代码

                -- LEFT JOIN manu_sfc s on s.SFC=rbr.SFC
                LEFT JOIN manu_sfc_info si on si.id= rbr.SfcInfoId -- 为了获取关联信息

                LEFT JOIN proc_material m on m.Id=si.ProductId  -- 为了查询物料编码
                LEFT JOIN plan_work_order o on o.Id=si.WorkOrderId -- 为了查询工单编码
                /**where**/  ";
    }
}
