using Dapper;
using Hymson.MES.Data.Repositories.Common.Command;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Quality;

public partial class QualOqcParameterGroupDetailRepository
{
    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(QualOqcParameterGroupDetailCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<QualOqcParameterGroupDetailCreateCommand> commands)
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
    public async Task<int> UpdateAsync(QualOqcParameterGroupDetailUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<QualOqcParameterGroupDetailUpdateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, commands);
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
        return await conn.ExecuteAsync(DeleteSql, command);
    }

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteMoreAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeletesSql, command);
    }

    /// <summary>
    /// 根据主表Id删除数据
    /// </summary>
    /// <param name="qualIqcInspectionItemId"></param>
    /// <returns></returns>
    public async Task<int> DeleteByMainIdAsync(long ParameterGroupId)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByMainIdSql, new { ParameterGroupId = ParameterGroupId });
    }

    /// <summary>
    /// 根据主表Id 批量删除数据
    /// </summary>
    /// <param name="qualIqcInspectionItemIds"></param>
    /// <returns></returns>
    public async Task<int> DeleteByMainIdsAsync(IEnumerable<long> ParameterGroupIds)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByMainIdsSql, new { ParameterGroupId = ParameterGroupIds });
    }

    #endregion
}
