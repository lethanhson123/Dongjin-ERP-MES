namespace MES.Controllers
{
    public class B07_2Controller : Controller
    {
        private readonly IB07_2Service _B07_2Service;
        public B07_2Controller(IB07_2Service B07_2Service)
        {
            _B07_2Service = B07_2Service;
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
                BaseResult = await _B07_2Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

