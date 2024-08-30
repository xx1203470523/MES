using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO;
using Hymson.MES.Data.Repositories.NIO.Query;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.NIO
{
    /// <summary>
    /// 服务（物料及其关键下级件信息表） 
    /// </summary>
    public class NioPushKeySubordinateService : INioPushKeySubordinateService
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
        /// 仓储接口（物料及其关键下级件信息表）
        /// </summary>
        private readonly INioPushKeySubordinateRepository _nioPushKeySubordinateRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="nioPushKeySubordinateRepository"></param>
        public NioPushKeySubordinateService(ICurrentUser currentUser, ICurrentSite currentSite,
            INioPushKeySubordinateRepository nioPushKeySubordinateRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _nioPushKeySubordinateRepository = nioPushKeySubordinateRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(NioPushKeySubordinateSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushKeySubordinateEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _nioPushKeySubordinateRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(NioPushKeySubordinateSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushKeySubordinateEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _nioPushKeySubordinateRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _nioPushKeySubordinateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _nioPushKeySubordinateRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushKeySubordinateDto?> QueryByIdAsync(long id) 
        {
           var nioPushKeySubordinateEntity = await _nioPushKeySubordinateRepository.GetByIdAsync(id);
           if (nioPushKeySubordinateEntity == null) return null;
           
           return nioPushKeySubordinateEntity.ToModel<NioPushKeySubordinateDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushKeySubordinateDto>> GetPagedListAsync(NioPushKeySubordinatePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<NioPushKeySubordinatePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _nioPushKeySubordinateRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushKeySubordinateDto>());
            return new PagedInfo<NioPushKeySubordinateDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
