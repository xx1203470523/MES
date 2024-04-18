/*
 *creator: Karl
 *
 *describe: 设备验证 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-28 09:02:39
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备验证仓储
    /// </summary>
    public partial class EquEquipmentVerifyRepository :BaseRepository, IEquEquipmentVerifyRepository
    {

        public EquEquipmentVerifyRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（真删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new {Ids=ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentVerifyEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentVerifyEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentVerifyEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentVerifyEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentVerifyPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentVerifyEntity>> GetPagedInfoAsync(EquEquipmentVerifyPagedQuery equEquipmentVerifyPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (equEquipmentVerifyPagedQuery.PageIndex - 1) * equEquipmentVerifyPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equEquipmentVerifyPagedQuery.PageSize });
            sqlBuilder.AddParameters(equEquipmentVerifyPagedQuery);

            using var conn = GetMESDbConnection();
            var equEquipmentVerifyEntitiesTask = conn.QueryAsync<EquEquipmentVerifyEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equEquipmentVerifyEntities = await equEquipmentVerifyEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquEquipmentVerifyEntity>(equEquipmentVerifyEntities, equEquipmentVerifyPagedQuery.PageIndex, equEquipmentVerifyPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equEquipmentVerifyQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentVerifyEntity>> GetEquEquipmentVerifyEntitiesAsync(EquEquipmentVerifyQuery equEquipmentVerifyQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquEquipmentVerifyEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equEquipmentVerifyEntities = await conn.QueryAsync<EquEquipmentVerifyEntity>(template.RawSql, equEquipmentVerifyQuery);
            return equEquipmentVerifyEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentVerifyEntity equEquipmentVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equEquipmentVerifyEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquEquipmentVerifyEntity> equEquipmentVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equEquipmentVerifyEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentVerifyEntity equEquipmentVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equEquipmentVerifyEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equEquipmentVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquEquipmentVerifyEntity> equEquipmentVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equEquipmentVerifyEntitys);
        }
        #endregion


        /// <summary>
        /// 根据设备ID批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesByEquipmentIdsAsync(long[] equipmentIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByEquipmentIdsSql, new { EquipmentIds = equipmentIds });
        }

        /// <summary>
        /// 根据设备ID查询对应的验证
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentVerifyEntity>> GetEquipmentVerifyByEquipmentIdAsync(long equipmentId) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentVerifyEntity>(GetEquipmentVerifyByEquipmentIdSql, new { EquipmentId = equipmentId });
        }
    }

    public partial class EquEquipmentVerifyRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_verify` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_verify` /**where**/ ";
        const string GetEquEquipmentVerifyEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_equipment_verify` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_equipment_verify`(  `Id`, `SiteId`, `EquipmentId`, `Account`, `Password`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @EquipmentId, @Account, @Password, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_equipment_verify`(  `Id`, `SiteId`, `EquipmentId`, `Account`, `Password`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @EquipmentId, @Account, @Password, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";

        const string UpdateSql = "UPDATE `equ_equipment_verify` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, Account = @Account, Password = @Password, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_equipment_verify` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, Account = @Account, Password = @Password, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "DELETE FROM `equ_equipment_verify` WHERE Id = @Id ";
        const string DeletesSql = "DELETE FROM `equ_equipment_verify` WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `EquipmentId`, `Account`, `Password`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `equ_equipment_verify`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `EquipmentId`, `Account`, `Password`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `equ_equipment_verify`  WHERE Id IN @Ids ";
        const string DeletesByEquipmentIdsSql = "DELETE FROM `equ_equipment_verify` WHERE EquipmentId IN @EquipmentIds";

        const string GetEquipmentVerifyByEquipmentIdSql = "SELECT * FROM `equ_equipment_verify` WHERE EquipmentId=@EquipmentId ";
        #endregion
    }
}
