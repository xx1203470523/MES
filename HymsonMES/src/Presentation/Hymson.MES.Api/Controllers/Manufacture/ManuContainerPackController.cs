using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（容器装载表（物理删除））
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuContainerPackController : ControllerBase
    {
        /// <summary>
        /// 接口（容器装载表（物理删除））
        /// </summary>
        private readonly IManuContainerPackService _manuContainerPackService;
        private readonly ILogger<ManuContainerPackController> _logger;

        /// <summary>
        /// 构造函数（容器装载表（物理删除））
        /// </summary>
        /// <param name="manuContainerPackService"></param>
        /// <param name="logger"></param>
        public ManuContainerPackController(IManuContainerPackService manuContainerPackService, ILogger<ManuContainerPackController> logger)
        {
            _manuContainerPackService = manuContainerPackService;
            _logger = logger;
        }


        #region 框架生成方法
        /// <summary>
        /// 分页查询列表（容器装载表（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuContainerPackDto>> QueryPagedManuContainerPackAsync([FromQuery] ManuContainerPackPagedQueryDto parm)
        {
            return await _manuContainerPackService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（容器装载表（物理删除））
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuContainerPackDto> QueryManuContainerPackByIdAsync(long id)
        {
            return await _manuContainerPackService.QueryManuContainerPackByIdAsync(id);
        }

        /// <summary>
        /// 添加（容器装载表（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("容器包装", BusinessType.INSERT)]
        public async Task AddManuContainerPackAsync([FromBody] ManuContainerPackCreateDto parm)
        {
            await _manuContainerPackService.CreateManuContainerPackAsync(parm);
        }

        /// <summary>
        /// 更新（容器装载表（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("容器包装", BusinessType.UPDATE)]
        public async Task UpdateManuContainerPackAsync([FromBody] ManuContainerPackModifyDto parm)
        {
            await _manuContainerPackService.ModifyManuContainerPackAsync(parm);
        }

        /// <summary>
        /// 删除（容器装载表（物理删除））
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("容器包装", BusinessType.DELETE)]
        [PermissionDescription("manu:containerPack:delete")]
        public async Task DeleteManuContainerPackAsync(ManuContainerPackUnpackDto param)
        {
            await _manuContainerPackService.DeletesManuContainerPackAsync(param);
        }

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteAll")]
        [LogDescription("容器包装", BusinessType.DELETE)]
        [PermissionDescription("manu:containerPack:deleteAll")]
        public async Task DeleteManuContainerPackAsync( ContainerUnpackDto param)
        {
            await _manuContainerPackService.DeleteAllByContainerBarCodeIdAsync(param);
        }

        #endregion


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateContainerPackExJobDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("exJob")]
        public async Task<Dictionary<string, JobResponseBo>> ExecuteJobAsync(ManuFacePlateContainerPackExJobDto manuFacePlateContainerPackExJobDto)
        {
            return await _manuContainerPackService.ExecuteJobAsync(manuFacePlateContainerPackExJobDto);
        }
    }
}