using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 转换系数表仓储
    /// </summary>
    public partial class ProcPrintSetupRepository : BaseRepository, IProcPrintSetupRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcPrintSetupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
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
        public async Task<ProcConversionFactorEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcConversionFactorEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcConversionFactorEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcConversionFactorEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetByResourceIdAsync(long resourceId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointEntity>(GetByResourceIdSql, new { resourceId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procPrintSetupPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrintSetupView>> GetPagedInfoAsync(IProcPrintSetupPagedQuery procPrintSetupPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            //sqlBuilder.LeftJoin("JOIN proc_material B ON A.MaterialId = B.Id JOIN proc_resource C ON A.ResourceId = C.Id JOIN proc_printer D ON A.PrintId = D.Id JOIN proc_label_template E ON A.LabelTemplateId = E.Id JOIN proc_resource_type F ON C.ResTypeId = F.Id");
            sqlBuilder.Where("A.IsDeleted=0");
            if (string.IsNullOrEmpty(procPrintSetupPagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("A.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procPrintSetupPagedQuery.Sorting);
            }

            sqlBuilder.Where("A.SiteId = @SiteId");
            //查询
            if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.MaterialCode))
            {
                procPrintSetupPagedQuery.MaterialCode = $"%{procPrintSetupPagedQuery.MaterialCode}%";
                sqlBuilder.Where("b.MaterialCode like @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Version))
            {
                procPrintSetupPagedQuery.Version = $"%{procPrintSetupPagedQuery.Version}%";
                sqlBuilder.Where("b.Version like @Version");
            }

            if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Name))
            {
                procPrintSetupPagedQuery.Name = $"%{procPrintSetupPagedQuery.Name}%";
                sqlBuilder.Where("e.Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.PrintName))
            {
                procPrintSetupPagedQuery.PrintName = $"%{procPrintSetupPagedQuery.PrintName}%";
                sqlBuilder.Where("c.PrintName like @PrintName");
            }
            if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Status.ToString()))
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (procPrintSetupPagedQuery.PageIndex - 1) * procPrintSetupPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procPrintSetupPagedQuery.PageSize });
            sqlBuilder.AddParameters(procPrintSetupPagedQuery);

            using var conn = GetMESDbConnection();

            var procProcedureEntitiesTask = conn.QueryAsync<ProcPrintSetupView>(templateData.RawSql, templateData.Parameters);

            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureEntities = await procProcedureEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcPrintSetupView>(procProcedureEntities, procPrintSetupPagedQuery.PageIndex, procPrintSetupPagedQuery.PageSize, totalCount);            
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procPrintSetupQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcPrintSetupEntity>> GetProcConversionFactorEntitiesAsync(ProcPrintSetupQuery procConversionFactorQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcConversionFactorEntitiesSqlTemplate);
            sqlBuilder.Where(" IsDeleted=0 ");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.ResourceId.ToString()))
            {
                sqlBuilder.Where(" ResourceId = @ResourceId ");
            }
            if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.MaterialId.ToString()))
            {
                sqlBuilder.Where(" MaterialId = @MaterialId ");
            }
            if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.PrintId.ToString()))
            {
                sqlBuilder.Where(" PrintId = @PrintId ");
            }
            if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.LabelTemplateId.ToString()))
            {
                sqlBuilder.Where(" LabelTemplateId = @LabelTemplateId ");
            }
            using var conn = GetMESDbConnection();
            var procConversionFactortEntities = await conn.QueryAsync<ProcPrintSetupEntity>(template.RawSql, procConversionFactorQuery);
            return procConversionFactortEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcPrintSetupEntity procConversionFactorEntity)
        {
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(InsertSql, procConversionFactorEntity);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procLoadPointEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcConversionFactorEntity procLoadPointEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLoadPointEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointEntitys);
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
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcPrintSetupRepository
    {
        //const string GetPagedInfoDataSqlTemplate = @"select * from proc_print_configure /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        //const string GetPagedInfoDataSqlTemplate = @"select A.Type,A.CreatedOn,B.MaterialCode,B.Version,C.Status,D.PrintName from proc_print_configure A JOIN proc_material B ON A.MaterialId = B.Id JOIN proc_resource C ON A.ResourceId = C.Id  JOIN proc_printer D ON A.PrintId = D.Id /**where**/ /**orderby**/ LIMIT @Offset,@Rows";

        const string GetPagedInfoDataSqlTemplate = @"select A.UpdatedBy,A.Type,A.UpdatedOn,B.MaterialCode,B.Version,C.Status,D.PrintName,E.`Name`,F.ResTypeName from proc_print_configure A JOIN proc_material B ON A.MaterialId = B.Id JOIN proc_resource C ON A.ResourceId = C.Id  JOIN proc_printer D ON A.PrintId = D.Id JOIN proc_label_template E ON A.LabelTemplateId = E.Id JOIN proc_resource_type F ON C.ResTypeId = F.Id  /**where**/ /**orderby**/ LIMIT @Offset,@Rows";

        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) from proc_print_configure a /**join**/ /**where**/ ";

        const string GetProcConversionFactorEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_print_configure` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_print_configure` (`Id`, `MaterialId`, `Type`, `ResourceId`, `Program`, `BusinessType`, `PrintId`, `LabelTemplateId`, `Count`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`, `Status`) VALUES (@Id, @MaterialId, @Type, @ResourceId, @Program, @BusinessType, @PrintId, @LabelTemplateId, @Count, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted, @Status);";
        const string InsertsSql = "INSERT INTO `proc_product_process_conversion_factor`( `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_product_process_conversion_factor` SET  Remark = @Remark, ConversionFactor = @ConversionFactor,OpenStatus = @OpenStatus, MaterialId=@MaterialId, ProcedureId=@ProcedureId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_product_process_conversion_factor` SET   SiteId = @SiteId, LoadPoint = @LoadPoint, LoadPointName = @LoadPointName, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_product_process_conversion_factor` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_product_process_conversion_factor` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                              `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_product_process_conversion_factor`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                           `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_product_process_conversion_factor`  WHERE Id IN @ids ";
        const string GetByResourceIdSql = @"SELECT PLP.* FROM proc_product_process_conversion_factor PLP
                                LEFT JOIN proc_product_process_conversion_factor_link_resource PLPLR ON PLPLR.LoadPointId = PLP.Id
                                WHERE PLPLR.ResourceId = @resourceId AND PLP.IsDeleted = 0";


        const string UpdateStatusSql = "UPDATE `proc_product_process_conversion_factor` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
    }
}
