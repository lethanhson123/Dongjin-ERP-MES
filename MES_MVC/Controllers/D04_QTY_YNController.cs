namespace MES.Controllers
{
    public class D04_QTY_YNController : Controller
    {
        private readonly ID04_QTY_YNService _D04_QTY_YNService;
        public D04_QTY_YNController(ID04_QTY_YNService D04_QTY_YNService)
        {
            _D04_QTY_YNService = D04_QTY_YNService;
        }
        public IActionResult Index()
        {
            return View();
        }       
    }
}

