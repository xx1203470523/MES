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
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Http;

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
        private readonly AbstractValidator<InteSFCBoxImportDto> _inteSFCBoxImportValidator;
        public InteSFCBoxService(IExcelService excelService,
            ILocalizationService localizationService,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<InteSFCBoxImportDto> validationRules,
            IInteSFCBoxRepository inteSFCBoxRepository)
        {
            _excelService = excelService;
            _localizationService = localizationService;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteSFCBoxImportValidator = validationRules;
            _inteSFCBoxRepository = inteSFCBoxRepository;
        }


        public async Task<int> ImportDataAsync(UploadSFCBoxDto uploadStockDetailDto)
        {
            IFormFile formFile = uploadStockDetailDto.File;
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
            //var rows = 1;
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
            }

            //    foreach (var stockTakeDetailExcelImportDto in stockTakeDetailExcelImportDtos)
            //{
            //    var validationResult = await _inteSFCBoxImportValidator.ValidateAsync(stockTakeDetailExcelImportDto);
            //    if (!validationResult.IsValid)
            //    {
            //        if (validationResult.Errors != null && validationResult.Errors.Any())
            //        {
            //            foreach (var validationFailure in validationResult.Errors)
            //            {
            //                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rows);
            //                validationFailures.Add(validationFailure);
            //            }
            //            break;
            //        }
            //    }
            //    rows++;
            //}

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            //组装数据
            var insert = new List<InteSFCBoxEntity>();
            foreach (var item in stockTakeDetailExcelImportDtos)
            {
                //var Entity = item.ToEntity<InteSFCBoxEntity>();

                insert.Add(new InteSFCBoxEntity
                {
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
                    SiteId = _currentSite.SiteId ?? 0,
                    IsDeleted = 0

                }); ;
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
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteSFCBoxRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteSFCBoxDto>());
            return new PagedInfo<InteSFCBoxDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        public async Task<PagedInfo<InteSFCBoxRView>> GetBoxCodeListAsync(InteSFCBoxQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<InteSFCBoxQueryRep>();
            var rep = new InteSFCBoxQueryRep()
            {
                BoxCode = pagedQueryDto.BoxCode,
                SFC = pagedQueryDto.SFC,
                SiteId = pagedQueryDto.SiteId ?? 0,
                Sorting = pagedQueryDto.Sorting,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            var pagedInfo = await _inteSFCBoxRepository.GetBoxCodeAsync(rep);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteSFCBoxRView>()); 

            return new PagedInfo<InteSFCBoxRView>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
