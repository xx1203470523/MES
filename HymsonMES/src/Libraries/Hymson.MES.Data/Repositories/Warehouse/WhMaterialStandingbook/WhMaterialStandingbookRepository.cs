using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料台账仓储
    /// </summary>
    public partial class WhMaterialStandingbookRepository : BaseRepository, IWhMaterialStandingbookRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public WhMaterialStandingbookRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
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
        public async Task<WhMaterialStandingbookEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhMaterialStandingbookEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialStandingbookEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialStandingbookEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialStandingbookEntity>> GetPagedInfoAsync(WhMaterialStandingbookPagedQuery whMaterialStandingbookPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy(" CreatedOn DESC");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(whMaterialStandingbookPagedQuery.Batch))
            {
                whMaterialStandingbookPagedQuery.Batch = whMaterialStandingbookPagedQuery.Batch;
                sqlBuilder.Where("Batch = @Batch");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialStandingbookPagedQuery.MaterialBarCode))
            {
                whMaterialStandingbookPagedQuery.MaterialBarCode = $"%{whMaterialStandingbookPagedQuery.MaterialBarCode}%";
                sqlBuilder.Where("MaterialBarCode like @MaterialBarCode");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialStandingbookPagedQuery.MaterialCode))
            {
                whMaterialStandingbookPagedQuery.MaterialCode = $"%{whMaterialStandingbookPagedQuery.MaterialCode}%";
                sqlBuilder.Where("MaterialCode like @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(whMaterialStandingbookPagedQuery.MaterialVersion))
            {
                whMaterialStandingbookPagedQuery.MaterialVersion = $"%{whMaterialStandingbookPagedQuery.MaterialVersion}%";
                sqlBuilder.Where("MaterialVersion like @MaterialVersion");
            }

            var offSet = (whMaterialStandingbookPagedQuery.PageIndex - 1) * whMaterialStandingbookPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = whMaterialStandingbookPagedQuery.PageSize });
            sqlBuilder.AddParameters(whMaterialStandingbookPagedQuery);

            using var conn = GetMESDbConnection();
            var whMaterialStandingbookEntitiesTask = conn.QueryAsync<WhMaterialStandingbookEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var whMaterialStandingbookEntities = await whMaterialStandingbookEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhMaterialStandingbookEntity>(whMaterialStandingbookEntities, whMaterialStandingbookPagedQuery.PageIndex, whMaterialStandingbookPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="whMaterialStandingbookQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialStandingbookEntity>> GetWhMaterialStandingbookEntitiesAsync(WhMaterialStandingbookQuery whMaterialStandingbookQuery)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy(" CreatedOn DESC");
            sqlBuilder.Where("SiteId=@SiteId");
            var template = sqlBuilder.AddTemplate(GetWhMaterialStandingbookEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var whMaterialStandingbookEntities = await conn.QueryAsync<WhMaterialStandingbookEntity>(template.RawSql, whMaterialStandingbookQuery);
            return whMaterialStandingbookEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialStandingbookEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhMaterialStandingbookEntity whMaterialStandingbookEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<WhMaterialStandingbookEntity>? whMaterialStandingbookEntitys)
        {
            if (whMaterialStandingbookEntitys == null || !whMaterialStandingbookEntitys.Any()) return 0;

            var (sql, param) = SqlHelper.JoinInsertSql(InsertsSqlInsert, InsertsSqlValue, whMaterialStandingbookEntitys);
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        /// <summary>
        /// 批量新增(拼接字符串方式,解决插入速度太慢问题)
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeByConcatSqlAsync(IEnumerable<WhMaterialStandingbookEntity> whMaterialStandingbookEntitys)
        {
            if (whMaterialStandingbookEntitys == null || !whMaterialStandingbookEntitys.Any()) return 0;

            var tempSql = $"INSERT INTO `wh_material_standingbook`(`Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES ";

            DynamicParameters parameters = new();
            for (int i = 0; i < whMaterialStandingbookEntitys.Count(); i++)
            {
                tempSql += $"(@Id{i}, @MaterialCode{i}, @MaterialName{i}, @MaterialVersion{i}, @MaterialBarCode{i}, @Batch{i}, @Quantity{i}, @Unit{i}, @SupplierId{i}, @Type{i}, @Source{i}, @CreatedBy{i}, @CreatedOn{i}, @UpdatedBy{i}, @UpdatedOn{i}, @IsDeleted{i}, @SiteId{i}),";

                var item = whMaterialStandingbookEntitys.ElementAt(i);
                parameters.Add($"@Id{i}", item.Id);
                parameters.Add($"@MaterialCode{i}", item.MaterialCode);
                parameters.Add($"@MaterialName{i}", item.MaterialName);
                parameters.Add($"@MaterialVersion{i}", item.MaterialVersion);
                parameters.Add($"@MaterialBarCode{i}", item.MaterialBarCode);
                parameters.Add($"@Batch{i}", item.Batch);
                parameters.Add($"@Quantity{i}", item.Quantity);
                parameters.Add($"@Unit{i}", item.Unit);
                parameters.Add($"@SupplierId{i}", item.SupplierId);
                parameters.Add($"@Type{i}", item.Type);
                parameters.Add($"@Source{i}", item.Source);
                parameters.Add($"@CreatedBy{i}", item.CreatedBy);
                parameters.Add($"@CreatedOn{i}", item.CreatedOn);
                parameters.Add($"@UpdatedBy{i}", item.UpdatedBy);
                parameters.Add($"@UpdatedOn{i}", item.UpdatedOn);
                parameters.Add($"@IsDeleted{i}", item.IsDeleted);
                parameters.Add($"@SiteId{i}", item.SiteId);
            }
            tempSql = tempSql.Remove(tempSql.Length - 1);
            tempSql += ";";

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(tempSql, parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialStandingbookEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhMaterialStandingbookEntity whMaterialStandingbookEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="whMaterialStandingbookEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<WhMaterialStandingbookEntity> whMaterialStandingbookEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, whMaterialStandingbookEntitys);
        }

    }

    public partial class WhMaterialStandingbookRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `wh_material_standingbook`  /**innerjoin**/ /**leftjoin**/ /**where**/  /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `wh_material_standingbook`  /**where**/ ";
        const string GetWhMaterialStandingbookEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `wh_material_standingbook` /**where**/  ";

        const string InsertSql = "INSERT INTO `wh_material_standingbook`(  `Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @MaterialCode, @MaterialName, @MaterialVersion, @MaterialBarCode, @Batch, @Quantity, @Unit, @SupplierId, @Type, @Source, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `wh_material_standingbook`(  `Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @MaterialCode, @MaterialName, @MaterialVersion, @MaterialBarCode, @Batch, @Quantity, @Unit, @SupplierId, @Type, @Source, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSqlInsert = "INSERT INTO `wh_material_standingbook`(  `Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES ";
        const string InsertsSqlValue = "( @Id, @MaterialCode, @MaterialName, @MaterialVersion, @MaterialBarCode, @Batch, @Quantity, @Unit, @SupplierId, @Type, @Source, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `wh_material_standingbook` SET   MaterialCode = @MaterialCode, MaterialName = @MaterialName, MaterialVersion = @MaterialVersion, MaterialBarCode = @MaterialBarCode, Batch = @Batch, Quantity = @Quantity, Unit = @Unit, Type = @Type, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `wh_material_standingbook` SET   MaterialCode = @MaterialCode, MaterialName = @MaterialName, MaterialVersion = @MaterialVersion, MaterialBarCode = @MaterialBarCode, Batch = @Batch, Quantity = @Quantity, Unit = @Unit, Type = @Type, Source = @Source, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `wh_material_standingbook` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `wh_material_standingbook` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_material_standingbook`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `MaterialCode`, `MaterialName`, `MaterialVersion`, `MaterialBarCode`, `Batch`, `Quantity`, `Unit`, `SupplierId`, `Type`, `Source`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `wh_material_standingbook`  WHERE Id IN @ids ";
    }
}
