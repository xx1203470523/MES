using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Hymson.MES.Services.Services.Integrated.InteSFCBox;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Hymson.MES.Api.Controllers.Integrated
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteSFCBoxController : ControllerBase
    {
        private readonly IInteSFCBoxService _inteSFCBoxService;
        private readonly ILogger<InteSFCBoxController> _logger;

        public InteSFCBoxController(IInteSFCBoxService inteSFCBoxService,
            ILogger<InteSFCBoxController> logger)
        {
            _inteSFCBoxService = inteSFCBoxService;
            _logger = logger;
        }

        /// <summary>
        /// 箱码工单校验
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        [Route("pda/sfcboxvalidate")]
        [HttpPost]       
        public async Task<InteSFCBoxValidateResponse> SFCValidate(InteSFCBoxValidateQuery validate)
        {
            return await _inteSFCBoxService.SFCValidate(validate);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        //[PermissionDescription("inte:container:list")]
        public async Task<PagedInfo<InteSFCBoxDto>> GetPagedListAsync([FromQuery] InteSFCBoxQueryDto pagedQueryDto)
        {
            return await _inteSFCBoxService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 工单新增选择列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("pagelist")]
        [HttpGet]
        public async Task<PagedInfo<InteSFCBoxRView>> GetBoxCodeListAsync([FromQuery] InteSFCBoxQueryDto pagedQueryDto)
        {
            return await _inteSFCBoxService.GetBoxCodeListAsync(pagedQueryDto);
        }


        /// <summary>
        /// 删除（电芯批次码维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("电芯批次码维护", BusinessType.DELETE)]
        public async Task DeletesAsync(long[] ids)
        {
            await _inteSFCBoxService.DeletesAsync(ids);
        }


        /// <summary>
        /// 电芯批次导入
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        [HttpPost("importData")]
        [LogDescription("电芯批次导入", BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<int> ImportDataAsync([FromForm] UploadSFCBoxDto uploadStockDetailDto)
        {
            return await _inteSFCBoxService.ImportDataNoRepeatAsync(uploadStockDetailDto);
        }


        /// <summary>
        /// 电芯批次导入
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        [HttpPost("importData2")]
        [LogDescription("电芯批次导入（过滤重复的）", BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<int> ImportDatNoRepeataAsync([FromForm] UploadSFCBoxDto uploadStockDetailDto)
        {
            return await _inteSFCBoxService.ImportDataNoRepeatAsync(uploadStockDetailDto);
        }


        /// <summary>
        /// 电芯明细导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [LogDescription("电芯明细导入模板", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadImportTemplateAsync()
        {
            using MemoryStream stream = new MemoryStream();
            await _inteSFCBoxService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", HttpUtility.UrlEncode($"电芯明细导入模板.xlsx"));
        }
    }
}
