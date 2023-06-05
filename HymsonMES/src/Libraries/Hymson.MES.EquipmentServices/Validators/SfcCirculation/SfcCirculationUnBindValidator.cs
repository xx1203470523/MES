using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;

namespace Hymson.MES.EquipmentServices.Validators.SfcCirculation
{
    internal class SfcCirculationUnBindValidator : AbstractValidator<SfcCirculationUnBindDto>
    {
        /// <summary>
        /// 条码解绑验证
        /// </summary>
        public SfcCirculationUnBindValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            //RuleFor(x => x.UnBindSFCs).NotEmpty().Must(list => list.Length > 0).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.UnBindSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.Trim())).Any()).WithErrorCode(ErrorCode.MES19120);
            //条码不允许重复
            RuleFor(x => x.UnBindSFCs).Must(list => list.GroupBy(sfc => sfc.Trim()).Where(sfc => sfc.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);
        }
    }
}
