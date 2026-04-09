using HelperMySQL;
using MES_MVC; // CookieHelper
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading;
using ZXing;

namespace MES.Controllers
{
    public class MAINController : Controller
    {
        private readonly IMAINService _MAINService;
        private readonly CookieHelper _cookieHelper;

        public MAINController(IMAINService MAINService, CookieHelper cookieHelper)
        {
            _MAINService = MAINService;
            _cookieHelper = cookieHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangeLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = GlobalHelper.LanguageCodeEN;
            }

            // Đặt văn hóa cho thread hiện tại
            var culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Lưu vào cookie (cookie mã hóa)       
            _cookieHelper.SetCookie(GlobalHelper.Language, language, 30);

            // Trở lại trang trước hoặc về Index
            var refererUrl = Request.GetTypedHeaders()?.Referer?.ToString();
            if (string.IsNullOrEmpty(refererUrl))
                refererUrl = Url.Action("Index", "MAIN");

            return Redirect(refererUrl!);
        }

        public IActionResult ChangeMenuParent(string ParentID)
        {
            if (string.IsNullOrWhiteSpace(ParentID))
            {
                ParentID = "-1";
            }

            // Lưu menu vào cookie

            _cookieHelper.SetCookie(GlobalHelper.MenuParent, ParentID);

            var refererUrl = Request.GetTypedHeaders()?.Referer?.ToString();
            if (string.IsNullOrEmpty(refererUrl))
                refererUrl = Url.Action("Index", "MAIN");

            return Redirect(refererUrl!);
        }

        public IActionResult Logout()
        {
            var STOP_MC = _cookieHelper.GetCookie("MC_Name");
            var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var C_USER = _cookieHelper.GetCookie(GlobalHelper.USER_IDX);
            // xử lý báo no worker
            string sql = @"INSERT INTO `TSNON_OPER` (`TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_STIME`, `CREATE_DTM`, `CREATE_USER`, `TSNON_OPER_CODE`, `REMARK`) VALUES 
                        ('" + STOP_MC + "',(SELECT User_ID FROM tsuser WHERE User_IDX =  '" + C_USER + "'), DATE_FORMAT(NOW(), '%Y-%m-%d'), NOW(), '" + ST_TM + "', (SELECT User_ID FROM tsuser WHERE User_IDX =  '" + C_USER + "'), 'N','Logout System')";
            string sqlResult = MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);

            sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + STOP_MC + "', 'No Worker', 'Y','N')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= 'No Worker', `tsnon_oper_mitor_RUNYN` = 'Y',`StopCode`='N'";
            sqlResult = MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);

            sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
            sqlResult = MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);


            _cookieHelper.DeleteCookie(GlobalHelper.USER_IDX);
            // _cookieHelper.DeleteCookie(GlobalHelper.Language);
            _cookieHelper.DeleteCookie(GlobalHelper.MenuParent);

            
            // thêm mã cập nhật noworker cho hệ thống

           // Console.WriteLine("Da LogOut khoi he thong "+ ST_TM + ": may " + STOP_MC + " , ID nhân viên " + C_USER);

            return RedirectToAction("Index", "MAIN_Login");
        }
    }
}
