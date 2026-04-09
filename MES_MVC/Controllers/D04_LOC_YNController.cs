namespace MES.Controllers
{
    public class D04_LOC_YNController : Controller
    {
        private readonly ID04_LOC_YNService _D04_LOC_YNService;
        public D04_LOC_YNController(ID04_LOC_YNService D04_LOC_YNService)
        {
            _D04_LOC_YNService = D04_LOC_YNService;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}

