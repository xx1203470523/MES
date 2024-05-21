using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace Hymson.MES.Services.Services.Equipment.EquEquipmentFaultType
{
    /// <summary>
    /// 设备故障类型服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EquEquipmentFaultTypeService : IEquEquipmentFaultTypeService
    {
        private readonly IEquipmentFaultTypeRepository _equipmentFaultTypeRepository;
        private readonly AbstractValidator<EQualUnqualifiedGroupCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EQualUnqualifiedGroupModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备故障类型服务
        /// </summary>
        /// <param name="equipmentFaultTypeRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// /// <param name="currentUser"></param>
        /// /// <param name="currentSite"></param>
        public EquEquipmentFaultTypeService(IEquipmentFaultTypeRepository equipmentFaultTypeRepository, AbstractValidator<EQualUnqualifiedGroupCreateDto> validationCreateRules, AbstractValidator<EQualUnqualifiedGroupModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _equipmentFaultTypeRepository = equipmentFaultTypeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquipmentFaultTypeDto>> GetPageListAsync(EquipmentFaultTypePagedQueryDto param)
        {
            var equipmentFaultTypePagedQuery = param.ToQuery<EquipmentFaultTypePagedQuery>();
            equipmentFaultTypePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equipmentFaultTypeRepository.GetPagedInfoAsync(equipmentFaultTypePagedQuery);

            //实体到DTO转换 装载数据
            List<EquipmentFaultTypeDto> qualUnqualifiedGroupDtos = PrepareQualUnqualifiedGroupDtos(pagedInfo);
            return new PagedInfo<EquipmentFaultTypeDto>(qualUnqualifiedGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> CreateQualUnqualifiedGroupAsync(EQualUnqualifiedGroupCreateDto param)
        {
            param.Code = param.Code.ToTrimSpace().ToUpperInvariant();
            param.Name = param.Name.Trim();

            //await _validationCreateRules.ValidateAndThrowAsync(param);

            //判断设备故障类型Code是否重复
            var qualUnqualifiedGroupEntity = await _equipmentFaultTypeRepository.GetByCodeAsync(new QualUnqualifiedGroupByCodeQuery { Code = param.Code, Site = _currentSite.SiteId ?? 0 });
            if (qualUnqualifiedGroupEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11206)).WithData("code", param.Code);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            qualUnqualifiedGroupEntity = param.ToEntity<EquEquipmentFaultTypeEntity>();
            qualUnqualifiedGroupEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedGroupEntity.CreatedBy = userId;
            qualUnqualifiedGroupEntity.UpdatedBy = userId;
            qualUnqualifiedGroupEntity.SiteId = _currentSite.SiteId ?? 0;
            //故障现象
            List<EquipmentFaultTypesPhenomenonRelation> qualUnqualifiedCodeGroupRelationlist = new List<EquipmentFaultTypesPhenomenonRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedCodeGroupRelationlist.Add(new EquipmentFaultTypesPhenomenonRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        FaultTypeId= qualUnqualifiedGroupEntity.Id,
                        FaultPhenomenonId= item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }
            //设备组
            List<EQualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<EQualUnqualifiedGroupProcedureRelation>();
            if (param.ProcedureIds != null && param.ProcedureIds.Any())
            {
                foreach (var item in param.ProcedureIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new EQualUnqualifiedGroupProcedureRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        FaultTypeId = qualUnqualifiedGroupEntity.Id,
                        EquipmentGroupId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //插入设备故障类型数据
            await _equipmentFaultTypeRepository.InsertAsync(qualUnqualifiedGroupEntity);
            //插入设备故障类型关联故障现象
            if (qualUnqualifiedCodeGroupRelationlist != null && qualUnqualifiedCodeGroupRelationlist.Any())
            {
                await _equipmentFaultTypeRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(qualUnqualifiedCodeGroupRelationlist);
            }
            //插入设备故障类型关联设备组
            if (qualUnqualifiedGroupProcedureRelationList != null && qualUnqualifiedGroupProcedureRelationList.Any())
            {
                await _equipmentFaultTypeRepository.InsertQualUnqualifiedGroupProcedureRelationRangAsync(qualUnqualifiedGroupProcedureRelationList);
            }
            ts.Complete();
            return qualUnqualifiedGroupEntity.Id;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedGroupAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }
            var entitys = await _equipmentFaultTypeRepository.GetByIdsAsync(ids);
            if (entitys != null && entitys.Any(a => a.Status != DisableOrEnableEnum.Disable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            var userId = _currentUser.UserName;
            return await _equipmentFaultTypeRepository.DeleteRangAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquipmentFaultTypeDto> PrepareQualUnqualifiedGroupDtos(PagedInfo<EquEquipmentFaultTypeEntity> pagedInfo)
        {
            var qualUnqualifiedGroupDtos = new List<EquipmentFaultTypeDto>();
            foreach (var qualUnqualifiedGroupEntity in pagedInfo.Data)
            {
                var qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<EquipmentFaultTypeDto>();
                qualUnqualifiedGroupDtos.Add(qualUnqualifiedGroupDto);
            }

            return qualUnqualifiedGroupDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedGroupAsync(EQualUnqualifiedGroupModifyDto param)
        {
            //验证DTO
            //await _validationModifyRules.ValidateAndThrowAsync(param);

            var userId = _currentUser.UserName;
            //DTO转换实体
            var qualUnqualifiedGroupEntity = param.ToEntity<EquEquipmentFaultTypeEntity>();
            qualUnqualifiedGroupEntity.UpdatedBy = userId;
            qualUnqualifiedGroupEntity.UpdatedOn = HymsonClock.Now();
            //设备故障现象
            List<EquipmentFaultTypesPhenomenonRelation> qualUnqualifiedCodeGroupRelationlist = new List<EquipmentFaultTypesPhenomenonRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedCodeGroupRelationlist.Add(new EquipmentFaultTypesPhenomenonRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,

                        FaultTypeId = qualUnqualifiedGroupEntity.Id,
                        FaultPhenomenonId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }
            //设备组
            List<EQualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<EQualUnqualifiedGroupProcedureRelation>();
            if (param.ProcedureIds != null && param.ProcedureIds.Any())
            {
                foreach (var item in param.ProcedureIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new EQualUnqualifiedGroupProcedureRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,

                        FaultTypeId = qualUnqualifiedGroupEntity.Id,
                        EquipmentGroupId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }
            //先删在插入
            using var ts = TransactionHelper.GetTransactionScope();
            await _equipmentFaultTypeRepository.UpdateAsync(qualUnqualifiedGroupEntity);

            await _equipmentFaultTypeRepository.RealDelteQualUnqualifiedCodeGroupRelationAsync(param.Id);
            //更新故障现象
            if (qualUnqualifiedCodeGroupRelationlist != null && qualUnqualifiedCodeGroupRelationlist.Any())
            {
                await _equipmentFaultTypeRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(qualUnqualifiedCodeGroupRelationlist);
            }
            //更新设备组
            await _equipmentFaultTypeRepository.RealDelteQualUnqualifiedGroupProcedureRelationAsync(param.Id);
            //不合格组关联工序
            if (qualUnqualifiedGroupProcedureRelationList != null && qualUnqualifiedGroupProcedureRelationList.Any())
            {
                await _equipmentFaultTypeRepository.InsertQualUnqualifiedGroupProcedureRelationRangAsync(qualUnqualifiedGroupProcedureRelationList);
            }
            ts.Complete();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquipmentFaultTypeDto> QueryQualUnqualifiedGroupByIdAsync(long id)
        {
            var qualUnqualifiedGroupEntity = await _equipmentFaultTypeRepository.GetByIdAsync(id);

            if (qualUnqualifiedGroupEntity != null)
            {
                var qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<EquipmentFaultTypeDto>();
                return qualUnqualifiedGroupDto;
            }
            else
            {
                return new EquipmentFaultTypeDto();
            }
        }

        /// <summary>
        /// 获取设备故障类型关联设备故障现象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EquipmentFaultTypePhenomenonRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            var qualUnqualifiedCodeGroupRelationList = await _equipmentFaultTypeRepository.GetQualUnqualifiedCodeGroupRelationAsync(id);
            var qualUnqualifiedGroupCodeRelationList = new List<EquipmentFaultTypePhenomenonRelationDto>();
            if (qualUnqualifiedCodeGroupRelationList != null && qualUnqualifiedCodeGroupRelationList.Any())
            {

                foreach (var item in qualUnqualifiedCodeGroupRelationList)
                {
                    qualUnqualifiedGroupCodeRelationList.Add(new EquipmentFaultTypePhenomenonRelationDto()
                    {
                        Id = item.Id,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UnqualifiedCode=item.Code,
                        UnqualifiedCodeName=item.Name,
                    });
                }
            }
            return qualUnqualifiedGroupCodeRelationList;
        }

        /// <summary>
        /// 获取设备故障类型关联故障现象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EquipmentFaultTypeEquipmentGroupRelationDto>> GetQualUnqualifiedCodeProcedureRelationByIdAsync(long id)
        {
            var qualUnqualifiedCodeProcedureRelationList = await _equipmentFaultTypeRepository.GetQualUnqualifiedCodeProcedureRelationAsync(id);
            var qualUnqualifiedGroupProcedureRelationList = new List<EquipmentFaultTypeEquipmentGroupRelationDto>();
            if (qualUnqualifiedCodeProcedureRelationList != null && qualUnqualifiedCodeProcedureRelationList.Any())
            {

                foreach (var item in qualUnqualifiedCodeProcedureRelationList)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new EquipmentFaultTypeEquipmentGroupRelationDto()
                    {
                        Id = item.Id,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        ProcedureCode=item.ProcedureCode,
                        UnqualifiedCodeName=item.UnqualifiedCodeName,
                    });
                }
            }
            return qualUnqualifiedGroupProcedureRelationList;
        }
    }
}
