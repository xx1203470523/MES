using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.BindSFC;
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
        private readonly AbstractValidator<BindSFCRequest> _validationBindRequestRules;
        private readonly AbstractValidator<UnBindSFCRequest> _validationUnBindRequestRules;
        private readonly IManuSfcBindRecordRepository _manuSfcBindRecordRepository;
        private readonly IManuSfcBindRepository _manuSfcBindRepository;

        public BindSFCService(AbstractValidator<BindSFCRequest> validationBindRequestRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindSFCRequest> validationUnBindRequestRules, IManuSfcBindRecordRepository manuSfcBindRecordRepository, IManuSfcBindRepository manuSfcBindRepository)
        {
            _validationBindRequestRules = validationBindRequestRules;
            _currentEquipment = currentEquipment;
            _validationUnBindRequestRules = validationUnBindRequestRules;
            _manuSfcBindRecordRepository = manuSfcBindRecordRepository;
            _manuSfcBindRepository = manuSfcBindRepository;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCRequest"></param>
        /// <returns></returns>
        public async Task BindSFCAsync(BindSFCRequest bindSFCRequest)
        {
            //验证参数
            await _validationBindRequestRules.ValidateAndThrowAsync(bindSFCRequest);
            List<ManuSfcBindEntity> sfcBindList = new();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            foreach (var item in bindSFCRequest.BindSFCs)
            {
                sfcBindList.Add(new ManuSfcBindEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bindSFCRequest.SFC,
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
                    SFC = bindSFCRequest.SFC,
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
        /// <param name="unBindSFCRequest"></param>
        /// <returns></returns>
        public async Task UnBindSFCAsync(UnBindSFCRequest unBindSFCRequest)
        {
            //验证参数
            await _validationUnBindRequestRules.ValidateAndThrowAsync(unBindSFCRequest);
            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCRequest.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new ValidationException(nameof(ErrorCode.MES19106));
            }

            //需要解绑的SFC
            var unBindSFCs = bindSfcs.Where(c => unBindSFCRequest.BindSFCs.Contains(c.BindSFC));
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
                    SFC = unBindSFCRequest.SFC,
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
            //    //await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
            //    //提交
            //    ts.Complete();
            //}


        }
    }
}
