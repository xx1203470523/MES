using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（转子装箱记录表）
    /// @author User
    /// @date 2024-07-24 01:43:00
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuRotorPackListController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuRotorPackListController> _logger;
        /// <summary>
        /// 服务接口（转子装箱记录表）
        /// </summary>
        private readonly IManuRotorPackListService _manuRotorPackListService;


        /// <summary>
        /// 构造函数（转子装箱记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuRotorPackListService"></param>
        public ManuRotorPackListController(ILogger<ManuRotorPackListController> logger, IManuRotorPackListService manuRotorPackListService)
        {
            _logger = logger;
            _manuRotorPackListService = manuRotorPackListService;
        }

        /// <summary>
        /// 添加（转子装箱记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuRotorPackListSaveDto saveDto)
        {
             await _manuRotorPackListService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（转子装箱记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuRotorPackListSaveDto saveDto)
        {
             await _manuRotorPackListService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（转子装箱记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuRotorPackListService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（转子装箱记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuRotorPackListDto?> QueryByIdAsync(long id)
        {
            return await _manuRotorPackListService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（转子装箱记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuRotorPackListDto>> QueryPagedListAsync([FromQuery] ManuRotorPackListPagedQueryDto pagedQueryDto)
        {
            return await _manuRotorPackListService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// （转子装箱记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("rotorpacklist")]
        public async Task<IEnumerable<ManuRotorPackViewDto>> QueryrotorpacklistAsync([FromQuery] ManuRotorPackListQuery pagedQueryDto)
        {
            return await _manuRotorPackListService.QueryByIdAsync(pagedQueryDto);
        }
    }
}