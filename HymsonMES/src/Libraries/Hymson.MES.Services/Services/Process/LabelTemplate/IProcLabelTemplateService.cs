using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.LabelTemplate
{
    /// <summary>
    /// 仓库标签模板 service接口
    /// </summary>
    public interface IProcLabelTemplateService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procLabelTemplatePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLabelTemplateDto>> GetPageListAsync(ProcLabelTemplatePagedQueryDto procLabelTemplatePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLabelTemplateDto"></param>
        /// <returns></returns>
        Task CreateProcLabelTemplateAsync(ProcLabelTemplateCreateDto procLabelTemplateCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLabelTemplateDto"></param>
        /// <returns></returns>
        Task ModifyProcLabelTemplateAsync(ProcLabelTemplateModifyDto procLabelTemplateModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcLabelTemplateAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        Task<int> DeletesProcLabelTemplateAsync(long[] idsAr);
        Task<(string base64Str, bool result)> PreviewProcLabelTemplateAsync(long id);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id);
    }
}
