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

        /// <summary>
        /// 仓储接口（配方维护）
        /// </summary>
        private readonly IProcFormulaRepository _procFormulaRepository;

        private readonly ILocalizationService _localizationService;

        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IProcProcessEquipmentGroupRepository _procProcessEquipmentGroupRepository;

        private readonly IProcFormulaOperationGroupRepository _procFormulaOperationGroupRepository;

        private readonly IProcFormulaDetailsRepository _procFormulaDetailsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procFormulaRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessEquipmentGroupRepository"></param>
        /// <param name="procFormulaOperationGroupRepository"></param>
        public ProcFormulaService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcFormulaSaveDto> validationSaveRules, 
            IProcFormulaRepository procFormulaRepository, ILocalizationService localizationService, IProcMaterialRepository procMaterialRepository, IProcProcedureRepository procProcedureRepository, IProcProcessEquipmentGroupRepository procProcessEquipmentGroupRepository, IProcFormulaOperationGroupRepository procFormulaOperationGroupRepository, IProcFormulaDetailsRepository procFormulaDetailsRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procFormulaRepository = procFormulaRepository;
            _localizationService = localizationService;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessEquipmentGroupRepository = procProcessEquipmentGroupRepository;
            _procFormulaOperationGroupRepository = procFormulaOperationGroupRepository;
            _procFormulaDetailsRepository = procFormulaDetailsRepository;
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

            // 保存
            return await _procFormulaRepository.InsertAsync(entity);
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

            var procFormula = await _procFormulaRepository.GetByIdAsync(saveDto.Id);
            if (procFormula == null) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15701));
            }

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcFormulaEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _procFormulaRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _procFormulaRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _procFormulaRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ProcFormulaDetailViewDto?> QueryByIdAsync(long id) 
        {
           var procFormulaEntity = await _procFormulaRepository.GetByIdAsync(id);
           if (procFormulaEntity == null) return null;

            var detailViewDto = procFormulaEntity.ToModel<ProcFormulaDetailViewDto>();

            if (procFormulaEntity.MaterialId > 0) 
            {
                var material= await _procMaterialRepository.GetByIdAsync(procFormulaEntity.MaterialId);
                if (material != null) 
                {
                    detailViewDto.MaterialCode = material.MaterialCode;
                    detailViewDto.MaterialName = material.MaterialName;
                    detailViewDto.MaterialVersion = material.Version??""; 
                }
            }
            if (procFormulaEntity.ProcedureId > 0)
            {
                var procedure = await _procProcedureRepository.GetByIdAsync(procFormulaEntity.ProcedureId);
                if (procedure != null)
                {
                    detailViewDto.ProcedureName = procedure.Name;
                    detailViewDto.ProcedureCode = procedure.Code;
                }
            }
            if (procFormulaEntity.EquipmentGroupId > 0) 
            {
                var processEquipmentGroupEntity = await _procProcessEquipmentGroupRepository.GetByIdAsync(procFormulaEntity.EquipmentGroupId);
                if (processEquipmentGroupEntity != null)
                {
                    detailViewDto.EquipmentGroupCode = processEquipmentGroupEntity.Code;
                    detailViewDto.EquipmentGroupName = processEquipmentGroupEntity.Name;
                }
            }

            if (procFormulaEntity.FormulaOperationGroupId > 0)
            {
                var formulaOperationGroupEntity  = await _procFormulaOperationGroupRepository.GetByIdAsync(procFormulaEntity.FormulaOperationGroupId);
                if (formulaOperationGroupEntity != null)
                {
                    detailViewDto.FormulaOperationGroupCode = formulaOperationGroupEntity.Code;
                    detailViewDto.FormulaOperationGroupName = formulaOperationGroupEntity.Name;
                }
            }

            return detailViewDto;
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
            var formulaDetails=  await _procFormulaDetailsRepository.GetFormulaDetailsByFormulaIdAsync(formulaId);

            return formulaDetails.Select(s => s.ToModel<ProcFormulaDetailsViewDto>());
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10130));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procFormulaRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

    }
}
