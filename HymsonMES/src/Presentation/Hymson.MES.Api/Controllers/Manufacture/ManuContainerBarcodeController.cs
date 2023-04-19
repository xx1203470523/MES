/*
 *creator: Karl
 *
 *describe: 容器条码表    控制器 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（容器条码表）
    /// @author wxk
    /// @date 2023-04-12 02:29:23
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuContainerBarcodeController : ControllerBase
    {
        /// <summary>
        /// 接口（容器条码表）
        /// </summary>
        private readonly IManuContainerBarcodeService _manuContainerBarcodeService;
        private readonly ILogger<ManuContainerBarcodeController> _logger;

        /// <summary>
        /// 构造函数（容器条码表）
        /// </summary>
        /// <param name="manuContainerBarcodeService"></param>
        public ManuContainerBarcodeController(IManuContainerBarcodeService manuContainerBarcodeService, ILogger<ManuContainerBarcodeController> logger)
        {
            _manuContainerBarcodeService = manuContainerBarcodeService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（容器条码表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuContainerBarcodeDto>> QueryPagedManuContainerBarcodeAsync([FromQuery] ManuContainerBarcodePagedQueryDto parm)
        {
            return await _manuContainerBarcodeService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（容器条码表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByIdAsync(long id)
        {
            return await _manuContainerBarcodeService.QueryManuContainerBarcodeByIdAsync(id);
        }

        /// <summary>
        /// 添加（容器条码表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ManuContainerBarcodeView> AddManuContainerBarcodeAsync([FromBody] CreateManuContainerBarcodeDto parm)
        {
            return await _manuContainerBarcodeService.CreateManuContainerBarcodeAsync(parm);
        }

        /// <summary>
        /// 更新（容器条码表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateManuContainerBarcodeAsync([FromBody] ManuContainerBarcodeModifyDto parm)
        {
             await _manuContainerBarcodeService.ModifyManuContainerBarcodeAsync(parm);
        }


        [HttpPost]
        [Route("updateStatus")]
        public async Task UpdateManuContainerBarcodeStatusAsync([FromBody] UpdateManuContainerBarcodeStatusDto parm)
        {
            await _manuContainerBarcodeService.ModifyManuContainerBarcodeStatusAsync(parm);
        }

        /// <summary>
        /// 删除（容器条码表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuContainerBarcodeAsync([FromBody] long[] ids)
        {
            await _manuContainerBarcodeService.DeletesManuContainerBarcodeAsync(ids);
        }

        #endregion
    }
}