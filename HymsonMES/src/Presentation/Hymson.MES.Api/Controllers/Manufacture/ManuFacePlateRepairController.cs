/*
 *creator: Karl
 *
 *describe: 在制品维修    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-04-12 10:32:46
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（在制品维修）
    /// @author pengxin
    /// @date 2023-04-12 10:32:46
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
        public async Task<List<ManuFacePlateRepairButJobReturnTypeEnum>> ExecuteexecuteJobAsync(ManuFacePlateRepairExJobDto manuFacePlateRepairExJobDto)
        {
            return await _manuFacePlateRepairService.ExecuteexecuteJobAsync(manuFacePlateRepairExJobDto);
        }

        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="beginRepairDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("beginRepair")]
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