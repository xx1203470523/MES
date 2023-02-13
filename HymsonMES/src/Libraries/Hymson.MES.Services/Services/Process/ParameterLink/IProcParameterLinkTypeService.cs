/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
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
    /// 标准参数关联类型表 service接口
    /// </summary>
    public interface IProcParameterLinkTypeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procParameterLinkTypePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterLinkTypeDto>> GetPageListAsync(ProcParameterLinkTypePagedQueryDto procParameterLinkTypePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterLinkTypeDto"></param>
        /// <returns></returns>
        Task CreateProcParameterLinkTypeAsync(ProcParameterLinkTypeCreateDto procParameterLinkTypeCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterLinkTypeDto"></param>
        /// <returns></returns>
        Task ModifyProcParameterLinkTypeAsync(ProcParameterLinkTypeModifyDto procParameterLinkTypeModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcParameterLinkTypeAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcParameterLinkTypeAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcParameterLinkTypeDto> QueryProcParameterLinkTypeByIdAsync(long id);
    }
}
