/*
 *creator: Karl
 *
 *describe: 条码绑定关系表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
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
    /// 条码绑定关系表仓储
    /// </summary>
    public partial class ManuSfcBindRepository :BaseRepository, IManuSfcBindRepository
    {

        public ManuSfcBindRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        /// 批量删除（硬删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteTruesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTruesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcBindEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcBindEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcBindEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcBindEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 根据SFC查询绑定数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcBindEntity>> GetBySFCAsync(string sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcBindEntity>(GetBySFCSql, new { SFC = sfc });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcBindPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcBindEntity>> GetPagedInfoAsync(ManuSfcBindPagedQuery manuSfcBindPagedQuery)
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
           
            var offSet = (manuSfcBindPagedQuery.PageIndex - 1) * manuSfcBindPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcBindPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcBindPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcBindEntitiesTask = conn.QueryAsync<ManuSfcBindEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcBindEntities = await manuSfcBindEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcBindEntity>(manuSfcBindEntities, manuSfcBindPagedQuery.PageIndex, manuSfcBindPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcBindQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcBindEntity>> GetManuSfcBindEntitiesAsync(ManuSfcBindQuery manuSfcBindQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcBindEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcBindEntities = await conn.QueryAsync<ManuSfcBindEntity>(template.RawSql, manuSfcBindQuery);
            return manuSfcBindEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcBindEntity manuSfcBindEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcBindEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcBindEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcBindEntity> manuSfcBindEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcBindEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcBindEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcBindEntity manuSfcBindEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcBindEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcBindEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcBindEntity> manuSfcBindEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcBindEntitys);
        }
        #endregion

    }

    public partial class ManuSfcBindRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_bind` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_bind` /**where**/ ";
        const string GetManuSfcBindEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_bind` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_bind`(  `Id`,`SiteId`, `SFC`, `BindSFC`, `Type`, `Status`, `BindingTime`, `UnbindingTime`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @BindSFC, @Type, @Status, @BindingTime, @UnbindingTime, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_bind`(  `Id`,`SiteId`, `SFC`, `BindSFC`, `Type`, `Status`, `BindingTime`, `UnbindingTime`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @BindSFC, @Type, @Status, @BindingTime, @UnbindingTime, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_bind` SET  SiteId = @SiteId, SFC = @SFC, BindSFC = @BindSFC, Type = @Type, Status = @Status, BindingTime = @BindingTime, UnbindingTime = @UnbindingTime, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_bind` SET SiteId = @SiteId,  SFC = @SFC, BindSFC = @BindSFC, Type = @Type, Status = @Status, BindingTime = @BindingTime, UnbindingTime = @UnbindingTime, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_bind` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_bind` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`,`SiteId`,  `SFC`, `BindSFC`, `Type`, `Status`, `BindingTime`, `UnbindingTime`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_bind`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`,`SiteId`,  `SFC`, `BindSFC`, `Type`, `Status`, `BindingTime`, `UnbindingTime`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_bind`  WHERE Id IN @Ids ";

        const string GetBySFCSql = @"SELECT 
                               `Id`,`SiteId`,  `SFC`, `BindSFC`, `Type`, `Status`, `BindingTime`, `UnbindingTime`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_bind`  WHERE SFC = @SFC ";

        //硬删除
        const string DeleteTruesSql = "Delete FROM  `manu_sfc_bind` WHERE Id IN @Ids";
        #endregion
    }
}
