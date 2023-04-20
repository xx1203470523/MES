using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表仓储
    /// </summary>
    public partial class ManuSfcRepository : BaseRepository, IManuSfcRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSfcRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        #region 方法
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
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcPassDownView>> GetPagedListAsync(ManuSfcPassDownPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("manu_sfc_info MSI ON MSI.SfcId = MS.Id");
            sqlBuilder.LeftJoin("plan_work_order PWO ON PWO.Id = MSI.WorkOrderId");
            sqlBuilder.LeftJoin("proc_material PM ON PM.Id = MSI.ProductId");
            sqlBuilder.Where("MS.IsDeleted = 0");
            sqlBuilder.Where("MS.SiteId = @SiteId");
            sqlBuilder.OrderBy("MS.UpdatedOn DESC");
            sqlBuilder.Select("MS.Id, MS.SFC, MS.IsUsed, MS.UpdatedOn, PWO.OrderCode, PM.MaterialCode, PM.MaterialName");

            if (pagedQuery.IsUsed.HasValue)
            {
                sqlBuilder.Where("MS.IsUsed = @IsUsed");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkOrderCode))
            {
                pagedQuery.WorkOrderCode = $"%{pagedQuery.WorkOrderCode}%";
                sqlBuilder.Where("PWO.OrderCode LIKE @WorkOrderCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("PM.MaterialCode LIKE @MaterialCode");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<ManuSfcPassDownView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<ManuSfcPassDownView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(ManuSfcQuery manuSfcQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcEntities = await conn.QueryAsync<ManuSfcEntity>(template.RawSql, manuSfcQuery);
            return manuSfcEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ManuSfcEntity> manuSfcEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ManuSfcEntity> manuSfcEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcEntitys);
        }

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcView>> GetManuSfcInfoEntitiesAsync(ManuSfcStatusQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfoEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            if (param.Sfcs != null && param.Sfcs.Any())
            {
                sqlBuilder.Where("sfc.SFC in @Sfcs");
            }
            if (param.Status.HasValue)
            {
                sqlBuilder.Where("sfc.Status =@Status");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var list = await conn.QueryAsync<ManuSfcView>(template.RawSql, param);
            return list;
        }

        /// <summary>
        /// 批量更新条码状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ManuSfcUpdateCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetBySFCAsync(string sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetBySFCSql, new { SFC = sfc });
        }

        /// <summary>
        /// 更具sfc 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(IEnumerable<string> sfcs)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetBySFCSql, new { SFC = sfcs });
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_sfc MS /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc MS /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetManuSfcEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc` /**where**/  ";
        const string GetManuSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            sfc.Id ,sfc.SiteId ,sfc.SFC ,sfc.Qty ,sfc.Status ,info.WorkOrderId ,info.ProductId ,info.IsUsed  FROM manu_sfc sfc LEFT JOIN  manu_sfc_info info on sfc.Id =info.SfcId  and info.IsUsed =1
                                            /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";

        const string DeleteSql = "UPDATE `manu_sfc` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `manu_sfc`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc`  WHERE Id IN @Ids ";

        const string GetBySFCSql = @"SELECT * FROM `manu_sfc`  WHERE SFC = @SFC ";
        const string GetBySFCsSql = @"SELECT * FROM `manu_sfc`  WHERE SFC IN @SFCs AND IsDeleted=0 ";
    }
}
