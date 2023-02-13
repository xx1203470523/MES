/*
 *creator: Karl
 *
 *describe: 标准参数表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
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
    /// 标准参数表 service接口
    /// </summary>
    public interface IProcParameterService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procParameterPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterDto>> GetPageListAsync(ProcParameterPagedQueryDto procParameterPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterDto"></param>
        /// <returns></returns>
        Task CreateProcParameterAsync(ProcParameterCreateDto procParameterCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterDto"></param>
        /// <returns></returns>
        Task ModifyProcParameterAsync(ProcParameterModifyDto procParameterModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcParameterAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcParameterAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcParameterDto> QueryProcParameterByIdAsync(long id);
    }
}
