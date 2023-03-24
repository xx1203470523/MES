/*
 *creator: Karl
 *
 *describe: 条码信息表    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码信息表 service接口
    /// </summary>
    public interface IManuSfcInfoService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcInfoDto>> GetPageListAsync(ManuSfcInfoPagedQueryDto manuSfcInfoPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcInfoDto"></param>
        /// <returns></returns>
        Task CreateManuSfcInfoAsync(ManuSfcInfoCreateDto manuSfcInfoCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcInfoDto"></param>
        /// <returns></returns>
        Task ModifyManuSfcInfoAsync(ManuSfcInfoModifyDto manuSfcInfoModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuSfcInfoAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuSfcInfoAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcInfoDto> QueryManuSfcInfoByIdAsync(long id);
    }
}
