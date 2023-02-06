using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.OnStock
{
    public partial class WhStockChangeRecordRepository: IWhStockChangeRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public WhStockChangeRecordRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql);
        }

        public async Task<WhStockChangeRecordEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<WhStockChangeRecordEntity>(GetByIdSql);
        }

        public async Task<PagedInfo<WhStockChangeRecordEntity>> GetPagedInfoAsync(WhStockChangeRecordPagedQuery whStockChangeRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(whStockChangeRecordPagedQuery.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(whStockChangeRecordPagedQuery.SourceNo))
            {
                sqlBuilder.Where("SourceNo=@SourceNo");
            }
            if (whStockChangeRecordPagedQuery.ChangeType.HasValue)
            {
                sqlBuilder.Where("ChangeType=@ChangeType");
            }
            var offSet = (whStockChangeRecordPagedQuery.PageIndex - 1) * whStockChangeRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = whStockChangeRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(whStockChangeRecordPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var whStockChangeRecordEntitiesTask =   conn.QueryAsync<WhStockChangeRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask =   conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var whStockChangeRecordEntities = await whStockChangeRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhStockChangeRecordEntity>(whStockChangeRecordEntities, whStockChangeRecordPagedQuery.PageIndex, whStockChangeRecordPagedQuery.PageSize, totalCount);
        }

        public async Task<IEnumerable<WhStockChangeRecordEntity>> GetWhStockChangeRecordEntitiesAsync(WhStockChangeRecordQuery whStockChangeRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetWhStockChangeRecordEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var whStockChangeRecordEntities = await conn.QueryAsync<WhStockChangeRecordEntity>(template.RawSql, whStockChangeRecordQuery);
            return whStockChangeRecordEntities;
        }

        public async Task InsertAsync(WhStockChangeRecordEntity whStockChangeRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, whStockChangeRecordEntity);
            whStockChangeRecordEntity.Id = id;
        }

        public async Task<int> UpdateAsync(WhStockChangeRecordEntity whStockChangeRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, whStockChangeRecordEntity);
        }
    }

    public partial class WhStockChangeRecordRepository {

        const string DeleteSql = "";
        const string GetByIdSql = "";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `wh_stock_change_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `wh_stock_change_record` /**where**/";
        const string GetWhStockChangeRecordEntitiesSqlTemplate = "";
        const string InsertSql = "INSERT INTO `wh_stock_change_record`(`Id`, `SiteCode`, `ChangeType`, `SourceNo`, `SourceItem`, `SourceBillType`, `LabelId`, `MaterialId`, `MaterialCode`, `ProjectNo`, `StockManageFeature`, `OldMaterialCode`, `BatchNo`, `Sn`, `Unit`, `Quantity`, `WarehouseId`, `WarehouseAreaId`, `WarehouseRackId`, `WarehouseBinId`, `ContainerId`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `BeforeStockManageFeature`) VALUES (@Id, @SiteCode, @ChangeType, @SourceNo, @SourceItem, @SourceBillType, @LabelId, @MaterialId, @MaterialCode, @ProjectNo, @StockManageFeature, @OldMaterialCode, @BatchNo, @Sn, @Unit, @Quantity, @WarehouseId, @WarehouseAreaId, @WarehouseRackId, @WarehouseBinId, @ContainerId, @Remark, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted, @BeforeStockManageFeature);";
        const string UpdateSql = "";
    }
}
