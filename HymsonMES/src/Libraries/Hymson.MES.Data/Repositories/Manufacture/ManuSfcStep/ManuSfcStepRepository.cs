using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表仓储
    /// </summary>
    public partial class ManuSfcStepRepository : BaseRepository, IManuSfcStepRepository
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcStepRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcStepEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcStepEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoAsync(ManuSfcStepPagedQuery manuSfcStepPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("ProcedureId,CreatedOn");

            if (!string.IsNullOrWhiteSpace(manuSfcStepPagedQuery.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }

            var offSet = (manuSfcStepPagedQuery.PageIndex - 1) * manuSfcStepPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcStepPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcStepPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcStepEntitiesTask = conn.QueryAsync<ManuSfcStepEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepEntities = await manuSfcStepEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepEntity>(manuSfcStepEntities, manuSfcStepPagedQuery.PageIndex, manuSfcStepPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcStepQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetManuSfcStepEntitiesAsync(ManuSfcStepQuery manuSfcStepQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcStepEntitiesSqlTemplate);
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            if (!string.IsNullOrEmpty(manuSfcStepQuery.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }
            if (manuSfcStepQuery?.SFCs?.Any() == true)
            {
                string sfcstr = string.Join("','", manuSfcStepQuery.SFC);
                sqlBuilder.Where($"SFC IN ('{sfcstr}')");
            }
            if (manuSfcStepQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcStepEntities = await conn.QueryAsync<ManuSfcStepEntity>(template.RawSql, manuSfcStepQuery);
            return manuSfcStepEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcStepEntity manuSfcStepEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcStepEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcStepEntity> manuSfcStepEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcStepEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcStepEntity manuSfcStepEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcStepEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcStepEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcStepEntity> manuSfcStepEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcStepEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        #region 业务表
        /// <summary>
        /// 插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntitie"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcStepBusinessAsync(MaunSfcStepBusinessEntity maunSfcStepBusinessEntitie)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSfcStepBusinessSql, maunSfcStepBusinessEntitie);
        }

        /// <summary>
        /// 批量插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcStepBusinessRangeAsync(IEnumerable<MaunSfcStepBusinessEntity> maunSfcStepBusinessEntities)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSfcStepBusinessSql, maunSfcStepBusinessEntities);
        }
        #endregion

        /// <summary>
        /// 获取SFC的进出站步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetSFCInOutStepAsync(SfcInOutStepQuery sfcQuery)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcStepEntities = await conn.QueryAsync<ManuSfcStepEntity>(GetSFCInOutStepSql, sfcQuery);
            return manuSfcStepEntities;

        }

        /// <summary>
        /// 取NG步骤信息(永泰)
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetNgStepAsync()
        {
            //sfc预留
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcStepEntities = await conn.QueryAsync<ManuSfcStepEntity>(GetNGStepSql);
            return manuSfcStepEntities;

        }

        /// <summary>
        /// 分页查询 根据SFC
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoBySFCAsync(ManuSfcStepBySfcPagedQuery queryParam)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetBySFCPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetBySFCPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(queryParam.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }

            var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
            sqlBuilder.AddParameters(queryParam);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcStepEntitiesTask = conn.QueryAsync<ManuSfcStepEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepEntities = await manuSfcStepEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepEntity>(manuSfcStepEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcStepRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_step` /**where**/ ";
        const string GetManuSfcStepEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_step` /**where**/  ";
        const string InsertSfcStepBusinessSql = "INSERT INTO `manu_sfc_step_business`(  `Id`, `SiteId`, `SfcStepId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcStepId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertSql = "INSERT INTO manu_sfc_step(Id, SFC, ProductId, WorkOrderId, WorkCenterId, ProductBOMId, Qty, EquipmentId, ResourceId, ProcedureId, Operatetype, CurrentStatus,Remark,CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId, Passed, IsPassingStation) VALUES (   @Id, @SFC, @ProductId, @WorkOrderId, @WorkCenterId, @ProductBOMId, @Qty, @EquipmentId, @ResourceId, @ProcedureId, @Operatetype, @CurrentStatus,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @Passed, @IsPassingStation )  ";
        const string UpdateSql = "UPDATE `manu_sfc_step` SET   SFC = @SFC, ProductId = @ProductId, WorkOrderId = @WorkOrderId, WorkCenterId = @WorkCenterId, ProductBOMId = @ProductBOMId, Qty = @Qty, EquipmentId = @EquipmentId, ResourceId = @ResourceId, ProcedureId = @ProcedureId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId,  Passed = @Passed, IsPassingStation = @IsPassingStation,IsReplenish=@IsReplenish  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_step` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SFC`, `ProductId`, `WorkOrderId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Type`, `Status`, `Lock`, `IsMultiplex`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Passed`, `IsPassingStation`
                            FROM `manu_sfc_step`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SFC`, `ProductId`, `WorkOrderId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Type`, `Status`, `Lock`, `IsMultiplex`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Passed`, `IsPassingStation`
                            FROM `manu_sfc_step`  WHERE Id IN @ids ";

        const string GetSFCInOutStepSql = @" 
                        SELECT 
                           `Id`, `SFC`, `ProductId`, `WorkOrderId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `CurrentStatus`, `Operatetype`, `IsRepair`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Passed`, `IsPassingStation`
                        FROM `manu_sfc_step` 
                        WHERE IsDeleted=0
                        AND Operatetype in (3,4)
                        and SFC=@Sfc
                        AND SiteId=@SiteId 
                        ORDER BY CreatedOn asc
                        ";

        const string GetNGStepSql = @" select main.*  from manu_sfc_step_ng ng left join manu_sfc_step main on main.Id =ng.BarCodeStepId
                                        where main.ProcedureId in(select PP.Id from proc_procedure pp where pp.Code in('OP17','OP18'))
                                        and ng.IsReplenish=0 order by main.UpdatedOn desc";

        const string GetBySFCPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY CreatedOn desc LIMIT @Offset,@Rows ";
        const string GetBySFCPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_step` /**where**/ ";
    }
}
