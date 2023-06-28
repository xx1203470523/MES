using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    public class EquHeartbeatReportService : IEquHeartbeatReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquHeartbeatRepository _equHeartbeatRepository;
        public EquHeartbeatReportService(IEquHeartbeatRepository equHeartbeatRepository, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _equHeartbeatRepository = equHeartbeatRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquHeartbeatReportViewDto>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquHeartbeatReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equHeartbeatRepository.GetEquHeartbeatReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<EquHeartbeatReportViewDto>();
            });
            return new PagedInfo<EquHeartbeatReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
