using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（单位维护） 
    /// </summary>
    public class InteUnitService : IInteUnitService
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
        /// 仓储接口（单位维护）
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="inteUnitRepository"></param>
        public InteUnitService(ICurrentUser currentUser, ICurrentSite currentSite,
            //AbstractValidator<InteUnitSaveDto> validationSaveRules, 
            IInteUnitRepository inteUnitRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteUnitRepository = inteUnitRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateInteUnitAsync(InteUnitSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            // 验证DTO
            if (saveDto.Code.Contains(' '))
                throw new CustomerValidationException(nameof(ErrorCode.MES18800));

            saveDto.Name = saveDto.Name.ToTrimSpace();
            if (saveDto.Name == "")
                throw new CustomerValidationException(nameof(ErrorCode.MES18801));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteUnitEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _inteUnitRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = _currentSite.SiteId ?? 0,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);

            // 保存
            return await _inteUnitRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyInteUnitAsync(InteUnitSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            saveDto.Name = saveDto.Name.ToTrimSpace();
            if (saveDto.Name == "") throw new CustomerValidationException(nameof(ErrorCode.MES18801));

            // DTO转换实体
            var entity = saveDto.ToEntity<InteUnitEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;
            return await _inteUnitRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteInteUnitAsync(long id)
        {
            return await _inteUnitRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteUnitAsync(long[] ids)
        {
            return await _inteUnitRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteUnitDto?> QueryInteUnitByIdAsync(long id)
        {
            var inteUnitEntity = await _inteUnitRepository.GetByIdAsync(id);
            if (inteUnitEntity == null) return null;

            return inteUnitEntity.ToModel<InteUnitDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteUnitDto>> GetPagedListAsync(InteUnitPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteUnitPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteUnitRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteUnitDto>());
            return new PagedInfo<InteUnitDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
