using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（产品检验参数表）
    /// </summary>
    public partial class ProcProductParameterGroupRepository : BaseRepository, IProcProductParameterGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcProductParameterGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProductParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProductParameterGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProductParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcProductParameterGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcProductParameterGroupEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProductParameterGroupEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductParameterGroupEntity>> GetByProductProcedureListAsync(EntityByProductProcedureQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductParameterGroupEntity>(GetByProductProcedureSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProductParameterGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProductParameterGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductParameterGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductParameterGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductParameterGroupEntity>> GetEntitiesAsync(ProcProductParameterGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductParameterGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProductParameterGroupView>> GetPagedListAsync(ProcProductParameterGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("proc_material PM ON PM.Id = T.MaterialId");
            sqlBuilder.LeftJoin("proc_procedure PP ON PP.Id = T.ProcedureId");
            sqlBuilder.Select("T.*");
            sqlBuilder.Select("PM.MaterialCode, PM.MaterialName");
            sqlBuilder.Select("PP.Code AS ProcedureCode, PP.Name AS ProcedureName");
            sqlBuilder.OrderBy("T.UpdatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("T.Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("T.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("T.Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("PM.MaterialCode LIKE @MaterialCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialName))
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where("PM.MaterialName LIKE @MaterialName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureCode))
            {
                pagedQuery.ProcedureCode = $"%{pagedQuery.ProcedureCode}%";
                sqlBuilder.Where("PP.Code LIKE @ProcedureCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureName))
            {
                pagedQuery.ProcedureName = $"%{pagedQuery.ProcedureName}%";
                sqlBuilder.Where("PP.Name LIKE @ProcedureName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcProductParameterGroupView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProductParameterGroupView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

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

        /// <summary>
        /// 更改物料与工序的 非当前版本
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateSameMaterialIdProcedureIdToNoVersionAsync(ProcProductParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSameMaterialIdProcedureIdToNoVersionSql, entity);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcedureParamView>> GetParamListAsync(ProcedureParamQuery query)
        {
            string sql = $@"
                select t1.Code ,t1.Name ,t1.Version ,t2.UpperLimit ,t2.CenterValue ,t2.LowerLimit ,t1.UpdatedOn ,
	                t3.id ProcedureId , t3.Code procedureCode,t3.Name ProcedureName,t4.id parameterId, t4.ParameterName ,t4.ParameterCode,t4.DataType ,
                    t4.Remark,t4.ParameterUnit  
                from proc_product_parameter_group t1
                inner join proc_product_parameter_group_detail t2 on t1.Id = t2.ParameterGroupId  and t2.IsDeleted  = 0
                inner join proc_procedure t3 on t3.Id  = t1.ProcedureId and t3.IsDeleted = 0
                inner join proc_parameter t4 on t4.Id = t2.ParameterId and t4.IsDeleted = 0
                where t1.SiteId  = {query.SiteId}
                and t1.Status in (1,2)
                and t1.IsDeleted  = 0
            ";

            using var conn = GetMESDbConnection();
            var dbList = await conn.QueryAsync<ProcedureParamView>(sql);

            return dbList;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProductParameterGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_product_parameter_group` T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_product_parameter_group` T /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_product_parameter_group` /**where**/  ";
#if DM
        const string InsertSql = "INSERT  `proc_product_parameter_group`(`Id`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @IsDefaultVersion, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT  `proc_product_parameter_group`(`Id`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @IsDefaultVersion, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#else
   const string InsertSql = "INSERT IGNORE `proc_product_parameter_group`(`Id`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @IsDefaultVersion, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT IGNORE `proc_product_parameter_group`(`Id`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @IsDefaultVersion, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

#endif
        const string UpdateSql = "UPDATE `proc_product_parameter_group` SET Name = @Name, Version = @Version, IsDefaultVersion=@IsDefaultVersion, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_product_parameter_group` SET Name = @Name, Version = @Version, IsDefaultVersion=@IsDefaultVersion, Status = @Status, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_product_parameter_group` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_product_parameter_group` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByCodeSql = "SELECT * FROM proc_product_parameter_group WHERE IsDeleted = 0 AND SiteId = @Site AND Code = @Code AND Version = @Version LIMIT 1";
        const string GetByProductProcedureSql = "SELECT * FROM proc_product_parameter_group WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialId = @ProductId AND ProcedureId = @ProcedureId";
        const string GetByIdSql = @"SELECT * FROM `proc_product_parameter_group`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_product_parameter_group`  WHERE Id IN @Ids ";

        const string UpdateStatusSql = "UPDATE `proc_product_parameter_group` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
        const string UpdateSameMaterialIdProcedureIdToNoVersionSql = @"UPDATE `proc_product_parameter_group` SET IsDefaultVersion= 0 WHERE MaterialId= @MaterialId AND ProcedureId =@ProcedureId AND IsDeleted = 0 ";
    }
}
