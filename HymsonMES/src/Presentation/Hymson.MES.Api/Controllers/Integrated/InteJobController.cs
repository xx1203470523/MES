using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 作业表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteJobController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<InteJobController> _logger;
        private readonly IInteJobService _inteJobService;
        private readonly IJobCommonService _jobCommonService;


        /// <summary>
        /// 作业表控制器
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteJobService"></param>
        /// <param name="jobCommonService"></param>
        public InteJobController(ILogger<InteJobController> logger,
            IInteJobService inteJobService,
            IJobCommonService jobCommonService)
        {
            _logger = logger;
            _inteJobService = inteJobService;
            _jobCommonService = jobCommonService;
        }


        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        //[PermissionDescription("inte:job:list")]
        public async Task<PagedInfo<InteJobDto>> QueryPagedInteJobAsync([FromQuery] InteJobPagedQueryDto param)
        {
            return await _inteJobService.GetPageListAsync(param);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteJobDto> QueryInteJobByIdAsync(long id)
        {
            return await _inteJobService.QueryInteJobByIdAsync(id);
        }

        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("classProgram")]
        public async Task<IEnumerable<SelectOptionDto>> GetClassProgramListAsync()
        {
            return await _jobCommonService.GetClassProgramOptionsAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("作业", BusinessType.INSERT)]
        [PermissionDescription("inte:job:insert")]
        public async Task<long> AddInteJobAsync([FromBody] InteJobCreateDto param)
        {
            return await _inteJobService.CreateInteJobAsync(param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("作业", BusinessType.UPDATE)]
        [PermissionDescription("inte:job:update")]
        public async Task UpdateInteJobAsync([FromBody] InteJobModifyDto param)
        {
            await _inteJobService.ModifyInteJobAsync(param);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("作业", BusinessType.DELETE)]
        [PermissionDescription("inte:job:delete")]
        public async Task DeleteInteJobAsync(long[] ids)
        {
            await _inteJobService.DeleteRangInteJobAsync(ids);
        }

        /// <summary>
        /// 查询详情（规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("rule/{id}")]
        public async Task<IEnumerable<InteJobConfigDto>> GetConfigByJobIdAsync(long id)
        {
            return await _inteJobService.GetConfigByJobIdAsync(id);
        }
    }
}