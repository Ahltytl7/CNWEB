using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace CNWEB.Areas.Admin.Controllers
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            return data == null ? default : JsonConvert.DeserializeObject<T>(data);
        }
    }
}
