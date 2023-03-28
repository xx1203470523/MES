/*
 *creator: Karl
 *
 *describe: 条码信息表仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码信息表仓储接口
    /// </summary>
    public interface IManuSfcInfoRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcInfoEntity manuSfcInfoEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcInfoEntity> manuSfcInfoEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcInfoEntity manuSfcInfoEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcInfoEntity> manuSfcInfoEntitys);

        /// <summary>
        /// 批量更新条码状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ManuSfcInfoUpdateCommand command);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcInfoQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetManuSfcInfoEntitiesAsync(ManuSfcInfoQuery manuSfcInfoQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfoPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcInfoEntity>> GetPagedInfoAsync(ManuSfcInfoPagedQuery manuSfcInfoPagedQuery);
    }
}
