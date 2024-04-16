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
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Transactions;


namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 客户维护 服务
    /// </summary>
    public class InteCustomService : IInteCustomService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IExcelService _excelService;
        private readonly IInteCustomRepository _inteCustomRepository1;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 客户维护 仓储
        /// </summary>
        private readonly IInteCustomRepository _inteCustomRepository;
        private readonly AbstractValidator<InteCustomCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCustomModifyDto> _validationModifyRules;
        private readonly AbstractValidator<InteCustomImportDto> _validationImportRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="inteCustomRepository"></param>
        /// <param name="excelService"></param>
        /// <param name="inteCustomRepository1"></param>
        /// <param name="minioService"></param>
        /// <param name="localizationService"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public InteCustomService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IInteCustomRepository inteCustomRepository,
            IExcelService excelService,
            IInteCustomRepository inteCustomRepository1,
            IMinioService minioService,
            ILocalizationService localizationService,
            AbstractValidator<InteCustomImportDto> validationImportRules,
            AbstractValidator<InteCustomCreateDto> validationCreateRules,
           AbstractValidator<InteCustomModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _excelService = excelService;
            _minioService = minioService;
            _localizationService = localizationService;
            _inteCustomRepository = inteCustomRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationImportRules = validationImportRules;
            _inteCustomRepository1 = inteCustomRepository1;

        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCustomCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateInteCustomAsync(InteCustomCreateDto inteCustomCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteCustomCreateDto);

            if (inteCustomCreateDto.Telephone != null)
            {
                if (!BeAValidPhone(inteCustomCreateDto.Telephone))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18411));
                }
            }

            //DTO转换实体
            var inteCustomEntity = inteCustomCreateDto.ToEntity<InteCustomEntity>();
            inteCustomEntity.Id = IdGenProvider.Instance.CreateId();
            inteCustomEntity.CreatedBy = _currentUser.UserName;
            inteCustomEntity.UpdatedBy = _currentUser.UserName;
            inteCustomEntity.CreatedOn = HymsonClock.Now();
            inteCustomEntity.UpdatedOn = HymsonClock.Now();
            inteCustomEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity = await _inteCustomRepository.GetByCodeAsync(inteCustomEntity.Code.Trim());
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18402));
            }

            //入库
            await _inteCustomRepository.InsertAsync(inteCustomEntity);
            return inteCustomEntity.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteCustomAsync(long id)
        {
            await _inteCustomRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteCustomAsync(long[] ids)
        {
            return await _inteCustomRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteCustomPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCustomDto>> GetPagedListAsync(InteCustomPagedQueryDto inteCustomPagedQueryDto)
        {
            var inteCustomPagedQuery = inteCustomPagedQueryDto.ToQuery<InteCustomPagedQuery>();
            inteCustomPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteCustomRepository.GetPagedInfoAsync(inteCustomPagedQuery);

            //实体到DTO转换 装载数据
            List<InteCustomDto> inteCustomDtos = PrepareInteCustomDtos(pagedInfo);
            return new PagedInfo<InteCustomDto>(inteCustomDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteCustomDto> PrepareInteCustomDtos(PagedInfo<InteCustomEntity> pagedInfo)
        {
            var inteCustomDtos = new List<InteCustomDto>();
            foreach (var inteCustomEntity in pagedInfo.Data)
            {
                var inteCustomDto = inteCustomEntity.ToModel<InteCustomDto>();
                inteCustomDtos.Add(inteCustomDto);
            }

            return inteCustomDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCustomModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCustomAsync(InteCustomModifyDto inteCustomModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCustomModifyDto);

            if (inteCustomModifyDto.Telephone != null)
            {
                if (!BeAValidPhone(inteCustomModifyDto.Telephone))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18411));
                }
            }

            //DTO转换实体
            var inteCustomEntity = inteCustomModifyDto.ToEntity<InteCustomEntity>();
            inteCustomEntity.UpdatedBy = _currentUser.UserName;
            inteCustomEntity.UpdatedOn = HymsonClock.Now();

            await _inteCustomRepository.UpdateAsync(inteCustomEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCustomDto> QueryInteCustomByIdAsync(long id)
        {
            var inteCustomEntity = await _inteCustomRepository.GetByIdAsync(id);
            if (inteCustomEntity != null)
            {
                return inteCustomEntity.ToModel<InteCustomDto>();
            }
            throw new CustomerValidationException(nameof(ErrorCode.MES18401));
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<InteCustomImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "客户维护导入模板");
        }

        /// <summary>
        /// 导入客户信息表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportInteCustomAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<InteCustomImportDto>(memoryStream);

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
                throw new CustomerValidationException("客户编码{repeats}重复").WithData("repeats", string.Join(",", repeats));
            }

            List<InteCustomEntity> inteCustomList = new();
            #endregion

            #region  验证数据库中是否存在数据，且组装数据
            var customCodes = await _inteCustomRepository1.GetInteCustomEntitiesAsync(new InteCustomQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.Code).Distinct().ToArray()
            });

            var currentRow = 0;
            var customCode = customCodes.Select(x => x.Code).Distinct().ToList();
            foreach (var item in excelImportDtos)
            {
                currentRow++;

                if (customCode.Contains(item.Code))
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES18402), item.Code, currentRow, "Code"));
                }
                //如果客户编码不存在，则组装数据
                if (!customCode.Contains(item.Code))
                {
                    var inteCustomInfoEntity = new InteCustomEntity()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        Describe = item.Describe ?? "",
                        Address = item.Address ?? "",
                        Telephone = item.Telephone ?? "",

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    inteCustomList.Add(inteCustomInfoEntity);
                }
            }
            #endregion

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {

                //保存记录 
                if (inteCustomList.Any())
                    await _inteCustomRepository1.InsertsAsync(inteCustomList);
                ts.Complete();
            }

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
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InteCustomExportResultDto> ExprotInteCustomPageListAsync(InteCustomPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<InteCustomPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _inteCustomRepository.GetPagedInfoAsync(pagedQuery);

            //实体到DTO转换 装载数据
            List<InteCustomExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("CustomInfo"), _localizationService.GetResource("CustomInfo"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new InteCustomExportResultDto
                {
                    FileName = _localizationService.GetResource("CustomInfo"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new InteCustomExportDto()
                {
                    Code = item.Code ?? "",
                    Name = item.Name ?? "",
                    Describe = item.Describe ?? "",
                    Address = item.Address,
                    Telephone = item.Telephone ?? ""
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("CustomInfo"), _localizationService.GetResource("CustomInfo"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new InteCustomExportResultDto
            {
                FileName = _localizationService.GetResource("CustomInfo"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

        /// <summary>
        /// 电话校验
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private bool BeAValidPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            // 中国手机号码的正则表达式
            var mobileRegex = @"^1[3-9]\d{9}$";
            // 中国固定电话的正则表达式
            var telephoneRegex = @"^0\d{2,3}-?\d{7,8}$";

            // 检查是否匹配中国手机号码或固定电话
            return Regex.IsMatch(phoneNumber, mobileRegex) || Regex.IsMatch(phoneNumber, telephoneRegex);
        }

    }
}
