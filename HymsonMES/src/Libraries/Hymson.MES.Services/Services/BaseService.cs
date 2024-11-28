using Hymson.Infrastructure;
using Hymson.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services;

public abstract class BaseService
{

    protected static T FillCreateCommand<T>(T entity,string? createdBy = "System") where T : BaseEntity
    {
        if (entity == null) return entity;

        entity.Id = IdGenProvider.Instance.CreateId();
        entity.CreatedOn = DateTime.Now;
        entity.CreatedBy = entity.CreatedBy;


        return entity;
    }

}
