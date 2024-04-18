using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 仓储（设备最新信息）
    /// </summary>
    public partial class ManuEuqipmentNewestInfoRepository : BaseRepository, IManuEuqipmentNewestInfoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuEuqipmentNewestInfoRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuEuqipmentNewestInfoEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuEuqipmentNewestInfoEntity entity)
        {
            string curUpdateSql = string.Empty;
            if(string.IsNullOrEmpty(entity.Status) == false)
            {
                curUpdateSql = UpdateSqlStatus;
            }
            else if(string.IsNullOrEmpty(entity.Heart) == false)
            {
                curUpdateSql = UpdateSqlHeart;
            }
            else if (string.IsNullOrEmpty(entity.LoginResult) == false)
            {
                curUpdateSql = UpdateSqlLogin;
            }

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(curUpdateSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuEuqipmentNewestInfoEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuEuqipmentNewestInfoEntity> entities)
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
        public async Task<ManuEuqipmentNewestInfoEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuEuqipmentNewestInfoEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuEuqipmentNewestInfoEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuEuqipmentNewestInfoEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuEuqipmentNewestInfoEntity>> GetEntitiesAsync(ManuEuqipmentNewestInfoQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("EquipmentId = @EquipmentId");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuEuqipmentNewestInfoEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEuqipmentNewestInfoEntity>> GetPagedListAsync(ManuEuqipmentNewestInfoPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuEuqipmentNewestInfoEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuEuqipmentNewestInfoEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuEuqipmentNewestInfoRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_euqipment_newest_info /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_euqipment_newest_info /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_euqipment_newest_info /**where**/  ";

        const string InsertSql = "INSERT INTO manu_euqipment_newest_info(  `Id`, `EquipmentId`, `LoginResult`, `LoginResultUpdatedOn`, `Status`, `StatusUpdatedOn`, `Heart`, `HeartUpdatedOn`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`, `DownReason`) VALUES (  @Id, @EquipmentId, @LoginResult, @LoginResultUpdatedOn, @Status, @StatusUpdatedOn, @Heart, @HeartUpdatedOn, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId, @DownReason) ";
        const string InsertsSql = "INSERT INTO manu_euqipment_newest_info(  `Id`, `EquipmentId`, `LoginResult`, `LoginResultUpdatedOn`, `Status`, `StatusUpdatedOn`, `Heart`, `HeartUpdatedOn`, `CreatedBy`, `CreatdeOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`, `DownReason`) VALUES (  @Id, @EquipmentId, @LoginResult, @LoginResultUpdatedOn, @Status, @StatusUpdatedOn, @Heart, @HeartUpdatedOn, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId, @DownReason) ";

        const string UpdateSql = "UPDATE manu_euqipment_newest_info SET   EquipmentId = @EquipmentId, LoginResult = @LoginResult, LoginResultUpdatedOn = @LoginResultUpdatedOn, Status = @Status, StatusUpdatedOn = @StatusUpdatedOn, Heart = @Heart, HeartUpdatedOn = @HeartUpdatedOn, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId, DownReason = @DownReason WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_euqipment_newest_info SET   EquipmentId = @EquipmentId, LoginResult = @LoginResult, LoginResultUpdatedOn = @LoginResultUpdatedOn, Status = @Status, StatusUpdatedOn = @StatusUpdatedOn, Heart = @Heart, HeartUpdatedOn = @HeartUpdatedOn, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId, DownReason = @DownReason WHERE Id = @Id ";

        const string UpdateSqlHeart = "UPDATE manu_euqipment_newest_info SET Heart = @Heart, HeartUpdatedOn = @HeartUpdatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateSqlLogin = "UPDATE manu_euqipment_newest_info SET LoginResult = @LoginResult, LoginResultUpdatedOn = @LoginResultUpdatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateSqlStatus = "UPDATE manu_euqipment_newest_info SET Status = @Status, StatusUpdatedOn = @StatusUpdatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, DownReason = @DownReason WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_euqipment_newest_info SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_euqipment_newest_info SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_euqipment_newest_info WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_euqipment_newest_info WHERE Id IN @Ids ";

    }
}
