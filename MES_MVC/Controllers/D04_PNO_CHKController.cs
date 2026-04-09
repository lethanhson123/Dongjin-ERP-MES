namespace MES.Controllers
{
    public class D04_PNO_CHKController : Controller
    {
        private readonly ID04_PNO_CHKService _D04_PNO_CHKService;
        public D04_PNO_CHKController(ID04_PNO_CHKService D04_PNO_CHKService)
        {
            _D04_PNO_CHKService = D04_PNO_CHKService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> PO_CODE()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_PNO_CHKService.PO_CODE(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> TextBoxA2_KeyDown()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_PNO_CHKService.TextBoxA2_KeyDown(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

