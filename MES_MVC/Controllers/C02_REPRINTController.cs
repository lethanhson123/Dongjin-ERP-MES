namespace MES.Controllers
{
    public class C02_REPRINTController : Controller
    {
        private readonly IC02_REPRINTService _C02_REPRINTService;
        public C02_REPRINTController(IC02_REPRINTService C02_REPRINTService)
        {
            _C02_REPRINTService = C02_REPRINTService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> C02_REPRINT_Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C02_REPRINTService.C02_REPRINT_Load(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> PrintDocument1_PrintPage()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C02_REPRINTService.PrintDocument1_PrintPage(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

