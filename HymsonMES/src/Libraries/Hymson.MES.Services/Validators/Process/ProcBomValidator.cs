/*
 *creator: Karl
 *
 *describe: BOM表    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// BOM表 更新 验证
    /// </summary>
    internal class ProcBomCreateValidator : AbstractValidator<ProcBomCreateDto>
    {
        public ProcBomCreateValidator()
        {
            RuleFor(x => x.BomCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10613));
            RuleFor(x => x.BomCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10614));
            RuleFor(x => x.BomName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10615));
            RuleFor(x => x.BomName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10616));
            RuleFor(x => x.Version).NotEmpty().Must(it => it != "").WithErrorCode(nameof(ErrorCode.MES10618));

            RuleFor(x => x.MaterialList).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10603));
            RuleFor(x => x.MaterialList).Must(it => it.Any(a => a.Usages > 0)).WithErrorCode(nameof(ErrorCode.MES10619));
            RuleFor(x => x.MaterialList).Must(it => it.Any(a => a.DataCollectionWay != null && Enum.IsDefined(typeof(MaterialSerialNumberEnum), a.DataCollectionWay ?? 0))).WithErrorCode(nameof(ErrorCode.MES10620));
        }
    }

    internal class ProcBomImportValidator : AbstractValidator<ImportBomDto>
    {
        public ProcBomImportValidator()
        {
            RuleFor(x => x.BomCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10613));
            RuleFor(x => x.BomCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10614));
            RuleFor(x => x.BomName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10615));
            RuleFor(x => x.BomName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10616));
            RuleFor(x => x.Version).NotEmpty().Must(it => it != "").WithErrorCode(nameof(ErrorCode.MES10618));
        }
    }

    /// <summary>
    /// BOM表 修改 验证
    /// </summary>
    internal class ProcBomModifyValidator : AbstractValidator<ProcBomModifyDto>
    {
        public ProcBomModifyValidator()
        {
            RuleFor(x => x.BomName).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10615));
            RuleFor(x => x.BomName).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10616));
            RuleFor(x => x.Version).NotEmpty().Must(it => it != "").WithErrorCode(nameof(ErrorCode.MES10618));

            RuleFor(x => x.MaterialList).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10603));
            RuleFor(x => x.MaterialList).Must(it => !it.Any(a => a.Usages == 0)).WithErrorCode(nameof(ErrorCode.MES10619));
            RuleFor(x => x.MaterialList).Must(it => it.Any(a => a.DataCollectionWay != null || Enum.IsDefined(typeof(MaterialSerialNumberEnum), a.DataCollectionWay ?? 0))).WithErrorCode(nameof(ErrorCode.MES10620));
        }
    }
}
