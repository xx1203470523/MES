using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture;

public partial class ManuFacePlateService : IManuFacePlateService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    private readonly IManuFacePlateRepository _manuFacePlateRepository;
    private readonly IManuFacePlateProductionRepository _manuFacePlateProductionRepository;
    private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;
    private readonly IManuFacePlateContainerPackRepository _manuFacePlateContainerPackRepository;
    private readonly IManuFacePlateButtonRepository _manuFacePlateButtonRepository;
    private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;
    private readonly IManuSfcRepository _manuSfcRepository;
    private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
    private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
    private readonly IManuContainerPackRepository _manuContainerPackRepository;
    private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;    

    private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
    private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
    private readonly IInteContainerInfoRepository _inteContainerInfoRepository;
    private readonly IInteContainerFreightRepository _inteContainerFreightRepository;
    private readonly IInteJobRepository _inteJobRepository;

    private readonly IProcMaterialRepository _procMaterialRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcResourceRepository _procResourceRepository;

    private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

    private readonly ILocalizationService _localizationService;
    private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

    private readonly AbstractValidator<ManuFacePlateCreateDto> _validationCreateRules;
    private readonly AbstractValidator<ManuFacePlateModifyDto> _validationModifyRules;

    private readonly AbstractValidator<ManuFacePlateProductionCreateDto> _validationProductionCreateRules;
    private readonly AbstractValidator<ManuFacePlateProductionModifyDto> _validationProductionModifyRules;

    private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationRepairCreateRules;
    private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationRepairModifyRules;

    private readonly AbstractValidator<ManuFacePlateContainerPackCreateDto> _validationContainerPackCreateRules;
    private readonly AbstractValidator<ManuFacePlateContainerPackModifyDto> _validationContainerPackModifyRules;

    private readonly AbstractValidator<ManuFacePlateButtonModifyDto> _validationButtonModifyRules;
    private readonly AbstractValidator<ManuFacePlateButtonCreateDto> _validationButtonCreateRules;

    private readonly AbstractValidator<ManuFacePlatePackDto> _validationsManuFacePlateConfirmByContainerCodeRules;

    public ManuFacePlateService(
        ICurrentUser currentUser,
        ICurrentSite currentSite,
        IManuFacePlateRepository manuFacePlateRepository,
        IManuFacePlateProductionRepository manuFacePlateProductionRepository,
        IManuFacePlateRepairRepository manuFacePlateRepairRepository,
        IManuFacePlateContainerPackRepository manuFacePlateContainerPackRepository,
        IManuFacePlateButtonRepository manuFacePlateButtonRepository,
        IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository,
        IManuSfcRepository manuSfcRepository,
        IManuSfcInfoRepository manuSfcInfoRepository,
        IManuContainerBarcodeRepository manuContainerBarcodeRepository,
        IManuContainerPackRepository manuContainerPackRepository,
        IManuContainerPackRecordRepository manuContainerPackRecordRepository,
        IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
        IInteContainerInfoRepository inteContainerInfoRepository,
        IInteContainerFreightRepository inteContainerFreightRepository,
        IInteJobRepository inteJobRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository procResourceRepository,
        IPlanWorkOrderRepository planWorkOrderRepository,
        ILocalizationService localizationService,
        AbstractValidator<ManuFacePlateCreateDto> validationCreateRules,
        AbstractValidator<ManuFacePlateModifyDto> validationModifyRules,
        AbstractValidator<ManuFacePlateProductionCreateDto> validationProductionCreateRules,
        AbstractValidator<ManuFacePlateProductionModifyDto> validationProductionModifyRules,
        AbstractValidator<ManuFacePlateRepairCreateDto> validationRepairCreateRules,
        AbstractValidator<ManuFacePlateRepairModifyDto> validationRepairModifyRules,
        AbstractValidator<ManuFacePlateContainerPackCreateDto> validationContainerPackCreateRules,
        AbstractValidator<ManuFacePlateContainerPackModifyDto> validationContainerPackModifyRules,
        AbstractValidator<ManuFacePlateButtonModifyDto> validationButtonModifyRules,
        AbstractValidator<ManuFacePlateButtonCreateDto> validationButtonCreateRules,
        AbstractValidator<ManuFacePlatePackDto> validationsManuFacePlateConfirmByContainerCodeRules,
        IManuGenerateBarcodeService manuGenerateBarcodeService,
        IInteCodeRulesRepository inteCodeRulesRepository)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;
        _manuFacePlateRepository = manuFacePlateRepository;
        _manuFacePlateProductionRepository = manuFacePlateProductionRepository;
        _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
        _manuFacePlateContainerPackRepository = manuFacePlateContainerPackRepository;
        _manuFacePlateButtonRepository = manuFacePlateButtonRepository;
        _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
        _manuSfcRepository = manuSfcRepository;
        _manuSfcInfoRepository = manuSfcInfoRepository;
        _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
        _manuContainerPackRepository = manuContainerPackRepository;
        _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
        _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
        _inteContainerInfoRepository = inteContainerInfoRepository;
        _inteContainerFreightRepository = inteContainerFreightRepository;
        _inteJobRepository = inteJobRepository;
        _procMaterialRepository = procMaterialRepository;
        _procProcedureRepository = procProcedureRepository;
        _procResourceRepository = procResourceRepository;
        _planWorkOrderRepository = planWorkOrderRepository;
        _localizationService = localizationService;
        _validationCreateRules = validationCreateRules;
        _validationModifyRules = validationModifyRules;
        _validationProductionCreateRules = validationProductionCreateRules;
        _validationProductionModifyRules = validationProductionModifyRules;
        _validationRepairCreateRules = validationRepairCreateRules;
        _validationRepairModifyRules = validationRepairModifyRules;
        _validationContainerPackCreateRules = validationContainerPackCreateRules;
        _validationContainerPackModifyRules = validationContainerPackModifyRules;
        _validationButtonModifyRules = validationButtonModifyRules;
        _validationButtonCreateRules = validationButtonCreateRules;
        _validationsManuFacePlateConfirmByContainerCodeRules = validationsManuFacePlateConfirmByContainerCodeRules;
        _manuGenerateBarcodeService = manuGenerateBarcodeService;
        _inteCodeRulesRepository = inteCodeRulesRepository;
    }

    /// <summary>
    /// 批量查询Job编码
    /// </summary>
    /// <param name="scanJobIdStr"></param>
    /// <returns>返回使用,逗号分割的Code</returns>
    private async Task<string> QueryInteJobCodesAsync(string scanJobIdStr)
    {
        string jobCodeStr = string.Empty;
        if (!string.IsNullOrEmpty(scanJobIdStr))
        {
            var scanJobIdStrArry = scanJobIdStr.Split(',');
            List<long> longs = new List<long>();
            foreach (var jobidStr in scanJobIdStrArry)
            {
                //忽略转换失败的Id
                if (long.TryParse(jobidStr, out long jobid))
                {
                    longs.Add(jobid);
                }
            }
            var inteJobEntity = await _inteJobRepository.GetByIdsAsync(longs.ToArray());
            if (inteJobEntity != null)
            {
                jobCodeStr = string.Join(",", inteJobEntity.Select(c => c.Code));
            }
        }
        return jobCodeStr;
    }

    /// <summary>
    /// 批量将ManuFacePlateEntity转换为ManuFacePlateDto
    /// </summary>
    /// <param name="pagedInfo"></param>
    /// <returns></returns>
    private static List<ManuFacePlateDto> PrepareManuFacePlateDtos(PagedInfo<ManuFacePlateEntity> pagedInfo)
    {
        var manuFacePlateDtos = new List<ManuFacePlateDto>();
        foreach (var manuFacePlateEntity in pagedInfo.Data)
        {
            var manuFacePlateDto = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
            manuFacePlateDtos.Add(manuFacePlateDto);
        }

        return manuFacePlateDtos;
    }
}
