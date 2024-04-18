using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap.Command;
using Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap.View;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap
{
    /// <summary>
    /// 物料报废仓储
    /// </summary>
    public partial class WhMaterialInventoryScrapRepository : BaseRepository, IWhMaterialInventoryScrapRepository
    {
        public WhMaterialInventoryScrapRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryScrapEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(IEnumerable<WhMaterialInventoryScrapEntity> whMaterialInventoryScrapEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryScrapEntity);
        }
        
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="whMaterialInventoryScrapEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertSingleAsync(WhMaterialInventoryScrapEntity whMaterialInventoryScrapEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryScrapEntity);
        }
        /// <summary>
        /// 批量更新物料报废是否取消状态
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<int> UpdateIncreaseQuantityResidueRangeAsync(IEnumerable<UpdateCancellationCommand> updateCancellationCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateInventoryScrapCancellationSql, updateCancellationCommand);
        }

        /// <summary>
        /// 根据物料条码查询物料报废信息
        /// </summary>
        /// <param name="bomIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryScrapView>> GetMaterialInventoryScrapByMaterialCodeAsync(MaterialScrappingCommand command)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetByMaterialCodeSql);
            sqlBuilder.Select("quc.UnqualifiedCode,quc.UnqualifiedCodeName as UnqualifiedName,wmis.Remark,wmis.SupplierId,wmis.MaterialId,wmis.ProcedureId,wmis.WorkOrderId,wmis.ScrapType,pm.Unit,wmis.Batch,pm.Version,wmis.id as InventoryScrapId, wmis.MaterialBarCode,pm.MaterialCode,pm.MaterialName,pwo.OrderCode,pp.Name as ProcedureName,iwc.Name as WorkCenterName,wmis.ScrapQty as ScrapQuantity,pwo.ProductId");
            sqlBuilder.LeftJoin("plan_work_order pwo on wmis.WorkOrderId =pwo.Id and pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_material pm on wmis.MaterialId =pm.Id and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on wmis.ProcedureId =pp.Id and pp.IsDeleted =0");
            sqlBuilder.LeftJoin("inte_work_center iwc on pwo.WorkCenterId =iwc.Id and iwc.IsDeleted=0 ");
            sqlBuilder.LeftJoin("wh_material_inventory_scrap_ng_relation wmisnr on wmis.id =wmisnr.MaterialInventoryScrapId and wmisnr.IsDeleted=0 ");
            sqlBuilder.LeftJoin("qual_unqualified_code quc on wmisnr.UnqualifiedCodeId =quc.id and quc.IsDeleted=0 ");

            if (!string.IsNullOrWhiteSpace(command.MaterialBarCode))
            {
                sqlBuilder.Where("wmis.MaterialBarCode=@MaterialBarCode");
            }
            sqlBuilder.Where("wmis.SiteId=@SiteId");
            sqlBuilder.Where("wmis.IsCancellation=0");
            sqlBuilder.Where("wmis.IsDeleted=0");
            using var conn = GetMESDbConnection();
            var pmInfo = await conn.QueryAsync<WhMaterialInventoryScrapView>(template.RawSql, command);
            return pmInfo;
        }
        #endregion

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialInventoryScrapEntity>> GetPagedInfoAsync(WhMaterialInventoryScrapPagedQuery dto)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wmis.IsDeleted=0");
            sqlBuilder.Where("wmis.SiteId=@SiteId");
            sqlBuilder.Select("wmis.*");
            if (dto.IsCancellation.HasValue)
            {
                sqlBuilder.Where("wmis.IsCancellation=@IsCancellation");
            }
            if (!string.IsNullOrWhiteSpace(dto.OrderCode))
            {
                sqlBuilder.LeftJoin(" plan_work_order pwo ON wmis.WorkOrderId=pwo.Id");
                sqlBuilder.Where("pwo.OrderCode=@OrderCode");
            }
            if (!string.IsNullOrWhiteSpace(dto.MaterialCode))
            {
                sqlBuilder.LeftJoin(" proc_material pm ON wmis.MaterialId=pm.Id");
                sqlBuilder.Where("pm.MaterialCode=@MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(dto.MaterialBarCode))
            {
                sqlBuilder.Where("wmis.MaterialBarCode=@MaterialBarCode");
            }
            if (!string.IsNullOrWhiteSpace(dto.ProcedureCode))
            {
                sqlBuilder.LeftJoin(" proc_procedure pp ON wmis.ProcedureId=pp.Id");
                sqlBuilder.Where(" pp.Code=@ProcedureCode");
            }
            var offSet = (dto.PageIndex - 1) * dto.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = dto.PageSize });
            sqlBuilder.AddParameters(dto);

            using var conn = GetMESDbConnection();
            var manuSfcInfo1EntitiesTask = conn.QueryAsync<WhMaterialInventoryScrapEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcInfo1Entities = await manuSfcInfo1EntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhMaterialInventoryScrapEntity>(manuSfcInfo1Entities, dto.PageIndex, dto.PageSize, totalCount);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryScrapEntity>> GetByMaterialIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialInventoryScrapEntity>(GetByMaterialIdsSql, new { ids });
        }
    }

    public partial class WhMaterialInventoryScrapRepository
    {
        #region 
        const string InsertSql = "INSERT INTO `wh_material_inventory_scrap`(  `Id`, `SiteId`,`SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `MaterialStandingbookId`, `ScrapQty`, `IsCancellation`, `ProcedureId`, `WorkOrderId`, `ScrapType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,`UnqualifiedId`) VALUES (   @Id, @SiteId, @SupplierId, @MaterialId, @MaterialBarCode, @Batch, @MaterialStandingbookId, @ScrapQty, @IsCancellation, @ProcedureId, @WorkOrderId,@ScrapType,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted,@UnqualifiedId)  ";
        const string GetByMaterialCodeSql = @"SELECT /**select**/ FROM wh_material_inventory_scrap wmis /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string UpdateInventoryScrapCancellationSql = "UPDATE wh_material_inventory_scrap SET IsCancellation=@IsCancellation,CancelMaterialStandingbookId=@CancelMaterialStandingbookId WHERE id = @InventoryScrapId; ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `wh_material_inventory_scrap` wmis /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY  CreatedOn  DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) FROM `wh_material_inventory_scrap` wmis /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetByMaterialIdsSql = @"SELECT * FROM `wh_material_inventory_scrap`
                            WHERE MaterialId IN @ids and IsDeleted=0 ";
        #endregion
    }
}