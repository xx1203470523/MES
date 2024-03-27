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
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（跨工序时间管控）
    /// </summary>
    public class ProcProcedureTimeControlService : IProcProcedureTimeControlService
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
        private readonly AbstractValidator<ProcProcedureTimeControlCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureTimeControlModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（跨工序时间管控）
        /// </summary>
        private readonly IProcProcedureTimeControlRepository _procProcedureTimeControlRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 公共接口
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procProcedureTimecontrolRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="localizationService"></param>
        public ProcProcedureTimeControlService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ProcProcedureTimeControlCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureTimeControlModifyDto> validationModifyRules,
            IProcProcedureTimeControlRepository procProcedureTimecontrolRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuCommonOldService manuCommonOldService,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procProcedureTimeControlRepository = procProcedureTimecontrolRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuCommonOldService = manuCommonOldService;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProcProcedureTimeControlCreateDto createDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // 验证时间上下限：上限时间是否大于下限时间;上限时间与下限时间是否相等
            if (createDto.UpperLimitMinute == 0) throw new CustomerValidationException(nameof(ErrorCode.MES14211));
            if (createDto.LowerLimitMinute == 0) throw new CustomerValidationException(nameof(ErrorCode.MES14212));
            if (createDto.UpperLimitMinute < createDto.LowerLimitMinute) throw new CustomerValidationException(nameof(ErrorCode.MES14209));
            if (createDto.UpperLimitMinute == createDto.LowerLimitMinute) throw new CustomerValidationException(nameof(ErrorCode.MES14210));

            /*
            // 验证工序：起始工序是否与到达工序为同一工序,	起始工序是否为到达工序的前工序
            if (createDto.FromProcedureId == createDto.ToProcedureId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14207));
            }
            */

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = createDto.ToEntity<ProcProcedureTimeControlEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.Status = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _procProcedureTimeControlRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12900)).WithData("Code", entity.Code);

            // 同产品、同起始工序、同结束工序数据是否已经存在
            var timeControlEntities = await _procProcedureTimeControlRepository.GetEntitiesAsync(new ProcProcedureTimeControlQuery
            {
                ProductId = entity.ProductId,
                FromProcedureId = entity.FromProcedureId,
                ToProcedureId = entity.ToProcedureId,
                SiteId = entity.SiteId
            });
            if (timeControlEntities != null && timeControlEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14206));
            }

            // 当起始和结束为不同工序时，验证起始工序是否在结束工序之前
            if (createDto.FromProcedureId != createDto.ToProcedureId)
            {
                var procMaterial = await _procMaterialRepository.GetByIdAsync(entity.ProductId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES14214));

                // 根据设备查工艺路线验证工序是否在工艺路线上，验证到达工序是否在起始工序之前
                if (procMaterial.ProcessRouteId.HasValue)
                {
                    var isProcessStartBeforeEnd = await _manuCommonOldService.IsProcessStartBeforeEndAsync(procMaterial.ProcessRouteId.Value, entity.FromProcedureId, entity.ToProcedureId);
                    if (!isProcessStartBeforeEnd) throw new CustomerValidationException(nameof(ErrorCode.MES14208));
                }
            }

            return await _procProcedureTimeControlRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ProcProcedureTimeControlModifyDto modifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            // 验证时间上下限：上限时间是否大于下限时间;上限时间与下限时间是否相等
            if (modifyDto.UpperLimitMinute == 0) throw new CustomerValidationException(nameof(ErrorCode.MES14211));
            if (modifyDto.LowerLimitMinute == 0) throw new CustomerValidationException(nameof(ErrorCode.MES14212));
            if (modifyDto.UpperLimitMinute < modifyDto.LowerLimitMinute) throw new CustomerValidationException(nameof(ErrorCode.MES14209));
            if (modifyDto.UpperLimitMinute == modifyDto.LowerLimitMinute) throw new CustomerValidationException(nameof(ErrorCode.MES14210));

            /*
            // 验证工序：起始工序是否与到达工序为同一工序,	起始工序是否为到达工序的前工序
            if (modifyDto.FromProcedureId == modifyDto.ToProcedureId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14207));
            }
            */

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = modifyDto.ToEntity<ProcProcedureTimeControlEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            var modelOrigin = await _procProcedureTimeControlRepository.GetByIdAsync(entity.Id)
                ?? throw new NotFoundException(nameof(ErrorCode.MES14213));

            if (modelOrigin.Status != SysDataStatusEnum.Build && entity.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            // 同产品、同起始工序、同结束工序数据是否已经存在
            var timeControlEntities = await _procProcedureTimeControlRepository.GetEntitiesAsync(new ProcProcedureTimeControlQuery
            {
                ProductId = entity.ProductId,
                FromProcedureId = entity.FromProcedureId,
                ToProcedureId = entity.ToProcedureId,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (timeControlEntities != null && timeControlEntities.Any(a => a.Id != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14206));
            }

            // 当起始和结束为不同工序时，验证起始工序是否在结束工序之前
            if (modifyDto.FromProcedureId != modifyDto.ToProcedureId)
            {
                var procMaterial = await _procMaterialRepository.GetByIdAsync(entity.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES14214));

                // 根据设备查工艺路线验证工序是否在工艺路线上，验证到达工序是否在起始工序之前
                if (procMaterial.ProcessRouteId.HasValue)
                {
                    var isProcessStartBeforeEnd = await _manuCommonOldService.IsProcessStartBeforeEndAsync(procMaterial.ProcessRouteId.Value, entity.FromProcedureId, entity.ToProcedureId);
                    if (!isProcessStartBeforeEnd) throw new CustomerValidationException(nameof(ErrorCode.MES14208));
                }
            }

            return await _procProcedureTimeControlRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _procProcedureTimeControlRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _procProcedureTimeControlRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ProcProcedureTimeControlDetailDto> QueryByIdAsync(long id)
        {
            var entity = await _procProcedureTimeControlRepository.GetByIdAsync(id);
            if (entity == null) return new ProcProcedureTimeControlDetailDto();

            var detailDto = entity.ToModel<ProcProcedureTimeControlDetailDto>();
            // 查询物料信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(detailDto.ProductId);
            if (procMaterialEntity != null)
            {
                detailDto.ProductCode = procMaterialEntity.MaterialCode;
                detailDto.ProductName = procMaterialEntity.MaterialName;
                detailDto.ProcessRouteId = procMaterialEntity.ProcessRouteId;
                detailDto.Version = procMaterialEntity.Version ?? "";
            }

            // 查询工序信息
            var ids = new[] { detailDto.FromProcedureId, detailDto.ToProcedureId };
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(ids);
            if (procedureEntities != null && procedureEntities.Any())
            {
                detailDto.FromProcedure = procedureEntities.FirstOrDefault(x => x.Id == detailDto.FromProcedureId)?.Code ?? "";
                detailDto.ToProcedure = procedureEntities.FirstOrDefault(x => x.Id == detailDto.ToProcedureId)?.Code ?? "";
            }

            return detailDto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureTimeControlDto>> GetPagedListAsync(ProcProcedureTimeControlPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcProcedureTimeControlPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ProductCode) || !string.IsNullOrWhiteSpace(pagedQueryDto.Version))
            {
                var procMaterialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery
                {
                    SiteId = pagedQuery.SiteId,
                    MaterialCode = pagedQueryDto.ProductCode,
                    Version = pagedQueryDto.Version
                });
                if (procMaterialEntities != null && procMaterialEntities.Any()) pagedQuery.ProductIds = procMaterialEntities.Select(s => s.Id);
                else pagedQuery.ProductIds = Array.Empty<long>();
            }

            // 转换起始工序编码变为工序ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.FromProcedure))
            {
                var procProcedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery
                {
                    SiteId = pagedQuery.SiteId,
                    Code = pagedQueryDto.FromProcedure
                });

                if (procProcedureEntities != null && procProcedureEntities.Any()) pagedQuery.FromProcedureIds = procProcedureEntities.Select(s => s.Id);
                else pagedQuery.FromProcedureIds = Array.Empty<long>();
            }

            // 转换到达工序编码变为工序ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ToProcedure))
            {
                var procProcedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery
                {
                    SiteId = pagedQuery.SiteId,
                    Code = pagedQueryDto.ToProcedure
                });

                if (procProcedureEntities != null && procProcedureEntities.Any()) pagedQuery.ToProcedureIds = procProcedureEntities.Select(s => s.Id);
                else pagedQuery.ToProcedureIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _procProcedureTimeControlRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareDtos(pagedInfo.Data);
            return new PagedInfo<ProcProcedureTimeControlDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }



        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto statusDto)
        {
            #region 参数校验
            if (statusDto.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), statusDto.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (statusDto.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = statusDto.Id,
                Status = statusDto.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _procProcedureTimeControlRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10388));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procProcedureTimeControlRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProcProcedureTimeControlDto>> PrepareDtos(IEnumerable<ProcProcedureTimeControlView> entities)
        {
            List<ProcProcedureTimeControlDto> dtos = new();

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(entities.Select(x => x.ProductId));
            var productDic = productEntities.ToDictionary(x => x.Id, x => x);

            // 读取工序
            var procedureIds = entities.Select(x => x.FromProcedureId).Union(entities.Select(x => x.ToProcedureId));
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);
            var procedureDic = procedureEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<ProcProcedureTimeControlDto>();
                if (dto == null) continue;

                // 产品
                var productEntity = productDic[entity.ProductId];
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                    dto.Version = productEntity.Version ?? "";
                }

                // 起始工序
                var fromProcedureEntity = procedureDic[entity.FromProcedureId];
                if (fromProcedureEntity != null) dto.FromProcedure = fromProcedureEntity.Code;

                // 到达工序
                var toProcedureEntity = procedureDic[entity.ToProcedureId];
                if (toProcedureEntity != null) dto.ToProcedure = toProcedureEntity.Code;

                dtos.Add(dto);
            }

            return dtos;
        }

    }
}
