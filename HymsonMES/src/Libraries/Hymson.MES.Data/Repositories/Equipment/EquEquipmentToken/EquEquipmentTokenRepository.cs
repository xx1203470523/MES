using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备Token仓储
    /// </summary>
    public partial class EquEquipmentTokenRepository : BaseRepository, IEquEquipmentTokenRepository
    {

        public EquEquipmentTokenRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// 根据EquipmentId获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentTokenEntity> GetByEquipmentIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentTokenEntity>(GetByEquipmentIdSql, new { EquipmentId = id });
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentTokenEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentTokenEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentTokenEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentTokenEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentTokenPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentTokenEntity>> GetPagedInfoAsync(EquEquipmentTokenPagedQuery equEquipmentTokenPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            var offSet = (equEquipmentTokenPagedQuery.PageIndex - 1) * equEquipmentTokenPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equEquipmentTokenPagedQuery.PageSize });
            sqlBuilder.AddParameters(equEquipmentTokenPagedQuery);

            using var conn = GetMESDbConnection();
            var equEquipmentTokenEntitiesTask = conn.QueryAsync<EquEquipmentTokenEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equEquipmentTokenEntities = await equEquipmentTokenEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquEquipmentTokenEntity>(equEquipmentTokenEntities, equEquipmentTokenPagedQuery.PageIndex, equEquipmentTokenPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equEquipmentTokenQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentTokenEntity>> GetEquEquipmentTokenEntitiesAsync(EquEquipmentTokenQuery equEquipmentTokenQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquEquipmentTokenEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equEquipmentTokenEntities = await conn.QueryAsync<EquEquipmentTokenEntity>(template.RawSql, equEquipmentTokenQuery);
            return equEquipmentTokenEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentTokenEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentTokenEntity equEquipmentTokenEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equEquipmentTokenEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentTokenEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquEquipmentTokenEntity> equEquipmentTokenEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equEquipmentTokenEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentTokenEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentTokenEntity equEquipmentTokenEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equEquipmentTokenEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equEquipmentTokenEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquEquipmentTokenEntity> equEquipmentTokenEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equEquipmentTokenEntitys);
        }
        #endregion

    }

    public partial class EquEquipmentTokenRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_token` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_token` /**where**/ ";
        const string GetEquEquipmentTokenEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_equipment_token` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_equipment_token`(  `Id`, `EquipmentId`,  `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentId,   @Token, @ExpirationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ON DUPLICATE KEY UPDATE UpdatedOn=@UpdatedOn";
        const string InsertsSql = "INSERT INTO `equ_equipment_token`(  `Id`, `EquipmentId`,   `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentId,  @Token, @ExpirationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ON DUPLICATE KEY UPDATE UpdatedOn=@UpdatedOn";

        const string UpdateSql = "UPDATE `equ_equipment_token` SET   EquipmentId = @EquipmentId,   Token = @Token, ExpirationTime = @ExpirationTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_equipment_token` SET   EquipmentId = @EquipmentId,   Token = @Token, ExpirationTime = @ExpirationTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_equipment_token` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_equipment_token` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `EquipmentId`,  `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_equipment_token`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `EquipmentId`,   `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_equipment_token`  WHERE Id IN @Ids ";

        const string GetByEquipmentIdSql = @"SELECT  
                               `Id`, `EquipmentId`,  `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_equipment_token`  WHERE IsDeleted=0 AND EquipmentId = @EquipmentId ";
        #endregion
    }
}
