using Microsoft.AspNetCore.Mvc;

namespace StudyTracker.Controllers
{
    public class HomeController : Controller
    {
        // ��� ������ �� ������� �������� ���������� �� ������ ������
        //public IActionResult Index()
        //{
        //    return RedirectToAction("Index", "Course");
        //}

        // �������� Privacy ���������, ���� �����
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        // ����������� ���������� ������
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(); // ��� �������� ������ ������, ���� ����
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
