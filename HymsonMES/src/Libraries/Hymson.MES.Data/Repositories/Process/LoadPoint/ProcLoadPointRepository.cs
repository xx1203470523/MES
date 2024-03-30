using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.LoadPoint.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点表仓储
    /// </summary>
    public partial class ProcLoadPointRepository : BaseRepository, IProcLoadPointRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcLoadPointRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// <param name="param"></param>
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
        public async Task<ProcLoadPointEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetByResourceIdAsync(long resourceId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcLoadPointEntity>(GetByResourceIdSql, new { resourceId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointEntity>> GetPagedInfoAsync(ProcLoadPointPagedQuery procLoadPointPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where(" IsDeleted = 0 ");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procLoadPointPagedQuery.LoadPoint))
            {
                procLoadPointPagedQuery.LoadPoint = $"%{procLoadPointPagedQuery.LoadPoint}%";
                sqlBuilder.Where(" LoadPoint like @LoadPoint ");
            }
            if (!string.IsNullOrWhiteSpace(procLoadPointPagedQuery.LoadPointName))
            {
                procLoadPointPagedQuery.LoadPointName = $"%{procLoadPointPagedQuery.LoadPointName}%";
                sqlBuilder.Where(" LoadPointName like @LoadPointName ");
            }
            if (procLoadPointPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status = @Status ");
            }

            var offSet = (procLoadPointPagedQuery.PageIndex - 1) * procLoadPointPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLoadPointPagedQuery.PageSize });
            sqlBuilder.AddParameters(procLoadPointPagedQuery);

            using var conn = GetMESDbConnection();
            var procLoadPointEntitiesTask = conn.QueryAsync<ProcLoadPointEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLoadPointEntities = await procLoadPointEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLoadPointEntity>(procLoadPointEntities, procLoadPointPagedQuery.PageIndex, procLoadPointPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetProcLoadPointEntitiesAsync(ProcLoadPointQuery procLoadPointQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLoadPointEntitiesSqlTemplate);
            sqlBuilder.Where(" IsDeleted=0 ");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procLoadPointQuery.LoadPoint))
            {
                sqlBuilder.Where(" LoadPoint = @LoadPoint ");
            }

            using var conn = GetMESDbConnection();
            var procLoadPointEntities = await conn.QueryAsync<ProcLoadPointEntity>(template.RawSql, procLoadPointQuery);
            return procLoadPointEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLoadPointEntity procLoadPointEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procLoadPointEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procLoadPointEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLoadPointEntity procLoadPointEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLoadPointEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointEntitys);
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        #region 顷刻

        /// <summary>
        /// 获取上料点或者设备
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PointOrEquipmentView>> GetPointOrEquipmmentAsync(ProcLoadPointEquipmentQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PointOrEquipmentView>(GetPointOrEquipmentSql, query);
        }

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointEntity> GetProcLoadPointAsync(ProcLoadPointQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointEntity>(GetLoadPointSql, query);
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcLoadPointRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_load_point` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_load_point` /**where**/ ";
        const string GetProcLoadPointEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_load_point` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_load_point`(  `Id`, `SiteId`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_load_point`(  `Id`, `SiteId`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_load_point` SET  LoadPointName = @LoadPointName, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_load_point` SET   SiteId = @SiteId, LoadPoint = @LoadPoint, LoadPointName = @LoadPointName, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_load_point` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_load_point` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point`  WHERE Id IN @ids ";
        const string GetByResourceIdSql = @"SELECT PLP.* FROM proc_load_point PLP
                                LEFT JOIN proc_load_point_link_resource PLPLR ON PLPLR.LoadPointId = PLP.Id
                                WHERE PLPLR.ResourceId = @resourceId AND PLP.IsDeleted = 0";


        const string UpdateStatusSql = "UPDATE `proc_load_point` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        #region 顷刻

        /// <summary>
        /// 获取上料或者设备
        /// </summary>
        const string GetPointOrEquipmentSql = @"
            select '1' as DataType, t1.Id, t1.LoadPoint Code, t3.id ResId, t3.ResCode ,t3.ResName, t1.SiteId
            from proc_load_point t1
            inner join proc_load_point_link_resource t2 on t1.Id  = t2.LoadPointId and t2.IsDeleted = 0
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            where t1.LoadPoint in @CodeList
            and t1.IsDeleted = 0
            union
            select '2' as DataType, m1.Id, m1.EquipmentCode Code, m3.id ResId, m3.ResCode ,m3.ResName ,m1.SiteId
            from equ_equipment m1
            inner join proc_resource_equipment_bind m2 on m1.Id = m2.EquipmentId and m2.IsDeleted = 0
            inner join proc_resource m3 on m3.Id = m2.ResourceId and m3.IsDeleted = 0
            where m1.IsDeleted = 0
            and m1.EquipmentCode in @CodeList
        ";

        /// <summary>
        /// 获取上料点
        /// </summary>
        const string GetLoadPointSql = @"select * from proc_load_point where IsDeleted = 0 and LoadPoint = @LoadPoint ";

        #endregion

    }
}
