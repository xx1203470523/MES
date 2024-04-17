

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
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemDetailRepository :BaseRepository, IQualIqcInspectionItemDetailRepository
{
    private readonly ConnectionOptions _connectionOptions;

    public QualIqcInspectionItemDetailRepository(IOptions<ConnectionOptions> connectionOptions):base(connectionOptions) 
    {
        
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<QualIqcInspectionItemDetailEntity> GetOneAsync(QualIqcInspectionItemDetailQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<QualIqcInspectionItemDetailEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualIqcInspectionItemDetailEntity>> GetListAsync(QualIqcInspectionItemDetailQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<QualIqcInspectionItemDetailEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<QualIqcInspectionItemDetailEntity>> GetPagedInfoAsync(QualIqcInspectionItemDetailPagedQuery query)
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

        var qualIqcInspectionItemDetailEntities = await conn.QueryAsync<QualIqcInspectionItemDetailEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<QualIqcInspectionItemDetailEntity>(qualIqcInspectionItemDetailEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(QualIqcInspectionItemDetailCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<QualIqcInspectionItemDetailCreateCommand> commands)
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
    public async Task<int> UpdateAsync(QualIqcInspectionItemDetailUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<QualIqcInspectionItemDetailUpdateCommand> commands)
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
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemDetailRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item_detail` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item_detail` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `qual_iqc_inspection_item_detail` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `qual_iqc_inspection_item_detail` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `qual_iqc_inspection_item_detail` (`Id`,`QualIqcInspectionItemId`,`ParameterId`,`Type`,`Utensil`,`Scale`,`LowerLimit`,`Center`,`UpperLimit`,`InspectionType`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedOn`,`UpdatedBy`,`SiteId`,`IsDeleted`) VALUES (@Id,@QualIqcInspectionItemId,@ParameterId,@Type,@Utensil,@Scale,@LowerLimit,@Center,@UpperLimit,@InspectionType,@Remark,@CreatedOn,@CreatedBy,@UpdatedOn,@UpdatedBy,@SiteId,@IsDeleted);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `qual_iqc_inspection_item_detail` SET `QualIqcInspectionItemId` = @QualIqcInspectionItemId ,`ParameterId` = @ParameterId ,`Type` = @Type ,`Utensil` = @Utensil ,`Scale` = @Scale ,`LowerLimit` = @LowerLimit ,`Center` = @Center ,`UpperLimit` = @UpperLimit ,`InspectionType` = @InspectionType ,`Remark` = @Remark ,`SiteId` = @SiteId  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `qual_iqc_inspection_item_detail` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `qual_iqc_inspection_item_detail` SET IsDeleted = Id WHERE Id IN @Ids;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：IQC检验项目详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public partial class QualIqcInspectionItemDetailRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, QualIqcInspectionItemDetailPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.QualIqcInspectionItemId.HasValue)
        {
            sqlBuilder.Where("QualIqcInspectionItemId = @QualIqcInspectionItemId");
        }

        if (query.QualIqcInspectionItemIds != null && query.QualIqcInspectionItemIds.Any())
        {
            sqlBuilder.Where("QualIqcInspectionItemId IN @QualIqcInspectionItemIds");
        }


        if (query.ParameterId.HasValue)
        {
            sqlBuilder.Where("ParameterId = @ParameterId");
        }

        if (query.ParameterIds != null && query.ParameterIds.Any())
        {
            sqlBuilder.Where("ParameterId IN @ParameterIds");
        }


        if (query.Type.HasValue)
        {
            sqlBuilder.Where("Type = @Type");
        }


        if (!string.IsNullOrWhiteSpace(query.Utensil))
        {
            sqlBuilder.Where("Utensil = @Utensil");
        }

        if (!string.IsNullOrWhiteSpace(query.UtensilLike))
        {
            query.UtensilLike = $"{query.UtensilLike}%";
            sqlBuilder.Where("Utensil Like @UtensilLike");
        }


        if (query.Scale.HasValue)
        {
            sqlBuilder.Where("Scale = @Scale");
        }

        if (query.Scales != null && query.Scales.Any())
        {
            sqlBuilder.Where("Scale IN @Scales");
        }


        if (query.LowerLimitMin.HasValue)
        {
            sqlBuilder.Where("LowerLimit >= @LowerLimitMin");
        }

        if (query.LowerLimitMax.HasValue)
        {
            sqlBuilder.Where("LowerLimit <= @LowerLimitMax");
        }


        if (query.CenterMin.HasValue)
        {
            sqlBuilder.Where("Center >= @CenterMin");
        }

        if (query.CenterMax.HasValue)
        {
            sqlBuilder.Where("Center <= @CenterMax");
        }


        if (query.UpperLimitMin.HasValue)
        {
            sqlBuilder.Where("UpperLimit >= @UpperLimitMin");
        }

        if (query.UpperLimitMax.HasValue)
        {
            sqlBuilder.Where("UpperLimit <= @UpperLimitMax");
        }


        if (!string.IsNullOrWhiteSpace(query.InspectionType))
        {
            sqlBuilder.Where("InspectionType = @InspectionType");
        }

        if (!string.IsNullOrWhiteSpace(query.InspectionTypeLike))
        {
            query.InspectionTypeLike = $"{query.InspectionTypeLike}%";
            sqlBuilder.Where("InspectionType Like @InspectionTypeLike");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, QualIqcInspectionItemDetailQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.QualIqcInspectionItemId.HasValue)
        {
            sqlBuilder.Where("QualIqcInspectionItemId = @QualIqcInspectionItemId");
        }

        if (query.QualIqcInspectionItemIds != null && query.QualIqcInspectionItemIds.Any())
        {
            sqlBuilder.Where("QualIqcInspectionItemId IN @QualIqcInspectionItemIds");
        }


        if (query.ParameterId.HasValue)
        {
            sqlBuilder.Where("ParameterId = @ParameterId");
        }

        if (query.ParameterIds != null && query.ParameterIds.Any())
        {
            sqlBuilder.Where("ParameterId IN @ParameterIds");
        }


        if (query.Type.HasValue)
        {
            sqlBuilder.Where("Type = @Type");
        }


        if (!string.IsNullOrWhiteSpace(query.Utensil))
        {
            sqlBuilder.Where("Utensil = @Utensil");
        }

        if (!string.IsNullOrWhiteSpace(query.UtensilLike))
        {
            query.UtensilLike = $"{query.UtensilLike}%";
            sqlBuilder.Where("Utensil Like @UtensilLike");
        }


        if (query.Scale.HasValue)
        {
            sqlBuilder.Where("Scale = @Scale");
        }

        if (query.Scales != null && query.Scales.Any())
        {
            sqlBuilder.Where("Scale IN @Scales");
        }


        if (query.LowerLimitMin.HasValue)
        {
            sqlBuilder.Where("LowerLimit >= @LowerLimitMin");
        }

        if (query.LowerLimitMax.HasValue)
        {
            sqlBuilder.Where("LowerLimit <= @LowerLimitMax");
        }


        if (query.CenterMin.HasValue)
        {
            sqlBuilder.Where("Center >= @CenterMin");
        }

        if (query.CenterMax.HasValue)
        {
            sqlBuilder.Where("Center <= @CenterMax");
        }


        if (query.UpperLimitMin.HasValue)
        {
            sqlBuilder.Where("UpperLimit >= @UpperLimitMin");
        }

        if (query.UpperLimitMax.HasValue)
        {
            sqlBuilder.Where("UpperLimit <= @UpperLimitMax");
        }


        if (!string.IsNullOrWhiteSpace(query.InspectionType))
        {
            sqlBuilder.Where("InspectionType = @InspectionType");
        }

        if (!string.IsNullOrWhiteSpace(query.InspectionTypeLike))
        {
            query.InspectionTypeLike = $"{query.InspectionTypeLike}%";
            sqlBuilder.Where("InspectionType Like @InspectionTypeLike");
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