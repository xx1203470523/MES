using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        
        /// <summary>
        /// 扣料
        /// </summary>
        /// <param name="productBOMId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(long productBOMId, long procedureId)
        {
            // 获取条码对应的工序BOM
            var bomMaterials = await _procBomDetailRepository.GetByBomIdAsync(productBOMId);

            // 未设置BOM
            if (bomMaterials == null || bomMaterials.Any() == false) throw new BusinessException(nameof(ErrorCode.MES10612));

            // 取得特定工序的BOM
            var deductList = new List<MaterialDeductDto> { };
            bomMaterials = bomMaterials.Where(w => w.ProcedureId == procedureId);

            // 统计扣料数据
            MaterialDeductDto deduct = new();
            foreach (var item in bomMaterials)
            {
                // 扣减数量
                deduct.MaterialId = item.MaterialId;
                deduct.Qty = item.Usages * item.Loss;

                // TODO 1.确认收集方式是否批次 item.ReferencePoint
                if (item.ReferencePoint == "TODO 收集方式是批次")
                {
                    // 添加到待扣料集合
                    deductList.Add(deduct);
                    continue;
                }

                var materialEntity = await _procMaterialRepository.GetByIdAsync(item.MaterialId);
                if (materialEntity == null) continue;

                // 2.确认主物料的收集方式，不是"批次"就结束
                if (materialEntity.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 如有设置消耗系数
                if (materialEntity.ConsumeRatio.HasValue == true) deduct.Qty *= materialEntity.ConsumeRatio.Value;

                // 添加到待扣料集合
                deductList.Add(deduct);
            }

            // TODO 扣料

            // 判断在线库存物料是否满足要求（物料编码，数量，状态）

            // 扣料并关联主条码

            // 判断BOM物料绑定？？

        }

    }
}
