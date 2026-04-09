namespace MES.Controllers
{
    public class D04_PLT_PRNTController : Controller
    {
        private readonly ID04_PLT_PRNTService _D04_PLT_PRNTService;
        public D04_PLT_PRNTController(ID04_PLT_PRNTService D04_PLT_PRNTService)
        {
            _D04_PLT_PRNTService = D04_PLT_PRNTService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> D04_PLT_PRNT_Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_PLT_PRNTService.D04_PLT_PRNT_Load(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_PLT_PRNTService.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

