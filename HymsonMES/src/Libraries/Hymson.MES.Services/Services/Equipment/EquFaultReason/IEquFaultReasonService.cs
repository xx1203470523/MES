/*
 *creator: pengxin
 *
 *describe: 设备故障原因表    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Equipment 
{
    /// <summary>
    /// 设备故障原因表 service接口
    /// </summary>
    public interface IEquFaultReasonService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="EquFaultReasonPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto EquFaultReasonPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquFaultReasonDto"></param>
        /// <returns></returns>
        Task<int> CreateEquFaultReasonAsync(EquFaultReasonCreateDto EquFaultReasonCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquFaultReasonDto"></param>
        /// <returns></returns>
        Task ModifyEquFaultReasonAsync(EquFaultReasonModifyDto EquFaultReasonModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquFaultReasonAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquFaultReasonAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id);
    }
}
