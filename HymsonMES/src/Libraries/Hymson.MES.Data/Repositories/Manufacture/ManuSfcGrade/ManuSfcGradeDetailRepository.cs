/*
 *creator: Karl
 *
 *describe: 条码档位明细表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:27
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位明细表仓储
    /// </summary>
    public partial class ManuSfcGradeDetailRepository :BaseRepository, IManuSfcGradeDetailRepository
    {

        public ManuSfcGradeDetailRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuSfcGradeDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcGradeDetailEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcGradeDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcGradeDetailEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcGradeDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcGradeDetailEntity>> GetPagedInfoAsync(ManuSfcGradeDetailPagedQuery manuSfcGradeDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (manuSfcGradeDetailPagedQuery.PageIndex - 1) * manuSfcGradeDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcGradeDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcGradeDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcGradeDetailEntitiesTask = conn.QueryAsync<ManuSfcGradeDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcGradeDetailEntities = await manuSfcGradeDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcGradeDetailEntity>(manuSfcGradeDetailEntities, manuSfcGradeDetailPagedQuery.PageIndex, manuSfcGradeDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcGradeDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcGradeDetailEntity>> GetManuSfcGradeDetailEntitiesAsync(ManuSfcGradeDetailQuery manuSfcGradeDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcGradeDetailEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcGradeDetailEntities = await conn.QueryAsync<ManuSfcGradeDetailEntity>(template.RawSql, manuSfcGradeDetailQuery);
            return manuSfcGradeDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcGradeDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcGradeDetailEntity manuSfcGradeDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcGradeDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcGradeDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcGradeDetailEntity> manuSfcGradeDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcGradeDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcGradeDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcGradeDetailEntity manuSfcGradeDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcGradeDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcGradeDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcGradeDetailEntity> manuSfcGradeDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcGradeDetailEntitys);
        }

        /// <summary>
        /// 根据档位Id查询明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcGradeDetailEntity>> GetByGradeIdAsync(ManuSfcGradeDetailByGradeIdQuery query) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcGradeDetailEntity>(GetByGradeIdSql, query);
        }
        #endregion

    }

    public partial class ManuSfcGradeDetailRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_grade_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_grade_detail` /**where**/ ";
        const string GetManuSfcGradeDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_grade_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_grade_detail`(  `Id`, `SiteId`, `GadeId`, `ProduceId`, `SFC`, `Grade`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `MinContainingType`, `MaxContainingType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @GadeId, @ProduceId, @SFC, @Grade, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @MinContainingType, @MaxContainingType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_grade_detail`(  `Id`, `SiteId`, `GadeId`, `ProduceId`, `SFC`, `Grade`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `MinContainingType`, `MaxContainingType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @GadeId, @ProduceId, @SFC, @Grade, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @MinContainingType, @MaxContainingType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_grade_detail` SET   SiteId = @SiteId, GadeId = @GadeId, ProduceId = @ProduceId, SFC = @SFC, Grade = @Grade, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, MinContainingType = @MinContainingType, MaxContainingType = @MaxContainingType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_grade_detail` SET   SiteId = @SiteId, GadeId = @GadeId, ProduceId = @ProduceId, SFC = @SFC, Grade = @Grade, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, MinContainingType = @MinContainingType, MaxContainingType = @MaxContainingType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_grade_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_grade_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `GadeId`, `ProduceId`, `SFC`, `Grade`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `MinContainingType`, `MaxContainingType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_grade_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `GadeId`, `ProduceId`, `SFC`, `Grade`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `MinContainingType`, `MaxContainingType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_grade_detail`  WHERE Id IN @Ids ";

        const string GetByGradeIdSql = @"SELECT * FROM `manu_sfc_grade_detail` WHERE SiteId = @SiteId AND IsDeleted = 0 AND  GadeId = @GadeId ";
        #endregion
    }
}
