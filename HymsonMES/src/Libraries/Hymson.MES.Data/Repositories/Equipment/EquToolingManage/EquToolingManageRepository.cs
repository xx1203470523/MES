using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 工具管理表仓储
    /// </summary>
    public partial class EquToolingManageRepository : BaseRepository, IEquToolingManageRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolingManageRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquToolingManageView> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolingManageView>(GetByIdSql, new { Id = id });
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
        /// <param name="procequToolingManagePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingManageView>> GetPagedInfoAsync(IEquToolingManagePagedQuery equToolingManagePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            //if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Code) || !string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Name))
            //{
            //    sqlBuilder.Join(" equ_tooling_type_manage b ON a.ToolsId = b.Id");
            //}
            //if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.MaterialCode) || !string.IsNullOrWhiteSpace(equToolingManagePagedQuery.MaterialName))
            //{
            //    sqlBuilder.Join(" proc_material c ON a.MaterialId = c.Id");
            //}

            sqlBuilder.Where("a.IsDeleted=0");
            if (string.IsNullOrEmpty(equToolingManagePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("a.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(equToolingManagePagedQuery.Sorting);
            }

            sqlBuilder.Where("a.SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Code))
            {
                equToolingManagePagedQuery.Code = $"%{equToolingManagePagedQuery.Code}%";
                sqlBuilder.Where("a.Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Name))
            {
                equToolingManagePagedQuery.Name = $"%{equToolingManagePagedQuery.Name}%";
                sqlBuilder.Where("a.Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Status.ToString()))
            {
                sqlBuilder.Where("a.Status = @Status");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.ToolsTypeCode))
            {
                equToolingManagePagedQuery.ToolsTypeCode = $"%{equToolingManagePagedQuery.ToolsTypeCode}%";
                sqlBuilder.Where("b.Code like @ToolsTypeCode");
            }
            if (equToolingManagePagedQuery.UpdatedOn != null && equToolingManagePagedQuery.UpdatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equToolingManagePagedQuery.UpdatedOn[0], EndTime = equToolingManagePagedQuery.UpdatedOn[1].AddDays(1) });
                sqlBuilder.Where("a.UpdatedOn >= @StartTime AND a.UpdatedOn < @EndTime");
            }

            var offSet = (equToolingManagePagedQuery.PageIndex - 1) * equToolingManagePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equToolingManagePagedQuery.PageSize });
            sqlBuilder.AddParameters(equToolingManagePagedQuery);

            using var conn = GetMESDbConnection();
            var procProcedureEntitiesTask = conn.QueryAsync<EquToolingManageView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureEntities = await procProcedureEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolingManageView>(procProcedureEntities, equToolingManagePagedQuery.PageIndex, equToolingManagePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procConversionFactorQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcConversionFactorEntity>> GetProcConversionFactorEntitiesAsync(IEquToolingManagePagedQuery procConversionFactorQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcConversionFactorEntitiesSqlTemplate);
            sqlBuilder.Where(" IsDeleted=0 ");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.ProcedureId.ToString()))
            //{
            //    sqlBuilder.Where(" ProcedureId = @ProcedureId ");
            //}
            //if (!string.IsNullOrWhiteSpace(procConversionFactorQuery.MaterialId.ToString()))
            //{
            //    sqlBuilder.Where(" MaterialId = @MaterialId ");
            //}
            using var conn = GetMESDbConnection();
            var procConversionFactortEntities = await conn.QueryAsync<ProcConversionFactorEntity>(template.RawSql, procConversionFactorQuery);
            return procConversionFactortEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcConversionFactorEntity procConversionFactorEntity)
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
    public partial class EquToolingManageRepository
    {

        const string GetPagedInfoDataSqlTemplate = @"select A.Code,A.Name,A.Status,A.RatedLife,A.IsCalibrated,A.CalibrationCycle, A.Remark, A.CreatedOn, A.CreatedBy, A.UpdatedBy, A.UpdatedOn, A.Id, B.Code AS ToolsTypeCode,B.Name AS ToolsTypeName  from equ_tools A JOIN equ_tooling_type_manage B ON A.ToolsId = B.Id  /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) from equ_tools A  JOIN equ_tooling_type_manage B ON A.ToolsId = B.Id /**join**/ /**where**/ ";

        const string GetProcConversionFactorEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_product_process_conversion_factor` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_product_process_conversion_factor`( `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES ( @Id, @SiteId, @ProcedureId, @MaterialId, @ConversionFactor,@OpenStatus, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )";
        const string InsertsSql = "INSERT INTO `proc_product_process_conversion_factor`( `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_product_process_conversion_factor` SET  Remark = @Remark, ConversionFactor = @ConversionFactor,OpenStatus = @OpenStatus, MaterialId=@MaterialId, ProcedureId=@ProcedureId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_product_process_conversion_factor` SET   SiteId = @SiteId, LoadPoint = @LoadPoint, LoadPointName = @LoadPointName, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_product_process_conversion_factor` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_product_process_conversion_factor` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT A.Code,A.Name,A.Status,A.RatedLife,A.IsCalibrated,A.CalibrationCycle, A.Remark, A.CreatedOn, A.CreatedBy, A.UpdatedBy, A.UpdatedOn, A.Id, B.Code AS ToolsTypeCode,B.Name AS ToolsTypeName  from equ_tools A JOIN equ_tooling_type_manage B ON A.ToolsId = B.Id  WHERE A.Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                           `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `ConversionFactor`, `OpenStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_product_process_conversion_factor`  WHERE Id IN @ids ";
        const string GetByResourceIdSql = @"SELECT PLP.* FROM proc_product_process_conversion_factor PLP
                                LEFT JOIN proc_product_process_conversion_factor_link_resource PLPLR ON PLPLR.LoadPointId = PLP.Id
                                WHERE PLPLR.ResourceId = @resourceId AND PLP.IsDeleted = 0";


        const string UpdateStatusSql = "UPDATE `proc_product_process_conversion_factor` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
    }
}
