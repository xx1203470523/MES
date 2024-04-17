

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：容器规格尺寸表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerSpecificationRepository :BaseRepository, IInteContainerSpecificationRepository
{
    

    public InteContainerSpecificationRepository(IOptions<ConnectionOptions> connectionOptions):base(connectionOptions)
    {
       
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<InteContainerSpecificationEntity> GetOneAsync(InteContainerSpecificationQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<InteContainerSpecificationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<InteContainerSpecificationEntity>> GetListAsync(InteContainerSpecificationQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<InteContainerSpecificationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<InteContainerSpecificationEntity>> GetPagedInfoAsync(InteContainerSpecificationPagedQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetPagedSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetCountSqlTemplate);

        WhereFill(sqlBuilder, query);

        if (!string.IsNullOrWhiteSpace(query.Sorting))
        {
            sqlBuilder.OrderBy(query.Sorting);
        }

        var offSet = (query.PageIndex - 1) * query.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = query.PageSize });
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        var inteContainerSpecificationEntities = await conn.QueryAsync<InteContainerSpecificationEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<InteContainerSpecificationEntity>(inteContainerSpecificationEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(InteContainerSpecificationCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<InteContainerSpecificationCreateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, commands);
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(InteContainerSpecificationUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<InteContainerSpecificationUpdateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, commands);
    }

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByIdSql, command);
    }

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteMoreAsync(DeleteMoreCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteMoreByIdSql, command);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：容器规格尺寸表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerSpecificationRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `inte_container_specification` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `inte_container_specification` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `inte_container_specification` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `inte_container_specification` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `inte_container_specification` (`Id`,`ContainerId`,`Height`,`Length`,`Width`,`MaxFillWeight`,`Weight`,`Remark`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`SiteId`) VALUES (@Id,@ContainerId,@Height,@Length,@Width,@MaxFillWeight,@Weight,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@SiteId);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `inte_container_specification` SET `ContainerId` = @ContainerId ,`Height` = @Height ,`Length` = @Length ,`Width` = @Width ,`MaxFillWeight` = @MaxFillWeight ,`Weight` = @Weight ,`Remark` = @Remark ,`SiteId` = @SiteId  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `inte_container_specification` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `inte_container_specification` SET IsDeleted = Id WHERE Id IN @Ids;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：容器规格尺寸表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerSpecificationRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, InteContainerSpecificationPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.ContainerId.HasValue)
        {
            sqlBuilder.Where("ContainerId = @ContainerId");
        }

        if (query.ContainerIds != null && query.ContainerIds.Any())
        {
            sqlBuilder.Where("ContainerId IN @ContainerIds");
        }


        if (query.HeightMin.HasValue)
        {
            sqlBuilder.Where("Height >= @HeightMin");
        }

        if (query.HeightMax.HasValue)
        {
            sqlBuilder.Where("Height <= @HeightMax");
        }


        if (query.LengthMin.HasValue)
        {
            sqlBuilder.Where("Length >= @LengthMin");
        }

        if (query.LengthMax.HasValue)
        {
            sqlBuilder.Where("Length <= @LengthMax");
        }


        if (query.WidthMin.HasValue)
        {
            sqlBuilder.Where("Width >= @WidthMin");
        }

        if (query.WidthMax.HasValue)
        {
            sqlBuilder.Where("Width <= @WidthMax");
        }


        if (query.MaxFillWeightMin.HasValue)
        {
            sqlBuilder.Where("MaxFillWeight >= @MaxFillWeightMin");
        }

        if (query.MaxFillWeightMax.HasValue)
        {
            sqlBuilder.Where("MaxFillWeight <= @MaxFillWeightMax");
        }


        if (query.WeightMin.HasValue)
        {
            sqlBuilder.Where("Weight >= @WeightMin");
        }

        if (query.WeightMax.HasValue)
        {
            sqlBuilder.Where("Weight <= @WeightMax");
        }


        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }


        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, InteContainerSpecificationQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.ContainerId.HasValue)
        {
            sqlBuilder.Where("ContainerId = @ContainerId");
        }

        if (query.ContainerIds != null && query.ContainerIds.Any())
        {
            sqlBuilder.Where("ContainerId IN @ContainerIds");
        }


        if (query.HeightMin.HasValue)
        {
            sqlBuilder.Where("Height >= @HeightMin");
        }

        if (query.HeightMax.HasValue)
        {
            sqlBuilder.Where("Height <= @HeightMax");
        }


        if (query.LengthMin.HasValue)
        {
            sqlBuilder.Where("Length >= @LengthMin");
        }

        if (query.LengthMax.HasValue)
        {
            sqlBuilder.Where("Length <= @LengthMax");
        }


        if (query.WidthMin.HasValue)
        {
            sqlBuilder.Where("Width >= @WidthMin");
        }

        if (query.WidthMax.HasValue)
        {
            sqlBuilder.Where("Width <= @WidthMax");
        }


        if (query.MaxFillWeightMin.HasValue)
        {
            sqlBuilder.Where("MaxFillWeight >= @MaxFillWeightMin");
        }

        if (query.MaxFillWeightMax.HasValue)
        {
            sqlBuilder.Where("MaxFillWeight <= @MaxFillWeightMax");
        }


        if (query.WeightMin.HasValue)
        {
            sqlBuilder.Where("Weight >= @WeightMin");
        }

        if (query.WeightMax.HasValue)
        {
            sqlBuilder.Where("Weight <= @WeightMax");
        }


        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }


        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：容器规格尺寸表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerSpecificationRepository
{
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：容器规格尺寸表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerSpecificationRepository
{
}