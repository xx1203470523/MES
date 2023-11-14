/*
 *creator: Karl
 *
 *describe: 分选规则    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.Query;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Microsoft.IdentityModel.Tokens;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 分选规则 更新 验证
    /// </summary>
    internal class ProcSortingRuleCreateValidator : AbstractValidator<ProcSortingRuleCreateDto>
    {
        private readonly IProcSortingRuleRepository _procSortingRuleRepository;
        private readonly ICurrentSite _currentSite;

        public ProcSortingRuleCreateValidator(IProcSortingRuleRepository procSortingRuleRepository, ICurrentSite currentSite)
        {
            _procSortingRuleRepository = procSortingRuleRepository;
            _currentSite = currentSite;
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11301));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11302));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11303));
            RuleFor(x => x.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11304));

            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES11310));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES11311));
            RuleFor(x => x.Version).MaximumLength(10).WithErrorCode(nameof(ErrorCode.MES11312));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11313));
            RuleFor(x => x.SortingParamDtos).Must(ManuSortingParamUpperAndLowerLimitValidator).WithErrorCode(nameof(ErrorCode.MES11306));

            RuleFor(x => x).MustAsync(ManuSortingyCodeAndVersionValidatorasync).WithErrorCode(nameof(ErrorCode.MES11307));
            RuleFor(x => x).MustAsync(ManuSortingCodeAndMaterialIdValidatorasync).WithErrorCode(nameof(ErrorCode.MES11308));
        }

        /// <summary>
        /// 参数上下限验证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool ManuSortingParamUpperAndLowerLimitValidator(IEnumerable<SortingParamDto>? param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.MaxValue.HasValue && !item.ParameterValue.HasValue&&item.MinValue.HasValue)
                    {
                            var differenceValue = item.MaxValue - item.MinValue;
                            if (differenceValue == 0)
                            {
                                if (item.MaxContainingType == ContainingTypeEnum.LtOrE)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else if (differenceValue<0)
                            {
                                return false;
                            }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 编码和版本 唯一验证
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationtoken">cancellationToken</param>
        /// <returns></returns>
        private async Task<bool> ManuSortingyCodeAndVersionValidatorasync(ProcSortingRuleCreateDto param, CancellationToken cancellationtoken)
        {
            var procSortingRuleEntity = await _procSortingRuleRepository.GetByCodeAndVersion(new ProcSortingRuleByCodeAndVersionQuery { SiteId = _currentSite.SiteId ?? 0, Code = param.Code, Version = param.Version });

            return procSortingRuleEntity == null;
        }

        /// <summary>
        /// 编码和物物料唯一验证WW
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationtoken">cancellationToken</param>
        /// <returns></returns>
        private async Task<bool> ManuSortingCodeAndMaterialIdValidatorasync(ProcSortingRuleCreateDto param, CancellationToken cancellationtoken)
        {
            var procSortingRuleEntity = await _procSortingRuleRepository.GetByCodeAndMaterialId(new ProcSortingRuleCodeAndMaterialIdQuery { SiteId = _currentSite.SiteId ?? 0, MaterialId = param.MaterialId });
            if (procSortingRuleEntity != null&& param.Code != procSortingRuleEntity.Code)
            {
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 分选规则 修改 验证
    /// </summary>
    internal class ProcSortingRuleModifyValidator : AbstractValidator<ProcSortingRuleModifyDto>
    {
        public ProcSortingRuleModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11302));
            RuleFor(x => x.SortingParamDtos).Must(ManuSortingParamUpperAndLowerLimitValidator).WithErrorCode(nameof(ErrorCode.MES11306));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES11311));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES11313));
        }

        /// <summary>
        /// 参数上下限验证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool ManuSortingParamUpperAndLowerLimitValidator(IEnumerable<SortingParamDto>? param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.MaxValue.HasValue && !item.ParameterValue.HasValue && item.MinValue.HasValue)
                    {
                        var differenceValue = item.MaxValue - item.MinValue;
                        if (differenceValue == 0)
                        {
                            if (item.MaxContainingType == ContainingTypeEnum.LtOrE)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (differenceValue < 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}
