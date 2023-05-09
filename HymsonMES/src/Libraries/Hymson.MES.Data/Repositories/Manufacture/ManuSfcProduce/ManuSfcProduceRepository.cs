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
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

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

            sqlBuilder.Select("msp.IsScrap,msp.ProductBOMId,msp.Id,msp.ProcessRouteId,msp.ProcedureId,msp.Sfc,msp.Status,pwo.OrderCode,pp.Code,pp.Name,pm.MaterialCode,pm.MaterialName,pm.Version,pr.ResCode ");

            sqlBuilder.LeftJoin("proc_material pm  on msp.ProductId =pm.Id  and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo on msp.WorkOrderId =pwo.Id  and pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on msp.ProcedureId =pp.Id and pp.IsDeleted =0");
            sqlBuilder.LeftJoin("proc_resource pr on msp.ResourceId =pr.Id and pr.IsDeleted =0");

            //状态
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (query.Lock.HasValue)
            {
                sqlBuilder.Where("msp.Lock=@Lock");
            }
            if (query.NoLock.HasValue)
            {
                if (query.NoLock != 1)
                {
                    sqlBuilder.Where("(msp.Lock!=@NoLock or `Lock`  is null)");
                }
            }
            if (query.IsScrap.HasValue)
            {
                sqlBuilder.Where("msp.IsScrap=@IsScrap");
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
            //资源
            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("pr.ResCode like @ResCode");
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

        public async Task<IEnumerable<ManuSfcProduceInfoView>> GetManuSfcProduceInfoEntitiesAsync(ManuSfcProduceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesInfoSqlTemplate);

            sqlBuilder.Select("msp.*,msi.Id as SfcInfoId");
            sqlBuilder.LeftJoin("manu_sfc mf  on mf.SFC =msp.sfc  and mf.IsDeleted=0");
            sqlBuilder.LeftJoin("manu_sfc_info msi on msi.SfcId =mf.Id  and msi.IsDeleted=0");

            sqlBuilder.Where("msp.SiteId = @SiteId");
            if (query.Sfcs != null && query.Sfcs.Length > 0)
            {
                sqlBuilder.Where("msp.Sfc in @Sfcs");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntities = await conn.QueryAsync<ManuSfcProduceInfoView>(template.RawSql, query);
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
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceEntity>(GetBySFCSql, new { sfc });
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

        /// <summary>
        /// 更新条码Status
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(UpdateStatusCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 更新工序ProcedureId
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcedureIdAsync(UpdateProcedureCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateProcedureIdSql, command);
        }

        /// <summary>
        /// 根据SFC批量更新工序与状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param 
        /// <returns></returns>
        public async Task<int> UpdateProcedureAndStatusRangeAsync(UpdateProcedureAndStatusCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateProcedureAndStatusSql, command);
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
        public async Task<int> InsertSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
        }

        /// <summary>
        /// 插入或者更新
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertOrUpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
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
        public async Task<int> UpdatestSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
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
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCAsync(SfcProduceBusinessQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCSql, query);
        }

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessView>> GetSfcProduceBusinessListBySFCAsync(SfcListProduceBusinessQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcProduceBusinessView>(GetSfcProduceBusinessBySFCsSql, query);
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
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinessBySfcInfoIdAsync(DeleteSfcProduceBusinesssBySfcInfoIdCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSfcProduceBusinessBySfcInfoIdSql, command);
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinesssAsync(DeleteSfcProduceBusinesssCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(RealDeletesSfcProduceBusinessSql, command);
        }
        #endregion
    }

    public partial class ManuSfcProduceRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_produce`  msp /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_produce`  msp  /**innerjoin**/ /**leftjoin**/  /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT * FROM `manu_sfc_produce` /**where**/  ";
        const string GetEntitiesInfoSqlTemplate = @"SELECT  /**select**/ FROM `manu_sfc_produce` msp /**innerjoin**/ /**leftjoin**/  /**where**/   ";

        const string InsertSql = "INSERT INTO `manu_sfc_produce`(  `Id`, `SFC`, `ProductId`, `WorkOrderId`, `BarCodeInfoId`, `ProcessRouteId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Status`, `Lock`, `LockProductionId`, `IsSuspicious`, `RepeatedCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsScrap`) VALUES (   @Id, @SFC, @ProductId, @WorkOrderId, @BarCodeInfoId, @ProcessRouteId, @WorkCenterId, @ProductBOMId, @Qty, @EquipmentId, @ResourceId, @ProcedureId, @Status, @Lock, @LockProductionId, @IsSuspicious, @RepeatedCount, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsScrap )  ";
        const string InsertSfcProduceBusinessSql = "INSERT INTO `manu_sfc_produce_business`(  `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcInfoId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_sfc_produce` SET Sfc = @Sfc, ProductId = @ProductId, WorkOrderId = @WorkOrderId, BarCodeInfoId = @BarCodeInfoId, ProcessRouteId = @ProcessRouteId, WorkCenterId = @WorkCenterId, ProductBOMId = @ProductBOMId, EquipmentId = @EquipmentId, ResourceId = @ResourceId, ProcedureId = @ProcedureId, Status = @Status, `Lock` = @Lock, LockProductionId = @LockProductionId, IsSuspicious = @IsSuspicious, RepeatedCount = @RepeatedCount, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateSfcProduceBusinessSql = "UPDATE `manu_sfc_produce_business` SET    BusinessType = @BusinessType, BusinessContent = @BusinessContent, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        //const string UpdateSql = "UPDATE `manu_sfc_produce` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id = @Id ";
        const string DeleteSql = "delete from manu_sfc_produce where Id = @Id  ";
        const string DeleteRangeSql = "UPDATE `manu_sfc_produce` SET IsDeleted = Id ,UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `manu_sfc_produce`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc_produce`  WHERE Id IN @ids ";
        const string GetSfcProduceBusinessBySFCIdSql = "SELECT * FROM manu_sfc_produce_business WHERE IsDeleted = 0 AND SfcInfoId = @SfcInfoId ";
        const string GetSfcProduceBusinessBySFCSql = @" SELECT SPB.* FROM manu_sfc_produce_business SPB  
 left join manu_sfc_info msi ON SPB.SfcInfoId = msi.Id 
 left join manu_sfc mf on mf.Id =msi.SfcId 
 left join manu_sfc_produce sfc on sfc.SFC =mf.SFC 
                            WHERE SPB.IsDeleted = 0 AND SPB.BusinessType = @BusinessType AND SFC.SFC = @Sfc ";
        const string GetSfcProduceBusinessBySFCsSql = @"SELECT SFC.Sfc,SPB.* FROM manu_sfc_produce_business SPB  
                            LEFT JOIN manu_sfc_info info ON SPB.SfcInfoId = info.Id 
							LEFT JOIN manu_sfc SFC ON info.SfcId = SFC.Id 
                            WHERE SPB.IsDeleted = 0 AND SPB.BusinessType = @BusinessType AND SFC.SFC IN @Sfcs ";
        const string GetSfcProduceBusinessBySFCIdsSql = "SELECT `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted` FROM manu_sfc_produce_business WHERE SfcInfoId IN @SfcInfoIds  AND IsDeleted=0";
        const string GetBySFCSql = @"SELECT * FROM manu_sfc_produce WHERE SFC = @sfc ";
        const string DeletePhysicalSql = "DELETE FROM manu_sfc_produce WHERE SFC = @sfc";
        const string DeletePhysicalRangeSql = "DELETE FROM manu_sfc_produce WHERE SFC in @Sfcs";
        const string DeleteSfcProduceBusinessBySfcInfoIdSql = "DELETE FROM manu_sfc_produce_business WHERE SiteId = @SiteId AND SfcInfoId = @SfcInfoId";
        const string RealDeletesSfcProduceBusinessSql = "DELETE FROM manu_sfc_produce_business WHERE SfcInfoId IN @SfcInfoIds AND BusinessType=@BusinessType";
        const string InsertOrUpdateSfcProduceBusinessSql = @"INSERT INTO `manu_sfc_produce_business`(  `Id`, `SiteId`, `SfcInfoId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcInfoId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted ) ON DUPLICATE KEY UPDATE
                                                             BusinessContent = @BusinessContent,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  ";
        //质量锁定sql
        const string UpdateQualityLockSql = "update  manu_sfc_produce set `Lock`=@Lock,LockProductionId=@LockProductionId,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn where SFC in  @Sfcs";
        const string UpdateIsScrapSql = "UPDATE `manu_sfc_produce` SET IsScrap = @IsScrap, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";

        //在制维修 
        const string UpdateStatusSql = "UPDATE `manu_sfc_produce` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateProcedureIdSql = "UPDATE `manu_sfc_produce` SET ProcedureId = @ProcedureId, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        //在制品步骤控制 
        const string UpdateProcedureAndStatusSql = "UPDATE `manu_sfc_produce` SET ProcedureId = @ProcedureId,Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";

    }
}
