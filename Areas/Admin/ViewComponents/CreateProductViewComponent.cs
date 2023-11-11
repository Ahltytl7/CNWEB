using Microsoft.AspNetCore.Mvc;
using CNWEB.Models;
using System.Collections.Generic;

namespace CNWEB.Areas.Admin.ViewComponents
{
    public class CreateProductViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
