

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
/// <para>@描述：容器维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerInfoRepository :BaseRepository, IInteContainerInfoRepository
{
  

    public InteContainerInfoRepository(IOptions<ConnectionOptions> connectionOptions):base(connectionOptions)
    {
       
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<InteContainerInfoEntity> GetOneAsync(InteContainerInfoQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<InteContainerInfoEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<InteContainerInfoEntity>> GetListAsync(InteContainerInfoQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<InteContainerInfoEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<InteContainerInfoEntity>> GetPagedInfoAsync(InteContainerInfoPagedQuery query)
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

        var inteContainerInfoEntities = await conn.QueryAsync<InteContainerInfoEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<InteContainerInfoEntity>(inteContainerInfoEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(InteContainerInfoCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<InteContainerInfoCreateCommand> commands)
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
    public async Task<int> UpdateAsync(InteContainerInfoUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<InteContainerInfoUpdateCommand> commands)
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
/// <para>@描述：容器维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerInfoRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `inte_container_info` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `inte_container_info` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `inte_container_info` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `inte_container_info` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `inte_container_info` (`Id`,`Code`,`Name`,`Remark`,`Status`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`SiteId`) VALUES (@Id,@Code,@Name,@Remark,@Status,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@SiteId);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `inte_container_info` SET `Code` = @Code ,`Name` = @Name ,`Remark` = @Remark ,`Status` = @Status ,`SiteId` = @SiteId  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `inte_container_info` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `inte_container_info` SET IsDeleted = Id WHERE Id IN @Ids;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：容器维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerInfoRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, InteContainerInfoPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.Code))
        {
            sqlBuilder.Where("Code = @Code");
        }

        if (!string.IsNullOrWhiteSpace(query.CodeLike))
        {
            query.CodeLike = $"%{query.CodeLike}%";
            sqlBuilder.Where("Code Like @CodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlBuilder.Where("Name = @Name");
        }

        if (!string.IsNullOrWhiteSpace(query.NameLike))
        {
            query.NameLike = $"%{query.NameLike}%";
            sqlBuilder.Where("Name Like @NameLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, InteContainerInfoQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.Code))
        {
            sqlBuilder.Where("Code = @Code");
        }

        if (!string.IsNullOrWhiteSpace(query.CodeLike))
        {
            query.CodeLike = $"{query.CodeLike}%";
            sqlBuilder.Where("Code Like @CodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlBuilder.Where("Name = @Name");
        }

        if (!string.IsNullOrWhiteSpace(query.NameLike))
        {
            query.NameLike = $"{query.NameLike}%";
            sqlBuilder.Where("Name Like @NameLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
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
/// <para>@描述：容器维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerInfoRepository
{
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：容器维护;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public partial class InteContainerInfoRepository
{
}