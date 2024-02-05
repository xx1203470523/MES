/*
 *creator: Karl
 *
 *describe: 物料供应商关系    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-27 02:30:48
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 物料供应商关系 更新 验证
    /// </summary>
    internal class ProcMaterialSupplierRelationCreateValidator: AbstractValidator<ProcMaterialSupplierRelationCreateDto>
    {
        public ProcMaterialSupplierRelationCreateValidator()
        {

        }
    }

    /// <summary>
    /// 物料供应商关系 修改 验证
    /// </summary>
    internal class ProcMaterialSupplierRelationModifyValidator : AbstractValidator<ProcMaterialSupplierRelationModifyDto>
    {
        public ProcMaterialSupplierRelationModifyValidator()
        {

        }
    }
}
