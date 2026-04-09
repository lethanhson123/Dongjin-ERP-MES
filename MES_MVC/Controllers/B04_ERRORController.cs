namespace MES.Controllers
{
    public class B04_ERRORController : Controller
    {
        private readonly IB03_1Service _B03_1Service;
        public B04_ERRORController(IB03_1Service B03_1Service)
        {
            _B03_1Service = B03_1Service;
        }
        public IActionResult Index()
        {
            return View();
        }      
    }
}

