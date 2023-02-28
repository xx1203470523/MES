/*
 *creator: pengxin
 *
 *describe: 设备故障原因表    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-02-28 02:50:20
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备故障原因表 更新 验证
    /// </summary>
    internal class EquFaultReasonCreateValidator: AbstractValidator<EquFaultReasonCreateDto>
    {
        public EquFaultReasonCreateValidator()
        {
            RuleFor(x => x.FaultReasonCode).NotEmpty().WithErrorCode(ErrorCode.MES13009);
            RuleFor(x => x.FaultReasonName).NotEmpty().WithErrorCode(ErrorCode.MES13010);
            RuleFor(x => x.UseStatus).NotEmpty().WithErrorCode(ErrorCode.MES13008);
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }

    /// <summary>
    /// 设备故障原因表 修改 验证
    /// </summary>
    internal class EquFaultReasonModifyValidator : AbstractValidator<EquFaultReasonModifyDto>
    {
        public EquFaultReasonModifyValidator()
        {
            //RuleFor(x => x.Remark).NotEmpty().WithErrorCode(ErrorCode.MES10508);
            //RuleFor(x => x.UseStatus).NotEmpty().WithErrorCode(ErrorCode.MES10508);
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
