using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过程参数仓储
    /// </summary>
    public partial class ManuProductParameterRepository : BaseRepository, IManuProductParameterRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuProductParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuProductParameterEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuProductParameterEntity> entities)
        {
            using var conn = GetMESDbConnection();
            StringBuilder stringBuilder = new StringBuilder(InsertHeadSql);
            var insertsParams = new DynamicParameters();
            var i = 0;
            foreach (var item in entities)
            {
                stringBuilder.AppendFormat(InsertTailSql,i);
                insertsParams.Add($"{nameof(item.Id)}{i}", item.Id);
                insertsParams.Add($"{nameof(item.SiteId)}{i}", item.SiteId);
                insertsParams.Add($"{nameof(item.ProcedureId)}{i}", item.ProcedureId);
                insertsParams.Add($"{nameof(item.ResourceId)}{i}", item.ResourceId);
                insertsParams.Add($"{nameof(item.EquipmentId)}{i}", item.EquipmentId);
                insertsParams.Add($"{nameof(item.SFC)}{i}", item.SFC);
                insertsParams.Add($"{nameof(item.WorkOrderId)}{i}", item.WorkOrderId);
                insertsParams.Add($"{nameof(item.ProductId)}{i}", item.ProductId);
                insertsParams.Add($"{nameof(item.ParameterId)}{i}", item.ParameterId);
                insertsParams.Add($"{nameof(item.ParamValue)}{i}", item.ParamValue);
                insertsParams.Add($"{nameof(item.StandardUpperLimit)}{i}", item.StandardUpperLimit);
                insertsParams.Add($"{nameof(item.StandardLowerLimit)}{i}", item.StandardLowerLimit);
                insertsParams.Add($"{nameof(item.JudgmentResult)}{i}", item.JudgmentResult);
                insertsParams.Add($"{nameof(item.TestDuration)}{i}", item.TestDuration);
                insertsParams.Add($"{nameof(item.TestTime)}{i}", item.TestTime);
                insertsParams.Add($"{nameof(item.TestResult)}{i}", item.TestResult);
                insertsParams.Add($"{nameof(item.LocalTime)}{i}", item.LocalTime);
                insertsParams.Add($"{nameof(item.CreatedBy)}{i}", item.CreatedBy);
                insertsParams.Add($"{nameof(item.CreatedOn)}{i}", item.CreatedOn);
                insertsParams.Add($"{nameof(item.UpdatedBy)}{i}", item.UpdatedBy);
                insertsParams.Add($"{nameof(item.UpdatedOn)}{i}", item.UpdatedOn);
                insertsParams.Add($"{nameof(item.IsDeleted)}{i}", item.IsDeleted);
                insertsParams.Add($"{nameof(item.StepId)}{i}", item.StepId);
                i++;

            }
            stringBuilder.Length--;
            return await conn.ExecuteAsync(stringBuilder.ToString(), insertsParams);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(EquipmentIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteScalarAsync(IsExistsSql, query) != null;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuProductParameterEntity manuProductParameterEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuProductParameterEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterEntity> manuProductParameterEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuProductParameterEntities);
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetManuProductParameterAsync(ManuProductParameterQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetManuProductParameterEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where("SFC = @Sfc");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuProductParameterEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 条码上报参数分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductParameterView>> GetManuProductParameterPagedInfoAsync(ManuProductParameterPagedQuery queryParam)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetProductParameterPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetProductParameterPagedInfoCountSqlTemplate);
            sqlBuilder.Select("T1.Id,T1.SiteId,T1.ProcedureId,T1.ResourceId,T1.EquipmentId,T1.SFC,T1.WorkOrderId,T1.ProductId,T1.LocalTime," +
                "T2.ParameterCode,T2.ParameterName,T1.ParamValue,T2.ParameterUnit,T1.CreatedOn,T1.CreatedBy,T1.UpdatedOn,T1.UpdatedBy,T1.StepId,T3.ParameterType");

            sqlBuilder.LeftJoin("proc_parameter T2 ON T1.ParameterId = T2.Id AND T2.SiteId=T1.SiteId AND T2.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_parameter_link_type T3 ON T3.ParameterID=T1.ParameterId  AND T3.SiteId=T1.SiteId AND T3.IsDeleted=0");

            sqlBuilder.Where("T1.SiteId=@SiteId ")
                .Where("T1.IsDeleted=0 ")
                .Where("T1.SFC=@SFC ");//条码必须传递
            if (queryParam.ParameterType.HasValue)
            {
                sqlBuilder.Where("T3.ParameterType = @ParameterType ");
            }
            if (queryParam.StartTime.HasValue)
            {
                sqlBuilder.Where("T1.CreatedOn >=@StartTime ");
            }
            if (queryParam.EndTime.HasValue)
            {
                sqlBuilder.Where("T1.EndTime <@EndTime ");
            }
            if (queryParam.LocalTimeStartTime.HasValue)
            {
                sqlBuilder.Where("T1.LocalTime >=@LocalTimeStartTime ");
            }
            if (queryParam.LocalTimeEndTime.HasValue)
            {
                sqlBuilder.Where("T1.LocalTime <@LocalTimeEndTime ");
            }
            var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
            sqlBuilder.AddParameters(queryParam);

            using var conn = GetMESDbConnection();
            var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuProductParameterView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductParameterView>(manuSfcCirculationEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductParameterRepository
    {
        const string GetManuProductParameterEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_product_parameter` /**where**/  ";

        const string InsertSql = @"INSERT INTO `manu_product_parameter`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `ParameterId`, `ParamValue`, StandardUpperLimit, StandardLowerLimit, JudgmentResult, TestDuration, TestTime, TestResult,`LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `StepId`) 
                        VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @ParameterId, @ParamValue, @StandardUpperLimit, @StandardLowerLimit, @JudgmentResult, @TestDuration, @TestTime, @TestResult, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @StepId )  ";
        const string InsertHeadSql = @"INSERT INTO `manu_product_parameter`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `ParameterId`, `ParamValue`, StandardUpperLimit, StandardLowerLimit, JudgmentResult, TestDuration, TestTime, TestResult,`LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `StepId`) 
                        VALUES ";
        const string InsertTailSql = @"(@Id{0}, @SiteId{0}, @ProcedureId{0}, @ResourceId{0}, @EquipmentId{0}, @SFC{0}, @WorkOrderId{0}, @ProductId{0}, @ParameterId{0}, @ParamValue{0}, @StandardUpperLimit{0}, @StandardLowerLimit{0}, @JudgmentResult{0}, @TestDuration{0}, @TestTime{0}, @TestResult{0}, @LocalTime{0}, @CreatedBy{0}, @CreatedOn{0}, @UpdatedBy{0}, @UpdatedOn{0}, @IsDeleted{0}, @StepId{0}),";

        const string IsExistsSql = @"SELECT Id FROM manu_product_parameter WHERE `IsDeleted` = 0 AND SiteId = @SiteId AND EquipmentId = @EquipmentId AND ResourceId = @ResourceId AND SFC = @SFC LIMIT 1";
        const string UpdateSql = "UPDATE `manu_product_parameter` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, `LocalTime` = @LocalTime, ParameterId = @ParameterId, ParamValue = @ParamValue, StandardUpperLimit = @StandardUpperLimit, StandardLowerLimit = @StandardLowerLimit, JudgmentResult = @JudgmentResult, TestDuration = @TestDuration, TestTime = @TestTime, TestResult = @TestResult, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, StepId = @StepId  WHERE Id = @Id ";

        const string GetProductParameterPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_product_parameter T1  /**leftjoin**/ /**where**/  /**orderby**/  LIMIT @Offset,@Rows ";

        const string GetProductParameterPagedInfoCountSqlTemplate = @"SELECT count(1) FROM manu_product_parameter T1  /**leftjoin**/ /**where**/ ";
    }
}
