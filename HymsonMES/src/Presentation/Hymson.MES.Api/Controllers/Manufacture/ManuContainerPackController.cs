/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    控制器 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（容器装载表（物理删除））
    /// @author wxk
    /// @date 2023-04-12 02:33:13
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
        public async Task UpdateManuContainerPackAsync([FromBody] ManuContainerPackModifyDto parm)
        {
             await _manuContainerPackService.ModifyManuContainerPackAsync(parm);
        }

        /// <summary>
        /// 删除（容器装载表（物理删除））
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuContainerPackAsync([FromBody] long[] ids)
        {
            await _manuContainerPackService.DeletesManuContainerPackAsync(ids);
        }

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="containerBarCodeId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteAll")]
        public async Task DeleteManuContainerPackAsync([FromBody] long containerBarCodeId)
        {
            await _manuContainerPackService.DeleteAllByContainerBarCodeIdAsync(containerBarCodeId);
        }

        #endregion


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateContainerPackExJobDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("exJob")]
        public async Task<Dictionary<string, JobResponseDto>> ExecuteJobAsync(ManuFacePlateContainerPackExJobDto manuFacePlateContainerPackExJobDto)
        {
            return await _manuContainerPackService.ExecuteJobAsync(manuFacePlateContainerPackExJobDto);
        }
    }
}