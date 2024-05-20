using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 在制品拆解
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InProductDismantleController : ControllerBase
    {
        private readonly IInProductDismantleService _inProductDismantleService;
        private readonly ILogger<InProductDismantleController> _logger;

        /// <summary>
        /// 构造函数（物料加载）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inProductDismantleService"></param>
        public InProductDismantleController(ILogger<InProductDismantleController> logger, IInProductDismantleService inProductDismantleService)
        {
            _logger = logger;
            _inProductDismantleService = inProductDismantleService;
        }

        /// <summary>
        /// 查询Bom维护表详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mainMaterials")]
        public async Task<InProductDismantleComponentDto> GetProcBomDetailAsync([FromQuery] InProductDismantleQueryDto queryDto)
        {
            return await _inProductDismantleService.GetProcBomDetailAsync(queryDto);
        }

        /// <summary>
        /// 在制品移除
        /// </summary>
        /// <param name="removeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("removeModule")]
        [LogDescription("在制品移除添加", BusinessType.DELETE)]
        [PermissionDescription("manu:inProductDismantle:delete")]
        public async Task RemoveModuleAsync(InProductDismantleRemoveDto removeDto)
        {
            await _inProductDismantleService.RemoveModuleAsync(removeDto);
        }

        /// <summary>
        /// 在制品拆解添加组件
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addModule")]
        [LogDescription("在制品移除添加", BusinessType.INSERT)]
        [PermissionDescription("manu:inProductDismantle:insert")]
        public async Task AddModuleAsync(InProductDismantleAddDto addDto)
        {
            await _inProductDismantleService.AddModuleAsync(addDto);
        }

        /// <summary>
        /// 在制品拆解换件
        /// </summary>
        /// <param name="replaceDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("replaceModule")]
        [LogDescription("在制品拆解换件", BusinessType.OTHER)]
        [PermissionDescription("manu:inProductDismantle:replace")]
        public async Task ReplaceModuleAsync(InProductDismantleReplaceDto replaceDto)
        {
            await _inProductDismantleService.ReplaceModuleAsync(replaceDto);
        }

        /// <summary>
        /// 获取主物料下的所有物料列表
        /// </summary>
        /// <param name="id">bom详情id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("bomMaterials/{id}")]
        public async Task<List<InProductDismantleDto>> GetBomMaterialsAsync(long id)
        {
            return await _inProductDismantleService.GetBomMaterialsAsync(id);
        }

    }
}
