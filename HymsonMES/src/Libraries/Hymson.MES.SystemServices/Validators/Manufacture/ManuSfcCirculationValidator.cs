using FluentValidation;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Validators.Manufacture
{
    /// <summary>
    /// 查询条码流转记录校验
    /// </summary>
    public class ManuSfcCirculationValidator : AbstractValidator<ManuSfcCirculationDto>
    {
        private readonly ICurrentSystem _currentSystem;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        /// <summary>
        /// 查询条码流转记录校验
        /// </summary>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="currentSystem"></param>
        public ManuSfcCirculationValidator(IManuSfcCirculationRepository manuSfcCirculationRepository, ICurrentSystem currentSystem)
        {
            _currentSystem = currentSystem;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;

        }
    }
}
