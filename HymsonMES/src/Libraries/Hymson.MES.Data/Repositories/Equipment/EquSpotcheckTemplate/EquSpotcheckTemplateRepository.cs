/*
 *creator: Karl
 *
 *describe: 设备点检模板 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSpotcheckTemplate;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板仓储
    /// </summary>
    public partial class EquSpotcheckTemplateRepository : BaseRepository, IEquSpotcheckTemplateRepository
    {

        public EquSpotcheckTemplateRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquSpotcheckTemplateEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTemplateEntity>(GetByIdSql, new { Id = id });
        }



        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTemplateEntity> GetByCodeAsync(EquSpotcheckTemplateQuery param) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTemplateEntity>(GetByCodeSql, param);
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplatePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTemplateEntity>> GetPagedInfoAsync(EquSpotcheckTemplatePagedQuery equSpotcheckTemplatePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("equ_spotcheck_template_equipment_group_relation relation ON est.Id=relation.SpotCheckTemplateId");
            sqlBuilder.LeftJoin("equ_equipment_group  egroup ON relation.EquipmentGroupId=egroup.Id");
            sqlBuilder.Where("est.IsDeleted=0");
            sqlBuilder.Where("est.SiteId = @SiteId");
            sqlBuilder.Select("est.*");

            if (!string.IsNullOrWhiteSpace(equSpotcheckTemplatePagedQuery.Code))
            {
                sqlBuilder.Where("est.Code=@Code");
            }

            if (!string.IsNullOrWhiteSpace(equSpotcheckTemplatePagedQuery.Name))
            {
                sqlBuilder.Where("est.Name=@Name");
            }

            if (equSpotcheckTemplatePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("est.Status=@Status");
            }

            //设备组
            if (!string.IsNullOrWhiteSpace(equSpotcheckTemplatePagedQuery.EquipmentGroupCode))
            {
                sqlBuilder.Where("egroup.EquipmentGroupCode=@EquipmentGroupCode");
            }
            if (!string.IsNullOrWhiteSpace(equSpotcheckTemplatePagedQuery.EquipmentGroupName))
            {
                sqlBuilder.Where("group.EquipmentGroupName=@EquipmentGroupName");
            }

            var offSet = (equSpotcheckTemplatePagedQuery.PageIndex - 1) * equSpotcheckTemplatePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSpotcheckTemplatePagedQuery.PageSize });
            sqlBuilder.AddParameters(equSpotcheckTemplatePagedQuery);

            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateEntitiesTask = conn.QueryAsync<EquSpotcheckTemplateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSpotcheckTemplateEntities = await equSpotcheckTemplateEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckTemplateEntity>(equSpotcheckTemplateEntities, equSpotcheckTemplatePagedQuery.PageIndex, equSpotcheckTemplatePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSpotcheckTemplateQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateEntity>> GetEquSpotcheckTemplateEntitiesAsync(EquSpotcheckTemplateQuery equSpotcheckTemplateQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSpotcheckTemplateEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateEntities = await conn.QueryAsync<EquSpotcheckTemplateEntity>(template.RawSql, equSpotcheckTemplateQuery);
            return equSpotcheckTemplateEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckTemplateEntity equSpotcheckTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSpotcheckTemplateEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSpotcheckTemplateEntity> equSpotcheckTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSpotcheckTemplateEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckTemplateEntity equSpotcheckTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSpotcheckTemplateEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSpotcheckTemplateEntity> equSpotcheckTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSpotcheckTemplateEntitys);
        }
        #endregion

    }

    public partial class EquSpotcheckTemplateRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spotcheck_template` est /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spotcheck_template` est /**where**/ ";
        const string GetEquSpotcheckTemplateEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spotcheck_template` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spotcheck_template`(  `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_spotcheck_template`(  `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `equ_spotcheck_template` SET   Code = @Code, Name = @Name, Status = @Status, Version = @Version, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spotcheck_template` SET   Code = @Code, Name = @Name, Status = @Status, Version = @Version, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_spotcheck_template` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spotcheck_template` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_spotcheck_template`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_spotcheck_template`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT  
                               `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_spotcheck_template`  WHERE SiteId=@SiteId AND Code = @Code AND Version=@Version AND IsDeleted=0  ";
        #endregion
    }
}
