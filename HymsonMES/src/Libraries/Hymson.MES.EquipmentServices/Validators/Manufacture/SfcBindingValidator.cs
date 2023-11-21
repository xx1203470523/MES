using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.InBound;

namespace Hymson.MES.EquipmentServices.Validators.Manufacture
{
    /// <summary>
    /// 托盘条码
    /// </summary>
    public class SfcBindingValidator : AbstractValidator<SfcBindingDto>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SfcBindingValidator()
        {
            //条码不能为空
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);

            //资源编码不能为空
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);

            //设备编码不能为空
            RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);

            ////条码列表不允许为空 
            RuleFor(x => x.BindSfcs).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19926);
            ////每个条码都不允许为空
            RuleFor(x => x.BindSfcs).Must(list =>
                list.Any(c => !string.IsNullOrEmpty(c.SFC.Trim()))).WithErrorCode(ErrorCode.MES19927);
            ////条码不允许重复
            RuleFor(x => x.BindSfcs).Must(list => list.GroupBy(c => c.SFC.Trim()).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19925);

            ////位置不允许重复
            RuleFor(x => x.BindSfcs).Must(list => list.GroupBy(c => c.Location).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19924);
        }
    }
}
