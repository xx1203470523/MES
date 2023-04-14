/*
 *creator: Karl
 *
 *describe: 操作面板    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using System.Linq;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板-生产过站 service接口
    /// </summary>
    public class ManuFacePlateProductionService : IManuFacePlateProductionService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        #region Repository
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

        public ManuFacePlateProductionService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcProduceRepository manuSfcProduceRepository, IProcBomDetailRepository procBomDetailRepository, IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository, IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }
        #endregion

        #region 组装
        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ManuFacePlateProductionPackageDto> GetManuFacePlateProductionPackageInfo(ManuFacePlateProductionPackageQueryDto param)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(param.SFC);
            //判断工序是否一致
            if (manuSfcProduceEntity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16901));
            }

            //获取对应bom下所有的物料(包含替代物料)
            //读取替代物料
            var mainBomDetails = (await _procBomDetailRepository.GetListMainAsync(manuSfcProduceEntity.ProductBOMId)).Where(x=>x.ProcedureId== param.ProcedureId).OrderBy(x=>x.Seq).ToList();
            var replaceBomDetails = (await _procBomDetailRepository.GetListReplaceAsync(manuSfcProduceEntity.ProductBOMId)).Where(x => x.ProcedureId == param.ProcedureId);


            //获取对应 条码流转表 里已经组装过的数据
            var types = new List<SfcCirculationTypeEnum>();
            //types.Add(SfcCirculationTypeEnum.Consume);
            types.Add(SfcCirculationTypeEnum.ModuleAdd);
            //types.Add(SfcCirculationTypeEnum.ModuleReplace);

            var query = new ManuSfcCirculationQuery
            {
                Sfc = param.SFC,
                SiteId = _currentSite.SiteId ?? 0,
                CirculationTypes = types.ToArray(),
                ProcedureId = param.ProcedureId,
                
                //CirculationMainProductId = manuSfcProduceEntity.ProductId
            };
            var manuSfcCirculationEntitys= await _manuSfcCirculationRepository.GetSfcMoudulesAsync(query);

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
                if (hasAssembleNum == item.Usages)
                {
                    continue;
                }
                else 
                {
                    return new ManuFacePlateProductionPackageDto()
                    {
                        MaterialId=item.MaterialId,
                        MaterialCode=item.MaterialCode,
                        MaterialName=item.MaterialName,
                        MaterialVersion=item.Version,
                        Usages=item.Usages,
                        HasAssembleNum= hasAssembleNum.HasValue? hasAssembleNum.Value:0,

                        BomMainMaterialNum= mainBomDetails.Count,
                        CurrentMainMaterialIndex= mainBomDetails.IndexOf(item)+1,

                        ReplaceMaterialBomDetails= replaceBomDetails.Where(x => x.MaterialId == item.MaterialId)//找到替代物料
                    };
                }
            }

            return null;
        }

        public async Task AddPackageCom() 
        {
            #region 验证

            #endregion

            #region 准备数据
            //var sfcCirculationEntity = new ManuSfcCirculationEntity()
            //{
            //    Id = IdGenProvider.Instance.CreateId(),
            //    SiteId = _currentSite.SiteId ?? 0,
            //    ProcedureId = addDto.ProcedureId,
            //    SFC = addDto.Sfc,
            //    WorkOrderId = manuSfcProduce.WorkOrderId,
            //    ProductId = manuSfcProduce.ProductId,
            //    CirculationBarCode = addDto.CirculationBarCode,
            //    CirculationProductId = whMaterialInventory.MaterialId,
            //    CirculationMainProductId = addDto.CirculationMainProductId,
            //    CirculationQty = circulationQty,
            //    CirculationType = SfcCirculationTypeEnum.ModuleAdd,
            //    CreatedBy = _currentUser.UserName,
            //    UpdatedBy = _currentUser.UserName
            //};
            //var quantityCommand = new UpdateQuantityCommand
            //{
            //    BarCode = addDto.CirculationBarCode,
            //    QuantityResidue = circulationQty,
            //    UpdatedBy = _currentUser.UserName
            //};
            #endregion


        }

        #endregion

    }
}
