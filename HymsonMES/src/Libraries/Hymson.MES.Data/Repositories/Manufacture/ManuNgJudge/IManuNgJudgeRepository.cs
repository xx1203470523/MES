/*
 *creator: Karl
 *
 *describe: 托盘条码关系仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘条码关系仓储接口
    /// </summary>
    public interface IManuNgJudgeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcNgJudgeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcNgJudgeEntity manuSfcNgJudgeEntity);
        #endregion
    }
}
