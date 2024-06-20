/*
 *creator: Karl
 *
 *describe: 物料库存    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Warehouse
{
    /// <summary>
    /// 物料库存 更新 验证
    /// </summary>
    internal class WhMaterialInventoryCreateValidator : AbstractValidator<WhMaterialInventoryCreateDto>
    {
        public WhMaterialInventoryCreateValidator()
        {
        }


    }

    /// <summary>
    /// 物料库存 修改 验证
    /// </summary>
    internal class WhMaterialInventoryModifyValidator : AbstractValidator<WhMaterialInventoryModifyDto>
    {
        public WhMaterialInventoryModifyValidator()
        {

        }
    }

    /// <summary>
    /// 物料拆分验证
    /// </summary>
    internal class WhMaterialBarCodeSplitAdjustValidator : AbstractValidator<MaterialBarCodeSplitAdjustDto>
    {
        public WhMaterialBarCodeSplitAdjustValidator()
        {
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15203));
            RuleFor(x => x.Qty).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15125));
        }
    }
    internal class PickMaterialsRequestValidator : AbstractValidator<PickMaterialsRequest>
    {
        public PickMaterialsRequestValidator()
        {
            RuleFor(x => x.WarehouseCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15203));
            RuleFor(x => x.Qty).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15125));
            RuleFor(x => x.WorkCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES15125));
        }
    }


}
