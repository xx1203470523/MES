using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfc
{
    /// <summary>
    /// 条码服务
    /// </summary>
    public class ManuSfcService : IManuSfcService
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
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        public ManuSfcService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
        }


        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcPassDownDto>> GetPagedListAsync(ManuSfcPassDownPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcPassDownPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => new ManuSfcPassDownDto
            {
                Id = s.Id,
                SFC = s.SFC,
                IsUsed = s.IsUsed,
                UpdatedOn = s.UpdatedOn,
                OrderCode = s.OrderCode,
                MaterialCode = s.MaterialCode,
                MaterialName = s.MaterialName
            });
            return new PagedInfo<ManuSfcPassDownDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
