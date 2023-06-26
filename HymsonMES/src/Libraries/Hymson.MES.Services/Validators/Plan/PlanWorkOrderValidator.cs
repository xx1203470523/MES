/*
 *creator: Karl
 *
 *describe: 工单信息表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 工单信息表 更新 验证
    /// </summary>
    internal class PlanWorkOrderCreateValidator : AbstractValidator<PlanWorkOrderCreateDto>
    {
        public PlanWorkOrderCreateValidator()
        {
            RuleFor(x => x.OrderCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16020));
            RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16021));
            RuleFor(x => x.Qty).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES16022));
            //RuleFor(x => x.ProductBOMId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16023));
            RuleFor(x => x.ProcessRouteId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16024));
            RuleFor(x => x.WorkCenterId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16025));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16026));
            RuleFor(x => x.PlanStartTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16027));
            RuleFor(x => x.PlanEndTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16028));

            RuleFor(x => x.PlanStartTime).LessThanOrEqualTo(x => x.PlanEndTime).WithErrorCode(nameof(ErrorCode.MES16029));
            //RuleFor(x => x.PlanStartTime).Must((x,planStatTime)=>x.PlanStartTime<=x.PlanEndTime).WithErrorCode(nameof(ErrorCode.MES16028));

            RuleFor(x => x.OrderCode).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(500).WithErrorCode(nameof(ErrorCode.MES16031));
            RuleFor(x => x.OverScale).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES16034));


            RuleFor(x => x.WorkCenterId).Must(it => it != null && it > 0).WithErrorCode(nameof(ErrorCode.MES16039));
            RuleFor(x => x.WorkCenterType).Must(it => it != null && Enum.IsDefined(typeof(WorkCenterTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16040));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(PlanWorkOrderTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16041));
            RuleFor(x => x.OverScale).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES16042));
            RuleFor(x => x.ProductId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES16021));
            RuleFor(x => x.Qty).Must(it => it > 0 && new Regex("@\"^[0-9]\\d*$\"").IsMatch(it.ToString())).WithErrorCode(nameof(ErrorCode.MES16022));
            RuleFor(x => x.OrderCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES16043));
        }
    }

    /// <summary>
    /// 工单信息表 修改 验证
    /// </summary>
    internal class PlanWorkOrderModifyValidator : AbstractValidator<PlanWorkOrderModifyDto>
    {
        public PlanWorkOrderModifyValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16032));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16033));

            RuleFor(x => x.OrderCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16020));
            RuleFor(x => x.ProductId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16021));
            RuleFor(x => x.Qty).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES16022));
            //RuleFor(x => x.ProductBOMId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16023));
            RuleFor(x => x.ProcessRouteId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16024));
            RuleFor(x => x.WorkCenterId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16025));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16026));
            RuleFor(x => x.PlanStartTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16027));
            RuleFor(x => x.PlanEndTime).NotEmpty().WithErrorCode(nameof(ErrorCode.MES16028));

            RuleFor(x => x.PlanStartTime).LessThanOrEqualTo(x => x.PlanEndTime).WithErrorCode(nameof(ErrorCode.MES16029));
            //RuleFor(x => x.PlanStartTime).Must((x,planStatTime)=>x.PlanStartTime<=x.PlanEndTime).WithErrorCode(nameof(ErrorCode.MES16028));

            RuleFor(x => x.OrderCode).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES16030));
            RuleFor(x => x.Remark).MaximumLength(500).WithErrorCode(nameof(ErrorCode.MES16031));

            RuleFor(x => x.OverScale).GreaterThanOrEqualTo(0).WithErrorCode(nameof(ErrorCode.MES16034));



            RuleFor(x => x.WorkCenterId).Must(it => it != null && it > 0).WithErrorCode(nameof(ErrorCode.MES16039));
            RuleFor(x => x.WorkCenterType).Must(it => it != null && Enum.IsDefined(typeof(WorkCenterTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16040));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(PlanWorkOrderTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES16041));
            RuleFor(x => x.OverScale).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES16042));
            RuleFor(x => x.ProductId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES16021));
            RuleFor(x => x.Qty).Must(it => it > 0 && new Regex("@\"^[0-9]\\d*$\"").IsMatch(it.ToString())).WithErrorCode(nameof(ErrorCode.MES16022));
            RuleFor(x => x.OrderCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES16043));
        }
    }
}
