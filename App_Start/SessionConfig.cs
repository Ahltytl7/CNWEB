using CNWEB.Models;
using Newtonsoft.Json;

namespace CNWEB.App_Start
{
    public class SessionConfig
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionConfig(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public SessionConfig()
        {
        }

        //1. Lưu user
        public void SetUser(User user)
        {
            //lưu vào session
            _httpContextAccessor.HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
        }
        //2. Lay session
        public User GetUser()
        {
            //lưu vào session
            var userJson = _httpContextAccessor.HttpContext.Session.GetString("user");
            return userJson != null ? JsonConvert.DeserializeObject<User>(userJson) : null;
        }
    }
}
