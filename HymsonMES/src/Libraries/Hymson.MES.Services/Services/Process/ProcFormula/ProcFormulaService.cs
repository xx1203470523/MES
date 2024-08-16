using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（配方维护） 
    /// </summary>
    public class ProcFormulaService : IProcFormulaService
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
        private readonly AbstractValidator<ProcFormulaSaveDto> _validationSaveRules;
        private readonly AbstractValidator<ProcFormulaDetailsDto> _validationDetailsDtoRules;

        /// <summary>
        /// 仓储接口（配方维护）
        /// </summary>
        private readonly IProcFormulaRepository _procFormulaRepository;

        private readonly ILocalizationService _localizationService;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IProcProcessEquipmentGroupRepository _procProcessEquipmentGroupRepository;

        private readonly IProcFormulaOperationGroupRepository _procFormulaOperationGroupRepository;

        private readonly IProcFormulaOperationRepository _procFormulaOperationRepository;

        private readonly IProcFormulaDetailsRepository _procFormulaDetailsRepository;

        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;

        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="validationDetailsDtoRules"></param>
        /// <param name="procFormulaRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessEquipmentGroupRepository"></param>
        /// <param name="procFormulaOperationGroupRepository"></param>
        /// <param name="procFormulaDetailsRepository"></param>
        /// <param name="procMaterialGroupRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="procFormulaOperationRepository"></param>
        public ProcFormulaService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcFormulaSaveDto> validationSaveRules, AbstractValidator<ProcFormulaDetailsDto> validationDetailsDtoRules,
            IProcFormulaRepository procFormulaRepository, ILocalizationService localizationService, IProcMaterialRepository procMaterialRepository, IProcProcedureRepository procProcedureRepository, IProcProcessEquipmentGroupRepository procProcessEquipmentGroupRepository, IProcFormulaOperationGroupRepository procFormulaOperationGroupRepository, IProcFormulaDetailsRepository procFormulaDetailsRepository, IProcMaterialGroupRepository procMaterialGroupRepository, IProcParameterRepository procParameterRepository, IProcFormulaOperationRepository procFormulaOperationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationDetailsDtoRules = validationDetailsDtoRules;
            _procFormulaRepository = procFormulaRepository;
            _localizationService = localizationService;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessEquipmentGroupRepository = procProcessEquipmentGroupRepository;
            _procFormulaOperationGroupRepository = procFormulaOperationGroupRepository;
            _procFormulaDetailsRepository = procFormulaDetailsRepository;
            _procMaterialGroupRepository = procMaterialGroupRepository;
            _procParameterRepository = procParameterRepository;
            _procFormulaOperationRepository = procFormulaOperationRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProcFormulaSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            if (saveDto.ProcFormulaDetailsDtos != null && saveDto.ProcFormulaDetailsDtos.Any())
            {
                foreach (var item in saveDto.ProcFormulaDetailsDtos)
                {
                    await _validationDetailsDtoRules.ValidateAndThrowAsync(item);
                }

                //查询操作
                var formulaOperations = await _procFormulaOperationRepository.GetByIdsAsync(saveDto.ProcFormulaDetailsDtos.Select(x => x.FormulaOperationId).Distinct().ToArray());

                foreach (var item in saveDto.ProcFormulaDetailsDtos)
                {
                    var formulaOperation = formulaOperations.FirstOrDefault(x => x.Id == item.FormulaOperationId);

                    if (formulaOperation == null) throw new CustomerValidationException(nameof(ErrorCode.MES15723)).WithData("id", item.FormulaOperationId);

                    switch (formulaOperation.Type)
                    {
                        case Core.Enums.Process.FormulaOperationTypeEnum.Material:
                            if (!item.MaterialId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15724));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.MaterialGroup:
                            if (!item.MaterialGroupId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15725));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.FixedValue:
                            //这个不需要设置值
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.SetValue:
                            if (string.IsNullOrWhiteSpace(item.FunctionCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15726));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.ParameterValue:
                            if (!item.ParameterId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15727));
                            break;
                        default:
                            break;
                    }

                }
            }

            #region 验证 配方编码 + 版本是否唯一
            var formulaByCodeAndVersion = await _procFormulaRepository.GetByCodeAndVersionAsync(new ProcFormulaByCodeAndVersion { SiteId = _currentSite.SiteId ?? 0, Code = saveDto.Code, Version = saveDto.Version });
            if (formulaByCodeAndVersion != null) throw new CustomerValidationException(nameof(ErrorCode.MES15703)).WithData("code", saveDto.Code).WithData("version", saveDto.Version);
            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcFormulaEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            #region 组装详情数据
            var sort = 0;

            var procFormulaDetailsEntitys = new List<ProcFormulaDetailsEntity>();
            if (saveDto.ProcFormulaDetailsDtos != null)
                foreach (var (detail, index) in saveDto.ProcFormulaDetailsDtos.Select((value, i) => (value, i)))
                //foreach (var detail in saveDto.ProcFormulaDetailsDtos)
                {
                    sort++;
                    var detailEntity = detail.ToEntity<ProcFormulaDetailsEntity>();

                    //校验上下限
                    if ((detailEntity.UpperLimit - detailEntity.LowLimit) < 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15730)).WithData("line", index);
                    }

                    detailEntity.Id = IdGenProvider.Instance.CreateId();
                    detailEntity.Sort = sort;
                    detailEntity.CreatedBy = updatedBy;
                    detailEntity.CreatedOn = updatedOn;
                    detailEntity.UpdatedBy = updatedBy;
                    detailEntity.UpdatedOn = updatedOn;
                    detailEntity.SiteId = _currentSite.SiteId ?? 0;

                    detailEntity.FormulaId = entity.Id;
                    procFormulaDetailsEntitys.Add(detailEntity);
                }

            #endregion

            var row = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                row = await _procFormulaRepository.InsertAsync(entity);

                if (procFormulaDetailsEntitys.Any()) await _procFormulaDetailsRepository.InsertRangeAsync(procFormulaDetailsEntitys);

                ts.Complete();
            }
            return row;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ProcFormulaSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            if (saveDto.Id <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15711));
            }

            if (saveDto.ProcFormulaDetailsDtos != null && saveDto.ProcFormulaDetailsDtos.Any())
                foreach (var item in saveDto.ProcFormulaDetailsDtos)
                {
                    await _validationDetailsDtoRules.ValidateAndThrowAsync(item);
                }

            var procFormula = await _procFormulaRepository.GetByIdAsync(saveDto.Id);
            if (procFormula == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15701));
            }

            //校验 详情中 某些参数不能为空
            if (saveDto.ProcFormulaDetailsDtos != null && saveDto.ProcFormulaDetailsDtos.Any())
            {
                //查询操作
                var formulaOperations = await _procFormulaOperationRepository.GetByIdsAsync(saveDto.ProcFormulaDetailsDtos.Select(x => x.FormulaOperationId).Distinct().ToArray());

                foreach (var item in saveDto.ProcFormulaDetailsDtos)
                {
                    var formulaOperation = formulaOperations.FirstOrDefault(x => x.Id == item.FormulaOperationId);

                    if (formulaOperation == null) throw new CustomerValidationException(nameof(ErrorCode.MES15723)).WithData("id", item.FormulaOperationId);

                    switch (formulaOperation.Type)
                    {
                        case Core.Enums.Process.FormulaOperationTypeEnum.Material:
                            if (!item.MaterialId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15724));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.MaterialGroup:
                            if (!item.MaterialGroupId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15725));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.FixedValue:
                            //这个不需要设置值
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.SetValue:
                            if (string.IsNullOrWhiteSpace(item.FunctionCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15726));
                            break;
                        case Core.Enums.Process.FormulaOperationTypeEnum.ParameterValue:
                            if (!item.ParameterId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES15727));
                            break;
                        default:
                            break;
                    }

                }
            }

            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcFormulaEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = updatedOn;

            #region 组装详情数据
            var procFormulaDetailsEntitys = new List<ProcFormulaDetailsEntity>();

            var sort = 0;
            if (saveDto.ProcFormulaDetailsDtos != null)
                foreach (var (detail, index) in saveDto.ProcFormulaDetailsDtos.Select((value, i) => (value, i)))
                //foreach (var detail in saveDto.ProcFormulaDetailsDtos)
                {
                    sort++;
                    var detailEntity = detail.ToEntity<ProcFormulaDetailsEntity>();

                    //校验上下限
                    if ((detailEntity.UpperLimit - detailEntity.LowLimit) < 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15730)).WithData("line", index + 1);
                    }

                    detailEntity.Id = IdGenProvider.Instance.CreateId();
                    detailEntity.Sort = sort;
                    detailEntity.CreatedBy = _currentUser.UserName;
                    detailEntity.CreatedOn = updatedOn;
                    detailEntity.UpdatedBy = _currentUser.UserName;
                    detailEntity.UpdatedOn = updatedOn;
                    detailEntity.SiteId = _currentSite.SiteId ?? 0;

                    detailEntity.FormulaId = entity.Id;
                    procFormulaDetailsEntitys.Add(detailEntity);
                }

            #endregion

            var row = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                row = await _procFormulaRepository.UpdateAsync(entity);

                await _procFormulaDetailsRepository.DeletesTrueByFormulaIdsAsync(new long[] { entity.Id });

                if (procFormulaDetailsEntitys.Any()) await _procFormulaDetailsRepository.InsertRangeAsync(procFormulaDetailsEntitys);

                ts.Complete();
            }
            return row;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await DeletesAsync(new long[] { id });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (ids.Length < 1) throw new CustomerValidationException(nameof(ErrorCode.MES10102));

            #region 参数校验
            var entitys = await _procFormulaRepository.GetByIdsAsync(ids);

            if (entitys != null && entitys.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }
            #endregion

            var row = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                row = await _procFormulaRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = ids,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });

                await _procFormulaDetailsRepository.DeletesTrueByFormulaIdsAsync(ids);
                ts.Complete();
            }

            return row;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcFormulaViewDto?> QueryByIdAsync(long id)
        {
            var procFormulaEntity = await _procFormulaRepository.GetByIdAsync(id);
            if (procFormulaEntity == null) return null;

            var procFormulaViewDto = procFormulaEntity.ToModel<ProcFormulaViewDto>();

            if (procFormulaEntity.MaterialId > 0)
            {
                var material = await _procMaterialRepository.GetByIdAsync(procFormulaEntity.MaterialId);
                if (material != null)
                {
                    procFormulaViewDto.MaterialCode = material.MaterialCode;
                    procFormulaViewDto.MaterialName = material.MaterialName;
                    procFormulaViewDto.MaterialVersion = material.Version ?? "";
                }
            }
            if (procFormulaEntity.ProcedureId > 0)
            {
                var procedure = await _procProcedureRepository.GetByIdAsync(procFormulaEntity.ProcedureId);
                if (procedure != null)
                {
                    procFormulaViewDto.ProcedureName = procedure.Name;
                    procFormulaViewDto.ProcedureCode = procedure.Code;
                }
            }
            if (procFormulaEntity.EquipmentGroupId > 0)
            {
                var processEquipmentGroupEntity = await _procProcessEquipmentGroupRepository.GetByIdAsync(procFormulaEntity.EquipmentGroupId);
                if (processEquipmentGroupEntity != null)
                {
                    procFormulaViewDto.EquipmentGroupCode = processEquipmentGroupEntity.Code;
                    procFormulaViewDto.EquipmentGroupName = processEquipmentGroupEntity.Name;
                }
            }

            if (procFormulaEntity.FormulaOperationGroupId > 0)
            {
                var formulaOperationGroupEntity = await _procFormulaOperationGroupRepository.GetByIdAsync(procFormulaEntity.FormulaOperationGroupId);
                if (formulaOperationGroupEntity != null)
                {
                    procFormulaViewDto.FormulaOperationGroupCode = formulaOperationGroupEntity.Code;
                    procFormulaViewDto.FormulaOperationGroupName = formulaOperationGroupEntity.Name;
                }
            }

            return procFormulaViewDto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaViewDto>> GetPagedListAsync(ProcFormulaPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcFormulaPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procFormulaRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcFormulaViewDto>());
            return new PagedInfo<ProcFormulaViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="formulaId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaDetailsViewDto>> GetFormulaDetailsByFormulaIdAsync(long formulaId)
        {
            var formulaDetails = await _procFormulaDetailsRepository.GetFormulaDetailsByFormulaIdAsync(formulaId);

            if (formulaDetails == null || !formulaDetails.Any()) return Enumerable.Empty<ProcFormulaDetailsViewDto>();

            //查找物料
            var materialIds = formulaDetails.Where(x => x.MaterialId.HasValue).Select(x => x.MaterialId!.Value).Distinct().ToArray();
            var materials = (materialIds != null && materialIds.Any()) ? await _procMaterialRepository.GetByIdsAsync(materialIds) : null;

            //查找物料组
            var materialGroupIds = formulaDetails.Where(x => x.MaterialGroupId.HasValue).Select(x => x.MaterialGroupId!.Value).Distinct().ToArray();
            var materialGroups = materialGroupIds.Any() ? await _procMaterialGroupRepository.GetByIdsAsync(materialGroupIds) : null;

            //查找参数
            var parameterIds = formulaDetails.Where(x => x.ParameterId.HasValue).Select(x => x.ParameterId!.Value).Distinct().ToArray();
            var parameters = parameterIds.Any() ? await _procParameterRepository.GetByIdsAsync(parameterIds) : null;

            var formulaDetailViewDtos = new List<ProcFormulaDetailsViewDto>();

            foreach (var item in formulaDetails)
            {
                var formulaDetailViewDto = item.ToModel<ProcFormulaDetailsViewDto>();

                var material = materials?.FirstOrDefault(x => x.Id == item.MaterialId);
                if (material != null)
                {
                    formulaDetailViewDto.MaterialCode = material.MaterialCode;
                }

                var materialGroup = materialGroups?.FirstOrDefault(x => x.Id == item.MaterialGroupId);
                if (materialGroup != null)
                {
                    formulaDetailViewDto.MaterialGroupCode = materialGroup.GroupCode;
                }

                var parameter = parameters?.FirstOrDefault(x => x.Id == item.ParameterId);
                if (parameter != null)
                {
                    formulaDetailViewDto.ParameterCode = parameter.ParameterCode;
                }

                formulaDetailViewDtos.Add(formulaDetailViewDto);
            }

            return formulaDetailViewDtos.ToList();
            //return formulaDetailViewDtos.OrderBy(x => x.Serial).ToList();
        }


        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _procFormulaRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10136));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            //当是需要更改为启用状态时 判断是否 物料 + 工序 + 工艺设备组 是否已经有启用状态的的配方
            if (changeStatusCommand.Status == SysDataStatusEnum.Enable)
            {
                var sameEnableFormulas = await _procFormulaRepository.GetEntitiesAsync(new ProcFormulaQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    MaterialId = entity.MaterialId,
                    ProcedureId = entity.ProcedureId,
                    EquipmentGroupId = entity.EquipmentGroupId,
                    Status = SysDataStatusEnum.Enable
                });

                if (sameEnableFormulas != null && sameEnableFormulas.Any())
                {
                    var material = await _procMaterialRepository.GetByIdAsync(entity.MaterialId);
                    var procedure = await _procProcedureRepository.GetByIdAsync(entity.ProcedureId);
                    var equipmentGroup = await _procProcessEquipmentGroupRepository.GetByIdAsync(entity.EquipmentGroupId);

                    throw new CustomerValidationException(nameof(ErrorCode.MES15702)).WithData("materialCode", material?.MaterialCode ?? "").WithData("procedureCode", $"{procedure?.Code}({procedure?.Name})").WithData("equipmentGroupCode", equipmentGroup?.Code ?? "");
                }
            }

            #region 操作数据库
            await _procFormulaRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

    }
}
