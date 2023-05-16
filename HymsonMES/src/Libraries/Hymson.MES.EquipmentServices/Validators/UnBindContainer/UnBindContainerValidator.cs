using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Request.UnBindContainer;

namespace Hymson.MES.EquipmentServices.Validators.BindContainer
{
    internal class UnBindContainerValidator : AbstractValidator<UnBindContainerRequest>
    {
        /// <summary>
        /// 容器绑定验证
        /// </summary>
        public UnBindContainerValidator()
        {
            RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.ContainerCode).NotEmpty().WithErrorCode(ErrorCode.MES19102);
            RuleFor(x => x.ContainerSFCs).NotEmpty().Must(list => list.Length <= 0).WithErrorCode(ErrorCode.MES19103);
        }
    }
}
