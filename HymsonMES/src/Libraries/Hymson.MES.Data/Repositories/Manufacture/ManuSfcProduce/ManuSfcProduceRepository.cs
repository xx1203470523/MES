/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除） 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除）仓储
    /// </summary>
    public partial class ManuSfcProduceRepository : IManuSfcProduceRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ManuSfcProduceRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceView>> GetPagedInfoAsync(ManuSfcProducePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("msp.SiteId = @SiteId");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select("msp.ProductBOMId,msp.Id,msp.Lock,msp.Sfc,msp.LockProductionId,msp.Status,pwo.OrderCode,pp.Code,pp.Name,pm.MaterialCode,pm.MaterialName,pm.Version ");

            sqlBuilder.LeftJoin("proc_material pm  on msp.ProductId =pm.Id  and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo on msp.WorkOrderId =pwo.Id  and pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on msp.ProcedureId =pp.Id and pp.IsDeleted =0");

            //状态
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (query.Lock.HasValue)
            {
                sqlBuilder.Where("msp.Lock=@Lock");
            }
            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                query.Sfc = $"%{query.Sfc}%";
                sqlBuilder.Where("msp.Sfc like @Sfc");
            }
            if (query.SfcArray != null && query.SfcArray.Length > 0)
            {
                sqlBuilder.Where("msp.Sfc in @SfcArray");
            }
            //工单
            if (!string.IsNullOrWhiteSpace(query.OrderCode))
            {
                query.OrderCode = $"%{query.OrderCode}%";
                sqlBuilder.Where("pwo.OrderCode like @OrderCode");
            }
            //工序
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("pp.Code like @Code");
            }
            //资源-》资源类型
            if (query.ResourceTypeId.HasValue)
            {
                sqlBuilder.Where("pp.ResourceTypeId=@ResourceTypeId");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetManuSfcProduceEntitiesAsync(ManuSfcProduceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            if (query.Sfcs != null && query.Sfcs.Length > 0)
            {
                sqlBuilder.Where("Sfc in @Sfcs");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntities = await conn.QueryAsync<ManuSfcProduceEntity>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcProduceEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetBySFCAsync(string sfc)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceEntity>(GetBySFCSql, new { sfc=sfc });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcProduceEntity manuSfcProduceEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcProduceEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcProduceEntitys);
        }

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> QualityLockAsync(QualityLockCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateQualityLockSql, command);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcProduceEntity manuSfcProduceEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcProduceEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcProduceEntitys);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(IEnumerable<long> ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteRangeSql, new { ids = ids });
        }

        /// <summary>
        /// 删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalAsync(string sfc)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletePhysicalSql, new { sfc });
        }

        /// <summary>
        /// 批量删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalRangeAsync(IEnumerable<string> sfcs)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletePhysicalRangeSql, new { Sfcs = sfcs });
        }

        /// <summary>
        /// 批量更新条码IsScrap
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateIsScrapAsync(UpdateIsScrapCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateIsScrapSql, command);
        }

        #region 在制品业务
        /// <summary>
        /// 新增在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSfcProduceBusinessSql, manuSfcProduceBusinessEntity);
        }

        /// <summary>
        /// 批量新增在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcProduceBusinessRangAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
        }

        /// <summary>
        /// 更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdatetSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntity);
        }

        /// <summary>
        /// 批量更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatestSfcProduceBusinessRangAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
        }

        /// <summary>
        /// 根据ID获取在制品业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCIdAsync(long sfcInfoId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCIdSql, new { SfcInfoId = sfcInfoId });
        }

        /// <summary>
        /// 根据IDs批量获取在制品业务
        /// </summary>
        /// <param name="sfcInfoIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetSfcProduceBusinessBySFCIdsAsync(IEnumerable<long> sfcInfoIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCIdsSql, new { SfcInfoIds = sfcInfoIds });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="sfcInfoIds"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinesssAsync(IEnumerable<long> sfcInfoIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(RealDeletesSfcProduceBusinessSql, new { SfcInfoIds = sfcInfoIds });
        }
        #endregion
    }

    public partial class ManuSfcProduceRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_produce`  msp /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_produce`  msp  /**innerjoin**/ /**leftjoin**/  /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT * FROM `manu_sfc_produce` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_produce`(  `Id`, `Sfc`, `ProductId`, `WorkOrderId`, `BarCodeInfoId`, `ProcessRouteId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Status`, `Lock`, `LockProductionId`, `IsSuspicious`, `RepeatedCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Sfc, @ProductId, @WorkOrderId, @BarCodeInfoId, @ProcessRouteId, @WorkCenterId, @ProductBOMId,@Qty, @EquipmentId, @ResourceId, @ProcedureId, @Status, @Lock, @LockProductionId, @IsSuspicious, @RepeatedCount, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertSfcProduceBusinessSql = "INSERT INTO `manu_sfc_produce_business`(  `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcInfoId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_sfc_produce` SET   Sfc = @Sfc, ProductId = @ProductId, WorkOrderId = @WorkOrderId, BarCodeInfoId = @BarCodeInfoId, ProcessRouteId = @ProcessRouteId, WorkCenterId = @WorkCenterId, ProductBOMId = @ProductBOMId, EquipmentId = @EquipmentId, ResourceId = @ResourceId, ProcedureId = @ProcedureId, Status = @Status, Lock = @Lock, LockProductionId = @LockProductionId, IsSuspicious = @IsSuspicious, RepeatedCount = @RepeatedCount, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateSfcProduceBusinessSql = "UPDATE `manu_sfc_produce_business` SET    BusinessType = @BusinessType, BusinessContent = @BusinessContent, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_produce` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id = @Id ";
        const string DeleteRangeSql = "UPDATE `manu_sfc_produce` SET IsDeleted = Id ,UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `Sfc`, `ProductId`, `WorkOrderId`, `BarCodeInfoId`, `ProcessRouteId`, `WorkCenterId`, `ProductBOMId`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Status`, `Lock`, `LockProductionId`, `IsSuspicious`, `RepeatedCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_produce`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Sfc`, `ProductId`, `WorkOrderId`, `BarCodeInfoId`, `ProcessRouteId`, `WorkCenterId`, `ProductBOMId`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Status`, `Lock`, `LockProductionId`, `IsSuspicious`, `RepeatedCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_produce`  WHERE Id IN @ids ";
        const string GetSfcProduceBusinessBySFCIdSql = "SELECT `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted` FROM manu_sfc_produce_business WHERE SfcInfoId = @SfcInfoId AND IsDeleted=0";
        const string GetSfcProduceBusinessBySFCIdsSql = "SELECT `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted` FROM manu_sfc_produce_business WHERE SfcInfoId IN @SfcInfoIds  AND IsDeleted=0";
        const string GetBySFCSql = @"SELECT * FROM manu_sfc_produce WHERE SFC = @sfc ";
        const string DeletePhysicalSql = "DELETE FROM manu_sfc_produce WHERE SFC = @sfc";
        const string DeletePhysicalRangeSql = "delete from manu_sfc_produce where SFC in @Sfcs";
        const string RealDeletesSfcProduceBusinessSql = "delete from manu_sfc_produce_business where SfcInfoId IN @SfcInfoIds";
        //质量锁定sql
        const string UpdateQualityLockSql = "update  manu_sfc_produce set `Lock`=@Lock,LockProductionId=@LockProductionId,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn where SFC in  @Sfcs";
        const string UpdateIsScrapSql = "UPDATE `manu_sfc_produce` SET IsScrap = @IsScrap, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";
    }
}
