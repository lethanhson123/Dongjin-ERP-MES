
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MES.Controllers
{
    public class MAIN_LoginController : Controller
    {
        private readonly IMAIN_LoginService _MAIN_LoginService;
        private readonly CookieHelper _cookieHelper;

        public MAIN_LoginController(IMAIN_LoginService MAIN_LoginService, CookieHelper cookieHelper)
        {
            _MAIN_LoginService = MAIN_LoginService;
            _cookieHelper = cookieHelper;
        }

        public IActionResult Index()
        {
            var BaseParameter = new BaseParameter
            {
                UserID = _cookieHelper.GetCookie(GlobalHelper.UserID) ?? string.Empty,
                Password = string.Empty // Không lưu password vào cookie
            };

            return View(BaseParameter);
        }

        [HttpPost]
        public async Task<IActionResult> OK_Click(BaseParameter BaseParameter)
        {
            string controller = "MAIN_Login";
            string action = "Index";

            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress;
                BaseParameter.IPAddress = ipAddress?.MapToIPv4().ToString() ?? "Unknown";
                BaseParameter.NON_OPER_IDX = GlobalHelper.InitializationNumber;

                var BaseResult = await _MAIN_LoginService.OK_Click(BaseParameter);
                if (BaseResult?.tsuser != null)
                {
                    _cookieHelper.SetCookie(GlobalHelper.MenuParent, "-1");
                    _cookieHelper.SetCookie(GlobalHelper.UserID, BaseResult.tsuser.USER_ID);
                    _cookieHelper.SetCookie(GlobalHelper.USER_IDX, BaseResult.tsuser.USER_IDX?.ToString());
                    _cookieHelper.SetCookie(GlobalHelper.USER_NM, BaseResult.tsuser.USER_NM);
                    _cookieHelper.SetCookie("MC_Name", BaseParameter.MC_NAME);

                    // BẮT ĐỔI PASSWORD
                    if (BaseResult.RequireChangePassword)
                    {
                        TempData["RequireChangePassword"] = true;
                        TempData["Username"] = BaseResult.tsuser.USER_ID;
                        TempData["Reason"] = BaseResult.Message;

                        return RedirectToAction("Index", "MAIN_Login");
                    }

                    return RedirectToAction("Index", "MAIN");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
            }

            return RedirectToAction(action, controller);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordFirstLogin([FromBody] ChangePasswordModel model)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            model.IPAddress = ipAddress?.MapToIPv4().ToString() ?? "Unknown";

            var result = await _MAIN_LoginService.ChangePassword_Click(model);

            return Json(new
            {
                success = result.Message == "Change password Successfully",
                message = result.Message
            });
        }

        public IActionResult Logout()
        {
            _cookieHelper.DeleteCookie(GlobalHelper.UserID);
            _cookieHelper.DeleteCookie(GlobalHelper.USER_IDX);
            _cookieHelper.DeleteCookie(GlobalHelper.USER_NM);
            _cookieHelper.DeleteCookie(GlobalHelper.MenuParent);

            return RedirectToAction("Index", "MAIN_Login");
        }

        public IActionResult ChangeLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
                language = GlobalHelper.LanguageCodeEN;

            CultureInfo culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            _cookieHelper.SetCookie(GlobalHelper.Language, language, 30);

            string refererUrl = Request.GetTypedHeaders().Referer?.ToString() ?? Url.Action("Index", "MAIN");
            return Redirect(refererUrl);
        }
    }
}
