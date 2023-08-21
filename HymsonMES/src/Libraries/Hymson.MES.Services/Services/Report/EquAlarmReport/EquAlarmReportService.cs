using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.EquAlarmReport
{
    public class EquAlarmReportService : IEquAlarmReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquAlarmRepository _equAlarmRepository;
        public EquAlarmReportService(IEquAlarmRepository equAlarmRepository, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _equAlarmRepository = equAlarmRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取设备报警报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquAlarmReportViewDto>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquAlarmReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equAlarmRepository.GetEquAlarmReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<EquAlarmReportViewDto>();
            });
            return new PagedInfo<EquAlarmReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
