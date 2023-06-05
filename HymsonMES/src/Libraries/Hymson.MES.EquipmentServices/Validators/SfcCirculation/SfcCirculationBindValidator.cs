using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;

namespace Hymson.MES.EquipmentServices.Validators.SfcCirculation
{
    internal class SfcCirculationBindValidator : AbstractValidator<SfcCirculationBindDto>
    {
        /// <summary>
        /// 条码绑定验证
        /// </summary>
        public SfcCirculationBindValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.BindSFCs).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19101);
            //每个条码都不允许为空
            RuleFor(x => x.BindSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.SFC.Trim())).Any()).WithErrorCode(ErrorCode.MES19119);
            //条码不允许重复
            RuleFor(x => x.BindSFCs).Must(list => list.GroupBy(c => c.SFC.Trim()).Where(c => c.Count() < 2).Any()).WithErrorCode(ErrorCode.MES19007);
        }
    }
}
