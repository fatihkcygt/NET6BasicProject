using KUSYS_Demo.Data.Context;
using KUSYS_Demo.Data.Interfaces;
using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KUSYS_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<User, StudentCourseContext> _userRepository;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<User, StudentCourseContext> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username, string password, string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            var user = await _userRepository.GetOneByWhereList(x => x.Username == username);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return View("login");
            }
            else
            {
                if (new PasswordHasher<object>().VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));

                    var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var claimsprincipal = new ClaimsPrincipal(claimsidentity);

                    var expiresetprop = new AuthenticationProperties
                    {
                        ExpiresUtc = default(DateTime?)
                    };

                    await HttpContext.SignInAsync(claimsprincipal, expiresetprop);
                    return Redirect(ReturnUrl);

                }

            }
            TempData["Error"] = "Şifre yanlış.";
            return View("login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}