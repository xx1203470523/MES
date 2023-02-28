/*
 *creator: Karl
 *
 *describe: BOM表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="procBomDto"></param>
        /// <returns></returns>
        Task CreateProcBomAsync(ProcBomCreateDto procBomCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomDto"></param>
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
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(long id);
    }
}
