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
    public class ManuContainerUnPackController : ControllerBase
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
        public ManuContainerUnPackController(IManuContainerPackService manuContainerPackService, ILogger<ManuContainerPackController> logger)
        {
            _manuContainerPackService = manuContainerPackService;
            _logger = logger;
        }

        /// <summary>
        /// 删除（容器装载表（物理删除））
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("容器解包", BusinessType.DELETE)]
        [PermissionDescription("manu:containerUnPackaging:delete")]
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
        [LogDescription("容器解包", BusinessType.DELETE)]
        [PermissionDescription("manu:containerUnPackaging:deleteAll")]
        public async Task DeleteManuContainerPackAsync(ContainerUnpackDto param)
        {
            await _manuContainerPackService.DeleteAllByContainerBarCodeIdAsync(param);
        }
    }
}