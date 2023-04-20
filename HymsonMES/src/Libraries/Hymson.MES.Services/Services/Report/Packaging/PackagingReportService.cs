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

        public async Task<ManuContainerBarcodeDto> QueryManuContainerByCodeAsync(string code)
        {
            //var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(id);
            //if (manuContainerBarcodeEntity != null)
            //{
            //    return manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
            //}
            return null;
        }
    }
}
