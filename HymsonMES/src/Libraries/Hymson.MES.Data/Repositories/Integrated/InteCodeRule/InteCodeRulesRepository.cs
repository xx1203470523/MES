/*
 *creator: Karl
 *
 *describe: 编码规则 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Plan;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则仓储
    /// </summary>
    public partial class InteCodeRulesRepository : BaseRepository, IInteCodeRulesRepository
    {
        public InteCodeRulesRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

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
        /// <param name="param"></param>
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
        public async Task<InteCodeRulesEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCodeRulesEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesEntity> GetInteCodeRulesByProductIdAsync(InteCodeRulesByProductQuery param)
        {
            using var conn = GetMESDbConnection();
            var entity = await conn.QueryFirstOrDefaultAsync<InteCodeRulesEntity>(GetInteCodeRulesByProductIdSql, param);
            if (entity != null)
            {
                entity.ProductId = param.ProductId;
            }
            return entity;
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCodeRulesEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCodeRulesPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesPageView>> GetPagedInfoAsync(InteCodeRulesPagedQuery inteCodeRulesPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("cr.IsDeleted=0");
            sqlBuilder.Where("cr.SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(inteCodeRulesPagedQuery.MaterialCode))
            {
                inteCodeRulesPagedQuery.MaterialCode = $"%{inteCodeRulesPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" m.MaterialCode like @MaterialCode ");
            }

            if (!string.IsNullOrWhiteSpace(inteCodeRulesPagedQuery.MaterialVersion))
            {
                inteCodeRulesPagedQuery.MaterialVersion = $"%{inteCodeRulesPagedQuery.MaterialVersion}%";
                sqlBuilder.Where(" m.Version like @MaterialVersion ");
            }

            if (!string.IsNullOrWhiteSpace(inteCodeRulesPagedQuery.Remark))
            {
                inteCodeRulesPagedQuery.Remark = $"%{inteCodeRulesPagedQuery.Remark}%";
                sqlBuilder.Where(" cr.Remark like @Remark ");
            }

            if (inteCodeRulesPagedQuery.CodeType.HasValue)
            {
                sqlBuilder.Where(" cr.CodeType = @CodeType ");
            }

            if (!string.IsNullOrWhiteSpace(inteCodeRulesPagedQuery.ContainerCode))
            {
                inteCodeRulesPagedQuery.ContainerCode = $"%{inteCodeRulesPagedQuery.ContainerCode}%";
                sqlBuilder.Where(" c.CODE like @ContainerCode ");
            }

            if (!string.IsNullOrWhiteSpace(inteCodeRulesPagedQuery.ContainerName))
            {
                inteCodeRulesPagedQuery.ContainerName = $"%{inteCodeRulesPagedQuery.ContainerName}%";
                sqlBuilder.Where(" c.NAME like @ContainerName ");
            }

            var offSet = (inteCodeRulesPagedQuery.PageIndex - 1) * inteCodeRulesPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteCodeRulesPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteCodeRulesPagedQuery);

            using var conn = GetMESDbConnection();
            var inteCodeRulesEntitiesTask = conn.QueryAsync<InteCodeRulesPageView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteCodeRulesEntities = await inteCodeRulesEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCodeRulesPageView>(inteCodeRulesEntities, inteCodeRulesPagedQuery.PageIndex, inteCodeRulesPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List like
        /// </summary>
        /// <param name="inteCodeRulesQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesEntity>> GetInteCodeRulesEntitiesAsync(InteCodeRulesQuery inteCodeRulesQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCodeRulesEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteCodeRulesEntities = await conn.QueryAsync<InteCodeRulesEntity>(template.RawSql, inteCodeRulesQuery);
            return inteCodeRulesEntities;
        }

        /// <summary>
        /// 查询List 查询条件为等于的
        /// </summary>
        /// <param name="inteCodeRulesQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesEntity>> GetInteCodeRulesEntitiesEqualAsync(InteCodeRulesQuery inteCodeRulesQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCodeRulesEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (inteCodeRulesQuery.ProductId > 0)
            {
                sqlBuilder.Where(" ProductId=@ProductId ");
            }
            if (inteCodeRulesQuery.CodeType.HasValue)
            {
                sqlBuilder.Where(" CodeType=@CodeType ");
            }
            if (inteCodeRulesQuery.PackType.HasValue) {
                sqlBuilder.Where(" PackType=@PackType ");
            }

            using var conn = GetMESDbConnection();
            var inteCodeRulesEntities = await conn.QueryAsync<InteCodeRulesEntity>(template.RawSql, inteCodeRulesQuery);
            return inteCodeRulesEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCodeRulesEntity inteCodeRulesEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteCodeRulesEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCodeRulesEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<InteCodeRulesEntity> inteCodeRulesEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteCodeRulesEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCodeRulesEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCodeRulesEntity inteCodeRulesEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteCodeRulesEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteCodeRulesEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<InteCodeRulesEntity> inteCodeRulesEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteCodeRulesEntitys);
        }

    }

    public partial class InteCodeRulesRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT
	                                    cr.*,
                                        crm.MaterialId,
	                                    m.MaterialCode,
	                                    m.MaterialName,
	                                    m.Version AS MaterialVersion,
	                                    c.CODE AS ContainerCode,
	                                    c.NAME AS ContainerName 
                                    FROM
	                                    `inte_code_rules` cr
                                        LEFT JOIN inte_code_rules_material crm ON crm.CodeRulesId = cr.Id AND crm.IsDeleted = 0
	                                    LEFT JOIN proc_material m ON crm.MaterialId = m.Id
	                                    LEFT JOIN inte_container_info c ON cr.ContainerInfoId = c.Id
                                    /**where**/ Order by cr.CreatedOn desc LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) 
                                            FROM `inte_code_rules` cr
                                            LEFT JOIN inte_code_rules_material crm ON crm.CodeRulesId = cr.Id AND crm.IsDeleted = 0
                                            LEFT JOIN proc_material m ON crm.MaterialId = m.Id
                                            LEFT JOIN inte_container_info c ON cr.ContainerInfoId = c.Id
                                            /**where**/ ";
        const string GetInteCodeRulesEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_code_rules` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_code_rules`(  `Id`, `ProductId`, `CodeType`, CodeMode, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, StartNumber, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @ProductId, @CodeType, @CodeMode, @PackType, @Base, @IgnoreChar, @Increment, @OrderLength, @ResetType, @StartNumber, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_code_rules`(  `Id`, `ProductId`, `CodeType`, CodeMode, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, StartNumber, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @ProductId, @CodeType, @CodeMode, @PackType, @Base, @IgnoreChar, @Increment, @OrderLength, @ResetType, @StartNumber, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `inte_code_rules` SET   ProductId = @ProductId, CodeType = @CodeType, CodeMode=@CodeMode, PackType = @PackType, Base = @Base, IgnoreChar = @IgnoreChar, Increment = @Increment, OrderLength = @OrderLength, ResetType = @ResetType, StartNumber=@StartNumber, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_code_rules` SET   ProductId = @ProductId, CodeType = @CodeType, CodeMode=@CodeMode, PackType = @PackType, Base = @Base, IgnoreChar = @IgnoreChar, Increment = @Increment, OrderLength = @OrderLength, ResetType = @ResetType, StartNumber=@StartNumber, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_code_rules` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_code_rules` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT * FROM `inte_code_rules`  WHERE Id = @Id ";
        const string GetInteCodeRulesByProductIdSql = @"SELECT icr.* FROM `inte_code_rules` icr LEFT JOIN inte_code_rules_material icrm ON icrm.CodeRulesId = icr.Id AND icrm.IsDeleted = 0 WHERE icrm.MaterialId = @ProductId AND icr.CodeType=@CodeType AND icr.IsDeleted=0 ";
        const string GetByIdsSql = @"SELECT * FROM `inte_code_rules`  WHERE Id IN @ids ";
    }
}
