/*
 *creator: Karl
 *
 *describe: 设备保养模板 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplate
{
    /// <summary>
    /// 设备保养模板仓储
    /// </summary>
    public partial class EquMaintenanceTemplateRepository : BaseRepository, IEquMaintenanceTemplateRepository
    {

        public EquMaintenanceTemplateRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquMaintenanceTemplateEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTemplateEntity>(GetByIdSql, new { Id = id });
        }



        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenanceTemplateEntity> GetByCodeAsync(EquMaintenanceTemplateQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTemplateEntity>(GetByCodeSql, param);
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplatePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTemplateEntity>> GetPagedInfoAsync(EquMaintenanceTemplatePagedQuery EquMaintenanceTemplatePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            //sqlBuilder.LeftJoin("equ_Maintenance_template_equipment_group_relation relation ON relation.MaintenanceTemplateId=est.Id");
            //sqlBuilder.LeftJoin("equ_equipment_group egroup ON egroup.Id=relation.EquipmentGroupId");
            //sqlBuilder.LeftJoin("equ_equipment ee ON ee.EquipmentGroupId=egroup.Id");
            sqlBuilder.Select("est.*");
            sqlBuilder.Where("est.IsDeleted=0");
            sqlBuilder.Where("est.SiteId = @SiteId");
            sqlBuilder.GroupBy("est.*");
            sqlBuilder.OrderBy("est.CreatedOn DESC");
            if (!string.IsNullOrWhiteSpace(EquMaintenanceTemplatePagedQuery.Code))
            {
                EquMaintenanceTemplatePagedQuery.Code = $"%{EquMaintenanceTemplatePagedQuery.Code}%";
                sqlBuilder.Where("est.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(EquMaintenanceTemplatePagedQuery.Name))
            {
                EquMaintenanceTemplatePagedQuery.Name = $"%{EquMaintenanceTemplatePagedQuery.Name}%";
                sqlBuilder.Where("est.Name LIKE @Name");
            }

            if (EquMaintenanceTemplatePagedQuery.MaintenanceTemplateIds != null && EquMaintenanceTemplatePagedQuery.MaintenanceTemplateIds.Any())
            {
                sqlBuilder.Where("est.Id IN @MaintenanceTemplateIds");
            }

            if (EquMaintenanceTemplatePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("est.Status=@Status");
            }

            //if (!string.IsNullOrWhiteSpace(EquMaintenanceTemplatePagedQuery.EquipmentGroupCode))
            //{
            //    sqlBuilder.Where("egroup.EquipmentGroupCode=@EquipmentGroupCode");
            //}
            //if (!string.IsNullOrWhiteSpace(EquMaintenanceTemplatePagedQuery.EquipmentCode))
            //{
            //    sqlBuilder.Where("ee.EquipmentCode=@EquipmentCode");
            //}
            //if (EquMaintenanceTemplatePagedQuery.EquipmentId.HasValue)
            //{
            //    sqlBuilder.Where("ee.Id=@EquipmentId");
            //}

            var offSet = (EquMaintenanceTemplatePagedQuery.PageIndex - 1) * EquMaintenanceTemplatePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquMaintenanceTemplatePagedQuery.PageSize });
            sqlBuilder.AddParameters(EquMaintenanceTemplatePagedQuery);

            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateEntitiesTask = conn.QueryAsync<EquMaintenanceTemplateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquMaintenanceTemplateEntities = await EquMaintenanceTemplateEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenanceTemplateEntity>(EquMaintenanceTemplateEntities, EquMaintenanceTemplatePagedQuery.PageIndex, EquMaintenanceTemplatePagedQuery.PageSize, totalCount);


        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquMaintenanceTemplateQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateEntity>> GetEquMaintenanceTemplateEntitiesAsync(EquMaintenanceTemplateQuery EquMaintenanceTemplateQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquMaintenanceTemplateEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateEntities = await conn.QueryAsync<EquMaintenanceTemplateEntity>(template.RawSql, EquMaintenanceTemplateQuery);
            return EquMaintenanceTemplateEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenanceTemplateEntity EquMaintenanceTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, EquMaintenanceTemplateEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquMaintenanceTemplateEntity> EquMaintenanceTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, EquMaintenanceTemplateEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenanceTemplateEntity EquMaintenanceTemplateEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, EquMaintenanceTemplateEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquMaintenanceTemplateEntity> EquMaintenanceTemplateEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, EquMaintenanceTemplateEntitys);
        }
        #endregion

    }

    public partial class EquMaintenanceTemplateRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_Maintenance_template` est /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_Maintenance_template` est /**where**/ ";
        const string GetEquMaintenanceTemplateEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_Maintenance_template` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_Maintenance_template`(  `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_Maintenance_template`(  `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `equ_Maintenance_template` SET   Code = @Code, Name = @Name, Status = @Status, Version = @Version, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_Maintenance_template` SET   Code = @Code, Name = @Name, Status = @Status, Version = @Version, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_Maintenance_template` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_Maintenance_template` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_Maintenance_template`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_Maintenance_template`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT  
                               `Id`, `Code`, `Name`, `Status`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_Maintenance_template`  WHERE SiteId=@SiteId AND Code = @Code AND Version=@Version AND IsDeleted=0  ";
        #endregion
    }
}
