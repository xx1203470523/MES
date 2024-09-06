using Dapper;
using Hymson.MES.BackgroundServices.Stator;

namespace Hymson.MES.Data.Repositories.Stator
{
    /// <summary>
    /// 仓储（定子条码表）
    /// </summary>
    public partial class StatorBarCodeRepository : BaseRepository, IStatorBarCodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public StatorBarCodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<StatorBarCodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<StatorBarCodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StatorBarCodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<StatorBarCodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatorBarCodeEntity>> GetEntitiesAsync(StatorBarCodeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.InnerIds != null && query.InnerIds.Any())
            {
                sqlBuilder.Where("InnerId IN @InnerIds");
            }

            if (query.InnerBarCodes != null && query.InnerBarCodes.Any())
            {
                sqlBuilder.Where("InnerBarCode IN @InnerBarCodes");
            }

            if (query.OuterBarCodes != null && query.OuterBarCodes.Any())
            {
                sqlBuilder.Where("OuterBarCode IN @OuterBarCodes");
            }

            if (query.BusBarCodes != null && query.BusBarCodes.Any())
            {
                sqlBuilder.Where("BusBarCode IN @BusBarCodes");
            }

            if (query.ProductionCodes != null && query.ProductionCodes.Any())
            {
                sqlBuilder.Where("ProductionCode IN @ProductionCodes");
            }

            if (query.WireBarCodes != null && query.WireBarCodes.Any())
            {
                sqlBuilder.Where("WireBarCode IN @WireBarCodes");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<StatorBarCodeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List（未赋值的列）
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetInnerIdsByNullColumnAsync(string columnName)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("InnerId");

            if (columnName != null && !string.IsNullOrWhiteSpace(columnName))
            {
                sqlBuilder.Where($"`{columnName}` IS NULL");
            }

            // 只读取创建时间两个月内的
            sqlBuilder.Where($"CreatedOn >= (CURDATE() - INTERVAL {StatorConst.MONTHLIMIT} MONTH)");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<long>(template.RawSql);
        }

        /// <summary>
        /// 查询List（已赋值的列）
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetInnerIdsByNonNullColumnAsync(string columnName)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("InnerId");

            if (columnName != null && !string.IsNullOrWhiteSpace(columnName))
            {
                sqlBuilder.Where($"`{columnName}` IS NOT NULL");
            }

            // 只读取创建时间两个月内的
            sqlBuilder.Where($"CreatedOn >= (CURDATE() - INTERVAL {StatorConst.MONTHLIMIT} MONTH)");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<long>(template.RawSql);
        }

    }


    /// <summary>
    /// 定子条码表
    /// </summary>
    public partial class StatorBarCodeRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_stator_barcode /**where**/  ";

        const string InsertSql = "INSERT INTO manu_stator_barcode(ID, InnerId, InnerBarCode, OuterBarCode, BusBarCode, PaperBottomLotBarcode, PaperTopLotBarcode, ProductionCode, Remark, CreatedOn, UpdatedOn, SiteId) VALUES (@ID, @InnerId, @InnerBarCode, @OuterBarCode, @BusBarCode, @PaperBottomLotBarcode, @PaperTopLotBarcode, @ProductionCode, @Remark, @CreatedOn, @UpdatedOn, @SiteId) ";

        const string UpdateSql = "UPDATE manu_stator_barcode SET OuterBarCode = @OuterBarCode, BusBarCode = @BusBarCode, PaperBottomLotBarcode = @PaperBottomLotBarcode, PaperTopLotBarcode = @PaperTopLotBarcode, ProductionCode = @ProductionCode, Remark = @Remark, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string GetByIdSql = @"SELECT * FROM manu_stator_barcode WHERE Id = @Id ";

    }
}
