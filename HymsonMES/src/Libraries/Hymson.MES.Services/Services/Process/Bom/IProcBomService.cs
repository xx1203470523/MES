using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// BOM表 service接口
    /// </summary>
    public interface IProcBomService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procBomPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBomDto>> GetPageListAsync(ProcBomPagedQueryDto procBomPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomCreateDto"></param>
        /// <returns></returns>
        Task<long> CreateProcBomAsync(ProcBomCreateDto procBomCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcBomAsync(ProcBomModifyDto procBomModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcBomAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcBomAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBomDto> QueryProcBomByIdAsync(long id);

        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(long bomId);
        Task<List<ProcBomDetailView>> GetPickBomMaterialAsync(long orderId);

        Task<List<ProcOrderBomDetailDto>> GetOrderBomMaterialAsync(long orderId);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入Bom录入表格
        /// </summary>
        /// <returns></returns>
        Task ImportBomAsync(IFormFile formFile);

        /// <summary>
        /// 根据查询条件导出Bom信息
        /// </summary>
        /// <returns></returns>
        Task<BomExportResultDto> ExprotBomPageListAsync(ProcBomPagedQuery param);

        /// <summary>
        /// 判断bom是否被激活工单引用
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        Task<bool> JudgeBomIsReferencedByActivatedWorkOrder(long bomId);
    }
}
