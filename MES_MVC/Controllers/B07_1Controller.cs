namespace MES.Controllers
{
    public class B07_1Controller : Controller
    {
        private readonly IB07_1Service _B07_1Service;
        public B07_1Controller(IB07_1Service B07_1Service)
        {
            _B07_1Service = B07_1Service;
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
                BaseResult = await _B07_1Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

