using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.QueryContainerBindSfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.QueryContainerBindSfc
{
    /// <summary>
    ///容器绑定条码查询
    /// </summary>
    internal class QueryContainerBindSfcValidator : AbstractValidator<QueryContainerBindSfcDto>
    {
        public QueryContainerBindSfcValidator()
        {
            //RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
            // RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.ContaineCode).NotEmpty().WithErrorCode(ErrorCode.MES19106);
        }
    }
}
