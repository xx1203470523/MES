using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateProductionCreateValidator : AbstractValidator<ManuFacePlateProductionCreateDto>
    {
        public ManuFacePlateProductionCreateValidator()
        {

        }
    }

    /// <summary>
    /// 新增操作面板验证
    /// </summary>
    internal class ManuFacePlateProductionModifyValidator : AbstractValidator<ManuFacePlateProductionModifyDto>
    {
        public ManuFacePlateProductionModifyValidator()
        {

        }
    }
}
