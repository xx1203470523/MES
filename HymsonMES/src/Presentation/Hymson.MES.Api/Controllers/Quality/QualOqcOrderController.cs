using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（OQC检验单）
    /// @author xiaofei
    /// @date 2024-03-04 10:53:43
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualOqcOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualOqcOrderController> _logger;
        /// <summary>
        /// 服务接口（OQC检验单）
        /// </summary>
        private readonly IQualOqcOrderService _qualOqcOrderService;


        /// <summary>
        /// 构造函数（OQC检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualOqcOrderService"></param>
        public QualOqcOrderController(ILogger<QualOqcOrderController> logger, IQualOqcOrderService qualOqcOrderService)
        {
            _logger = logger;
            _qualOqcOrderService = qualOqcOrderService;
        }

        /// <summary>
        /// 添加（OQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualOqcOrderSaveDto saveDto)
        {
            await _qualOqcOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（OQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualOqcOrderSaveDto saveDto)
        {
            await _qualOqcOrderService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（OQC检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualOqcOrderService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（OQC检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualOqcOrderDto?> QueryByIdAsync(long id)
        {
            return await _qualOqcOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（OQC检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualOqcOrderDto>> QueryPagedListAsync([FromQuery] QualOqcOrderPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取OQC单价检验类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOqcOrderType/{id}")]
        public async Task<IEnumerable<QualOqcOrderTypeOutDto>> GetOqcOrderTypeAsync(long id)
        {
            return await _qualOqcOrderService.GetOqcOrderTypeAsync(id);
        }

        /// <summary>
        /// 校验条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkBarCode")]
        public async Task<CheckBarCodeOutDto> CheckBarCodeAsync([FromQuery] CheckBarCodeQuqryDto checkBarCodeQuqryDto)
        {
            return await _qualOqcOrderService.CheckBarCodeAsync(checkBarCodeQuqryDto);
        }
    }
}