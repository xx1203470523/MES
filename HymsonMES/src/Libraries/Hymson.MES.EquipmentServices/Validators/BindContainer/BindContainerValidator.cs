using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Request.BindContainer;

namespace Hymson.MES.EquipmentServices.Validators.BindContainer
{
    internal class BindContainerValidator : AbstractValidator<BindContainerRequest>
    {
        /// <summary>
        /// 容器绑定验证
        /// </summary>
        public BindContainerValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.ContainerCode).NotEmpty().WithErrorCode(ErrorCode.MES19102);
            RuleFor(x => x.ContainerSFCs).NotEmpty().Must(list => list.Length <= 0).WithErrorCode(ErrorCode.MES19103);
        }
    }
}
