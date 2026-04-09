namespace MES.Controllers
{
    public class C05_DC_READController : Controller
    {
        private readonly IC05_DC_READService _C05_DC_READService;
        public C05_DC_READController(IC05_DC_READService C05_DC_READService)
        {
            _C05_DC_READService = C05_DC_READService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> TB_BARCODE_KeyDown()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C05_DC_READService.TB_BARCODE_KeyDown(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> BARCODE_LOAD2()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C05_DC_READService.BARCODE_LOAD2(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

