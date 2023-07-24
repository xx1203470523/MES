/*
 *creator: Karl
 *
 *describe: 单位表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-29 02:13:40
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 单位表仓储
    /// </summary>
    public partial class InteUnitRepository :BaseRepository, IInteUnitRepository
    {

        public InteUnitRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteUnitEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteUnitEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteUnitEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteUnitEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteUnitPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteUnitEntity>> GetPagedInfoAsync(InteUnitPagedQuery inteUnitPagedQuery)
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
           
            var offSet = (inteUnitPagedQuery.PageIndex - 1) * inteUnitPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteUnitPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteUnitPagedQuery);

            using var conn = GetMESDbConnection();
            var inteUnitEntitiesTask = conn.QueryAsync<InteUnitEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteUnitEntities = await inteUnitEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteUnitEntity>(inteUnitEntities, inteUnitPagedQuery.PageIndex, inteUnitPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteUnitQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteUnitEntity>> GetInteUnitEntitiesAsync(InteUnitQuery inteUnitQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteUnitEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted =0");
            sqlBuilder.Select("*");

            if (inteUnitQuery.Codes != null && inteUnitQuery.Codes.Any())
            {
                sqlBuilder.Where(" Code  in @Codes ");
            }

            using var conn = GetMESDbConnection();
            var inteUnitEntities = await conn.QueryAsync<InteUnitEntity>(template.RawSql, inteUnitQuery);
            return inteUnitEntities;
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InteUnitEntity> GetByCodeAsync(EntityByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteUnitEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteUnitEntity inteUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteUnitEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteUnitEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteUnitEntity> inteUnitEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteUnitEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteUnitEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteUnitEntity inteUnitEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteUnitEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteUnitEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteUnitEntity> inteUnitEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteUnitEntitys);
        }
        #endregion

    }

    public partial class InteUnitRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_unit` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_unit` /**where**/ ";
        const string GetInteUnitEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_unit` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_unit`(  `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_unit`(  `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_unit` SET   Code = @Code, Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_unit` SET   Code = @Code, Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_unit` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_unit` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_unit`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_unit`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_unit`    WHERE Code = @Code  AND IsDeleted=0 ";
        #endregion
    }
}
