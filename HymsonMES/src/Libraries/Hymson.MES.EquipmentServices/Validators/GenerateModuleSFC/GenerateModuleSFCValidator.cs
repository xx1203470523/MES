using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.GenerateModuleSFC
{
    /// <summary>
    /// 请求生成模组码-电芯堆叠验证
    /// </summary>
    internal class GenerateModuleSFCValidator : AbstractValidator<GenerateModuleSFCDto>
    {
        public GenerateModuleSFCValidator()
        {

        }
    }
}
