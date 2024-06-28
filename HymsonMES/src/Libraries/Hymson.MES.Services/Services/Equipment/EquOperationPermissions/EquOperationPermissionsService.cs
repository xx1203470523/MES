using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
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
using MimeKit;
using Minio.DataModel;
using System.Net.NetworkInformation;
using System.Security.Policy;
using static Dapper.SqlMapper;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（设备维保权限） 
    /// </summary>
    public class EquOperationPermissionsService : IEquOperationPermissionsService
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
        private readonly AbstractValidator<EquOperationPermissionsSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备维保权限）
        /// </summary>
        private readonly IEquOperationPermissionsRepository _equOperationPermissionsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equOperationPermissionsRepository"></param>
        public EquOperationPermissionsService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquOperationPermissionsSaveDto> validationSaveRules,
            IEquOperationPermissionsRepository equOperationPermissionsRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equOperationPermissionsRepository = equOperationPermissionsRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquOperationPermissionsSaveDto saveDto)
        {
            // 判断是否有获取到站点编码
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO

            var siteId = _currentSite.SiteId ?? 0;
            //判断设备点检项目在系统中是否已经存在
            var equOperationPermissionsQuery = new EquOperationPermissionsQuery { SiteId = siteId, EquipmentId = saveDto.Id, Type = saveDto.Type };
            var equOperationPermissions = await _equOperationPermissionsRepository.GetEntitiesAsync(equOperationPermissionsQuery);
            if (equOperationPermissions != null && equOperationPermissions.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12615))
                    .WithData("EquipmentCode", saveDto.EquipmentCode ?? "")
                    .WithData("Type", saveDto.Type);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            string leaders = saveDto.Leaderids != null ? string.Join(",", saveDto.Leaderids) : "";
            string executorIds = saveDto.Executorids != null ? string.Join(",", saveDto.Executorids) : "";

            // DTO转换实体
            var entity = saveDto.ToEntity<EquOperationPermissionsEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.LeaderIds = leaders;
            entity.ExecutorIds = executorIds;
            entity.Equipmentid = saveDto.Id;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = siteId;

            // 保存
            return await _equOperationPermissionsRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquOperationPermissionsSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            string leaders = saveDto.Leaderids != null ? string.Join(",", saveDto.Leaderids) : "";
            string executorIds = saveDto.Executorids != null ? string.Join(",", saveDto.Executorids) : "";
            var entity = saveDto.ToEntity<EquOperationPermissionsEntity>();
            entity.ExecutorIds = executorIds;
            entity.LeaderIds = leaders;
            entity.Equipmentid = saveDto.Id;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equOperationPermissionsRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> BatchModifyAsync(EquOperationPermissionsBatchSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            var equOperations = await _equOperationPermissionsRepository.GetByIdsAsync(saveDto.Ids.ToArray());
            string leaders = saveDto.Leaderids != null ? string.Join(",", saveDto.Leaderids) : "";
            string executorIds = saveDto.Executorids != null ? string.Join(",", saveDto.Executorids) : "";
            foreach (var equOperation in equOperations)
            {
                equOperation.ExecutorIds = executorIds;
                equOperation.LeaderIds = leaders;
                equOperation.UpdatedBy = _currentUser.UserName;
                equOperation.UpdatedOn= HymsonClock.Now();
            }
            return await _equOperationPermissionsRepository.UpdateRangeAsync(equOperations);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equOperationPermissionsRepository.DeleteAsync(id);
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

            return await _equOperationPermissionsRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquOperationPermissionsDetailDto?> QueryByIdAsync(long id)
        {
            var equInspectionItemEntity = await _equOperationPermissionsRepository.GetByIdAsync(id);
            if (equInspectionItemEntity == null)
            {
                return null;
            }

            var model = new EquOperationPermissionsDetailDto
            {
                Id = equInspectionItemEntity.Id,
                Equipmentid = equInspectionItemEntity.Equipmentid,
                Status = equInspectionItemEntity.Status,
                Type = equInspectionItemEntity.Type,
                Remark = equInspectionItemEntity.Remark,
                EquipmentCode = equInspectionItemEntity.EquipmentCode,
                EquipmentName = equInspectionItemEntity.EquipmentName,
                EquipmentGroupCode = equInspectionItemEntity.EquipmentGroupCode,
                EquipmentGroupName = equInspectionItemEntity.EquipmentGroupName,
                Location = equInspectionItemEntity.Location,
                ExecutorIds = string.IsNullOrWhiteSpace(equInspectionItemEntity.ExecutorIds) ?null: equInspectionItemEntity.ExecutorIds.Split(','),
                LeaderIds = string.IsNullOrWhiteSpace(equInspectionItemEntity.LeaderIds) ?null: equInspectionItemEntity.LeaderIds.Split(','),
                CreatedBy =equInspectionItemEntity.CreatedBy,
                CreatedOn = equInspectionItemEntity.CreatedOn,
                UpdatedBy = equInspectionItemEntity.UpdatedBy,
                UpdatedOn = equInspectionItemEntity.UpdatedOn
            };
            return model;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquOperationPermissionsDto>> GetPagedListAsync(EquOperationPermissionsQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquOperationPermissionsPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equOperationPermissionsRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquOperationPermissionsDto>());
            return new PagedInfo<EquOperationPermissionsDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
