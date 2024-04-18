using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板-生产过站 service接口
    /// </summary>
    public class ManuFacePlateProductionService : IManuFacePlateProductionService
    {
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        /// <summary>
        /// 条码流转表仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        public ManuFacePlateProductionService(ICurrentSite currentSite, IManuSfcProduceRepository manuSfcProduceRepository, IProcBomDetailRepository procBomDetailRepository, IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository, IManuSfcCirculationRepository manuSfcCirculationRepository, IProcMaterialRepository procMaterialRepository, IWhMaterialInventoryRepository whMaterialInventoryRepository, IProcMaskCodeRuleRepository procMaskCodeRuleRepository, IProcReplaceMaterialRepository procReplaceMaterialRepository)
        {
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
        }


        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateProductionPackageDto> GetManuFacePlateProductionPackageInfoAsync(ManuFacePlateProductionPackageQueryDto param)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = param.SFC
            });
            //判断工序是否一致
            if (manuSfcProduceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16901));
            }

            //获取对应bom下所有的物料(包含替代物料)
            //读取替代物料
            var mainBomDetails = (await _procBomDetailRepository.GetListMainAsync(manuSfcProduceEntity.ProductBOMId)).Where(x => x.ProcedureId == param.ProcedureId).Where(x => x.Usages > 0).OrderBy(x => x.Seq).ToList();
            var replaceBomDetails = (await _procBomDetailRepository.GetListReplaceAsync(manuSfcProduceEntity.ProductBOMId)).Where(x => x.ProcedureId == param.ProcedureId).ToList();

            //获取对应 条码流转表 里已经组装过的数据
            var types = new List<SfcCirculationTypeEnum>();
            types.Add(SfcCirculationTypeEnum.Consume);
            types.Add(SfcCirculationTypeEnum.ModuleAdd);
            types.Add(SfcCirculationTypeEnum.ModuleReplace);

            var query = new ManuSfcCirculationQuery
            {
                Sfc = param.SFC,
                SiteId = _currentSite.SiteId ?? 0,
                CirculationTypes = types.ToArray(),
                ProcedureId = param.ProcedureId,
                IsDisassemble = TrueOrFalseEnum.No

                //CirculationMainProductId = manuSfcProduceEntity.ProductId
            };
            var manuSfcCirculationEntitys = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(query);

            //按bom主物料顺序处理
            foreach (var item in mainBomDetails)
            {
                //查找每个主物料是否已经完成组装 --根据装配数量来判断
                var hasAssembleNum = manuSfcCirculationEntitys.Where(x => x.CirculationMainProductId == item.MaterialId).Sum(x => x.CirculationQty);
                if (hasAssembleNum >= item.Usages)
                {

                }
                else
                {
                    var mainReplaceMaterials = new List<MainReplaceMaterial>();

                    if (replaceBomDetails == null || replaceBomDetails.Count == 0)
                    {
                        replaceBomDetails = null;

                        if (item.IsEnableReplace)
                        {
                            //当bom下没有替代物料时，获取 主物料下 维护的 替代物料
                            var mainMaterialReplaces = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialQuery
                            {
                                SiteId = _currentSite.SiteId ?? 0,
                                MaterialId = item.MaterialId
                            });
                            foreach (var replace in mainMaterialReplaces)
                            {
                                mainReplaceMaterials.Add(new MainReplaceMaterial()
                                {
                                    MaterialId = replace.Id,
                                    MaterialCode = replace.MaterialCode,
                                    MaterialName = replace.MaterialName,
                                    MaterialVersion = replace.Version!,

                                    SerialNumber = replace.SerialNumber
                                });
                            }

                        }
                    }
                    else
                    {
                        var needQueryMaterialIds = new List<long>();
                        var needQueryMaterialInfos = new List<ProcMaterialEntity>();
                        foreach (var replace in replaceBomDetails)
                        {
                            if (!replace.DataCollectionWay.HasValue)
                            {
                                needQueryMaterialIds.Add(replace.ReplaceMaterialId.ParseToLong());
                            }
                        }
                        if (needQueryMaterialIds.Count > 0)
                        {
                            needQueryMaterialInfos = (await _procMaterialRepository.GetByIdsAsync(needQueryMaterialIds.ToArray())).ToList();
                        }

                        foreach (var replace in replaceBomDetails)
                        {
                            mainReplaceMaterials.Add(new MainReplaceMaterial()
                            {
                                MaterialId = replace.ReplaceMaterialId.ParseToLong(),
                                MaterialCode = replace.MaterialCode,
                                MaterialName = replace.MaterialName,
                                MaterialVersion = replace.Version,

                                SerialNumber = replace.DataCollectionWay.HasValue ? replace.DataCollectionWay.Value : needQueryMaterialInfos.FirstOrDefault(x => x.Id == replace.ReplaceMaterialId.ParseToLong())?.SerialNumber
                            });
                        }
                    }

                    return new ManuFacePlateProductionPackageDto()
                    {
                        MaterialId = item.MaterialId,
                        MaterialCode = item.MaterialCode,
                        MaterialName = item.MaterialName,
                        MaterialVersion = item.Version,
                        SerialNumber = item.DataCollectionWay,
                        Usages = item.Usages,
                        HasAssembleNum = hasAssembleNum.HasValue ? hasAssembleNum.Value : 0,

                        BomMainMaterialNum = mainBomDetails.Count,
                        CurrentMainMaterialIndex = mainBomDetails.IndexOf(item) + 1,
                        // 表proc_bom_detail对应的ID
                        Id = item.Id,
                        MainReplaceMaterials = mainReplaceMaterials
                    };
                }
            }

            return new ManuFacePlateProductionPackageDto();
        }

    }
}
