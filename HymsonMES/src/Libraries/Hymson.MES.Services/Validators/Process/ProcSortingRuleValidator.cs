/*
 *creator: Karl
 *
 *describe: 分选规则    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.Query;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 分选规则 更新 验证
    /// </summary>
    internal class ProcSortingRuleCreateValidator : AbstractValidator<ProcSortingRuleCreateDto>
    {
        private readonly IProcSortingRuleRepository _procSortingRuleRepository;
        public ProcSortingRuleCreateValidator(IProcSortingRuleRepository procSortingRuleRepository)
        {
            _procSortingRuleRepository = procSortingRuleRepository;
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11301));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11302));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11303));
            RuleFor(x => x.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11304));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11305));
            RuleFor(x => x.SortingParamDtos).Must(ManuSortingParamUpperAndLowerLimitValidator).WithErrorCode(nameof(ErrorCode.MES11306));
            RuleFor(x => x).MustAsync(ManuSortingParamcodeRepeatValidatorasync).WithErrorCode(nameof(ErrorCode.MES11306));
        }

        /// <summary>
        /// 参数上下限验证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool ManuSortingParamUpperAndLowerLimitValidator(IEnumerable<SortingParamDto> param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.MaxValue != null)
                    {
                        if (item.MaxValue < item.MaxValue)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="">cancellationToken</param>
        /// <returns></returns>
        private async Task<bool> ManuSortingParamcodeRepeatValidatorasync(ProcSortingRuleCreateDto param, CancellationToken cancellationtoken)
        {
          //  await _procSortingRuleRepository.GetByCodeAndVersion(new ProcSortingRuleByCodeAndVersionQuery { SiteId = });
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
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11303));
            RuleFor(x => x.MaterialId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11304));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11305));
            RuleFor(x => x.SortingParamDtos).Must(ManuSortingParamUpperAndLowerLimitValidator).WithErrorCode(nameof(ErrorCode.MES11306));
        }

        /// <summary>
        /// 参数上下限验证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool ManuSortingParamUpperAndLowerLimitValidator(IEnumerable<SortingParamDto> param)
        {
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.MaxValue != null)
                    {
                        if (item.MaxValue < item.MaxValue)
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
