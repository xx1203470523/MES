/*
 *creator: Karl
 *
 *describe: BOM明细表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
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
    /// BOM明细表 service接口
    /// </summary>
    public interface IProcBomDetailService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procBomDetailPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBomDetailDto>> GetPageListAsync(ProcBomDetailPagedQueryDto procBomDetailPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailDto"></param>
        /// <returns></returns>
        Task CreateProcBomDetailAsync(ProcBomDetailCreateDto procBomDetailCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomDetailDto"></param>
        /// <returns></returns>
        Task ModifyProcBomDetailAsync(ProcBomDetailModifyDto procBomDetailModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcBomDetailAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcBomDetailAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBomDetailDto> QueryProcBomDetailByIdAsync(long id);
    }
}
