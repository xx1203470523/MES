/*
 *creator: Karl
 *
 *describe: 仓库标签模板    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process
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

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id);
    }
}
