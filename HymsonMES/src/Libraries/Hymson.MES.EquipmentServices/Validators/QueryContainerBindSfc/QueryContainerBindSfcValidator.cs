using FluentValidation;
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

        }
    }
}
