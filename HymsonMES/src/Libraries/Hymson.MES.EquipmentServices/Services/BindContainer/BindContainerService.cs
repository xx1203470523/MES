using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.BindContainer
{
    /// <summary>
    /// 容器绑定服务
    /// </summary>
    public class BindContainerService : IBindContainerService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<BindContainerRequest> _validationBindContainerRules;
        private readonly AbstractValidator<UnBindContainerRequest> _validationUnBindContainerRules;

        private readonly IManuTrayLoadRepository _manuTrayLoadRepository;
        private readonly IManuTraySfcRecordRepository _manuTraySfcRecordRepository;
        private readonly IManuTraySfcRelationRepository _manuTraySfcRelationRepository;


        public BindContainerService(AbstractValidator<BindContainerRequest> validationBindContainerRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindContainerRequest> validationUnBindContainerRules,
           IManuTraySfcRecordRepository manuTraySfcRecordRepository, IManuTraySfcRelationRepository manuTraySfcRelationRepository, IManuTrayLoadRepository manuTrayLoadRepository)
        {
            _validationBindContainerRules = validationBindContainerRules;
            _currentEquipment = currentEquipment;
            _validationUnBindContainerRules = validationUnBindContainerRules;
            _manuTraySfcRecordRepository = manuTraySfcRecordRepository;
            _manuTraySfcRelationRepository = manuTraySfcRelationRepository;
            _manuTrayLoadRepository = manuTrayLoadRepository;
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task BindContainerAsync(BindContainerRequest bindContainerRequest)
        {
            await _validationBindContainerRules.ValidateAndThrowAsync(bindContainerRequest);
            //查找绑定容器
            var inteTrayLoad = await _manuTrayLoadRepository.GetByTrayCodeAsync(bindContainerRequest.ContainerCode);
            //容器不存在
            if (inteTrayLoad == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES19104));
            }
            List<ManuTraySfcRelationEntity> traySfcRelations = new List<ManuTraySfcRelationEntity>();
            List<ManuTraySfcRecordEntity> traySfcRecord = new List<ManuTraySfcRecordEntity>();
            foreach (var item in bindContainerRequest.ContainerSFCs)
            {
                //容器绑定关系
                ManuTraySfcRelationEntity sfc = new()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = item.SFC,
                    TrayLoadId = inteTrayLoad.Id,
                    Seq = item.Location,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                };
                traySfcRelations.Add(sfc);
                //容器绑定记录
                ManuTraySfcRecordEntity sfcRecord = new()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    Tray = inteTrayLoad.TrayCode,
                    OperationType = ManuTraySfcRecordOperationTypeEnum.Bind,
                    SFC = item.SFC,
                    Seq = item.Location,
                    LoadQty = 1,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                };
                traySfcRecord.Add(sfcRecord);
            }
            //更新托盘信息
            inteTrayLoad.UpdatedBy = _currentEquipment.Name;
            inteTrayLoad.UpdatedOn = HymsonClock.Now();
            //计算绑定数量
            inteTrayLoad.LoadQty = traySfcRelations.Count;

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuTrayLoadRepository.UpdateAsync(inteTrayLoad);
                await _manuTraySfcRelationRepository.InsertsAsync(traySfcRelations);
                await _manuTraySfcRecordRepository.InsertsAsync(traySfcRecord);
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindContainerRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UnBindContainerAsync(UnBindContainerRequest unBindContainerRequest)
        {
            await _validationUnBindContainerRules.ValidateAndThrowAsync(unBindContainerRequest);
            //查找绑定容器
            var inteTrayLoad = await _manuTrayLoadRepository.GetByTrayCodeAsync(unBindContainerRequest.ContainerCode);
            //容器不存在
            if (inteTrayLoad == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES19104));
            }
            //查找已装载记录
            var trayLoads = await _manuTraySfcRelationRepository.GetByTrayLoadIdAsync(inteTrayLoad.Id);
            if (trayLoads == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES19105));
            }

            //需要解绑的SFC
            var unBindSFCs = trayLoads.Where(c => unBindContainerRequest.ContainerSFCs.Contains(c.SFC));
            List<long> idsList = new List<long>();
            List<ManuTraySfcRecordEntity> traySfcRecord = new List<ManuTraySfcRecordEntity>();
            foreach (var item in unBindSFCs)
            {
                //容器绑定记录
                ManuTraySfcRecordEntity sfcRecord = new()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    Tray = inteTrayLoad.TrayCode,
                    OperationType = ManuTraySfcRecordOperationTypeEnum.UnBind,
                    SFC = item.SFC,
                    Seq = item.Seq,
                    LoadQty = 1,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                };
                traySfcRecord.Add(sfcRecord);
                idsList.Add(item.Id);
            }
            //更新托盘信息
            inteTrayLoad.UpdatedBy = _currentEquipment.Name;
            inteTrayLoad.UpdatedOn = HymsonClock.Now();
            //计算绑定数量
            inteTrayLoad.LoadQty = trayLoads.Count() - unBindSFCs.Count();

            //删除
            var command = new DeleteCommand
            {
                UserId = _currentEquipment.Name,
                DeleteOn = HymsonClock.Now(),
                Ids = idsList.ToArray()
            };
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuTrayLoadRepository.UpdateAsync(inteTrayLoad);
                await _manuTraySfcRelationRepository.DeleteTruesAsync(command);
                await _manuTraySfcRecordRepository.InsertsAsync(traySfcRecord);
                //提交
                ts.Complete();
            }
        }
    }
}