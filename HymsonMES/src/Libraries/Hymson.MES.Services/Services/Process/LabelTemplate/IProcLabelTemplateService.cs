using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
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
        /// <param name="procLabelTemplateCreateDto"></param>
        /// <returns></returns>
        Task CreateProcLabelTemplateAsync(ProcLabelTemplateCreateDto procLabelTemplateCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLabelTemplateModifyDto"></param>
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

        /// <summary>
        /// 查询标签模板对应的打印设计信息
        /// </summary>
        /// <param name="labelTemplateId"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateRelationDto?> QueryProcLabelTemplateRelationByLabelTemplateIdAsync(long labelTemplateId);

        /// <summary>
        /// 获取打印类对应的选项
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PrintClassOptionDto>> GetPrintClassListAsync();


        /// <summary>
        /// 获取对应任务的打印数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<PrintDataResultDto> GetAboutPrintDataAsync(long id);
    }
}
