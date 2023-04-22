using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 包装报告 服务
    /// </summary>
    public class PackagingReportService : IPackagingReportService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器维护 仓储
        /// </summary>
        private readonly IInteContainerRepository _inteContainerRepository;
        /// <summary>
        /// 容器条码表仓储接口
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 容器装载表（物理删除） 仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        public PackagingReportService(ICurrentSite currentSite,
         IInteContainerRepository inteContainerRepository,
         IManuContainerBarcodeRepository manuContainerBarcodeRepository,
         IProcMaterialRepository procMaterialRepository,
         IManuContainerPackRepository manuContainerPackRepository,
         IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentSite = currentSite;
            _inteContainerRepository = inteContainerRepository;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository=planWorkOrderRepository;
        }

        /// <summary>
        /// 根据容器编码查询容器当前信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeViewDto> QueryManuContainerByCodeAsync(string barCode)
        {
            var barcodeViewDto = new ManuContainerBarcodeViewDto();
            var query = new ManuContainerBarcodeQuery { BarCode = barCode, SiteId = _currentSite.SiteId ?? 0 };
            var barcodeEntity = await _manuContainerBarcodeRepository.GetByCodeAsync(query);
            if (barcodeEntity == null)
            {
                return barcodeViewDto;
            }

            return await GetBarCodeViewAsync(barcodeEntity);
        }

        /// <summary>
        /// 查询装载条码的包装信息
        /// </summary>
        /// <param name="ladeBarCode"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeViewDto> QueryContainerByLadeBarCodeeAsync(string ladeBarCode)
        {
            var barcodeViewDto = new ManuContainerBarcodeViewDto();

            //根据装载的条码获取到容器的id
            var containerPackEntity = await _manuContainerPackRepository.GetByLadeBarCodeAsync(ladeBarCode);
            if (containerPackEntity == null)
            {
                return barcodeViewDto;
            }

            var barcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(containerPackEntity.Id);
            if (barcodeEntity == null)
            {
                return barcodeViewDto;
            }

            return await GetBarCodeViewAsync(barcodeEntity);
        }

        /// <summary>
        /// 查询工单信息
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderListDetailViewDto> GetByWorkOrderCodeAsync(string workOrderCode)
        {
            var query = new PlanWorkOrderQuery
            {
                OrderCode = workOrderCode,
                SiteId = _currentSite.SiteId ?? 0
            };
            await _planWorkOrderRepository.GetByCodeAsync(query);
            return new PlanWorkOrderListDetailViewDto();
        }

        private async Task<ManuContainerBarcodeViewDto> GetBarCodeViewAsync(ManuContainerBarcodeEntity barcodeEntity)
        {
            var barcodeViewDto = new ManuContainerBarcodeViewDto();

            //获取产品信息
            var materials = await _procMaterialRepository.GetByIdAsync(barcodeEntity.ProductId);
            barcodeViewDto.Id = barcodeEntity.Id;
            barcodeViewDto.ProductCode = materials?.MaterialCode ?? "";
            barcodeViewDto.ProductName = materials?.MaterialName ?? "";
            barcodeViewDto.Status = barcodeEntity.Status;

            var inteContainer = await _inteContainerRepository.GetByIdAsync(barcodeEntity.ContainerId);
            barcodeViewDto.Level = inteContainer?.Level;

            var lists = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeEntity.Id);
            barcodeViewDto.CurrentQuantity = lists.ToList().Count;

            return barcodeViewDto;
        }
    }
}
