/*
 *creator: Karl
 *
 *describe: 操作面板    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 操作面板 更新 验证
    /// </summary>
    internal class ManuFacePlateCreateValidator : AbstractValidator<ManuFacePlateCreateDto>
    {
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        public ManuFacePlateCreateValidator(IManuFacePlateRepository manuFacePlateRepository)
        {
            _manuFacePlateRepository = manuFacePlateRepository;
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17201));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17202));
            RuleFor(x => x.Code).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES17206));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17203));
            RuleFor(x => x.Name).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES17207));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17204));
            RuleFor(x => x).MustAsync(async (manuFacePlate, cancellation) =>
            {
                var isExists = await _manuFacePlateRepository.IsExists(manuFacePlate.Code.Trim(), manuFacePlate.Id);
                return !isExists;
            }).WithErrorCode(nameof(ErrorCode.MES17205));
        }
    }

    /// <summary>
    /// 操作面板 修改 验证
    /// </summary>
    internal class ManuFacePlateModifyValidator : AbstractValidator<ManuFacePlateModifyDto>
    {
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        public ManuFacePlateModifyValidator(IManuFacePlateRepository manuFacePlateRepository)
        {
            _manuFacePlateRepository = manuFacePlateRepository;
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17201));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17202));
            RuleFor(x => x.Code).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES17206));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17203));
            RuleFor(x => x.Name).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES17207));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES17204));
            RuleFor(x => x).MustAsync(async (manuFacePlate, cancellation) =>
            {
                var isExists = await _manuFacePlateRepository.IsExists(manuFacePlate.Code.Trim(), manuFacePlate.Id);
                return !isExists;
            }).WithErrorCode(nameof(ErrorCode.MES17205));
        }
    }
}
