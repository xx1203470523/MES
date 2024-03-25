using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料库存仓储
    /// </summary>
    public partial class WhMaterialInventoryRepository : BaseRepository, IWhMaterialInventoryRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public WhMaterialInventoryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhMaterialInventoryEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhMaterialInventoryEntity>(GetByBarCodeSql, query);
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarCodesQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialInventoryEntity>(GetByBarCodesSql, param);
        }

        /// <summary>
        /// 根据物料条码获取数据（剩余数量大于0的条码）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesOfHasQtyAsync(WhMaterialInventoryBarCodesQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialInventoryEntity>(GetByBarCodesOfHasQty, param);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialInventoryEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whMaterialInventoryPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialInventoryPageListView>> GetPagedInfoAsync(WhMaterialInventoryPagedQuery whMaterialInventoryPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select(@" wmi.Id, wmi.MaterialBarCode,wmi.Batch, wmi.QuantityResidue,wmi.DueDate,wmi.Source,wmi.CreatedOn,wmi.Status,
                                pm.Unit, pm.MaterialCode, pm.MaterialName, pm.Version, ws.Code as SupplierCode, ws.Name as SupplierName");
            sqlBuilder.LeftJoin(" wh_supplier ws ON  ws.Id= wmi.SupplierId");
            sqlBuilder.LeftJoin(" proc_material pm ON  pm.Id= wmi.MaterialId");
            sqlBuilder.Where(" wmi.IsDeleted = 0");
            sqlBuilder.Where(" wmi.SiteId=@SiteId");
            sqlBuilder.OrderBy(" wmi.UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(whMaterialInventoryPagedQuery.Batch))
            {
                whMaterialInventoryPagedQuery.Batch = $"%{whMaterialInventoryPagedQuery.Batch}%";
                sqlBuilder.Where(" wmi.Batch like @Batch");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialInventoryPagedQuery.MaterialBarCode))
            {
                whMaterialInventoryPagedQuery.MaterialBarCode = $"%{whMaterialInventoryPagedQuery.MaterialBarCode}%";
                sqlBuilder.Where(" wmi.MaterialBarCode like @MaterialBarCode");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialInventoryPagedQuery.MaterialCode))
            {
                whMaterialInventoryPagedQuery.MaterialCode = $"%{whMaterialInventoryPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" pm.MaterialCode like @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialInventoryPagedQuery.Version))
            {
                whMaterialInventoryPagedQuery.Version = $"%{whMaterialInventoryPagedQuery.Version}%";
                sqlBuilder.Where(" pm.Version like @Version");
            }
            if (whMaterialInventoryPagedQuery.Status > 0)
            {
                //Enum.GetValues(whMaterialInventoryPagedQuery.Status)
                sqlBuilder.Where(" wmi.Status=@Status");
            }
            if (whMaterialInventoryPagedQuery.CreatedOnRange != null && whMaterialInventoryPagedQuery.CreatedOnRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = whMaterialInventoryPagedQuery.CreatedOnRange[0], CreatedOnEnd = whMaterialInventoryPagedQuery.CreatedOnRange[1].AddDays(1) });
                sqlBuilder.Where("wmi.CreatedOn >= @CreatedOnStart AND wmi.CreatedOn < @CreatedOnEnd");
            }
            if (whMaterialInventoryPagedQuery.Sources != null && whMaterialInventoryPagedQuery.Sources.Length > 0)
            {
                sqlBuilder.Where("wmi.Source in @Sources");
            }

            var offSet = (whMaterialInventoryPagedQuery.PageIndex - 1) * whMaterialInventoryPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = whMaterialInventoryPagedQuery.PageSize });
            sqlBuilder.AddParameters(whMaterialInventoryPagedQuery);

            using var conn = GetMESDbConnection();
            var whMaterialInventoryEntitiesTask = conn.QueryAsync<WhMaterialInventoryPageListView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var whMaterialInventoryEntities = await whMaterialInventoryEntitiesTask;
            var totalCount = await totalCountTask;
            var pageList = new PagedInfo<WhMaterialInventoryPageListView>(whMaterialInventoryEntities, whMaterialInventoryPagedQuery.PageIndex, whMaterialInventoryPagedQuery.PageSize, totalCount);
            return pageList;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="whMaterialInventoryQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetWhMaterialInventoryEntitiesAsync(WhMaterialInventoryQuery whMaterialInventoryQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetWhMaterialInventoryEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(whMaterialInventoryQuery.MaterialBarCode))
            {
                sqlBuilder.Where("MaterialBarCode=@MaterialBarCode");
            }

            using var conn = GetMESDbConnection();
            var whMaterialInventoryEntities = await conn.QueryAsync<WhMaterialInventoryEntity>(template.RawSql, whMaterialInventoryQuery);
            return whMaterialInventoryEntities;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhMaterialInventoryEntity whMaterialInventoryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<WhMaterialInventoryEntity>? whMaterialInventoryEntitys)
        {
            if (whMaterialInventoryEntitys == null || !whMaterialInventoryEntitys.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhMaterialInventoryEntity whMaterialInventoryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, whMaterialInventoryEntitys);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePointByBarCodeAsync(UpdateStatusByBarCodeCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpPointByBarCode, command);
        }

        /// <summary>
        /// 批量更新更新状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdatePointByBarCodeRangeAsync(IEnumerable<UpdateStatusByBarCodeCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpPointByBarCode, commands);
        }

        /// <summary>
        /// 更新状态（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePointByBarCodesAsync(IEnumerable<UpdateStatusByBarCodeCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpPointByBarCode, commands);
        }

        /// <summary>
        /// 清空库存
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateWhMaterialInventoryEmptyByBarCodeAync(UpdateWhMaterialInventoryEmptyCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWhMaterialInventoryEmptySql, command);
        }

        /// <summary>
        /// 批量清空库存(根据id)
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateWhMaterialInventoryEmptyByIdRangeAync(IEnumerable<UpdateWhMaterialInventoryEmptyByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWhMaterialInventoryEmptyByIdSql, commands);
        }

        /// <summary>
        /// 清空库存(根据id)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateWhMaterialInventoryEmptyByIdAync(UpdateWhMaterialInventoryEmptyByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWhMaterialInventoryEmptyByIdSql, command);
        }

        /// <summary>
        /// 更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<int> UpdateIncreaseQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateIncreaseQuantityResidueSql, updateQuantityCommand);
        }

        /// <summary>
        /// 批量更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<int> UpdateIncreaseQuantityResidueRangeAsync(IEnumerable<UpdateQuantityRangeCommand> updateQuantityCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateIncreaseQuantityResidueRangeSql, updateQuantityCommand);
        }


        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateReduceQuantityResidueSql, updateQuantityCommand);
        }

        /// <summary>
        /// 更新库存数量(减少库存)-带库存检查
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateReduceQuantityResidueWithCheckAsync(UpdateQuantityCommand updateQuantityCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateReduceQuantityResidueWithCheckSql, updateQuantityCommand);
        }

        /// <summary>
        /// 批量更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateReduceQuantityResidueRangeAsync(IEnumerable<UpdateQuantityRangeCommand> updateQuantityCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateReduceQuantityResidueRangeSql, updateQuantityCommand);
        }

        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public async Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityRangeCommand Command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateReduceQuantityResidueRangeSql, Command);
        }

        /// <summary>
        /// 根据物料编码获取物料数据
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialInfoView> GetProcMaterialByMaterialCodeAsync(long materialId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetMaterialByMaterialCodeSql);
            sqlBuilder.Select("*");
            sqlBuilder.Where("Id=@materialId");

            using var conn = GetMESDbConnection();
            var pmInfo = await conn.QueryFirstOrDefaultAsync<ProcMaterialInfoView>(template.RawSql, new { materialId });
            return pmInfo;
        }


        /// <summary>
        /// 根据物料编码获取供应商信息
        /// </summary>
        /// <param name="WhSupplierByMaterialCommand"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhSupplierInfoView>> GetWhSupplierByMaterialIdAsync(WhSupplierByMaterialCommand command)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetSupplierlByMaterialCodeSql);
            sqlBuilder.OrderBy("ws.UpdatedOn DESC");
            sqlBuilder.Select("ws.Id,ws.Code,ws.Name");
            sqlBuilder.InnerJoin("proc_material_supplier_relation pmsr ON pmsr.SupplierId=ws.Id");
            sqlBuilder.Where("pmsr.MaterialId=@materialId");
            sqlBuilder.Where("ws.SiteId=@SiteId");

            if (command.SupplierId > 0)
            {
                sqlBuilder.Where("ws.Id=@supplierId");
            }

            using var conn = GetMESDbConnection();
            var wsInfo = await conn.QueryAsync<WhSupplierInfoView>(template.RawSql, command);
            return wsInfo;
        }


        /// <summary>
        /// 修改外部来源库存
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateOutsideWhMaterilInventoryAsync(WhMaterialInventoryEntity whMaterialInventoryEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateOutsideWhMaterilInventorySql, whMaterialInventoryEntity);
        }

        /// <summary>
        /// 根据条码修改库存数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateQuantityResidueBySfcsAsync(UpdateQuantityResidueBySfcsCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQuantityResidueBySfcsSql, command);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class WhMaterialInventoryRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `wh_material_inventory` wmi /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `wh_material_inventory` wmi /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/";
        const string GetWhMaterialInventoryEntitiesSqlTemplate = @"SELECT /**select**/ FROM `wh_material_inventory` /**where**/  ";

        const string InsertSql = "INSERT INTO `wh_material_inventory`(`Id`, `SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `QuantityResidue`, `ReceivedQty`, `Status`, `DueDate`, `Source`, `MaterialType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, WorkOrderId) VALUES (@Id, @SupplierId, @MaterialId, @MaterialBarCode, @Batch, @QuantityResidue, @QuantityResidue, @Status, @DueDate, @Source,@MaterialType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkOrderId) ON DUPLICATE KEY UPDATE UpdatedOn = NOW()  ";
        const string UpdateSql = "UPDATE `wh_material_inventory` SET SupplierId = @SupplierId, MaterialId = @MaterialId, MaterialBarCode = @MaterialBarCode, Batch = @Batch, QuantityResidue = @QuantityResidue, Status = @Status, DueDate = @DueDate, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `wh_material_inventory` SET SupplierId = @SupplierId, MaterialId = @MaterialId, MaterialBarCode = @MaterialBarCode, Batch = @Batch, QuantityResidue = @QuantityResidue, Status = @Status, DueDate = @DueDate, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpPointByBarCode = "UPDATE wh_material_inventory SET Status = @Status, QuantityResidue = @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";
        const string UpdateWhMaterialInventoryEmptySql = "UPDATE wh_material_inventory SET  QuantityResidue =0, UpdatedBy = @UserName, UpdatedOn = @UpdateTime WHERE MaterialBarCode IN @BarCodeList AND SiteId=@SiteId";
        const string UpdateWhMaterialInventoryEmptyByIdSql = "UPDATE wh_material_inventory SET  QuantityResidue =0, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `wh_material_inventory` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `wh_material_inventory` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `wh_material_inventory`  WHERE Id = @Id ";

        const string GetByBarCodeSql = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialBarCode = @BarCode";
        const string GetByBarCodesSql = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialBarCode IN @BarCodes AND QuantityResidue > 0";
        const string GetByBarCodesOfHasQty = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialBarCode IN @BarCodes AND QuantityResidue > 0";
        const string GetByIdsSql = @"SELECT * FROM `wh_material_inventory` WHERE Id IN @ids ";


        const string GetMaterialByMaterialCodeSql = @"SELECT   
                                            /**select**/
                                           FROM `proc_material` /**where**/  ";

        const string GetSupplierlByMaterialCodeSql = @"SELECT /**select**/ FROM wh_supplier ws /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/";

        const string UpdateIncreaseQuantityResidueSql = "UPDATE wh_material_inventory SET QuantityResidue =QuantityResidue+ @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";

        const string UpdateReduceQuantityResidueSql = "UPDATE wh_material_inventory SET QuantityResidue = QuantityResidue - @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";
        const string UpdateReduceQuantityResidueWithCheckSql = "UPDATE wh_material_inventory SET QuantityResidue = QuantityResidue - @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode AND QuantityResidue = @QuantityOriginal; ";

        const string UpdateIncreaseQuantityResidueRangeSql = "UPDATE wh_material_inventory SET QuantityResidue =QuantityResidue+ @QuantityResidue, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";
        const string UpdateReduceQuantityResidueRangeSql = "UPDATE wh_material_inventory SET QuantityResidue=QuantityResidue- @QuantityResidue, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";

        const string UpdateOutsideWhMaterilInventorySql = "UPDATE wh_material_inventory SET  MaterialId=@MaterialId, QuantityResidue =@QuantityResidue, Batch=@Batch, SupplierId=@SupplierId,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id; ";

        const string UpdateQuantityResidueBySfcsSql = "UPDATE wh_material_inventory SET QuantityResidue = @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE SiteId=@SiteId AND MaterialBarCode IN @Sfcs AND QuantityResidue >0  ";
    }
}
