using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcGrade.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位表仓储
    /// </summary>
    public partial class ManuSfcGradeRepository :BaseRepository, IManuSfcGradeRepository
    {

        public ManuSfcGradeRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuSfcGradeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcGradeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcGradeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcGradeEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcGradePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcGradeEntity>> GetPagedInfoAsync(ManuSfcGradePagedQuery manuSfcGradePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (manuSfcGradePagedQuery.PageIndex - 1) * manuSfcGradePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcGradePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcGradePagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcGradeEntitiesTask = conn.QueryAsync<ManuSfcGradeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcGradeEntities = await manuSfcGradeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcGradeEntity>(manuSfcGradeEntities, manuSfcGradePagedQuery.PageIndex, manuSfcGradePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcGradeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcGradeEntity>> GetManuSfcGradeEntitiesAsync(ManuSfcGradeQuery manuSfcGradeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcGradeEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if(manuSfcGradeQuery.Sfcs!=null&& manuSfcGradeQuery.Sfcs.Any())
            {
                sqlBuilder.Where("sfc in @Sfcs");
            }

            using var conn = GetMESDbConnection();
            var manuSfcGradeEntities = await conn.QueryAsync<ManuSfcGradeEntity>(template.RawSql, manuSfcGradeQuery);
            return manuSfcGradeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcGradeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcGradeEntity manuSfcGradeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcGradeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcGradeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcGradeEntity> manuSfcGradeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcGradeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcGradeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcGradeEntity manuSfcGradeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcGradeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="gradeCommands"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<UpdateGradeCommand> gradeCommands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, gradeCommands);
        }
        #endregion

    }

    public partial class ManuSfcGradeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_grade` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_grade` /**where**/ ";
        const string GetManuSfcGradeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_grade` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_grade`(  `Id`, `SiteId`, `SFC`, `Grade`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Grade, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_grade`(  `Id`, `SiteId`, `SFC`, `Grade`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Grade, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_sfc_grade` SET Grade = @Grade,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_grade` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_grade` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SFC`, `Grade`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_grade`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SFC`, `Grade`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_grade`  WHERE Id IN @Ids ";
        #endregion
    }
}
