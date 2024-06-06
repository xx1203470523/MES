using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 设备参数组（Recipe）
    /// </summary>
    public class ProcEquipmentGroupParamService : IProcEquipmentGroupParamService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备参数组 仓储
        /// </summary>
        private readonly IProcEquipmentGroupParamRepository _procEquipmentGroupParamRepository;
        private readonly AbstractValidator<ProcEquipmentGroupParamCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcEquipmentGroupParamModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ProcEquipmentGroupParamDetailCreateDto> _paramDetailValidationCreateRules;

        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessEquipmentGroupRepository _procProcessEquipmentGroupRepository;
        private readonly IProcEquipmentGroupParamDetailRepository _procEquipmentGroupParamDetailRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly ILocalizationService _localizationService;

        private readonly IExcelService _excelService;

        private readonly AbstractValidator<ProcEquipmentGroupParamDetailParamImportDto> _validationImportRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="procEquipmentGroupParamRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="paramDetailValidationCreateRules"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProcessEquipmentGroupRepository"></param>
        /// <param name="procEquipmentGroupParamDetailRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="excelService"></param>
        public ProcEquipmentGroupParamService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcEquipmentGroupParamRepository procEquipmentGroupParamRepository,
            AbstractValidator<ProcEquipmentGroupParamCreateDto> validationCreateRules,
            AbstractValidator<ProcEquipmentGroupParamModifyDto> validationModifyRules,
            AbstractValidator<ProcEquipmentGroupParamDetailCreateDto> paramDetailValidationCreateRules,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProcessEquipmentGroupRepository procProcessEquipmentGroupRepository,
            IProcEquipmentGroupParamDetailRepository procEquipmentGroupParamDetailRepository,
            IProcParameterRepository procParameterRepository,
            ILocalizationService localizationService,
            AbstractValidator<ProcEquipmentGroupParamDetailParamImportDto> validationImportRules,
            IExcelService excelService
            )
        {
            _excelService = excelService;
            _validationImportRules = validationImportRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEquipmentGroupParamRepository = procEquipmentGroupParamRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _paramDetailValidationCreateRules = paramDetailValidationCreateRules;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessEquipmentGroupRepository = procProcessEquipmentGroupRepository;
            _procEquipmentGroupParamDetailRepository = procEquipmentGroupParamDetailRepository;
            _procParameterRepository = procParameterRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(ProcEquipmentGroupParamCreateDto saveDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcEquipmentGroupParamEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.Status = SysDataStatusEnum.Build;

            // 验证是否编码唯一
            await CheckUniqueAsync(entity);

            #region 处理 参数数据
            List<ProcEquipmentGroupParamDetailEntity> details = new();
            if (saveDto.ParamList.Any())
            {
                foreach (var item in saveDto.ParamList)
                {
                    // 验证数据
                    await _paramDetailValidationCreateRules.ValidateAndThrowAsync(item);

                    details.Add(new ProcEquipmentGroupParamDetailEntity
                    {
                        RecipeId = entity.Id,
                        ParamId = item.ParamId,
                        //ParamValue= item.ParamValue,
                        CenterValue = item.CenterValue,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                        DecimalPlaces = item.DecimalPlaces,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId
                    });
                }
            }
            #endregion

            using var trans = TransactionHelper.GetTransactionScope();
            var rows = await _procEquipmentGroupParamRepository.InsertAsync(entity);
            if (rows <= 0)
            {
                trans.Dispose();
            }

            if (details.Any())
            {
                await _procEquipmentGroupParamDetailRepository.InsertsAsync(details);
            }

            trans.Complete();
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task ModifyAsync(ProcEquipmentGroupParamModifyDto saveDto)
        {
            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcEquipmentGroupParamEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 检查数据之前的状态是否允许修改
            var dbEntity = await _procEquipmentGroupParamRepository.GetByIdAsync(saveDto.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            // 验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == dbEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // 验证是否编码唯一
            await CheckUniqueAsync(entity);

            #region 处理 参数数据
            List<ProcEquipmentGroupParamDetailEntity> details = new();
            if (saveDto.ParamList.Any())
            {
                foreach (var item in saveDto.ParamList)
                {
                    // 验证数据
                    await _paramDetailValidationCreateRules.ValidateAndThrowAsync(item);

                    details.Add(new ProcEquipmentGroupParamDetailEntity
                    {
                        RecipeId = entity.Id,
                        ParamId = item.ParamId,
                        //ParamValue = item.ParamValue,
                        CenterValue = item.CenterValue,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                        DecimalPlaces = item.DecimalPlaces,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId
                    });
                }
            }
            #endregion

            using var ts = TransactionHelper.GetTransactionScope();
            await _procEquipmentGroupParamRepository.UpdateAsync(entity);

            await _procEquipmentGroupParamDetailRepository.DeleteTrueByRecipeIdsAsync(new long[] { entity.Id });
            if (details.Any())
            {
                await _procEquipmentGroupParamDetailRepository.InsertsAsync(details);
            }

            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcEquipmentGroupParamAsync(long id)
        {
            await _procEquipmentGroupParamRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            //查询是否不是新建
            var entitys = await _procEquipmentGroupParamRepository.GetByIdsAsync(ids);
            if (entitys.Any() && entitys.Any(x => x.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18714));
            }

            return await _procEquipmentGroupParamRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procEquipmentGroupParamPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamViewDto>> GetPagedListAsync(ProcEquipmentGroupParamPagedQueryDto procEquipmentGroupParamPagedQueryDto)
        {
            var procEquipmentGroupParamPagedQuery = procEquipmentGroupParamPagedQueryDto.ToQuery<ProcEquipmentGroupParamPagedQuery>();
            procEquipmentGroupParamPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procEquipmentGroupParamRepository.GetPagedInfoAsync(procEquipmentGroupParamPagedQuery);

            List<ProcEquipmentGroupParamViewDto> procEquipmentGroupParamViewDtos = new List<ProcEquipmentGroupParamViewDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                //查询相关的信息
                var procEquipmentGroups = await _procProcessEquipmentGroupRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.EquipmentGroupId).ToArray());

                foreach (var item in pagedInfo.Data)
                {
                    var procEquipmentGroupParamViewDto = item.ToModel<ProcEquipmentGroupParamViewDto>();
                    procEquipmentGroupParamViewDto.MaterialNameVersion = item.MaterialName + "/" + item.MaterialVersion;
                    procEquipmentGroupParamViewDto.EquipmentGroupCode = procEquipmentGroups.FirstOrDefault(x => x.Id == item.EquipmentGroupId)?.Code ?? "";
                    procEquipmentGroupParamViewDto.EquipmentGroupName = procEquipmentGroups.FirstOrDefault(x => x.Id == item.EquipmentGroupId)?.Name ?? "";

                    procEquipmentGroupParamViewDtos.Add(procEquipmentGroupParamViewDto);

                }
            }

            return new PagedInfo<ProcEquipmentGroupParamViewDto>(procEquipmentGroupParamViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcEquipmentGroupParamDto> PrepareProcEquipmentGroupParamDtos(PagedInfo<ProcEquipmentGroupParamEntity> pagedInfo)
        {
            var procEquipmentGroupParamDtos = new List<ProcEquipmentGroupParamDto>();
            foreach (var procEquipmentGroupParamEntity in pagedInfo.Data)
            {
                var procEquipmentGroupParamDto = procEquipmentGroupParamEntity.ToModel<ProcEquipmentGroupParamDto>();
                procEquipmentGroupParamDtos.Add(procEquipmentGroupParamDto);
            }

            return procEquipmentGroupParamDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamViewDto> QueryProcEquipmentGroupParamByIdAsync(long id)
        {
            var procEquipmentGroupParamEntity = await _procEquipmentGroupParamRepository.GetByIdAsync(id);
            if (procEquipmentGroupParamEntity != null)
            {
                var dto = procEquipmentGroupParamEntity.ToModel<ProcEquipmentGroupParamViewDto>();

                //查询相关的信息
                var product = await _procMaterialRepository.GetByIdAsync(procEquipmentGroupParamEntity.ProductId);
                var procedure = await _procProcedureRepository.GetByIdAsync(procEquipmentGroupParamEntity.ProcedureId);
                var procEquipmentGroup = await _procProcessEquipmentGroupRepository.GetByIdAsync(procEquipmentGroupParamEntity.EquipmentGroupId);

                dto.MaterialCode = product?.MaterialCode ?? "";
                dto.MaterialName = product?.MaterialName ?? "";
                dto.MaterialNameVersion = product?.MaterialName + "/" + product?.Version ?? "";
                dto.ProcedureCode = procedure?.Code ?? "";
                dto.ProcedureName = procedure?.Name ?? "";
                dto.EquipmentGroupCode = procEquipmentGroup?.Code ?? "";
                dto.EquipmentGroupName = procEquipmentGroup?.Name ?? "";

                return dto;
            }
            throw new CustomerValidationException(nameof(ErrorCode.MES18701));
        }

        /// <summary>
        /// 获取载具类型验证根据vehicleTypeId查询
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailDto>> QueryProcEquipmentGroupParamDetailByRecipeIdAsync(long recipeId)
        {
            var procEquipmentGroupParamDetailEntitys = await _procEquipmentGroupParamDetailRepository.GetEntitiesByRecipeIdsAsync(new long[] { recipeId });

            var procEquipmentGroupParamDetailDtos = new List<ProcEquipmentGroupParamDetailDto>();
            #region 处理数据
            if (procEquipmentGroupParamDetailEntitys.Any())
            {
                //批量查询
                var paramEntitys = await _procParameterRepository.GetByIdsAsync(procEquipmentGroupParamDetailEntitys.Select(x => x.ParamId).Distinct().ToArray());

                foreach (var item in procEquipmentGroupParamDetailEntitys)
                {
                    var procEquipmentGroupParamDetailDto = item.ToModel<ProcEquipmentGroupParamDetailDto>();
                    var paramEntity = paramEntitys.FirstOrDefault(x => x.Id == item.ParamId);
                    procEquipmentGroupParamDetailDto.ParameterCode = paramEntity?.ParameterCode ?? "";
                    procEquipmentGroupParamDetailDto.ParameterName = paramEntity?.ParameterName ?? "";
                    procEquipmentGroupParamDetailDto.ParameterUnit = paramEntity?.ParameterUnit ?? "";
                    procEquipmentGroupParamDetailDto.DataType = paramEntity?.DataType;

                    procEquipmentGroupParamDetailDtos.Add(procEquipmentGroupParamDetailDto);
                }
            }

            #endregion

            return procEquipmentGroupParamDetailDtos;
        }

        /// <summary>
        /// 分页查询详情的参数
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterDto>> GetDetailParamByProductIdAndProcedureIdPagedAsync(ProcEquipmentGroupParamDetailParamPagedQueryDto queryDto)
        {
            var resultZero = new PagedInfo<ProcParameterDto>(new List<ProcParameterDto>(), 1, queryDto.PageSize, 0);

            var procEquipmentGroupParams = await _procEquipmentGroupParamRepository.GetProcEquipmentGroupParamEntitiesAsync(new ProcEquipmentGroupParamQuery { SiteId = _currentSite.SiteId ?? 0, ProductId = queryDto.ProductId, ProcedureId = queryDto.ProcedureId });

            if (procEquipmentGroupParams == null || !procEquipmentGroupParams.Any()) return resultZero;

            //找到对应的detail信息
            var procEquipmentGroupParamDetails = await _procEquipmentGroupParamDetailRepository.GetEntitiesByRecipeIdsAsync(procEquipmentGroupParams.Select(x => x.Id).ToArray());

            if (procEquipmentGroupParamDetails == null || !procEquipmentGroupParamDetails.Any()) return resultZero;

            //查询参数分页
            var pagedInfo = await _procParameterRepository.GetPagedListAsync(new Data.Repositories.Process.Query.ProcParameterPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Ids = procEquipmentGroupParamDetails.Select(x => x.ParamId).ToArray(),

                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
            });

            return new PagedInfo<ProcParameterDto>(pagedInfo.Data.Select(x => x.ToModel<ProcParameterDto>()), pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        #region 内部方法
        /// <summary>
        /// 验证唯一性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task CheckUniqueAsync(ProcEquipmentGroupParamEntity entity)
        {
            // 编码唯一性验证
            var checkEntity = await _procEquipmentGroupParamRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code,
                Version = entity.Version
            });
            if (checkEntity != null && checkEntity.Id != entity.Id) throw new CustomerValidationException(nameof(ErrorCode.MES10520))
                    .WithData("Code", entity.Code)
                    .WithData("Version", entity.Version);

            var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.ProductId);
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(entity.ProcedureId);
            var equipmentGroupEntity = await _procProcessEquipmentGroupRepository.GetByIdAsync(entity.EquipmentGroupId);
            if (materialEntity == null || procedureEntity == null || equipmentGroupEntity == null) return;

            // 校验功能类型、产品、工序、工艺组是否唯一
            var checkUniqueMaterialProcedureEntities = await _procEquipmentGroupParamRepository.GetByRelatesInformationAsync(new ProcEquipmentGroupParamRelatesInformationQuery
            {
                SiteId = entity.SiteId,
                ProductId = entity.ProductId,
                ProcedureId = entity.ProcedureId,
                EquipmentGroupId = entity.EquipmentGroupId
            });
            if (checkUniqueMaterialProcedureEntities == null || !checkUniqueMaterialProcedureEntities.Any()) return;

            // 校验功能类型+产品编码+工序编码+工艺组是否唯一
            if (checkUniqueMaterialProcedureEntities.Any(a => a.Type == entity.Type
            && a.ProductId == entity.ProductId
            && a.ProcedureId == entity.ProcedureId
            && a.EquipmentGroupId == entity.EquipmentGroupId
            && a.Id != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10536))
                    .WithData("Type", entity.Type.GetDescription())
                    .WithData("ProductCode", materialEntity.MaterialCode)
                    .WithData("ProcedureCode", procedureEntity.Code)
                    .WithData("EquipmentGroupCode", equipmentGroupEntity.Code);
            }

            // 状态为启用时，校验启用状态的 功能类型+产品编码+工序编码+工艺组 唯一性
            if (entity.Status == SysDataStatusEnum.Enable && checkUniqueMaterialProcedureEntities.Any(a => a.Type == entity.Type
            && a.ProductId == entity.ProductId
            && a.ProcedureId == entity.ProcedureId
            && a.EquipmentGroupId == entity.EquipmentGroupId
            && a.Status == entity.Status
            && a.Id != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10537))
                    .WithData("Type", entity.Type.GetDescription())
                    .WithData("ProductCode", materialEntity.MaterialCode)
                    .WithData("ProcedureCode", procedureEntity.Code)
                    .WithData("EquipmentGroupCode", equipmentGroupEntity.Code);
            }
        }
        #endregion

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
            var entity = await _procEquipmentGroupParamRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18701));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procEquipmentGroupParamRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ProcEquipmentGroupParamDetailParamImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "Recipe参数导入模板");
        }

        /// <summary>
        /// 导入信息表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportInteEquipmentGroupParamAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ProcEquipmentGroupParamDetailParamImportDto>(memoryStream);

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入数据为空");
            }

            #region 验证基础数据
            var validationFailures = new List<ValidationFailure>();
            var rows = 1;
            foreach (var item in excelImportDtos)
            {
                var validationResult = await _validationImportRules!.ValidateAsync(item);
                if (!validationResult.IsValid && validationResult.Errors != null && validationResult.Errors.Any())
                {
                    foreach (var validationFailure in validationResult.Errors)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rows);
                        validationFailures.Add(validationFailure);
                    }
                }
                rows++;
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }
            #endregion

            #region 检测导入数据编码是否重复
            var repeats = new List<string>();
            var hasDuplicates = excelImportDtos.GroupBy(x => new { x.Code });
            foreach (var item in hasDuplicates)
            {
                if (item.Count() > 1)
                {
                    repeats.Add($@"[{item.Key.Code}]");
                }
            }
            if (repeats.Any())
            {
                throw new CustomerValidationException("参数集编码【{repeats}】重复").WithData("repeats", string.Join(",", repeats));
            }

            //var paramCoderepeats = new List<string>();
            //var paramCodehasDuplicates = excelImportDtos.GroupBy(x => new { x.ParamCode });
            //foreach (var item in paramCodehasDuplicates)
            //{
            //    if (item.Count() > 1)
            //    {
            //        paramCoderepeats.Add($@"[{item.Key.ParamCode}]");
            //    }
            //}
            //if (paramCoderepeats.Any())
            //{
            //    throw new CustomerValidationException("参数编码【{paramCoderepeats}】重复").WithData("paramCoderepeats", string.Join(",", paramCoderepeats));
            //}


            List<ProcEquipmentGroupParamEntity> inteCustomList = new();
            List<ProcEquipmentGroupParamDetailEntity> inteVehicleVerifiesList = new();
            #endregion

            #region  验证数据库中是否存在数据，且组装数据
            // 读取参数编码信息
            var procParameterEntities = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.ParamCode ?? "").Distinct().ToArray()
            });

            // 读取参数集编码信息
            var procEquipmentGroupParamEntities = await _procEquipmentGroupParamRepository.GetByCodesAsync(new ProcEquipmentGroupParamByCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.Code).Distinct().ToArray()
            });
            

            //查询工序信息
            var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.ProcedureCode).Distinct().ToArray()
            });

            //查询工艺设备组
            var equipmentGroupEntity = await _procProcessEquipmentGroupRepository.GetByCodesAsync(new ProcProcessEquipmentGroupByCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.EquipmentGroupCode).Distinct().ToArray()
            });
            //查询产品信息
            var materialQuery = new ProcMaterialQuery() { SiteId = _currentSite.SiteId ?? 0, MaterialCodes = excelImportDtos.Where(x => !string.IsNullOrEmpty(x.ProductCode)).Select(x => x.ProductCode).Distinct().ToArray() };
            var procMaterials = await _procMaterialRepository.GetProcMaterialEntitiesAsync(materialQuery);

            var procEquipmentGroupParams = await _procEquipmentGroupParamRepository.GetByProductIdsAndProcedureIdsAsync(new ProcEquipmentGroupParamByIdsQuery {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureIds = procedureEntities.Select(x => x.Id).Distinct().ToArray(),
                EquipmentGroupIds = equipmentGroupEntity.Select(x => x.Id).Distinct().ToArray(),
                ProductIds = procMaterials.Select(x => x.Id).Distinct().ToArray()
            });

            var currentRow = 0;
            var customCode = procEquipmentGroupParamEntities.Select(x => x.Code).Distinct().ToList();
            foreach (var item in excelImportDtos)
            {
                currentRow++;

                //if (customCode.Contains(item.Code))
                //{
                //    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10521), item.Code, currentRow, "Code"));
                //}
                var isCodeAndVersion = procEquipmentGroupParamEntities.Where(x => x.Code == item.Code && x.Version == item.Version);
                if (isCodeAndVersion.Any())
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10520), item.Code, currentRow, "Code", "Version"));
                }
               
                var procMaterial = procMaterials.Where(x => x.MaterialCode == item.ProductCode).FirstOrDefault();
                if (procMaterial == null && item.ProductCode != null)
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES19118), item.ProductCode, currentRow, "Code"));
                }

                var procProcedure = procedureEntities.Where(x => x.Code == item.ProcedureCode).FirstOrDefault();
                if (procProcedure == null && item.ProcedureCode != null)
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10538), item.ProcedureCode, currentRow, "ProcedureCode"));
                }

                var procParameterEntity = procParameterEntities.Where(x => x.ParameterCode == item.ParamCode).FirstOrDefault();
                if (procParameterEntity == null && item.ParamCode != null)
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES19108), item.ParamCode, currentRow, "Code"));
                }

                var procEquipment = equipmentGroupEntity.Where(x => x.Code == item.EquipmentGroupCode).FirstOrDefault();
                if (procEquipment == null && item.EquipmentGroupCode != null)
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10539), item.EquipmentGroupCode, currentRow, "Code"));
                }

                var isProductAndprocProcedure = procEquipmentGroupParams.Where(x => x.Type == item.Type && x.ProcedureId == procProcedure?.Id && x.ProductId == procMaterial?.Id && x.EquipmentGroupId == procEquipment?.Id);
                if (isProductAndprocProcedure.Any())
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10536), item.Code, currentRow, "Code", "Version"));
                }
                //如果载具编码不存在，则组装数据
                if (!customCode.Contains(item.Code))
                {
                    var procEquipmentGroupParam = new ProcEquipmentGroupParamEntity()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        Status = SysDataStatusEnum.Build,
                        ProcedureId = procProcedure?.Id ?? 0,
                        ProductId = procMaterial?.Id ?? 0,
                        Type = item.Type,
                        Version = item.Version,
                        EquipmentGroupId = procEquipment?.Id ?? 0,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    // 验证是否编码唯一
                    await CheckUniqueAsync(procEquipmentGroupParam);
                    inteCustomList.Add(procEquipmentGroupParam);
                    #region 处理 载具验证数据
                    if (item.ParamCode != null)
                    {
                        var inteVehicleVerify = new ProcEquipmentGroupParamDetailEntity
                        {
                            RecipeId = procEquipmentGroupParam.Id,
                            ParamId = procParameterEntity?.Id ?? 0,
                            CenterValue = item.CenterValue,
                            MaxValue = item.MaxValue,
                            MinValue = item.MinValue,
                            DecimalPlaces = item.DecimalPlaces,
                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedBy = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                            SiteId = _currentSite.SiteId ?? 0
                        };
                        inteVehicleVerifiesList.Add(inteVehicleVerify);
                    }
                    #endregion
                }

            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //保存记录 
                if (inteCustomList.Any())
                    await _procEquipmentGroupParamRepository.InsertsAsync(inteCustomList);

                if (inteVehicleVerifiesList.Any())
                    await _procEquipmentGroupParamDetailRepository.InsertsAsync(inteVehicleVerifiesList);
                ts.Complete();
            }
            #endregion
        }
        /// <summary>
        /// 获取验证对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="codeFormattedMessage"></param>
        /// <param name="cuurrentRow"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { key, codeFormattedMessage }
            };
            return validationFailure;
        }

        /// <summary>
        /// 获取验证对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="codeFormattedMessage"></param>
        /// <param name="cuurrentRow"></param>
        /// <param name="key"></param>
        /// <param name="keys"></param>
        /// 
        /// <returns></returns>
        private ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code", string keys = "codes")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { key, codeFormattedMessage },
                {keys,codeFormattedMessage }
            };
            return validationFailure;
        }
    }
}
