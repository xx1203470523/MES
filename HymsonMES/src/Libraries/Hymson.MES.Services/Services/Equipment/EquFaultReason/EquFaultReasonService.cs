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
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 设备故障原因表 服务
    /// </summary>
    public class EquFaultReasonService : IEquFaultReasonService
    {
        /// <summary>
        /// 设备故障原因表 仓储
        /// </summary>
        private readonly IEquFaultReasonRepository _equFaultReasonRepository;
        private readonly AbstractValidator<EquFaultReasonSaveDto> _validationSaveRules;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="equFaultReasonRepository"></param>
        public EquFaultReasonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultReasonSaveDto> validationCreateRules,
            IEquFaultReasonRepository equFaultReasonRepository)
        {
            _currentSite = currentSite;
            _currentUser = currentUser;
            _validationSaveRules = validationCreateRules;
            _equFaultReasonRepository = equFaultReasonRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="EquFaultReasonCreateDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonCreateDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(EquFaultReasonCreateDto);

            //DTO转换实体
            var EquFaultReasonEntity = EquFaultReasonCreateDto.ToEntity<EquFaultReasonEntity>();
            EquFaultReasonEntity.Id = IdGenProvider.Instance.CreateId();
            EquFaultReasonEntity.CreatedBy = _currentUser.UserName;
            EquFaultReasonEntity.UpdatedBy = _currentUser.UserName;
            EquFaultReasonEntity.CreatedOn = HymsonClock.Now();
            EquFaultReasonEntity.UpdatedOn = HymsonClock.Now();
            EquFaultReasonEntity.FaultReasonCode = EquFaultReasonEntity.FaultReasonCode.ToUpper();
            EquFaultReasonEntity.SiteId = _currentSite.SiteId;

            //判断编号是否已经存在
            var exists = await _equFaultReasonRepository.GetEquFaultReasonEntitiesAsync(new EquFaultReasonQuery()
            {
                SiteId = EquFaultReasonEntity.SiteId,
                FaultReasonCode = EquFaultReasonEntity.FaultReasonCode,
            });
            if (exists != null && exists.Any() == true)
            {
                throw new BusinessException(nameof(ErrorCode.MES13002)).WithData("FaultReasonCode", EquFaultReasonEntity.FaultReasonCode);
            }

            //入库
            return await _equFaultReasonRepository.InsertAsync(EquFaultReasonEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquFaultReasonModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonModifyDto)
        {
            if (EquFaultReasonModifyDto == null)
            {
                throw new ValidationException(ErrorCode.MES13003);
            }

            //DTO转换实体
            var EquFaultReasonEntity = EquFaultReasonModifyDto.ToEntity<EquFaultReasonEntity>();
            EquFaultReasonEntity.UpdatedBy = _currentUser.UserName;
            EquFaultReasonEntity.UpdatedOn = HymsonClock.Now();
            EquFaultReasonEntity.SiteId = _currentSite.SiteId;

            //验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(EquFaultReasonModifyDto);

            var modelOrigin = await _equFaultReasonRepository.GetByIdAsync(EquFaultReasonEntity.Id);
            if (modelOrigin == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES13004));
            }
            //判断编号是否已经存在
            var exists = (await _equFaultReasonRepository.GetEquFaultReasonEntitiesAsync(new EquFaultReasonQuery()
            {
                SiteId = EquFaultReasonEntity.SiteId,
                FaultReasonCode = EquFaultReasonEntity.FaultReasonCode,
            })).Where(x => x.Id != EquFaultReasonEntity.Id).ToList();
            if (exists != null && exists.Any() == true)
            {
                throw new BusinessException(nameof(ErrorCode.MES13002)).WithData("FaultReasonCode", EquFaultReasonEntity.FaultReasonCode);
            }

            await _equFaultReasonRepository.UpdateAsync(EquFaultReasonEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquFaultReasonAsync(long id)
        {
            await _equFaultReasonRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquFaultReasonAsync(long[] ids)
        {
            if (ids == null || ids.Any() == false)
            {
                throw new ValidationException(ErrorCode.MES13005);
            }

            return await _equFaultReasonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="EquFaultReasonPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto EquFaultReasonPagedQueryDto)
        {
            var EquFaultReasonPagedQuery = EquFaultReasonPagedQueryDto.ToQuery<EquFaultReasonPagedQuery>();
            EquFaultReasonPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equFaultReasonRepository.GetPagedInfoAsync(EquFaultReasonPagedQuery);

            //实体到DTO转换 装载数据
            List<EquFaultReasonDto> EquFaultReasonDtos = PrepareEquFaultReasonDtos(pagedInfo).ToList();
            return new PagedInfo<EquFaultReasonDto>(EquFaultReasonDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static IEnumerable<EquFaultReasonDto> PrepareEquFaultReasonDtos(PagedInfo<EquFaultReasonEntity> pagedInfo)
        {
            var EquFaultReasonDtos = new List<EquFaultReasonDto>();
            foreach (var EquFaultReasonEntity in pagedInfo.Data)
            {
                var EquFaultReasonDto = EquFaultReasonEntity.ToModel<EquFaultReasonDto>();
                EquFaultReasonDtos.Add(EquFaultReasonDto);
            }

            return EquFaultReasonDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id)
        {
            var EquFaultReasonEntity = await _equFaultReasonRepository.GetByIdAsync(id);
            var dto = EquFaultReasonEntity.ToModel<CustomEquFaultReasonDto>();
            return dto;
        }
    }
}
