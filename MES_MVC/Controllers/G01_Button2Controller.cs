namespace MES.Controllers
{
    public class G01_Button2Controller : Controller
    {
        private readonly IG01_1Service _G01_1Service;
        public G01_Button2Controller(IG01_1Service G01_1Service)
        {
            _G01_1Service = G01_1Service;
        }
        public IActionResult Index()
        {
            return View();
        }      
    }
}

