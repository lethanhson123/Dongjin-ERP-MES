namespace MES.Controllers
{
    public class B04_2Controller : Controller
    {
        private readonly IB04_2Service _B04_2Service;
        public B04_2Controller(IB04_2Service B04_2Service)
        {
            _B04_2Service = B04_2Service;
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
                BaseResult = await _B04_2Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

