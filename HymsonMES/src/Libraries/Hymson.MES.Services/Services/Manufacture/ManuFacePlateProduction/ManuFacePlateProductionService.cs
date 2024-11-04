using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
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
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;
        /// <summary>
        /// 条码流转表仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        public ManuFacePlateProductionService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcProduceRepository manuSfcProduceRepository, IProcBomDetailRepository procBomDetailRepository, IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository, IManuSfcCirculationRepository manuSfcCirculationRepository, IProcMaterialRepository procMaterialRepository, IWhMaterialInventoryRepository whMaterialInventoryRepository, IProcMaskCodeRuleRepository procMaskCodeRuleRepository, IProcReplaceMaterialRepository procReplaceMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
        }


        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ManuFacePlateProductionPackageDto> GetManuFacePlateProductionPackageInfoAsync(ManuFacePlateProductionPackageQueryDto param)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = param.SFC
            });
            //判断工序是否一致
            if (manuSfcProduceEntity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16901));
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
                SiteId = _currentSite.SiteId ?? 123456,
                CirculationTypes = types.ToArray(),
                ProcedureId = param.ProcedureId,
                IsDisassemble = TrueOrFalseEnum.No

                //CirculationMainProductId = manuSfcProduceEntity.ProductId
            };
            var manuSfcCirculationEntitys = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(query);

            //按bom主物料顺序处理
            foreach (var item in mainBomDetails)
            {
                long mainMaterialId = 0;
                if (!long.TryParse(item.MaterialId, out mainMaterialId))
                {
                    throw new BusinessException(nameof(ErrorCode.MES16902));
                }

                //查找每个主物料是否已经完成组装 --根据装配数量来判断
                var hasAssembleNum = manuSfcCirculationEntitys.Where(x => x.CirculationMainProductId == mainMaterialId).Sum(x => x.CirculationQty);
                if (hasAssembleNum >= item.Usages)
                {
                    continue;
                }
                else
                {
                    var mainReplaceMaterials = new List<MainReplaceMaterial>();

                    if (replaceBomDetails == null || replaceBomDetails.Count() == 0)
                    {
                        replaceBomDetails = new List<ProcBomDetailView>();

                        if (item.IsEnableReplace)
                        {
                            //当bom下没有替代物料时，获取 主物料下 维护的 替代物料
                            var mainMaterialReplaces = await _procReplaceMaterialRepository.GetProcReplaceMaterialViewsAsync(new ProcReplaceMaterialQuery
                            {
                                SiteId = _currentSite.SiteId ?? 123456,
                                MaterialId = long.Parse(item.MaterialId)
                            });
                            foreach (var replace in mainMaterialReplaces)
                            {
                                mainReplaceMaterials.Add(new MainReplaceMaterial()
                                {
                                    MaterialId = replace.Id,
                                    MaterialCode = replace.MaterialCode,
                                    MaterialName = replace.MaterialName,
                                    MaterialVersion = replace.Version,

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

                                SerialNumber = replace.DataCollectionWay.HasValue ? replace.DataCollectionWay.Value : needQueryMaterialInfos.Where(x => x.Id == replace.ReplaceMaterialId.ParseToLong()).FirstOrDefault()?.SerialNumber
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

                        Id = item.Id,// 表proc_bom_detail对应的ID
                        MainReplaceMaterials = mainReplaceMaterials
                    };
                }
            }

            return null;
        }

        /*
        /// <summary>
        /// 组装  （废弃）
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="BusinessException"></exception>
        public async Task<string> AddPackageComAsync(ManuFacePlateProductionPackageAddDto addDto)
        {
            #region 验证
            if (addDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (string.IsNullOrEmpty(addDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16912));
            }

            var manuSfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(addDto.SFC);
            if (manuSfcProduce == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16901));
            }

            if (manuSfcProduce.ProcedureId != addDto.ProcedureId)
            {
                throw new BusinessException(nameof(ErrorCode.MES16903));
            }

            var realityUseProductId = addDto.CirculationProductId.HasValue && addDto.CirculationProductId.Value > 0 ? addDto.CirculationProductId.Value : addDto.CirculationMainProductId;//真实使用的物料

            if (realityUseProductId <= 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES16910));
            }

            //检查实际使用的物料是否存在
            var material = await _procMaterialRepository.GetByIdAsync(realityUseProductId);
            if (material == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16904));
            }

            //检查当前 物料条码 的数据收集方式是哪种
            var serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
            {
                ProductId = realityUseProductId,
                CirculationBarCode = addDto.CirculationBarCode,
                CirculationMainProductId = addDto.CirculationMainProductId,
                BomDetailId = addDto.BomDetailId
            });

            //如果是外部的，只检查条码是否符合掩码规则，无需扣库存
            if (serialNumber == MaterialSerialNumberEnum.Outside)
            {
                var isCorrect = await GetOutsideBarCodeAsync(new CirculationQueryDto
                {
                    CirculationBarCode = addDto.CirculationBarCode,
                    ProductId = realityUseProductId
                });

                if (!isCorrect)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16605));
                }
            }
            else if (serialNumber == MaterialSerialNumberEnum.Batch)
            {//批次的，则检查库存里是否有该条码，检查库存

                //查找库存条码
                var whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = addDto.CirculationBarCode });
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16908)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (whMaterialInventory.MaterialId != material.Id)
                {
                    return nameof(ErrorCode.MES16911);
                    //throw new BusinessException(nameof(ErrorCode.MES16911));
                }

                //库存数量与用量比较，库存状态
                if (whMaterialInventory.QuantityResidue < material.Batch)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16909)).WithData("barCode", addDto.CirculationBarCode);
                }
            }

            #endregion

            #region 准备数据
            var sfcCirculationEntity = new ManuSfcCirculationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 123456,
                ProcedureId = addDto.ProcedureId,
                ResourceId = addDto.ResourceId,
                SFC = addDto.SFC,
                WorkOrderId = manuSfcProduce.WorkOrderId,
                ProductId = manuSfcProduce.ProductId,
                CirculationBarCode = addDto.CirculationBarCode,
                CirculationProductId = realityUseProductId,
                CirculationMainProductId = addDto.CirculationMainProductId,
                CirculationQty = material.Batch,//TODO
                CirculationType = SfcCirculationTypeEnum.ModuleAdd,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now()
            };
            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = addDto.CirculationBarCode,
                QuantityResidue = material.Batch,//TODO
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            #endregion


            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //添加组件信息
                await _manuSfcCirculationRepository.InsertAsync(sfcCirculationEntity);

                if (serialNumber != MaterialSerialNumberEnum.Outside)
                {
                    //回写库存数据
                    await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(quantityCommand);
                }
                trans.Complete();
            }
            return "";
        }

        /// <summary>
        /// 获取物料的数据采集方式
        /// </summary>
        /// <param name="wayQueryDto"></param>
        /// <returns></returns>
        private async Task<MaterialSerialNumberEnum?> GetProductSerialNumberAsync(BarCodeDataCollectionWayQueryDto wayQueryDto)
        {
            //实际使用的物料ID
            long productId = wayQueryDto.ProductId;
            long bomDetailId = wayQueryDto.BomDetailId;
            long circulationMainProductId = wayQueryDto.CirculationMainProductId;//目标主物料
            string circulationBarCode = wayQueryDto.CirculationBarCode;//库存中物料条码

            MaterialSerialNumberEnum? serialNumber = null;
            var material = await _procMaterialRepository.GetByIdAsync(productId);

            if (material == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16904));
            }

            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(bomDetailId);
            //使用的是主物料
            if (circulationMainProductId == productId)
            {
                if (bomDetailEntity?.DataCollectionWay != null)
                {
                    serialNumber = bomDetailEntity?.DataCollectionWay.Value;
                }

                if (!serialNumber.HasValue)
                {
                    //读取物料信息，取物料上的数据采集方式
                    serialNumber = material?.SerialNumber;
                }
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16905)).WithData("materialCode", material.MaterialCode);
                }

                return serialNumber;
            }

            //判断当前主物料是否开启了启用替代物料
            bool isEnableReplace = bomDetailEntity?.IsEnableReplace ?? false;
            if (!isEnableReplace)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16906));
            }

            //获取对应主物料下替代物料
            var replaceMaterials = await _procBomDetailReplaceMaterialRepository.GetByBomDetailIdAsync(bomDetailId);
            if (replaceMaterials.Any())
            {
                var replaceMaterialEntity = replaceMaterials.FirstOrDefault(x => x.ReplaceMaterialId == productId);//找到实际使用的替代物料
                if (replaceMaterialEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16904));
                }

                if (replaceMaterialEntity?.DataCollectionWay != null)
                {
                    serialNumber = replaceMaterialEntity?.DataCollectionWay.Value;
                }

                if (!serialNumber.HasValue)
                {
                    //读取物料信息，取物料上的数据采集方式
                    serialNumber = material?.SerialNumber;
                }

                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16905)).WithData("materialCode", material.MaterialCode);
                }

                return serialNumber;
            }
            else
            {
                throw new BusinessException(nameof(ErrorCode.MES16907));
            }
        }

        /// <summary>
        /// 添加时判断输入的外部组件条码是否存在 
        /// 判断条码是否符合掩码规则
        /// </summary>
        /// <param name="circulationQuery"></param>
        /// <returns></returns>
        private async Task<bool> GetOutsideBarCodeAsync(CirculationQueryDto circulationQuery)
        {
            //读取物料信息
            var material = await _procMaterialRepository.GetByIdAsync(circulationQuery.ProductId);
            var maskCodeId = material?.MaskCodeId ?? 0;
            if (maskCodeId < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16605));
            }

            //查询掩码规则校验
            var procMaskCodes = await _procMaskCodeRuleRepository.GetByMaskCodeIdAsync(maskCodeId);
            if (!procMaskCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16605));
            }

            //根据掩码规则去验证条码，验证不通过就报错  TODO

            return true;
        }
        */

    }
}
