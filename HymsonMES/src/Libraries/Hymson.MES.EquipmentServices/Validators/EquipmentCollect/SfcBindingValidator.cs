using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Validators.SfcBinding
{
    /// <summary>
    ///条码绑定
    /// </summary>
    internal class SfcBindingValidator : AbstractValidator<SfcBindingDto>
    {
        public SfcBindingValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {
            //条码不能为空
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);

            //资源编码不能为空
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);

            //设备编码不能为空
            RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);

            //条码列表不允许为空 
            RuleFor(x => x.BindSfc).NotEmpty().Must(list => list.Any()).WithErrorCode(ErrorCode.MES19926);
            //每个条码都不允许为空
            RuleFor(x => x.BindSfc).Must(list =>
                list.Any(c => !string.IsNullOrEmpty(c.SFC.Trim()))).WithErrorCode(ErrorCode.MES19927);
            //条码不允许重复
            RuleFor(x => x.BindSfc).Must(list => list.GroupBy(c => c.SFC.Trim()).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19925);

            //位置不允许重复
            RuleFor(x => x.BindSfc).Must(list => list.GroupBy(c => c.Seq).Any(c => c.Count() < 2)).WithErrorCode(ErrorCode.MES19924);
        }
    }
}
