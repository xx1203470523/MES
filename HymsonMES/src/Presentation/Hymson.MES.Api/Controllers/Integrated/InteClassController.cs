using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteClass;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（班制维护）
    /// @author 陈志谱
    /// @date 2023-02-10 08:55:55
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteClassController : ControllerBase
    {
        /// <summary>
        /// 接口（班制维护）
        /// </summary>
        private readonly IInteClassService _inteClassService;
        private readonly ILogger<InteClassController> _logger;

        /// <summary>
        /// 构造函数（班制维护）
        /// </summary>
        /// <param name="inteClassService"></param>
        public InteClassController(IInteClassService inteClassService, ILogger<InteClassController> logger)
        {
            _inteClassService = inteClassService;
            _logger = logger;
        }



        /// <summary>
        /// 添加（班制维护）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> AddInteClassAsync([FromBody] InteClassCreateDto createDto)
        {
            return await _inteClassService.AddInteClassAsync(createDto);
        }

        /// <summary>
        /// 更新（班制维护）
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<int> UpdateInteClassAsync([FromBody] InteClassModifyDto modifyDto)
        {
            return await _inteClassService.UpdateInteClassAsync(modifyDto);
        }

        /// <summary>
        /// 删除（班制维护）
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        public async Task<int> DeleteInteClassAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _inteClassService.DeleteInteClassAsync(idsArr);
        }

        /// <summary>
        /// 查询列表（班制维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<PagedInfo<InteClassDto>> GetPagedListAsync([FromQuery] InteClassPagedQueryDto pagedQueryDto)
        {
            return await _inteClassService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（班制维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<InteClassWithDetailDto> GetInteClassAsync(long id)
        {
            return await _inteClassService.GetInteClassAsync(id);
        }


    }
}