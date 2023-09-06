/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcProduce
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
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListNewAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto);

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task QualityLockAsync(ManuSfcProduceLockDto parm);

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task QualityScrapAsync(ManuSfScrapDto parm);

        /// <summary>
        /// 条码取消报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task QualityCancelScrapAsync(ManuSfScrapDto parm);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcProduceCreateDto"></param>
        /// <returns></returns>
        Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceModifyDto"></param>
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

        /// <summary>
        /// 根据sfc查询
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcProduceDto> QueryManuSfcProduceBySFCAsync(string sfc);

        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceViewDto>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto);

        /// <summary>
        /// 分页查询（查询所有条码信息:不包含锁定和报废的）
        /// 优化
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcSelectPagedInfoAsync(ManuSfcProduceSelectPagedQueryDto queryDto);

        Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcPagedInfoAsync(ManuSfcProduceSelectPagedQueryDto queryDto);

        /// <summary>
        /// 根据SFC查询在制品步骤列表
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<List<ManuSfcProduceStepViewDto>> QueryManuSfcProduceStepBySFCsAsync(List<ManuSfcProduceStepSFCDto> sfcs);

        /// <summary>
        /// 保存在制品步骤
        /// </summary>
        /// <param name="sfcProduceStepDto"></param>
        /// <returns></returns>
        Task SaveManuSfcProduceStepAsync(SaveManuSfcProduceStepDto sfcProduceStepDto);

        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<List<ManuUpdateViewDto>> GetManuUpdateListAsync(string[] sfcs);

        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<List<ManuUpdateProcedureViewDto>> GetProcedureByOrderIdListAsync(long workOrderId);

        /// <summary>
        /// 保存生产更改
        /// </summary>
        /// <param name="manuUpdateSaveDto"></param>
        /// <returns></returns>
        Task SaveManuUpdateListAsync(ManuUpdateSaveDto manuUpdateSaveDto);

        /// <summary>
        /// 获取工艺路线末尾工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        Task<long> GetLastProcedureAsync(long processRouteId);

        /// <summary>
        /// 根据sfcs查询条码信息关联降级等级 
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceAboutDowngradingViewDto>> GetManuSfcAboutManuDowngradingBySfcsAsync(string[] sfcs);
    }
}
