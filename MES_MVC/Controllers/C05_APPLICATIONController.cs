namespace MES.Controllers
{
    public class C05_APPLICATIONController : Controller
    {
        private readonly IC05_APPLICATIONService _C05_APPLICATIONService;
        public C05_APPLICATIONController(IC05_APPLICATIONService C05_APPLICATIONService)
        {
            _C05_APPLICATIONService = C05_APPLICATIONService;
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
                BaseResult = await _C05_APPLICATIONService.Button1_Click(BaseParameter);
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
                BaseResult = await _C05_APPLICATIONService.Button2_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

