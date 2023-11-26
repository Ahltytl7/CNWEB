namespace CNWEB.Models
{
    public class mapTaiKhoan
    {
        private readonly WebContext _context;

        public mapTaiKhoan()
        {

        }

        public mapTaiKhoan(WebContext context)
        {
            _context = context;

        }
        public User? TimKiem(string username, string password) {
            var user = _context.Users.Where(m => m.Username == username & m.Password == password).ToList();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
             
        }
        public User? TimKiem(string id)
        {
            if (_context == null)
            {
                // Handle the scenario where _context is null
                return null;
            }

            var user = _context.Users.Find(id);
            return user;
        }
    }
}
