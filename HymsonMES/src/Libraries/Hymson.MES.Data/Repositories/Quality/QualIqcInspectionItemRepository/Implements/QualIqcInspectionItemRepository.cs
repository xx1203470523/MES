

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Qual;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemRepository :BaseRepository, IQualIqcInspectionItemRepository
{
    private readonly ConnectionOptions _connectionOptions;

    public QualIqcInspectionItemRepository(IOptions<ConnectionOptions> connectionOptions):base(connectionOptions)
    {
        
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<QualIqcInspectionItemEntity> GetOneAsync(QualIqcInspectionItemQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<QualIqcInspectionItemEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualIqcInspectionItemEntity>> GetListAsync(QualIqcInspectionItemQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<QualIqcInspectionItemEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<QualIqcInspectionItemEntity>> GetPagedInfoAsync(QualIqcInspectionItemPagedQuery query)
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

        var qualIqcInspectionItemEntities = await conn.QueryAsync<QualIqcInspectionItemEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<QualIqcInspectionItemEntity>(qualIqcInspectionItemEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(QualIqcInspectionItemCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<QualIqcInspectionItemCreateCommand> commands)
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
    public async Task<int> UpdateAsync(QualIqcInspectionItemUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<QualIqcInspectionItemUpdateCommand> commands)
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
    public async Task<int> DeleteMoreAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteMoreByIdSql, command);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `qual_iqc_inspection_item` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `qual_iqc_inspection_item` (`Id`,`Code`,`Name`,`MaterialId`,`SupplierId`,`Status`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedOn`,`UpdatedBy`,`SiteId`,`IsDeleted`) VALUES (@Id,@Code,@Name,@MaterialId,@SupplierId,@Status,@Remark,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@SiteId,@IsDeleted);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `qual_iqc_inspection_item` SET `Code` = @Code ,`Name` = @Name ,`MaterialId` = @MaterialId ,`SupplierId` = @SupplierId ,`Status` = @Status ,`Remark` = @Remark ,`SiteId` = @SiteId  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `qual_iqc_inspection_item` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `qual_iqc_inspection_item` SET IsDeleted = Id WHERE Id IN @Ids;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, QualIqcInspectionItemPagedQuery query)
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


        if (query.MaterialId.HasValue)
        {
            sqlBuilder.Where("MaterialId = @MaterialId");
        }

        if (query.MaterialIds != null && query.MaterialIds.Any())
        {
            sqlBuilder.Where("MaterialId IN @MaterialIds");
        }


        if (query.SupplierId.HasValue)
        {
            sqlBuilder.Where("SupplierId = @SupplierId");
        }

        if (query.SupplierIds != null && query.SupplierIds.Any())
        {
            sqlBuilder.Where("SupplierId IN @SupplierIds");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
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


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, QualIqcInspectionItemQuery query)
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


        if (query.MaterialId.HasValue)
        {
            sqlBuilder.Where("MaterialId = @MaterialId");
        }

        if (query.MaterialIds != null && query.MaterialIds.Any())
        {
            sqlBuilder.Where("MaterialId IN @MaterialIds");
        }


        if (query.SupplierId.HasValue)
        {
            sqlBuilder.Where("SupplierId = @SupplierId");
        }

        if (query.SupplierIds != null && query.SupplierIds.Any())
        {
            sqlBuilder.Where("SupplierId IN @SupplierIds");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
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


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
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