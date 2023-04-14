/*
 *creator: Karl
 *
 *describe: 条码生产信息验证规则  
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuValidatorDto;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 质量锁定参数验证
    /// </summary>
    internal class ManuSfcProduceLockValidator : AbstractValidator<ManuSfcProduceLockDto>
    {
        public ManuSfcProduceLockValidator()
        {
            RuleFor(x => x.Sfcs).MustAsync(ManuSfcProduceSFCSValidator).WithErrorCode(nameof(ErrorCode.MES15301));
            RuleFor(x => x.Sfcs).MustAsync(ManuSfcProduceSFCSLenghtValidator).WithErrorCode(nameof(ErrorCode.MES15305));
            RuleFor(x => x).MustAsync(ManuSfcProduceValidator).WithErrorCode(nameof(ErrorCode.MES15300));
        }

        /// <summary>
        ///集合非空
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuSfcProduceSFCSValidator(string[] sfcs, CancellationToken cancellationToken)
        {
            if (sfcs == null || !sfcs.Any())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 条码集合长度不超过100
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuSfcProduceSFCSLenghtValidator(string[] sfcs, CancellationToken cancellationToken)
        {
            if (sfcs != null && sfcs.Any() && sfcs.Length > 100)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 未来锁不工序不为空
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuSfcProduceValidator(ManuSfcProduceLockDto parm, CancellationToken cancellationToken)
        {
            if (parm.OperationType == QualityLockEnum.FutureLock)
            {
                if (!parm.LockProductionId.HasValue)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 质量锁定条码验证
    /// </summary>
    internal class ManuSfcLockValidator : AbstractValidator<ManuSfcLockValidatorDto>
    {
        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        public ManuSfcLockValidator(IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;

            RuleFor(x => x.SFC).MustAsync(ManuLockSfcValidator).WithErrorCode(nameof(ErrorCode.MES15304));
            RuleFor(x => x).MustAsync(ManuFutureLockSfcValidator).WithErrorCode(nameof(ErrorCode.MES15304));
            RuleFor(x => x).MustAsync(ManuInstantLockSfcValidator).WithErrorCode(nameof(ErrorCode.MES15304));
            RuleFor(x => x).MustAsync(ManuUnLockEnumValidator).WithErrorCode(nameof(ErrorCode.MES15307));
        }

        /// <summary>
        ///条码是否是在制品
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuLockSfcValidator(string sfc, CancellationToken cancellationToken)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(sfc);
            if (manuSfcProduceEntity == null) { return false; }
            return true;
        }

        /// <summary>
        /// 未来锁验证
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuFutureLockSfcValidator(ManuSfcLockValidatorDto parm, CancellationToken cancellationToken)
        {
            if (parm.OperationType == QualityLockEnum.FutureLock)
            {
            }
            return true;
        }

        /// <summary>
        /// 及时锁验证
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuInstantLockSfcValidator(ManuSfcLockValidatorDto parm, CancellationToken cancellationToken)
        {
            if (parm.OperationType == QualityLockEnum.InstantLock)
            {
              
            }
            return true;
        }

        /// <summary>
        /// 取消锁验证
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> ManuUnLockEnumValidator(ManuSfcLockValidatorDto parm, CancellationToken cancellationToken)
        {
            if (parm.OperationType == QualityLockEnum.Unlock)
            {
                var manuSfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(parm.SFC);
                if (manuSfcProduceBusinessEntity == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 条码生产信息（物理删除） 修改 验证
    /// </summary>
    internal class ManuSfcProduceModifyValidator : AbstractValidator<ManuSfcProduceModifyDto>
    {
        public ManuSfcProduceModifyValidator()
        {
            //RuleFor(x => x.BatchNo).NotEmpty().WithErrorCode("11").WithMessage("11");
            //RuleFor(x => x.BatchNo).MaximumLength(10).WithErrorCode("111").WithMessage("111");
        }
    }
}
