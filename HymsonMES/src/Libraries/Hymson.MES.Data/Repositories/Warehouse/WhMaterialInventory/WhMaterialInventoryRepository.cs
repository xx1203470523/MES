/*
 *creator: Karl
 *
 *describe: 物料库存 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料库存仓储
    /// </summary>
    public partial class WhMaterialInventoryRepository : IWhMaterialInventoryRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public WhMaterialInventoryRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<WhMaterialInventoryEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<WhMaterialInventoryEntity>(GetByBarCodeSql, query);
        }

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarCodesQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<WhMaterialInventoryEntity>(GetByBarCodes, param);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
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
            sqlBuilder.Select(@" wmi.MaterialBarCode,wmi.Batch, wmi.QuantityResidue,wmi.DueDate,wmi.Source,wmi.CreatedOn,wmi.Status,
                                pm.Unit, pm.MaterialCode, pm.MaterialName, pm.Version, ws.Code as SupplierCode, ws.Name as SupplierName");
            sqlBuilder.LeftJoin(" wh_supplier ws ON  ws.Id= wmi.SupplierId");
            sqlBuilder.LeftJoin(" proc_material pm ON  pm.Id= wmi.MaterialId");
            sqlBuilder.Where(" wmi.IsDeleted = 0");
            sqlBuilder.OrderBy(" wmi.UpdatedOn DESC");
            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

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
                sqlBuilder.Where(" pm.Version=@Version");
            }
            if (whMaterialInventoryPagedQuery.Status > 0)
            {
                //Enum.GetValues(whMaterialInventoryPagedQuery.Status)
                sqlBuilder.Where(" wmi.Status=@Status");
            }

            var offSet = (whMaterialInventoryPagedQuery.PageIndex - 1) * whMaterialInventoryPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = whMaterialInventoryPagedQuery.PageSize });
            sqlBuilder.AddParameters(whMaterialInventoryPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
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
            //sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(whMaterialInventoryQuery.MaterialBarCode))
            {
                sqlBuilder.Where("MaterialBarCode=@MaterialBarCode");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
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
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, whMaterialInventoryEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhMaterialInventoryEntity whMaterialInventoryEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, whMaterialInventoryEntitys);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePointByBarCodeAsync(UpdateStatusByBarCodeCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpPointByBarCode, command);
        }

        /// <summary>
        /// 清空库存
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateWhMaterialInventoryEmptyByBarCodeAync(UpdateWhMaterialInventoryEmptyCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateWhMaterialInventoryEmptySql, command);
        }

        /// <summary>
        /// 更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<int> UpdateIncreaseQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateIncreaseQuantityResidueSql, updateQuantityCommand);
        }

        /// <summary>
        /// 批量更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<int> UpdateIncreaseQuantityResidueRangeAsync(IEnumerable<UpdateQuantityCommand> updateQuantityCommand)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateIncreaseQuantityResidueSql, updateQuantityCommand);
        }


        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateReduceQuantityResidueSql, updateQuantityCommand);
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

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var pmInfo = await conn.QueryFirstOrDefaultAsync<ProcMaterialInfoView>(template.RawSql, new { materialId });
            return pmInfo;
        }


        /// <summary>
        /// 根据物料编码获取供应商信息
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhSupplierInfoView>> GetWhSupplierByMaterialIdAsync(long materialId, long supplierId = 0)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetSupplierlByMaterialCodeSql);
            sqlBuilder.OrderBy("ws.UpdatedOn DESC");
            sqlBuilder.Select("ws.Id,ws.Code,ws.Name");
            sqlBuilder.InnerJoin("proc_material_supplier_relation pmsr ON pmsr.SupplierId=ws.Id");
            sqlBuilder.Where("pmsr.MaterialId=@materialId");
            if (supplierId > 0)
            {
                sqlBuilder.Where("ws.Id=@supplierId");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var wsInfo = await conn.QueryAsync<WhSupplierInfoView>(template.RawSql, new { materialId, supplierId });
            return wsInfo;
        }

    }

    public partial class WhMaterialInventoryRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `wh_material_inventory` wmi /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `wh_material_inventory` wmi /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/";
        const string GetWhMaterialInventoryEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `wh_material_inventory` /**where**/  ";

        const string InsertSql = "INSERT INTO `wh_material_inventory`(  `Id`, `SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `QuantityResidue`, `Status`, `DueDate`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @SupplierId, @MaterialId, @MaterialBarCode, @Batch, @QuantityResidue, @Status, @DueDate, @Source, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `wh_material_inventory`(  `Id`, `SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `QuantityResidue`, `Status`, `DueDate`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @SupplierId, @MaterialId, @MaterialBarCode, @Batch, @QuantityResidue, @Status, @DueDate, @Source, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `wh_material_inventory` SET   SupplierId = @SupplierId, MaterialId = @MaterialId, MaterialBarCode = @MaterialBarCode, Batch = @Batch, QuantityResidue = @QuantityResidue, Status = @Status, DueDate = @DueDate, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `wh_material_inventory` SET   SupplierId = @SupplierId, MaterialId = @MaterialId, MaterialBarCode = @MaterialBarCode, Batch = @Batch, QuantityResidue = @QuantityResidue, Status = @Status, DueDate = @DueDate, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpPointByBarCode = "UPDATE wh_material_inventory SET Status = @Status, QuantityResidue = @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";
        const string UpdateWhMaterialInventoryEmptySql = "UPDATE wh_material_inventory SET  QuantityResidue =0, UpdatedBy = @UserName, UpdatedOn = @UpdateTime WHERE MaterialBarCode IN @BarCodeList AND SiteId=@SiteId";
        const string DeleteSql = "UPDATE `wh_material_inventory` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `wh_material_inventory` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `QuantityResidue`, `Status`, `DueDate`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_material_inventory`  WHERE Id = @Id ";
        const string GetByBarCode = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND MaterialBarCode = @barCode";
        const string GetByBarCodeSql = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialBarCode = @BarCode";
        const string GetByBarCodes = "SELECT * FROM wh_material_inventory WHERE IsDeleted = 0 AND MaterialBarCode in @BarCodes AND SiteId=@SiteId";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SupplierId`, `MaterialId`, `MaterialBarCode`, `Batch`, `QuantityResidue`, `Status`, `DueDate`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_material_inventory`  WHERE Id IN @ids ";


        const string GetMaterialByMaterialCodeSql = @"SELECT   
                                            /**select**/
                                           FROM `proc_material` /**where**/  ";

        const string GetSupplierlByMaterialCodeSql = @"SELECT /**select**/ FROM wh_supplier ws /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/";

        const string UpdateIncreaseQuantityResidueSql = "UPDATE wh_material_inventory SET QuantityResidue =QuantityResidue+ @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";

        const string UpdateReduceQuantityResidueSql = "UPDATE wh_material_inventory SET QuantityResidue=QuantityResidue- @QuantityResidue, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE MaterialBarCode = @BarCode; ";
    }
}
