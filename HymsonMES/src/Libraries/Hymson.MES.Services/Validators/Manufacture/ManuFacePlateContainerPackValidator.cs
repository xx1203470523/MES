using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateContainerPackCreateValidator : AbstractValidator<ManuFacePlateContainerPackCreateDto>
    {
        public ManuFacePlateContainerPackCreateValidator()
        {

        }
    }

    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateContainerPackModifyValidator : AbstractValidator<ManuFacePlateContainerPackModifyDto>
    {
        public ManuFacePlateContainerPackModifyValidator()
        {

        }
    }
}
