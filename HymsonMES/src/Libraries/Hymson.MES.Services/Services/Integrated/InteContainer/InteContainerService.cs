using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Data;
using System.Text.RegularExpressions;
using static Mysqlx.Notice.Warning.Types;

namespace Hymson.MES.Services.Services.Integrated.InteContainer
{
    /// <summary>
    /// 服务（容器维护）
    /// </summary>
    public class InteContainerService : IInteContainerService
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
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<InteContainerSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（容器维护）
        /// </summary>
        private readonly IInteContainerRepository _inteContainerRepository;

        /// <summary>
        ///  仓储（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        ///  仓储（物料组）
        /// </summary>
        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteContainerRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procMaterialGroupRepository"></param>
        public InteContainerService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            AbstractValidator<InteContainerSaveDto> validationSaveRules,
            IInteContainerRepository inteContainerRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcMaterialGroupRepository procMaterialGroupRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteContainerRepository = inteContainerRepository;
            _procMaterialRepository = procMaterialRepository;
            _procMaterialGroupRepository = procMaterialGroupRepository;
        }


        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteContainerSaveDto createDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(createDto);
            await ValidationSaveDto(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<InteContainerEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 123456;

            // 验证是否相同物料或者物料组已经设置过
            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
            {
                DefinitionMethod = entity.DefinitionMethod,
                MaterialId = entity.MaterialId,
                MaterialGroupId = entity.MaterialGroupId,
                Level = entity.Level
            });

            if (entityByRelation != null) throw new CustomerValidationException(nameof(ErrorCode.MES12503));

            // 保存实体
            return await _inteContainerRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteContainerSaveDto modifyDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);
            await ValidationSaveDto(modifyDto);
            var inteContainerEntity = await _inteContainerRepository.GetByIdAsync(modifyDto.Id ?? 0);
            if (inteContainerEntity.Status != SysDataStatusEnum.Build && modifyDto.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12510));
            }

            // DTO转换实体
            var entity = modifyDto.ToEntity<InteContainerEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 验证是否相同物料或者物料组已经设置过
            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
            {
                DefinitionMethod = entity.DefinitionMethod,
                MaterialId = entity.MaterialId,
                MaterialGroupId = entity.MaterialGroupId,
                Level = entity.Level
            });

            if (entityByRelation != null && entityByRelation.Id != entity.Id) throw new CustomerValidationException(nameof(ErrorCode.MES12503));

            // 更新实体
            return await _inteContainerRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（容器维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var list = await _inteContainerRepository.GetByIdsAsync(idsArr);
            if (list != null && list.Any(x => x.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12509));
            }
            return await _inteContainerRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 获取分页数据（容器维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteContainerDto>> GetPagedListAsync(InteContainerPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteContainerPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _inteContainerRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteContainerDto>());
            return new PagedInfo<InteContainerDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerDto> GetDetailAsync(long id)
        {
            var inteContainerEntity = await _inteContainerRepository.GetByIdAsync(id);
            if (inteContainerEntity == null) return null;

            return inteContainerEntity.ToModel<InteContainerDto>();
        }

        /// <summary>
        /// 验证对象
        /// </summary>
        /// <param name="dto"></param>
        private async Task ValidationSaveDto(InteContainerSaveDto dto)
        {
            if (dto == null) throw new CustomerValidationException(nameof(ErrorCode.MES12503));

            var pattern = @"^[1-9]\d*$";
            if (Regex.IsMatch($"{dto.Minimum}", pattern) == false) throw new CustomerValidationException(nameof(ErrorCode.MES12504));
            if (Regex.IsMatch($"{dto.Maximum}", pattern) == false) throw new CustomerValidationException(nameof(ErrorCode.MES12505));
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), dto.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES12511));

            // 判断物料/物料组是否存在
            switch (dto.DefinitionMethod)
            {
                case DefinitionMethodEnum.Material:
                    _ = await _procMaterialRepository.GetByIdAsync(dto.MaterialId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10204));
                    break;
                case DefinitionMethodEnum.MaterialGroup:
                    _ = await _procMaterialGroupRepository.GetByIdAsync(dto.MaterialGroupId) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10219));
                    break;
                default:
                    break;
            }
        }

    }
}
