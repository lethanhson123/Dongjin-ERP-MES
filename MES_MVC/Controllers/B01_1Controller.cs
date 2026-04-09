namespace MES.Controllers
{
    public class B01_1Controller : Controller
    {
        private readonly IB01_1Service _B01_1Service;
        public B01_1Controller(IB01_1Service B01_1Service)
        {
            _B01_1Service = B01_1Service;
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
                BaseResult = await _B01_1Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }       
    }
}

