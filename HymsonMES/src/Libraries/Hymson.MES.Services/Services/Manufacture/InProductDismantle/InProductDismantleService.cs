using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using Mysqlx.Crud;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

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
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

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
        /// 条码
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码关系表 仓储
        /// </summary>
        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="replaceMaterialRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procReplaceMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="circulationRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuBarCodeRelationRepository"></param>
        /// <param name="localizationService"></param>
        public InProductDismantleService(ICurrentUser currentUser, ICurrentSite currentSite,
            IMasterDataService masterDataService,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository replaceMaterialRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository resourceRepository,
            IManuCommonOldService manuCommonOldService,
            IManuSfcCirculationRepository circulationRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository, IManuBarCodeRelationRepository manuBarCodeRelationRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _masterDataService = masterDataService;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _replaceMaterialRepository = replaceMaterialRepository;
            _procMaterialRepository = procMaterialRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuCommonOldService = manuCommonOldService;
            _circulationRepository = circulationRepository;
            _resourceRepository = resourceRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 根据条码查询Bom主物料信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<InProductDismantleComponentDto> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto)
        {
            if (queryDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var bomDetailViews = new InProductDismantleComponentDto();

            var inProductDismantleInfo = new List<InProductDismantleInfoDto>();

            //查询条码表信息
            var sfcList = await _manuSfcRepository.GetAllManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = new List<string> { queryDto.Sfc } });
            if (!sfcList.Any()) return bomDetailViews;
            // 查询bom
            var bomId = sfcList.Select(x => x.ProductBOMId);
            var bominfos = await _procBomRepository.GetByIdsAsync(bomId.OfType<long>());
            if (!bominfos.Any()) return bomDetailViews;

            // 物料Bom
            var barCodeMaterialIds = sfcList.Select(x => x.ProductBOMId).Distinct();

            // 查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdsAsync(barCodeMaterialIds.OfType<long>());
            if (!bomDetails.Any()) return bomDetailViews;

            //查询工序信息
            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = new List<ProcProcedureEntity>();
            if (procedureIds.Any())
            {
                procProcedures = (await _procProcedureRepository.GetByIdsAsync(procedureIds)).ToList();
            }
            // 产品Id
            var procMaterials = sfcList.Select(x => x.ProductId);
            var materialIds = new List<long>();
            // Bom表中的物料ID
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).Distinct();
            if (bomMaterialIds.Any())
            {
                materialIds.AddRange(bomMaterialIds);
            }
            if (procMaterials.Any())
            {
                materialIds.AddRange(procMaterials);
            }
         
            // 查询产品信息
            var procMaterialList = new List<ProcMaterialEntity>();
            var manuProcMaterial = new ManuProcMaterialViewDto();
            var materials = materialIds.Distinct();
            if (materials.Any())
            {
                // 查询Bom和条码信息中的物料id
                procMaterialList = (await _procMaterialRepository.GetByIdsAsync(materials.ToArray())).ToList();
                // 查询条码信息表中使用状态为在用的产品id，作为表头
                var productId = sfcList.Where(x => x.IsUsed == YesOrNoEnum.Yes).Select(x => x.ProductId).FirstOrDefault();
                var procMaterialInfo = procMaterialList.Where(x => x.Id == productId).FirstOrDefault();
                manuProcMaterial = new ManuProcMaterialViewDto
                {
                    Sfc = queryDto.Sfc,
                    MaterialCode = procMaterialInfo?.MaterialCode ?? "",
                    MaterialName = procMaterialInfo?.MaterialName ?? "",
                    Version = procMaterialInfo?.Version ?? ""
                };
            }
            // 查询组件信息
            var manuSfcCirculations = await _manuBarCodeRelationRepository.GetSfcMoudulesAsync(new ManuComponentBarcodeRelationQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = queryDto.Sfc,
                IsDisassemble = queryDto.Type
            });

            if (!manuSfcCirculations.Any() && queryDto.Type != SFCCirculationReportTypeEnum.Whole)
            {
                // 组件物料
                var manuBarCodeMaterialIds = manuSfcCirculations.Select(x => x.InputBarCodeMaterialId).Distinct();
                if (manuBarCodeMaterialIds.Any())
                {
                    materialIds.AddRange(manuBarCodeMaterialIds.OfType<long>());
                }
                bomDetailViews.ManuProcMaterial = manuProcMaterial;
                return bomDetailViews;
            }
              
            //查询资源信息
            var procResources = await GetComponentResourcesAsync(manuSfcCirculations);

            //查询组件信息
            foreach (var detailEntity in bomDetails)
            {
                var material = procMaterialList.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);
                var bom = bominfos.FirstOrDefault(item => item.Id == detailEntity.BomId);
                var bomDetail = new InProductDismantleInfoDto
                {
                    BomDetailId = detailEntity.Id,
                    Usages = detailEntity.Usages,
                    MaterialId = detailEntity.MaterialId,
                    ProcedureId = detailEntity.ProcedureId,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    SerialNumber = detailEntity.DataCollectionWay.HasValue ? detailEntity.DataCollectionWay.Value : material?.SerialNumber,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    BomRemark = bom?.BomCode + "/" + bom?.Version,
                    AssembleCount = 0,
                    Children = new List<ManuSfcChildCirculationDto>()
                };
                inProductDismantleInfo.Add(bomDetail);

                if (!manuSfcCirculations.Any())
                {
                    continue;
                }

                var assembleCount = 0M;
               
                var listCirculations = manuSfcCirculations.Where(a => a.OutputBarCode == queryDto.Sfc).ToList();
              
                foreach (var circulation in listCirculations)
                {
                    var inProductDismantleBusiness = circulation.BusinessContent.ToDeserialize<InProductDismantleBusinessContent>();
                    if (inProductDismantleBusiness == null)
                    {
                        continue;
                    }
                    if (inProductDismantleBusiness.BomMainMaterialId != detailEntity.Id)
                    {
                        continue;
                    }
                    var barcodeMaterial = procMaterialList.FirstOrDefault(item => item.Id == circulation.InputBarCodeMaterialId);

                    var manuSfcChild = new ManuSfcChildCirculationDto
                    {
                        Id = circulation.Id,
                        BomDetailId = detailEntity.Id,
                        ProcedureId = bomDetail.ProcedureId,
                        ProductId = bomDetail.MaterialId,
                        CirculationBarCode = circulation.InputBarCode,
                        CirculationQty = circulation.InputQty,
                        MaterialRemark = barcodeMaterial?.MaterialCode + "/" + barcodeMaterial?.Version,
                        ResCode = circulation.ResourceId.HasValue ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        Status = circulation.IsDisassemble == TrueOrFalseEnum.Yes ? SFCCirculationReportTypeEnum.Remove : SFCCirculationReportTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    };
                    bomDetail.Children.Add(manuSfcChild);
                    assembleCount += manuSfcChild.Status == SFCCirculationReportTypeEnum.Activity ? circulation.InputQty : 0;
                }
                bomDetail.AssembleCount = assembleCount;
            }

            bomDetailViews.ManuProcMaterial = manuProcMaterial;
            bomDetailViews.InProductDismantleInfoDtos = inProductDismantleInfo;
            //查询子组件
            return bomDetailViews;
        }

        /// <summary>
        /// 获取组件资源信息
        /// </summary>
        /// <param name="manuSfcCirculations"></param>
        /// <returns></returns>
        private async Task<List<ProcResourceEntity>> GetComponentResourcesAsync(IEnumerable<ManuBarCodeRelationEntity> manuSfcCirculations)
        {
            var resourceIds = new List<long>();
            foreach (var item in manuSfcCirculations.Select(x => x.ResourceId))
            {
                if (item.HasValue && item.Value > 0 && !resourceIds.Contains(item.Value))
                {
                    resourceIds.Add(item.Value);
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
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetProcBomDetailOldAsync(InProductDismantleQueryDto queryDto)
        {
            if (queryDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var bomDetailViews = new List<InProductDismantleDto>();

            // 查询bom
            var bom = await _procBomRepository.GetByIdAsync(queryDto.BomId);
            if (bom == null) return bomDetailViews;

            // 查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any()) return bomDetailViews;

            // 查询组件信息
            var manuSfcCirculations = await _masterDataService.GetSFCCirculationEntitiesByTypesAsync(new SFCCirculationBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = queryDto.Sfc,
                Type = queryDto.Type
            });
            if (!manuSfcCirculations.Any() && queryDto.Type != SFCCirculationReportTypeEnum.Whole) return bomDetailViews;

            // 组件物料
            var barCodeMaterialIds = manuSfcCirculations.Select(x => x.CirculationProductId).Distinct();

            //bom物料
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).Distinct();

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
                    SerialNumber = detailEntity.DataCollectionWay.HasValue ? detailEntity.DataCollectionWay.Value : material?.SerialNumber,
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

                    var manuSfcChild = new ManuSfcChildCirculationDto
                    {
                        Id = circulation.Id,
                        BomDetailId = detailEntity.Id,
                        ProcedureId = bomDetail.ProcedureId,
                        ProductId = bomDetail.MaterialId,
                        CirculationBarCode = circulation.CirculationBarCode,
                        CirculationQty = circulation.CirculationQty ?? 0,
                        MaterialRemark = barcodeMaterial?.MaterialCode + "/" + barcodeMaterial?.Version,
                        ResCode = circulation.ResourceId.HasValue ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        Status = circulation.IsDisassemble == TrueOrFalseEnum.Yes ? SFCCirculationReportTypeEnum.Remove : SFCCirculationReportTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    };
                    bomDetail.Children.Add(manuSfcChild);
                    assembleCount += manuSfcChild.Status == SFCCirculationReportTypeEnum.Activity ? circulation.CirculationQty ?? 0 : 0;
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
            foreach (var item in manuSfcCirculations.Select(x => x.ResourceId))
            {
                if (item.HasValue && item.Value > 0 && !resourceIds.Contains(item.Value))
                {
                    resourceIds.Add(item.Value);
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
        /// 在制品移除
        /// </summary>
        /// <param name="removeDto"></param>
        /// <returns></returns>
        public async Task RemoveModuleOLdAsync(InProductDismantleRemoveDto removeDto)
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
            if (circulationEntity.IsDisassemble == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16613));
            }

            //验证条码
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = new string[] { removeDto.Sfc }, SiteId = _currentSite.SiteId ?? 0 };
            // 获取条码列表
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (manuSfcProduces == null || !manuSfcProduces.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16600));
            }
            var manuSfcProduce = manuSfcProduces.ToList()[0];

            //报废的不能操作
            if (manuSfcProduce.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }
            await VerifyLockOrRepairAsync(removeDto.Sfc, manuSfcProduce.ProcedureId, manuSfcProduce.Id);

            var inventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = circulationEntity.SiteId,
                BarCode = circulationEntity.CirculationBarCode
            });
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

            var sfcStepEntity = CreateSFCStepEntity(manuSfcProduce, ManuSfcStepTypeEnum.Disassembly, "");

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改组件状态
                rows += await _circulationRepository.DisassemblyUpdateAsync(command);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    trans.Dispose();
                    return;
                }

                // 记录step信息
                rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                if (inventoryEntity != null)
                {
                    // 回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueAsync(quantityCommand);
                }

                trans.Complete();
            }
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

            var circulationEntity = await _manuBarCodeRelationRepository.GetByIdAsync(removeDto.Id);
            if (circulationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (circulationEntity.IsDisassemble == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16613));
            }

            // 查询条码表信息
            var sfcList = await _manuSfcRepository.GetAllManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = new List<string> { removeDto.Sfc } });
            if (sfcList == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16744));
            }
            var manuSfcProduce = sfcList.ToList()[0];

            //报废的不能操作
            if (manuSfcProduce.Status == SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }

            // await VerifyLockOrRepairAsync(removeDto.Sfc, circulationEntity.ProcedureId ?? 0, manuSfcProduce.Id);

            var sfcStepEntity = CreateBarCodeRelationSFCStepEntity(circulationEntity, ManuSfcStepTypeEnum.Disassembly, "");

            var inventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = circulationEntity.SiteId,
                BarCode = circulationEntity.OutputBarCode
            });
            var command = new DManuBarCodeRelationCommand
            {
                Id = removeDto.Id,
                // RelationType = ManuBarCodeRelationTypeEnum.SFC_Split,
                IsDisassemble = TrueOrFalseEnum.Yes,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                DisassembledSfcStepId = sfcStepEntity.Id
            };
            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = circulationEntity.OutputBarCode,
                QuantityResidue = circulationEntity.InputQty,
                UpdatedBy = _currentUser.UserName
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改组件状态
                rows += await _manuBarCodeRelationRepository.ManuBarCodeRelationUpdateAsync(command);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    trans.Dispose();
                    return;
                }

                // 记录step信息
                rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                if (inventoryEntity != null)
                {
                    // 回写库存数据
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
            if (addDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 获取条码列表
            var sfcList = await _manuSfcRepository.GetAllManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = new List<string> { addDto.Sfc } });
            if (sfcList == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16744));
            }
            var manuSfcProduce = sfcList.ToList()[0];
            //if (addDto.IsAssemble && manuSfcProduce.ProcedureId != addDto.ProcedureId)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES16612));
            //}

            //报废的不能操作
            if (manuSfcProduce.Status == SfcStatusEnum.Invalid)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }
            // await VerifyLockOrRepairAsync(addDto.Sfc, manuSfcProduce.ProcedureId, manuSfcProduce.Id);

            // 查询条码关系主条码的数据
            var manuBarCodeRelations = await _manuBarCodeRelationRepository.GetSfcMoudulesAsync(new ManuComponentBarcodeRelationQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = addDto.Sfc,
                IsDisassemble = SFCCirculationReportTypeEnum.Activity,
                BomMainMaterialId = addDto.BomDetailId.ToString(),
            });
           
            //查询bom明细
            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(addDto.BomDetailId);
            var remainQty = bomDetailEntity?.Usages - manuBarCodeRelations.Sum(item => item.InputQty) ?? 0;
            if (remainQty <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16615));
            }
          
            var circulationQty = 0m;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            MaterialSerialNumberEnum? serialNumber = null;

            //如果选择了产品id，根据选的产品id取找是内部、外部还是批次
            if (addDto.CirculationProductId.HasValue)
            {
                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = addDto.CirculationProductId.Value,
                    CirculationBarCode = addDto.CirculationBarCode,
                    CirculationMainProductId = addDto.CirculationMainProductId ?? 0,
                    BomDetailId = addDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Outside)
                {
                    // 验证外部条码合法性
                    var isCorrect = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(addDto.CirculationBarCode, addDto.CirculationProductId ?? 0);
                    if (!isCorrect) throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", addDto.CirculationBarCode);

                    circulationQty = await GetOutsideQtyAsync(addDto.CirculationProductId.Value);
                    if (circulationQty < 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("barCode", addDto.CirculationBarCode);
                    }
                    //如果批次数量大于需要的数量报错
                    if (circulationQty > remainQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16611)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
                else
                {
                    //找库存表，同时判断库存表的产品id需要跟选择的产品id一致，不一致报错
                    //数量为批次大小
                    whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                    {
                        SiteId = _currentSite.SiteId,
                        BarCode = addDto.CirculationBarCode
                    });
                    if (whMaterialInventory == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                    }
                    if (whMaterialInventory.Status == WhMaterialInventoryStatusEnum.InUse || whMaterialInventory.Status == WhMaterialInventoryStatusEnum.Locked)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15122)).WithData("materialBarCode", whMaterialInventory.MaterialBarCode).WithData("status", _localizationService.GetResource($"{typeof(WhMaterialInventoryStatusEnum).FullName}.{whMaterialInventory.Status.ToString()}"));
                    }

                    //扫描的产品需要与选择的产品一致
                    if (whMaterialInventory.MaterialId != addDto.CirculationProductId.Value)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", addDto.CirculationBarCode);
                    }

                    if (serialNumber == MaterialSerialNumberEnum.Inside)
                    {
                        circulationQty = ManuSfcCirculation.CirculationQty;
                    }
                    else
                    {
                        //获取需要上的物料数量
                        circulationQty = remainQty;
                    }

                    //库存数量，库存状态
                    if (whMaterialInventory.QuantityResidue < circulationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
            }
            else
            {
                // 全是内部或者批次数据
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId,
                    BarCode = addDto.CirculationBarCode
                });
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                }
                if (whMaterialInventory.Status == WhMaterialInventoryStatusEnum.InUse || whMaterialInventory.Status == WhMaterialInventoryStatusEnum.Locked)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15122)).WithData("materialBarCode", whMaterialInventory.MaterialBarCode).WithData("status", _localizationService.GetResource($"{typeof(WhMaterialInventoryStatusEnum).FullName}.{whMaterialInventory.Status.ToString()}"));
                }
                addDto.CirculationProductId = whMaterialInventory.MaterialId;

                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = whMaterialInventory.MaterialId,
                    CirculationBarCode = addDto.CirculationBarCode,
                    CirculationMainProductId = addDto.CirculationMainProductId ?? 0,
                    BomDetailId = addDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Inside)
                {
                    circulationQty = ManuSfcCirculation.CirculationQty;
                }
                else
                {
                    //获取需要上的物料数量:用量-已装载数量
                    //获取已装载数量
                    circulationQty = remainQty;
                }

                //库存数量，库存状态
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                }
            }

            // 内部的不允许重复绑定
            if (serialNumber == MaterialSerialNumberEnum.Inside || serialNumber == MaterialSerialNumberEnum.Outside)
            {
                var flag = IsBarCodeComponentRepetAsync(addDto.CirculationBarCode, manuBarCodeRelations.ToList());
                if (flag)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", addDto.CirculationBarCode).WithData("SFC", addDto.Sfc);
                }
            }

            #endregion

            #region 组装数据
            var manuBarCodeRelation = new ManuBarCodeRelationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = addDto.ProcedureId,
                ResourceId = addDto.ResourceId,
                OutputBarCode = addDto.Sfc.ToUpperInvariant(),//主条码
                OutputBarCodeMaterialId = manuSfcProduce.ProductId,
                InputBarCode = addDto.CirculationBarCode.ToUpperInvariant(),//添加的条码
                InputBarCodeMaterialId = addDto.CirculationMainProductId ?? 0,
                InputBarCodeLocation = addDto.Location ?? "",
                IsDisassemble = TrueOrFalseEnum.No,
                InputQty = circulationQty,
                OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,
                RelationType = ManuBarCodeRelationTypeEnum.SFC_Bind,
                BusinessContent = new
                {
                    BomId = bomDetailEntity?.BomId,
                    BomMainMaterialId = addDto.BomDetailId
                }.ToSerialize(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };

            // 2023.10.18 add
            if (addDto.Location != null && !string.IsNullOrEmpty(addDto.Location))
            {
                var manuSfcCirculationEntities = await _manuBarCodeRelationRepository.GetByLocationAsync(new ManuComponentBarcodeRelationLocationQuery
                {
                    SiteId = manuBarCodeRelation.SiteId,
                    SFC = addDto.CirculationBarCode,
                    Location = addDto.Location
                });

                // 说明位置号已经存在
                if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any(a => a.InputBarCode != addDto.CirculationBarCode))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16619))
                        .WithData("Current", addDto.CirculationBarCode)
                        .WithData("Location", addDto.Location)
                        .WithData("BarCode", string.Join(',', manuSfcCirculationEntities.Select(s => s.InputBarCode)));
                }
            }
            
            var type = !addDto.IsAssemble ? ManuSfcStepTypeEnum.Add : ManuSfcStepTypeEnum.Assemble;
            var sfcStepEntity = CreateBarCodeRelationSFCStepEntity(manuBarCodeRelation, type, "");
            #endregion

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 记录step信息
            rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

            // 添加条码关系表信息
            rows += await _manuBarCodeRelationRepository.InsertAsync(manuBarCodeRelation);

            if (serialNumber != MaterialSerialNumberEnum.Outside)
            {
                // 回写库存数据
                rows += await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(new UpdateQuantityCommand
                {
                    BarCode = addDto.CirculationBarCode,
                    QuantityResidue = circulationQty,
                    UpdatedBy = _currentUser.UserName
                });
            }

            trans.Complete();
        }
       
        /// <summary>
        /// 在制品拆解添加组件
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddModuleOldAsync(InProductDismantleAddDto addDto)
        {
            #region 验证
            if (addDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 获取条码列表
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                Sfcs = new string[] { addDto.Sfc },
                SiteId = _currentSite.SiteId ?? 0
            });
            if (manuSfcProduces == null || !manuSfcProduces.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16600));
            }
            var manuSfcProduce = manuSfcProduces.ToList()[0];
            if (addDto.IsAssemble && manuSfcProduce.ProcedureId != addDto.ProcedureId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16612));
            }

            //报废的不能操作
            if (manuSfcProduce.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }
            await VerifyLockOrRepairAsync(addDto.Sfc, manuSfcProduce.ProcedureId, manuSfcProduce.Id);

            // 用量
            var circulationEntities = await GetBarCodesAsync(new BarCodeQueryDto
            {
                Sfc = addDto.Sfc,
                ProcedureId = addDto.ProcedureId,
                ProductId = addDto.CirculationMainProductId ?? 0,
                CirculationBarCode = addDto.CirculationBarCode,
                Type = SFCCirculationReportTypeEnum.Activity
            });

            //查询bom明细
            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(addDto.BomDetailId);
            var remainQty = bomDetailEntity?.Usages - circulationEntities.Sum(item => item.CirculationQty) ?? 0;
            if (remainQty <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16615));
            }

            var circulationQty = 0m;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            MaterialSerialNumberEnum? serialNumber = null;

            //如果选择了产品id，根据选的产品id取找是内部、外部还是批次
            if (addDto.CirculationProductId.HasValue)
            {
                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = addDto.CirculationProductId.Value,
                    CirculationBarCode = addDto.CirculationBarCode,
                    CirculationMainProductId = addDto.CirculationMainProductId ?? 0,
                    BomDetailId = addDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Outside)
                {
                    // 验证外部条码合法性
                    var isCorrect = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(addDto.CirculationBarCode, addDto.CirculationProductId ?? 0);
                    if (!isCorrect) throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", addDto.CirculationBarCode);

                    circulationQty = await GetOutsideQtyAsync(addDto.CirculationProductId.Value);
                    if (circulationQty < 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("barCode", addDto.CirculationBarCode);
                    }
                    //如果批次数量大于需要的数量报错
                    if (circulationQty > remainQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16611)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
                else
                {
                    //找库存表，同时判断库存表的产品id需要跟选择的产品id一致，不一致报错
                    //数量为批次大小
                    whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                    {
                        SiteId = _currentSite.SiteId,
                        BarCode = addDto.CirculationBarCode
                    });
                    if (whMaterialInventory == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                    }

                    //扫描的产品需要与选择的产品一致
                    if (whMaterialInventory.MaterialId != addDto.CirculationProductId.Value)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", addDto.CirculationBarCode);
                    }

                    if (serialNumber == MaterialSerialNumberEnum.Inside)
                    {
                        circulationQty = ManuSfcCirculation.CirculationQty;
                    }
                    else
                    {
                        //获取需要上的物料数量
                        circulationQty = remainQty;
                    }

                    //库存数量，库存状态
                    if (whMaterialInventory.QuantityResidue < circulationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                    }
                }
            }
            else
            {
                // 全是内部或者批次数据
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId,
                    BarCode = addDto.CirculationBarCode
                });
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", addDto.CirculationBarCode);
                }

                addDto.CirculationProductId = whMaterialInventory.MaterialId;

                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = whMaterialInventory.MaterialId,
                    CirculationBarCode = addDto.CirculationBarCode,
                    CirculationMainProductId = addDto.CirculationMainProductId ?? 0,
                    BomDetailId = addDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", addDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Inside)
                {
                    circulationQty = ManuSfcCirculation.CirculationQty;
                }
                else
                {
                    //获取需要上的物料数量:用量-已装载数量
                    //获取已装载数量
                    circulationQty = remainQty;
                }

                //库存数量，库存状态
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", addDto.CirculationBarCode);
                }
            }

            // 内部的不允许重复绑定
            if (serialNumber == MaterialSerialNumberEnum.Inside || serialNumber == MaterialSerialNumberEnum.Outside)
            {
                var flag = IsBarCodeRepetAsync(addDto.CirculationBarCode, circulationEntities.ToList());
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
                ResourceId = addDto.ResourceId,
                SFC = addDto.Sfc.ToUpperInvariant(),
                Location = addDto.Location,
                WorkOrderId = manuSfcProduce.WorkOrderId,
                ProductId = manuSfcProduce.ProductId,
                CirculationBarCode = addDto.CirculationBarCode.ToUpperInvariant(),
                CirculationProductId = addDto.CirculationProductId.Value,
                CirculationMainProductId = addDto.CirculationMainProductId,
                CirculationQty = circulationQty,
                CirculationType = SfcCirculationTypeEnum.ModuleAdd,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };

            // 2023.10.18 add
            if (addDto.Location != null && !string.IsNullOrEmpty(addDto.Location))
            {
                var manuSfcCirculationEntities = await _circulationRepository.GetByLocationAsync(new LocationQuery
                {
                    SiteId = sfcCirculationEntity.SiteId,
                    SFC = sfcCirculationEntity.SFC,
                    Location = addDto.Location
                });

                // 说明位置号已经存在
                if (manuSfcCirculationEntities != null && manuSfcCirculationEntities.Any(a => a.CirculationBarCode != sfcCirculationEntity.CirculationBarCode))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16619))
                        .WithData("Current", sfcCirculationEntity.CirculationBarCode)
                        .WithData("Location", addDto.Location)
                        .WithData("BarCode", string.Join(',', manuSfcCirculationEntities.Select(s => s.CirculationBarCode)));
                }
            }

            var type = !addDto.IsAssemble ? ManuSfcStepTypeEnum.Add : ManuSfcStepTypeEnum.Assemble;
            var sfcStepEntity = CreateSFCStepEntity(manuSfcProduce, type, "");
            #endregion

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            // 记录step信息
            rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

            // 添加组件信息
            rows += await _circulationRepository.InsertAsync(sfcCirculationEntity);

            if (serialNumber != MaterialSerialNumberEnum.Outside)
            {
                // 回写库存数据
                rows += await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(new UpdateQuantityCommand
                {
                    BarCode = addDto.CirculationBarCode,
                    QuantityResidue = circulationQty,
                    UpdatedBy = _currentUser.UserName
                });
            }

            trans.Complete();
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

            // 获取条码列表
            var sfcList = await _manuSfcRepository.GetAllManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = new List<string> { replaceDto.Sfc } });
            if (sfcList == null || !sfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16744));
            }
            var manuSfcProduce = sfcList.ToList()[0];
            //if (addDto.IsAssemble && manuSfcProduce.ProcedureId != addDto.ProcedureId)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES16612));
            //}

            //报废的不能操作
            if (manuSfcProduce.Status == SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }
            // await VerifyLockOrRepairAsync(replaceDto.Sfc, manuSfcProduce.ProcedureId, manuSfcProduce.Id);
            var circulationEntity = await _manuBarCodeRelationRepository.GetByIdAsync(replaceDto.Id);
            if (circulationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16607));
            }

            //只能替换活动的组件信息
            if (circulationEntity.IsDisassemble == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16614));
            }

            var replaceOld = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId,
                BarCode = replaceDto.OldCirculationBarCode
            });
            //用量
            var manuBarCodeRelations = await _manuBarCodeRelationRepository.GetSfcMoudulesAsync(new ManuComponentBarcodeRelationQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                InputBarCode = replaceDto.CirculationBarCode,
                IsDisassemble = SFCCirculationReportTypeEnum.Activity,
                BomMainMaterialId = replaceDto.BomDetailId.ToString(),
            });
            //// 查询bom明细表中ID
            //if (manuBarCodeRelations.Any())
            //{
            //    manuBarCodeRelations = manuBarCodeRelations
            //        .Where(bd => !string.IsNullOrWhiteSpace(bd.BusinessContent))
            //        .Where(bd =>
            //        {
            //            var businessContentObj = JObject.Parse(bd.BusinessContent);
            //            long bomMainMaterialId = businessContentObj["BomMainMaterialId"]?.Value<long>() ?? 0;
            //            return bomMainMaterialId == replaceDto.BomDetailId;
            //        }).ToList();
            //}
            //查询bom明细
            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(replaceDto.BomDetailId);
            var remainQty = bomDetailEntity?.Usages - manuBarCodeRelations.Sum(item => item.InputQty) ?? 0;

            var circulationQty = 0m;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            MaterialSerialNumberEnum? serialNumber = null;

            //如果选择了产品id，根据选的产品id取找是内部、外部还是批次
            if (replaceDto.CirculationProductId.HasValue)
            {
                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = replaceDto.CirculationProductId.Value,
                    CirculationBarCode = replaceDto.CirculationBarCode,
                    CirculationMainProductId = replaceDto.CirculationMainProductId ?? 0,
                    BomDetailId = replaceDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Outside)
                {
                    // 验证外部条码合法性
                    var isCorrect = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(replaceDto.CirculationBarCode, replaceDto.CirculationProductId ?? 0);
                    if (!isCorrect) throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", replaceDto.CirculationBarCode);

                    circulationQty = await GetOutsideQtyAsync(replaceDto.CirculationProductId ?? 0);
                    if (circulationQty < 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                    //如果批次数量大于需要的数量报错
                    if (circulationQty > remainQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16611)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                }
                else
                {
                    //找库存表，同时判断库存表的产品id需要跟选择的产品id一致，不一致报错
                    //数量为批次大小
                    whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                    {
                        SiteId = _currentSite.SiteId,
                        BarCode = replaceDto.CirculationBarCode
                    });
                    if (whMaterialInventory == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", replaceDto.CirculationBarCode);
                    }

                    //扫描的产品需要与选择的产品一致
                    if (whMaterialInventory.MaterialId != replaceDto.CirculationProductId.Value)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                    if (serialNumber == MaterialSerialNumberEnum.Inside)
                    {
                        circulationQty = ManuSfcCirculation.CirculationQty;
                    }
                    else
                    {
                        //获取需要上的物料数量
                        circulationQty = remainQty;
                    }

                    //库存数量，库存状态
                    if (whMaterialInventory.QuantityResidue < circulationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                }
            }
            else
            {
                //全是内部或者批次数据
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = replaceDto.CirculationBarCode });
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                replaceDto.CirculationProductId = whMaterialInventory.MaterialId;

                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = whMaterialInventory.MaterialId,
                    CirculationBarCode = replaceDto.CirculationBarCode,
                    CirculationMainProductId = replaceDto.CirculationMainProductId ?? 0,
                    BomDetailId = replaceDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Inside)
                {
                    circulationQty = ManuSfcCirculation.CirculationQty;
                }
                else
                {
                    //获取需要上的物料数量:用量-已装载数量
                    //获取已装载数量
                    circulationQty = remainQty;
                }

                //库存数量，库存状态
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", replaceDto.CirculationBarCode);
                }
            }

            //内部的不允许重复绑定
            if (serialNumber == MaterialSerialNumberEnum.Inside || serialNumber == MaterialSerialNumberEnum.Outside)
            {
                var flag = IsBarCodeComponentRepetAsync(replaceDto.CirculationBarCode, manuBarCodeRelations.ToList());
                if (flag)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", replaceDto.CirculationBarCode).WithData("SFC", replaceDto.Sfc);
                }
            }
            #endregion

            #region 组装数据
            // 步骤表
            var sfcStepEntity = CreateBarCodeRelationSFCStepEntity(circulationEntity, ManuSfcStepTypeEnum.Replace, "");

            var circulationProductId = whMaterialInventory.MaterialId;
            var command = new DisassemBarCodeRelationblyCommand
            {
                Id = replaceDto.Id,
                // CirculationType = SfcCirculationTypeEnum.Disassembly,
                IsDisassemble = TrueOrFalseEnum.Yes,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                DisassembledSfcStepId = sfcStepEntity.Id
            };
            var oldQuantityCommand = new UpdateQuantityCommand
            {
                BarCode = circulationEntity.InputBarCode,
                QuantityResidue = circulationEntity.InputQty,
                UpdatedBy = _currentUser.UserName
            };

            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = replaceDto?.CirculationBarCode ?? "",
                QuantityResidue = circulationQty,
                UpdatedBy = _currentUser.UserName
            };

            //新件
            var manuBarCodeRelation = new ManuBarCodeRelationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = replaceDto?.ProcedureId ?? 0,
                ResourceId = replaceDto?.ResourceId,
                OutputBarCode = replaceDto?.Sfc.ToUpperInvariant() ?? "",//主条码
                OutputBarCodeMaterialId = replaceDto?.CirculationMainProductId,
                InputBarCode = replaceDto?.CirculationBarCode.ToUpperInvariant() ?? "",//添加的条码
                InputBarCodeMaterialId = manuSfcProduce.ProductId,
                InputBarCodeLocation = replaceDto?.Location ?? "",
                IsDisassemble = TrueOrFalseEnum.No,
                InputQty = circulationQty,
                OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,
                RelationType = ManuBarCodeRelationTypeEnum.SFC_Bind,
                BusinessContent = new
                {
                    BomId = bomDetailEntity?.BomId,
                    BomMainMaterialId = replaceDto?.BomDetailId ?? 0
                }.ToSerialize(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 修改组件状态
                rows += await _manuBarCodeRelationRepository.DisassemblyUpdateAsync(command);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    trans.Dispose();
                    return;
                }

                // 记录step信息
                rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                // 旧件如果不是外部的需要去加库存
                if (replaceOld != null)
                {
                    // 回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueAsync(oldQuantityCommand);
                }

                //插入新件信息
                rows += await _manuBarCodeRelationRepository.InsertAsync(manuBarCodeRelation);
                //新件如果不是外部的需要去减库存
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
        public async Task ReplaceModuleOldAsync(InProductDismantleReplaceDto replaceDto)
        {
            #region 验证

            if (replaceDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //验证条码
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = new string[] { replaceDto.Sfc }, SiteId = _currentSite.SiteId ?? 0 };
            // 获取条码列表
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (manuSfcProduces == null || !manuSfcProduces.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16600));
            }
            var manuSfcProduce = manuSfcProduces.ToList()[0];
            //报废的不能操作
            if (manuSfcProduce.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16617));
            }
            await VerifyLockOrRepairAsync(replaceDto.Sfc, manuSfcProduce.ProcedureId, manuSfcProduce.Id);

            var circulationEntity = await _circulationRepository.GetByIdAsync(replaceDto.Id);
            if (circulationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16607));
            }
            //只能替换活动的组件信息
            if (circulationEntity.IsDisassemble == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16614));
            }

            var replaceOld = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId,
                BarCode = replaceDto.OldCirculationBarCode
            });
            //用量
            var queryDto = new BarCodeQueryDto
            {
                Sfc = replaceDto.Sfc,
                ProcedureId = replaceDto.ProcedureId,
                ProductId = replaceDto.CirculationMainProductId ?? 0,
                CirculationBarCode = replaceDto.CirculationBarCode,
                Type = SFCCirculationReportTypeEnum.Activity
            };
            var circulationEntities = (await GetBarCodesAsync(queryDto)).ToList();
            var oldBarCode = circulationEntities.FirstOrDefault(x => x.Id == replaceDto.Id);
            if (oldBarCode != null)
            {
                circulationEntities.Remove(oldBarCode);
            }

            //查询bom明细
            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(replaceDto.BomDetailId);
            var remainQty = bomDetailEntity?.Usages - circulationEntities.Sum(item => item.CirculationQty) ?? 0;

            var circulationQty = 0m;
            var whMaterialInventory = new WhMaterialInventoryEntity();
            MaterialSerialNumberEnum? serialNumber = null;

            //如果选择了产品id，根据选的产品id取找是内部、外部还是批次
            if (replaceDto.CirculationProductId.HasValue)
            {
                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = replaceDto.CirculationProductId.Value,
                    CirculationBarCode = replaceDto.CirculationBarCode,
                    CirculationMainProductId = replaceDto.CirculationMainProductId ?? 0,
                    BomDetailId = replaceDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Outside)
                {
                    // 验证外部条码合法性
                    var isCorrect = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(replaceDto.CirculationBarCode, replaceDto.CirculationProductId ?? 0);
                    if (!isCorrect) throw new CustomerValidationException(nameof(ErrorCode.MES16605)).WithData("barCode", replaceDto.CirculationBarCode);

                    circulationQty = await GetOutsideQtyAsync(replaceDto.CirculationProductId.Value);
                    if (circulationQty < 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16610)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                    //如果批次数量大于需要的数量报错
                    if (circulationQty > remainQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16611)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                }
                else
                {
                    //找库存表，同时判断库存表的产品id需要跟选择的产品id一致，不一致报错
                    //数量为批次大小
                    whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                    {
                        SiteId = _currentSite.SiteId,
                        BarCode = replaceDto.CirculationBarCode
                    });
                    if (whMaterialInventory == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", replaceDto.CirculationBarCode);
                    }

                    //扫描的产品需要与选择的产品一致
                    if (whMaterialInventory.MaterialId != replaceDto.CirculationProductId.Value)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                    if (serialNumber == MaterialSerialNumberEnum.Inside)
                    {
                        circulationQty = ManuSfcCirculation.CirculationQty;
                    }
                    else
                    {
                        //获取需要上的物料数量
                        circulationQty = remainQty;
                    }

                    //库存数量，库存状态
                    if (whMaterialInventory.QuantityResidue < circulationQty)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", replaceDto.CirculationBarCode);
                    }
                }
            }
            else
            {
                //全是内部或者批次数据
                whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = replaceDto.CirculationBarCode });
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                replaceDto.CirculationProductId = whMaterialInventory.MaterialId;

                //根据选择的产品去找类型
                serialNumber = await GetProductSerialNumberAsync(new BarCodeDataCollectionWayQueryDto
                {
                    ProductId = whMaterialInventory.MaterialId,
                    CirculationBarCode = replaceDto.CirculationBarCode,
                    CirculationMainProductId = replaceDto.CirculationMainProductId ?? 0,
                    BomDetailId = replaceDto.BomDetailId
                });
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", replaceDto.CirculationBarCode);
                }

                if (serialNumber == MaterialSerialNumberEnum.Inside)
                {
                    circulationQty = ManuSfcCirculation.CirculationQty;
                }
                else
                {
                    //获取需要上的物料数量:用量-已装载数量
                    //获取已装载数量
                    circulationQty = remainQty;
                }

                //库存数量，库存状态
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604)).WithData("barCode", replaceDto.CirculationBarCode);
                }
            }

            //内部的不允许重复绑定
            if (serialNumber == MaterialSerialNumberEnum.Inside || serialNumber == MaterialSerialNumberEnum.Outside)
            {
                var flag = IsBarCodeRepetAsync(replaceDto.CirculationBarCode, circulationEntities.ToList());
                if (flag)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", replaceDto.CirculationBarCode).WithData("SFC", replaceDto.Sfc);
                }
            }
            #endregion

            #region 组装数据

            var circulationProductId = whMaterialInventory.MaterialId;
            var command = new DisassemblyCommand
            {
                Id = replaceDto.Id,
                CirculationType = SfcCirculationTypeEnum.Disassembly,
                IsDisassemble = TrueOrFalseEnum.Yes,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            var oldQuantityCommand = new UpdateQuantityCommand
            {
                BarCode = circulationEntity.CirculationBarCode,
                QuantityResidue = circulationEntity.CirculationQty ?? 0,
                UpdatedBy = _currentUser.UserName
            };

            var quantityCommand = new UpdateQuantityCommand
            {
                BarCode = replaceDto?.CirculationBarCode ?? "",
                QuantityResidue = circulationQty,
                UpdatedBy = _currentUser.UserName
            };

            //新件
            var sfcCirculationEntity = new ManuSfcCirculationEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = replaceDto.ProcedureId,
                SFC = replaceDto.Sfc.ToUpperInvariant(),
                WorkOrderId = manuSfcProduce.WorkOrderId,
                ProductId = manuSfcProduce.ProductId,
                CirculationBarCode = replaceDto.CirculationBarCode.ToUpperInvariant(),
                CirculationProductId = circulationProductId,
                CirculationMainProductId = replaceDto.CirculationMainProductId,
                CirculationQty = circulationQty,
                CirculationType = SfcCirculationTypeEnum.ModuleReplace,
                SubstituteId = replaceDto.Id,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName
            };

            var sfcStepEntity = CreateSFCStepEntity(manuSfcProduce, ManuSfcStepTypeEnum.Replace, "");
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 修改组件状态
                rows += await _circulationRepository.DisassemblyUpdateAsync(command);

                // 未更新到数据，事务回滚
                if (rows <= 0)
                {
                    trans.Dispose();
                    return;
                }

                // 记录step信息
                rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                // 旧件如果不是外部的需要去加库存
                if (replaceOld != null)
                {
                    // 回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueAsync(oldQuantityCommand);
                }

                //插入新件信息
                rows += await _circulationRepository.InsertAsync(sfcCirculationEntity);
                //新件如果不是外部的需要去减库存
                if (serialNumber != MaterialSerialNumberEnum.Outside)
                {
                    //回写库存数据
                    rows += await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(quantityCommand);
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取外部条码每次上料数量
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<decimal> GetOutsideQtyAsync(long productId)
        {
            //外部获取批次数量
            decimal qty = 0;
            var material = await _procMaterialRepository.GetByIdAsync(productId);
            if (material != null && material.Batch != null)
            {
                qty = string.IsNullOrEmpty(material.Batch) ? 0 : decimal.Parse(material.Batch);
            }
            return qty;
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
                CirculationMainProductId = queryDto.ProductId,
                IsDisassemble = TrueOrFalseEnum.No
            };
            return await _circulationRepository.GetSfcMoudulesAsync(query);
        }

        /// <summary>
        /// 判断内部条码是否重复
        /// </summary>
        /// <param name="circulationBarCode"></param>
        /// <param name="circulationEntities"></param>
        /// <returns></returns>
        private static bool IsBarCodeComponentRepetAsync(string circulationBarCode, List<ManuBarCodeRelationEntity> circulationEntities)
        {
            if (circulationEntities.Any())
            {
                var entity = circulationEntities.FirstOrDefault(item => item.InputBarCode.ToUpperInvariant() == circulationBarCode.ToUpperInvariant());
                if (entity != null)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断内部条码是否重复
        /// </summary>
        /// <param name="circulationBarCode"></param>
        /// <param name="circulationEntities"></param>
        /// <returns></returns>
        private static bool IsBarCodeRepetAsync(string circulationBarCode, List<ManuSfcCirculationEntity> circulationEntities)
        {
            if (circulationEntities.Any())
            {
                var entity = circulationEntities.FirstOrDefault(item => item.CirculationBarCode.ToUpperInvariant() == circulationBarCode.ToUpperInvariant());
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
            MaterialSerialNumberEnum? mainserialNumber = null;
            var material = await _procMaterialRepository.GetByIdAsync(productId);

            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(bomDetailId);
            if (bomDetailEntity?.DataCollectionWay != null && bomDetailEntity.DataCollectionWay.HasValue)
            {
                mainserialNumber = bomDetailEntity?.DataCollectionWay.Value;
            }

            if (!mainserialNumber.HasValue)
            {
                //读取物料信息，取物料上的数据采集方式
                mainserialNumber = material?.SerialNumber;
            }

            if (circulationMainProductId == productId)
            {
                serialNumber = mainserialNumber;
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", circulationBarCode);
                }
                return serialNumber;
            }

            //读取替代物料
            var replaceMaterials = await _replaceMaterialRepository.GetByBomDetailIdAsync(bomDetailId);
            if (replaceMaterials.Any())
            {
                var replaceMaterialEntity = replaceMaterials.FirstOrDefault(x => x.ReplaceMaterialId == productId);
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
                return serialNumber;
            }
            else
            {
                bool isEnableReplace = bomDetailEntity?.IsEnableReplace ?? false;
                if (!isEnableReplace)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }

                //查询物料下的启用的替代料，找到数据采集方式
                var procReplaces = await _procReplaceMaterialRepository.GetByMaterialIdAsync(circulationMainProductId);
                if (!procReplaces.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }

                var replace = procReplaces.FirstOrDefault(item => item.ReplaceMaterialId == productId);
                if (replace == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16608)).WithData("barCode", circulationBarCode);
                }
                serialNumber = material?.SerialNumber;
                if (!serialNumber.HasValue)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16609)).WithData("barCode", circulationBarCode);
                }
            }

            if (mainserialNumber == MaterialSerialNumberEnum.Batch && serialNumber != MaterialSerialNumberEnum.Batch)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16618)).WithData("barCode", circulationBarCode);
            }

            if (mainserialNumber != MaterialSerialNumberEnum.Batch && serialNumber == MaterialSerialNumberEnum.Batch)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16618)).WithData("barCode", circulationBarCode);
            }
            return serialNumber;
        }

        /// <summary>
        /// 获取主物料下的所有物料列表
        /// </summary>
        /// <param name="bomDetailId"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetBomMaterialsAsync(long bomDetailId)
        {
            var procMaterials = new List<InProductDismantleDto>();
            //读取bom主物料信息
            var bomDetailEntity = await _procBomDetailRepository.GetByIdAsync(bomDetailId);
            if (bomDetailEntity == null)
            {
                return procMaterials;
            }

            long mainProductId = bomDetailEntity.MaterialId;
            var materialIds = new List<long>
            {
                bomDetailEntity.MaterialId
            };

            var procReplaces = new List<ProcReplaceMaterialEntity>();
            //读取替代物料
            var replaceMaterials = (await _replaceMaterialRepository.GetByBomDetailIdAsync(bomDetailId)).ToList();
            if (replaceMaterials.Any())
            {
                var replaceMaterialIds = replaceMaterials.Select(item => item.ReplaceMaterialId).ToList();
                materialIds.AddRange(replaceMaterialIds);
            }
            else
            {
                bool isEnableReplace = bomDetailEntity?.IsEnableReplace ?? false;
                if (isEnableReplace)
                {
                    procReplaces = (await _procReplaceMaterialRepository.GetByMaterialIdAsync(mainProductId)).ToList();
                    if (procReplaces.Any())
                    {
                        var replaceMaterialIds = procReplaces.Select(item => item.ReplaceMaterialId).ToList();
                        materialIds.AddRange(replaceMaterialIds);
                    }
                }
            }

            var procMaterialList = new List<ProcMaterialEntity>();
            var materials = materialIds.Distinct();
            procMaterialList = (await _procMaterialRepository.GetByIdsAsync(materials.ToArray())).ToList();
            if (!procMaterialList.Any())
            {
                return procMaterials;
            }

            //添加主物料
            var mainItem = procMaterialList.FirstOrDefault(x => x.Id == bomDetailEntity?.MaterialId);
            if (mainItem != null)
            {
                procMaterials.Add(new InProductDismantleDto
                {
                    MaterialId = mainItem.Id,
                    MaterialCode = mainItem.MaterialCode,
                    MaterialName = mainItem.MaterialName,
                    SerialNumber = bomDetailEntity?.DataCollectionWay.HasValue == true ? bomDetailEntity.DataCollectionWay.Value : mainItem?.SerialNumber,
                    Version = mainItem?.Version ?? ""
                });
            }

            //添加bom替代料
            replaceMaterials.ForEach(item =>
            {
                var material = procMaterialList.FirstOrDefault(x => x.Id == item.ReplaceMaterialId);
                if (material != null)
                {
                    procMaterials.Add(new InProductDismantleDto
                    {
                        MaterialId = material.Id,
                        MaterialCode = material.MaterialCode,
                        MaterialName = material.MaterialName,
                        SerialNumber = item?.DataCollectionWay.HasValue == true ? item.DataCollectionWay.Value : mainItem?.SerialNumber,
                        Version = material.Version ?? ""
                    });
                }
            });

            //物料维护中的替代料
            procReplaces.ForEach(item =>
            {
                var material = procMaterialList.FirstOrDefault(x => x.Id == item.ReplaceMaterialId);
                if (material != null)
                {
                    procMaterials.Add(new InProductDismantleDto
                    {
                        MaterialId = item.ReplaceMaterialId,
                        MaterialCode = material.MaterialCode,
                        MaterialName = material.MaterialName,
                        SerialNumber = material.SerialNumber,
                        Version = material.Version ?? ""
                    });
                }
            });
            return procMaterials;
        }

        /// <summary>
        /// 验证条码是否锁定和是否有返修任务
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfcInfoId"></param>
        /// <returns></returns>
        private async Task VerifyLockOrRepairAsync(string sfc, long procedureId, long sfcInfoId)
        {
            IEnumerable<long> sfcInfoIds = new[] { sfcInfoId };
            var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCIdsAsync(sfcInfoIds);
            if (sfcProduceBusinessEntities != null && sfcProduceBusinessEntities.Any())
            {
                //锁定的
                var lockEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Lock);
                if (lockEntity != null)
                {
                    lockEntity.VerifyProcedureLock(sfc, procedureId);
                }

                //有缺陷的返修业务
                var repairEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Repair);
                if (repairEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16319)).WithData("SFC", sfc);
                }
            }
        }

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, string remark = "")
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrderId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.ProductBOMId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = type,
                CurrentStatus = sfc.Status,
                Remark = remark,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = sfc.UpdatedBy
            };
        }

        /// <summary>
        /// 创建条码关系步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private ManuSfcStepEntity CreateBarCodeRelationSFCStepEntity(ManuBarCodeRelationEntity sfc, ManuSfcStepTypeEnum type, string remark = "")
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.OutputBarCode,
                Qty = sfc.InputQty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = type,
                Remark = remark,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = sfc.UpdatedBy
            };
        }
    }
}