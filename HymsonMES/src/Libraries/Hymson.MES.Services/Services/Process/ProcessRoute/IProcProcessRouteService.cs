/*
 *creator: Karl
 *
 *describe: 工艺路线表    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.ProcessRoute
{
    /// <summary>
    /// 工艺路线表 service接口
    /// </summary>
    public interface IProcProcessRouteService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessRouteDto>> GetPageListAsync(ProcProcessRoutePagedQueryDto procProcessRoutePagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomProcessRouteDto> GetCustomProcProcessRouteAsync(long id);

        /// <summary>
        /// 根据ID查询工艺路线工序列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProcProcessRouteDetailNodeViewDto>> GetNodesByRouteIdAsync(long id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        Task AddProcProcessRouteAsync(ProcProcessRouteCreateDto procProcessRouteDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        Task UpdateProcProcessRouteAsync(ProcProcessRouteModifyDto procProcessRouteDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcessRouteAsync(long[] idsAr);

        /// <summary>
        /// 根据不合个工艺路线Id查询不合格工艺路线列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
         Task<IEnumerable<ProcProcessRouteDto>> GetListByIdsAsync(long[] ids);
    }
}
