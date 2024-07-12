using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Microsoft.Extensions.Options;



namespace Hymson.MES.Data.Repositories.Integrated.InteContainer
{
    /// <summary>
    /// 仓储（容器维护）
    /// </summary>
    public partial class InteContainerRepository :BaseRepository, IInteContainerRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteContainerRepository(IOptions<ConnectionOptions> connectionOptions):base(connectionOptions)
        {
            
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteContainerEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 容器信息新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertInfoAsync(InteContainerInfoEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertInfoSql, entity);
        }

        /// <summary>
        /// 容器规格新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertSpecificationAsync(InteContainerSpecificationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSpecificationSql, entity);
        }

        /// <summary>
        /// 容器货物新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertFreightAsync(IEnumerable<InteContainerFreightEntity> entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertFreightSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteContainerInfoEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);

        }

        /// <summary>
        /// 删除（批量、容器货物）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByParentId, command);
        }

        /// <summary>
        /// 删除（容器参数）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSpecificationByParentIdAsync(DeleteByParentIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSpecificationByParentId, command);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteContainerInfoEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteContainerInfoEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteContainerEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取Info数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerInfoEntity> GetContainerInfoByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteContainerInfoEntity>(GetInfoByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据容器ID获取容器规格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerSpecificationEntity> GetSpecificationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteContainerSpecificationEntity>(GetSpecificationByIdSql, new { Id = id,IsDeleted=0 });
        }

        /// <summary>
        /// 查询Freight列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteContainerFreightEntity>> GetFreightAsync(EntityByParentIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("ContainerId = @ParentId");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteContainerFreightEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据IDs获取批量数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteContainerInfoEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteContainerInfoEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 通过关联ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteContainerEntity> GetByRelationIdAsync(InteContainerQuery query)
        {
            var sql = GetByMaterialIdSql;
            if (query.DefinitionMethod == DefinitionMethodEnum.MaterialGroup) sql = GetByMaterialGroupIdSql;
            //是否转入状态条件
            if (query.Status.HasValue)
            {
                sql += " AND Status = @Status";
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteContainerEntity>(sql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteContainerView>> GetPagedInfoAsync(InteContainerPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("ICI.IsDeleted = 0");
            sqlBuilder.Where("ICI.SiteId = @SiteId");
            sqlBuilder.OrderBy("ICI.UpdatedOn DESC");

            sqlBuilder.Select("ICI.Id, ICI.Name, ICI.Code, ICI.Remark, ICI.Status, ICI.UpdatedBy, ICI.UpdatedBy, ICI.UpdatedOn");

            //sqlBuilder.LeftJoin("inte_container_freight ICF ON ICI.Id = ICF.ContainerId");
            //sqlBuilder.LeftJoin("inte_container_specification ICS ON ICS.ContainerId = ICI.Id");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("(ICI.Name LIKE @Name)");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("(ICI.Code LIKE @Code)");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("ICI.Status = @Status");
            }


            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<InteContainerView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<InteContainerView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
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

        public Task<InteContainerView> GetInfoByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteContainerRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_container_info ICI /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";

        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_container_info ICI /**innerjoin**/ /**leftjoin**/ /**where**/";

        const string InsertInfoSql = "INSERT INTO inte_container_info(  `Id`, `Code`, `Name`, `Remark`, `Status`,`CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,`SiteId`) VALUES (  @Id, @Code, @Name, @Remark, @Status, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted,@SiteId) ";

        const string InsertSpecificationSql = "INSERT INTO `inte_container_specification`( `Id`, `ContainerId`, `Height`, `Length`, Width, `MaxFillWeight`, `Weight`, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, SiteId) VALUES (   @Id, @ContainerId, @Height, @Length, @Width, @MaxFillWeight, @Weight, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId)  ";
        const string InsertFreightSql = "INSERT INTO inte_container_freight(  `Id`, `Type`,`ContainerId`, `MaterialId`, `MaterialGroupId`,`FreightContainerId`,`Minimum`,`Maximum`,`Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,SiteId,`LevelValue`) VALUES (  @Id,@Type,@ContainerId, @MaterialId, @MaterialGroupId,@FreightContainerId,@Minimum,@Maximum,@Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted,@SiteId,@LevelValue) ";

        const string InsertSql = "INSERT INTO `inte_container`( `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, Level, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, SiteId) VALUES (   @Id, @DefinitionMethod, @MaterialId, @MaterialGroupId, @Level, @Status, @Maximum, @Minimum, @Height, @Length, @Width, @MaxFillWeight, @Weight, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId)  ";
        const string UpdateSql = "UPDATE `inte_container_info` SET Code = @Code, Name = @Name, Remark = @Remark, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE inte_container_info SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";

        const string GetByIdSql = @"SELECT 
                               `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, Level, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_container`  WHERE Id = @Id ";

        const string GetInfoByIdSql = @"SELECT 
                                `Id`,`Code`,`Name`,`Remark`,`Status`
                            FROM `inte_container_info`  WHERE Id = @Id ";

        const string GetSpecificationByIdSql = @"SELECT 
                               `Height`,`Length`,`Width`, `MaxFillWeight`, `Weight`
                               FROM `inte_container_specification`  WHERE ContainerId = @Id and IsDeleted=@IsDeleted ";
        const string GetByIdsSql = @"SELECT 
                            *
                            FROM `inte_container_info`  WHERE Id IN @Ids ";

        const string GetByMaterialIdSql = @"SELECT * FROM inte_container WHERE IsDeleted = 0 AND DefinitionMethod = @DefinitionMethod AND MaterialId = @MaterialId   AND Level = @Level ";

        const string GetByMaterialGroupIdSql = @"SELECT * FROM inte_container WHERE IsDeleted = 0 AND DefinitionMethod = @DefinitionMethod AND MaterialGroupId = @MaterialGroupId AND Level = @Level ";
        const string GetByCodeSql = "SELECT * FROM inte_container_info WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string UpdateStatusSql = "UPDATE `inte_container_info` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_container_freight /**where**/  ";
        const string DeleteByParentId = "UPDATE  inte_container_freight SET `IsDeleted`=Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE ContainerId = @ParentId";
        const string DeleteSpecificationByParentId = "UPDATE  inte_container_specification SET `IsDeleted`=Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE ContainerId = @ParentId";
    }
}
