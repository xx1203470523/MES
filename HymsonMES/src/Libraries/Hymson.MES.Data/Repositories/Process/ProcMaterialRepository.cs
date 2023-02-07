/*
 *creator: Karl
 *
 *describe: 物料维护 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
	/// <summary>
    /// 物料维护仓储
    /// </summary>
    public partial class ProcMaterialRepository : IProcMaterialRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcMaterialRepository(IOptions<ConnectionOptions> connectionOptions)
		{
			_connectionOptions = connectionOptions.Value;
		}

		public async Task<int> DeleteAsync(long id)
        {
			using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
			return await conn.ExecuteAsync(DeleteSql);
        }

        public async Task<ProcMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetByIdSql);
        }

        public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcMaterialEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialEntities = await conn.QueryAsync<ProcMaterialEntity>(template.RawSql, procMaterialQuery);
            return procMaterialEntities;
        }

        public async Task InsertAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, procMaterialEntity);
            procMaterialEntity.Id = id;
        }

        public async Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procMaterialEntity);
        }
    }

    public partial class ProcMaterialRepository
    {
        const string DeleteSql = "";
        const string GetByIdSql = "";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `wh_stock_change_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `wh_stock_change_record` /**where**/";
        const string GetProcMaterialEntitiesSqlTemplate = "";
        const string InsertSql = "INSERT INTO `wh_stock_change_record`(`Id`, `SiteCode`, `ChangeType`, `SourceNo`, `SourceItem`, `SourceBillType`, `LabelId`, `MaterialId`, `MaterialCode`, `ProjectNo`, `StockManageFeature`, `OldMaterialCode`, `BatchNo`, `Sn`, `Unit`, `Quantity`, `WarehouseId`, `WarehouseAreaId`, `WarehouseRackId`, `WarehouseBinId`, `ContainerId`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `BeforeStockManageFeature`) VALUES (@Id, @SiteCode, @ChangeType, @SourceNo, @SourceItem, @SourceBillType, @LabelId, @MaterialId, @MaterialCode, @ProjectNo, @StockManageFeature, @OldMaterialCode, @BatchNo, @Sn, @Unit, @Quantity, @WarehouseId, @WarehouseAreaId, @WarehouseRackId, @WarehouseBinId, @ContainerId, @Remark, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted, @BeforeStockManageFeature);";
        const string UpdateSql = "";
    }
}
