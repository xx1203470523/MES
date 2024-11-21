using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.Resource
{
    /// <summary>
    /// 资源接口service
    /// </summary>
    public interface IProcResourceService
    {
        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceViewDto> GetByIdAsync(long id);

        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceViewDto>> GetPageListAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceViewDto>> GetPageListByScwAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceDto>> GetListAsync(ProcResourcePagedQueryDto query);


        /// <summary>
        /// 更具线体和工序查询资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceDto>> GetPageListBylineIdAndProcProcedureIdAsync(ProcResourcePagedlineIdAndProcProcedureIdDto query);

        /// <summary>
        /// 查询资源类型下关联的资源(资源类型详情：可用资源，已分配资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcResourceDto>> GetListForGroupAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 根据工序id查询资源列表(工序 > 资源类型 > 资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceViewDto>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQueryDto query);

        /// <summary>
        /// 资源关联打印机数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigPrintViewDto>> GetcResourceConfigPrintAsync(ProcResourceConfigPrintPagedQueryDto query);

        /// <summary>
        /// 资源设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigResDto>> GetcResourceConfigResAsync(ProcResourceConfigResPagedQueryDto query);

        /// <summary>
        /// 获取资源关联设备数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceEquipmentBindViewDto>> GetcResourceConfigEquAsync(ProcResourceEquipmentBindPagedQueryDto query);

        /// <summary>
        /// 获取资源关联作业
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto);

        /// <summary>
        /// 获取资源产出设置
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductSetDto>> GetResourceProductSetListAsync(ProcProductSetQueryDto queryDto);

        /// <summary>
        /// 获取资质认证设置
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcQualificationAuthenticationDto>> GetResourceAuthSetListAsync(long resourceId);

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> AddProcResourceAsync(ProcResourceCreateDto param);

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateProcResrouceAsync(ProcResourceModifyDto param);

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        Task<int> DeleteProcResourceAsync(long[] idsAr);

        /// <summary>
        /// 查询资源绑定的设备
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryEquipmentsByResourceIdAsync(long resourceId);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

    }
}
