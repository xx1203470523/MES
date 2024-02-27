using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 顷刻能源设备接口控制器
    /// </summary>
    [ApiController]
    [Route("QknyEqu/api/v1")]
    public class QknyController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyController()
        {

        }

        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorLogin")]
        [LogDescription("操作员登录", BusinessType.OTHER, "OperatorLoginMes001", ReceiverTypeEnum.MES)]
        public async Task OperatorLoginAsync(OperationLoginDto dto)
        {
            List<string> list = null;
            int count = list.Count;

            //throw new CustomerValidationException(nameof(ErrorCode.MES10100));
        }

        /// <summary>
        /// 时间同步
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TimeSynch")]
        [LogDescription("时间同步", BusinessType.OTHER, "TimeSynchMes002", ReceiverTypeEnum.MES)]
        public async Task<string> TimeSynchAsync(QknyBaseDto dto)
        {
            DateTime date = HymsonClock.Now();

            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 进站多个
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundMore")]
        [LogDescription("进站多个", BusinessType.OTHER, "TimeSynchMes010", ReceiverTypeEnum.MES)]
        public async Task<InboundMoreReturnDto> InboundMoreAsync(InboundMoreDto dto)
        {
            InboundMoreReturnDto result = new InboundMoreReturnDto();

            return result;
        }
    }
}
