using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.PrintConfig;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 打印机配置表Controller
    /// @tableName PrintConfig
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcPrintConfigController : ControllerBase
    {
        /// <summary>
        /// 打印配置表接口
        /// </summary>
        private readonly IProcPrintConfigService _procPrintConfigService;
        private readonly ILogger<ProcPrintConfigController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procResourceService"></param>
        /// <param name="logger"></param>
        public ProcPrintConfigController(IProcPrintConfigService procResourceService, ILogger<ProcPrintConfigController> logger)
        {
            _procPrintConfigService = procResourceService;
            _logger = logger;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcPrinterDto>> QueryProcPrintConfigAsync([FromQuery] ProcPrinterPagedQueryDto query)
        {
            return await _procPrintConfigService.GetPageListAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcPrinterDto>> GetProcPrintConfigListAsync([FromQuery] ProcPrinterPagedQueryDto query)
        {
            return await _procPrintConfigService.GetListAsync(query);
        }

        /// <summary>
        /// 查询打印配置表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcPrinterDto> GetProcPrintConfigAsync(long id)
        {
            return await _procPrintConfigService.GetByIdAsync(id);
        }
        /// <summary>
        /// 查询打印配置表详情
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        [Route("printName")]
        [HttpGet]
        public async Task<ProcPrinterDto> GetProcPrintConfigAsync(string printName)
        {
            return await _procPrintConfigService.GetByPrintNameAsync(printName);
        }

        /// <summary>
        /// 添加打印配置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcPrintConfigAsync([FromBody] ProcPrinterDto parm)
        {
            await _procPrintConfigService.AddProcPrintConfigAsync(parm);
        }

        /// <summary>
        /// 更新打印配置表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProcPrintConfigAsync([FromBody] ProcPrinterUpdateDto parm)
        {
            await _procPrintConfigService.UpdateProcPrintConfigAsync(parm);
        }

        /// <summary>
        /// 删除打印配置表
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteProcPrintConfigAsync(DeleteDto deleteDto)
        {
            await _procPrintConfigService.DeleteProcPrintConfigAsync(deleteDto.Ids);
        }
    }
}