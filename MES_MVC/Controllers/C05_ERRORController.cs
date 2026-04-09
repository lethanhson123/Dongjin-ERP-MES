namespace MES.Controllers
{
    public class C05_ERRORController : Controller
    {
        private readonly IC05_ERRORService _C05_ERRORService;
        public C05_ERRORController(IC05_ERRORService C05_ERRORService)
        {
            _C05_ERRORService = C05_ERRORService;
        }
        public IActionResult Index()
        {
            return View();
        }       
    }
}

