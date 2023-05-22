using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;

namespace Hymson.MES.EquipmentServices.Validators.BindContainer
{
    internal class BindContainerValidator : AbstractValidator<BindContainerDto>
    {
        /// <summary>
        /// 容器绑定验证
        /// </summary>
        public BindContainerValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.ContainerCode).NotEmpty().WithErrorCode(ErrorCode.MES19102);
            //每个条码都不允许为空
            RuleFor(x => x.ContainerSFCs).Must(list =>
                list.Where(c => !string.IsNullOrEmpty(c.SFC.Trim())).Any()).WithErrorCode(ErrorCode.MES19003);
            RuleFor(x => x.ContainerSFCs).NotEmpty().Must(list => list.Length > 0).WithErrorCode(ErrorCode.MES19103);
        }
    }
}
