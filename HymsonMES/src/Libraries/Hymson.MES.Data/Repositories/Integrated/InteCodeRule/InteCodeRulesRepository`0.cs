using Dapper;

namespace Hymson.MES.Data.Repositories.Integrated;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：编码规则;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-25</para>
/// </summary>
public partial class InteCodeRulesRepository
{
    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, InteCodeRulesReQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.ProductId.HasValue)
        {
            sqlBuilder.Where("ProductId = @ProductId");
        }

        if (query.ProductIds != null && query.ProductIds.Any())
        {
            sqlBuilder.Where("ProductId IN @ProductIds");
        }


        if (query.CodeType.HasValue)
        {
            sqlBuilder.Where("CodeType = @CodeType");
        }


        if (query.CodeMode.HasValue)
        {
            sqlBuilder.Where("CodeMode = @CodeMode");
        }


        if (query.PackType.HasValue)
        {
            sqlBuilder.Where("PackType = @PackType");
        }


        if (!string.IsNullOrWhiteSpace(query.IgnoreChar))
        {
            sqlBuilder.Where("IgnoreChar = @IgnoreChar");
        }

        if (!string.IsNullOrWhiteSpace(query.IgnoreCharLike))
        {
            query.IgnoreCharLike = $"{query.IgnoreCharLike}%";
            sqlBuilder.Where("IgnoreChar Like @IgnoreCharLike");
        }


        if (query.ResetType.HasValue)
        {
            sqlBuilder.Where("ResetType = @ResetType");
        }


        if (query.ContainerInfoId.HasValue)
        {
            sqlBuilder.Where("ContainerInfoId = @ContainerInfoId");
        }

        if (query.ContainerInfoIds != null && query.ContainerInfoIds.Any())
        {
            sqlBuilder.Where("ContainerInfoId IN @ContainerInfoIds");
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