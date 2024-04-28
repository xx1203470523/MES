using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（在制品维修）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFacePlateRepairController : ControllerBase
    {
        /// <summary>
        /// 接口（在制品维修）
        /// </summary>
        private readonly IManuFacePlateRepairService _manuFacePlateRepairService;
        private readonly ILogger<ManuFacePlateRepairController> _logger;

        /// <summary>
        /// 构造函数（在制品维修）
        /// </summary>
        /// <param name="manuFacePlateRepairService"></param>
        /// <param name="logger"></param>
        public ManuFacePlateRepairController(IManuFacePlateRepairService manuFacePlateRepairService, ILogger<ManuFacePlateRepairController> logger)
        {
            _manuFacePlateRepairService = manuFacePlateRepairService;
            _logger = logger;
        }


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateRepairExJobDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("exJob")]
        [LogDescription("执行作业", BusinessType.OTHER)]
        public async Task<List<ManuFacePlateRepairButJobReturnTypeEnum>> ExecuteJobAsync(ManuFacePlateRepairExJobDto manuFacePlateRepairExJobDto)
        {
            return await _manuFacePlateRepairService.ExecuteJobAsync(manuFacePlateRepairExJobDto);
        }

        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="beginRepairDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("beginRepair")]
        [LogDescription("开始维修", BusinessType.OTHER)]
        public async Task<ManuFacePlateRepairOpenInfoDto> BeginManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {
            return await _manuFacePlateRepairService.BeginManuFacePlateRepairAsync(beginRepairDto);
        }

        /// <summary>
        /// 结束维修
        /// </summary>
        /// <param name="beginRepairDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("endRepair")]
        [LogDescription("结束维修", BusinessType.OTHER)]
        public async Task<ManuFacePlateRepairOpenInfoDto> EndManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {
            return await _manuFacePlateRepairService.EndManuFacePlateRepairAsync(beginRepairDto);
        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="confirmSubmitDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirmSubmit")]
        [LogDescription("确认提交", BusinessType.OTHER)]
        public async Task ConfirmSubmitManuFacePlateRepairAsync([FromBody] ManuFacePlateRepairConfirmSubmitDto confirmSubmitDto)
        {
            await _manuFacePlateRepairService.ConfirmSubmitManuFacePlateRepairAsync(confirmSubmitDto);
        }


        /// <summary>
        /// 获取初始信息
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getInitialInfo")]
        public async Task<ManuFacePlateRepairInitialInfoDto> GetInitialInfoManuFacePlateRepairAsync(long facePlateId)
        {
            return await _manuFacePlateRepairService.GetInitialInfoManuFacePlateRepairAsync(facePlateId);
        }

    }
}