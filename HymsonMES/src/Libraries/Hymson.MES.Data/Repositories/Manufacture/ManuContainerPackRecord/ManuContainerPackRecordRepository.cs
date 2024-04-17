/*
 *creator: Karl
 *
 *describe: 容器装载记录 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

using Org.BouncyCastle.Tsp;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载记录仓储
    /// </summary>
    public partial class ManuContainerPackRecordRepository :BaseRepository, IManuContainerPackRecordRepository
    {

        public ManuContainerPackRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuContainerPackRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuContainerPackRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerPackRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuContainerPackRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuContainerPackRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackRecordEntity>> GetPagedInfoAsync(ManuContainerPackRecordPagedQuery manuContainerPackRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (manuContainerPackRecordPagedQuery.ContainerBarCodeId.HasValue)
            {
                sqlBuilder.Where("ContainerBarCodeId=@ContainerBarCodeId");
            }
            if (!string.IsNullOrWhiteSpace(manuContainerPackRecordPagedQuery.LadeBarCode))
            {
                sqlBuilder.Where("LadeBarCode=@LadeBarCode");
            }

            var offSet = (manuContainerPackRecordPagedQuery.PageIndex - 1) * manuContainerPackRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuContainerPackRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuContainerPackRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuContainerPackRecordEntitiesTask = conn.QueryAsync<ManuContainerPackRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuContainerPackRecordEntities = await manuContainerPackRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuContainerPackRecordEntity>(manuContainerPackRecordEntities, manuContainerPackRecordPagedQuery.PageIndex, manuContainerPackRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuContainerPackRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuContainerPackRecordEntity>> GetManuContainerPackRecordEntitiesAsync(ManuContainerPackRecordQuery manuContainerPackRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuContainerPackRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuContainerPackRecordEntities = await conn.QueryAsync<ManuContainerPackRecordEntity>(template.RawSql, manuContainerPackRecordQuery);
            return manuContainerPackRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuContainerPackRecordEntity manuContainerPackRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuContainerPackRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerPackRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuContainerPackRecordEntity> manuContainerPackRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuContainerPackRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerPackRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuContainerPackRecordEntity manuContainerPackRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuContainerPackRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuContainerPackRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuContainerPackRecordEntity> manuContainerPackRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuContainerPackRecordEntitys);
        }
        #endregion

    }

    public partial class ManuContainerPackRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_container_pack_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_container_pack_record` /**where**/ ";
        const string GetManuContainerPackRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_container_pack_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_container_pack_record`(  `Id`, `SiteId`,`ResourceId`,`ProcedureId`, `ContainerBarCodeId`, `LadeBarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId,@ResourceId,@ProcedureId, @ContainerBarCodeId, @LadeBarCode, @OperateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_container_pack_record`(  `Id`, `SiteId`,`ResourceId`,`ProcedureId`, `ContainerBarCodeId`, `LadeBarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId,@ResourceId,@ProcedureId, @ContainerBarCodeId, @LadeBarCode, @OperateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_container_pack_record` SET   SiteId = @SiteId,ResourceId=@ResourceId,ProcedureId=@ProcedureId, ContainerBarCodeId = @ContainerBarCodeId, LadeBarCode = @LadeBarCode, OperateType = @OperateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_container_pack_record` SET   SiteId = @SiteId,ResourceId=@ResourceId,ProcedureId=@ProcedureId, ContainerBarCodeId = @ContainerBarCodeId, LadeBarCode = @LadeBarCode, OperateType = @OperateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_container_pack_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_container_pack_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`,`ResourceId`,`ProcedureId`, `ContainerBarCodeId`, `LadeBarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`,`ResourceId`,`ProcedureId`, `ContainerBarCodeId`, `LadeBarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_container_pack_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
