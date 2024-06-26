using Hymson.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients
{
    public static class CommonHttpClient
    {
        public static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new CustomerValidationException(response.StatusCode.ToString(), content);
                }

                throw new NotFoundException(response.StatusCode.ToString(), content);
            }
        }
    }
}
