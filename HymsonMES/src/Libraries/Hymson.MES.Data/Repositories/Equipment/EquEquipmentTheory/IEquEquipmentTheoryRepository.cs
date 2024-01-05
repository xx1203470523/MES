using Hymson.MES.Core.Domain.Equipment;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment;

public interface IEquEquipmentTheoryRepository
{

    /// <summary>
    /// 新增理论产能
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<int> InsertAsync(EquEquipmentTheoryCreateCommand command);

    /// <summary>
    /// 更新理论产能
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<int> UpdateAsync(EquEquipmentTheoryUpdateCommand command);

    /// <summary>
    /// 更新理论产能
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<int> InsertOrUpdateAsync(EquEquipmentTheoryUpdateCommand command);

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<IEnumerable<EquEquipmentTheoryEntity>> GetListAsync(EquEquipmentTheoryQuery query);

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public Task<EquEquipmentTheoryEntity> GetOneAsync(EquEquipmentTheoryQuery query);
}
