
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Qual;

namespace Hymson.MES.Data.Repositories.Quality
{
    public partial interface IQualOqcParameterGroupRepository
    {
        /// <summary>
        /// 数据插入（过滤出错行）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreAsync(QualOqcParameterGroupCreateCommand command);

        /// <summary>
        /// 数据批量插入（过滤出错行）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreAsync(IEnumerable<QualOqcParameterGroupCreateCommand> commands);

        #region 修改

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualOqcParameterGroupUpdateCommand command);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(IEnumerable<QualOqcParameterGroupUpdateCommand> commands);

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
        #endregion
    }
}
