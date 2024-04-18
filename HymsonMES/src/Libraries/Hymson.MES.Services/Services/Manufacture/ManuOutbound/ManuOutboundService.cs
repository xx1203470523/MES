using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;

namespace Hymson.MES.Services.Services.Manufacture.ManuOutbound
{
    /// <summary>
    /// 出站服务
    /// </summary>
    public class ManuOutboundService : IManuOutboundService
    {

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        ///构造函数 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuPassStationService"></param>
        public ManuOutboundService(ICurrentUser currentUser, ICurrentSite currentSite, IManuPassStationService manuPassStationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuPassStationService = manuPassStationService;
        }

        /// <summary>
        /// 产出确认
        /// </summary>
        /// <param name="outputConfirmDto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseDto>> OutputConfirmAsync(OutputConfirmDto outputConfirmDto)
        {
          var result = new Dictionary<string, JobResponseDto> { }; // 返回结果
            var jobResponses = await _manuPassStationService.OutputConfirmBySFCAsync(new CoreServices.Bos.Manufacture.OutputConfirmBo
            {
                ResourceId = outputConfirmDto.ResourceId,
                ProcedureId = outputConfirmDto.ProcedureId,
                SFC = outputConfirmDto.SFC,
                UnQualifiedQty = outputConfirmDto.UnQualifiedQty,
                QualifiedQty = outputConfirmDto.QualifiedQty,
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                Unqualifieds = outputConfirmDto.Unqualifieds
            }, RequestSourceEnum.Panel);

            foreach (var item in jobResponses)
            {
                result.Add(item.Key, new JobResponseDto
                {
                    IsSuccess = item.Value.IsSuccess,
                    Rows = item.Value.Rows,
                    Content = item.Value.Content,
                    Message = item.Value.Message,
                    Time = item.Value.Time
                });
            }
            return result;
        }
    }
}
