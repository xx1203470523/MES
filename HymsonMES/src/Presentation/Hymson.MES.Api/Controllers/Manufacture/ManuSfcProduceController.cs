using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码生产信息（物理删除））
    /// @author zhaoqing
    /// @date 2023-03-18 05:37:27
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcProduceController : ControllerBase
    {
        /// <summary>
        /// 接口（条码生产信息（物理删除））
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;
        private readonly ILogger<ManuSfcProduceController> _logger;

        /// <summary>
        /// 构造函数（条码生产信息（物理删除））
        /// </summary>
        /// <param name="manuSfcProduceService"></param>
        /// <param name="logger"></param>
        public ManuSfcProduceController(IManuSfcProduceService manuSfcProduceService, ILogger<ManuSfcProduceController> logger)
        {
            _manuSfcProduceService = manuSfcProduceService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuSfcProduceViewDto>> QueryPagedManuSfcProduceAsync([FromQuery] ManuSfcProducePagedQueryDto parm)
        {
            return await _manuSfcProduceService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("lock")]
        [PermissionDescription("qual:qualityLock:lock")]
        public async Task QualityLockAsync(ManuSfcProduceLockDto parm)
        {
            await _manuSfcProduceService.QualityLockAsync(parm);
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("scrap")]
        [PermissionDescription("qual:productScrap:scrap")]
        public async Task QualityScrapAsync(ManuSfScrapDto parm)
        {
            if(parm.OperationType== ScrapOperateTypeEnum.Scrapping)
            {
                await _manuSfcProduceService.QualityScrapAsync(parm);
            }
            else
            {
                await _manuSfcProduceService.QualityCancelScrapAsync(parm);
            }
        }

        ///// <summary>
        ///// 条码取消报废
        ///// </summary>
        ///// <param name="parm"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("cancelScrap")]
        //[PermissionDescription("qual:productScrap:scrap")]
        //public async Task QualityCancelScrapAsync(ManuSfScrapDto parm)
        //{
        //    await _manuSfcProduceService.QualityCancelScrapAsync(parm);
        //}

        /// <summary>
        /// 查询详情（条码生产信息（物理删除））
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id)
        {
            return await _manuSfcProduceService.QueryManuSfcProduceByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（条码生产信息（物理删除））
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("getBySFC/{sfc}")]
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceBySFCAsync(string sfc)
        {
            return await _manuSfcProduceService.QueryManuSfcProduceBySFCAsync(sfc);
        }

        /// <summary>
        /// 添加（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddManuSfcProduceAsync([FromBody] ManuSfcProduceCreateDto parm)
        {
            await _manuSfcProduceService.CreateManuSfcProduceAsync(parm);
        }

        /// <summary>
        /// 更新（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateManuSfcProduceAsync([FromBody] ManuSfcProduceModifyDto parm)
        {
            await _manuSfcProduceService.ModifyManuSfcProduceAsync(parm);
        }

        /// <summary>
        /// 删除（条码生产信息（物理删除））
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteManuSfcProduceAsync(string ids)
        {
            await _manuSfcProduceService.DeletesManuSfcProduceAsync(ids);
        }

        #region 在制品步骤控制



        /// <summary>
        /// 分页查询列表（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getManuSfcPageList")]
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetManuSfcPagedInfoAsync([FromQuery] ManuSfcProducePagedQueryDto parm)
        {
            return await _manuSfcProduceService.GetManuSfcPagedInfoAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（条码生产信息（物理删除））
        /// 优化
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getManuSfcSelectPageList")]
        public async Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcSelectPagedInfoAsync([FromQuery] ManuSfcProduceSelectPagedQueryDto parm)
        {
            return await _manuSfcProduceService.GetManuSfcSelectPagedInfoAsync(parm);
        }

        /// <summary>
        /// 根据SFC查询在制品步骤列表
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpPost("getManuSfcProduceStep")]
        public async Task<List<ManuSfcProduceStepViewDto>> QueryManuSfcProduceStepBySFCsAsync(List<ManuSfcProduceStepSFCDto> sfcs)
        {
            return await _manuSfcProduceService.QueryManuSfcProduceStepBySFCsAsync(sfcs);
        }

        /// <summary>
        /// 保存在制品步骤
        /// </summary>
        /// <param name="sfcProduceStepDto"></param>
        /// <returns></returns>
        [HttpPost("saveManuSfcProduceStep")]
        [LogDescription("在制品步骤控制", BusinessType.INSERT)]
        [PermissionDescription("manu:manSfcStepControl:saveStep")]
        public async Task SaveManuSfcProduceStepAsync(SaveManuSfcProduceStepDto sfcProduceStepDto)
        {
            await _manuSfcProduceService.SaveManuSfcProduceStepAsync(sfcProduceStepDto);
        }
        #endregion

        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        [HttpPost("getManuUpdateList")]
        public async Task<List<ManuUpdateViewDto>> GetManuUpdateList(string[] sfcs)
        {
            return await _manuSfcProduceService.GetManuUpdateListAsync(sfcs);
        }

        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpGet("getProcedureByOrderId/{workOrderId}")]
        public async Task<List<ManuUpdateProcedureViewDto>> GetProcedureByOrderIdList(long workOrderId)
        {
            return await _manuSfcProduceService.GetProcedureByOrderIdListAsync(workOrderId);
        }

        /// <summary>
        /// 保存生产更改
        /// </summary>
        /// <param name="manuUpdateSaveDto"></param>
        /// <returns></returns>
        [HttpPost("saveManuUpdate")]
        [LogDescription("生产更改", BusinessType.INSERT)]
        [PermissionDescription("manu:manuUpdate:saveUpdate")]
        public async Task SaveManuUpdateList(ManuUpdateSaveDto manuUpdateSaveDto)
        {
            await _manuSfcProduceService.SaveManuUpdateListAsync(manuUpdateSaveDto);
        }

        /// <summary>
        /// 获取工艺路线末尾工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        [HttpGet("getLastProcedure/{processRouteId}")]
        public async Task<long> GetLastProcedureAsync(long processRouteId)
        {
            return await _manuSfcProduceService.GetLastProcedureAsync(processRouteId);
        }

        /// <summary>
        /// 根据条码获取条码相关联降级等级信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        [HttpPost("getSfcAboutManuDowngrading")]
        public async Task<IEnumerable<ManuSfcProduceAboutDowngradingViewDto>> GetLastProcedureAsync(string[] sfcs)
        {
            return await _manuSfcProduceService.GetManuSfcAboutManuDowngradingBySfcsAsync(sfcs);
        }
    }
}