﻿using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
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
        private readonly AbstractValidator<BindContainerDto> _validationBindContainerRules;
        private readonly AbstractValidator<UnBindContainerDto> _validationUnBindContainerRules;

        private readonly IManuTrayLoadRepository _manuTrayLoadRepository;
        private readonly IManuTraySfcRecordRepository _manuTraySfcRecordRepository;
        private readonly IManuTraySfcRelationRepository _manuTraySfcRelationRepository;


        public BindContainerService(AbstractValidator<BindContainerDto> validationBindContainerRules, ICurrentEquipment currentEquipment, AbstractValidator<UnBindContainerDto> validationUnBindContainerRules,
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
        /// <param name="bindContainerDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task BindContainerAsync(BindContainerDto bindContainerDto)
        {
            await _validationBindContainerRules.ValidateAndThrowAsync(bindContainerDto);
            //查找绑定容器
            var inteTrayLoad = await _manuTrayLoadRepository.GetByTrayCodeAsync(bindContainerDto.ContainerCode);
            //容器不存在
            if (inteTrayLoad == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19104));
            }
            var sfcs = bindContainerDto.ContainerSFCs.Select(c => c.SFC);
            var existsBindSfc = await _manuTraySfcRelationRepository.GetByTrayLoadSFCAsync(inteTrayLoad.Id, sfcs.ToArray());
            if (existsBindSfc.Any())
            {
                var sfcStr = string.Join(",", existsBindSfc.Select(c => c.SFC));
                throw new CustomerValidationException(nameof(ErrorCode.MES19122)).WithData("ContainerCode", inteTrayLoad.TrayCode)
                    .WithData("SFC", sfcStr);
            }

            List<ManuTraySfcRelationEntity> traySfcRelations = new List<ManuTraySfcRelationEntity>();
            List<ManuTraySfcRecordEntity> traySfcRecord = new List<ManuTraySfcRecordEntity>();
            foreach (var item in bindContainerDto.ContainerSFCs)
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
        /// <param name="unBindContainerDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UnBindContainerAsync(UnBindContainerDto unBindContainerDto)
        {
            await _validationUnBindContainerRules.ValidateAndThrowAsync(unBindContainerDto);
            //查找绑定容器
            var inteTrayLoad = await _manuTrayLoadRepository.GetByTrayCodeAsync(unBindContainerDto.ContainerCode);
            //容器不存在
            if (inteTrayLoad == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19104));
            }
            //查找已装载记录
            var trayLoads = await _manuTraySfcRelationRepository.GetByTrayLoadIdAsync(inteTrayLoad.Id);
            if (trayLoads == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19105));
            }

            //需要解绑的SFC
            var unBindSFCs = trayLoads.Where(c => unBindContainerDto.ContainerSFCs.Contains(c.SFC));
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