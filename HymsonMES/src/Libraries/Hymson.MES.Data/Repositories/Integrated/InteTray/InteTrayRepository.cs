/*
 *creator: Karl
 *
 *describe: 托盘信息 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteTray.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 托盘信息仓储
    /// </summary>
    public partial class InteTrayRepository :BaseRepository, IInteTrayRepository
    {

        public InteTrayRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteTrayEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteTrayEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteTrayEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteTrayEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 根据容器编码获取数据
        /// </summary>
        /// <param name="containerCode"></param>
        /// <returns></returns>
        public async Task<InteTrayEntity> GetByCodeAsync(string containerCode) {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteTrayEntity>(GetByCodeSql, new { Code = containerCode });
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteTrayPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteTrayEntity>> GetPagedInfoAsync(InteTrayPagedQuery inteTrayPagedQuery)
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
           
            var offSet = (inteTrayPagedQuery.PageIndex - 1) * inteTrayPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteTrayPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteTrayPagedQuery);

            using var conn = GetMESDbConnection();
            var inteTrayEntitiesTask = conn.QueryAsync<InteTrayEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteTrayEntities = await inteTrayEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteTrayEntity>(inteTrayEntities, inteTrayPagedQuery.PageIndex, inteTrayPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteTrayQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteTrayEntity>> GetInteTrayEntitiesAsync(InteTrayQuery inteTrayQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteTrayEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteTrayEntities = await conn.QueryAsync<InteTrayEntity>(template.RawSql, inteTrayQuery);
            return inteTrayEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteTrayEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteTrayEntity inteTrayEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteTrayEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteTrayEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteTrayEntity> inteTrayEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteTrayEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteTrayEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteTrayEntity inteTrayEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteTrayEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteTrayEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteTrayEntity> inteTrayEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteTrayEntitys);
        }
        #endregion

    }

    public partial class InteTrayRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_tray` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_tray` /**where**/ ";
        const string GetInteTrayEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_tray` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_tray`(  `Id`, `SiteId`, `Code`, `Name`, `MaxLoadQty`, `MaxSeq`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @MaxLoadQty, @MaxSeq, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_tray`(  `Id`, `SiteId`, `Code`, `Name`, `MaxLoadQty`, `MaxSeq`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @MaxLoadQty, @MaxSeq, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_tray` SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaxLoadQty = @MaxLoadQty, MaxSeq = @MaxSeq, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_tray` SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaxLoadQty = @MaxLoadQty, MaxSeq = @MaxSeq, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_tray` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_tray` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `MaxLoadQty`, `MaxSeq`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_tray`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `MaxLoadQty`, `MaxSeq`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_tray`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `MaxLoadQty`, `MaxSeq`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_tray`  WHERE Code = @Code ";
        #endregion
    }
}
