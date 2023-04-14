/*
 *creator: Karl
 *
 *describe: 在制品维修    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-04-12 10:32:46
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品维修 服务
    /// </summary>
    public class ManuFacePlateRepairService : IManuFacePlateRepairService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 在制品维修 仓储
        /// </summary>
        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储 
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 资源表 仓储
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;

        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationModifyRules;

        public ManuFacePlateRepairService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository, IProcMaterialRepository procMaterialRepository,
             IManuProductBadRecordRepository manuProductBadRecordRepository, IProcResourceRepository procResourceRepository,
        AbstractValidator<ManuFacePlateRepairCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateRepairModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _procResourceRepository = procResourceRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }



        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="beginRepairDto"></param> 
        /// <returns></returns>
        public async Task<ManuFacePlateRepairOpenInfoDto> BeginManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {

            #region 调用作业
            //调用作业

            #endregion

            #region 验证条码更新在制信息
            //验证条码
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(beginRepairDto.SFC);
            if (manuSfcProduceEntit == null)
            {
                //是否已入库完成 待使用算完成
                var whMaterialInventoryEntit = await _whMaterialInventoryRepository.GetByBarCodeAsync(beginRepairDto.SFC);
                if (whMaterialInventoryEntit == null)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16306));
                }
                else if (whMaterialInventoryEntit.Status != WhMaterialInventoryStatusEnum.ToBeUsed)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16311)).WithData("Status", whMaterialInventoryEntit.Status.ToString());
                }
                else
                {
                    throw new BusinessException(nameof(ErrorCode.MES16310));
                }
            }
            //验证在制信息
            if (manuSfcProduceEntit.ProcedureId != beginRepairDto.ProcedureId || manuSfcProduceEntit.Status != SfcProduceStatusEnum.lineUp)
            {
                throw new BusinessException(nameof(ErrorCode.MES16308));
            }
            //更新状态

            // 更改状态，将条码由"排队"改为"活动"
            manuSfcProduceEntit.Status = SfcProduceStatusEnum.lineUp;
            manuSfcProduceEntit.UpdatedBy = _currentUser.UserName;
            manuSfcProduceEntit.UpdatedOn = HymsonClock.Now();
            var manuSfcProduceUpdate = await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntit);
            if (manuSfcProduceUpdate <= 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES17304));
            }
            #endregion

            #region 获取展示信息 
            ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new ManuFacePlateRepairOpenInfoDto();

            //产品信息
            //工单
            var planWorkOrderEntit = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntit.WorkOrderId);
            if (planWorkOrderEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17313));
            }
            //工序
            var procProcedureEntit = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntit.ProcedureId);
            if (procProcedureEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17311));
            }
            //产品
            var procMaterialEntit = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntit.ProductId);
            if (procMaterialEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17314));
            }
            var model = new ManuFacePlateRepairProductInfoDto
            {
                SFC = manuSfcProduceEntit.SFC,
                Status = manuSfcProduceEntit.Status.ToString(),
                ProcedureCode = procProcedureEntit.Code,
                OrderCode = planWorkOrderEntit.OrderCode,
                MaterialCode = procMaterialEntit.MaterialCode,
                Version = procMaterialEntit.Version,
                MaterialName = procMaterialEntit.MaterialName,
            };
            manuFacePlateRepairOpenInfoDto.productInfo = model;

            //获取不合格信息
            var query = new ManuProductBadRecordQuery
            {
                SFC = manuSfcProduceEntit.SFC,
                Status = ProductBadRecordStatusEnum.Open,
                SiteId = _currentSite.SiteId ?? 0
            };
            var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query);
            if (manuProductBads == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17316));
            }
            foreach (var item in manuProductBads)
            {
                manuFacePlateRepairOpenInfoDto.productBadInfo.Add(new ManuFacePlateRepairProductBadInfoDto
                {
                    BadRecordId = item.Id,
                    UnqualifiedId = item.UnqualifiedId,
                    UnqualifiedCode = item.UnqualifiedCode,
                    UnqualifiedCodeName = item.UnqualifiedCodeName,
                    ResCode = item.ResCode,
                    IsClose = item.Status,
                });
            }

            //返回工序信息
            //工艺路线
            //var procProcessRouteEntit = _procProcessRouteRepository.GetByIdAsync(procMaterialEntit.ProcessRouteId ?? 0);
            //工艺路线节点
            var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = procMaterialEntit.ProcessRouteId ?? 0 });
            foreach (var itemNode in procProcessRouteNodeList)
            {
                procProcedureEntit = await _procProcedureRepository.GetByIdAsync(itemNode.ProcedureId);
                if (procProcedureEntit.IsRepairReturn == '0')
                {

                }
                manuFacePlateRepairOpenInfoDto.returnProcedureInfo.Add(new ManuFacePlateRepairReturnProcedureDto
                {
                    ProcedureId = itemNode.ProcedureId,
                    ProcedureCode = procProcedureEntit.Code
                });
            }

            return manuFacePlateRepairOpenInfoDto;
            #endregion
        }

        /// <summary>
        /// 获取展示信息 
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<ManuFacePlateRepairOpenInfoDto> GetManuFacePlateRepairOpenInfoDto(string sfc)
        {

            ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new ManuFacePlateRepairOpenInfoDto();
            //获取产品信息
            var manuSfcProduce = await _manuSfcProduceRepository.GetPagedInfoAsync(new ManuSfcProducePagedQuery { PageSize = 1, PageIndex = 1, Sfc = sfc });
            var manuSfcProduceInfo = manuSfcProduce.Data.FirstOrDefault();
            if (manuSfcProduceInfo == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17306));
            }
            var productInfo = new ManuFacePlateRepairProductInfoDto
            {
                SFC = manuSfcProduceInfo.Sfc,
                Status = manuSfcProduceInfo.Status.ToString(),
                ProcedureCode = manuSfcProduceInfo.Code,
                OrderCode = manuSfcProduceInfo.OrderCode,
                MaterialCode = manuSfcProduceInfo.MaterialCode,
                Version = manuSfcProduceInfo.Version,
                MaterialName = manuSfcProduceInfo.MaterialName,
            };
            manuFacePlateRepairOpenInfoDto.productInfo = productInfo;

            return manuFacePlateRepairOpenInfoDto;
        }


        /// <summary>
        /// 结束维修
        /// </summary>
        /// <param name="beginRepairDto"></param>
        /// <returns></returns>
        public async Task EndManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {

            #region 调用作业 
            //调用作业

            #endregion
        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="confirmSubmitDto"></param>
        /// <returns></returns>
        public async Task ConfirmSubmitManuFacePlateRepairAsync(ManuFacePlateRepairConfirmSubmitDto confirmSubmitDto)
        {
            #region 验证数据  

            if (string.IsNullOrWhiteSpace(confirmSubmitDto.SFC))
            {
                throw new BusinessException(nameof(ErrorCode.MES17303));
            }
            //检查缺陷是否关闭
            var isCloseALL = confirmSubmitDto.confirmSubmitDetail.Where(it => it.IsClose == ProductBadRecordStatusEnum.Open).Any();
            if (isCloseALL)
            {
                throw new BusinessException(nameof(ErrorCode.MES17307));
            }
            //获取面版
            var facePlateEntit = _manuFacePlateRepairRepository.GetByFacePlateIdAsync(confirmSubmitDto.FacePlateId);
            if (facePlateEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17309));
            }
            #endregion

            #region 更新不良信息
            var badRecordsList = new List<ManuProductBadRecordEntity>();
            var manuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            foreach (var item in confirmSubmitDto.confirmSubmitDetail)
            {
                var badRecordEntit = await _manuProductBadRecordRepository.GetByIdAsync(item.BadRecordId);
                if (badRecordEntit == null)
                {
                    throw new BusinessException(nameof(ErrorCode.MES17316));
                }
                badRecordEntit.Status = ProductBadRecordStatusEnum.Close;
                ManuSfcRepairDetailEntity manuSfcRepairDetailEntity = new ManuSfcRepairDetailEntity();
                manuSfcRepairDetailEntity.SfcRepairId = facePlateEntit.Id;
                manuSfcRepairDetailEntity.ProductBadId = item.BadRecordId;
                manuSfcRepairDetailEntity.RepairMethod = item.RepairMethod;
                manuSfcRepairDetailEntity.CauseAnalyse = item.CauseAnalyse;
                manuSfcRepairDetailEntity.IsClose = ManuSfcRepairDetailIsCloseEnum.Close;

                manuSfcRepairDetailEntity.Id = IdGenProvider.Instance.CreateId();
                manuSfcRepairDetailEntity.CreatedBy = _currentUser.UserName;
                manuSfcRepairDetailEntity.UpdatedBy = _currentUser.UserName;
                manuSfcRepairDetailEntity.CreatedOn = HymsonClock.Now();
                manuSfcRepairDetailEntity.UpdatedOn = HymsonClock.Now();
                manuSfcRepairDetailEntity.SiteId = _currentSite.SiteId ?? 0;

                badRecordsList.Add(badRecordEntit);
                manuSfcRepairDetailList.Add(manuSfcRepairDetailEntity);
            }

            #endregion

            #region 返回工序
            //获取条码生产信息
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(confirmSubmitDto.SFC);
            if (manuSfcProduceEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17306));
            }
            manuSfcProduceEntit.ProcedureId = confirmSubmitDto.ReturnProcedureId;
            //var manuSfcProduceUpdate = await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntit);
            //if (manuSfcProduceUpdate <= 0)
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES17308));
            //}
            #endregion

            #region 事务入库

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _manuProductBadRecordRepository.UpdateRangeAsync(badRecordsList);
                rows += await _manuFacePlateRepairRepository.InsertsDetailAsync(manuSfcRepairDetailList);
                rows += await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntit);

                trans.Complete();
            }
            if (rows == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES17310));
            }
            #endregion
        }


        /// <summary>
        /// 获取初始信息
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairInitialInfoDto> GetInitialInfoManuFacePlateRepairAsync(long facePlateId)
        {
            //获取面版
            var facePlateEntit = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(facePlateId);
            if (facePlateEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17309));
            }
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(facePlateEntit.ProcedureId);
            if (facePlateEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17311));
            }
            var procResourceEntity = await _procResourceRepository.GetByIdAsync(facePlateEntit.ResourceId);
            if (facePlateEntit == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES17312));
            }
            ManuFacePlateRepairInitialInfoDto manuFacePlateRepairOpenInfoView = new ManuFacePlateRepairInitialInfoDto();
            manuFacePlateRepairOpenInfoView.ProcedureId = procProcedureEntity.Id;
            manuFacePlateRepairOpenInfoView.ProcedureCode = procProcedureEntity.Code;

            manuFacePlateRepairOpenInfoView.ResourceId = procResourceEntity.Id;
            manuFacePlateRepairOpenInfoView.ResourceCode = procResourceEntity.ResCode;
            return manuFacePlateRepairOpenInfoView;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateRepairCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateRepairAsync(ManuFacePlateRepairCreateDto manuFacePlateRepairCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateRepairCreateDto);

            //DTO转换实体
            var manuFacePlateRepairEntity = manuFacePlateRepairCreateDto.ToEntity<ManuFacePlateRepairEntity>();
            manuFacePlateRepairEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateRepairEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuFacePlateRepairRepository.InsertAsync(manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateRepairAsync(long id)
        {
            await _manuFacePlateRepairRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateRepairAsync(long[] ids)
        {
            return await _manuFacePlateRepairRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlateRepairPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateRepairDto>> GetPagedListAsync(ManuFacePlateRepairPagedQueryDto manuFacePlateRepairPagedQueryDto)
        {
            var manuFacePlateRepairPagedQuery = manuFacePlateRepairPagedQueryDto.ToQuery<ManuFacePlateRepairPagedQuery>();
            var pagedInfo = await _manuFacePlateRepairRepository.GetPagedInfoAsync(manuFacePlateRepairPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateRepairDto> manuFacePlateRepairDtos = PrepareManuFacePlateRepairDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateRepairDto>(manuFacePlateRepairDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateRepairDto> PrepareManuFacePlateRepairDtos(PagedInfo<ManuFacePlateRepairEntity> pagedInfo)
        {
            var manuFacePlateRepairDtos = new List<ManuFacePlateRepairDto>();
            foreach (var manuFacePlateRepairEntity in pagedInfo.Data)
            {
                var manuFacePlateRepairDto = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                manuFacePlateRepairDtos.Add(manuFacePlateRepairDto);
            }

            return manuFacePlateRepairDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateRepairDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateRepairAsync(ManuFacePlateRepairModifyDto manuFacePlateRepairModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateRepairModifyDto);

            //DTO转换实体
            var manuFacePlateRepairEntity = manuFacePlateRepairModifyDto.ToEntity<ManuFacePlateRepairEntity>();
            manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();

            await _manuFacePlateRepairRepository.UpdateAsync(manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairDto> QueryManuFacePlateRepairByIdAsync(long id)
        {
            var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByIdAsync(id);
            if (manuFacePlateRepairEntity != null)
            {
                return manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
            }
            return null;
        }
    }
}
