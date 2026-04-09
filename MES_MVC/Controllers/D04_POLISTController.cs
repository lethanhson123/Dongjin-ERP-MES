namespace MES.Controllers
{
    public class D04_POLISTController : Controller
    {
        private readonly ID04_POLISTService _D04_POLISTService;
        public D04_POLISTController(ID04_POLISTService D04_POLISTService)
        {
            _D04_POLISTService = D04_POLISTService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> D04_POLIST_Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_POLISTService.D04_POLIST_Load(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

