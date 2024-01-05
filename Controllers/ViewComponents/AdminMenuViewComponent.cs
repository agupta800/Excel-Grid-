//using Microsoft.AspNetCore.Mvc;

//namespace AsterWebApp.Controllers.ViewComponents
//{
//    public class AdminMenuViewComponent : ViewComponent
//    {
//        //private readonly AsterCommonContext db;
//        private readonly IMenuService _menuService;

//        public AdminMenuViewComponent(IMenuService context) => _menuService = context;

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var items = _menuService.GetMenuViewModels("Admin");
//            return View(items);
//        }

//    }
//}
