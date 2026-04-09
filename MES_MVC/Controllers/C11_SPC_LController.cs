namespace MES.Controllers
{
    public class C11_SPC_LController : Controller
    {
        private readonly IC11_SPC_LService _C11_SPC_LService;
        public C11_SPC_LController(IC11_SPC_LService C11_SPC_LService)
        {
            _C11_SPC_LService = C11_SPC_LService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> C02_SPC_Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_SPC_LService.C02_SPC_Load(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }       
        [HttpPost]
        public async Task<BaseResult> Button1_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_SPC_LService.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

