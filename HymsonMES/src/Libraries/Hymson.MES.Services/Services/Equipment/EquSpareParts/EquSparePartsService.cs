using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using IdGen;
using Microsoft.JSInterop;
using Minio.DataModel;
using Org.BouncyCastle.Tls;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（备件注册表） 
    /// </summary>
    public class EquSparePartsService : IEquSparePartsService
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
        private readonly AbstractValidator<EquSparePartsSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（备件注册表）
        /// </summary>
        private readonly IEquSparePartsRepository _equSparePartsRepository;
        private readonly IEquSparePartsGroupRepository _equSparePartsGroupRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSparePartsRepository"></param>
        /// <param name="equSparePartsGroupRepository"></param>
        public EquSparePartsService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSparePartsSaveDto> validationSaveRules,
            IEquSparePartsRepository equSparePartsRepository, IEquSparePartsGroupRepository equSparePartsGroupRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSparePartsRepository = equSparePartsRepository;
            _equSparePartsGroupRepository = equSparePartsGroupRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquSparePartsAsync(EquSparePartsSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparePartsEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _equSparePartsRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }


            // 保存
            return await _equSparePartsRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquSparePartsAsync(EquSparePartsSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparePartsEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            if (entity.SparePartsGroupId == 0)
            {
                var equSparePartsEntity = await _equSparePartsRepository.GetByIdAsync(entity.Id);
                entity.SparePartsGroupId = equSparePartsEntity.SparePartsGroupId;
            }


            return await _equSparePartsRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquSparePartsAsync(long id)
        {
            return await _equSparePartsRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSparePartsAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _equSparePartsRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status == DisableOrEnableEnum.Enable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            return await _equSparePartsRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquSparePartsDto?> QueryEquSparePartsByIdAsync(long id)
        {
            var equSparePartsEntity = await _equSparePartsRepository.GetByIdAsync(id);
            if (equSparePartsEntity == null) return null;

            if (equSparePartsEntity.SparePartsGroupId.HasValue)
            {
             var equSparePartTypeEntity = await _equSparePartsGroupRepository.GetByIdAsync(equSparePartsEntity.SparePartsGroupId??0);
                equSparePartsEntity.SparePartsGroup = equSparePartTypeEntity?.Code;
            }
          
            var a= equSparePartsEntity.ToModel<EquSparePartsDto>();
            return a;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsDto>> GetPagedListAsync(EquSparePartsPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparePartsPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparePartsRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartsDto>());
            return new PagedInfo<EquSparePartsDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);

        }


        /// <summary>
        /// 获取分页数据(过滤已有类型的备件)
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsDto>> GetPagedAsync(EquSparePartsPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparePartsPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparePartsRepository.GetPagedInfoNotWithTypeoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartsDto>());
            return new PagedInfo<EquSparePartsDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);

        }

        /// <summary>
        /// 获取备件组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EquSparePartsDto>> GetSparePartsGroupRelationByIdAsync(long id)
        {
            var unqualifiedCodeGroupRelations = await _equSparePartsRepository.GetSparePartsGroupRelationAsync(id);
            var sparePartsGroupRelationList = new List<EquSparePartsDto>();
            if (unqualifiedCodeGroupRelations != null && unqualifiedCodeGroupRelations.Any())
            {

                foreach (var item in unqualifiedCodeGroupRelations)
                {
                    sparePartsGroupRelationList.Add(new EquSparePartsDto()
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = item.Name,
                        SparePartsGroupId = item.SparePartsGroupId,
                        CreatedBy = item.CreatedBy,
                        IsDeleted = item.IsDeleted,
                        CreatedOn = item.CreatedOn
                    });
                }
            }
            return sparePartsGroupRelationList;
        }


    }
}
