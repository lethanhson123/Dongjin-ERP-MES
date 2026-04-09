namespace MES.Controllers
{
    public class C11_ER_RController : Controller
    {
        private readonly IC11_ER_RService _C11_ER_RService;
        public C11_ER_RController(IC11_ER_RService C11_ER_RService)
        {
            _C11_ER_RService = C11_ER_RService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> C11_ERROR_Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_ER_RService.C11_ERROR_Load(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> C11_ERROR_FormClosed()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_ER_RService.C11_ERROR_FormClosed(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Button1_ClickSub()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_ER_RService.Button1_ClickSub(BaseParameter);
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
                BaseResult = await _C11_ER_RService.Button1_Click(BaseParameter);
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
                BaseResult = await _C11_ER_RService.Button2_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

