/*
 *creator: Karl
 *
 *describe: 操作面板按钮作业 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 03:34:48
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板按钮作业仓储
    /// </summary>
    public partial class ManuFacePlateButtonJobRelationRepository : BaseRepository, IManuFacePlateButtonJobRelationRepository
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
        public ManuFacePlateButtonJobRelationRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
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
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueAsync(long facePlateButtonId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueSql, new { FacePlateButtonId = facePlateButtonId });
        }

        /// <summary>
        /// 批量删除（硬删除）
        /// </summary>
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueAsync(long[] facePlateButtonIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueSql, new { FacePlateButtonIds = facePlateButtonIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateButtonJobRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateButtonJobRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetByFacePlateButtonIdAsync(long facePlateButtonId)
        {
            var key = $"{CachedTables.MANU_FACE_PLATE_BUTTON_JOB_RELATION}&{facePlateButtonId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<ManuFacePlateButtonJobRelationEntity>(GetByFacePlateButtonIdSql, new { facePlateButtonId });
            });
        }

        /// <summary>
        /// 根据FacePlateIds批量获取数据
        /// </summary>
        /// <param name="facePlateButtonId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetByFacePlateButtonIdsAsync(long[] facePlateButtonIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateButtonJobRelationEntity>(GetByFacePlateButtonIdsSql, new { facePlateButtonIds });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateButtonJobRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateButtonJobRelationEntity>> GetPagedInfoAsync(ManuFacePlateButtonJobRelationPagedQuery manuFacePlateButtonJobRelationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (manuFacePlateButtonJobRelationPagedQuery.PageIndex - 1) * manuFacePlateButtonJobRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlateButtonJobRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlateButtonJobRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateButtonJobRelationEntitiesTask = conn.QueryAsync<ManuFacePlateButtonJobRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateButtonJobRelationEntities = await manuFacePlateButtonJobRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateButtonJobRelationEntity>(manuFacePlateButtonJobRelationEntities, manuFacePlateButtonJobRelationPagedQuery.PageIndex, manuFacePlateButtonJobRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetManuFacePlateButtonJobRelationEntitiesAsync(ManuFacePlateButtonJobRelationQuery manuFacePlateButtonJobRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateButtonJobRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateButtonJobRelationEntities = await conn.QueryAsync<ManuFacePlateButtonJobRelationEntity>(template.RawSql, manuFacePlateButtonJobRelationQuery);
            return manuFacePlateButtonJobRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateButtonJobRelationEntity manuFacePlateButtonJobRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateButtonJobRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateButtonJobRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateButtonJobRelationEntity manuFacePlateButtonJobRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuFacePlateButtonJobRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateButtonJobRelationEntitys);
        }
        #endregion

    }

    public partial class ManuFacePlateButtonJobRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate_button_job_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate_button_job_relation` /**where**/ ";
        const string GetManuFacePlateButtonJobRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate_button_job_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate_button_job_relation`(  `Id`, `SiteId`, `FacePlateButtonId`, `JobId`,`Seq`,`IsClear`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateButtonId, @JobId,@Seq,@IsClear,@CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate_button_job_relation`(  `Id`, `SiteId`, `FacePlateButtonId`, `JobId`,`Seq`,`IsClear`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateButtonId, @JobId,@Seq,@IsClear, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_face_plate_button_job_relation` SET   SiteId = @SiteId, FacePlateButtonId = @FacePlateButtonId, JobId = @JobId,Seq=@Seq,IsClear=@IsClear, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate_button_job_relation` SET   SiteId = @SiteId, FacePlateButtonId = @FacePlateButtonId, JobId = @JobId,Seq=@Seq,IsClear=@IsClear, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate_button_job_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate_button_job_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FacePlateButtonId`, `JobId`,`Seq`,`IsClear`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_button_job_relation`  WHERE Id = @Id ";
        const string GetByFacePlateButtonIdSql = "SELECT * FROM manu_face_plate_button_job_relation WHERE IsDeleted = 0 AND FacePlateButtonId = @facePlateButtonId  ORDER BY Seq ";
        const string GetByFacePlateButtonIdsSql = "SELECT * FROM manu_face_plate_button_job_relation WHERE IsDeleted = 0 AND FacePlateButtonId IN @facePlateButtonIds  ORDER BY Seq ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FacePlateButtonId`, `JobId`,`Seq`,`IsClear`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_button_job_relation`  WHERE Id IN @Ids ";
        const string DeleteTrueSql = "DELETE FROM  `manu_face_plate_button_job_relation` WHERE FacePlateButtonId = @FacePlateButtonId ";
        const string DeletesTrueSql = "DELETE FROM  `manu_face_plate_button_job_relation` WHERE FacePlateButtonId IN @FacePlateButtonIds ";
        #endregion
    }
}
