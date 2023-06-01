using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Dtos.NGData;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services
{
    /// <summary>
    /// 获取条码NG数据
    /// </summary>
    public class NGDataService : INGDataService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;

        public NGDataService(IManuSfcStepRepository manuSfcStepRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IManuSfcStepNgRepository manuSfcStepNgRepository, ICurrentEquipment currentEquipment)
        {
            _manuSfcStepRepository = manuSfcStepRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuSfcStepNgRepository = manuSfcStepNgRepository;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 获取条码NG数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NGDataDto> GetNGData(string sfc)
        {
            if (string.IsNullOrEmpty(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
            NGDataDto nGDataDto = new();
            var queryStep = new SFCInOutStepQuery
            {
                Sfc = sfc,
                SiteId = _currentEquipment.SiteId
            };
            //条码所有步骤记录
            var manuSfcStepEntities = await _manuSfcStepRepository.GetSFCInOutStepAsync(queryStep);
            if (manuSfcStepEntities.Any())
            {
                var barCodeStepIds = manuSfcStepEntities.Select(c => c.Id).ToArray();
                //获取所有步骤NG数据
                var manuSfcStepIdsNgQuery = new ManuSfcStepIdsNgQuery
                {
                    BarCodeStepIds = barCodeStepIds,
                    SiteId = _currentEquipment.SiteId
                };
                var manuSfcStepNgEntities = await _manuSfcStepNgRepository.GetByBarCodeStepIdsAsync(manuSfcStepIdsNgQuery);
                //获取NG代码信息
                IEnumerable<QualUnqualifiedCodeEntity> qualUnqualifiedCodeEntities = new List<QualUnqualifiedCodeEntity>();
                if (manuSfcStepNgEntities.Any())
                {
                    var qualUnqualifiedCodeByCodesQuery = new QualUnqualifiedCodeByCodesQuery
                    {
                        Codes = manuSfcStepNgEntities.Select(c => c.UnqualifiedCode).ToArray(),
                        Site = _currentEquipment.SiteId
                    };
                    qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(qualUnqualifiedCodeByCodesQuery);
                }
                //去重后组合NG代码和名称
                var ngUnqualifieds = manuSfcStepNgEntities.GroupBy(c => c.UnqualifiedCode).Select(s =>
                {
                    var unqualifiedCode = s.FirstOrDefault()?.UnqualifiedCode;
                    NGUnqualifiedDto ngUnqualifiedDto = new();
                    if (!string.IsNullOrEmpty(unqualifiedCode))
                    {
                        var unqualifiedCodeInfo = qualUnqualifiedCodeEntities.Where(c => c.UnqualifiedCode == unqualifiedCode).FirstOrDefault();
                        if (unqualifiedCodeInfo != null)
                        {
                            ngUnqualifiedDto.NGCode = unqualifiedCode;
                            ngUnqualifiedDto.NGName = unqualifiedCodeInfo.UnqualifiedCodeName;
                        }
                    }
                    return ngUnqualifiedDto;
                });
                nGDataDto.NGList = ngUnqualifieds.ToArray();
            }
            return nGDataDto;
        }
    }
}
