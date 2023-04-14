using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Crypto;
using System.Collections;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品拆解服务类
    /// </summary>
    public class InProductDismantleService : IInProductDismantleService
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
        /// BOM表仓储接口
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// BOM明细表仓储接口
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// BOM明细替代料表仓储接口
        /// </summary>
        private readonly IProcBomDetailReplaceMaterialRepository _replaceMaterialRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 物料替代组件表仓储
        /// </summary>
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 条码流转表仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _circulationRepository;
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 掩码维护仓储
        /// </summary>
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InProductDismantleService(ICurrentUser currentUser, ICurrentSite currentSite,
         IProcBomRepository procBomRepository,
        IProcBomDetailRepository procBomDetailRepository,
        IProcBomDetailReplaceMaterialRepository replaceMaterialRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcReplaceMaterialRepository procReplaceMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository resourceRepository,
        IManuSfcCirculationRepository circulationRepository,
        IManuSfcProduceRepository manuSfcProduceRepository,
         IWhMaterialInventoryRepository whMaterialInventoryRepository,
         IProcMaskCodeRuleRepository procMaskCodeRuleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _replaceMaterialRepository = replaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _circulationRepository = circulationRepository;
            _resourceRepository = resourceRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto)
        {
            var bomDetailViews = new List<InProductDismantleDto>();

            if (queryDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //查询bom
            var bom = await _procBomRepository.GetByIdAsync(queryDto.BomId);
            if (bom == null)
            {
                return bomDetailViews;
            }

            //查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }

            //查询组件信息
            var manuSfcCirculations = await GetCirculationsBySfcAsync(queryDto);

            //组件物料
            var barCodeMaterialIds = manuSfcCirculations.Select(x => x.CirculationProductId).ToArray().Distinct();

            //bom物料
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).ToArray().Distinct();

            var materialIds = new List<long>();
            if (barCodeMaterialIds.Any())
            {
                materialIds.AddRange(barCodeMaterialIds);
            }
            if (bomMaterialIds.Any())
            {
                materialIds.AddRange(bomMaterialIds);
            }

            var procMaterials = new List<ProcMaterialEntity>();
            var materials = materialIds.Distinct();
            if (materials.Any())
            {
                procMaterials = (await _procMaterialRepository.GetByIdsAsync(materials.ToArray())).ToList();
            }

            //查询工序信息
            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = new List<ProcProcedureEntity>();
            if (procedureIds.Any())
            {
                procProcedures = (await _procProcedureRepository.GetByIdsAsync(procedureIds)).ToList();
            }

            //查询资源信息
            var procResources = await GetResourcesAsync(manuSfcCirculations);

            foreach (var detailEntity in bomDetails)
            {
                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);

                var bomDetail = new InProductDismantleDto
                {
                    BomDetailId = detailEntity.Id,
                    Usages = detailEntity.Usages,
                    MaterialId = detailEntity.MaterialId,
                    ProcedureId = detailEntity.ProcedureId,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    SerialNumber = detailEntity.DataCollectionWay.HasValue == true ? detailEntity.DataCollectionWay.Value : material?.SerialNumber,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    BomRemark = bom.BomCode + "/" + bom.Version,
                    AssembleCount = 0,
                    Children = new List<ManuSfcChildCirculationDto>()
                };
                bomDetailViews.Add(bomDetail);

                if (!manuSfcCirculations.Any())
                {
                    continue;
                }

                var assembleCount = 0M;
                var listCirculations = manuSfcCirculations.Where(a => a.ProcedureId == bomDetail.ProcedureId && a.CirculationMainProductId == bomDetail.MaterialId).ToList();
                foreach (var circulation in listCirculations)
                {
                    var barcodeMaterial = procMaterials.FirstOrDefault(item => item.Id == circulation.CirculationProductId);
                    assembleCount += circulation.CirculationQty ?? 0;

                    bomDetail.Children.Add(new ManuSfcChildCirculationDto
                    {
                        Id = circulation.Id,
                        ProcedureId = bomDetail.ProcedureId,
                        ProductId = bomDetail.MaterialId,
                        CirculationBarCode = circulation.CirculationBarCode,
                        CirculationQty = circulation.CirculationQty ?? 0,
                        MaterialRemark = barcodeMaterial?.MaterialName ?? "" + "/" + barcodeMaterial?.Version ?? "",
                        ResCode = circulation.ResourceId.HasValue == true ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        Status = circulation.CirculationType == SfcCirculationTypeEnum.Disassembly ? InProductDismantleTypeEnum.Remove : InProductDismantleTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    });
                }
                bomDetail.AssembleCount = assembleCount;
            }

            //查询子组件
            return bomDetailViews;
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="manuSfcCirculations"></param>
        /// <returns></returns>
        private async Task<List<ProcResourceEntity>> GetResourcesAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculations)
        {
            var resourceIds = new List<long>();
            foreach (var item in manuSfcCirculations)
            {
                if (item.ResourceId.HasValue && item.ResourceId.Value > 0)
                {
                    if (!resourceIds.Contains(item.ResourceId.Value))
                    {
                        resourceIds.Add(item.ResourceId.Value);
                    }
                }
            }
            var procResources = new List<ProcResourceEntity>();
            if (resourceIds.Any())
            {
                procResources = (await _resourceRepository.GetListByIdsAsync(resourceIds.ToArray())).ToList();
            }
            return procResources;
        }

        /// <summary>
        /// 获取sfc组件信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationEntity>> GetCirculationsBySfcAsync(InProductDismantleQueryDto queryDto)
        {
            var types = new List<SfcCirculationTypeEnum>();
            if (queryDto.Type == InProductDismantleTypeEnum.Remove
                || queryDto.Type == InProductDismantleTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Disassembly);
            }

            if (queryDto.Type == InProductDismantleTypeEnum.Activity
              || queryDto.Type == InProductDismantleTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Consume);
                types.Add(SfcCirculationTypeEnum.ModuleAdd);
                types.Add(SfcCirculationTypeEnum.ModuleReplace);
            }

            var query = new ManuSfcCirculationQuery { Sfc = queryDto.Sfc, SiteId = _currentSite.SiteId ?? 0, CirculationTypes = types.ToArray() };
            return await _circulationRepository.GetSfcMoudulesAsync(query);
        }

        /// <summary>
        /// 在制品移除
        /// </summary>
        /// <param name="removeDto"></param>
        /// <returns></returns>
        public async Task RemoveModuleAsync(InProductDismantleRemoveDto removeDto)
        {
            if (removeDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var circulationEntity = await _circulationRepository.GetByIdAsync(removeDto.Id);
            if (circulationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var command = new DisassemblyCommand
            {
                Id = removeDto.Id,
                CirculationType = SfcCirculationTypeEnum.Disassembly,
                IsDisassemble = TrueOrFalseEnum.Yes,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = circulationEntity.CirculationBarCode,
                QuantityResidue = circulationEntity.CirculationQty ?? 0,
                UpdatedBy = _currentUser.UserName
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改组件状态
                rows += await _circulationRepository.DisassemblyUpdateAsync(command);

                if (removeDto.SerialNumber != MaterialSerialNumberEnum.Outside)
                {
                    //回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueAsync(quantityCommand);
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 在制品拆解添加组件
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddModuleAsync(InProductDismantleAddDto addDto)
        {
            #region 验证
            if (addDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var manuSfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(addDto.Sfc);
            if (manuSfcProduce == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16600));
            }

            //用量
            var circulationQty = 0M;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            MaterialSerialNumberEnum? serialNumber = null;
            //如果选择了产品id，根据选的产品id取找是内部、外部还是批次
            if (addDto.ProductId.HasValue)
            {
                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = addDto.ProductId.Value,
                    CirculationBarCode= addDto.CirculationBarCode,
                    CirculationMainProductId= addDto.MainProductId ?? 0,
                    BomDetailId= addDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Outside)
                {
                    //验证外部条码合法性
                    var isCorrect = await GetOutsideBarCodeAsync(new CirculationQueryDto
                    {
                        CirculationBarCode = addDto.CirculationBarCode,
                        ProductId = addDto.ProductId ?? 0
                    });

                    if (!isCorrect)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", addDto.CirculationBarCode);
                    }
                    circulationQty = await GetOutsideQtyAsync(addDto.ProductId.Value);
                    if (circulationQty < 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("barCode", addDto.CirculationBarCode);
                    }
                    //如果批次数量大于需要的数量报错
                    if (circulationQty > addDto.CirculationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16611)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
                else
                {
                    //找库存表，同时判断库存表的产品id需要跟选择的产品id一致，不一致报错
                    //数量为批次大小
                    whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(addDto.CirculationBarCode);
                    if (whMaterialInventory == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                    }

                    //扫描的产品需要与选择的产品一致
                    if (whMaterialInventory.MaterialId != addDto.ProductId)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", addDto.CirculationBarCode);
                    }

                    //获取需要上的物料数量
                    circulationQty = addDto.CirculationQty;
                    //库存数量，库存状态
                    if (whMaterialInventory.QuantityResidue < circulationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
            }
            else
            {
                //全是内部或者批次数据
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(addDto.CirculationBarCode);
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                }

                //获取需要上的物料数量
                circulationQty = addDto.CirculationQty;

                //判断物料是否存在，不存在报错

                //库存数量，库存状态
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                }
            }

            //内部的不允许重复绑定
            if (serialNumber == MaterialSerialNumberEnum.Inside)
            {
                var flag = await IsBarCodeRepetAsync(new BarCodeQueryDto
                {
                    Sfc = addDto.Sfc,
                    ProcedureId = addDto.ProcedureId,
                    ProductId = addDto.MainProductId ?? 0,
                    CirculationBarCode = addDto.CirculationBarCode
                });
                if (flag)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", addDto.CirculationBarCode).WithData("SFC", addDto.Sfc);
                }
            }

            #endregion

            #region 组装数据
            var sfcCirculationEntity = new ManuSfcCirculationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = addDto.ProcedureId,
                SFC = addDto.Sfc,
                WorkOrderId = manuSfcProduce.WorkOrderId,
                ProductId = manuSfcProduce.ProductId,
                CirculationBarCode = addDto.CirculationBarCode,
                CirculationProductId = whMaterialInventory.MaterialId,
                CirculationMainProductId = addDto.MainProductId,
                CirculationQty = circulationQty,
                CirculationType = SfcCirculationTypeEnum.ModuleAdd,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };
            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = addDto.CirculationBarCode,
                QuantityResidue = circulationQty,
                UpdatedBy = _currentUser.UserName
            };
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //添加组件信息
                rows += await _circulationRepository.InsertAsync(sfcCirculationEntity);

                if (serialNumber != MaterialSerialNumberEnum.Outside)
                {
                    //回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(quantityCommand);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 在制品拆解换件
        /// </summary>
        /// <param name="replaceDto"></param>
        /// <returns></returns>
        public async Task ReplaceModuleAsync(InProductDismantleReplaceDto replaceDto)
        {
            #region 验证

            if (replaceDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var manuSfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(replaceDto.Sfc);
            if (manuSfcProduce == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16600));
            }

            var circulationEntity = await _circulationRepository.GetByIdAsync(replaceDto.Id);
            if (circulationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16607));
            }

            var circulationQty = circulationEntity?.CirculationQty ?? 0;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            if (replaceDto.SerialNumber != MaterialSerialNumberEnum.Outside)
            {
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(replaceDto.CirculationBarCode);
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                //只有内部和批次的需要校验库存
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", replaceDto.CirculationBarCode); ;
                }
            }
            else
            {
                //验证外部条码合法性
                var isCorrect = await GetOutsideBarCodeAsync(new CirculationQueryDto
                {
                    CirculationBarCode = replaceDto.CirculationBarCode,
                    ProductId = replaceDto.ProductId ?? 0
                });

                if (!isCorrect)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", replaceDto.CirculationBarCode);
                }
            }

            //内部的不允许重复绑定
            if (replaceDto.SerialNumber == MaterialSerialNumberEnum.Inside)
            {
                var flag = await IsBarCodeRepetAsync(new BarCodeQueryDto
                {
                    Sfc = replaceDto.Sfc,
                    ProcedureId = replaceDto.ProcedureId,
                    ProductId = replaceDto.ProductId ?? 0,
                    CirculationBarCode = replaceDto.CirculationBarCode
                });
                if (flag)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", replaceDto.CirculationBarCode).WithData("SFC", replaceDto.Sfc);
                }
            }
            #endregion

            #region 组装数据

            var circulationProductId = replaceDto.SerialNumber == MaterialSerialNumberEnum.Outside ? replaceDto.ProductId.Value : whMaterialInventory.MaterialId;
            var command = new DisassemblyCommand
            {
                Id = replaceDto.Id,
                CirculationType = SfcCirculationTypeEnum.Disassembly,
                IsDisassemble = TrueOrFalseEnum.Yes,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = circulationEntity?.CirculationBarCode ?? "",
                QuantityResidue = circulationQty,
                UpdatedBy = _currentUser.UserName
            };

            //新件
            var sfcCirculationEntity = new ManuSfcCirculationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = replaceDto.ProcedureId,
                SFC = replaceDto.Sfc,
                WorkOrderId = manuSfcProduce.WorkOrderId,
                ProductId = manuSfcProduce.ProductId,
                CirculationBarCode = replaceDto.CirculationBarCode,
                CirculationProductId = circulationProductId,
                CirculationMainProductId = replaceDto.CirculationMainProductId,
                CirculationQty = circulationQty,
                CirculationType = SfcCirculationTypeEnum.ModuleReplace,
                SubstituteId = replaceDto.Id,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改组件状态
                rows += await _circulationRepository.DisassemblyUpdateAsync(command);

                if (replaceDto.SerialNumber != MaterialSerialNumberEnum.Outside)
                {
                    //回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueAsync(quantityCommand);
                }

                //插入新件信息
                rows += await _circulationRepository.InsertAsync(sfcCirculationEntity);

                trans.Complete();
            }
        }

        /// <summary>
        /// 添加、替换时判断输入的组件条码是否存在
        /// </summary>
        /// <param name="circulationQuery"></param>
        /// <returns></returns>
        private async Task<bool> GetOutsideBarCodeAsync(CirculationQueryDto circulationQuery)
        {
            //读取主物料信息
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

            //根据掩码规则去验证条码，验证不通过就报错
            return true;
        }

        /// <summary>
        /// 获取外部条码每次上料数量
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<int> GetOutsideQtyAsync(long productId)
        {
            //外部获取批次数量
            var material = await _procMaterialRepository.GetByIdAsync(productId);
            return material.Batch;
        }

        /// <summary>
        /// 获取已经装载的数量
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<decimal> GetQtyAsync(BarCodeQueryDto queryDto)
        {
            var count = 0m;
            //外部获取批次数量
            var circulationEntities = await GetBarCodesAsync(queryDto);
            if (circulationEntities.Any())
            {
                var list = circulationEntities.ToList();
                foreach (var entity in list)
                {
                    count += entity.CirculationQty ?? 0;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取挂载的活动组件信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationEntity>> GetBarCodesAsync(BarCodeQueryDto queryDto)
        {
            var types = new List<SfcCirculationTypeEnum>();
            types.Add(SfcCirculationTypeEnum.Consume);
            types.Add(SfcCirculationTypeEnum.ModuleAdd);
            types.Add(SfcCirculationTypeEnum.ModuleReplace);

            var query = new ManuSfcCirculationQuery
            {
                Sfc = queryDto.Sfc,
                SiteId = _currentSite.SiteId ?? 0,
                CirculationTypes = types.ToArray(),
                ProcedureId = queryDto.ProcedureId,
                CirculationMainProductId = queryDto.ProductId
            };
            return await _circulationRepository.GetSfcMoudulesAsync(query);
        }

        /// <summary>
        /// 判断条码是否重复
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<bool> IsBarCodeRepetAsync(BarCodeQueryDto queryDto)
        {
            var circulationEntities = await GetBarCodesAsync(queryDto);
            if (circulationEntities.Any())
            {
                var entity = circulationEntities.FirstOrDefault(item => item.CirculationBarCode == queryDto.CirculationBarCode);
                if (entity != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取产品的数据采集方式
        /// </summary>
        /// <param name="wayQueryDto"></param>
        /// <returns></returns>
        private async Task<MaterialSerialNumberEnum?> GetProductSerialNumberAsync(BarCodeDataCollectionWayQueryDto wayQueryDto)
        {
            long productId = wayQueryDto.ProductId;
            long bomDetailId = wayQueryDto.BomDetailId;
            long circulationMainProductId = wayQueryDto.CirculationMainProductId;
            string circulationBarCode = wayQueryDto.CirculationBarCode;

            MaterialSerialNumberEnum? serialNumber = null;
            var material = await _procMaterialRepository.GetByIdAsync(productId);

            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(bomDetailId);
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
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", circulationBarCode);
                }
            }

            //读取替代物料
            var replaceMaterials = await _replaceMaterialRepository.GetByBomDetailIdAsync(bomDetailId);
            if (replaceMaterials.Any())
            {
                var replaceMaterialEntity = replaceMaterials.FirstOrDefault(x => x.ReplaceMaterialId == bomDetailId);
                if (replaceMaterialEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
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
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", circulationBarCode);
                }
            }
            else
            {
                bool isEnableReplace = bomDetailEntity?.IsEnableReplace ?? false;
                if (!isEnableReplace)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }

                //查询物料下的启用的替代料，找到数据采集方式
                var procReplaces = await _procReplaceMaterialRepository.GetByMaterialIdAsync(productId);
                if (!procReplaces.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }

                var replace = procReplaces.FirstOrDefault(item => item.ReplaceMaterialId == productId);
                if (replace == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }
            }
            return serialNumber;
        }

    }
}