/*
 *creator: Karl
 *
 *describe: 操作面板 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板仓储
    /// </summary>
    public partial class ManuFacePlateRepository : BaseRepository, IManuFacePlateRepository
    {

        public ManuFacePlateRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuFacePlateEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据code获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateEntity> GetByCodeAsync(EntityByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateEntity>(GetByCodeSql, new { Code = param.Code, SiteId = param.Site, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 判断条码是否已经存在且未删除
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id">修改时的ID</param>
        /// <returns></returns>
        public async Task<bool> IsExists(string code, long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteScalarAsync(IsExistsSql, new { Code = code, Id = id }) != null;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlatePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateEntity>> GetPagedInfoAsync(ManuFacePlatePagedQuery manuFacePlatePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.OrderBy("CreatedOn DESC");
            sqlBuilder.Select("Id,Code, Name, Type, Status, ConversationTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn");

            if (manuFacePlatePagedQuery.Type.HasValue)
            {
                sqlBuilder.Where("Type = @Type");
            }

            if (manuFacePlatePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(manuFacePlatePagedQuery.Code) )
            {
                manuFacePlatePagedQuery.Code = $"%{manuFacePlatePagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(manuFacePlatePagedQuery.Name) )
            {
                manuFacePlatePagedQuery.Name = $"%{manuFacePlatePagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            var offSet = (manuFacePlatePagedQuery.PageIndex - 1) * manuFacePlatePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlatePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlatePagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateEntitiesTask = conn.QueryAsync<ManuFacePlateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateEntities = await manuFacePlateEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateEntity>(manuFacePlateEntities, manuFacePlatePagedQuery.PageIndex, manuFacePlatePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateEntity>> GetManuFacePlateEntitiesAsync(ManuFacePlateQuery manuFacePlateQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateEntities = await conn.QueryAsync<ManuFacePlateEntity>(template.RawSql, manuFacePlateQuery);
            return manuFacePlateEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateEntity manuFacePlateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateEntity> manuFacePlateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateEntity manuFacePlateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuFacePlateEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateEntity> manuFacePlateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateEntitys);
        }
        #endregion

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }
    }

    public partial class ManuFacePlateRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate` /**where**/ ";
        const string GetManuFacePlateEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate`(  `Id`, `Code`, `Name`, `Type`, `Status`, `ConversationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Type, @Status, @ConversationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate`(  `Id`, `Code`, `Name`, `Type`, `Status`, `ConversationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Type, @Status, @ConversationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `manu_face_plate` SET  Name = @Name, Type = @Type, ConversationTime = @ConversationTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate` SET  Name = @Name, Type = @Type, Status = @Status, ConversationTime = @ConversationTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Type`, `Status`, `ConversationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_face_plate`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Type`, `Status`, `ConversationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_face_plate`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Type`, `Status`, `ConversationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_face_plate`  WHERE Code = @Code  and Status=@Status    and IsDeleted=0 AND SiteId=@SiteId ";

        const string IsExistsSql = "SELECT Id FROM manu_face_plate WHERE `IsDeleted` = 0 AND Code = @Code AND SiteId = @Id";
        #endregion

        const string UpdateStatusSql = "UPDATE `manu_face_plate` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    }
}
