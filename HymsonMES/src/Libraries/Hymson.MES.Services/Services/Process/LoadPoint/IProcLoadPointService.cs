/*
 *creator: Karl
 *
 *describe: 上料点表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
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
    /// 上料点表 service接口
    /// </summary>
    public interface IProcLoadPointService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procLoadPointPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLoadPointDto>> GetPageListAsync(ProcLoadPointPagedQueryDto procLoadPointPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        Task CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        Task ModifyProcLoadPointAsync(ProcLoadPointModifyDto procLoadPointModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcLoadPointAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcLoadPointAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLoadPointDto> QueryProcLoadPointByIdAsync(long id);
    }
}
