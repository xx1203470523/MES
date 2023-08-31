/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Validators.Plan
{
    /// <summary>
    /// 工单激活（物理删除） 更新 验证
    /// </summary>
    internal class PlanWorkOrderBindCreateValidator: AbstractValidator<PlanWorkOrderBindCreateDto>
    {
        public PlanWorkOrderBindCreateValidator()
        {

        }
    }

    /// <summary>
    /// 工单激活（物理删除） 修改 验证
    /// </summary>
    internal class PlanWorkOrderBindModifyValidator : AbstractValidator<PlanWorkOrderBindModifyDto>
    {
        public PlanWorkOrderBindModifyValidator()
        {

        }
    }
}
