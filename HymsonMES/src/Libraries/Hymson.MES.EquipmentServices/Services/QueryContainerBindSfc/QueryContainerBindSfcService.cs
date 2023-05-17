using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.QueryContainerBindSfc;
using Hymson.MES.Data.Repositories.Manufacture;
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

        private readonly AbstractValidator<QueryContainerBindSfcDto> _validationQueryContainerBindSfcDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationQueryContainerBindSfcDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public QueryContainerBindSfcService(IManuTraySfcRelationRepository manuTraySfcRelationRepository, AbstractValidator<QueryContainerBindSfcRequest> validationQueryContainerBindSfcRequestRules, ICurrentEquipment currentEquipment)
        {

            _manuTraySfcRelationRepository = manuTraySfcRelationRepository; 

            _validationQueryContainerBindSfcDtoRules = validationQueryContainerBindSfcDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 容器绑定条码查询
        /// </summary>
        /// <param name="queryContainerBindSfcDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<QueryContainerBindSfcReaponse>> QueryContainerBindSfcAsync(QueryContainerBindSfcDto queryContainerBindSfcDto)
        {
            await _validationQueryContainerBindSfcDtoRules.ValidateAndThrowAsync(queryContainerBindSfcDto);
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
