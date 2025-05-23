using Microsoft.AspNetCore.Mvc;

namespace StudyTracker.Controllers
{
    public class HomeController : Controller
    {
        // При заходе на главную страницу редиректим на список курсов
        //public IActionResult Index()
        //{
        //    return RedirectToAction("Index", "Course");
        //}

        // Страница Privacy оставляем, если нужна
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        // Стандартный обработчик ошибок
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(); // Или передать модель ошибки, если есть
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "MyCourses");
                }
                else if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction("Index", "Course");
                }
            }
            return View();
        }
    }
}
