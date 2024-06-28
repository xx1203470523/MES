using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuJointProductAndByproductsReceiveRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（联副产品收货）
    /// @author User
    /// @date 2024-06-05 02:15:16
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuJointProductAndByproductsReceiveRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuJointProductAndByproductsReceiveRecordController> _logger;
        /// <summary>
        /// 服务接口（联副产品收货）
        /// </summary>
        private readonly IManuJointProductAndByproductsReceiveRecordService _manuJointProductAndByproductsReceiveRecordService;


        /// <summary>
        /// 构造函数（联副产品收货）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuJointProductAndByproductsReceiveRecordService"></param>
        public ManuJointProductAndByproductsReceiveRecordController(ILogger<ManuJointProductAndByproductsReceiveRecordController> logger, IManuJointProductAndByproductsReceiveRecordService manuJointProductAndByproductsReceiveRecordService)
        {
            _logger = logger;
            _manuJointProductAndByproductsReceiveRecordService = manuJointProductAndByproductsReceiveRecordService;
        }

        /// <summary>
        /// 添加（联副产品收货）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
             await _manuJointProductAndByproductsReceiveRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（联副产品收货）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
             await _manuJointProductAndByproductsReceiveRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（联副产品收货）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuJointProductAndByproductsReceiveRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（联副产品收货）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuJointProductAndByproductsReceiveRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuJointProductAndByproductsReceiveRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（联副产品收货）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuJointProductAndByproductsReceiveRecordDto>> QueryPagedListAsync([FromQuery] ManuJointProductAndByproductsReceiveRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuJointProductAndByproductsReceiveRecordService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 根据工单Id查询Bom联副产品列表
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getJointProductAndByproductsReceiveRecord/{workOrderId}")]
        public async Task<ManuJointProductAndByproductsReceiveRecordResult> GetWorkIdByBomJointProductAndByProductsListAsync(long workOrderId)
        {
            return await _manuJointProductAndByproductsReceiveRecordService.GetWorkIdByBomJointProductAndByProductsListAsync(workOrderId);
        }

        /// <summary>
        /// 添加（联副产品收货）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveInfo")]
        public async Task<ManuJointProductAndByproductsReceiveRecordSaveResultDto> SaveJointProductAndByproductsInfoAsync([FromBody] ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
            return await _manuJointProductAndByproductsReceiveRecordService.SaveJointProductAndByproductsInfoAsync(saveDto);
        }
    }
}