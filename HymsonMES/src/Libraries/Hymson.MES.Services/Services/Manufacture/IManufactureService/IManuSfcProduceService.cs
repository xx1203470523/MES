/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
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
    /// 条码生产信息（物理删除） service接口
    /// </summary>
    public interface IManuSfcProduceService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcProduceDto"></param>
        /// <returns></returns>
        Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceDto"></param>
        /// <returns></returns>
        Task ModifyManuSfcProduceAsync(ManuSfcProduceModifyDto manuSfcProduceModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuSfcProduceAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuSfcProduceAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id);
    }
}
