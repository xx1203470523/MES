using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    ///工具管理仓储接口
    /// </summary>
    public interface IEquToolingManageRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procConversionFactorEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcConversionFactorEntity procConversionFactorEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcLoadPointEntity> procLoadPointEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcConversionFactorEntity procLoadPointEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcLoadPointEntity> procLoadPointEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolingManageView> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcConversionFactorEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointEntity>> GetByResourceIdAsync(long resourceId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcConversionFactorEntity>> GetProcConversionFactorEntitiesAsync(IEquToolingManagePagedQuery procConversionFactorQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolingManageView>> GetPagedInfoAsync(IEquToolingManagePagedQuery procLoadPointPagedQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

    }
}
