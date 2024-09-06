using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（定子装箱记录表）
    /// @author Yxx
    /// @date 2024-09-04 11:54:20
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuStatorPackListController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuStatorPackListController> _logger;
        /// <summary>
        /// 服务接口（定子装箱记录表）
        /// </summary>
        private readonly IManuStatorPackListService _manuStatorPackListService;


        /// <summary>
        /// 构造函数（定子装箱记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuStatorPackListService"></param>
        public ManuStatorPackListController(ILogger<ManuStatorPackListController> logger, IManuStatorPackListService manuStatorPackListService)
        {
            _logger = logger;
            _manuStatorPackListService = manuStatorPackListService;
        }

        /// <summary>
        /// 添加（定子装箱记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuStatorPackListSaveDto saveDto)
        {
             await _manuStatorPackListService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 创建箱体码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("createBox")]
        public async Task<string> AddBoxAsync()
        {
            return await _manuStatorPackListService.AddBoxAsync();
        }

        /// <summary>
        /// 打印箱体码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("print")]
        public async Task<int> PrintAsync(ManuStatorPackPrintDto parame)
        {
            return 0;
        }

        /// <summary>
        /// 更新（定子装箱记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuStatorPackListSaveDto saveDto)
        {
             await _manuStatorPackListService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（定子装箱记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuStatorPackListService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（定子装箱记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuStatorPackListDto?> QueryByIdAsync(long id)
        {
            return await _manuStatorPackListService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（定子装箱记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuStatorPackListDto>> QueryPagedListAsync([FromQuery] ManuStatorPackListPagedQueryDto pagedQueryDto)
        {
            return await _manuStatorPackListService.GetPagedListAsync(pagedQueryDto);
        }

    }
}