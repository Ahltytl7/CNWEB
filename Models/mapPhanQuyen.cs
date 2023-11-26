namespace CNWEB.Models
{
    public class mapPhanQuyen
    {
        private readonly WebContext _context;
       

        public mapPhanQuyen(WebContext context)
        {
            _context = context;
         
        }

        public mapPhanQuyen()
        {
        }

        public string message = "";
        public bool KiemTra(string idTaiKhoan,string maChucNang)
        {
            var dem = _context.UserRoles.Count(m=>m.UserId== idTaiKhoan && m.RolesId==maChucNang);
            if(dem > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<AppRole> DanhSachChucNang()
        {
            return _context.AppRoles.ToList();
        }
        public bool LuuPhanQuyen(string idTaiKhoan,string maChucNang)
        {
            var role = _context.UserRoles.SingleOrDefault(m => m.UserId == idTaiKhoan & m.RolesId == maChucNang);
            if (role != null)
            {
                _context.UserRoles.Remove(role);
                _context.SaveChanges();
                return true;
            }
            else
            {
                var role2 = new UserRole();
                role2.UserId = idTaiKhoan;
                role2.RolesId = maChucNang;
                _context.UserRoles.Add(role2);
                _context.SaveChanges();
                return true;    
            }
        }
    }
}
