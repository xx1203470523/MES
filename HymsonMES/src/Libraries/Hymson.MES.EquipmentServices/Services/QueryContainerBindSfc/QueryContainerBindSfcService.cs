using FluentValidation;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc
{
    /// <summary>
    /// 容器绑定条码查询服务
    /// </summary>
    public class QueryContainerBindSfcService : IQueryContainerBindSfcService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IManuTraySfcRelationRepository _manuTraySfcRelationRepository;
        private readonly AbstractValidator<QueryContainerBindSfcRequest> _validationQueryContainerBindSfcRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationQueryContainerBindSfcRequestRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="manuTraySfcRelationRepository"></param>
        public QueryContainerBindSfcService(
            IManuTraySfcRelationRepository manuTraySfcRelationRepository,
        AbstractValidator<QueryContainerBindSfcRequest> validationQueryContainerBindSfcRequestRules, ICurrentEquipment currentEquipment)
        {
            _manuTraySfcRelationRepository = manuTraySfcRelationRepository;
            _validationQueryContainerBindSfcRequestRules = validationQueryContainerBindSfcRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 容器绑定条码查询
        /// </summary>
        /// <param name="queryContainerBindSfcRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<QueryContainerBindSfcReaponse>> QueryContainerBindSfcAsync(QueryContainerBindSfcRequest queryContainerBindSfcRequest)
        {
            await _validationQueryContainerBindSfcRequestRules.ValidateAndThrowAsync(queryContainerBindSfcRequest);
            var manuTraySfcRelationEntits = await _manuTraySfcRelationRepository.GetManuTraySfcRelationByTrayCodeAsync(new ManuTraySfcRelationByTrayCodeQuery { TrayCode = queryContainerBindSfcRequest.ContaineCode, SiteId = _currentEquipment.SiteId });
            List<QueryContainerBindSfcReaponse> list = new List<QueryContainerBindSfcReaponse>();
            foreach (var item in manuTraySfcRelationEntits)
            {
                list.Add(new QueryContainerBindSfcReaponse
                {
                    SFC = item.SFC,
                    Location = (item.Seq ?? 0).ToString(),
                });
            }
            return list;
        }
    }
}
