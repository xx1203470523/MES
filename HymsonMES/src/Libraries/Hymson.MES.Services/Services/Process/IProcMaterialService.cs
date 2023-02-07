/*
 *creator: Karl
 *
 *describe: 物料维护    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
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
    /// 物料维护 service接口
    /// </summary>
    public interface IProcMaterialService
    {
		/// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialDto>> GetListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

		/// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		Task CreateProcMaterialAsync(ProcMaterialDto procMaterialDto);

		/// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		Task ModifyProcMaterialAsync(ProcMaterialDto procMaterialDto);

		/// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		Task DeleteProcMaterialAsync(long id);
    }
}
