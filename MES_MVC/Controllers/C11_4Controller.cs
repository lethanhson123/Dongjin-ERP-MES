namespace MES.Controllers
{
    public class C11_4Controller : Controller
    {
        private readonly IC11_4Service _C11_4Service;
        public C11_4Controller(IC11_4Service C11_4Service)
        {
            _C11_4Service = C11_4Service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> ORDER_LOAD()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_4Service.ORDER_LOAD(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> ORDER_COUNT()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C11_4Service.ORDER_COUNT(BaseParameter);
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
                BaseResult = await _C11_4Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

