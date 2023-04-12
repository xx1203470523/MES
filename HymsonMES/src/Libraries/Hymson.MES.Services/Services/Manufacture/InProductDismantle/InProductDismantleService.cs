using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
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
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Crypto;
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
        /// BOM明细表仓储接口
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
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
        /// 构造函数
        /// </summary>
        public InProductDismantleService(ICurrentUser currentUser, ICurrentSite currentSite,
        IProcBomDetailRepository procBomDetailRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository resourceRepository,
        IManuSfcCirculationRepository circulationRepository,
        IManuSfcProduceRepository manuSfcProduceRepository,
         IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _circulationRepository = circulationRepository;
            _resourceRepository = resourceRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto)
        {
            if (queryDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var bomDetailViews = new List<InProductDismantleDto>();
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }

            //查询物料信息
            var materialIds = bomDetails.Select(item => item.MaterialId).ToArray();
            var procMaterials = new List<ProcMaterialEntity>();
            if (materialIds.Any())
            {
                procMaterials = (await _procMaterialRepository.GetByIdsAsync(materialIds)).ToList();
            }

            //查询工序信息
            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = new List<ProcProcedureEntity>();
            if (procedureIds.Any())
            {
                procProcedures = (await _procProcedureRepository.GetByIdsAsync(procedureIds)).ToList();
            }

            //查询组件信息
            var manuSfcCirculations = await GetCirculationsBySfcAsync(queryDto);

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
                    Children = new List<ManuSfcChildCirculationDto>()
                };
                bomDetailViews.Add(bomDetail);

                if (manuSfcCirculations.Any())
                {
                    var listCirculations = manuSfcCirculations.Where(a => a.ProcedureId == bomDetail.ProcedureId && a.CirculationMainProductId == bomDetail.MaterialId).ToList();
                    foreach (var circulation in listCirculations)
                    {
                        bomDetail.Children.Add(new ManuSfcChildCirculationDto
                        {
                            Id = circulation.Id,
                            ProcedureId = bomDetail.ProcedureId,
                            ProductId = bomDetail.MaterialId,
                            CirculationBarCode = circulation.CirculationBarCode,
                            ResCode = circulation.ResourceId.HasValue == true ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                            Status = circulation.CirculationType == SfcCirculationTypeEnum.Disassembly ? InProductDismantleTypeEnum.Remove : InProductDismantleTypeEnum.Activity,
                            UpdatedBy = circulation.UpdatedBy ?? "",
                            UpdatedOn = circulation.UpdatedOn
                        });
                    }
                }
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

            if (addDto.SerialNumber != MaterialSerialNumberEnum.Batch)
            {
                var types = new List<SfcCirculationTypeEnum>();
                types.Add(SfcCirculationTypeEnum.Consume);
                types.Add(SfcCirculationTypeEnum.ModuleAdd);
                types.Add(SfcCirculationTypeEnum.ModuleReplace);

                var query = new ManuSfcCirculationQuery
                {
                    Sfc = addDto.Sfc,
                    SiteId = _currentSite.SiteId ?? 0,
                    CirculationTypes = types.ToArray(),
                    ProcedureId = addDto.ProcedureId
                };
                var circulationEntities = await _circulationRepository.GetSfcMoudulesAsync(query);
                if (circulationEntities.Any())
                {
                    var entity = circulationEntities.FirstOrDefault(item => item.CirculationBarCode == addDto.CirculationBarCode);
                    if (entity != null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", addDto.CirculationBarCode);
                    }
                }
            }

            //用量
            var circulationQty = 0;
            //内部、外部、批次


            //只有内部和批次的需要校验库存
            var whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(addDto.CirculationBarCode);
            if (whMaterialInventory == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16603));
            }

            //只有内部和批次的需要校验库存
            if (addDto.SerialNumber != MaterialSerialNumberEnum.Outside)
            {
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604));
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
                CirculationProductId = addDto.CirculationProductId,
                CirculationMainProductId = addDto.CirculationMainProductId,
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

                if (addDto.SerialNumber != MaterialSerialNumberEnum.Outside)
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var circulationQty = circulationEntity?.CirculationQty ?? 0;
            if (replaceDto.SerialNumber != MaterialSerialNumberEnum.Outside)
            {
                var whMaterialInventory = await _whMaterialInventoryRepository.GetByBarCodeAsync(replaceDto.CirculationBarCode);
                if (whMaterialInventory == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16603));
                }

                //只有内部和批次的需要校验库存
                //库存数量，库存状态
                //WhMaterialInventoryStatusEnum.Locked
                if (whMaterialInventory.QuantityResidue < circulationQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16604));
                }
            }

            if (replaceDto.SerialNumber != MaterialSerialNumberEnum.Batch)
            {
                var types = new List<SfcCirculationTypeEnum>();
                types.Add(SfcCirculationTypeEnum.Consume);
                types.Add(SfcCirculationTypeEnum.ModuleAdd);
                types.Add(SfcCirculationTypeEnum.ModuleReplace);

                var query = new ManuSfcCirculationQuery
                {
                    Sfc = replaceDto.Sfc,
                    SiteId = _currentSite.SiteId ?? 0,
                    CirculationTypes = types.ToArray(),
                    ProcedureId = replaceDto.ProcedureId
                };
                var circulationEntities = await _circulationRepository.GetSfcMoudulesAsync(query);
                if (circulationEntities.Any())
                {
                    var entity = circulationEntities.FirstOrDefault(item => item.CirculationBarCode == replaceDto.CirculationBarCode);
                    if (entity != null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16601)).WithData("CirculationBarCode", replaceDto.CirculationBarCode);
                    }
                }

            }
            #endregion

            #region 组装数据

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
                BarCode = circulationEntity.CirculationBarCode,
                QuantityResidue = circulationEntity.CirculationQty ?? 0,
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
                CirculationProductId = replaceDto.CirculationProductId,
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
        public async Task GetOutsideBarCodeAsync(CirculationQueryDto circulationQuery)
        {
            //读取主物料信息
           var material= await _procMaterialRepository.GetByIdAsync(circulationQuery.ProductId);
            var maskCodeId= material.MaskCodeId;

        }

    }
}