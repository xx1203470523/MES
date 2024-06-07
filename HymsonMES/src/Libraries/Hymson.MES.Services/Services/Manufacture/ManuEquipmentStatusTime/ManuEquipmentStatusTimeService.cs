using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime.Query;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（设备状态时间） 
    /// </summary>
    public class ManuEquipmentStatusTimeService : IManuEquipmentStatusTimeService
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
        /// 仓储接口（设备状态时间）
        /// </summary>
        private readonly IManuEquipmentStatusTimeRepository _manuEquipmentStatusTimeRepository;

        /// <summary>
        /// 设备最新状态
        /// </summary>
        private readonly IManuEuqipmentNewestInfoRepository _manuEuqipmentNewestInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuEquipmentStatusTimeRepository"></param>
        /// <param name="manuEuqipmentNewestInfoRepository"></param>
        public ManuEquipmentStatusTimeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuEquipmentStatusTimeRepository manuEquipmentStatusTimeRepository,
            IManuEuqipmentNewestInfoRepository manuEuqipmentNewestInfoRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuEquipmentStatusTimeRepository = manuEquipmentStatusTimeRepository;
            _manuEuqipmentNewestInfoRepository = manuEuqipmentNewestInfoRepository;
        }

        /// <summary>
        /// 设备状态监控报表分页查询
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEquipmentStatusReportViewDto>> GetPageListAsync(ManuEquipmentStatusTimePagedQueryDto pagedQueryDto)
        {
            var pageQuery = new ManuEquipmentStatusTimePagedQuery()
            {
                WorkCenterId = pagedQueryDto.WorkCenterId,
                EquipmentId = pagedQueryDto.EquipmentId,
                SiteId = _currentSite.SiteId ?? 0,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            var pagedInfo = await _manuEquipmentStatusTimeRepository.GetPagedListAsync(pageQuery);
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuEquipmentStatusReportViewDto>());
            return new PagedInfo<ManuEquipmentStatusReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
