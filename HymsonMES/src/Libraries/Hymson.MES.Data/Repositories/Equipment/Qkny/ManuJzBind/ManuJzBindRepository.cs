using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuJzBind;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;
using IdGen;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.ManuJzBind
{
    /// <summary>
    /// 仓储（极组绑定）
    /// </summary>
    public partial class ManuJzBindRepository : BaseRepository, IManuJzBindRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuJzBindRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 根据极组条码查询信息
        /// </summary>
        /// <param name="jzSfc"></param>
        /// <returns></returns>
        public async Task<ManuJzBindEntity> GetByJzSfcAsync(ManuJzBindQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuJzBindEntity>(GetByJzSfcSql, query);
        }

        /// <summary>
        /// 根据极组条码查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> UpdateSfcById(UpdateSfcByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSfcByIdSql, command);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicsAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicsSql, new { Id = id });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuJzBindEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuJzBindEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuJzBindEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuJzBindEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuJzBindEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuJzBindEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuJzBindEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuJzBindEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuJzBindEntity>> GetEntitiesAsync(ManuJzBindQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuJzBindEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuJzBindEntity>> GetPagedListAsync(ManuJzBindPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuJzBindEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuJzBindEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuJzBindRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_jz_bind /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_jz_bind /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_jz_bind /**where**/  ";

        const string InsertSql = "INSERT INTO manu_jz_bind(  `Id`, `EquipmentId`, `JzSfc1`, `JzSfc2`, `Sfc`, `BindType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @JzSfc1, @JzSfc2, @Sfc, @BindType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";
        const string InsertsSql = "INSERT INTO manu_jz_bind(  `Id`, `EquipmentId`, `JzSfc1`, `JzSfc2`, `Sfc`, `BindType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @JzSfc1, @JzSfc2, @Sfc, @BindType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";

        const string UpdateSql = "UPDATE manu_jz_bind SET   EquipmentId = @EquipmentId, JzSfc1 = @JzSfc1, JzSfc2 = @JzSfc2, Sfc = @Sfc, BindType = @BindType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_jz_bind SET   EquipmentId = @EquipmentId, JzSfc1 = @JzSfc1, JzSfc2 = @JzSfc2, Sfc = @Sfc, BindType = @BindType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_jz_bind SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_jz_bind SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_jz_bind WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_jz_bind WHERE Id IN @Ids ";

        /// <summary>
        /// 获取极组条码
        /// </summary>
        const string GetByJzSfcSql = @"
            select * 
            from manu_jz_bind 
            where (jzSfc1 = @JzSfc or jzSfc2 = @JzSfc)
            and IsDeleted = 0
            and SiteId = @SiteId
        ";

        /// <summary>
        /// 根据ID更新电芯码
        /// </summary>
        const string UpdateSfcByIdSql = @"
            update manu_jz_bind set sfc = @Sfc, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn 
            where id = @id
        ";

        /// <summary>
        /// 物料删除
        /// </summary>
        const string DeletePhysicsSql = "delete from manu_jz_bind WHERE Id = @Id ";
    }
}
