using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（点检任务）
    /// @author User
    /// @date 2024-05-14 09:00:47
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSpotcheckTaskController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquSpotcheckTaskController> _logger;
        /// <summary>
        /// 服务接口（点检任务）
        /// </summary>
        private readonly IEquSpotcheckTaskService _equSpotcheckTaskService;


        /// <summary>
        /// 构造函数（点检任务）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equSpotcheckTaskService"></param>
        public EquSpotcheckTaskController(ILogger<EquSpotcheckTaskController> logger, IEquSpotcheckTaskService equSpotcheckTaskService)
        {
            _logger = logger;
            _equSpotcheckTaskService = equSpotcheckTaskService;
        }

        /// <summary>
        /// 添加（点检任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquSpotcheckTaskSaveDto saveDto)
        {
             await _equSpotcheckTaskService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（点检任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquSpotcheckTaskSaveDto saveDto)
        {
             await _equSpotcheckTaskService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（点检任务）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equSpotcheckTaskService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（点检任务）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSpotcheckTaskDto?> QueryByIdAsync(long id)
        {
            return await _equSpotcheckTaskService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（点检任务）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSpotcheckTaskDto>> QueryPagedListAsync([FromQuery] EquSpotcheckTaskPagedQueryDto pagedQueryDto)
        {
            return await _equSpotcheckTaskService.GetPagedListAsync(pagedQueryDto);
        }      

        /// <summary>
        /// 更改单据状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("operation")]
        [LogDescription("更改单据状态", BusinessType.OTHER)]
        public async Task<long> OperationOrderAsync([FromBody] EquSpotcheckTaskOrderOperationStatusDto requestDto)
        {
            return await _equSpotcheckTaskService.OperationOrderAsync(requestDto);
        }


        /// <summary>
        /// 查询点检单明细项数据(执行-查询)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("snapshot")]
        public async Task<IEnumerable<TaskItemUnionSnapshotView>> querySnapshotItemAsync([FromQuery] SpotcheckTaskSnapshotItemQueryDto requestDto)
        {
            return await _equSpotcheckTaskService.querySnapshotItemAsync(requestDto);
        }

        /// <summary>
        /// 查询明细数据(结果处理)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("sample/pagelist")]
        public async Task<PagedInfo<TaskItemUnionSnapshotView>> QueryItemPagedListAsync([FromQuery] SpotcheckTaskItemPagedQueryDto dto)
        {
            return await _equSpotcheckTaskService.QueryItemPagedListAsync(dto);
        }

        /// <summary>
        /// 保存明细项(执行-保存)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [LogDescription("保存明细项", BusinessType.OTHER)]
        public async Task<long> SaveOrderAsync([FromBody] SpotcheckTaskItemSaveDto requestDto)
        {
            return await _equSpotcheckTaskService.SaveAndUpdateTaskItemAsync(requestDto);
        }

        /// <summary>
        /// 完成明细检验单(执行-完成)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("complete")]
        [LogDescription("完成检验单", BusinessType.OTHER)]
        public async Task<long> CompleteOrderAsync(SpotcheckTaskCompleteDto requestDto)
        {
            return await _equSpotcheckTaskService.CompleteOrderAsync(requestDto);
        }


        /// <summary>
        /// 结果处理(首页-处理)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("close")]
        [LogDescription("结果处理", BusinessType.OTHER)]
        public async Task<long> CloseOrderAsync(SpotcheckTaskCloseDto requestDto)
        {
            return await _equSpotcheckTaskService.CloseOrderAsync(requestDto);
        }

        /// <summary>
        /// 上传单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        [LogDescription("上传单据附件", BusinessType.EXPORT)]
        public async Task SaveAttachmentAsync([FromBody] SpotcheckTaskSaveAttachmentDto dto)
        {
            await _equSpotcheckTaskService.SaveAttachmentAsync(dto);
        }

        /// <summary>
        /// 删除单据附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{orderAnnexId}")]
        [LogDescription("删除单据附件", BusinessType.OTHER)]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _equSpotcheckTaskService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _equSpotcheckTaskService.QueryOrderAttachmentListByIdAsync(orderId);
        }


    }
}