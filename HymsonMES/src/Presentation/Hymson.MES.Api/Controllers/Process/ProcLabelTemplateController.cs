/*
 *creator: Karl
 *
 *describe: 仓库标签模板    控制器 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.LabelTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（仓库标签模板）
    /// @author wxk
    /// @date 2023-03-09 02:51:26
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcLabelTemplateController : ControllerBase
    {
        /// <summary>
        /// 接口（仓库标签模板）
        /// </summary>
        private readonly IProcLabelTemplateService _procLabelTemplateService;
        private readonly ILogger<ProcLabelTemplateController> _logger;

        /// <summary>
        /// 构造函数（仓库标签模板）
        /// </summary>
        /// <param name="procLabelTemplateService"></param>
        public ProcLabelTemplateController(IProcLabelTemplateService procLabelTemplateService, ILogger<ProcLabelTemplateController> logger)
        {
            _procLabelTemplateService = procLabelTemplateService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcLabelTemplateDto>> QueryPagedProcLabelTemplateAsync([FromQuery] ProcLabelTemplatePagedQueryDto parm)
        {
            return await _procLabelTemplateService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（仓库标签模板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id)
        {
            return await _procLabelTemplateService.QueryProcLabelTemplateByIdAsync(id);
        }

        /// <summary>
        /// 添加（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcLabelTemplateAsync([FromBody] ProcLabelTemplateCreateDto parm)
        {
             await _procLabelTemplateService.CreateProcLabelTemplateAsync(parm);
        }

        /// <summary>
        /// 更新（仓库标签模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcLabelTemplateAsync([FromBody] ProcLabelTemplateModifyDto parm)
        {
             await _procLabelTemplateService.ModifyProcLabelTemplateAsync(parm);
        }

        /// <summary>
        /// 删除（仓库标签模板）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteProcLabelTemplateAsync(DeleteDto deleteDto)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procLabelTemplateService.DeletesProcLabelTemplateAsync(deleteDto.Ids);
        }
        ///// <summary>
        ///// 下载模板文件
        ///// </summary>
        ///// <param name="url"></param>
        //[HttpGet("{url}")]
        //[Route("downloadFile")]
        //public void downloadFile(string url)
        //{
            
        //}

}
}