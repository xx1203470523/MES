using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（转换系数表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcConversionFactorController : ControllerBase
    {
        /// <summary>
        /// 接口（转换系数表）
        /// </summary>
        private readonly IProcConversionFactorService _procConversionFactorService;
        private readonly ILogger<ProcConversionFactorController> _logger;

        /// <summary>
        /// 构造函数（转换系数表）
        /// </summary>
        /// <param name="procConversionFactorService"></param>
        /// <param name="logger"></param>
        public ProcConversionFactorController(IProcConversionFactorService procConversionFactorService, ILogger<ProcConversionFactorController> logger)
        {
            _procConversionFactorService = procConversionFactorService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（转换系数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcConversionFactorViewDto>> QueryPagedProcProcedure([FromQuery] ProcConversionFactorPagedQueryDto parm)
        {
            return await _procConversionFactorService.GetPageListAsync(parm);
        }


        /// <summary>
        /// 查询详情（转换系数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcConversionFactorDto> QueryProcLoadPointByIdAsync(long id)
        {
            return await _procConversionFactorService.QueryProcLoadPointByIdAsync(id);
        }


        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("print/list")]
        public async Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureBomConfigPrintListAsync([FromQuery] ProcProcedurePrintReleationPagedQueryDto parm)
        {
            return await _procConversionFactorService.GetProcedureConfigPrintListAsync(parm);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("job/list")]
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureBomConfigJobList([FromQuery] InteJobBusinessRelationPagedQueryDto parm)
        {
            return await _procConversionFactorService.GetProcedureConfigJobListAsync(parm);
        }


        /// <summary>
        /// 新增（转换系数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工序维护", BusinessType.INSERT)]
        [PermissionDescription("proc:ConversionFactor:insert")]
        public async Task<long> AddProcProcedureAsync([FromBody] AddConversionFactorDto parm)
        {
            return await _procConversionFactorService.AddProcProcedureAsync(parm);
        }


        /// <summary>
        /// 删除（转换系数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("转换系数维护", BusinessType.DELETE)]
        [PermissionDescription("proc:ConversionFactor:delete")]
        public async Task DeleteProcLoadPointAsync([FromBody] long[] ids)
        {
            await _procConversionFactorService.DeleteProcProcedureAsync(ids);
        }

        /// <summary>
        /// 根据工序读取工序详细信息和资源信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("getByCode/{code}")]
        public async Task<ProcProcedureCodeDto> GetByCodeAsync(string code)
        {
            return await _procConversionFactorService.GetByCodeAsync(code);
        }

        /// <summary>
        /// 更新（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:conversionFactor:update")]
        public async Task UpdateProcConversionFactorAsync([FromBody] ProcConversionFactorModifyDto parm)
        {
            await _procConversionFactorService.ModifyProcConversionFactorAsync(parm);
        }
    }
}