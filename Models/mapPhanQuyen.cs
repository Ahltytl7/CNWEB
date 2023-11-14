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
    }
}
