using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.EquEquipmentGroup
{
    /// <summary>
    /// 设备组 服务
    /// </summary>
    public class EquEquipmentGroupService : IEquEquipmentGroupService
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
        private readonly AbstractValidator<EquEquipmentGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（设备组）
        /// </summary>
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;

        /// <summary>
        /// 仓储（设备）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentGroupRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        public EquEquipmentGroupService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquEquipmentGroupSaveDto> validationSaveRules,
            IEquEquipmentGroupRepository equEquipmentGroupRepository,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquEquipmentGroupSaveDto createDto)
        {
            if (createDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 验证DTO
            createDto.EquipmentGroupCode = createDto.EquipmentGroupCode.ToTrimSpace();
            createDto.EquipmentGroupCode = createDto.EquipmentGroupCode.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<EquEquipmentGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            // 编码唯一性验证
            var checkEntity = await _equEquipmentGroupRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.EquipmentGroupCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12700)).WithData("Code", entity.EquipmentGroupCode);

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equEquipmentGroupRepository.InsertAsync(entity);
                if (createDto.EquipmentIDs.Any() == true)
                {
                    rows += await _equEquipmentRepository.UpdateEquipmentGroupIdAsync(new UpdateEquipmentGroupIdCommand
                    {
                        EquipmentGroupId = entity.Id,
                        EquipmentIds = createDto.EquipmentIDs
                    });
                }

                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquEquipmentGroupSaveDto modifyDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);
            //判断名称重复

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquEquipmentGroupEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _equEquipmentGroupRepository.UpdateAsync(entity);
                rows += await _equEquipmentRepository.ClearEquipmentGroupIdAsync(entity.Id);
                rows += await _equEquipmentRepository.UpdateEquipmentGroupIdAsync(new UpdateEquipmentGroupIdCommand
                {
                    EquipmentGroupId = entity.Id,
                    EquipmentIds = modifyDto.EquipmentIDs
                });
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equEquipmentGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equEquipmentGroupRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentGroupListDto>> GetPagedListAsync(EquEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equEquipmentGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentGroupListDto>());
            return new PagedInfo<EquEquipmentGroupListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（设备组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupDto> GetDetailAsync(long id)
        {
            EquEquipmentGroupDto dto = new();
            IEnumerable<EquEquipmentEntity> equipmentEntitys;

            if (id == 0)
            {
                equipmentEntitys = await _equEquipmentRepository.GetByGroupIdAsync(new EquEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 123456, EquipmentGroupId = id });
            }
            else
            {
                dto.Info = (await _equEquipmentGroupRepository.GetByIdAsync(id)).ToModel<EquEquipmentGroupListDto>();
                equipmentEntitys = await _equEquipmentRepository.GetByGroupIdAsync(new EquEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 123456, EquipmentGroupId = id });
            }

            dto.Equipments = equipmentEntitys.Select(s => s.ToModel<EquEquipmentBaseDto>());
            return dto;
        }
    }
}