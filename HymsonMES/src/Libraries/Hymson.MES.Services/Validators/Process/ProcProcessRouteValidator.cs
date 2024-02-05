/*
 *creator: Karl
 *
 *describe: 工艺路线表    验证规则 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 工艺路线表 更新 验证
    /// </summary>
    internal class ProcProcessRouteCreateValidator: AbstractValidator<ProcProcessRouteCreateDto>
    {
        public ProcProcessRouteCreateValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10432));
            RuleFor(x => x.Code).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10444));

            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10433));
            RuleFor(x => x.Name).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10445));

            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10434));
            RuleFor(x => x.Version).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10450));

         
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(ProcessRouteTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10452));

            RuleFor(x => x.DynamicData).Cascade(CascadeMode.Stop).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10453)).Must(x => x.Links != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10454)).Must(x => x.Nodes != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10455));
   
        }
    }

    /// <summary>
    /// 工艺路线表 修改 验证
    /// </summary>
    internal class ProcProcessRouteModifyValidator : AbstractValidator<ProcProcessRouteModifyDto>
    {
        public ProcProcessRouteModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10433));
            RuleFor(x => x.Name).MaximumLength(60).WithErrorCode(nameof(ErrorCode.MES10445));


            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(ProcessRouteTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10452));

            RuleFor(x => x.DynamicData).Cascade(CascadeMode.Stop).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10453)).Must(x => x.Links != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10454)).Must(x => x.Nodes != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10455));
        }
    }

    internal class ProcFlowDynamicLinkValidator : AbstractValidator<FlowDynamicLinkDto>
    {
        public ProcFlowDynamicLinkValidator()
        {
            RuleFor(i => i.SerialNo).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10456));
            RuleFor(i => i.PreProcessRouteDetailId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10457));
            RuleFor(i => i.ProcessRouteDetailId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10458));
            RuleFor(i => i.Extra1).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10459));
        }
    }

    internal class ProcFlowDynamicNodeValidator : AbstractValidator<FlowDynamicNodeDto>
    {
        public ProcFlowDynamicNodeValidator()
        {
            RuleFor(i => i.SerialNo).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10460));
            RuleFor(i => i.ManualSortNumber).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10474));
            RuleFor(x => x.ManualSortNumber).MaximumLength(18).WithErrorCode(nameof(ErrorCode.MES10475));
            RuleFor(i => i.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10461));

            RuleFor(i => i.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10463));

            RuleFor(x => x.ProcessType).Must(it => it == null || Enum.IsDefined(typeof(ProcedureTypeEnum), (ProcedureTypeEnum)it )).WithErrorCode(nameof(ErrorCode.MES10469));

            RuleFor(x => x.CheckType).Must(it => Enum.IsDefined(typeof(ProcessRouteInspectTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10470));

            RuleFor(x => x.IsWorkReport).Must(it => Enum.IsDefined(typeof(TrueOrFalseEnum), (TrueOrFalseEnum)it)).WithErrorCode(nameof(ErrorCode.MES10471));

            RuleFor(x => x.IsFirstProcess).Must(it => Enum.IsDefined(typeof(TrueOrFalseEnum), (TrueOrFalseEnum)it)).WithErrorCode(nameof(ErrorCode.MES10472));
            RuleFor(i => i.Extra1).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10468));

            RuleFor(x => x).Must(it => it.CheckType != ProcessRouteInspectTypeEnum.FixedScale ||( it.CheckRate.HasValue && it.CheckRate >= 2) ).WithErrorCode(nameof(ErrorCode.MES10473));
        }
    }

}
