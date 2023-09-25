using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码绑定服务
    /// </summary>
    public class BindSFCService : IBindSFCService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<BindSFCInputDto> _validationBindDtoRules;
        private readonly AbstractValidator<UnBindSFCInputDto> _validationUnBindDtoRules;
        private readonly IManuSfcBindRecordRepository _manuSfcBindRecordRepository;
        private readonly IManuSfcBindRepository _manuSfcBindRepository;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationUnBindDtoRules"></param>
        /// <param name="manuSfcBindRecordRepository"></param>
        /// <param name="manuSfcBindRepository"></param>
        /// <param name="currentUser"></param>
        public BindSFCService(
            ICurrentUser currentUser,
            ICurrentEquipment currentEquipment,
            IManuSfcBindRecordRepository manuSfcBindRecordRepository,
            IManuSfcBindRepository manuSfcBindRepository,
            AbstractValidator<BindSFCInputDto> validationBindDtoRules,
            AbstractValidator<UnBindSFCInputDto> validationUnBindDtoRules)

        {
            _validationBindDtoRules = validationBindDtoRules;
            _currentEquipment = currentEquipment;
            _validationUnBindDtoRules = validationUnBindDtoRules;
            _manuSfcBindRecordRepository = manuSfcBindRecordRepository;
            _manuSfcBindRepository = manuSfcBindRepository;
            _currentUser = currentUser;
        }


        /// <summary>
        /// 根据SFC或BindSFC查询绑定SFC
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<BindSFCOutputDto> GetBindSFC(BindSFCInputDto bindSFCDto)
        {
            var result = new BindSFCOutputDto();

            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(bindSFCDto.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }


            result.Data = bindSfcs;

            //TODO NG位置给一下呗
            result.NGLocationId = 0;


            return result;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        public async Task BindSFCAsync(UnBindSFCInputDto bindSFCDto)
        {
            //验证参数
            await _validationBindDtoRules.ValidateAndThrowAsync(bindSFCDto);

            List<ManuSfcBindEntity> sfcBindList = new();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();

            var existsBindSfc = await _manuSfcBindRepository.GetByBindSFCAsync(bindSFCDto.SFC, bindSFCDto.BindSFCs);
            if (existsBindSfc.Any())
            {
                var bindSfcs = string.Join(",", existsBindSfc.Select(c => c.BindSFC));
                throw new CustomerValidationException(nameof(ErrorCode.MES19121)).WithData("SFC", bindSFCDto.SFC).WithData("BindSFC", bindSfcs);
            }

            foreach (var item in bindSFCDto.BindSFCs)
            {
                sfcBindList.Add(new ManuSfcBindEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bindSFCDto.SFC,
                    BindSFC = item,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    Status = ManuSfcBindStatusEnum.Bind,
                    BindingTime = HymsonClock.Now(),
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
                sfcBindRecordList.Add(new ManuSfcBindRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bindSFCDto.SFC,
                    BindSFC = item,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    OperationType = ManuSfcBindStatusEnum.Bind,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcBindRepository.InsertsAsync(sfcBindList);
                //绑定记录备用
                //await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCDto"></param>
        /// <returns></returns>
        public async Task UnBindSFCAsync(UnBindSFCInputDto unBindSFCDto)
        {
            //验证参数
            //await _validationUnBindDtoRules.ValidateAndThrowAsync(unBindSFCDto);
            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCDto.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            //需要解绑的SFC
            var unBindSFCs = bindSfcs.Where(c => unBindSFCDto.BindSFCs.Where(p => p.ToUpper().Equals(c.BindSFC.ToUpper())).Any());
            List<long> idsList = new List<long>();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            foreach (var item in unBindSFCs)
            {
                //更新解绑信息
                item.UnbindingTime = HymsonClock.Now();
                item.Status = ManuSfcBindStatusEnum.UnBind;
                item.UpdatedBy = _currentEquipment.Name;
                item.UpdatedOn = HymsonClock.Now();

                //解绑记录
                sfcBindRecordList.Add(new ManuSfcBindRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = unBindSFCDto.SFC,
                    BindSFC = item.BindSFC,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    OperationType = ManuSfcBindStatusEnum.UnBind,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
                idsList.Add(item.Id);
            }
            //删除
            var command = new DeleteCommand
            {
                UserId = _currentEquipment.Name,
                DeleteOn = HymsonClock.Now(),
                Ids = idsList.ToArray()
            };
            await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());
            //using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            //{
            //    await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());
            //    //await _manuSfcBindRepository.DeleteTruesAsync(command);
            //    //绑定记录备用 如果解绑需要删除绑定数据使用
            await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
            //    //提交
            //    ts.Complete();
            //}


        }


        /// <summary>
        /// PDA全部解绑
        /// </summary>
        /// <param name="unBindSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task UnBindPDAAsync(UnBindSFCInput unBindSFCDto)
        {
            //验证参数

            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCDto.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            //需要解绑的SFC
            var unBindSFCs = bindSfcs.Where(c => unBindSFCDto.BindSFCs.Where(p => p.ToUpper().Equals(c.BindSFC.ToUpper())).Any());
            List<long> idsList = new List<long>();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            foreach (var item in unBindSFCs)
            {
                //更新解绑信息
                item.UnbindingTime = HymsonClock.Now();
                item.Status = ManuSfcBindStatusEnum.UnBind;
                item.UpdatedBy = _currentEquipment.Name;
                item.UpdatedOn = HymsonClock.Now();

                //解绑记录
                sfcBindRecordList.Add(new ManuSfcBindRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = unBindSFCDto.SFC,
                    BindSFC = item.BindSFC,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    OperationType = ManuSfcBindStatusEnum.UnBind,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
                idsList.Add(item.Id);
            }
            //删除
            var command = new DeleteCommand
            {
                UserId = _currentEquipment.Name,
                DeleteOn = HymsonClock.Now(),
                Ids = idsList.ToArray()
            };

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());

                await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);

                ts.Complete();
            }

        }


        /// <summary>
        /// 换绑
        /// </summary>
        /// <param name="BindSFCDto"></param>
        /// <returns></returns>
        public async Task SwitchBindSFCAsync(SwitchBindInputDto BindSFCDto)
        {
            string[] BindSFCs = { BindSFCDto.OldBindSFC };
            //验证参数
            //await _validationUnBindDtoRules.ValidateAndThrowAsync(BindSFCDto);
            var existsBindSfc = await _manuSfcBindRepository.GetByBindSFCAsync(BindSFCDto.SFC, BindSFCs);
            if (!existsBindSfc.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19120)).WithData("SFC", BindSFCDto.SFC);
            }

            if (existsBindSfc.Any(x => x.BindSFC == BindSFCDto.OldBindSFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19146)).WithData("SFC", BindSFCDto.SFC);
            }

            var updateEntity = existsBindSfc.FirstOrDefault();

            updateEntity.BindSFC = BindSFCDto.NewBindSFC;
            updateEntity.UpdatedBy = _currentUser.UserName;
            updateEntity.UpdatedOn = HymsonClock.Now();

            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            //换绑记录
            sfcBindRecordList.Add(new ManuSfcBindRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = BindSFCDto.SFC,
                BindSFC = BindSFCDto.NewBindSFC,
                Type = 2,//预留字段
                Location = 0,//预留
                OperationType = ManuSfcBindStatusEnum.Bind,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),                 
            });             

            await _manuSfcBindRepository.UpdateAsync(updateEntity);
            await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);

        }

        /// <summary>
        /// 复投
        /// </summary>
        /// <param name="BindSFCDto"></param>
        /// <returns></returns>
        public async Task RepeatManuSFCAsync(BindSFCInputDto BindSFCDto)
        {
            //解绑
            //await UnBindSFCAsync(BindSFCDto);


            //在制
            //var createManuSfcProduceEntity = new ManuSfcProduceEntity
            //{
            //    Id = IdGenProvider.Instance.CreateId(),
            //    SiteId = _currentEquipment.SiteId,
            //    SFC = sfcBindingDto.SFC,
            //    ProductId = productId,
            //    WorkOrderId = planWorkOrderEntity.Id,
            //    BarCodeInfoId = createManuMainSfcInfoEntity.Id,
            //    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
            //    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
            //    ProductBOMId = planWorkOrderEntity.ProductBOMId,
            //    Qty = 1,
            //    ProcedureId = procedureEntity.Id,
            //    Status = SfcProduceStatusEnum.lineUp,
            //    RepeatedCount = 0,
            //    IsScrap = TrueOrFalseEnum.No,
            //    CreatedBy = _currentEquipment.Name,
            //    UpdatedBy = _currentEquipment.Name
            //};

            //await _manuSfcProduceRepository.InsertAsync(createManuSfcProduceEntity);

            //报废


            //记录
        }

    }
}
