/*
 *creator: Karl
 *
 *describe: 设备维修记录 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录仓储
    /// </summary>
    public partial class EquRepairOrderRepository : BaseRepository, IEquRepairOrderRepository
    {

        public EquRepairOrderRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquRepairOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据FROM数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public async Task<EquRepairOrderEntity> GetFromByIdAsync(long id) 
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryFirstOrDefaultAsync<EquRepairOrderEntity>(GetFromByIdSql, new { Id = id });
        //}

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderPageView>> GetPagedInfoAsync(EquRepairOrderPagedQuery equRepairOrderPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin(" equ_equipment ee ON ee.Id=ero.EquipmentId");
            sqlBuilder.LeftJoin(" equ_repair_result err ON err.RepairOrderId=ero.Id");
            sqlBuilder.Where("ero.IsDeleted=0");
            sqlBuilder.Where("ero.SiteId=@SiteId");
            sqlBuilder.Select("ero.Id,ero.RepairOrder,ee.EquipmentCode,ee.EquipmentName,ero.Status,ero.FaultTime,ero.CreatedBy,ero.CreatedOn,err.RepairPerson,err.RepairStartTime,err.RepairEndTime,err.ConfirmResult,err.ConfirmBy,err.ConfirmOn");
            sqlBuilder.OrderBy("ero.CreatedOn DESC");

            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.RepairOrder))
            {
                equRepairOrderPagedQuery.RepairOrder = $"%{equRepairOrderPagedQuery.RepairOrder}%";
                sqlBuilder.Where("ero.RepairOrder LIKE @RepairOrder");
            }
            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.EquipmentCode))
            {
                equRepairOrderPagedQuery.EquipmentCode = $"%{equRepairOrderPagedQuery.EquipmentCode}%";
                sqlBuilder.Where("ee.EquipmentCode LIKE @EquipmentCode");
            }
            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.EquipmentName))
            {
                equRepairOrderPagedQuery.EquipmentName = $"%{equRepairOrderPagedQuery.EquipmentName}%";
                sqlBuilder.Where("ee.EquipmentName LIKE @EquipmentName");
            }
            if (equRepairOrderPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("ero.Status=@Status");
            }
            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.CreatedBy))
            {
                equRepairOrderPagedQuery.CreatedBy = $"%{equRepairOrderPagedQuery.CreatedBy}%";
                sqlBuilder.Where("ero.CreatedBy LIKE @CreatedBy");
            }
            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.RepairPerson))
            {
                equRepairOrderPagedQuery.RepairPerson = $"%{equRepairOrderPagedQuery.RepairPerson}%";
                sqlBuilder.Where("err.RepairPerson LIKE @RepairPerson");
            }
            if (!string.IsNullOrWhiteSpace(equRepairOrderPagedQuery.ConfirmBy))
            {
                equRepairOrderPagedQuery.ConfirmBy = $"%{equRepairOrderPagedQuery.ConfirmBy}%";
                sqlBuilder.Where("err.ConfirmBy LIKE @ConfirmBy");
            }
            if (equRepairOrderPagedQuery.ConfirmResult.HasValue)
            {
                sqlBuilder.Where("err.ConfirmResult = @ConfirmResult");
            }


            if (equRepairOrderPagedQuery.CreatedOn != null && equRepairOrderPagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equRepairOrderPagedQuery.CreatedOn[0], EndTime = equRepairOrderPagedQuery.CreatedOn[1] });
                sqlBuilder.Where("ero.CreatedOn >= @StartTime AND ero.CreatedOn <= @EndTime");
            }
            if (equRepairOrderPagedQuery.FaultTime != null && equRepairOrderPagedQuery.FaultTime.Length >=2)
            {
                sqlBuilder.AddParameters(new { StartTime = equRepairOrderPagedQuery.FaultTime[0], EndTime = equRepairOrderPagedQuery.FaultTime[1]});
                sqlBuilder.Where("ero.FaultTime >= @StartTime AND ero.FaultTime <= @EndTime");
            }
            if (equRepairOrderPagedQuery.RepairStartTime != null && equRepairOrderPagedQuery.RepairStartTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equRepairOrderPagedQuery.RepairStartTime[0], EndTime = equRepairOrderPagedQuery.RepairStartTime[1] });
                sqlBuilder.Where("err.RepairStartTime >= @StartTime AND err.RepairStartTime <= @EndTime");
            }
            if (equRepairOrderPagedQuery.RepairEndTime != null && equRepairOrderPagedQuery.RepairEndTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equRepairOrderPagedQuery.RepairEndTime[0], EndTime = equRepairOrderPagedQuery.RepairEndTime[1] });
                sqlBuilder.Where("err.RepairEndTime >= @StartTime AND err.RepairEndTime <= @EndTime");
            }
            if (equRepairOrderPagedQuery.ConfirmOn != null && equRepairOrderPagedQuery.ConfirmOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equRepairOrderPagedQuery.ConfirmOn[0], EndTime = equRepairOrderPagedQuery.ConfirmOn[1] });
                sqlBuilder.Where("err.ConfirmOn >= @StartTime AND err.ConfirmOn <= @EndTime");
            }

            var offSet = (equRepairOrderPagedQuery.PageIndex - 1) * equRepairOrderPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equRepairOrderPagedQuery.PageSize });
            sqlBuilder.AddParameters(equRepairOrderPagedQuery);

            try
            {
                using var conn = GetMESDbConnection();
                var equRepairOrderEntitiesTask = conn.QueryAsync<EquRepairOrderPageView>(templateData.RawSql, templateData.Parameters);
                var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
                var equRepairOrderEntities = await equRepairOrderEntitiesTask;
                var totalCount = await totalCountTask;
                return new PagedInfo<EquRepairOrderPageView>(equRepairOrderEntities, equRepairOrderPagedQuery.PageIndex, equRepairOrderPagedQuery.PageSize, totalCount);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equRepairOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderEntity>> GetEquRepairOrderEntitiesAsync(EquRepairOrderQuery equRepairOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquRepairOrderEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equRepairOrderEntities = await conn.QueryAsync<EquRepairOrderEntity>(template.RawSql, equRepairOrderQuery);
            return equRepairOrderEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquRepairOrderEntity equRepairOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equRepairOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquRepairOrderEntity> equRepairOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equRepairOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquRepairOrderEntity equRepairOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equRepairOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquRepairOrderEntity> equRepairOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equRepairOrderEntitys);
        }
        #endregion

    }

    public partial class EquRepairOrderRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_repair_order` ero /**innerjoin**/ /**leftjoin**/ /**where**/  /**groupby**/   /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_repair_order` ero /**innerjoin**/ /**leftjoin**/ /**where**/  /**groupby**/   /**orderby**/  ";
        const string GetEquRepairOrderEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_repair_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_repair_order`(  `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrder, @EquipmentId, @EquipmentRecordId, @FaultTime, @IsShutdown, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_repair_order`(  `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrder, @EquipmentId, @EquipmentRecordId, @FaultTime, @IsShutdown, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_repair_order` SET   SiteId = @SiteId, RepairOrder = @RepairOrder, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, FaultTime = @FaultTime, IsShutdown = @IsShutdown, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_repair_order` SET   SiteId = @SiteId, RepairOrder = @RepairOrder, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, FaultTime = @FaultTime, IsShutdown = @IsShutdown, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_repair_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_repair_order` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order`  WHERE Id IN @Ids ";
        const string GetFromByIdSql = @"SELECT /**select**/ FROM `equ_repair_order` ero /**innerjoin**/ /**leftjoin**/ /**where**/  /**groupby**/   /**orderby**/";

        #endregion
    }
}
