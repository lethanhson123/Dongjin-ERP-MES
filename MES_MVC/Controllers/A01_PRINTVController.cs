namespace MES.Controllers
{
    public class A01_PRINTV : Controller
    {
        private readonly IA01_FILEService _A01_FILEService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01_PRINTV(IA01_FILEService A01_FILEService, IWebHostEnvironment webHostEnvironment)
        {
            _A01_FILEService = A01_FILEService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }        
    }
}

