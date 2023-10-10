/*
 *creator: Karl
 *
 *describe: 条码步骤表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-22 05:17:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表仓储接口
    /// </summary>
    public interface IManuSfcStepRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcStepEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GeListtByStartwaterMarkIdAsync(ManuSfcStepStatisticQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcStepQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetManuSfcStepEntitiesAsync(ManuSfcStepQuery manuSfcStepQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoAsync(ManuSfcStepPagedQuery manuSfcStepPagedQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcStepEntity manuSfcStepEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcStepEntity>? manuSfcStepEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcStepEntity manuSfcStepEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcStepEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcStepEntity> manuSfcStepEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntitie"></param>
        /// <returns></returns>
        Task<int> InsertSfcStepBusinessAsync(MaunSfcStepBusinessEntity maunSfcStepBusinessEntitie);

        /// <summary>
        /// 批量插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntities"></param>
        /// <returns></returns>
        Task<int> InsertSfcStepBusinessRangeAsync(IEnumerable<MaunSfcStepBusinessEntity> maunSfcStepBusinessEntities);

        /// <summary>
        /// 获取SFC的进出站步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetSFCInOutStepAsync(SfcInOutStepQuery sfcQuery);

        /// <summary>
        /// 分页查询 根据SFC
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoBySFCAsync(ManuSfcStepBySfcPagedQuery queryParam);
    }
}
