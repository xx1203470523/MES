using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 打印设置仓储
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
        public async Task<ProcPrintSetupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcPrintSetupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcPrintSetupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcPrintSetupEntity>(GetByIdsSql, new { ids = ids });
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
          
            try
            {
                var sqlBuilder = new SqlBuilder();
                var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
                var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
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
                //物料编码
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.MaterialCode))
                {
                    procPrintSetupPagedQuery.MaterialCode = $"%{procPrintSetupPagedQuery.MaterialCode}%";
                    sqlBuilder.Where("B.MaterialCode like @MaterialCode");
                }
                //物料版本
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Version))
                {
                    procPrintSetupPagedQuery.Version = $"%{procPrintSetupPagedQuery.Version}%";
                    sqlBuilder.Where("B.Version like @Version");
                }
                //业务类型
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.BusinessType.ToString()))
                {
                    //procPrintSetupPagedQuery.BusinessType = $"%{procPrintSetupPagedQuery.BusinessType}%";
                    sqlBuilder.Where("A.BusinessType like @BusinessType");
                }
                //模板名称
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Name))
                {
                    procPrintSetupPagedQuery.Name = $"%{procPrintSetupPagedQuery.Name}%";
                    sqlBuilder.Where("E.Name like @Name");
                }
                //资源/类
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Type.ToString()))
                {
                    //procPrintSetupPagedQuery.Name = $"%{procPrintSetupPagedQuery.Name}%";
                    sqlBuilder.Where("A.Type like @Type");
                }
                //打印机
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.PrintName))
                {
                    procPrintSetupPagedQuery.PrintName = $"%{procPrintSetupPagedQuery.PrintName}%";
                    sqlBuilder.Where("D.PrintName like @PrintName");
                }
                //状态
                if (!string.IsNullOrWhiteSpace(procPrintSetupPagedQuery.Status.ToString()))
                {
                    //procPrintSetupPagedQuery.Status = $"%{procPrintSetupPagedQuery.Status}%";
                    sqlBuilder.Where("A.Status = @Status");
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
            catch (Exception ex)
            {

                throw;
            }
                       
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
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcPrintSetupEntity procLoadPointEntity)
        {
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(UpdateSql, procLoadPointEntity);
            }
            catch (Exception ex) 
            {

                throw;
            }
           
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
        const string GetPagedInfoDataSqlTemplate = @"select A.Id,A.UpdatedBy,A.Type,A.UpdatedOn,A.BusinessType,A.`Status`,B.MaterialCode,B.Version,D.PrintName,E.`Name` from proc_print_configure A LEFT JOIN proc_material B ON A.MaterialId = B.Id LEFT JOIN proc_resource C ON A.ResourceId = C.Id  LEFT JOIN proc_printer D ON A.PrintId = D.Id LEFT JOIN proc_label_template E ON A.LabelTemplateId = E.Id LEFT JOIN proc_resource_type F ON C.ResTypeId = F.Id  /**where**/ /**orderby**/ LIMIT @Offset,@Rows";

        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) FROM  proc_print_configure A LEFT JOIN proc_material B ON A.MaterialId = B.Id LEFT JOIN proc_resource C ON A.ResourceId = C.Id LEFT JOIN proc_printer D ON A.PrintId = D.Id LEFT JOIN proc_label_template E ON A.LabelTemplateId = E.Id LEFT JOIN proc_resource_type F ON C.ResTypeId = F.Id  /**join**/ /**where**/ ";

        const string GetProcConversionFactorEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_print_configure` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_print_configure` (`Id`, `MaterialId`, `Type`, `ResourceId`, `Program`, `BusinessType`, `PrintId`, `LabelTemplateId`, `Count`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`, `Status`) VALUES (@Id, @MaterialId, @Type, @ResourceId, @Program, @BusinessType, @PrintId, @LabelTemplateId, @Count, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted, @Status);";
        const string UpdateSql = "UPDATE `proc_print_configure` SET  Remark = @Remark, Status = @Status, MaterialId=@MaterialId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, Count = @Count, LabelTemplateId = @LabelTemplateId, BusinessType = @BusinessType, Class = @Class, Program = @Program, ResourceId = @ResourceId  WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_print_configure` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT A.Id, A.Class, A.MaterialId, A.Type, A.ResourceId, A.Program, A.BusinessType, A.PrintId, A.LabelTemplateId, A.Count, A.Remark, A.CreatedOn, A.CreatedBy, A.UpdatedBy, A.UpdatedOn, A.SiteId, A.IsDeleted, A.Status, B.MaterialCode, B.Version, C.ResCode, D.PrintName, E.Name, E.Content
                                   FROM proc_print_configure A
                                   LEFT JOIN proc_material B ON A.MaterialId = B.Id
                                   LEFT JOIN proc_resource C ON A.ResourceId = C.Id
                                   LEFT JOIN proc_printer D ON A.PrintId = D.Id
                                   LEFT JOIN proc_label_template E ON A.LabelTemplateId = E.Id
                                   LEFT JOIN proc_resource_type F ON C.ResTypeId = F.Id                                  
                                   WHERE A.Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `MaterialId`, `Type`, `ResourceId`, `Program`, `BusinessType`, `PrintId`, `LabelTemplateId`, `Count`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`, `Status`, `Class`
                            FROM `proc_print_configure`  WHERE Id IN @ids ";
        const string GetByResourceIdSql = @"SELECT PLP.* FROM proc_product_process_conversion_factor PLP
                                LEFT JOIN proc_product_process_conversion_factor_link_resource PLPLR ON PLPLR.LoadPointId = PLP.Id
                                WHERE PLPLR.ResourceId = @resourceId AND PLP.IsDeleted = 0";


        const string UpdateStatusSql = "UPDATE `proc_product_process_conversion_factor` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
    }
}
