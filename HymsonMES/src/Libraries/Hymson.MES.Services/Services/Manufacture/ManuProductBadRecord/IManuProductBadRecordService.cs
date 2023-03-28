/*
 *creator: Karl
 *
 *describe: 产品不良录入    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
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
    /// 产品不良录入 service接口
    /// </summary>
    public interface IManuProductBadRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuProductBadRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductBadRecordDto>> GetPageListAsync(ManuProductBadRecordPagedQueryDto manuProductBadRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuProductBadRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuProductBadRecordAsync(ManuProductBadRecordCreateDto manuProductBadRecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuProductBadRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuProductBadRecordAsync(ManuProductBadRecordModifyDto manuProductBadRecordModifyDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesManuProductBadRecordAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id);
    }
}
