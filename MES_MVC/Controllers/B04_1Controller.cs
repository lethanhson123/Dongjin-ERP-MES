namespace MES.Controllers
{
    public class B04_1Controller : Controller
    {
        private readonly IB03_1Service _B03_1Service;
        public B04_1Controller(IB03_1Service B03_1Service)
        {
            _B03_1Service = B03_1Service;
        }
        public IActionResult Index()
        {
            return View();
        }      
    }
}

