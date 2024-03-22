using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ResourceType.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ResourceType
{
    public interface IProcResourceTypeRepository
    {
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceTypeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 查询资源类型是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ProcResourceTypeEntity> GetByCodeAsync(ProcResourceTypeEntity param);

        /// <summary>
        ///  查询资源类型维护表列表(关联资源：一个类型被多个资源关联就展示多条)
        /// </summary>
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceTypeView>> GetPageListNewAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery);

        /// <summary>
        /// 获取资源类型分页列表
        /// </summary>
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceTypeEntity>> GetListAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery);

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="resourceTypeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcResourceTypeAddCommand addCommand);

        /// <summary>
        /// 更新资源类型维护数据
        /// </summary>
        /// <param name="updateCommand"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceTypeUpdateCommand updateCommand);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="resourceTypeEntities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcResourceTypeEntity> resourceTypeEntities);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="resourceTypeEntities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcResourceTypeEntity> resourceTypeEntities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="resourceTypeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceTypeEntity>> GetEntitiesAsync(ProcResourceTypeQuery resourceTypeQuery);
    }
}
