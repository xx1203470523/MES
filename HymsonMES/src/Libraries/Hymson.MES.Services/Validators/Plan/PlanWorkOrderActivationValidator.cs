/*
 *creator: Karl
 *
 *describe: 工单激活    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 工单激活 更新 验证
    /// </summary>
    internal class PlanWorkOrderActivationCreateValidator: AbstractValidator<PlanWorkOrderActivationCreateDto>
    {
        public PlanWorkOrderActivationCreateValidator()
        {

        }
    }

    /// <summary>
    /// 工单激活 修改 验证
    /// </summary>
    internal class PlanWorkOrderActivationModifyValidator : AbstractValidator<PlanWorkOrderActivationModifyDto>
    {
        public PlanWorkOrderActivationModifyValidator()
        {

        }
    }
}
