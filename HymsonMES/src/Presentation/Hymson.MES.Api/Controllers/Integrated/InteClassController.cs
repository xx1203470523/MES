/*
 *creator: Karl
 *
 *describe: 生产班次    控制器 | 代码由框架生成  
 *builder:  陈志谱
 *build datetime: 2023-02-10 08:55:55
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.InteClass;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.InteClass
{
    /// <summary>
    /// 控制器（生产班次）
    /// @author 陈志谱
    /// @date 2023-02-10 08:55:55
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteClassController : ControllerBase
    {
        /// <summary>
        /// 接口（生产班次）
        /// </summary>
        private readonly IInteClassService _inteClassService;
        private readonly ILogger<InteClassController> _logger;

        /// <summary>
        /// 构造函数（生产班次）
        /// </summary>
        /// <param name="inteClassService"></param>
        public InteClassController(IInteClassService inteClassService, ILogger<InteClassController> logger)
        {
            _inteClassService = inteClassService;
            _logger = logger;
        }



        /// <summary>
        /// 添加生产班次
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> AddInteClassAsync([FromBody] AddInteClassDto createDto)
        {
            return await _inteClassService.AddInteClassAsync(createDto);
        }

        /// <summary>
        /// 更新生产班次
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<int> UpdateInteClassAsync([FromBody] UpdateInteClassDto modifyDto)
        {
            return await _inteClassService.UpdateInteClassAsync(modifyDto);
        }

        /// <summary>
        /// 删除生产班次
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        public async Task<int> DeleteInteClassAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _inteClassService.DeleteInteClassAsync(idsArr);
        }

        /// <summary>
        /// 查询生产班次列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<PagedInfo<InteClassDto>> GetPagedListAsync([FromQuery] InteClassPagedQueryDto pagedQueryDto)
        {
            return await _inteClassService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询生产班次详情
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