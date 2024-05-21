using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（设备点检项目） 
    /// </summary>
    public class EquSpotcheckItemService : IEquSpotcheckItemService
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
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<EquSpotcheckItemSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备点检项目）
        /// </summary>
        private readonly IEquSpotcheckItemRepository _equSpotcheckItemRepository;

        /// <summary>
        /// 单位
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSpotcheckItemRepository"></param>
        public EquSpotcheckItemService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSpotcheckItemSaveDto> validationSaveRules,
            IEquSpotcheckItemRepository equSpotcheckItemRepository,
            IInteUnitRepository inteUnitRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSpotcheckItemRepository = equSpotcheckItemRepository;
            _inteUnitRepository = inteUnitRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSpotcheckItemSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var Entitys = await _equSpotcheckItemRepository.GetEntitiesAsync(new EquSpotcheckItemQuery
            {
                Code = saveDto.Code,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (Entitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10405)).WithData("Code", saveDto.Code);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckItemEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equSpotcheckItemRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSpotcheckItemUpdateDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckItemEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId;

            return await _equSpotcheckItemRepository.UpdateAsync(entity);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSpotcheckItemRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equSpotcheckItemRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquSpotcheckItemDto?> QueryByIdAsync(long id)
        {
            var equSpotcheckItemEntity = await _equSpotcheckItemRepository.GetByIdAsync(id);
            if (equSpotcheckItemEntity == null) return null;
            var inteUnitEntity = await _inteUnitRepository.GetByIdAsync(equSpotcheckItemEntity.UnitId.GetValueOrDefault());

            var dto = equSpotcheckItemEntity.ToModel<EquSpotcheckItemDto>();
            dto.Unit = inteUnitEntity.Code;
            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckItemDto>> GetPagedListAsync(EquSpotcheckItemPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSpotcheckItemPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSpotcheckItemRepository.GetPagedListAsync(pagedQuery);



            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSpotcheckItemDto>());
            //var result = new PagedInfo<EquSpotcheckItemDto>(Enumerable.Empty<EquSpotcheckItemDto>(), pagedInfo.PageIndex, pagedInfo.PageSize);

            var result = new PagedInfo<EquSpotcheckItemDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);

            if (result.Data.Any())
            {
                var unitIds = result.Data.Select(m => m.UnitId.GetValueOrDefault());
                if (unitIds.Any())
                {
                    var unitEntitys = await _inteUnitRepository.GetByIdsAsync(unitIds.Distinct()!);

                    result.Data = result.Data.Select(s =>
                    {
                        var unit = unitEntitys.FirstOrDefault(f => f.Id == s.UnitId);
                        if (unit != null)
                        {
                            s.Unit = unit.Code;
                        }
                        return s;
                    });
                }

            }

            return result;
        }

    }
}
