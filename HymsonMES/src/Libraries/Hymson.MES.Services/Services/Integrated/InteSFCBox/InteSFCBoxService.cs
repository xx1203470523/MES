﻿using FluentValidation;
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
                SiteId = _currentSite.SiteId ?? 0
            };
            var workOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16003));

            var rsp = new InteSFCBoxValidateResponse() { State = -1, Msg = $"验证失败！箱码{validate.BoxCode}不在工单{validate.WorkOrderCode}中" };
            var sfcBox = await _inteSFCBoxRepository.GetByWorkOrderAsync(workOrderEntity.Id);
            if (sfcBox.Any(x => x.BoxCode == validate.BoxCode) == true)
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
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            //组装数据
            var insert = new List<InteSFCBoxEntity>();
            var batchNo=DateTime.Now.ToString("yyMMddHHmmss");

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
