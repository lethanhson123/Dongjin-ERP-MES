namespace MES.Controllers
{
    public class SETUP_FORMController : Controller
    {
        private readonly ISETUP_FORMService _SETUP_FORMService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public SETUP_FORMController(ISETUP_FORMService SETUP_FORMService, IWebHostEnvironment webHostEnvironment)
        {
            _SETUP_FORMService = SETUP_FORMService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> DB_MC_LIST()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _SETUP_FORMService.DB_MC_LIST(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

