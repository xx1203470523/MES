/*
 *creator: Karl
 *
 *describe: 供应商    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
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
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 供应商 服务
    /// </summary>
    public class WhSupplierService : IWhSupplierService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 供应商 仓储
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly IProcMaterialSupplierRelationRepository _procMaterialSupplierRelationRepository;
        private readonly AbstractValidator<WhSupplierCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhSupplierModifyDto> _validationModifyRules;
        private readonly AbstractValidator<WhSupplierImportDto> _validationImportRules;
        private readonly ICurrentSite _currentSite;

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        private readonly ILocalizationService _localizationService;


        public WhSupplierService(ICurrentUser currentUser,
            IWhSupplierRepository whSupplierRepository,
            AbstractValidator<WhSupplierCreateDto> validationCreateRules,
            AbstractValidator<WhSupplierModifyDto> validationModifyRules,
            ICurrentSite currentSite,
            IProcMaterialSupplierRelationRepository procMaterialSupplierRelationRepository,
            IExcelService excelService,
            IMinioService minioService,
            AbstractValidator<WhSupplierImportDto> validationImportRules,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _whSupplierRepository = whSupplierRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
            _procMaterialSupplierRelationRepository = procMaterialSupplierRelationRepository;
            _excelService = excelService;
            _minioService = minioService;
            _validationImportRules = validationImportRules;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whSupplierCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhSupplierAsync(WhSupplierCreateDto whSupplierCreateDto)
        {
            whSupplierCreateDto.Code = whSupplierCreateDto.Code.Replace(" ", "");
            whSupplierCreateDto.Name = whSupplierCreateDto.Name.Replace(" ", "");
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whSupplierCreateDto);



            //英文和数字
            Regex reg = new Regex(@"^[A-Za-z0-9]+$");
            if (!reg.Match(whSupplierCreateDto.Code).Success)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15008)).WithData("Code", whSupplierCreateDto.Code);
            }
            whSupplierCreateDto.Code = whSupplierCreateDto.Code.ToUpper();
            //判断编号是否已经存在
            var exists = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Code = whSupplierCreateDto.Code
            });
            if (exists != null && exists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15002)).WithData("Code", whSupplierCreateDto.Code);
            }

            //DTO转换实体
            var whSupplierEntity = whSupplierCreateDto.ToEntity<WhSupplierEntity>();
            whSupplierEntity.Id = IdGenProvider.Instance.CreateId();
            whSupplierEntity.CreatedBy = _currentUser.UserName;
            whSupplierEntity.UpdatedBy = _currentUser.UserName;
            whSupplierEntity.CreatedOn = HymsonClock.Now();
            whSupplierEntity.UpdatedOn = HymsonClock.Now();
            whSupplierEntity.SiteId = _currentSite.SiteId ?? 0;


            //入库
            await _whSupplierRepository.InsertAsync(whSupplierEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhSupplierAsync(long id)
        {
            _ = VerifySupplier(new long[] { id });
            await _whSupplierRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhSupplierAsync(long[] ids)
        {
            await VerifySupplier(ids);
            return await _whSupplierRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }
        /// <summary>
        /// 验证
        /// </summary>
        private async Task VerifySupplier(long[] ids)
        {
            var data = await _procMaterialSupplierRelationRepository.GetBySupplierIdsAsync(ids);
            if (data != null && data.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15011));
            }
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whSupplierPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhSupplierDto>> GetPageListAsync(WhSupplierPagedQueryDto whSupplierPagedQueryDto)
        {
            var whSupplierPagedQuery = whSupplierPagedQueryDto.ToQuery<WhSupplierPagedQuery>();
            whSupplierPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whSupplierRepository.GetPagedInfoAsync(whSupplierPagedQuery);

            //实体到DTO转换 装载数据
            List<WhSupplierDto> whSupplierDtos = PrepareWhSupplierDtos(pagedInfo);
            return new PagedInfo<WhSupplierDto>(whSupplierDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhSupplierDto> PrepareWhSupplierDtos(PagedInfo<WhSupplierEntity> pagedInfo)
        {
            var whSupplierDtos = new List<WhSupplierDto>();
            foreach (var whSupplierEntity in pagedInfo.Data)
            {
                var whSupplierDto = whSupplierEntity.ToModel<WhSupplierDto>();
                whSupplierDtos.Add(whSupplierDto);
            }

            return whSupplierDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whSupplierModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyWhSupplierAsync(WhSupplierModifyDto whSupplierModifyDto)
        {
            whSupplierModifyDto.Code = whSupplierModifyDto.Code.Replace(" ", "");
            whSupplierModifyDto.Name = whSupplierModifyDto.Name.Replace(" ", "");
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whSupplierModifyDto);

            //DTO转换实体
            var whSupplierEntity = whSupplierModifyDto.ToEntity<WhSupplierEntity>();
            whSupplierEntity.UpdatedBy = _currentUser.UserName;
            whSupplierEntity.UpdatedOn = HymsonClock.Now();


            await _whSupplierRepository.UpdateAsync(whSupplierEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhSupplierDto> QueryWhSupplierByIdAsync(long id)
        {
            var whSupplierEntity = await _whSupplierRepository.GetByIdAsync(id);
            if (whSupplierEntity != null)
            {
                return new WhSupplierDto
                {
                    Id = whSupplierEntity.Id,
                    Code = whSupplierEntity.Code,
                    Name = whSupplierEntity.Name,
                    Remark = whSupplierEntity.Remark,
                    CreatedBy = whSupplierEntity.CreatedBy,
                    CreatedOn = whSupplierEntity.CreatedOn,
                    UpdatedBy = whSupplierEntity.UpdatedBy,
                    UpdatedOn = whSupplierEntity.UpdatedOn
                };
            }
            return new WhSupplierDto();
        }


        /// <summary>
        /// 根据ID查询(更改供应商)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UpdateWhSupplierDto> QueryUpdateWhSupplierByIdAsync(long id)
        {
            var whSupplierEntity = await _whSupplierRepository.GetByIdAsync(id);
            if (whSupplierEntity != null)
            {
                return new UpdateWhSupplierDto { Id = whSupplierEntity.Id, Code = whSupplierEntity.Code, Name = whSupplierEntity.Name, Remark = whSupplierEntity.Remark };
            }
            return new UpdateWhSupplierDto();
        }

        /// <summary>
        /// 导入供应商表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportWhSupplierAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<WhSupplierImportDto>(memoryStream);

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入的参数数据为空");
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

            //检测导入数据是否重复
            var repeats = new List<string>();
            //检验重复  目前只设定检验 参数编码 是否重复
            var hasDuplicates = excelImportDtos.GroupBy(x => new { x.Code });
            foreach (var item in hasDuplicates)
            {
                if (item.Count() > 1)
                {
                    repeats.Add(item.Key.Code);
                }
            }
            if (repeats.Any())
            {
                throw new CustomerValidationException("{repeats}相关的编码重复").WithData("repeats", string.Join(",", repeats));
            }

            List<WhSupplierEntity> addEntities = new List<WhSupplierEntity>();

            #region 验证数据到数据库比对   且组装数据
            var paramters = await _whSupplierRepository.GetByCodesAsync(new WhSuppliersByCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = excelImportDtos.Select(x => x.Code).Distinct().ToArray()
            });

            var cuurrentRow = 0;
            foreach (var item in excelImportDtos)
            {
                cuurrentRow++;
                //判断是否已经录入
                if (paramters.Any(x => x.Code == item.Code))
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES15002), item.Code, cuurrentRow, "code"));
                }
                else
                {
                    //判断编码是否重复 无重复则添加
                    if (!repeats.Contains(item.Code))
                    {
                        #region 组装数据
                        //标准参数信息 记录
                        var procParameterEntity = new WhSupplierEntity()
                        {
                            Code = item.Code,
                            Name = item.Name,
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
                await _whSupplierRepository.InsertsAsync(addEntities);
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
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<WhSupplierImportDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "供应商导入模板");
        }

        /// <summary>
        /// 根据查询条件导出供应商数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<WhSupplierExportResultDto> ExprotWhSupplierListAsync(WhSupplierPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<WhSupplierPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _whSupplierRepository.GetPagedInfoAsync(pagedQuery);

            List<WhSupplierExportDto> listDto = new List<WhSupplierExportDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WhSupplier"), _localizationService.GetResource("WhSupplier"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new WhSupplierExportResultDto
                {
                    FileName = _localizationService.GetResource("WhSupplier"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new WhSupplierExportDto()
                {
                    Code = item.Code,
                    Name = item.Name,
                    Remark = item.Remark ?? ""
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WhSupplier"), _localizationService.GetResource("WhSupplier"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new WhSupplierExportResultDto
            {
                FileName = _localizationService.GetResource("WhSupplier"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
