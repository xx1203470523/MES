using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（产品不良录入）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductBadRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordService _manuProductBadRecordService;
        private readonly ILogger<ManuProductBadRecordController> _logger;

        /// <summary>
        /// 构造函数（产品不良录入）
        /// </summary>
        /// <param name="manuProductBadRecordService"></param>
        /// <param name="logger"></param>
        public ManuProductBadRecordController(IManuProductBadRecordService manuProductBadRecordService, ILogger<ManuProductBadRecordController> logger)
        {
            _manuProductBadRecordService = manuProductBadRecordService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuProductBadRecordDto>> QueryPagedManuProductBadRecordAsync([FromQuery] ManuProductBadRecordPagedQueryDto parm)
        {
            return await _manuProductBadRecordService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（产品不良录入）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id)
        {
            return await _manuProductBadRecordService.QueryManuProductBadRecordByIdAsync(id);
        }

        /// <summary>
        /// 添加（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("产品不良录入", BusinessType.INSERT)]
        [PermissionDescription("manu:badRecord:insert")]
        public async Task AddManuProductBadRecordAsync([FromBody] ManuProductBadRecordCreateDto parm)
        {
            await _manuProductBadRecordService.CreateManuProductBadRecordAsync(parm);
        }

        /// <summary>
        /// 面板添加（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addManuProductBadRecordByFacePlate")]
        [LogDescription("面板添加产品不良录入", BusinessType.INSERT)]
        [PermissionDescription("manu:badRecord:insert")]
        public async Task FacePlateAddManuProductBadRecordAsync([FromBody] FacePlateManuProductBadRecordCreateDto parm)
        {
            await _manuProductBadRecordService.CreateManuProductBadRecordAboutCodeTypeAsync(parm);
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("badRecords")]
        public async Task<IEnumerable<ManuProductBadRecordViewDto>> GetBadRecordsBySfcAsync([FromQuery] ManuProductBadRecordQueryDto parm)
        {
            return await _manuProductBadRecordService.GetBadRecordsBySfcAsync(parm);
        }

        /// <summary>
        /// 不良复判
        /// </summary>
        /// <param name="badReJudgmentDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("badReJudgment")]
        [LogDescription("不良复判", BusinessType.INSERT)]
        [PermissionDescription("manu:badReJudgment:reJudgment")]
        public async Task BadReJudgmentAsync(BadReJudgmentDto badReJudgmentDto)
        {
            await _manuProductBadRecordService.BadReJudgmentAsync(badReJudgmentDto);
        }

        /// <summary>
        /// 取消标识
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelIdentify")]
        [LogDescription("取消标识", BusinessType.INSERT)]
        [PermissionDescription("manu:cancelIdentify:cancel")]
        public async Task CancelSfcIdentification(CancelSfcIdentificationDto parm)
        {
            await _manuProductBadRecordService.CancelSfcIdentificationAsync(parm);
        }

        /// <summary>
        /// 更新（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("产品不良录入", BusinessType.UPDATE)]
        public async Task UpdateManuProductBadRecordAsync([FromBody] ManuProductBadRecordModifyDto parm)
        {
            await _manuProductBadRecordService.ModifyManuProductBadRecordAsync(parm);
        }

        /// <summary>
        /// 删除（产品不良录入）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("产品不良录入", BusinessType.DELETE)]
        public async Task DeleteManuProductBadRecordAsync(DeleteDto deleteDto)
        {
            await _manuProductBadRecordService.DeletesManuProductBadRecordAsync(deleteDto.Ids);
        }

        #region

        /// <summary>
        /// 保存（不良标识录入）
        /// </summary>
        /// <param name="productBadRecordMarkSaveDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveBadRecordMarkEntry")]
        [LogDescription("不良标识录入", BusinessType.INSERT)]
        //[PermissionDescription("manu:sfcMarking:save")]
        public async Task SaveBadRecordMarkEntryAsync([FromBody] List<ManuProductBadRecordMarkSaveDto> productBadRecordMarkSaveDtos)
        {
            await _manuProductBadRecordService.SaveBadRecordMarkEntryAsync(productBadRecordMarkSaveDtos);
        }

        /// <summary>
        /// 导入 不良标识 数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importBadRecordMark")]
        [LogDescription("不良标识录入", BusinessType.INSERT)]
        //[PermissionDescription("manu:BadRecordMark:import")]
        public async Task ImportBadRecordMarkAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _manuProductBadRecordService.ImportBadRecordMarkEntryAsync(formFile);
        }

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportBadRecordMarkTemplate")]
        [LogDescription("不良标识导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            //throw new NotImplementedException();

            using MemoryStream stream = new MemoryStream();
            await _manuProductBadRecordService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"录入标识导入模板.xlsx");
        }



        #endregion

    }
}