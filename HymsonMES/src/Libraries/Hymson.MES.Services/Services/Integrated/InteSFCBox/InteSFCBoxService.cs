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
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using IdGen;
using Microsoft.AspNetCore.Http;
using System.Drawing.Printing;
using ValidationException = FluentValidation.ValidationException;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Hymson.MES.Services.Services.Integrated.InteSFCBox
{
    public class InteSFCBoxService : IInteSFCBoxService
    {
        //private readonly ILogService _logService;
        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IInteSFCBoxRepository _inteSFCBoxRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<InteSFCBoxImportDto> _inteSFCBoxImportValidator;
        public InteSFCBoxService(IExcelService excelService,
            ILocalizationService localizationService,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<InteSFCBoxImportDto> validationRules,
            IInteSFCBoxRepository inteSFCBoxRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _excelService = excelService;
            _localizationService = localizationService;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteSFCBoxImportValidator = validationRules;
            _inteSFCBoxRepository = inteSFCBoxRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 箱码验证
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        public async Task<InteSFCBoxValidateResponse> SFCValidate(InteSFCBoxValidateQuery validate)
        {
            if (string.IsNullOrWhiteSpace(validate.BoxCode)) throw new CustomerValidationException(nameof(ErrorCode.MES19306)).WithData("Code", "箱码");
            if (string.IsNullOrWhiteSpace(validate.WorkOrderCode)) throw new CustomerValidationException(nameof(ErrorCode.MES19306)).WithData("Code", "工单");

            var query = new PlanWorkOrderQuery
            {
                OrderCode = validate.WorkOrderCode,
                SiteId = _currentSite.SiteId ?? 123456
            };
            var workOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16003));

            var rsp = new InteSFCBoxValidateResponse() { State = -1, Msg = $"验证失败！箱码{validate.BoxCode}不在工单{validate.WorkOrderCode}中" };
            var bindsfcBox = await _inteSFCBoxRepository.GetByWorkOrderAsync(workOrderEntity.Id);

            var sfcBoxQuery = new InteSFCBoxEntityQuery
            {
                BoxCode = validate.BoxCode
            };
            var sfcBoxinfo = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcBoxQuery);

            var infoFirst = sfcBoxinfo.FirstOrDefault();
            if (infoFirst == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19308)).WithData("Code", "箱码");
            }
            if (bindsfcBox.Any(x => x.BatchNo == infoFirst.BatchNo) == true)
            {
                rsp.State = 0;
                rsp.Msg = "验证成功";
            }
            return rsp;

        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task<int> ImportDataAsync(UploadSFCBoxDto uploadStockDetailDto)
        {
            IFormFile formFile = uploadStockDetailDto.File;

            string namestr = uploadStockDetailDto.File.FileName;
            int filenameindex = namestr.IndexOf('.');
            var batchNo = string.Empty;
            if (filenameindex >= 0)
            {
                batchNo = namestr.Substring(0, filenameindex).Trim();
            }
            if (string.IsNullOrEmpty(batchNo))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16353));
            }

            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            IEnumerable<InteSFCBoxImportDto> stockTakeDetailExcelImportDtos;
            try
            {
                stockTakeDetailExcelImportDtos = _excelService.Import<InteSFCBoxImportDto>(memoryStream);
            }
            catch (Exception)
            {
                //数据导入模板不正确或数据填写异常
                throw new CustomerValidationException(nameof(ErrorCode.MES16352));
            }

            //验证导入数据
            var validationFailures = new List<ValidationFailure>();

            foreach (var (item, i) in stockTakeDetailExcelImportDtos.Select((item, i) => (item, i)))
            {
                var validationResult = await _inteSFCBoxImportValidator.ValidateAsync(item);
                if (!validationResult.IsValid)
                {
                    if (validationResult.Errors != null && validationResult.Errors.Any())
                    {
                        foreach (var validationFailure in validationResult.Errors)
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                            validationFailures.Add(validationFailure);
                        }
                        break;
                    }
                }
                //string[] boxCodes = { item.BoxCode };
                //var boxCodesAny = await _inteSFCBoxRepository.GetByBoxCodesAsync(boxCodes);
                //if (boxCodesAny.Any())
                //{
                //    var validatetion = new ValidationFailure();
                //    validatetion.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                //    validatetion.ErrorMessage = $"{item.BoxCode}已存在";
                //    validationFailures.Add(validatetion);
                //}
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            //校验电芯唯一
            string[] sfcs = stockTakeDetailExcelImportDtos.Select(s => s.SFC).ToArray();

            if (sfcs.Any())
            {
                var sfcsQuery = new InteSFCBoxEntityQuery
                {
                    SFCs = sfcs
                };

                var sfcAny = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcsQuery);

                if (sfcAny.Any())
                {
                    var sfchas = sfcAny.Select(s => s.SFC).ToArray();
                    throw new CustomerValidationException(nameof(ErrorCode.MES16354)).WithData("SFC", string.Join(",", sfchas));
                }
            }

            //校验箱码只能存在一个批次中
            string[] boxcodes = stockTakeDetailExcelImportDtos.Select(s => s.BoxCode).Distinct().ToArray();

            if (boxcodes.Any())
            {
                var sfcsQuery = new InteSFCBoxEntityQuery
                {
                    BoxCodes = boxcodes,
                    NotInBatch = batchNo
                };

                var boxAny = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcsQuery);

                if (boxAny.Any())
                {
                    var boxhas = boxAny.Select(s => s.BoxCode).Distinct().ToArray();
                    throw new CustomerValidationException(nameof(ErrorCode.MES16355)).WithData("BoxCode", string.Join(",", boxhas));
                }
            }


            //组装数据
            var insert = new List<InteSFCBoxEntity>();


            foreach (var item in stockTakeDetailExcelImportDtos)
            {
                //var Entity = item.ToEntity<InteSFCBoxEntity>();
                insert.Add(new InteSFCBoxEntity
                {
                    BatchNo = batchNo,
                    SFC = item.SFC,
                    Grade = item.Grade,
                    Status = SFCBoxEnum.Start,
                    BoxCode = item.BoxCode,
                    OCVB = item.OCVB,
                    OcvbDate = DateTime.Parse(item.OCVBDate),

                    Weight = item.Weight,
                    DC = item.DC,
                    DcDate = DateTime.Parse(item.DCDate),
                    IMPB = item.IMPB,
                    SelfDischargeRate = item.SelfDischargeRate,

                    Width = item.Width,
                    HeightF = item.HeightF,
                    HeightZ = item.HeightZ,

                    ShoulderHeightZ = item.ShoulderHeightZ,
                    ShoulderHeightF = item.ShoulderHeightF,
                    Thickness = item.Thickness,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    Localtime = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 123456,
                    IsDeleted = 0

                });
            }

            return await _inteSFCBoxRepository.InsertsAsync(insert);

        }

        /// <summary>
        /// 重复导入数据（过滤已经导入过的）
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task<int> ImportDataNoRepeatAsync(UploadSFCBoxDto uploadStockDetailDto)
        {
            IFormFile formFile = uploadStockDetailDto.File;

            string namestr = uploadStockDetailDto.File.FileName;
            int filenameindex = namestr.IndexOf('.');
            var batchNo = string.Empty;
            if (filenameindex >= 0)
            {
                batchNo = namestr.Substring(0, filenameindex).Trim();
            }
            if (string.IsNullOrEmpty(batchNo))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16353));
            }

            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            IEnumerable<InteSFCBoxImportDto> stockTakeDetailExcelImportDtos;
            try
            {
                stockTakeDetailExcelImportDtos = _excelService.Import<InteSFCBoxImportDto>(memoryStream);
            }
            catch (Exception)
            {
                //数据导入模板不正确或数据填写异常
                throw new CustomerValidationException(nameof(ErrorCode.MES16352));
            }

            //验证导入数据
            var validationFailures = new List<ValidationFailure>();

            //foreach (var (item, i) in stockTakeDetailExcelImportDtos.Select((item, i) => (item, i)))
            //{
            //    var validationResult = await _inteSFCBoxImportValidator.ValidateAsync(item);
            //    if (!validationResult.IsValid)
            //    {
            //        if (validationResult.Errors != null && validationResult.Errors.Any())
            //        {
            //            foreach (var validationFailure in validationResult.Errors)
            //            {
            //                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
            //                validationFailures.Add(validationFailure);
            //            }
            //            break;
            //        }
            //    }
            //    //string[] boxCodes = { item.BoxCode };
            //    //var boxCodesAny = await _inteSFCBoxRepository.GetByBoxCodesAsync(boxCodes);
            //    //if (boxCodesAny.Any())
            //    //{
            //    //    var validatetion = new ValidationFailure();
            //    //    validatetion.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
            //    //    validatetion.ErrorMessage = $"{item.BoxCode}已存在";
            //    //    validationFailures.Add(validatetion);
            //    //}
            //}

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            //校验电芯唯一
            string[] sfcs = stockTakeDetailExcelImportDtos.Select(s => s.SFC).ToArray();

            if (sfcs.Any())
            {
                var sfcsQuery = new InteSFCBoxEntityQuery
                {
                    SFCs = sfcs
                };

                var sfcAny = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcsQuery);

                //过滤已经存在的，继续导入
                stockTakeDetailExcelImportDtos = stockTakeDetailExcelImportDtos.Where(a=>!sfcAny.Any(b=>b.SFC.Equals(a.SFC)));

                //if (sfcAny.Any())
                //{
                //    var sfchas = sfcAny.Select(s => s.SFC).ToArray();
                //    throw new CustomerValidationException(nameof(ErrorCode.MES16354)).WithData("SFC", string.Join(",", sfchas));
                //}
            }

            //校验箱码只能存在一个批次中
            string[] boxcodes = stockTakeDetailExcelImportDtos.Select(s => s.BoxCode).Distinct().ToArray();

            //if (boxcodes.Any())
            //{
            //    var sfcsQuery = new InteSFCBoxEntityQuery
            //    {
            //        BoxCodes = boxcodes,
            //        NotInBatch = batchNo
            //    };

            //    var boxAny = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcsQuery);

            //    if (boxAny.Any())
            //    {
            //        var boxhas = boxAny.Select(s => s.BoxCode).Distinct().ToArray();
            //        throw new CustomerValidationException(nameof(ErrorCode.MES16355)).WithData("BoxCode", string.Join(",", boxhas));
            //    }
            //}


            //组装数据
            var insert = new List<InteSFCBoxEntity>();


            foreach (var item in stockTakeDetailExcelImportDtos)
            {
                //var Entity = item.ToEntity<InteSFCBoxEntity>();
                insert.Add(new InteSFCBoxEntity
                {
                    BatchNo = batchNo,
                    SFC = item.SFC,
                    Grade = item.Grade,
                    Status = SFCBoxEnum.Start,
                    BoxCode = item.BoxCode,
                    OCVB = item.OCVB,
                    OcvbDate = DateTime.Parse(item.OCVBDate),

                    Weight = item.Weight,
                    DC = item.DC,
                    DcDate = DateTime.Parse(item.DCDate),
                    IMPB = item.IMPB,
                    SelfDischargeRate = item.SelfDischargeRate,

                    Width = item.Width,
                    HeightF = item.HeightF,
                    HeightZ = item.HeightZ,

                    ShoulderHeightZ = item.ShoulderHeightZ,
                    ShoulderHeightF = item.ShoulderHeightF,
                    Thickness = item.Thickness,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    Localtime = HymsonClock.Now(),
                    SiteId = _currentSite?.SiteId ?? 123456,
                    IsDeleted = 0

                });
            }

            return await _inteSFCBoxRepository.InsertsAsync(insert);

        }

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var exportDto = new List<InteSFCBoxImportDto>();
            await _excelService.ExportAsync(exportDto, stream, "电芯明细导入模板");
        }


        public async Task<PagedInfo<InteSFCBoxDto>> GetPagedListAsync(InteSFCBoxQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteSFCBoxQueryRep>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _inteSFCBoxRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteSFCBoxDto>());
            return new PagedInfo<InteSFCBoxDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 工单新新查询弹出窗口
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteSFCBoxRView>> GetBoxCodeListAsync(InteSFCBoxQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<InteSFCBoxQueryRep>();
            var rep = new InteSFCBoxQueryRep()
            {
                BoxCode = pagedQueryDto.BoxCode,
                BatchNo = pagedQueryDto.BatchNo,
                SFC = pagedQueryDto.SFC,
                SiteId = pagedQueryDto.SiteId ?? 123456,
                Sorting = pagedQueryDto.Sorting,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            var pagedInfo = await _inteSFCBoxRepository.GetBoxCodeAsync(rep);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(x => new InteSFCBoxRView { BatchNo = x.BatchNo, CreatedOn = x.CreatedOn }).ToList()
                .Skip((pagedInfo.PageIndex-1)*pagedInfo.PageSize)
                .Take(pagedInfo.PageSize);
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<InteSFCBoxRView>());

            return new PagedInfo<InteSFCBoxRView>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            //var list = await _inteSFCBoxRepository.GetByIdsAsync(idsArr);
            //if (list != null && list.Any(x => x.Status != SysDataStatusEnum.Build))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12509));
            //}
            return await _inteSFCBoxRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

    }
}
