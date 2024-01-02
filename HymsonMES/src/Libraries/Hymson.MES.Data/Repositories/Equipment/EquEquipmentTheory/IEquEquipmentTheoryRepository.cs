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
    /// 构造函数
    /// </summary>
    /// <returns></returns>
    public bool IEquEquipmentTheoryRepository();

    /// <summary>
    /// 新增理论产能
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<int> InsertAsync(EquEquipmentTheoryCreateCommand command);
}
