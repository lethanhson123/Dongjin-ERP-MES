namespace MES.Controllers
{
    public class C11_APPLICATIONController : Controller
    {
        private readonly IC11_APPLICATIONService _C11_APPLICATIONService;
        public C11_APPLICATIONController(IC11_APPLICATIONService C11_APPLICATIONService)
        {
            _C11_APPLICATIONService = C11_APPLICATIONService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Button1_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_APPLICATIONService.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Button2_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_APPLICATIONService.Button2_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

