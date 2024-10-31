using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（领料记录详情） 
    /// </summary>
    public class ManuReturnOrderReportService : IManuReturnOrderReportService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（领料记录详情）
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuReturnOrderReportService(ICurrentUser currentUser, 
            ICurrentSite currentSite,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary> 
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportReturnOrderResultDto>> GetPagedListAsync(ReportReturnOrderQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<ManuRequistionOrderDetailPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuReturnOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data;
            return new PagedInfo<ReportReturnOrderResultDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
