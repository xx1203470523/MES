using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板按钮仓储
    /// </summary>
    public partial class ManuFacePlateButtonRepository : BaseRepository, IManuFacePlateButtonRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuFacePlateButtonRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
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
        /// 删除（硬删除）
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueAsync(long facePlateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueSql, new { FacePlateId = facePlateId });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateButtonEntity> GetByIdAsync(long id)
        {
            var key = $"{CachedTables.MANU_FACE_PLATE_BUTTON}&{id}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ManuFacePlateButtonEntity>(GetByIdSql, new { Id = id });
            });
        }

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonEntity>> GetByFacePlateIdAsync(long facePlateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateButtonEntity>(GetByFacePlateIdSql, new { facePlateId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateButtonEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateButtonPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateButtonEntity>> GetPagedInfoAsync(ManuFacePlateButtonPagedQuery manuFacePlateButtonPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (manuFacePlateButtonPagedQuery.FacePlateId.HasValue)
            {
                sqlBuilder.Where("FacePlateId=@FacePlateId");
            }

            var offSet = (manuFacePlateButtonPagedQuery.PageIndex - 1) * manuFacePlateButtonPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlateButtonPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlateButtonPagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateButtonEntitiesTask = conn.QueryAsync<ManuFacePlateButtonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateButtonEntities = await manuFacePlateButtonEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateButtonEntity>(manuFacePlateButtonEntities, manuFacePlateButtonPagedQuery.PageIndex, manuFacePlateButtonPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateButtonQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonEntity>> GetManuFacePlateButtonEntitiesAsync(ManuFacePlateButtonQuery manuFacePlateButtonQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateButtonEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateButtonEntities = await conn.QueryAsync<ManuFacePlateButtonEntity>(template.RawSql, manuFacePlateButtonQuery);
            return manuFacePlateButtonEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateButtonEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateButtonEntity manuFacePlateButtonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateButtonEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateButtonEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateButtonEntity> manuFacePlateButtonEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateButtonEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateButtonEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateButtonEntity manuFacePlateButtonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuFacePlateButtonEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateButtonEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateButtonEntity> manuFacePlateButtonEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateButtonEntitys);
        }
        #endregion

    }

    public partial class ManuFacePlateButtonRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate_button` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate_button` /**where**/ ";
        const string GetManuFacePlateButtonEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate_button` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate_button`(  `Id`, `SiteId`, `FacePlateId`, `Seq`, `Name`, `Percentage`, `Hotkeys`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateId, @Seq, @Name, @Percentage, @Hotkeys, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate_button`(  `Id`, `SiteId`, `FacePlateId`, `Seq`, `Name`, `Percentage`, `Hotkeys`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateId, @Seq, @Name, @Percentage, @Hotkeys, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_face_plate_button` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, Seq = @Seq, Name = @Name, Percentage = @Percentage, Hotkeys = @Hotkeys, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate_button` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, Seq = @Seq, Name = @Name, Percentage = @Percentage, Hotkeys = @Hotkeys, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate_button` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate_button` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `manu_face_plate_button`  WHERE Id = @Id ";
        const string GetByFacePlateIdSql = "SELECT * FROM manu_face_plate_button WHERE IsDeleted = 0 AND FacePlateId = @facePlateId ORDER BY Seq";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FacePlateId`, `Seq`, `Name`, `Percentage`, `Hotkeys`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_button`  WHERE Id IN @Ids ";
        const string DeleteTrueSql = "DELETE FROM  `manu_face_plate_button` WHERE FacePlateId = @FacePlateId ";
        #endregion
    }
}
