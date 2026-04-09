namespace MES.Controllers
{
    public class B03AddController : Controller
    {
        private readonly IB03_1Service _B03_1Service;
        public B03AddController(IB03_1Service B03_1Service)
        {
            _B03_1Service = B03_1Service;
        }
        public IActionResult Index()
        {
            return View();
        }      
    }
}

