namespace MES.Controllers
{
    public class WMP_PLAYController : Controller
    {
        private readonly IWMP_PLAYService _WMP_PLAYService;
        public WMP_PLAYController(IWMP_PLAYService WMP_PLAYService)
        {
            _WMP_PLAYService = WMP_PLAYService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

