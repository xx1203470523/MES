/*
 *creator: Karl
 *
 *describe: 作业表    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 作业表 service接口
    /// </summary>
    public interface IInteJobService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteJobPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobDto>> GetPageListAsync(InteJobPagedQueryDto inteJobPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobDto"></param>
        /// <returns></returns>
        Task CreateInteJobAsync(InteJobCreateDto inteJobCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteJobDto"></param>
        /// <returns></returns>
        Task ModifyInteJobAsync(InteJobModifyDto inteJobModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteJobAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteJobAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobDto> QueryInteJobByIdAsync(long id);
    }
}
