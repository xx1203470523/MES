using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Minio.DataModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment.EquToolingManage
{
    /// <summary>
    /// 工具管理 服务业务层
    /// </summary>
    public partial class EquToolingManageService : IEquToolingManageService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工具管理表 仓储
        /// </summary>
        private readonly IEquToolingManageRepository _equToolingManageRepository;

        /// <summary>
        /// 工具类型
        /// </summary>
        private readonly IEquToolingTypeGroupRepository _equToolingTypeGroupRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 导出
        /// </summary>
        private readonly IExcelService _excelService;

        /// <summary>
        /// 创建验证器
        /// </summary>
        private readonly AbstractValidator<AddEquToolingManageDto> _verifyValidationRules;

        /// <summary>
        /// 更新验证器
        /// </summary>
        private readonly AbstractValidator<EquToolingManageModifyDto> _verifyValidationeModifyRules;

        /// <summary>
        /// 导入验证器
        /// </summary>
        private readonly AbstractValidator<EquToolingManageExcelDto> _verifyValidationeExcelRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="equToolingManageRepository"></param>
        /// <param name="equToolingTypeGroupRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="verifyValidationRules"></param>
        /// <param name="verifyValidationeModifyRules"></param>
        /// <param name="verifyValidationeExcelRules"></param>
        public EquToolingManageService(ICurrentUser currentUser, ICurrentSite currentSite, IEquToolingManageRepository equToolingManageRepository, IEquToolingTypeGroupRepository equToolingTypeGroupRepository, ILocalizationService localizationService, IExcelService excelService, AbstractValidator<AddEquToolingManageDto> verifyValidationRules, AbstractValidator<EquToolingManageModifyDto> verifyValidationeModifyRules, AbstractValidator<EquToolingManageExcelDto> verifyValidationeExcelRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equToolingManageRepository = equToolingManageRepository;
            _equToolingTypeGroupRepository = equToolingTypeGroupRepository;
            _localizationService = localizationService;
            _excelService = excelService;
            _verifyValidationRules = verifyValidationRules;
            _verifyValidationeModifyRules = verifyValidationeModifyRules;
            _verifyValidationeExcelRules = verifyValidationeExcelRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingManageViewDto>> GetPageListAsync(EquToolingManagePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<IEquToolingManagePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equToolingManageRepository.GetPagedInfoAsync(procProcedurePagedQuery);
            //实体到DTO转换 装载数据
            List<EquToolingManageViewDto> procProcedureDtos = PreparEquToolingManageDtos(pagedInfo);
            return new PagedInfo<EquToolingManageViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquToolingManageViewDto> PreparEquToolingManageDtos(PagedInfo<EquToolingManageView> pagedInfo)
        {
            var procProcedureDtos = new List<EquToolingManageViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<EquToolingManageViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }
            return procProcedureDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolingManageViewDto> QueryEquToolingManageByIdAsync(long id)
        {
            //查询工具信息
            var equToolingManageEntity = await _equToolingManageRepository.GetByIdAsync(id);
            if (equToolingManageEntity == null)
            {
                return new EquToolingManageViewDto();
            }
            //对象映射
            EquToolingManageViewDto equToolingManageViewDto = new EquToolingManageViewDto
            {
                Id = equToolingManageEntity.Id,
                Status = equToolingManageEntity.Status,
                Code = equToolingManageEntity.Code,
                Name = equToolingManageEntity.Name,
                ToolsId = equToolingManageEntity.ToolsId,
                CalibrationCycle = equToolingManageEntity.CalibrationCycle,
                IsCalibrated = equToolingManageEntity.IsCalibrated,
                RatedLife = equToolingManageEntity.RatedLife,
                RatedLifeUnit = equToolingManageEntity.RatedLifeUnit,
                ToolsTypeCode = equToolingManageEntity.ToolsTypeCode,
                CumulativeUsedLife = equToolingManageEntity.CumulativeUsedLife,
                LastVerificationTime = equToolingManageEntity.LastVerificationTime,
                CalibrationCycleUnit = equToolingManageEntity.CalibrationCycleUnit,
                ToolsTypeName = equToolingManageEntity.ToolsTypeName,
                ResidualLife = equToolingManageEntity.RatedLife - equToolingManageEntity.CumulativeUsedLife
            };
            if (equToolingManageViewDto.IsCalibrated == YesOrNoEnum.Yes)
            {
                switch (equToolingManageViewDto.CalibrationCycleUnit)
                {
                    case ToolingTypeEnum.Day:
                        equToolingManageViewDto.NextVerificationTime = equToolingManageViewDto.LastVerificationTime?.AddDays(Convert.ToDouble(equToolingManageViewDto.CalibrationCycle ?? 0));
                        break;
                    case ToolingTypeEnum.Week:
                        equToolingManageViewDto.NextVerificationTime = equToolingManageViewDto.LastVerificationTime?.AddDays(Convert.ToDouble(equToolingManageViewDto.CalibrationCycle ?? 0) * 7);
                        break;
                    case ToolingTypeEnum.Month:
                        equToolingManageViewDto.NextVerificationTime = equToolingManageViewDto.LastVerificationTime?.AddMonths(Convert.ToInt32(equToolingManageViewDto.CalibrationCycle ?? 0));
                        break;
                }
            }
            return equToolingManageViewDto;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<long> AddEquToolingManageAsync(AddEquToolingManageDto param)
        {
            //校验
            if (param == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            await _verifyValidationRules.ValidateAndThrowAsync(param);
            //对象映射
            var equToolsEntity = param.ToEntity<EquToolsEntity>();
            equToolsEntity.Id = IdGenProvider.Instance.CreateId();
            equToolsEntity.CreatedBy = _currentUser.UserName;
            equToolsEntity.UpdatedBy = _currentUser.UserName;
            equToolsEntity.CreatedOn = HymsonClock.Now();
            equToolsEntity.UpdatedOn = HymsonClock.Now();
            equToolsEntity.SiteId = _currentSite.SiteId ?? 0;

            #region 数据库验证
            var checkEntity = await _equToolingManageRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = equToolsEntity.SiteId,
                Code = equToolsEntity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", equToolsEntity.Code);
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            int response = 0;
            // 入库
            response = await _equToolingManageRepository.InsertAsync(equToolsEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13502));
            }
            trans.Complete();
            return equToolsEntity.Id;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquToolingManageAsync(IEnumerable<long> idsAr)
        {
            if (idsAr == null || !idsAr.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _equToolingManageRepository.GetByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.Status != DisableOrEnableEnum.Disable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equToolingManageRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsAr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyEquToolingManageAsync(EquToolingManageModifyDto param)
        {
            if (param == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            await _verifyValidationeModifyRules.ValidateAndThrowAsync(param);
            // DTO转换实体
            var equToolsEntity = param.ToEntity<EquToolsEntity>();
            equToolsEntity.UpdatedBy = _currentUser.UserName;
            equToolsEntity.UpdatedOn = HymsonClock.Now();
            equToolsEntity.SiteId = _currentSite.SiteId ?? 0;

            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 入库
            rows = await _equToolingManageRepository.UpdateAsync(equToolsEntity);

            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13502));
            }

            ts.Complete();
        }

        /// <summary>
        /// 校准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task CalibrationAsync(long id)
        {
            var equToolsEntity = await _equToolingManageRepository.GetByIdAsync(id);
            equToolsEntity.UpdatedBy = _currentUser.UserName;
            equToolsEntity.UpdatedOn = HymsonClock.Now();
            equToolsEntity.SiteId = _currentSite.SiteId ?? 0;
            equToolsEntity.LastVerificationTime = HymsonClock.Now();
            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 入库
            rows = await _equToolingManageRepository.UpdateAsync(equToolsEntity);

            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13502));
            }

            ts.Complete();
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string> DownloadImportTemplateAsync(Stream stream)
        {
            var worksheetName = "工具管理";
            await _excelService.ExportAsync(Array.Empty<EquToolingManageExcelDto>(), stream, worksheetName);
            var re = new EquToolingManageExcelDto();
            return worksheetName;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        public async Task ImportAsync(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var dtos = _excelService.Import<EquToolingManageExcelDto>(memoryStream);
            if (dtos == null || !dtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10133));

            // 分组标准
            var time = HymsonClock.Now();
            var equToolingTypeGroupEntities = await _equToolingTypeGroupRepository.GetEntitiesAsync(new EquToolingTypeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = dtos.Select(x => x.ToolTypeCode)
            });

            var quToolingManageEntities = await _equToolingManageRepository.GetEntitiesAsync(new EquToolingManageQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = dtos.Select(x => x.Code)
            });

            List<EquToolsEntity> entities = new();
            var validationFailures = new List<ValidationFailure>();
            var index = 0;
            foreach (var item in dtos)
            {
                index++;
                var validationResult = await _verifyValidationeExcelRules.ValidateAsync(item);
                if (!validationResult.IsValid && validationResult.Errors != null && validationResult.Errors.Any())
                {
                    foreach (var validationFailure in validationResult.Errors)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                        validationFailures.Add(validationFailure);
                    }
                }
                var quToolingManageEntity = quToolingManageEntities.FirstOrDefault(x => x.Code == item.Code);
                if (quToolingManageEntity != null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", index}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES13504);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ToolCode", item.Code);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                var equToolingTypeGroupEntity = equToolingTypeGroupEntities.FirstOrDefault(x => x.Code == item.ToolTypeCode);
                if (equToolingTypeGroupEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", index}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", index);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES13503);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ToolTypeCode", item.ToolTypeCode);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                entities.Add(new EquToolsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    Code = item.ToolTypeCode,
                    Name = item.Name,
                    ToolsId = equToolingTypeGroupEntity.Id,
                    RatedLife = item.RatedLife,
                    LastVerificationTime = item.LastVerificationTime,
                    IsCalibrated = item.IsCalibrated,
                    CalibrationCycle = item.CalibrationCycle,
                    Status = item.Status,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = time,
                    UpdatedOn = time
                });
            }

            if (validationFailures != null && validationFailures.Any())
            {
                throw new FluentValidation.ValidationException(_localizationService.GetResource("ExcelRowError"), validationFailures);
            }

            using TransactionScope ts = TransactionHelper.GetTransactionScope();

            await _equToolingManageRepository.InsertRangeAsync(entities);

            ts.Complete();
        }
    }
}