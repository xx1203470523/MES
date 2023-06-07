using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Validators.InStation
{
    /// <summary>
    ///进站
    /// </summary>
    internal class InStationValidator : AbstractValidator<InStationDto>
    {
        public InStationValidator(IProcResourceRepository procResourceRepository, ICurrentEquipment currentEquipment)
        {

            //条码不能为空
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);

            //资源编码不能为空
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);

            //设备编码不能为空
            RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
        }
    }
}
