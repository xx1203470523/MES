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

            //RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11109));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(nameof(ErrorCode.MES10451));

            //RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11105));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(ProcessRouteTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10452));

            RuleFor(x => x.DynamicData).Cascade(CascadeMode.Stop).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10453)).Must(x => x.Links != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10454)).Must(x => x.Nodes != null && x.Links.Any()).WithErrorCode(nameof(ErrorCode.MES10455));
            //RuleForEach(x => x.DynamicData.Links)
            //    .ChildRules(inner =>
            //    {
            //        inner.RuleFor(i => i.SerialNo)
            //            .NotEmpty()
            //            .WithErrorCode(nameof(ErrorCode.MES10456));
            //        inner.RuleFor(i => i.PreProcessRouteDetailId)
            //            .NotEmpty()
            //            .WithErrorCode(nameof(ErrorCode.MES10457));
            //        inner.RuleFor(i => i.ProcessRouteDetailId)
            //            .NotEmpty()
            //            .WithErrorCode(nameof(ErrorCode.MES10458));
            //        inner.RuleFor(i => i.Extra1)
            //            .NotEmpty()
            //            .WithErrorCode(nameof(ErrorCode.MES10459));
            //    });
            //RuleForEach(x => x.DynamicData.Nodes)
            //    .ChildRules(inner =>
            //    {
            //        inner.RuleFor(i => i.SerialNo)
            //            .NotEmpty()
            //            .WithMessage("工序节点序号不能为空");
            //        inner.RuleFor(i => i.ProcedureId)
            //            .NotEmpty()
            //            .WithMessage("工序节点工序不能为空");
            //        inner.RuleFor(i => i.Code)
            //            .NotEmpty()
            //            .WithMessage("工序节点编码不能为空");
            //        inner.RuleFor(i => i.Name)
            //            .NotEmpty()
            //            .WithMessage("工序节点名称不能为空");
            //        inner.RuleFor(i => i.ProcessType)
            //            .NotEmpty()
            //            .WithMessage("工序节点工序类型不能为空");
            //        inner.RuleFor(i => i.CheckType)
            //            .NotEmpty()
            //            .WithMessage("工序节点抽检类型不能为空");
            //        inner.RuleFor(i => i.IsWorkReport)
            //            .NotEmpty()
            //            .WithMessage("工序节点是否报工不能为空");
            //        inner.RuleFor(i => i.IsFirstProcess)
            //            .NotEmpty()
            //            .WithMessage("工序节点是否首工序不能为空");
            //        inner.RuleFor(i => i.Extra1)
            //            .NotEmpty()
            //            .WithMessage("工序节点中扩展信息不能为空");
            //    });
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

            //RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11109));
            RuleFor(x => x.Status).Must(it => Enum.IsDefined(typeof(SysDataStatusEnum), it)).WithErrorCode(nameof(ErrorCode.MES10451));

            //RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES11105));
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
            RuleFor(i => i.ProcedureId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10461));
            //RuleFor(i => i.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10462));
            RuleFor(i => i.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10463));
            //RuleFor(i => i.ProcessType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10464));
            RuleFor(x => x.ProcessType).Must(it => it == null || Enum.IsDefined(typeof(ProcedureTypeEnum), (ProcedureTypeEnum)it )).WithErrorCode(nameof(ErrorCode.MES10469));
            //RuleFor(i => i.CheckType).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10465));
            RuleFor(x => x.CheckType).Must(it => Enum.IsDefined(typeof(ProcessRouteInspectTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES10470));
            //RuleFor(i => i.IsWorkReport).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10466));
            RuleFor(x => x.IsWorkReport).Must(it => Enum.IsDefined(typeof(TrueOrFalseEnum), (TrueOrFalseEnum)it)).WithErrorCode(nameof(ErrorCode.MES10471));
            //RuleFor(i => i.IsFirstProcess).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10467));
            RuleFor(x => x.IsFirstProcess).Must(it => Enum.IsDefined(typeof(TrueOrFalseEnum), (TrueOrFalseEnum)it)).WithErrorCode(nameof(ErrorCode.MES10472));
            RuleFor(i => i.Extra1).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10468));

            RuleFor(x => x).Must(it => it.CheckType != ProcessRouteInspectTypeEnum.FixedScale ||( it.CheckRate.HasValue && it.CheckRate >= 2) ).WithErrorCode(nameof(ErrorCode.MES10473));
        }
    }

}
