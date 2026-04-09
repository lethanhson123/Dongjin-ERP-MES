namespace MES.Controllers
{
    public class C09_REPRINTController : Controller
    {
        private readonly IC09_REPRINTService _C09_REPRINTService;
        public C09_REPRINTController(IC09_REPRINTService C09_REPRINTService)
        {
            _C09_REPRINTService = C09_REPRINTService;
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
                BaseResult = await _C09_REPRINTService.C02_REPRINT_Load(BaseParameter);
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
                BaseResult = await _C09_REPRINTService.PrintDocument1_PrintPage(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

