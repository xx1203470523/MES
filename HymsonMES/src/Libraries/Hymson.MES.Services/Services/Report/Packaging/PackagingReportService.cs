using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 包装报告 服务
    /// </summary>
    public class PackagingReportService: IPackagingReportService
    {
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;

        public PackagingReportService(
            IManuContainerBarcodeRepository manuContainerBarcodeRepository)
        {
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByIdAsync(long id)
        {
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(id);
            if (manuContainerBarcodeEntity != null)
            {
                return manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
            }
            return null;
        }
    }
}
