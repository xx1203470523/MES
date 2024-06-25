/*
 *creator: Karl
 *
 *describe: 条码接收    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 条码接收 更新 验证
    /// </summary>
    internal class PlanSfcReceiveCreateValidator : AbstractValidator<PlanSfcReceiveCreateDto>
    {
        public PlanSfcReceiveCreateValidator()
        {
            RuleFor(x => x.ReceiveType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16101));
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16102));
            RuleFor(x => x.SFCs).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16104));
            RuleFor(x => x).Must(ManuSfcProduceSFCSValidator).WithErrorCode(nameof(ErrorCode.MES16126));
        }

        /// <summary>
        /// 参数验证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool ManuSfcProduceSFCSValidator(PlanSfcReceiveCreateDto param)
        {
            if (param.RelevanceWorkOrderId==param.WorkOrderId)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 条码接收 修改 验证
    /// </summary>
    internal class PlanSfcReceiveModifyValidator : AbstractValidator<PlanSfcReceiveScanCodeDto>
    {
        public PlanSfcReceiveModifyValidator()
        {
            RuleFor(x => x.ReceiveType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16101));
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16102));
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16104));
            RuleFor(x => x).MustAsync(ManuSfcProduceSFCSValidatorAsync).WithErrorCode(nameof(ErrorCode.MES16126));
        }

        /// <summary>
        ///参数验证
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuSfcProduceSFCSValidatorAsync(PlanSfcReceiveScanCodeDto param, CancellationToken cancellationToken)
        {
            if (param.RelevanceWorkOrderId == param.WorkOrderId)
            {
                return await Task.FromResult(false); 
            }
            return await Task.FromResult(true); 
        }
    }

    /// <summary>
    /// 条码接收 修改 验证
    /// </summary>
    internal class PlanSfcReceiveScanListValidator : AbstractValidator<PlanSfcReceiveScanListDto>
    {
        public PlanSfcReceiveScanListValidator()
        {
            RuleFor(x => x.ReceiveType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16101));
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16102));
            RuleFor(x => x.SFCs).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16104));
            RuleFor(x => x).MustAsync((x, c) => Task.FromResult(x.WorkOrderId != x.RelevanceWorkOrderId.GetValueOrDefault())).WithErrorCode(nameof(ErrorCode.MES16126));
        }
    }
}
