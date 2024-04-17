using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（设备点检保养项目） 
    /// </summary>
    public class EquInspectionItemService : IEquInspectionItemService
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
        private readonly AbstractValidator<EquInspectionItemSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备点检保养项目）
        /// </summary>
        private readonly IEquInspectionItemRepository _equInspectionItemRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equInspectionItemRepository"></param>
        public EquInspectionItemService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquInspectionItemSaveDto> validationSaveRules,
            IEquInspectionItemRepository equInspectionItemRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equInspectionItemRepository = equInspectionItemRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquInspectionItemSaveDto saveDto)
        {
            // 判断是否有获取到站点编码
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            saveDto.Code = saveDto.Code.ToTrimSpace().ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var siteId = _currentSite.SiteId ?? 0;
            //判断设备点检项目在系统中是否已经存在
            var inspectionItemQuery= new EquInspectionItemQuery { SiteId = siteId, Code = saveDto.Code};
            var equInspectionItems = await _equInspectionItemRepository.GetEntitiesAsync(inspectionItemQuery);
            if (equInspectionItems != null&& equInspectionItems.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15801)).WithData("code", saveDto.Code);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionItemEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = siteId;

            // 保存
            return await _equInspectionItemRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquInspectionItemSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionItemEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equInspectionItemRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equInspectionItemRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (!ids.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10213));
            }

            return await _equInspectionItemRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquInspectionItemDto?> QueryByIdAsync(long id)
        {
            var equInspectionItemEntity = await _equInspectionItemRepository.GetByIdAsync(id);
            if (equInspectionItemEntity == null)
            {
                return null;
            }

            return equInspectionItemEntity.ToModel<EquInspectionItemDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionItemDto>> GetPagedListAsync(EquInspectionItemPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquInspectionItemPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equInspectionItemRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquInspectionItemDto>());
            return new PagedInfo<EquInspectionItemDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
