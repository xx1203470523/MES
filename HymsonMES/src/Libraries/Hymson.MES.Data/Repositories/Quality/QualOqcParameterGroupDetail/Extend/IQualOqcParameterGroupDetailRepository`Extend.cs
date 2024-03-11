using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Qual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality;

public partial interface IQualOqcParameterGroupDetailRepository
{

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> InsertAsync(QualOqcParameterGroupDetailCreateCommand command);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<QualOqcParameterGroupDetailCreateCommand> commands);

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(QualOqcParameterGroupDetailUpdateCommand command);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> UpdateAsync(IEnumerable<QualOqcParameterGroupDetailUpdateCommand> commands);

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(DeleteCommand command);

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    Task<int> DeleteMoreAsync(DeleteCommand commands);

    /// <summary>
    /// 根据主表Id删除数据
    /// </summary>
    /// <param name="ParameterGroupId"></param>
    /// <returns></returns>
    Task<int> DeleteByMainIdAsync(long ParameterGroupId);

    /// <summary>
    /// 根据主表Id 批量删除数据
    /// </summary>
    /// <param name="ParameterGroupIds"></param>
    /// <returns></returns>
    Task<int> DeleteByMainIdsAsync(IEnumerable<long> ParameterGroupIds);

    #endregion
}
