using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：编码规则;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-25</para>
/// </summary>
public partial class InteCodeRulesRepository 
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<InteCodeRulesEntity> GetOneAsync(InteCodeRulesReQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<InteCodeRulesEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<InteCodeRulesEntity>> GetListAsync(InteCodeRulesReQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<InteCodeRulesEntity>(templateData.RawSql, templateData.Parameters);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertReAsync(InteCodeRulesEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertReSql, entity);
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateReAsync(InteCodeRulesEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdReSql, entity);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：编码规则;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-25</para>
/// </summary>
public partial class InteCodeRulesRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `inte_code_rules` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `inte_code_rules` /**where**/;";

    #endregion

    #region 新增

    const string InsertReSql = "INSERT INTO `inte_code_rules` (`Id`,`ProductId`,`CodeType`,`CodeMode`,`PackType`,`Base`,`IgnoreChar`,`Increment`,`OrderLength`,`ResetType`,`StartNumber`,`ContainerInfoId`,`Remark`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`SiteId`) VALUES (@Id,@ProductId,@CodeType,@CodeMode,@PackType,@Base,@IgnoreChar,@Increment,@OrderLength,@ResetType,@StartNumber,@ContainerInfoId,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@SiteId);";

    #endregion


    #region 修改

    const string UpdateByIdReSql = "UPDATE `inte_code_rules` SET `ProductId` = @ProductId ,`CodeType` = @CodeType ,`CodeMode` = @CodeMode ,`PackType` = @PackType ,`Base` = @Base ,`IgnoreChar` = @IgnoreChar ,`Increment` = @Increment ,`OrderLength` = @OrderLength ,`ResetType` = @ResetType ,`StartNumber` = @StartNumber ,`ContainerInfoId` = @ContainerInfoId ,`Remark` = @Remark ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion
}