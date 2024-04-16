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
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Common;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 标准参数表 服务
    /// </summary>
    public class ProcParameterService : IProcParameterService
    {
        /// <summary>
        /// 标准参数表 仓储
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly AbstractValidator<ProcParameterCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcParameterModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ProcParameterImportDto> _validationImportRules;

        private readonly ILocalizationService _localizationService;

        private readonly IProcParameterLinkTypeRepository _procParameterLinkTypeRepository;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procParameterLinkTypeRepository"></param>
        /// <param name="currentSite"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="localizationService"></param>
        public ProcParameterService(ICurrentUser currentUser,
            IProcParameterRepository procParameterRepository,
            AbstractValidator<ProcParameterCreateDto> validationCreateRules,
            AbstractValidator<ProcParameterModifyDto> validationModifyRules,
            IProcParameterLinkTypeRepository procParameterLinkTypeRepository,
            ICurrentSite currentSite,
            IExcelService excelService,
            IMinioService minioService,
            AbstractValidator<ProcParameterImportDto> validationImportRules,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _procParameterRepository = procParameterRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procParameterLinkTypeRepository = procParameterLinkTypeRepository;
            _currentSite = currentSite;
            _excelService = excelService;
            _minioService = minioService;
            _validationImportRules = validationImportRules;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procParameterCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcParameterAsync(ProcParameterCreateDto procParameterCreateDto)
        {
            if (procParameterCreateDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            procParameterCreateDto.ParameterCode = procParameterCreateDto.ParameterCode.ToTrimSpace().ToUpperInvariant();
            procParameterCreateDto.ParameterName = procParameterCreateDto.ParameterName.Trim();
            procParameterCreateDto.Remark = procParameterCreateDto.Remark ?? "".Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procParameterCreateDto);

            //DTO转换实体
            var procParameterEntity = procParameterCreateDto.ToEntity<ProcParameterEntity>();
            procParameterEntity.Id = IdGenProvider.Instance.CreateId();
            procParameterEntity.CreatedBy = _currentUser.UserName;
            procParameterEntity.UpdatedBy = _currentUser.UserName;
            procParameterEntity.CreatedOn = HymsonClock.Now();
            procParameterEntity.UpdatedOn = HymsonClock.Now();
            procParameterEntity.SiteId = _currentSite.SiteId;

            //判断编号是否已经存在
            var exists = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery()
            {
                SiteId = procParameterEntity.SiteId,
                ParameterCode = procParameterEntity.ParameterCode,
            });
            if (exists != null && exists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10502)).WithData("parameterCode", procParameterEntity.ParameterCode);
            }
            try
            {
                //入库
                await _procParameterRepository.InsertAsync(procParameterEntity);
            }
            catch (Exception ex) { throw new CustomerValidationException(ex.ToString()); }

            return procParameterEntity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procParameterModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcParameterAsync(ProcParameterModifyDto procParameterModifyDto)
        {
            procParameterModifyDto.Remark = procParameterModifyDto.Remark ?? "".Trim();

            //DTO转换实体
            var procParameterEntity = procParameterModifyDto.ToEntity<ProcParameterEntity>();
            procParameterEntity.UpdatedBy = _currentUser.UserName;
            procParameterEntity.UpdatedOn = HymsonClock.Now();
            procParameterEntity.SiteId = _currentSite.SiteId;

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procParameterModifyDto);

            var modelOrigin = await _procParameterRepository.GetByIdAsync(procParameterEntity.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10504));

            await _procParameterRepository.UpdateAsync(procParameterEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcParameterAsync(long id)
        {
            await _procParameterRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcParameterAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10505));
            }

            //查询参数是否关联产品参数和设备参数
            var lists = await _procParameterLinkTypeRepository.GetByParameterIdsAsync(idsArr);
            if (lists != null && lists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10506));
            }

            return await _procParameterRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procParameterPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterDto>> GetPageListAsync(ProcParameterPagedQueryDto procParameterPagedQueryDto)
        {
            var procParameterPagedQuery = procParameterPagedQueryDto.ToQuery<ProcParameterPagedQuery>();
            procParameterPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procParameterRepository.GetPagedListAsync(procParameterPagedQuery);

            // 实体到DTO转换 装载数据
            List<ProcParameterDto> procParameterDtos = PrepareProcParameterDtos(pagedInfo);
            return new PagedInfo<ProcParameterDto>(procParameterDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcParameterDto> PrepareProcParameterDtos(PagedInfo<ProcParameterEntity> pagedInfo)
        {
            var procParameterDtos = new List<ProcParameterDto>();
            foreach (var procParameterEntity in pagedInfo.Data)
            {
                var procParameterDto = procParameterEntity.ToModel<ProcParameterDto>();
                procParameterDtos.Add(procParameterDto);
            }

            return procParameterDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterDto> QueryProcParameterByIdAsync(long id)
        {
            var siteId = _currentSite.SiteId;

            var procParameterEntity = await _procParameterRepository.GetByIdAsync(id);
            if (procParameterEntity != null)
            {
                var dto = procParameterEntity.ToModel<CustomProcParameterDto>();
                var linkTypes = await _procParameterLinkTypeRepository.GetProcParameterLinkTypeEntitiesAsync(new ProcParameterLinkTypeQuery()
                {
                    SiteId = siteId,
                    ParameterID = dto.Id
                });
                dto.Type = linkTypes.GroupBy(x => x.ParameterType).Select(x => x.Key).ToArray();

                return dto;
            }
            return new ProcParameterDto();
        }

        /// <summary>
        /// 导入参数表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportParameterAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ProcParameterImportDto>(memoryStream);

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入的参数数据为空");
            }

            ExcelCheck excelCheck = new ExcelCheck();
            // 读取Excel第一行的值
            var firstRowValues = await excelCheck.ReadFirstRowAsync(formFile);
            // 获取Excel模板的值
            var columnHeaders = excelCheck.GetColumnHeaders<ProcParameterImportDto>();
            // 校验
            if (firstRowValues != columnHeaders)
            {
                throw new CustomerValidationException("批量导入时使用错误模板提示请安模板导入数据");
            }

            #region 验证基础数据
            var validationFailures = new List<ValidationFailure>();
            var rows = 1;
            foreach (var item in excelImportDtos)
            {
                var validationResult = await _validationImportRules!.ValidateAsync(item);
                if (!validationResult.IsValid && validationResult.Errors.Any())
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

            //检测导入数据是否重复
            var repeats = new List<string>();
            //检验重复  目前只设定检验 参数编码 是否重复
            var hasDuplicates = excelImportDtos.GroupBy(x => new { x.ParameterCode });
            foreach (var item in hasDuplicates)
            {
                if (item.Count() > 1)
                {
                    repeats.Add(item.Key.ParameterCode);
                }
            }
            if (repeats.Any())
            {
                throw new CustomerValidationException("{repeats}相关的编码重复").WithData("repeats", string.Join(",", repeats));
            }

            List<ProcParameterEntity> addEntities = new List<ProcParameterEntity>();

            #region 验证数据到数据库比对   且组装数据
            var paramters = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.ParameterCode).Distinct().ToArray()
            });

            var cuurrentRow = 0;
            foreach (var item in excelImportDtos)
            {
                cuurrentRow++;
                //判断是否已经录入
                if (paramters.Any(x => x.ParameterCode == item.ParameterCode))
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10502), item.ParameterCode, cuurrentRow, "parameterCode"));
                }
                else
                {
                    //判断编码是否重复 无重复则添加
                    if (!repeats.Contains(item.ParameterCode))
                    {
                        #region 组装数据
                        //标准参数信息 记录
                        var procParameterEntity = new ProcParameterEntity()
                        {
                            ParameterCode = item.ParameterCode,
                            ParameterName = item.ParameterName,
                            ParameterUnit = item.ParameterUnit,
                            DataType = item.DataType,
                            Remark = item.Remark ?? "",

                            Id = IdGenProvider.Instance.CreateId(),
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                            SiteId = _currentSite.SiteId ?? 0
                        };
                        addEntities.Add(procParameterEntity);
                        #endregion
                    }
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            #endregion

            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            //插入数据
            if (addEntities.Any())
                await _procParameterRepository.InsertsAsync(addEntities);
            ts.Complete();

        }

        /// <summary>
        /// 获取验证对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="codeFormattedMessage"></param>
        /// <param name="cuurrentRow"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code")
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
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ProcParameterImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "参数导入模板");
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ParameterExportResultDto> ExprotParameterListAsync(ProcParameterPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ProcParameterPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _procParameterRepository.GetPagedListAsync(pagedQuery);

            List<ProcParameterExportDto> listDto = new List<ProcParameterExportDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("Parameter"), _localizationService.GetResource("Parameter"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ParameterExportResultDto
                {
                    FileName = _localizationService.GetResource("Parameter"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ProcParameterExportDto()
                {
                    ParameterCode = item.ParameterCode ?? "",
                    ParameterName = item.ParameterName ?? "",
                    ParameterUnit = item.ParameterUnit ?? "",
                    DataType = Enum.IsDefined(typeof(DataTypeEnum), item.DataType) ? item.DataType.GetDescription() : "",
                    Remark = item.Remark ?? ""
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("Parameter"), _localizationService.GetResource("Parameter"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ParameterExportResultDto
            {
                FileName = _localizationService.GetResource("Parameter"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
