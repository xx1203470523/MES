using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Extension;

public static class DtoExtension
{
    /// <summary>
    /// 转换到查询对象
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TQuery ToQuery<TQuery>(this QueryDtoAbstraction dto) where TQuery : QueryAbstraction
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return AutoMapperConfiguration.Mapper.Map<TQuery>(dto);
    }
}
