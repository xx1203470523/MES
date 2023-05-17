using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
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
        private readonly AbstractValidator<BindSFCDto> _validationBindDtoRules;
        private readonly AbstractValidator<UnBindSFCDto> _validationUnBindDtoRules;
        private readonly IManuSfcBindRecordRepository _manuSfcBindRecordRepository;
        private readonly IManuSfcBindRepository _manuSfcBindRepository;

        public BindSFCService(AbstractValidator<BindSFCDto> validationBindDtoRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindSFCDto> validationUnBindDtoRules, IManuSfcBindRecordRepository manuSfcBindRecordRepository, IManuSfcBindRepository manuSfcBindRepository)
        {
            _validationBindDtoRules = validationBindDtoRules;
            _currentEquipment = currentEquipment;
            _validationUnBindDtoRules = validationUnBindDtoRules;
            _manuSfcBindRecordRepository = manuSfcBindRecordRepository;
            _manuSfcBindRepository = manuSfcBindRepository;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        public async Task BindSFCAsync(BindSFCDto bindSFCDto)
        {
            //验证参数
            await _validationBindDtoRules.ValidateAndThrowAsync(bindSFCDto);
            List<ManuSfcBindEntity> sfcBindList = new();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
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
        public async Task UnBindSFCAsync(UnBindSFCDto unBindSFCDto)
        {
            //验证参数
            await _validationUnBindDtoRules.ValidateAndThrowAsync(unBindSFCDto);
            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCDto.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new ValidationException(nameof(ErrorCode.MES19106));
            }

            //需要解绑的SFC
            var unBindSFCs = bindSfcs.Where(c => unBindSFCDto.BindSFCs.Contains(c.BindSFC));
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
            //    //await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
            //    //提交
            //    ts.Complete();
            //}


        }
    }
}
